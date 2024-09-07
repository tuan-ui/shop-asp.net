$(document).ready(function () {
    const itemsPerPage = 5;
    let currentPage = 1;
    let products = [];

    function renderProducts() {
        $('#productList').empty();
        const start = (currentPage - 1) * itemsPerPage;
        const end = Math.min(start + itemsPerPage, products.length);

        for (let i = start; i < end; i++) {
            const product = products[i];
            const stockStatus = product.Quantity > 0
                ? '<span class="label text-success">✔ Còn hàng</span>'
                : '<span class="label text-danger">✘ Hết hàng</span>';

            const listItem = `
                <div class="product-item">
                    <div class="row">
                        <div class="col-md-4">
                            <img src="${product.Image}" alt="${product.Name}" width="200" height="200" class="img-responsive">
                        </div>
                        <div class="col-md-4">
                            <h5 class="product-name">${product.Name}</h5>
                            <p class="product-price text-danger">${formatMoney(product.PromotionPrice) + 'đ'}</p>
                            <p class="product-warranty">Bảo hành: ${product.Warranty} Tháng</p>
                            <p class="product-stock">${stockStatus}</p>
                        </div>
                        <div class="col-md-4 text-center">
                            <button class="btn btn-primary select-product" data-product-cata="${product.CategoryID}" data-product-id="${product.ID}" 
                                                                    data-product-name="${product.Name}" data-product-price="${product.PromotionPrice}"
                                                                    data-product-image="${product.Image}">+</button>
                        </div>
                    </div>
                </div>
                <hr>
            `;
            $('#productList').append(listItem);
        }

        $('#prevPage').toggleClass('disabled', currentPage === 1);
        $('#nextPage').toggleClass('disabled', end >= products.length);
    }

    function fetchProducts(cata) {
        var productIds = [];
        $('.txtQuantity').each(function () {
            var productId = parseInt($(this).data('id'));
            productIds.push(productId);
        });
        $.ajax({
            url: '/BuildPC/GetProductsTag',
            type: 'GET',
            traditional: true,
            data: { productIds: productIds, cata: cata },
            success: function (response) {
                products = response;
                renderProducts();
            },
            error: function () {
                toastr.error('Đã xảy ra lỗi khi tải sản phẩm.');
            }
        });
    }

    $('#prevPage').click(function () {
        if (currentPage > 1) {
            currentPage--;
            renderProducts();
        }
    });

    $('#nextPage').click(function () {
        if (currentPage * itemsPerPage < products.length) {
            currentPage++;
            renderProducts();
        }
    });
    function calculateTotalPrice() {

        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });

        $('.total-price-display').text(formatMoney(total) + 'đ');
    }
    $(window).on('load', function () {
        calculateTotalPrice();
    }); 
    $(document).on('click', '.selectProductButton', function () {
        currentPage = 1;
        const cata = $(this).data('cata');
        fetchProducts(cata);
        $('#productModal').modal('show');
    });


    $('#productList').on('click', '.select-product', function () {
        const cata = $(this).data('product-cata');
        const productID = $(this).data('product-id');
        const productName = $(this).data('product-name');
        const productPrice = $(this).data('product-price');
        const productImage = $(this).data('product-image');
        const Quantity = 1;

        const newButtonHtml = `
            <div class="row">
                <div class="col-md-3">
                    <img src="${productImage}" alt="${productName}" width="150" height="150" class="img-responsive">
                </div>
                <div class="col-md-3">
                    <h5 class="product-name">${productName}</h5>
                    <p class="product-price text-danger">${formatMoney(productPrice) + 'đ'}</p>
                </div>
                <div class="col-md-3">
                    <input type="number" data-id="${productID}" data-price="${productPrice}" value="${Quantity}" class="input txtQuantity" size="40"/>
                </div>
                <div class="col-md-3">
                    <button class="btn btn-danger deleteProductButton" data-product-cata="${cata}"><i class="fa fa-times" aria-hidden="true"></i></button>
                    <button class="btn btn-warning selectProductButton" data-toggle="modal" data-target="#productModal" data-cata="${cata}"><i class="fa fa-refresh" aria-hidden="true"></i></button>
                    <button class="btn btn-success btnAddToCart" data-id="${productID}"><i class="ti-bag" aria-hidden="true"></i></button>
                </div>
            </div>
        `;

        $('.labelholder' + cata).html(newButtonHtml);
        toastr.success('Đã thêm sản phẩm.');
    });

    $('#productModal').on('hidden.bs.modal', function () {
        $('#productList').empty();
        calculateTotalPrice();
    });

    function formatMoney(amount) {
        return amount.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    }
    $(document).on('click', '.btnAddToCart', function () {
        var productId = parseInt($(this).data('id'));

        var quantityInput = $('input.txtQuantity[data-id="' + productId + '"]');

        var quantity = quantityInput.val();
        var success = false;
        for (let i = 0; i < quantity; i++) {
            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId
                },
                type: 'POST',
                dataType: 'json'
            });
            success = true;
        }
        if (success)
            toastr.success('Thêm sản phẩm vào giỏ hàng thành công.');
        else
            toastr.error('Thêm sản phẩm vào giỏ hàng thất bại.');
    });
    $(document).on('click', '.btnAddToCartAll', function () {
        var success = false;

        $('.txtQuantity').each(function () {
            var productId = parseInt($(this).data('id'));
            var quantity = parseInt($(this).val());

            for (let i = 0; i < quantity; i++) {
                $.ajax({
                    url: '/ShoppingCart/Add',
                    data: {
                        productId: productId
                    },
                    type: 'POST',
                    dataType: 'json'
                });
            }
            success = true;
        });

        if (success) {
            toastr.success('Thêm tất cả sản phẩm vào giỏ hàng thành công.');
        } else {
            toastr.error('Không có sản phẩm nào được thêm vào giỏ hàng.');
        }
    });
    $(document).on('click', '.btnCheckPC', function () {
        var productIds = [];
        $('.txtQuantity').each(function () {
            var productId = parseInt($(this).data('id'));
            productIds.push(productId);
        });
        if (productIds.length == 0) {
            var errorMgs = ' <div class="text-danger">Chua chọn sản phẩm</div>';
            $('.reponseHolder').html(errorMgs);
        }
        else {
            var productList = $('.reponseHolder');
            productList.empty();
            $.ajax({
                url: '/BuildPC/CheckPC',
                data: {
                    productIds: productIds
                },
                traditional: true,
                type: 'Get',
                dataType: 'json',
                success: function (response) {
                    productList.append(response.Mgs);
                }
            });
        }
    });
    $(document).on('click', '.deleteProductButton', function () {
        var cata = $(this).data('product-cata');
        var newButtonHtml = `
                 <button class="btn btn-primary selectProductButton" data-toggle="modal" data-target="#productModal" data-cata="${cata}">+</button>
        `;
        $('.labelholder' + cata).html(newButtonHtml);
    });
    $(document).on('click', '#ButtonBuild', function () {
        var priceRange = $("#price_range_component").data("ionRangeSlider");
        var selectedPrice = priceRange.result.from;
        var selectedPurpose = $("input[name='purpose']:checked").val();

        $("input[name='selectedPrice']").val(selectedPrice);

        $.ajax({
            url: '/BuildPC/FilterPurpose',
            type: 'GET',
            data: {
                purpose: selectedPurpose,
                selectedPrice: selectedPrice
            },
            success: function (response) {
                if (response == false) {
                    toastr.error("Hiện tại chưa có cấu hình phù hợp cho nhu cầu này");
                }
                else {
                    response.forEach(function (product) {
                        var newButtonHtml = `
                        <div class="row">
                            <div class="col-md-3">
                                <img src="${product.Image}" alt="${product.Name}" width="150" height="150" class="img-responsive">
                            </div>
                            <div class="col-md-3">
                                <h5 class="product-name">${product.Name}</h5>
                                <p class="product-price text-danger">${formatMoney(product.PromotionPrice) + 'đ'}</p>
                            </div>
                            <div class="col-md-3">
                                <input type="number" data-id="${product.ID}" data-price="${product.PromotionPrice}" value="1" class="input txtQuantity" size="40"/>
                            </div>
                            <div class="col-md-3">
                                <button class="btn btn-danger deleteProductButton" data-product-cata="${product.CategoryID}"><i class="fa fa-times" aria-hidden="true"></i></button>
                                <button class="btn btn-warning selectProductButton" data-toggle="modal" data-target="#productModal" data-cata="${product.CategoryID}"><i class="fa fa-refresh" aria-hidden="true"></i></button>
                                <button class="btn btn-success btnAddToCart" data-id="${product.ID}"><i class="ti-bag" aria-hidden="true"></i></button>
                            </div>
                        </div>
                    `;

                        $('.labelholder' + product.CategoryID).html(newButtonHtml);
                    });
                    calculateTotalPrice();
                }
            },
            error: function (xhr, status, error) {
                toastr.error(error);
            }
        });
    });
    $(document).on('click', '#Clear', function () {
        var productIds = [];
        $('.txtQuantity').each(function () {
            var productId = parseInt($(this).data('id'));
            productIds.push(productId);
        });
        $.ajax({
            url: '/BuildPC/GetCata',
            traditional: true,
            data: {
                productIds: productIds
            },
            type: 'GET',
            success: function (response) {
                response.forEach(function (cata) {
                    var newButtonHtml = `
                <button class="btn btn-primary selectProductButton" data-toggle="modal" data-target="#productModal" data-cata="${cata}">+</button>
                 `;
                    $('.labelholder' + cata).html(newButtonHtml);
                });
            }
        });
    });
});
