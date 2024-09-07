(function (app) {
    app.controller('ordersEditController', ordersEditController);

    ordersEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function ordersEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.order = {};
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.UpdateOrder = UpdateOrder;

        function loadOrderDetail() {
            apiService.get('api/order/getbyid/' + $stateParams.id, null, function (result) {
                $scope.order = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateOrder() {
            apiService.put('api/order/update', $scope.order,
                function (result) {
                    notificationService.displaySuccess(result.data.ID + ' đã được cập nhật.');
                    $state.go('orders');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
       
        loadOrderDetail();
    }

})(angular.module('Shop.orders'));