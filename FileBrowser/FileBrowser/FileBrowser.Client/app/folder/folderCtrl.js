(function () {
    "use strict";
    angular
        .module("folderManagement")
        .controller("FolderCtrl",
                     ["statistic", "folderData", '$timeout',
                         FolderListCtrl]);

    //filter to show only file or folder name
    angular.module("folderManagement").filter('onlyname', function () {
        return function (input) {

            var str = input;
            var stringsearch = "\\";
            var count = 0;
            // to avoid problems with path like "C:\"
            for (var i = 0; i < str.length; count += +(stringsearch === str[i++]));
            if (count > 1) {
                return input.split('\\').pop();
            }
            else
                return input;
        };
    });


    function FolderListCtrl(statistic, folderData, $timeout) {
        var vm = this;
        vm.currPath = "C:\\TestThisFolder";

        //return statistic from api
        vm.getStatistic = function (operatedFolder) {
            vm.statistic = { bigSize: "", middleSize: "please wait", smallSize: "" };
            statistic.get({ path: encodeURIComponent(operatedFolder) }, function (data) {

                $timeout(function () {
                    if (vm.currPath == String(data.currentFolder)) {
                        vm.statistic = { bigSize: data.bigSize, middleSize: data.middleSize, smallSize: data.smallSize };
                    }
                }, 2000);
            });
        }

        //return list of subfolders, list of files, current path and root path
        vm.getFolderData = function (operatedFolder) {
            folderData.get({ path: encodeURIComponent(operatedFolder) }, function (data) {
                if (data.error == true) {
                    for (var key in vm.folderData.subFolders) {
                        if (vm.folderData.subFolders[key] == String(data.currentFolder)) {
                            var keySave = vm.folderData.subFolders[key];
                            vm.folderData.subFolders[key] = "no access!";
                            $timeout(function () {
                                vm.folderData.subFolders[key] = keySave;
                            }, 3000);
                        }
                    }


                }
                else {
                    vm.folderData = { rootFolder: String(data.rootFolder), currentFolder: String(data.currentFolder), subFolders: data.subFolders, files: data.files };
                    vm.currPath = String(data.currentFolder);
                }

            }
            );
        }

        vm.getFolderData("onload");
        $timeout(function () {
            vm.getStatistic("onload");

        }, 0);
    }



}());
