/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('Shop.promotions', ['Shop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('promotions', {
            url: "/promotions",
            parent: 'base',
            templateUrl: "/app/components/promotions/promotionListView.html",
            controller: "promotionListController"
        })
            .state('add_promotions', {
                url: "/add_promotions",
                parent: 'base',
                templateUrl: "/app/components/promotions/promotionAddView.html",
                controller: "promotionAddController"
            })
            .state('edit_promotions', {
                url: "/edit_promotions/:id",
                parent: 'base',
                templateUrl: "/app/components/promotions/promotionEditView.html",
                controller: "promotionEditController"
            });
    }
})();