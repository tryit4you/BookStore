var config = {
    pageSize: 16,
    pageIndex: 1
};
var homeScripts = {
    Init: function () {
        homeScripts.loadData();
        homeScripts.registerEvent();
    },
    registerEvent: function () {
        $('.btn-search').off('click').on('click', function () {
            homeScripts.loadData(true);
        });
        $('#txtSearch').keypress(function (event) {
            if (event.keyCode === 13) {
                homeScripts.loadData(true);
            }
        });
        $('#txtSearch').on('click', function () {
            $('#txtSearch').val('');
        });
        $('.btnPause').on('click', function (e) {
            e.preventDefault();
            if ($('.btnPause > .fa-pause').length === 1) {
                $('.btnPause > .fa').removeClass('fa-pause');
                $('.btnPause > .fa').addClass('fa-play');
                document.getElementById('marquee').stop();
            } else if ($('.btnPause > .fa-pause').length === 0) {
                $('.btnPause > .fa').removeClass('fa-play');
                $('.btnPause > .fa').addClass('fa-pause');
                document.getElementById('marquee').start();
            }
        });
    },
    loadData: function (changePageSize) {
        var search = $('#txtSearch').val();

        $.ajax({
            url: '/books/GetAll',
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
                    if (response.filter !== null) {
                        if (data.length === 0) {
                            toastr.clear();
                            toastr.info("Không có kết quả nào cho từ khóa " + "'" + response.filter + "'");
                        }
                    }
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        var url = "/v/" + item.metaName + "/" + item.id + "/";
                        html += Mustache.render(template, {
                            url: url,
                            name: item.name,
                            thumbnailUrl: item.thumbnailUrl,
                            downloads: commonController.countDownload(item.countDownload),
                            dateRelease: item.dateRelease,
                            Language: commonController.language(item.language)
                        });
                    });
                    $('#data-render').html(html);
                    homeScripts.paging(response.total, response.page, function () {
                        homeScripts.loadData();
                    }, changePageSize);
                }
            }
        });
    },
    paging: function (totalRow, page, callback) {
        if ($('#pagination a').length === 0) {
            $('#pagination').empty();
            $('#pagination').removeData("pagination");
            $('#pagination').unbind("page");
        }
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
homeScripts.Init();