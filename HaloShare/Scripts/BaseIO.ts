﻿
module XboxInternals.IO {

	export enum EndianType {
		BigEndian = 0,
		LittleEndian = 1,
		Default
	}

	export class BaseIO {


		private byteOrder: EndianType;
		public buffer: ArrayBuffer;
		private _position: number;

		constructor(buffer: ArrayBuffer) {
			this.buffer = buffer;
			this.SetPosition(0);
			this.byteOrder = EndianType.BigEndian;
		}

		public SetEndian(byteOrder: EndianType) {
			this.byteOrder = byteOrder;
		}

		public GetEndian(): EndianType {
			return this.byteOrder;
		}

		public SwapEndian() {
			if (this.byteOrder == EndianType.BigEndian)
				this.byteOrder = EndianType.LittleEndian;
			else
				this.byteOrder = EndianType.BigEndian;
		}

		public SetPosition(value: number) {
			this._position = value;
		}

		public GetPosition(): number {
			return this._position;
		}

		public GetLength(): number {
			return this.buffer.byteLength;
		}

		public SetBuffer(buffer: ArrayBuffer) {
			this.buffer = buffer;
		}

		public ReadByte(): Uint8Array {
			return this.Clone(new Uint8Array(this.buffer, this._position++, 1));
		}

		public ReadBytes(len: number): Uint8Array {
			var ret = new Uint8Array(this.buffer, this._position, len);
			this.SetPosition(this.GetPosition() + len);
			return this.Clone(ret);
		}

		public ReadUInt8(): number {
			var view = new DataView(this.buffer, this._position, 1);
			this.SetPosition(this.GetPosition() + 1);
			return view.getUint8(0);
		}

		public ReadInt16(): number {
			var view = new DataView(this.buffer, this._position, 2);
			this.SetPosition(this.GetPosition() + 2);
			return view.getInt16(0, this.byteOrder == 1);
		}

		public ReadWord(): number {
			var view = new DataView(this.buffer, this._position, 2);
			this.SetPosition(this.GetPosition() + 2);
			return view.getUint16(0, this.byteOrder == 1);
		}

		public ReadInt24(et: EndianType = EndianType.Default) {
			var orig: EndianType = this.byteOrder;

			if (et != EndianType.Default)
				this.byteOrder = et;

			var int24Bytes = this.ReadBytes(0x3);
			var returnVal;

			if (this.byteOrder == EndianType.LittleEndian)
				returnVal = (int24Bytes[2] << 16) | (int24Bytes[1] << 8) | (int24Bytes[0]);
			else
				returnVal = (int24Bytes[0] << 16) | (int24Bytes[1] << 8) | (int24Bytes[2]);

			//this.SetPosition(this.GetPosition() - 1);
			this.byteOrder = orig;

			return returnVal;
		}


		public ReadInt32(): number {
			var view = new DataView(this.buffer, this.GetPosition(), 4);
			this.SetPosition(this.GetPosition() + 4);
			return view.getInt32(0, this.byteOrder == 1);
		}

		public ReadDword(): number {
			var view = new DataView(this.buffer, this.GetPosition(), 4);
			this.SetPosition(this.GetPosition() + 4);
			return view.getUint32(0, this.byteOrder == 1);
		}

		public ReadMultiByte(size: number): number {
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
		}

		public ReadFloat(): number {
			var view = new DataView(this.buffer, this.GetPosition(), 4);
			this.SetPosition(this.GetPosition() + 4);
			return view.getFloat32(0, this.byteOrder == 1);
		}

		public ReadDouble(): number {
			var view = new DataView(this.buffer, this.GetPosition(), 8);
			this.SetPosition(this.GetPosition() + 4);
			return view.getFloat64(0, this.byteOrder == 1);
		}

		public ReadString(len = -1, nullTerminiator = 0, forceInclude0 = true, maxLength = 0x7FFFFFFF): string {

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
		}

