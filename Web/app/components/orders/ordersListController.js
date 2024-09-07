(function (app) {
    app.controller('ordersListController', ordersListController);

    ordersListController.$inject = ['$scope', 'apiService', 'notificationService', '$uibModal' ];

    function ordersListController($scope, apiService, notificationService, $uibModal) {
        $scope.order = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.keyword = '';
        $scope.getOrders = getOrders;
        $scope.search = search;
        $scope.exportPdf = exportPdf;
        $scope.exportExcel = exportExcel;
        $scope.format = 'dd/MM/yyyy';
        var now = new Date();
        $scope.fromDate = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.toDate = new Date();
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.popup1 = {
            opened: false
        };

        $scope.popup2 = {
            opened: false
        };

        $scope.open1 = function () {
            $scope.popup1.opened = true;
        };

        $scope.open2 = function () {
            $scope.popup2.opened = true;
        };
        function exportExcel() {
            var config = {
                params: {
                    filter: $scope.keyword,
                    fromDate: $scope.fromDate,
                    toDate: $scope.toDate
                }
            }
            apiService.get('/api/order/ExportXls', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }
        function exportPdf(productId) {
            var config = {
                params: {
                    id: productId
                }
            }
            apiService.get('/api/order/ExportPdf', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }
        function search() {
            getOrders();
        }

        function getOrders(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }
            $scope.loading = true;
            apiService.get('/api/order/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                $scope.order = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;

            }, function () {
                console.log('Load getOrder failed.');
                $scope.loading = false;

            });
        }
        
     
        
        $scope.getOrders();
    }
})(angular.module('Shop.orders'));