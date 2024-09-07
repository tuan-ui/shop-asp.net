var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('#btnCommentNew').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var productid = btn.data('productid');
            var userid = btn.data('userid');
            var commentmsg = document.getElementById('txtCommentNew');
            var rate = document.getElementById('ddlRate');
            if (commentmsg.value == "") {
                alert('Chưa nhập nội dung bình luận');
                return;
            }
            $.ajax({
                url: "/Product/AddNewComment",
                data: {
                    productid: productid,
                    userid: userid,
                    parentid: 0,
                    commentmsg: commentmsg.value,
                    rate: rate.value
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        commentmsg.value = "";
                        alert('Bạn đã thêm bình luận thành công');
                        $("#div_allcomment").load("/Product/GetComment?productid=" + productid);
                    }
                    else {
                        alert(response.mgs);
                    }
                }
            });
        });

        $('.subcomment').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var productid = btn.data('productid');
            var userid = btn.data('userid');
            var parenid = btn.data('parentid');

            var commentmsg = btn.data('commentmsg');

            var commentmsgvalue = document.getElementById(commentmsg);


            if (commentmsgvalue.value == "") {

                alert('Chưa nhập nội dung bình luận');
                return;
            }
            $.ajax({
                url: "/Product/AddNewComment",
                data: {
                    productid: productid,
                    userid: userid,
                    parentid: parenid,
                    commentmsg: commentmsgvalue.value,
                    rate: 0
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        commentmsg.value = "";
                        alert('Bạn đã thêm bình luận thành công');
                       
                        $("#div_allcomment").load("/Product/GetComment?productid=" + productid);
                    }
                    else {
                        alert(response.mgs);
                    }
                }
            });
        });
        $('.getallcomment').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var productid = btn.data('productid');
            $("#div_allcomment").load("/Product/GetCommentAll?productid=" + productid);
            
        });
        $('.deletecomment').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            var productid = btn.data('productid');
            $.ajax({
                url: "/Product/DeleteComment",
                data: {
                    id: id,
                    productid: productid
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        alert('Bạn đã xóa bình luận thành công');

                        $("#div_allcomment").load("/Product/GetComment?productid=" + response.productid);
                    }
                    else {
                        alert('Xóa bình luận lỗi');
                    }
                }
            });
        });

    }
}
product.init();