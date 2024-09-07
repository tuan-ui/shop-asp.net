(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];
        $scope.format = 'dd/MM/yyyy';
        var now = new Date();
        $scope.changeDate = changeDate;
        $scope.fromDate = new Date(now.getFullYear(), now.getMonth()-1, 1);
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
        $scope.chartdata = [];

        function getStatistic() {
            var config = {
                param: {
                    fromDate: $filter('date')($scope.fromDate, $scope.format),
                    toDate: $filter('date')($scope.toDate, $scope.format)
                }
            };


            apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                var dataByDate = {};

                $.each(response.data, function (i, item) {
                    var date = $filter('date')(item.Date, 'dd/MM/yyyy');
                    if (!dataByDate[date]) {
                        dataByDate[date] = {
                            Date: date,
                            Revenues: 0,
                            Benefit: 0
                        };
                    }
                    dataByDate[date].Revenues += item.Revenues;
                    dataByDate[date].Benefit += item.Benefit;
                });

                var labels = [];
                var revenues = [];
                var benefits = [];
                var chartData = [];

                $.each(dataByDate, function (date, item) {
                    labels.push(date);
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });

                chartData.push(revenues);
                chartData.push(benefits);

                $scope.tabledata = Object.values(dataByDate);
                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                console.error('Error response:', response);
                notificationService.displayError('Không thể tải dữ liệu');
            });
        }
        function changeDate() {
            getStatistic();
        }
        getStatistic();
    }

})(angular.module('Shop.statistics'));