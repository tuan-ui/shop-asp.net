var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Yêu cầu nhập tên",
                address: "Yêu cầu nhập địa chỉ",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                phone: {
                    required: "Số điện thoại được yêu cầu",
                    number: "Số điện thoại phải là số."
                }
            }
        });
        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });
        $('.txtQuantity').off('keyup').on('keyup', function () {
            var quantity = parseInt($(this).val());
            var productid = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (quantity <= 0) {
                alert('Số lượng không được âm hoặc bằng 0');
                $(this).val(1);
                quantity = 1;
            }
            if (isNaN(quantity) == false) {

                var amount = quantity * price;

                $('#amount_' + productid).text(numeral(amount).format('0,0'));
            }
            else {
                $('#amount_' + productid).text(0);
            }

            $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
            $('#lblTotalOrder1').text(numeral(cart.getTotalOrder()).format('0,0'));
            cart.updateAll();
            cart.getprice('#txtpromotion');
            cart.configPrice();
        });
        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });
        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();  
        });
        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            cart.configPrice();
            $('#divCheckout').show();
        });
        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked'))
                cart.getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
            }

        });
        $('#lblPromo').off('keyup').on('keyup', function () {
            var temp = parseFloat($('#lblTotalOrder1').text().replace(/,/g, ''));
            var promo = parseFloat($('#lblPromo').text().replace(/,/g, '')) || 0;
            var total = temp - promo;
            $('#lblTotal').text(numeral(total).format('0,0'));
        });
        $('#txtpromotion').off('keyup').on('keyup', function () {
            cart.getprice(this);        
        });
    },
    configPrice: function () {
        $('#lblTotalOrder1').text(numeral(cart.getTotalOrder()).format('0,0'));
        $('#lblPromo').text(numeral(0).format('0,0'));
        var temp = parseFloat($('#lblTotalOrder1').text().replace(/,/g, ''));
        var promo = parseFloat($('#lblPromo').text().replace(/,/g, '')) || 0;
        var total = temp - promo;
        $('#lblTotal').text(numeral(total).format('0,0'));
    },
    getprice: function (element) {
        var temp = parseFloat($('#lblTotalOrder1').text().replace(/,/g, ''));
        var promotion = $(element).val();
        if (promotion.length === 10) {
            $.ajax({
                url: '/ShoppingCart/Promo',
                type: 'POST',
                dataType: 'json',
                data: {
                    promotion: promotion,
                    temp: temp
                },
                success: function (response) {
                    if (response.status) {
                        $('#lblPromo').text(numeral(response.promo).format('0,0'));
                        $('#lblTotal').text(numeral(temp - response.promo).format('0,0'));
                    } else {
                        alert(response.message); 
                        $('#lblPromo').text(numeral(0).format('0,0'));
                        $('#lblTotal').text(numeral(temp).format('0,0'));
                    }
                }
            });
        }
    },
    createOrder: function () {
        var lblTotalOrder = $('#lblTotal').text(); 
        lblTotalOrder = lblTotalOrder.replace(/,/g, '');
        var TotalPrice = parseFloat(lblTotalOrder);
        var paymentMethod = $('input[name="paymentMethod"]:checked').val();
        var paymentMethodText = paymentMethod === 'vnpay' ? 'Thanh toán VNPay' : 'Thanh toán khi nhận hàng';

        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: paymentMethodText,
            TotalPrice: parseFloat(TotalPrice),
            Status: 0
        }
        if (paymentMethod === 'vnpay') {
            $.ajax({
                url: '/ShoppingCart/CreatePaymentUrl',
                type: 'POST',
                dataType: 'json',
                data: {
                    orderViewModel: JSON.stringify(order)
                },
                success: function (response) {
                    if (response.redirectUrl) {
                    window.location.href = response.redirectUrl;
                } 
                    if (response.code) {
                        alert(response.code);
                    }
                }
            });
        }
        else {
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    $('#divCheckout').hide();
                    cart.deleteAll();
                    setTimeout(function () {
                        $('#cartContent').html('Cảm ơn bạn đã đặt hàng. Mã đơn hàng của bạn là ' + response.orderId + '. Chúng tôi sẽ liên hệ sớm nhất.');
                    }, 2000);

                } else {
                    alert(response.message);
                }
            }
            });
        }
        var promotion = $('#txtpromotion').val();
        if (promotion != "") {
            $.ajax({
                url: '/ShoppingCart/MinusQuantity',
                type: 'POST',
                dataType: 'json',
                data: {
                    promotion: promotion
                },

            });
        }
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {   
                    cart.loadData();
                  
                }
            }
        });
    },
    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductId: $(item).data('id'),
                Quantity: $(item).val()
            });
        });
        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    
                }
            }
        });
    },
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    addItem: function (productId) {

    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();               
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            STT:i+1,
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Product.PromotionPrice,
                            PriceF: numeral(item.Product.PromotionPrice).format('0,0'),
                            Quantity: item.Quantity,
                            Amount: numeral(item.Quantity * item.Product.PromotionPrice).format('0,0')
                        });
                    });

                    $('#cartBody').html(html);
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                    $('#lblTotalOrder1').text(numeral(cart.getTotalOrder()).format('0,0'));
                    cart.getprice('#txtpromotion');    
                    cart.registerEvent();
                }
            }
        })
    }
}
cart.init();