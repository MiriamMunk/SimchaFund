$(() => {
    $("#new-contributer-btn").on('click', function () {
        $("#NewContributerModal").modal();
    })
    $("#new-simcha-btn").on('click', function () {
        $("#NewSimchaModal").modal();
    })
    $("#tbody").on('click', '#deposit-btn', function () {
        const contribId = $(this).data('prsnid');
        $('[name="personId"]').val(contribId);

        const tr = $(this).parent().parent();
        const name = tr.find('td:eq(1)').text();
        $("#deposit-name").text(name);

        $("#DepositModal").modal();

    })
    $("#tbody").on('click', '#edit-btn', function () {
        $("#fn").val($(this).data('first-name'))
        $("#ln").val($(this).data('last-name'))
        $("#phone").val($(this).data('cell'))
        $("#created").val($(this).data('date'))
        $("#id").val($(this).data('id'))

        //const e = $(this).data('always-include')
        //$("#include").val(e).prop('checked', alwaysInclude === "True")

        $("#EditModal").modal();
    })
    $("#clear-btn").on('click', function () {
        $("#search").val('')
        $("table tr:gt(0)").show()
    })
    $("#search").on('input', function () {
        const text = $(this).val();
        
        $("table tr:gt(0)").each(function () {
            const tr = $(this);
            const name = tr.find('td:eq(1)').text();
            if (name.toLowerCase().indexOf(text.toLowerCase()) !== -1) {
                tr.show();
            } else {
                tr.hide();
            }
        })
    })
})