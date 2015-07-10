var XboxInternals;
(function (XboxInternals) {
    var IO;
    (function (IO) {
        (function (EndianType) {
            EndianType[EndianType["BigEndian"] = 0] = "BigEndian";
            EndianType[EndianType["LittleEndian"] = 1] = "LittleEndian";
            EndianType[EndianType["Default"] = 2] = "Default";
        })(IO.EndianType || (IO.EndianType = {}));
        var EndianType = IO.EndianType;
        var BaseIO = (function () {
            function BaseIO(buffer) {
                this.buffer = buffer;
                this.SetPosition(0);
                this.byteOrder = 0 /* BigEndian */;
            }
            BaseIO.prototype.SetEndian = function (byteOrder) {
                this.byteOrder = byteOrder;
            };
            BaseIO.prototype.GetEndian = function () {
                return this.byteOrder;
            };
            BaseIO.prototype.SwapEndian = function () {
                if (this.byteOrder == 0 /* BigEndian */)
                    this.byteOrder = 1 /* LittleEndian */;
                else
                    this.byteOrder = 0 /* BigEndian */;
            };
            BaseIO.prototype.SetPosition = function (value) {
                this._position = value;
            };
            BaseIO.prototype.GetPosition = function () {
                return this._position;
            };
            BaseIO.prototype.GetLength = function () {
                return this.buffer.byteLength;
            };
            BaseIO.prototype.SetBuffer = function (buffer) {
                this.buffer = buffer;
            };
            BaseIO.prototype.ReadByte = function () {
                return this.Clone(new Uint8Array(this.buffer, this._position++, 1));
            };
            BaseIO.prototype.ReadBytes = function (len) {
                var ret = new Uint8Array(this.buffer, this._position, len);
                this.SetPosition(this.GetPosition() + len);
                return this.Clone(ret);
            };
            BaseIO.prototype.ReadUInt8 = function () {
                var view = new DataView(this.buffer, this._position, 1);
                this.SetPosition(this.GetPosition() + 1);
                return view.getUint8(0);
            };
            BaseIO.prototype.ReadInt16 = function () {
                var view = new DataView(this.buffer, this._position, 2);
                this.SetPosition(this.GetPosition() + 2);
                return view.getInt16(0, this.byteOrder == 1);
            };
            BaseIO.prototype.ReadWord = function () {
                var view = new DataView(this.buffer, this._position, 2);
                this.SetPosition(this.GetPosition() + 2);
                return view.getUint16(0, this.byteOrder == 1);
            };
            BaseIO.prototype.ReadInt24 = function (et) {
                if (et === void 0) { et = 2 /* Default */; }
                var orig = this.byteOrder;
                if (et != 2 /* Default */)
                    this.byteOrder = et;
                var int24Bytes = this.ReadBytes(0x3);
                var returnVal;
                if (this.byteOrder == 1 /* LittleEndian */)
                    returnVal = (int24Bytes[2] << 16) | (int24Bytes[1] << 8) | (int24Bytes[0]);
                else
                    returnVal = (int24Bytes[0] << 16) | (int24Bytes[1] << 8) | (int24Bytes[2]);
                //this.SetPosition(this.GetPosition() - 1);
                this.byteOrder = orig;
                return returnVal;
            };
            BaseIO.prototype.ReadInt32 = function () {
                var view = new DataView(this.buffer, this.GetPosition(), 4);
                this.SetPosition(this.GetPosition() + 4);
                return view.getInt32(0, this.byteOrder == 1);
            };
            BaseIO.prototype.ReadDword = function () {
                var view = new DataView(this.buffer, this.GetPosition(), 4);
                this.SetPosition(this.GetPosition() + 4);
                return view.getUint32(0, this.byteOrder == 1);
            };
            BaseIO.prototype.ReadMultiByte = function (size) {
                switch (size) {
                    case 1:
                        return this.ReadUInt8();
                    case 2:
                        return this.ReadWord();
                    case 4:
                        return this.ReadDword();
                    default:
                        throw "BaseIO: Invalid multi-byte size.";
                }
            };
            BaseIO.prototype.ReadFloat = function () {
                var view = new DataView(this.buffer, this.GetPosition(), 4);
                this.SetPosition(this.GetPosition() + 4);
                return view.getFloat32(0, this.byteOrder == 1);
            };
            BaseIO.prototype.ReadDouble = function () {
                var view = new DataView(this.buffer, this.GetPosition(), 8);
                this.SetPosition(this.GetPosition() + 4);
                return view.getFloat64(0, this.byteOrder == 1);
            };
            BaseIO.prototype.ReadString = function (len, nullTerminiator, forceInclude0, maxLength) {
                if (len === void 0) { len = -1; }
                if (nullTerminiator === void 0) { nullTerminiator = 0; }
                if (forceInclude0 === void 0) { forceInclude0 = true; }
                if (maxLength === void 0) { maxLength = 0x7FFFFFFF; }
                var stringBytes = new Uint8Array(new Uint8Array(this.buffer, this._position, len));
                var i = 0;
                for (i = 0; i < stringBytes.length; i++)
                    if (i + 1 < stringBytes.length && stringBytes[i + 1] == nullTerminiator) {
                        i++;
                        break;
                    }
                var val = String.fromCharCode.apply(null, new Uint8Array(new Uint8Array(this.buffer, this._position, i)));
                this.SetPosition(this.GetPosition() + len);
                return val;
            };
            BaseIO.prototype.ReadWString = function (len, nullTerminiator, forceInclude0, maxLength) {
                if (len === void 0) { len = -1; }
                if (nullTerminiator === void 0) { nullTerminiator = 0; }
                if (forceInclude0 === void 0) { forceInclude0 = true; }
                if (maxLength === void 0) { maxLength = 0x7FFFFFFF; }
                var stringBytes = new Uint16Array(len);
                var i = 0;
                var currentChar;
                var origPosition = this.GetPosition();
                while ((i++ < len || len == -1) && (currentChar = this.ReadWord()) != nullTerminiator)
                    stringBytes[i - 1] = currentChar;
                if (len != -1)
                    this.SetPosition(origPosition + len);
                return String.fromCharCode.apply(null, stringBytes.subarray(0, i));
            };
            BaseIO.prototype.ReadImage = function (length) {
                var binary = '';
                var bytes = this.Clone(new Uint8Array(this.buffer, this._position, length));
                for (var i = 0; i < bytes.length; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                var element = document.createElement("img");
                element.src = "data:image/png;base64," + btoa(binary);
                this.SetPosition(this.GetPosition() + length);
                return element;
            };
            BaseIO.prototype.WriteByte = function (byte) {
                var view = new DataView(this.buffer, this._position, 1);
                view.setUint8(0, byte[0]);
                this.SetPosition(this.GetPosition() + 1);
            };
            BaseIO.prototype.WriteBytes = function (bytes) {
                var view = new DataView(this.buffer, this._position, bytes.length);
                for (var i = 0; i < bytes.length; i++)
                    view.setUint8(i, bytes[i]);
                this.SetPosition(this.GetPosition() + bytes.length);
            };
            BaseIO.prototype.WriteWord = function (word) {
                var view = new DataView(this.buffer, this._position, 2);
                view.setInt16(0, word, this.byteOrder == 1);
                this.SetPosition(this.GetPosition() + 2);
            };
            BaseIO.prototype.WriteDword = function (dword) {
                var view = new DataView(this.buffer, this._position, 4);
                view.setInt32(0, dword, this.byteOrder == 1);
                this.SetPosition(this.GetPosition() + 4);
            };
            BaseIO.prototype.WriteInt24 = function (i24, et) {
                if (et === void 0) { et = 2 /* Default */; }
                var byteArray = new Uint8Array(3);
                var orig = this.byteOrder;
                if (et != 2 /* Default */)
                    this.byteOrder = et;
                if (this.byteOrder == 1 /* LittleEndian */) {
                    i24 <<= 8;
                    for (var i = 0; i < 3; i++)
                        byteArray[2 - i] = (i24 >> ((i + 1) * 8)) & 0xFF;
                    this.reverseByteArray(byteArray);
                }
                else {
                    for (var i = 0; i < 3; i++)
                        byteArray[2 - i] = (i24 >> (i * 8)) & 0xFF;
                }
                this.WriteBytes(byteArray);
                this.byteOrder = orig;
            };
            BaseIO.prototype.WriteString = function (str, forceLen, nullTermination, nullTerminator) {
                if (forceLen === void 0) { forceLen = -1; }
                if (nullTermination === void 0) { nullTermination = true; }
                if (nullTerminator === void 0) { nullTerminator = 0; }
                var stringArray = new Uint8Array(str.length + ((nullTermination) ? 1 : 0));
                for (var i = 0; i < str.length; i++)
                    stringArray[i] = str.charCodeAt(i);
                if (nullTermination)
                    stringArray[str.length] = nullTerminator;
                this.WriteBytes(stringArray);
                if (forceLen > 0) {
                    forceLen -= str.length;
                    var nullTerminatorArray = new Uint8Array(forceLen);
                    for (var i = 0; i < forceLen; i++)
                        nullTerminatorArray[i] = nullTerminator;
                    this.WriteBytes(nullTerminatorArray);
                }
            };
            BaseIO.prototype.reverseByteArray = function (array) {
                var temp;
                for (var i = 0; i < array.length / 2; i++) {
                    temp = array[i];
                    array[i] = array[array.length - i - 1];
                    array[array.length - i - 1] = temp;
                }
                return array;
            };
            // Clones the variable to stop the stream from writing as soon as the variable is changed.
            // Fixes the flags bug.
            BaseIO.prototype.Clone = function (obj) {
                // Handle the 3 simple types, and null or undefined
                if (null == obj || "object" != typeof obj)
                    return obj;
                // Handle Date
                if (obj instanceof Date) {
                    var copy = new Date();
                    copy.setTime(obj.getTime());
                    return copy;
                }
                // Handle Array
                if (obj instanceof Array) {
                    var copyArray = [];
                    for (var i = 0, len = obj.length; i < len; i++) {
                        copyArray[i] = this.Clone(obj[i]);
                    }
                    return copyArray;
                }
                if (obj instanceof Uint8Array) {
                    var copyIntArray = new Uint8Array(obj.length);
                    for (var i = 0; i < copyIntArray.length; i++)
                        copyIntArray[i] = obj[i];
                    return copyIntArray;
                }
                // Handle Object
                if (obj instanceof Object) {
                    var copyObject = {};
                    for (var attr in obj) {
                        if (obj.hasOwnProperty(attr))
                            copyObject[attr] = this.Clone(obj[attr]);
                    }
                    return copyObject;
                }
                throw new Error("Unable to copy obj! Its type isn't supported.");
            };
            BaseIO.prototype.Save = function (fileName) {
                // Creates blob from the arraybuffer.
                var blob = new Blob([this.buffer], { type: "application/octet-stream" });
                // Save blob exists, Save the File using function.
                if (navigator.msSaveBlob) {
                    navigator.msSaveBlob(blob, fileName);
                }
                else {
                    // Create download link to invok click on.
                    var downloadLink = document.createElement("a");
                    downloadLink.download = fileName;
                    // Set href to the Object URL from the Blob.
                    if (window.webkitURL != null) {
                        downloadLink.href = window.webkitURL.createObjectURL(blob);
                    }
                    else {
                        downloadLink.href = window.URL.createObjectURL(blob);
                        downloadLink.onclick = function (event) {
                            document.body.removeChild(event.target);
                        };
                        downloadLink.style.display = "none";
                        document.body.appendChild(downloadLink);
                    }
                    // Invoke click to download.
                    downloadLink.click();
                }
            };
            return BaseIO;
        })();
        IO.BaseIO = BaseIO;
    })(IO = XboxInternals.IO || (XboxInternals.IO = {}));
})(XboxInternals || (XboxInternals = {}));
//# sourceMappingURL=BaseIO.js.map