var detailScripts = {
    Init: function () {
        detailScripts.registerEvent();
    },
    registerEvent: function () {
        $('.btn-download').off('click').on('click', function () {
            var id = $('#ebookId').val();
            detailScripts.countDownload(id);
        });
        $('.show-des').on('click', function (e) {
            e.preventDefault();
            $('.description').toggleClass('show-desc', 1000);
            if ($('.show-des>.fa-angle-double-down').length === 1) {
                $('.show-des>.fa').removeClass('fa-angle-double-down');
                $('.show-des>.fa').addClass('fa-angle-double-up');
            } else {
                $('.show-des>.fa').removeClass('fa-angle-double-up');
                $('.show-des>.fa').addClass('fa-angle-double-down');
            }
        });
    },
    countDownload: function (id) {
        $.ajax({
            url: '/books/IncreaseDownload',
            data: { bookid: id },
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    console.log('success!');
                }
            }
        });
    }
}
detailScripts.Init();