/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('Shop.orders', ['Shop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('orders', {
            url: "/orders",
            parent: 'base',
            templateUrl: "/app/components/orders/ordersListView.html",
            controller: "ordersListController"
        })
            .state('edit_orders', {
            url: "/edit_orders/:id",
            parent: 'base',
            templateUrl: "/app/components/orders/ordersEditView.html",
            controller: "ordersEditController"
    });
    }
})();