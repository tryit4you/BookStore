"use strict";
var config = {
    pageSize: 20,
    pageIndex: 1
};
var courseController = {
    init: function () {
        courseController.loadData();
        courseController.loadCategory();
        courseController.registerEvent();
    },
    registerEvent: function () {
        $('#EditCourseCategory').select2();
        $('#txtCourseCategory').select2();
        $('#frmAddNew').validate({
            rules: {
                Name: "required",
                Image: "required",
                CourseCategory: "required",
                Description: "required",
                SharingUrl: "required"
            },
            messages: {
                Name: "Yêu cầu nhập tên Khóa Học",
                Image: "Yêu cầu phải có ảnh đại diện",
                CourseCategory: "Yêu cầu chọn một danh mục",
                Description: "Yêu cầu có mô tả",
                SharingUrl: "Yêu cầu linh chia sẻ"
            }
        });
        $('#frmEditData').validate({
            rules: {
                EditName: "required",
                EditImage: "required",
                EditCourseCategory: "required",
                EditDescription: "required",
                EditShareUrl: "required"
            },
            messages: {
                EditName: "Yêu cầu nhập tên",
                EditImage: "Yêu cầu ảnh",
                EditCourseCategory: "Yêu cầu chọn danh mục",
                EditDescription: "Yêu cầu mô tả",
                EditShareUrl: "Yêu cầu đường dẫn chia sẻ"
            }
        });

        $('#SaveEdit').off('click').on('click', function () {
            var id = $('#EditId').val();
            if ($('#frmEditData').valid()) {
                courseController.Put(id);
            }
        });

        $('#btnAdd').off('click').on('click', function () {
            if ($('#frmAddNew').valid()) {
                var name = $('#txtName').val();
                courseController.checkExist(name);
            }
        });
        $('.btn-clear').off('click').on('click', function () {
            $('#txtSearch').val('');
            bookController.loadData();
        });
        $('#txtSearch').keypress(function (event) {
            if (event.keyCode === 13) {
                bookController.loadData(true);
            }
        });

        $('.btn-view').off('click').on('click', function () {
            var id = $(this).data('id');
            courseController.getDetail(id);
        });

        $('.btn-edit').off('click').on('click', function () {
            var id = $(this).data('id');
            $('ul.nav.nav-tabs a:eq(2)').tab('show');
            courseController.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa danh mục này không?", function (result) {
                if (result) {
                    courseController.delete(id);
                }
            });
        });
        $('#btn-Deletemulti').off('click').on('click', function () {
            bootbox.confirm("Bạn có chắc muốn xóa các bản ghi được chọn không?", function (result) {
                if (result)
                    courseController.deleteMul();
            });
        });
        $('#txtSearch').change(function () {
            courseController.loadData(true);
        });
        $('#btn-Search').off('click').on('click', function () {
            courseController.loadData(true);
        });
        $('#btn-refresh').off('click').on('click', function () {
            $('#txtSearch').val('');
            courseController.loadData(true);
        });
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var target = $(this);
            $.ajax({
                url: '/admin/course/ChangeStatus',
                data: { id: id },
                type: 'post',
                dataType: 'json',
                success: function (res) {
                    if (res.status) {
                        target.removeClass('bg-red');
                        target.addClass('bg-green');
                        toastr.remove();
                        toastr.info("Đã kích hoạt");
                        target.html("<i class='fa fa-check'>Active</i>");
                    } else {
                        target.removeClass('bg-green');
                        target.addClass('bg-red');
                        toastr.remove();
                        toastr.info("Đã hủy kích hoạt");
                        target.html("<i class='fa fa-ban'>NotActive</i>");
                    }
                }
            });
        });
        $('#selectAll').change(function () {
            $('.selectedItem').prop('checked', $(this).prop('checked'));
        });
    },
    loadCategory: function () {
        var listOption = $('#txtCourseCategory');
        var editListOption = $('#EditCourseCategory');
        $.ajax({
            url: '/admin/courseCategory/GetAll',
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    listOption.empty();
                    var data = response.data;
                    $(data).each(function (i, item) {
                        listOption.append($('<option/>', { value: item.id, text: item.name }));
                        editListOption.append($('<option/>', { value: item.id, text: item.name }));
                    });
                }
                //$('#txtBookType').select2();
            }
        });
    },

    delete: function (id) {
        $.ajax({
            url: '/admin/course/Delete',
            data: { id: id },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                toastr.success(res.message);
                courseController.loadData(true);
            }
        });
    },
    deleteMul: function () {
        var listSelected = [];

        $("td input:checked").each(function () {
            listSelected.push($(this).data('id'));
        });
        if (listSelected.length === 0) {
            toastr.info("Không có phần tử nào được chọn!");
        } else {
            $.ajax({
                url: '/admin/course/DeleteMul',
                data: {
                    ids: listSelected
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    toastr.success(response.message);
                    courseController.loadData(true);
                }
            });
        }
    },
    checkExist: function (name) {
        var result = false;
        $.ajax({
            url: '/admin/course/CheckExist',
            data: {
                name: name
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.result === true) {
                    courseController.Post();
                } else {
                    $('.validate').html('Tên ' + name + ' đã tồn tại!');
                }
            }
        });
    },
    Post: function () {
        var Name = $('#txtName').val();
        var Image = $('#ImgThumbnail').attr('src');
        var CourseCategory = $('#txtCourseCategory').val();
        var Content = tinymce.get('txtContent').getContent();
        var Authors = $('#txtAuthors').val();
        var Reference = $('#txtReference').val();
        var LinkSharing = $('#txtSharingUrl').val();
        var Status = $('#txtStatus').prop('checked');
        var e = {
            Name: Name,
            MetaName: commonController.getSeoTitle(Name),
            AvatarData: Image,
            CourseId: CourseCategory,
            Authors: Authors,
            Reference: Reference,
            SharedUrl: LinkSharing,
            Description: Content,
            Status: Status
        };
        $.ajax({
            url: '/admin/course/Post',
            data: {
                course: JSON.stringify(e)
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    location.reload();
                    toastr.success(response.message);
                    courseController.loadData(true);
                } else {
                    toastr.error(response.message);
                }
            }
        });
    },
    Put: function () {
        var id = $("#EditId").val();
        var Name = $('#EditName').val();
        var Image = $('#EditThumbnail').attr('src');
        var CourseCategory = $('#EditCourseCategory').val();
        var Content = tinymce.get('EditContent').getContent();
        var Authors = $('#EditAuthors').val();
        var Reference = $('#EditReferenceText').val();
        var LinkSharing = $('#sharedUrl').val();
        var Status = $('#EditStatus').prop('checked');
        var e = {
            Id: id,
            Name: Name,
            MetaName: commonController.getSeoTitle(Name),
            AvatarData: Image,
            CourseId: CourseCategory,
            Authors: Authors,
            Reference: Reference,
            SharedUrl: LinkSharing,
            Description: Content,
            Status: Status
        };
        $.ajax({
            url: '/admin/course/Put',
            data: {
                course: JSON.stringify(e)
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    location.reload();
                    toastr.success(response.message);
                    courseController.loadData(true);
                } else {
                    toastr.error(response.message);
                }
            }
        });
    },

    loadDetail: function (id) {
        $.ajax({
            url: '/admin/course/GetDetail',
            data: {
                id: id
            },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    $('#EditId').val(data.id);
                    $('#EditThumbnail').attr('src', data.avatarData);
                    $('#EditName').val(data.name);
                    $('#EditAuthors').val(data.authors);
                    $('#EditReferenceText').val(data.reference);
                    $('#sharedUrl').val(data.sharedUrl);
                    tinymce.get('EditContent').setContent(data.description);
                    $('#EditStatus').prop('checked', data.status);
                }
            }
        });
    },
    getDetail: function (id) {
        $.ajax({
            url: '/admin/course/GetDetail',
            data: {
                id: id
            },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    $('.btnBrowser').attr('data-id', data.id);
                    $('#modal-title').html(data.name);
                    $('#content').html(data.content);
                    $('#courseViewModel').modal('show');
                }
            }
        });
    }
    ,
    loadData: function (changePageSize) {
        $.ajax({
            url: '/admin/course/GetAll',
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            Name: item.name,
                            Image: item.avatarData,
                            CreatedDate: $.datepicker.formatDate('dd-mm-yy', new Date(item.createdDate)),
                            Authors: item.authors,
                            Status: item.status ? "<a href='#' style='margin-top: 0%' class='btn-active badge bg-green' data-id='" + item.id + "'><i class='fa fa-check'></i>Active<\/a>" : "<a href='#' style='margin-top: 0%' class='btn-active badge bg-red' data-id='" + item.id + "'>< i class='fa fa-ban'></i>NotActive<\/a>"
                        });
                    });
                    $('#tbData').html(html);
                    var totalPage = Math.ceil(response.total / config.pageSize);
                    courseController.paging(response.total, function () {
                        courseController.loadData();
                        $('#currentpage').html(config.pageIndex);
                        $('#totalpage').html(totalPage);
                    }, changePageSize);
                    courseController.registerEvent();
                }
            }
        });
    },

    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / config.pageSize);
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }
        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp ',
            last: 'Cuối',

            visiblePages: 10,
            onPageClick: function (event, page) {
                config.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
};
courseController.init();