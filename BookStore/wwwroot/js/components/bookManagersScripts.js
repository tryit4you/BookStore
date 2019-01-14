var config = {
    pageSize: 16,
    pageIndex: 1,
    viewport: {
        width: 189,
        height: 245
    }
};
var bookManagerScripts = {
    init: function () {
        bookManagerScripts.loadData();
        bookManagerScripts.registerEvent();
    },
    registerEvent: function () {
        $('.Language').select2();
        $('#txtBookCategory').select2();
        jQuery.validator.setDefaults({
            debug: true,
            success: "valid"
        });
        $('#frmEbook').validate({
            rules: {
                Name: {
                    required: true,
                    minlength: 8
                },
                Image: "required",
                PageNumber: {
                    required: true,
                    min: 1,
                    number: true
                },
                Capacity: {
                    required: true,
                    min: 0,
                    number: true
                },
                Authors: "required",
                DateRelease: {
                    required: true,
                    min: 1980,
                    max: 2090,
                    number: true
                },
                EbookType: "required",
                EbookCategory: "required",
                shortDescription: "required",
                LongDescription: "required"
            },
            messages: {
                Name: {
                    required: "Tên là bắt buộc",
                    minlength: "Tên yêu cầu ít nhất 8 ký tự"
                },
                Image: "Yêu cầu có ảnh đại diện",
                PageNumber: {
                    required: "Nhập số trang",
                    min: "Số trang phải ít nhất là 1!",
                    number: "Số trang không hợp lệ!"
                },
                Capacity: {
                    required: "Nhập dung lượng",
                    min: "Số trang phải ít nhất là 0!",
                    number: "Dung lượng không hợp lệ!"
                },
                Authors: "Nhập tên tác giả",
                DateRelease: {
                    required: "Yêu cầu nhập năm",
                    min: "Năm lớn hơn 1980",
                    max: "Năm nhỏ hơn 2090",
                    number: "Yêu cầu phải là số"
                },
                EbookType: "Yêu cầu nhập thể loại",
                EbookCategory: "Danh mục",
                shortDescription: "Yêu cầu nhập mô tả",
                LongDescription: "Yêu cầu nhập mô tả"
            }
        });

        $('#SaveEdit').off('click').on('click', function () {
            if ($('#frmEditData').valid()) {
                bookManagerScripts.Put();
            }
        });

        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var target = $(this);
            $.ajax({
                url: '/books/ChangeStatus',
                data: { id: id },
                type: 'post',
                dataType: 'json',
                success: function (res) {
                    if (res.message === null) {
                        toastr.error(res.message);
                    } else {
                        if (res.status) {
                            target.removeClass('label-danger');
                            target.addClass('label-success');
                            toastr.remove();
                            toastr.info("Đã kích hoạt");
                            target.html("<i class='fa fa-check'></i>");
                        } else {
                            target.removeClass('label-success');
                            target.addClass('label-danger');
                            toastr.remove();
                            toastr.info("Đã hủy kích hoạt");
                            target.html("<i class='fa fa-ban'></i>");
                        }
                    }
                }
            });
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa ebook này?", function (result) {
                if (result) {
                    bookManagerScripts.delete(id);
                }
            });
        });
        $('#btn-Deletemulti').off('click').on('click', function () {
            bootbox.confirm("Bạn có chắc muốn xóa các bản ghi được chọn không?", function (result) {
                if (result)
                    bookManagerScripts.deleteMul();
            });
        });
        $('#txtSearch').keypress(function (event) {
            if (event.keyCode === 13) {
                bookManagerScripts.loadData(true);
            }
        });

        $('#btn-refresh').off('click').on('click', function () {
            $('#txtSearch').val('');
            bookManagerScripts.loadData(true);
        });

        $('#selectAll').change(function () {
            $('.selectedItem').prop('checked', $(this).prop('checked'));
            if ($('.selectedItem').prop('checked')) {
                $('#btn-Deletemulti').removeClass('hidden');
            } else {
                $('#btn-Deletemulti').addClass('hidden');
            }
        });
        $('.selectedItem').on('click', function () {
            $(this).attr('checked', this.checked ? '' : 'checked');
        });
        $('.btn-search').on('click', function () {
            bookManagerScripts.loadData();
        });
        $('.selectedItem').on('change', function () {
            var selectedItem = $('.selectedItem').attr('checked').length;
            if (selectedItem > 1) {
                $('#btn-Deletemulti').removeAttr('disabled');
            } else {
                $('#btn-Deletemulti').add('disabled');
            }
        });
    },
    delete: function (id) {
        $.ajax({
            url: '/Books/Delete',
            data: { id: id },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                toastr.success("Xóa thành công");
                bookManagerScripts.loadData(true);
            }
        });
    },
    deleteMul: function () {
        var listSelected = [];

        $("td input:checked").each(function () {
            listSelected.push($(this).data('id'));
        });
        if (listSelected.length === 0) {
            toastr.warning("Không có phần tử nào được chọn!");
        } else {
            $.ajax({
                url: '/books/DeleteMul',
                data: {
                    ids: listSelected
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        toastr.success(response.message);
                        bookManagerScripts.loadData(true);
                    } else {
                        toastr.error(response.message);
                        bookManagerScripts.loadData(true);
                    }
                }
            });
        }
    },
    loadData: function (changePageSize) {
        var search = $('#txtQuery').val();
        var userId = $('#userId').val();
        $.ajax({
            url: '/Books/ManagersAsync',
            type: 'GET',
            data: {
                query: search,
                userId: userId,
                page: config.pageIndex,
                pageSize: config.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    $('#txtQuery').val('');

                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            Name: item.name,
                            Image: item.thumbnailUrl,
                            CreatedDate: new Date(item.createdDate),
                            CountDown: item.countDownload,
                            Status: item.status === false ? "<span class='badge  badge-pill badge-warning'>chờ duyệt...</span>" : "<span class='badge badge-pill badge-primary'>đã duyệt</span>"
                        });
                    });
                    $('#tbData').html(html);
                    var totalPage = Math.ceil(response.total / config.pageSize);
                    bookManagerScripts.paging(response.total, response.page, function () {
                        bookManagerScripts.loadData();
                        $('#currentpage').html(config.pageIndex);
                        $('#totalpage').html(totalPage);
                    }, changePageSize);
                    bookManagerScripts.registerEvent();
                }
            }
        });
    },
    paging: function (totalRow, page, callback) {
        $('#pagination').pagination({
            items: totalRow,
            itemsOnPage: config.pageSize,
            currentPage: page,
            hrefTextPrefix: '#trang-',
            prevText: "&lt",
            nextText: "&gt;",
            cssStyle: 'light-theme',
            onPageClick: function (page, event) {
                config.pageIndex = page;
                currentPage = page;
                setTimeout(callback, 0);
            }
        });
    }
};
bookManagerScripts.init();