/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('Shop.statistics', ['Shop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('statistic_revenue', {
                url: "/statistic_revenue",
                parent: 'base',
                templateUrl: "/app/components/revenueStatistic/revenueStatisticView.html",
                controller: "revenueStatisticController"
            });
    }
})();