var config = {
    pageSize: 12,
    pageIndex: 1
};
var userAccountScripts = {
    Init: function () {
        userAccountScripts.registerEvent();
    },
    registerEvent: function () {
        $('#txtBookCategory').select2();
        $('#txtBookType').select2();
        $('.Language').select2();
        $('#emailRegister').keypress(function (event) {
            if ($('registerEmailForm').valid()) {
                if (event.keyCode === 13) {
                    var email = $('#emailRegister').val();
                    userAccountScripts.UploadEmailRegister(email);
                }
            }
        });
        $('#register-form').validate({
            rules: {
                register_username: {
                    required: true,
                    minlength: 8
                },
                register_email: {
                    required: true,
                    email: true
                },
                register_password: {
                    required: true,
                    minlength: 8
                }
            }, messages: {
                register_username: {
                    required: "Tên tài khoản không được trống",
                    minlength: "Tên tài khoản phải ít nhất 8 ký tự"
                },
                register_email: {
                    required: "Yêu cầu nhập email",
                    email: "Địa chỉ email không hợp lệ"
                },
                register_password: {
                    required: "Yêu cầu nhập mật khẩu",
                    minlength: "Mật khẩu ít nhất 8 ký tự"
                }
            }
        });
        $('#registerEmailForm').validate({
            rules: {
                EMAIL: {
                    required: true,
                    email: true
                }
            }, messages: {
                EMAIL: {
                    required: "Yêu cầu nhập email",
                    email: "Địa chỉ email không hợp lệ"
                }
            }
        });
        $('#frmFeedback').validate({
            rules: {
                name: {
                    required: true,
                    minlength: 8
                },
                email: {
                    required: true,
                    email: true
                },
                subject: {
                    required: true
                },
                message: {
                    required: true
                }
            },
            messages: {
                name: {
                    required: "Yêu cầu nhập tên",
                    minlength: "Tên phải có ít nhất 8 ký tự"
                },
                email: {
                    required: "Yêu cầu nhập email",
                    email: "Địa chỉ email không hợp lệ"
                },
                subject: {
                    required: "Yêu cầu nhập chủ đề"
                },
                message: {
                    required: "Yêu cầu nội dung"
                }
            }
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
                EbookCategory: "required",
                Description: "required",
                LinkPdf: {
                    url: true
                }, LinkEpub: {
                    url: true
                }, LinkMobi: {
                    url: true
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
                EbookCategory: "Chọn một danh mục",
                Description: "Yêu cầu nhập mô tả",
                LinkPdf: {
                    url: "Yêu cầu nhập đường dẫn hợp lệ!"
                }, LinkEpub: {
                    url: "Yêu cầu nhập đường dẫn hợp lệ!"
                }, LinkMobi: {
                    url: "Yêu cầu nhập đường dẫn hợp lệ!"
                }
            }
        });
        $('#frmEdit').validate({
            rules: {
                Name: {
                    required: true,
                    minlength: 8
                }
            },
            messages: {
                Name: {
                    required: "Vui lòng điền tên",
                    minlength: "Tên phải chứa ít nhất 8 ký tự"
                }
            }
        });
        $('#lost-form').validate({
            rules: {
                lost_email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                lost_email: {
                    required: "Yêu cầu nhập email",
                    email: "Địa chỉ email không hợp lệ"
                }
            }
        });
        $('#btn-login').on('click', function (e) {
            e.preventDefault();
            var userName = $('#login_username').val();
            var passWord = $('#login_password').val();
            var rememberMe = $('#rememberMe').prop('checked');
            userAccountScripts.login(userName, passWord, rememberMe);
        });

        $('#btn-register').off('click').on('click', function (e) {
            e.preventDefault();
            if ($('#register-form').valid()) {
                var userName = $('#register_username').val();
                var email = $('#register_email').val();
                var password = $('#register_password').val();
                var param = {
                    UserName: userName,
                    Email: email,
                    Password: password
                };
                userAccountScripts.register(param);
            }
        });
        $('#forgot-password-btn').off('click').on('click', function (e) {
            var email = $('#lost_email').val();
            if ($('#lost-form').valid()) {
                userAccountScripts.resetPassword(email);
            }
        });

        $('#txtName').on('change', function () {
            var name = $('#txtName').val();
            userAccountScripts.checkExist(name);
        });
        $('.btnUpload').off('click').on('click', function () {
            if ($('#frmEbook').valid()) {
                userAccountScripts.Post();
            }
        });
        $('.btnEdit').off('click').on('click', function () {
            $('#editModal').modal('show');
        });
        $('#btn-Feedback').on('click', function () {
            if ($('#frmFeedback').valid()) {
                userAccountScripts.sendFeedback();
            }
        });
        $('.btnSave').off('click').on('click', function () {
            if ($('#frmEdit').valid()) {
                userAccountScripts.SaveChange();
            }
        });
        $('#txtBookCategory').on('change', function () {
            var categoryId = $(this).val();
            if (categoryId !== null) {
                userAccountScripts.loadTypeByCategory(categoryId);
            }
        });

        $(function () {
            var $formLogin = $('#login-form');
            var $formLost = $('#lost-form');
            var $formRegister = $('#register-form');
            var $divForms = $('#div-forms');
            var $modalAnimateTime = 300;
            var $msgAnimateTime = 150;
            var $msgShowTime = 2000;

            $('#login_register_btn').click(function () { modalAnimate($formLogin, $formRegister); });
            $('#register_login_btn').click(function () { modalAnimate($formRegister, $formLogin); });
            $('#login_lost_btn').click(function () { modalAnimate($formLogin, $formLost); });
            $('#lost_login_btn').click(function () { modalAnimate($formLost, $formLogin); });
            $('#lost_register_btn').click(function () { modalAnimate($formLost, $formRegister); });
            $('#register_lost_btn').click(function () { modalAnimate($formRegister, $formLost); });

            function modalAnimate($oldForm, $newForm) {
                var $oldH = $oldForm.height() + 20;
                var $newH = $newForm.height() + 60;
                $divForms.css("height", $oldH);
                $oldForm.fadeToggle($modalAnimateTime, function () {
                    $divForms.animate({ height: $newH }, $modalAnimateTime, function () {
                        $newForm.fadeToggle($modalAnimateTime);
                    });
                });
            }

            function msgFade($msgId, $msgText) {
                $msgId.fadeOut($msgAnimateTime, function () {
                    $(this).text($msgText).fadeIn($msgAnimateTime);
                });
            }

            function msgChange($divTag, $iconTag, $textTag, $divClass, $iconClass, $msgText) {
                var $msgOld = $divTag.text();
                msgFade($textTag, $msgText);
                $divTag.addClass($divClass);
                $iconTag.removeClass("glyphicon-chevron-right");
                $iconTag.addClass($iconClass + " " + $divClass);
                setTimeout(function () {
                    msgFade($textTag, $msgOld);
                    $divTag.removeClass($divClass);
                    $iconTag.addClass("glyphicon-chevron-right");
                    $iconTag.removeClass($iconClass + " " + $divClass);
                }, $msgShowTime);
            }
        });
    },
    UploadEmailRegister: function (email) {
        $.ajax({
            url: '/emailregister/upload',
            data: { email: email },
            type: 'post',
            dataType: 'json',
            success: function (res) {
            }
        });
    },
    checkExist: function (name) {
        var result = false;
        $.ajax({
            url: '/Books/CheckExist',
            data: {
                name: name
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.result === true) {
                    userAccountScripts.Post();
                } else {
                    $('.validate').html('Tên ' + name + ' đã tồn tại!');
                }
            }
        });
    },
    loadTypeByCategory: function (categoryId) {
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
                        /// listOption1.append($('<option/>', { value: item.id, text: item.name }));
                    });
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
            LongDescription: LongDescription
        };
        if (d.PdfLink === "" && d.MobiLink === "" && d.EpubLink === "") {
            alert("Vui lòng cung cấp ít nhất một đường dẫn tải ebook ở bên dưới!");
        } else {
            $.ajax({
                url: '/Books/Post',
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
        }
    },
    /*end upload ebook area*/
    sendFeedback: function () {
        var userId = $('#txtId').val();
        //var name = $('#name').val();
        var email = $('#email').val();
        var subject = $('#subject').val();
        var message = $('#message').val();
        var data = {
            UserId: userId,
            Subject: subject,
            Email: email,
            Message: message
        };
        $.ajax({
            url: '/feedback/post',
            data: { feedback: JSON.stringify(data) },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    toastr.success("Cảm ơn bạn đã gởi phản hồi đến chúng tôi!");
                }
            }
        });
    },
    SaveChange: function () {
        var id = $('#txtId').val();
        var displayName = $('#txtName').val();
        var address = $('#txtAddress').val();
        var phone = $('#txtPhoneNumber').val();
        var e = {
            Id: id,
            DisplayName: displayName,
            Address: address,
            PhoneNumber: phone
        };
        $.ajax({
            url: '/UserAccount/UpdateUser',
            data: {
                user: JSON.stringify(e)
            },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    $('#editModal').modal('hide');
                    location.reload();
                    console.log(res.message);
                } else {
                    console.log("lỗi cập nhật!!!");
                }
            }
        });
    },
    resetPassword: function (email) {
        $.ajax({
            url: '/UserAccount/SendPasswordResetLink',
            data: { email: email },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (status) {
                    $('#error-rs').html(res.message);
                    $('#error-rs').delay(500).fadeIn();
                    $('#error-rs').delay(2000).fadeOut();
                } else {
                    $('#error-rs').html(res.message);
                }
            }
        });
    },
    register: function (param) {
        var data = JSON.stringify(param);
        $.ajax({
            url: '/UserAccount/Register',
            data: {
                models: data
            },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                var errorlist = $('#validate-register');
                errorlist.html('');
                if (res.status === true) {
                    toastr.success(res.message);
                } else {
                    var data = res.message;
                    $.each(data, function (i, item) {
                        toastr.error(data[i]);
                    });
                }
            }
        });
    },
    login: function (userName, password, rememberMe) {
        $.ajax({
            url: '/UserAccount/Login',
            data: {
                userName: userName,
                password: password,
                rememberMe: rememberMe
            },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (res.status === true) {
                    toastr.success(res.message);
                    setTimeout(2000, location.reload());
                } else {
                    toastr.error(res.message);
                }
            }
        });
    }
};
userAccountScripts.Init();