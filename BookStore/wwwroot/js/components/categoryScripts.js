var config = {
    pageSize: 16,
    pageIndex: 1
};
var categoryScripts = {
    Init: function () {
        categoryScripts.loadData();
        categoryScripts.registerEvent();
    },
    registerEvent: function () {
        $('.btn-search').off('click').on('click', function () {
            categoryScripts.loadData(true);
        });
        $('#txtSearch').keypress(function (event) {
            if (event.keyCode === 13) {
                categoryScripts.loadData(true);
            }
        });
        $('#txtSearch').on('click', function () {
            $('#txtSearch').val('');
        });
    },
    loadData: function (changePageSize) {
        var id = $('.hiddenId').val();
        var search = $('#txtSearch').val();
        $.ajax({
            url: '/books/GetByCategory',
            type: 'POST',
            data: {
                id: id,
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
                    var totalPage = Math.ceil(response.total / config.pageSize);
                    categoryScripts.paging(response.total, response.page, function () {
                        categoryScripts.loadData();
                        $('#currentpage').html(config.pageIndex);
                        $('#totalpage').html(totalPage);
                    }, changePageSize);
                    categoryScripts.registerEvent();
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
categoryScripts.Init();