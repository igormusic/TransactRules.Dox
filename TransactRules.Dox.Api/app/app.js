var app = angular.module('DoxRulesApp', ['ngRoute', 'ngResource', 'ui.bootstrap', 'smart-table','ngUpload']);

app.config(function ($routeProvider) {

    $routeProvider.when("/documentTemplates", {
        controller: "documentTemplateController",
        templateUrl: "/app/views/documentTemplates.html"
    });

    $routeProvider.when("/documents", {
        controller: "documentController",
        templateUrl: "/app/views/documents.html"
    });

    $routeProvider.when("/about", {
        controller: "aboutController",
        templateUrl: "/app/views/about.html"
    });

    $routeProvider.otherwise({ redirectTo: "/documents" });

});