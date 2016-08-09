(function () {
    "use strict";

    angular
   .module("common.services")
   .factory("statistic",
           ["$resource",
            "appSettings",
               statistic])

    function statistic($resource, appSettings) {
        return $resource(appSettings.serverPath + "/api/statistic?path=:path");
    }

    angular
  .module("common.services")
  .factory("folderData",
          ["$resource",
           "appSettings",
              folderData])

    function folderData($resource, appSettings) {
        return $resource(appSettings.serverPath + "/api/folderdata?path=:path");
    }

}());

