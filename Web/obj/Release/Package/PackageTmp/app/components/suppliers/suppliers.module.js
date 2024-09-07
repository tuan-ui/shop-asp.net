/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('Shop.suppliers', ['Shop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('suppliers', {
            url: "/suppliers",
            parent: 'base',
            templateUrl: "/app/components/suppliers/suppliersListView.html",
            controller: "suppliersListController"
        })
            .state('add_suppliers', {
                url: "/add_suppliers",
                parent: 'base',
                templateUrl: "/app/components/suppliers/suppliersAddView.html",
                controller: "suppliersAddController"
            })
            .state('edit_suppliers', {
                url: "/edit_suppliers/:id",
                parent: 'base',
                templateUrl: "/app/components/suppliers/suppliersEditView.html",
                controller: "suppliersEditController"
            });
    }
})();