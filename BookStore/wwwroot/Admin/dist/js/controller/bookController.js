var config = {
    pageSize: 12,
    pageIndex: 1,
    viewport: {
        width: 189,
        height: 245
    }
};
var bookController = {
    init: function () {
        bookController.loadData();
        bookController.loadCategory();
        bookController.registerEvent();
    },
    registerEvent: function () {
        $('.datepicker').datepicker({
            format: 'mm/dd/yyyy'
        });
        $('.Language').select2();
        $('#txtBookType').select2();
        $('#EditCategory').select2();
        $('#txtBookCategory').select2();
        $('#EditBookType').select2();
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
                LongDescription: {
                    required: true,
                    minlength: 20
                }
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
                LongDescription: {
                    required: "Yêu cầu nhập mô tả",
                    minlength: "Yêu cầu ít nhất 20 ký tự"
                }
            }
        });
        $('#frmEditData').validate({
            rules: {
                EditName: {
                    required: true,
                    minlength: 8
                },
                EditLink: "required",
                EditImage: "required",
                PageNumber: {
                    required: true,
                    min: 1,
                    number: true
                },
                Capacity: {
                    required: true,
                    min: 0.1,
                    number: true
                },
                Authors: "required",
                DateRelease: {
                    required: true,
                    min: 1980,
                    max: 2090,
                    number: true
                },
                EditBookTypeId: "required",
                EditCategory: "required",
                EditLongDescription: {
                    required: true,
                    minlength: 20
                }
            },
            messages: {
                EditName: {
                    required: "Tên là bắt buộc",
                    minlength: "Tên yêu cầu ít nhất 8 ký tự"
                },
                EditLink: "Nhập link download",
                EditImage: "Yêu cầu có ảnh đại diện",
                PageNumber: {
                    required: "Nhập số trang",
                    min: "Số trang phải ít nhất là 1!",
                    number: "Số trang không hợp lệ!"
                },
                Capacity: {
                    required: "Nhập dung lượng",
                    min: "Tối thiểu phải ít nhất là 0.1!",
                    number: "Dung lượng không hợp lệ!"
                },
                Authors: "Nhập tên tác giả",
                DateRelease: {
                    required: "Yêu cầu nhập năm",
                    min: "Năm lớn hơn 1980",
                    max: "Năm nhỏ hơn 2090",
                    number: "Yêu cầu phải là số"
                },
                EditBookTypeId: "Yêu cầu nhập thể loại",
                EditCategory: "Danh mục",
                EditLongDescription: {
                    required: "Yêu cầu nhập mô tả",
                    minlength: "Yêu cầu ít nhất 20 ký tự"
                }
            }
        });
        $('#txtBookCategory,#EditCategory').on('change', function () {
            var categoryId = $(this).val();
            if (categoryId !== null) {
                bookController.loadTypeByCategory(categoryId);
            }
        });
        $('#btnAdd').off('click').on('click', function () {
            if ($('#frmEbook').valid()) {
                bookController.Post();
            }
        });
        $('#txtName').on('change', function () {
            var name = $('#txtName').val();
            bookController.checkExist(name);
        });
        $('#SaveEdit').off('click').on('click', function () {
            if ($('#frmEditData').valid()) {
                bookController.Put();
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
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var target = $(this);
            $.ajax({
                url: '/admin/books/ChangeStatus',
                data: { id: id },
                type: 'post',
                dataType: 'json',
                success: function (res) {
                    if (res.message === null) {
                        toastr.error(res.message);
                    } else {
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
                }
            });
        });
        $('.btn-edit').off('click').on('click', function () {
            var id = $(this).data('id');
            $('ul.nav.nav-tabs a:eq(2)').tab('show');
            bookController.resetForm();
            bookController.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa danh mục này không?", function (result) {
                if (result) {
                    bookController.delete(id);
                }
            });
        });
        $('#btn-Deletemulti').off('click').on('click', function () {
            bootbox.confirm("Bạn có chắc muốn xóa các bản ghi được chọn không?", function (result) {
                if (result)
                    bookController.deleteMul();
            });
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

        $('#btn-refresh').off('click').on('click', function () {
            $('#txtSearch').val('');
            bookController.loadData(true);
        });

        $('#selectAll').change(function () {
            $('.selectedItem').prop('checked', $(this).prop('checked'));
            $('#btn-Deletemulti').removeAttr('disabled');
        });
        $('.selectedItem').on('click', function () {
            $(this).attr('checked', this.checked ? '' : 'checked');
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
            url: '/admin/Books/Delete',
            data: { id: id },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                toastr.success("Xóa thành công");
                bookController.loadData(true);
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
                url: '/admin/books/DeleteMul',
                data: {
                    ids: listSelected
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        toastr.success(response.message);
                        bookController.loadData(true);
                    } else {
                        toastr.error(response.message);
                        bookController.loadData(true);
                    }
                }
            });
        }
    },
    resetForm: function () {
        $('#txtId').val('');
        $('#txtName').val('');
        $('#txtMetaName').val('');
        $('#txtBookType').val('');
        $('#shortDescription').val('');
        tinymce.get("txtContent").setContent('');
    },
    checkExist: function (name) {
        var result = false;
        $.ajax({
            url: '/admin/books/CheckExist',
            data: {
                name: name
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.result === true) {
                    bookController.Post();
                } else {
                    $('.validate').html('Tên ' + name + ' đã tồn tại!');
                }
            }
        });
    },
    loadTypeByCategory: function (categoryId) {
        var listOption = $('#txtBookType');
        var listOption1 = $('#EditBookType');
        $.ajax({
            url: '/admin/booktype/getbycategory',
            type: 'get',
            data: { categoryId: categoryId },
            dataType: 'json',
            success: function (response) {
                listOption.empty();
                listOption1.empty();
                var data = response;
                $(data).each(function (i, item) {
                    listOption.append($('<option/>', { value: item.id, text: item.name }));
                    listOption1.append($('<option/>', { value: item.id, text: item.name }));
                });
            }
        });
    },
    loadCategory: function () {
        var listOption = $('#txtBookCategory');
        var listOption1 = $('#EditCategory');
        $.ajax({
            url: '/admin/Category/GetAll',
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    listOption.empty();
                    listOption1.empty();
                    var data = response.data;
                    $(data).each(function (i, item) {
                        listOption.append($('<option/>', { value: item.id, text: item.name }));
                        listOption1.append($('<option/>', { value: item.id, text: item.name }));
                    });
                }
                //$('#txtBookType').select2();
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/admin/Books/GetDetail',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status === true) {
                    var item = res.data.book;
                    var links = res.data.downloadLinks;
                    $('#EditId').val(item.id);
                    $('#EditName').val(item.name);
                    $('#EditLink').val(item.linkDownload);
                    $('#EditThumbnail').attr('src', item.thumbnailUrl);
                    $('#urlImg').val(item.thumbnailUrl);
                    $('#EditMetaName').val(item.metaName);
                    $('#EditPageNumber').val(item.pageNumber);
                    $('#EditCapacity').val(item.cappacity);
                    $('#EditAuthors').val(item.authors);
                    $('#EditPublishBy').val(item.publishBy);
                    $('#EditDateRelease').val(item.dateRelease);
                    $('#EditReferenceText').val(item.textReference);
                    $('#EditReferenceLink').val(item.linkReference);
                    tinymce.get('EditContent').setContent(item.longDescription);
                    $('#EditStatus').prop('checked', item.status);
                    /*link download*/
                    $('#EditLinkId').val(links.id);
                    $('#EditLinkPdf').val(links.pdfLink);
                    $('#EditLinkEpub').val(links.mobiLink);
                    $('#EditLinkMobi').val(links.epubLink);
                }
                else {
                    toastr.error(res.message);
                    bookController.loadData(true);
                }
            }
        });
    },
    Post: function () {
        var Name = $('#txtName').val();
        var MetaName = commonController.getSeoTitle(Name);
        var BookTypeId = $('#txtBookType').val();
        var CategoryId = $('#txtBookCategory').val();
        var ThumbnailUrl = $('#ImgThumbnail').attr('src');
        var LongDescription = tinymce.get('txtContent').getContent();
        var Language = $('.Language').val();
        var PageNumber = $('#txtPageNumber').val();
        var Capacity = $('#txtCapacity').val();
        var Author = $('#txtAuthors').val();
        var PublishBy = $('#txtPublishBy').val();
        var DateRelease = $('#txtDateRelease').val();
        var TextReference = $('#txtReferenceText').val();
        var ReferenceLink = $('#txtReferenceLink').val();
        var status = $('#txtStatus').prop('checked');
        //download link
        var pdfLink = $("#txtLinkPdf").val();
        var epubLink = $("#txtLinkEpub").val();
        var mobiLink = $("#txtLinkMobi").val();
        var d = {
            PdfLink: pdfLink,
            EpubLink: epubLink,
            MobiLink: mobiLink
        };
        var e = {
            Name: Name,
            Language: Language,
            PageNumber: PageNumber,
            Authors: Author,
            TextReference: TextReference,
            LinkReference: ReferenceLink,
            DateRelease: DateRelease,
            PublishBy: PublishBy,
            Cappacity: Capacity,
            MetaName: MetaName,
            BookTypeId: BookTypeId,
            CategoryId: CategoryId,
            ThumbnailUrl: ThumbnailUrl,
            LongDescription: LongDescription,
            Status: status
        };
        if (d.PdfLink !== "" || d.EpubLink !== "" || d.MobiLink !== "") {
            $.ajax({
                url: '/admin/Books/Post',
                data: {
                    book: JSON.stringify(e),
                    downloadLink: JSON.stringify(d)
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        location.reload();
                        toastr.success(response.message);
                    } else {
                        toastr.error(response.message);
                    }
                }
            });
        } else {
            alert("Vui lòng cung cấp ít nhất một đường dẫn download!");
        }
    },
    Put: function () {
        var id = $("#EditId").val();
        var Name = $('#EditName').val();
        var metaName = commonController.getSeoTitle(Name);
        var ThumbnailUrl = $('#EditThumbnail').attr('src');
        var bookTypeId = $('#EditBookType').val();
        var CategoryId = $('#EditCategory').val();
        var Content = tinymce.get('EditContent').getContent();
        var Language = $('#EditLanguage').val();
        var PageNumber = $('#EditPageNumber').val();
        var Capacity = $('#EditCapacity').val();
        var Author = $('#EditAuthors').val();
        var PublishBy = $('#EditPublishBy').val();
        var DateRelease = $('#EditDateRelease').val();
        var TextReference = $('#EditReferenceText').val();
        var ReferenceLink = $('#EditReferenceLink').val();
        var status = $('#EditStatus').prop('checked');
        //download link
        var LinkId = $('#EditLinkId').val();
        var pdfLink = $("#EditLinkPdf").val();
        var epubLink = $("#EditLinkEpub").val();
        var mobiLink = $("#EditLinkMobi").val();
        var d = {
            Id: LinkId,
            PdfLink: pdfLink,
            EpubLink: epubLink,
            MobiLink: mobiLink
        };
        var e = {
            ID: id,
            Name: Name,
            Language: Language,
            PageNumber: PageNumber,
            Authors: Author,
            TextReference: TextReference,
            LinkReference: ReferenceLink,
            DateRelease: DateRelease,
            PublishBy: PublishBy,
            Cappacity: Capacity,
            MetaName: metaName,
            BookTypeId: bookTypeId,
            CategoryId: CategoryId,
            ThumbnailUrl: ThumbnailUrl,
            LongDescription: Content,
            Status: status
        };
        if (d.PdfLink !== "" || d.EpubLink !== "" || d.MobiLink !== "") {
            $.ajax({
                url: '/admin/Books/Put',
                data: {
                    book: JSON.stringify(e),
                    downloadLink: JSON.stringify(d)
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        location.reload();
                        toastr.success(response.message);
                    } else {
                        toastr.error(response.message);
                    }
                }
            });
        } else {
            alert("Vui lòng cung cấp ít nhất một đường dẫn download!");
        }
    },
    loadData: function (changePageSize) {
        var search = $('#txtSearch').val();
        $.ajax({
            url: '/admin/Books/GetAll',
            type: 'GET',
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
                            Image: item.thumbnailUrl,
                            CreatedDate: $.datepicker.formatDate('dd-mm-yy', new Date(item.createdDate)),
                            CreatedBy: item.userName,
                            CountDown: item.countDownload,
                            Status: item.status ? " <a href='#' class='btn-active badge bg-green' data-id='" + item.id + "'><i class='fa fa-check'>Active</i><\/a>" : "<a href='#' class='btn-active badge bg-red' data-id='" + item.id + "'><i class='fa fa-ban'>NotActive</i><\/a>"
                        });
                    });
                    $('#tbData').html(html);
                    var totalPage = Math.ceil(response.total / config.pageSize);
                    bookController.paging(response.total, function () {
                        bookController.loadData();
                        $('#currentpage').html(config.pageIndex);
                        $('#totalpage').html(totalPage);
                    }, changePageSize);
                    bookController.registerEvent();
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
bookController.init();