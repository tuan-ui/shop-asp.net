(function (app) {
    app.controller('suppliersEditController', suppliersEditController);

    suppliersEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function suppliersEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.suppliers = {
            CreatedDate: new Date(),
            Status: true
        }

        $scope.UpdateSuppliers = UpdateSuppliers;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.suppliers.Alias = commonService.GetSeoTitle($scope.suppliers.Name);
        }

        function loadSuppliersDetail() {
            apiService.get('api/suppliers/getbyid/' + $stateParams.id, null, function (result) {
                $scope.suppliers = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateSuppliers() {
            apiService.put('api/suppliers/update', $scope.suppliers,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('suppliers');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }

        loadSuppliersDetail();
    }

})(angular.module('Shop.suppliers'));