(function (app) {
    app.controller('promotionAddController', promotionAddController);

    promotionAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function promotionAddController(apiService, $scope, notificationService, $state,commonService) {
        $scope.promotion = {
            Status: true,
            DateStart: '',
            DateEnd: ''
        }
        $scope.dateError = false;
        $scope.AddPromotion = AddPromotion;
        $scope.validateDates = function () {
            if ($scope.promotion.DateStart && $scope.promotion.DateEnd) {
                var startDate = new Date($scope.promotion.DateStart);
                var endDate = new Date($scope.promotion.DateEnd);
                $scope.dateError = startDate >= endDate;
            } else {
                $scope.dateError = false;
            }
        };
        function AddPromotion() {
            apiService.post('api/promotion/create', $scope.promotion,
                function (result) {
                    notificationService.displaySuccess(result.data.Code + ' đã được thêm mới.');
                    $state.go('promotions');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
        function loadProduct() {
            apiService.get('api/product/getallparents', null, function (result) {
                $scope.products = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }
        loadProduct();
    }

})(angular.module('Shop.promotions'));