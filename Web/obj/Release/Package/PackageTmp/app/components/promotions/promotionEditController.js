(function (app) {
    app.controller('promotionEditController', promotionEditController);

    promotionEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams','commonService'];

    function promotionEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        
        $scope.promotion = {
            Status: true,
            DateStart: '',
            DateEnd: ''
        }
        $scope.dateError = false;
        $scope.UpdatePromotion = UpdatePromotion;
        $scope.validateDates = function () {
            if ($scope.promotion.DateStart && $scope.promotion.DateEnd) {
                var startDate = new Date($scope.promotion.DateStart);
                var endDate = new Date($scope.promotion.DateEnd);
                $scope.dateError = startDate >= endDate;
            } else {
                $scope.dateError = false;
            }
        };
        function loadProductIdDetail() {
            apiService.get('api/promotion/getbyid/' + $stateParams.id, null, function (result) {
                $scope.promotion = result.data;
                $scope.promotion.DateStart = new Date($scope.promotion.DateStart);
                $scope.promotion.DateEnd = new Date($scope.promotion.DateEnd);
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdatePromotion() {
            apiService.put('api/promotion/update', $scope.promotion,
                function (result) {
                    notificationService.displaySuccess(result.data.Code + ' đã được cập nhật.');
                    $state.go('promotions');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
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
        loadProductIdDetail();
    }

})(angular.module('Shop.promotions'));