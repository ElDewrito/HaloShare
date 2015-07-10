/// <reference path='BaseIO.ts' />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var XboxInternals;
(function (XboxInternals) {
    var IO;
    (function (IO) {
        var FileIO = (function (_super) {
            __extends(FileIO, _super);
            function FileIO(buffer) {
                _super.call(this, buffer);
            }
            FileIO.LoadFromFile = function (file, callback) {
                // Create new FileReader to read the File as a ArrayBuffer.
                var reader = new FileReader();
                reader.onloadend = function () {
                    var io = new FileIO(reader.result);
                    io.fileName = file.name;
                    callback(io);
                };
                reader.onerror = function (e) {
                    console.error(e.message);
                };
                reader.readAsArrayBuffer(file);
            };
            FileIO.prototype.SaveFile = function () {
                // Saves the file with the original file name.
                this.Save(this.fileName);
            };
            FileIO.prototype.SetFileName = function (name) {
                this.fileName = name;
            };
            FileIO.prototype.GetFileName = function () {
                return this.fileName;
            };
            return FileIO;
        })(IO.BaseIO);
        IO.FileIO = FileIO;
    })(IO = XboxInternals.IO || (XboxInternals.IO = {}));
})(XboxInternals || (XboxInternals = {}));
//# sourceMappingURL=FileIO.js.map