		public ReadWString(len = -1, nullTerminiator = 0, forceInclude0 = true, maxLength = 0x7FFFFFFF): string {
			var stringBytes: Uint16Array = new Uint16Array(len);
			var i = 0;
			var currentChar;
			var origPosition = this.GetPosition();
			// Confusing code that reads the string in the correct endian
			while ((i++ < len || len == -1) && (currentChar = this.ReadWord()) != nullTerminiator)
				stringBytes[i - 1] = currentChar;

			if (len != -1)
				this.SetPosition(origPosition + len);
			return String.fromCharCode.apply(null, stringBytes.subarray(0, i));
		}

		public ReadImage(length: number): HTMLImageElement {
			var binary = '';
			var bytes = this.Clone(new Uint8Array(this.buffer, this._position, length));
			for (var i = 0; i < bytes.length; i++) {
				binary += String.fromCharCode(bytes[i]);
			}
			var element: HTMLImageElement = <HTMLImageElement>document.createElement("img");
			element.src = "data:image/png;base64," + btoa(binary);
			this.SetPosition(this.GetPosition() + length);
			return element;
		}

		public WriteByte(byte: Uint8Array) {
			var view = new DataView(this.buffer, this._position, 1);
			view.setUint8(0, byte[0]);
			this.SetPosition(this.GetPosition() + 1);
		}

		public WriteBytes(bytes: Uint8Array) {
			var view = new DataView(this.buffer, this._position, bytes.length);
			for (var i = 0; i < bytes.length; i++)
				view.setUint8(i, bytes[i]);
			this.SetPosition(this.GetPosition() + bytes.length);
		}

		public WriteWord(word: number) {
			var view = new DataView(this.buffer, this._position, 2);
			view.setInt16(0, word, this.byteOrder == 1);
			this.SetPosition(this.GetPosition() + 2);
		}

		public WriteDword(dword: number) {
			var view = new DataView(this.buffer, this._position, 4);
			view.setInt32(0, dword, this.byteOrder == 1);
			this.SetPosition(this.GetPosition() + 4);
		}

		public WriteInt24(i24: number, et: EndianType = EndianType.Default) {
			var byteArray = new Uint8Array(3);

			var orig = this.byteOrder;
			if (et != EndianType.Default)
				this.byteOrder = et;

			if (this.byteOrder == EndianType.LittleEndian) {
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
		}

		public WriteString(str: string, forceLen = -1, nullTermination = true, nullTerminator = 0) {
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
		}

		private reverseByteArray(array: Uint8Array): Uint8Array {
			var temp: number;
			for (var i = 0; i < array.length / 2; i++) {
				temp = array[i];
				array[i] = array[array.length - i - 1];
				array[array.length - i - 1] = temp;
			}
			return array;
		}

		// Clones the variable to stop the stream from writing as soon as the variable is changed.
		// Fixes the flags bug.
		public Clone(obj) {
			// Handle the 3 simple types, and null or undefined
			if (null == obj || "object" != typeof obj) return obj;

			// Handle Date
			if (obj instanceof Date) {
				var copy: any = new Date();
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
					if (obj.hasOwnProperty(attr)) copyObject[attr] = this.Clone(obj[attr]);
				}
				return copyObject;
			}

			throw new Error("Unable to copy obj! Its type isn't supported.");
		}

		public Save(fileName: string) {
			// Creates blob from the arraybuffer.
			var blob = new Blob([this.buffer], { type: "application/octet-stream" });

			// Save blob exists, Save the File using function.
			if (navigator.msSaveBlob) {
				navigator.msSaveBlob(blob, fileName);
			} else {

				// Create download link to invok click on.
				var downloadLink = <any>document.createElement("a");
				downloadLink.download = fileName;
				// Set href to the Object URL from the Blob.
				if ((<any>window).webkitURL != null) {
					downloadLink.href = (<any>window).webkitURL.createObjectURL(blob)
				} else {
					downloadLink.href = (<any>window).URL.createObjectURL(blob);
					downloadLink.onclick = (event) => {
						document.body.removeChild(event.target);
					};
					downloadLink.style.display = "none";
					document.body.appendChild(downloadLink);
				}
				// Invoke click to download.
				downloadLink.click();
			}
		}
	}

}