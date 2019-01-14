"use strict";
var config = {
    pageSize: 20,
    pageIndex: 1
};
var courseCategoryController = {
    init: function () {
        courseCategoryController.loadData();
        courseCategoryController.registerEvent();
    },
    registerEvent: function () {
        $('#frmAddNew').validate({
            rules: {
                Name: {
                    required: true,
                    minlength: 4
                }
            },
            messages: {
                Name: {
                    required: "Yêu cầu nhập tên danh mục khóa học",
                    minlength: "ít nhất có 4 ký tự"
                }
            }
        });
        $('#frmUpdate').validate({
            rules: {
                Name: {
                    required: true,
                    minlength: 4
                }
            },
            messages: {
                Name: {
                    required: "Yêu cầu nhập tên liên hệ",
                    minlength: "ít nhất có 4 ký tự"
                }
            }
        });

        $('#SaveEdit').off('click').on('click', function () {
            var id = $('#txtId').val();
            if ($('#frmUpdate').valid()) {
                courseCategoryController.Put();
            }
        });

        $('#btnAdd').off('click').on('click', function () {
            if ($('#frmAddNew').valid()) {
                var name = $('#txtName').val();
                courseCategoryController.checkExist(name);
            }
        });

        $('.btn-edit').off('click').on('click', function () {
            var id = $(this).data('id');
            $('ul.nav.nav-tabs a:eq(2)').tab('show');
            courseCategoryController.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa danh mục này không?", function (result) {
                if (result) {
                    courseCategoryController.delete(id);
                }
            });
        });
        $('#btn-Deletemulti').off('click').on('click', function () {
            bootbox.confirm("Bạn có chắc muốn xóa các bản ghi được chọn không?", function (result) {
                if (result)
                    courseCategoryController.deleteMul();
            });
        });
        $('.clear-search').off('click').on('click', function () {
            $('.txtSearch').val('');
            courseCategoryController.loadData();
        });
        $('.txtSearch').change(function () {
            courseCategoryController.loadData(true);
        });
        $('.btn-Search').off('click').on('click', function () {
            courseCategoryController.loadData(true);
        });
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var target = $(this);
            $.ajax({
                url: '/admin/courseCategory/ChangeStatus',
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
        $('#txtcourseCategory').val('');
        $('#ckStatus').prop('checked', 'true');
    },
    delete: function (id) {
        $.ajax({
            url: '/admin/courseCategory/Delete',
            data: { id: id },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                toastr.success(res.message);
                courseCategoryController.loadData(true);
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
                url: '/admin/courseCategory/DeleteMul',
                data: {
                    ids: listSelected
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    toastr.success(response.message);
                    courseCategoryController.loadData(true);
                }
            });
        }
    },
    checkExist: function (name) {
        var result = false;
        $.ajax({
            url: '/admin/courseCategory/CheckExist',
            data: {
                name: name
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.result === true) {
                    courseCategoryController.Post();
                } else {
                    $('.validate').html('Tên ' + name + ' đã tồn tại!');
                }
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/admin/courseCategory/GetDetail',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status === true) {
                    var item = res.data;
                    $('#EditId').val(item.id);
                    $('#EditName').val(item.name);
                    $('#EditStatus').prop('checked', item.status);
                }
                else {
                    toastr.error("Lỗi!");
                    courseCategoryController.loadData(true);
                }
            }
        });
    },
    Post: function () {
        var Name = $('#txtName').val();
        var MetaName = commonController.getSeoTitle(Name);
        var Status = $('#txtStatus').prop('checked');
        var e = {
            Name: Name,
            MetaName: MetaName,
            Status: Status
        };
        $.ajax({
            url: '/admin/courseCategory/Post',
            data: {
                category: JSON.stringify(e)
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    toastr.success(response.message);
                    courseCategoryController.loadData(true);
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
        var Status = $('#EditStatus').prop('checked');
        var e = {
            ID: id,
            Name: Name,
            MetaName: MetaName,
            Status: Status
        };
        $.ajax({
            url: '/admin/courseCategory/Put',
            data: {
                category: JSON.stringify(e)
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    toastr.success(response.message);
                    courseCategoryController.loadData(true);
                    location.reload();
                } else {
                    toastr.error(response.message);
                }
            }
        });
    },
    loadData: function (changePageSize) {
        var search = $('.txtSearch').val();
        $.ajax({
            url: '/admin/courseCategory/GetAll',
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
                            CreatedDate: $.datepicker.formatDate('dd-mm-yy', new Date(item.createDate)),
                            Status: item.status ? " <a href='#' class='btn-active badge bg-green' data-id='" + item.id + "'><i class='fa fa-check'>Active</i><\/a>" : "<a href='#' class='btn-active badge bg-red' data-id='" + item.id + "'><i class='fa fa-ban'>NotActive</i><\/a>"
                        });
                    });
                    $('#tbData').html(html);
                    var totalPage = Math.ceil(response.total / config.pageSize);
                    courseCategoryController.paging(response.total, function () {
                        courseCategoryController.loadData();
                        $('#currentpage').html(config.pageIndex);
                        $('#totalpage').html(totalPage);
                    }, changePageSize);
                    courseCategoryController.registerEvent();
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
courseCategoryController.init();