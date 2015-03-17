'use strict';
app.factory('configService', function ($http) {

    var serviceBase = '/api/DocumentTemplates';
    var configurationFactory = {};

    var _saveDocumentTemplate= function (documentTemplate) {
        //process venue to take needed properties

        //var miniVenue = {
        //    userName: userInContext,
        //    venueID: venue.id,
        //    venueName: venue.name,
        //    address: venue.location.address,
        //    category: venue.categories[0].shortName,
        //    rating: venue.rating
        //};

        //return $http.post(serviceBase, miniVenue).then(

        //    function (results) {
        //        toaster.pop('success', "Bookmarked Successfully", "Place saved to your bookmark!");
        //    },
        //    function (results) {
        //        if (results.status == 304) {
        //            toaster.pop('note', "Already Bookmarked", "Already bookmarked for user: " + miniVenue.userName);
        //        }
        //        else {
        //            toaster.pop('error', "Faield to Bookmark", "Something went wrong while saving :-(");
        //        }


        //        return results;
        //    });
    };

    var _getDocumentTemplates= function (pageIndex, pageSize) {

        return $http.get(serviceBase, { params: { page: pageIndex, pageSize: pageSize } }).then(function (results) {
            return results;
        });
    };

    
    configurationFactory.saveDocumentTemplate= _saveDocumentTemplate;
    configurationFactory.getDocumentTemplates = _getDocumentTemplates;

    return configurationFactory
});