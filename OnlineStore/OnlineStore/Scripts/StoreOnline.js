
$('img').click(function () {
    var id = $(this).attr('id');
    var item = $('#lblitem_' + id).html();
    var price = $('#lblprice_' + id).html();
    var qty = $('#txtQty_' + id).val();


    $.get('../Basket/AddItem', { itemid: id, qty: qty, price: price, name: item }, function (data) {
        //$('#feedback').text(data);
        $("#baskettable tr:last").after('<tr><td>' + item + '</td><td>' + price + '</td><td>' + qty + '</td></tr>');
    });
});

$('#btnAddCart').click(function () {

    alert("You clicked Add cart button");
    var id = $('#hid_Id').attr('value');
    var item = $('#div_item').html();
    var price = $('#div_price').html();
    var qty = $('#div_qty').html();

    $.get('../../Basket/AddItem', { itemid: id, qty: qty, price: price, name: item }, function (data) {
        //$('#feedback').text(data);
        //        $("#baskettable tr:last").after('<tr><td>' + item + '</td><td>' + price + '</td><td>' + qty + '</td></tr>');
    });
});

$('#btnAddQty').click(function () {
    var currentValue = parseInt($('#div_qty').html(), 10);
    $('#div_qty').html(currentValue + 1);
});

$('#btnMinusQty').click(function () {
    var currentValue = parseInt($('#div_qty').html(), 10);
    if (currentValue != 1) {
        $('#div_qty').html(currentValue - 1);
    };

});

$("#cmdCheckOut").click(function () {
    var amount = 1500;
    $.get('/Store/Pay', { totalAmount: amount }, function () {
    });
});


$("#btnPay").click(function () {
    var qty = $('#txtCardNumber').val();
    alert('The card number is ' + qty);
    $.get('/Payment/CheckOut', { totalAmount: qty }, function () {
    });
});

