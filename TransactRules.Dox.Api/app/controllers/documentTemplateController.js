'use strict';
app.controller('documentTemplateController', function ($scope) {

    // Clear form (reason for using the 'ng-model' directive on the input elements)
    $scope.templateName = '';

    $scope.documentTemplates = [
        { templateName: 'Loan Agreement', fileName: 'Loan Agreement 01.docx', size:389743 },
        { templateName: 'Loan Application', fileName: 'Loan Application 01.docx', size: 343255 },
        { templateName: 'Credit Bureau Authorization', fileName: 'Creadit Bureau Authorization 01.docx', size: 23435 }
    ];

    $scope.addTemplate = function addRandomItem() {
        $scope.documentTemplates.push({ templateName: 'Another Doc', fileName: 'AnotherLongNameDocument01.docx', size:33545 });
    };

    $scope.uploadComplete = function (content) {
        $scope.response = JSON.parse(content); // Presumed content is a json string!
        $scope.response.style = {
            color: $scope.response.color,
            "font-weight": "bold"
        };
    }
});

