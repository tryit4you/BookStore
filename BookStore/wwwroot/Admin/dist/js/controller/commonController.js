"use strict";
var commonController = {
    Init: function () {
        commonController.RegisterEvent();
        commonController.getSeoTitle();
        //commonController.parseDate();
    },
    RegisterEvent: function () {
        $('.user-menu').click(function () {
            if ($('.user-menu').hasClass('open')) {
                $('.user-menu').removeClass('open');
            } else {
                $('.user-menu').addClass('open');
            }
        });
    },
    getSeoTitle: function (input) {
        if (input === undefined || input === '')
            return '';
        //Đổi chữ hoa thành chữ thường
        var slug = input.toLowerCase();

        //Đổi ký tự có dấu thành không dấu
        slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
        slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
        slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
        slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
        slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
        slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
        slug = slug.replace(/đ/gi, 'd');
        //Xóa các ký tự đặt biệt
        slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
        //Đổi khoảng trắng thành ký tự gạch ngang
        slug = slug.replace(/ /gi, "-");
        //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
        //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
        slug = slug.replace(/\-\-\-\-\-/gi, '-');
        slug = slug.replace(/\-\-\-\-/gi, '-');
        slug = slug.replace(/\-\-\-/gi, '-');
        slug = slug.replace(/\-\-/gi, '-');
        //Xóa các ký tự gạch ngang ở đầu và cuối
        slug = '@' + slug + '@';
        slug = slug.replace(/\@\-|\-\@|\@/gi, '');

        return slug;
    },
    //parseDate: function (datetime) {
    //    var temp_result = moment.utc(datetime, "x").toISOString();
    //    moment.locale('vi');
    //    var result = moment(temp_result).format('LL');
    //    return result;
    //},
    countDownload: function (count) {
        var output = "";
        if (count <= Math.pow(10, 3)) {
            output = count;
        } else if (count >= Math.pow(10, 3) && count < Math.pow(10, 6)) {
            output = Math.round(count / Math.pow(10, 3), 3) + "k";
        } else if (count >= Math.pow(10, 6)) {
            output = Math.round(count / Math.pow(10, 6), 3) + "M";
        }
        return output;
    },
    language: function (lang) {
        var output = "";
        if (lang === "Tiếng Việt") {
            output = "Vi";
        } else if (lang === "Tiếng Anh") {
            output = "En";
        } else if (lang === "Tiếng Trung") {
            output = "China"
        } else if (lang === "Tiếng Nhật") {
            output = "Jp";
        }
        return output;
    }
};
commonController.Init();