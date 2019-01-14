"use strict";
var config = {
    pageSize: 20,
    pageIndex: 1
};
var bookTypeController = {
    init: function () {
        bookTypeController.loadData();
        bookTypeController.loadCategory();
        bookTypeController.registerEvent();
    },
    registerEvent: function () {
        $('#txtCategory').select2();
        $('#EditCategory').select2();
        $('#frmAddNew').validate({
            rules: {
                Name: {
                    required: true,
                    minlength: 4
                },
                Description: "required",
                CategoryId: "required"
            },
            messages: {
                Name: {
                    required: "Yêu cầu nhập tên liên hệ",
                    minlength: "ít nhất có 4 ký tự"
                },
                Description: "Yêu cầu nhập mô tả",
                CategoryId: "Yêu cầu danh mục"
            }
        });
        $('#frmUpdate').validate({
            rules: {
                Name: {
                    required: true,
                    minlength: 4
                },
                Description: "required",
                CategoryId: "required"
            },
            messages: {
                Name: {
                    required: "Yêu cầu nhập tên liên hệ",
                    minlength: "ít nhất có 4 ký tự"
                },
                Description: "Yêu cầu nhập mô tả",
                CategoryId: "Yêu cầu danh mục"
            }
        });

        $('#SaveEdit').off('click').on('click', function () {
            var id = $('#txtId').val();
            if ($('#frmUpdate').valid()) {
                bookTypeController.Put();
            }
        });

        $('#btnAdd').off('click').on('click', function () {
            if ($('#frmAddNew').valid()) {
                var name = $('#txtName').val();
                bookTypeController.checkExist(name);
            }
        });
        $('.btn-showTable').off('click').on('click', function () {
            $('.show-Table').removeClass('hidden');
            $('.show-Table').show(3000);
            $('.show-Colapse').addClass('hidden');
            $('.show-Colapse').hide(2000);
        });
        $('.btn-showCollapse').off('click').on('click', function () {
            $('.show-Colapse').removeClass('hidden');
            $('.show-Colapse').show(2000);
            $('.show-Table').addClass('hidden');
            $('.show-Table').hide(3000);
        });
        $('.btn-edit').off('click').on('click', function () {
            var id = $(this).data('id');
            $('ul.nav.nav-tabs a:eq(2)').tab('show');
            bookTypeController.resetForm();
            bookTypeController.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa danh mục này không?", function (result) {
                if (result) {
                    bookTypeController.delete(id);
                }
            });
        });
        $('#btn-Deletemulti').off('click').on('click', function () {
            bootbox.confirm("Bạn có chắc muốn xóa các bản ghi được chọn không?", function (result) {
                if (result)
                    bookTypeController.deleteMul();
            });
        });
        $('.clear-search').off('click').on('click', function () {
            $('#txtSearch').val('');
            bookTypeController.loadData();
        });
        $('#txtSearch').change(function () {
            bookTypeController.loadData(true);
        });
        $('#btn-Search').off('click').on('click', function () {
            bookTypeController.loadData(true);
        });
        $('#btn-refresh').off('click').on('click', function () {
            $('#txtSearch').val('');
            bookTypeController.loadData(true);
        });
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var target = $(this);
            $.ajax({
                url: '/admin/bookType/ChangeStatus',
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
    resetForm: function () {
        $('.validate').html('');
        $('#txtId').val('');
        $('#txtName').val('');
        $('#txtDescription').val('');
        $('#txtCategory').val('');
        $('#ckStatus').prop('checked', 'true');
    },
    delete: function (id) {
        $.ajax({
            url: '/admin/BookType/Delete',
            data: { id: id },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                toastr.success(res.message);
                bookTypeController.loadData(true);
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
                url: '/admin/bookType/DeleteMul',
                data: {
                    ids: listSelected
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    toastr.success(response.message);
                    bookTypeController.loadData(true);
                }
            });
        }
    },
    checkExist: function (name) {
        var result = false;
        $.ajax({
            url: '/admin/bookType/CheckExist',
            data: {
                name: name
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.result === true) {
                    bookTypeController.Post();
                } else {
                    $('.validate').html('Tên ' + name + ' đã tồn tại!');
                }
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/admin/BookType/GetDetail',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status === true) {
                    var item = res.data;
                    $('#EditId').val(item.id);
                    $('#EditThumbnail').attr('src', item.thumbnailUrl);
                    $('#EditName').val(item.name);
                    tinymce.get('EditDescription').setContent(item.description);
                    $('#EditStatus').prop('checked', item.status);
                }
                else {
                    toastr.error("Lỗi!");
                    bookTypeController.loadData(true);
                }
            }
        });
    },
    Post: function () {
        var Name = $('#txtName').val();
        var ThumbnailUrl = $('#ImgThumbnail').attr('src');
        var MetaName = commonController.getSeoTitle(Name);
        var Description = tinymce.get('txtContent').getContent();
        var CategoryId = $("#txtCategory").val();
        var Status = $('#txtStatus').prop('checked');
        var e = {
            Name: Name,
            ThumbnailUrl: ThumbnailUrl,
            MetaName: MetaName,
            Description: Description,
            CategoryId: CategoryId,
            Status: Status
        };
        $.ajax({
            url: '/admin/BookType/Post',
            data: {
                bookType: JSON.stringify(e)
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    toastr.success(response.message);
                    bookTypeController.loadData(true);
                    location.reload();
                } else {
                    toastr.error(response.message);
                }
            }
        });
    },
    Put: function () {
        var id = $("#EditId").val();
        var Name = $('#EditName').val();
        var MetaName = commonController.getSeoTitle(Name);
        var ThumbnailUrl = $('#EditThumbnail').attr('src');
        var Description = tinymce.get('EditDescription').getContent();
        var CategoryId = $("#EditCategory").val();
        var Status = $('#EditStatus').prop('checked');
        var e = {
            ID: id,
            ThumbnailUrl: ThumbnailUrl,
            Name: Name,
            MetaName: MetaName,
            Description: Description,
            CategoryId: CategoryId,
            Status: Status
        };
        $.ajax({
            url: '/admin/BookType/Put',
            data: {
                bookType: JSON.stringify(e)
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    toastr.success(response.message);
                    bookTypeController.loadData(true);
                    location.reload();
                } else {
                    toastr.error(response.message);
                }
            }
        });
    },
    loadCategory: function () {
        var listOption = $('#txtCategory,#EditCategory');
        $.ajax({
            url: '/admin/Category/GetAll',
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    $(data).each(function (i, item) {
                        listOption.append($('<option/>', { value: item.id, text: item.name }));
                    });
                }
            }
        });
    },
    loadData: function (changePageSize) {
        var search = $('#txtSearch').val();
        $.ajax({
            url: '/admin/BookType/GetAll',
            type: 'POST',
            data: {
                filter: search,
                page: config.pageIndex,
                pageSize: config.pageSize
            },
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
                            Thumbnail: item.thumbnail,
                            Description: item.description,
                            CreatedDate: $.datepicker.formatDate('dd-mm-yy', new Date(item.createdDate)),
                            Category: item.categoryName,
                            Status: item.status ? " <a href='#' class='btn-active badge bg-green ' data-id='" + item.id + "'><i class='fa fa-check'>Active</i><\/a>" : "<a href='#' class='btn-active badge bg-red' data-id='" + item.id + "'><i class='fa fa-ban'>NotActive</i><\/a>"
                        });
                    });
                    $('#tbData').html(html);
                    var totalPage = Math.ceil(response.total / config.pageSize);
                    bookTypeController.paging(response.total, function () {
                        bookTypeController.loadData();
                        $('#currentpage').html(config.pageIndex);
                        $('#totalpage').html(totalPage);
                    }, changePageSize);
                    bookTypeController.registerEvent();
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
bookTypeController.init();