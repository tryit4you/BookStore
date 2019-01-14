var updateScripts = {
    Init: function () {
        updateScripts.loadDetail();
        updateScripts.registerEvent();
        updateScripts.loadCategory();
    },
    registerEvent: function () {
        $('#txtBookCategory').select2();
        $('#txtBookType').select2();
        $('.Language').select2();
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
                LongDescription: "Yêu cầu nhập mô tả"
            }
        });
        $('#txtBookCategory').on('change', function () {
            var categoryId = $(this).val();
            if (categoryId !== null) {
                updateScripts.loadTypeByCategory(categoryId);
            }
        });
        $('#btnUpdate').off('click').on('click', function () {
            if ($('#frmEbook').valid()) {
                updateScripts.Put();
            }
        });
    }, loadTypeByCategory: function (categoryId) {
        var listOption = $('#txtBookType');
        $.ajax({
            url: '/Books/getbycategory',
            type: 'get',
            data: { categoryId: categoryId },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    listOption.empty();
                    var data = response.data;
                    $(data).each(function (i, item) {
                        listOption.append($('<option/>', { value: item.id, text: item.name }));
                    });
                }
            }
        });
    },
    loadCategory: function () {
        var listOption = $('#txtBookCategory');
        $.ajax({
            url: '/Books/GetBookCategory',
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    listOption.empty();
                    var data = response.data;
                    $(data).each(function (i, item) {
                        listOption.append($('<option/>', { value: item.id, text: item.name }));
                    });
                }
                $('#txtBookType').select2();
            }
        });
    },
    loadDetail: function () {
        var id = $('#txtId').val();
        $.ajax({
            url: '/Books/GetEbookDetail',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status === true) {
                    var item = res.data.book;
                    var links = res.data.downloadLinks;
                    $('#txtId').val(item.id);
                    $('#txtName').val(item.name);
                    $('#ImgThumbnail').attr('src', item.thumbnailUrl);
                    $('#txtPageNumber').val(item.pageNumber);
                    $('#txtPageNumber').val(item.pageNumber);
                    $('#txtCapacity').val(item.cappacity);
                    $('#txtAuthors').val(item.authors);
                    $('#txtPublishBy').val(item.publishBy);
                    $('#txtDateRelease').val(item.dateRelease);
                    $('#txtReferenceText').val(item.textReference);
                    $('#txtReferenceLink').val(item.linkReference);
                    tinymce.get('txtContent').setContent(item.longDescription);
                    /*link download*/
                    $('#txtLinkId').val(links.id);
                    $('#txtLinkPdf').val(links.pdfLink);
                    $('#txtLinkEpub').val(links.epubLink);
                    $('#txtLinkMobi').val(links.mobiLink);
                }
                else {
                    toastr.error(res.message);
                    updateScripts.loadData(true);
                }
            }
        });
    },
    Put: function () {
        var id = $("#txtId").val();
        var Name = $('#txtName').val();
        var metaName = commonController.getSeoTitle(Name);
        var ThumbnailUrl = $('#ImgThumbnail').attr('src');
        var bookTypeId = $('#txtBookType').val();
        var CategoryId = $('#txtBookCategory').val();
        var Content = tinymce.get('txtContent').getContent();
        var Language = $('.Language').val();
        var PageNumber = $('#txtPageNumber').val();
        var Capacity = $('#txtCapacity').val();
        var Author = $('#txtAuthors').val();
        var PublishBy = $('#txtPublishBy').val();
        var DateRelease = $('#txtDateRelease').val();
        var TextReference = $('#txtReferenceText').val();
        var ReferenceLink = $('#txtReferenceLink').val();
        //download link
        var linkId = $('#txtLinkId').val();
        var pdfLink = $("#txtLinkPdf").val();
        var epubLink = $("#txtLinkEpub").val();
        var mobiLink = $("#txtLinkMobi").val();
        var d = {
            Id: linkId,
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
            LongDescription: Content
        };
        if (d.PdfLink !== "" || d.EpubLink !== "" || d.MobiLink !== "") {
            $.ajax({
                url: '/Books/Put',
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
    }
};
updateScripts.Init();