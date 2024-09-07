(function (app) {
    app.controller('suppliersAddController', suppliersAddController);

    suppliersAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function suppliersAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.suppliers = {
            CreatedDate: new Date(),
            Status: true
        }
        $scope.GetSeoTitle = GetSeoTitle;
        function GetSeoTitle() {
            $scope.suppliers.Alias = commonService.GetSeoTitle($scope.suppliers.Name);
        }
        $scope.AddSuppliers = AddSuppliers;

        function AddSuppliers() {
            apiService.post('api/suppliers/create', $scope.suppliers,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('suppliers');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }

    }

})(angular.module('Shop.suppliers'));