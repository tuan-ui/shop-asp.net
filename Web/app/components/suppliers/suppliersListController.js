(function (app) {
    app.controller('suppliersListController', suppliersListController);

    suppliersListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function suppliersListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.productCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.keyword = '';
        $scope.getSuppliers = getSuppliers;
        $scope.search = search;
        $scope.deleteSuppliers = deleteSuppliers;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        $scope.isAll = false;
        function search() {
            getSuppliers();
        }

        function getSuppliers(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }
            $scope.loading = true;
            apiService.get('/api/suppliers/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                $scope.suppliers = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;

            }, function () {
                console.log('Load suppliers failed.');
                $scope.loading = false;

            });
        }
        function deleteSuppliers(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa? Việc xóa nhà cung cấp sẽ xóa tất cả sản phẩm thuộc nhà cung cấp này').then(function () {
                var config = {
                    params: {
                        id: id
                    } 
                }
                apiService.del('api/suppliers/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }
        $scope.$watch("suppliers", function (n, o) {
            if (angular.isArray(n)) {
                var checked = $filter("filter")(n, { checked: true });
                if (checked.length) {
                    $scope.selected = checked;
                    $('#btnDelete').removeAttr('disabled');
                } else {
                    $('#btnDelete').attr('disabled', 'disabled');
                }
            }
        }, true);
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.suppliers, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.suppliers, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }
        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedSuppliers: JSON.stringify(listId)
                }
            }
            apiService.del('api/suppliers/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }
        $scope.getSuppliers();
    }
})(angular.module('Shop.suppliers'));