(function (app) {
    app.controller('promotionListController', promotionListController);

    promotionListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function promotionListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.promotion = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.keyword = '';
        $scope.getPromotion = getPromotion;
        $scope.search = search;
        $scope.deletePromotion = deletePromotion;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;
        $scope.isAll = false;
        function search() {
            getPromotion();
        }

        function getPromotion(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }
            $scope.loading = true;
            apiService.get('/api/promotion/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                $scope.promotion = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;

            }, function () {
                console.log('Load promotion failed.');
                $scope.loading = false;

            });
        }
        function deletePromotion(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/promotion/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }
        $scope.$watch("promotion", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.promotion, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.promotion, function (item) {
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
                    checkedpromotion: JSON.stringify(listId)
                }
            }
            apiService.del('api/promotion/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }
        $scope.getPromotion();
    }
})(angular.module('Shop.promotions'));