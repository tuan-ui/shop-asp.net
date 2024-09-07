(function (app) {
    'use strict';
    app.directive('fileUpload', function () {
        return {
            scope: true,       
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var files = event.target.files;
                    for (var i = 0; i < files.length; i++) {
                        scope.$emit("fileSelected", { file: files[i] });
                    }
                });
            }
        };
    });
})(angular.module('Shop.common'));