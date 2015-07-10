$(".file-select").on('dragenter', function (e) {
	$(".file-select").addClass("active");
});
$(".file-select").on('dragleave', function (e) {
	$(".file-select").removeClass("active");
});
$(".file-select").on('mouseleave',() => {
	$(".file-select").removeClass("active");
});

var variant: VariantLib.Variant;

var $inputForm = <HTMLFormElement>document.getElementById("input-form");
var $inputFile = <HTMLInputElement>document.getElementById("input-file");

$("#input-file").change(() => {
	hideAlert();

	var file: File = $inputFile.files[0];


	XboxInternals.IO.FileIO.LoadFromFile(file,(io) => {
		// Make sure we are using the correct Endian type.
		io.SetEndian(XboxInternals.IO.EndianType.LittleEndian);

		
		var detected = VariantLib.VariantDetector.Detect(io);

		switch (detected) {
			case VariantLib.VariantType.ForgeVariant:
				loadForgeVariant(io);
				break;
			case VariantLib.VariantType.GameVariant:
				loadGameVariant(io);
				break;
			default:
				$inputFile.value = "";
				if (file.name.indexOf(".frg") != -1) {
					showAlert("<b>Pardon our Dust!</b> It looks like you've selected an older, unsupported save format. " +
						"Please load the save using Darkc0de's Forge Tool and re-save it from within the game.");
				} else {
					showAlert("<b>Keep it Clean!</b> The selected file is not supported.");
				}
				break;
		}

		
		
	});
});

$("#input-cancel").click(() => {
	$(".page-header").text("Upload");
	$inputFile.value = "";
	$("#input-container").fadeOut(1);
	$("#details-container").fadeOut(1);

	$(".file-select").fadeIn();
});

$("#input-submit").click(() => {
	$("#input-submit").prop("disabled", true);
	$("#input-submit").text("Saving...");
	

	if (!checkValidity())
		return;

	var formData = new (<any>FormData)($inputForm);

	var url: string;
	switch (variant.Type) {
		case VariantLib.VariantType.ForgeVariant:
			url = "/upload/forge";
			break;
		case VariantLib.VariantType.GameVariant:
			url = "/upload/gametype";
			break;
	}

	$.ajax({
		url: url,
		type: "POST",
		data: formData,
		processData: false,
		contentType: false,
		success: (response) => {
			window.location.href = response;
		},
		error: (response) => {
			showAlert(response.responseText);
			$("#input-submit").prop("disabled", false);
			$("#input-submit").text("Save & Upload");
		}
	});
});

function loadForgeVariant(io: XboxInternals.IO.BaseIO) {
	$(".page-header").text("Upload - Forge Variant");
	$(".file-select").fadeOut(1);

	variant = new VariantLib.ForgeVariant(io);
	var map = Data.Maps.filter((n) => n.InternalId == (<VariantLib.ForgeVariant>variant).MapId)[0];
	
	if (map == null) {
		showAlert("The Forge variant has a invalid Map type.");
		return;
	}

	$("#input-title").val(variant.VariantName);
	$("#input-description").val(variant.VariantDescription);
	
	$("#details-title").text(map.Name);
	$("#details-description").text(map.Description);
	$("#details-image").attr("src", "/Content/Images/Maps/" + map.InternalName + ".jpg");

	$("#input-container").fadeIn();
	$("#details-container").fadeIn();
}

function loadGameVariant(io: XboxInternals.IO.BaseIO) {
	$(".page-header").text("Upload - Game Variant");
	$(".file-select").fadeOut(1);

	variant = new VariantLib.GameVariant(io);
	var gt = Data.Types.filter((n) => n.InternalId == (<VariantLib.GameVariant>variant).TypeId)[0];

	if (gt == null) {
		showAlert("The game variant has a invalid game type.");
		return;
	}

	$("#input-title").val(variant.VariantName);
	$("#input-description").val(variant.VariantDescription);

	$("#details-title").text(gt.Name);
	$("#details-description").text(gt.Description);
	$("#details-image").attr("src", "/Content/Images/Gametypes/" + gt.InternalName + ".png");

	$("#input-container").fadeIn();
	$("#details-container").fadeIn();
}

function checkValidity() {

	$inputForm.checkValidity();
	var titleValidity = (<HTMLInputElement>document.getElementById("input-title")).validity;
	var descriptionValidity = (<HTMLTextAreaElement>document.getElementById("input-description")).validity;
	var contentValidity = (<HTMLTextAreaElement>document.getElementById("input-content")).validity;

	$("#input-title-group").toggleClass("has-error", !titleValidity.valid);
	$("#input-description-group").toggleClass("has-error", !descriptionValidity.valid);
	$("#input-content-group").toggleClass("has-error", !contentValidity.valid);

	if (!titleValidity.valid || !descriptionValidity.valid || !contentValidity.valid) {
		$("#input-submit").prop("disabled", false);
		$("#input-submit").text("Save & Upload");
		return false;
	}
	return true;
}

module VariantLib {

	export interface GameMap {
		Id: number;
		Name: string;
		InternalName: string;
		InternalId: number;
		Description: string;
	}

	export interface GameType {
		Id: number;
		Name: string;
		InternalName: string;
		InternalId: number;
		Description: string;
	}

	export class Variant {

		public Type: VariantType;
		public VariantName: string;
		public VariantDescription: string;
		public VariantAuthor: string;


		constructor(io: XboxInternals.IO.BaseIO) {

			io.SetPosition(0x48);
			this.VariantName = io.ReadWString(32);

			io.SetPosition(0x68);
			this.VariantDescription = io.ReadString(128);

			io.SetPosition(0xE8);
			this.VariantAuthor = io.ReadString(16);
		}
	}

	export class ForgeVariant extends Variant {

		public MapId: number;

		constructor(io: XboxInternals.IO.BaseIO) {
			super(io);
			this.Type = VariantType.ForgeVariant;

			io.SetPosition(0x120);
			this.MapId = io.ReadInt32();
		}
	}


	export class GameVariant extends Variant {

		public TypeId: number;

		constructor(io: XboxInternals.IO.BaseIO) {
			super(io);
			this.Type = VariantType.GameVariant;

			io.SetPosition(0xF8);
			this.TypeId = io.ReadInt32();
		}
	}


	export class VariantDetector {

		public static Detect(io: XboxInternals.IO.BaseIO): VariantType {

			io.SetPosition(0x0);
			if (io.ReadString(4) !== "_blf")
				return VariantType.Invalid;

			io.SetPosition(0x138);
			switch (io.ReadString(4)) {
				case "mapv":
					return VariantType.ForgeVariant;
				case "mpvr":
					return VariantType.GameVariant;
				default:
					return VariantType.Invalid;
			}
		}
	}

	export enum VariantType {
		Invalid,
		ForgeVariant,
		GameVariant
	}
}

function detectSupport() {
	if (File && FileReader && FileList && Blob) {
		return true;
	}
	return false;
}
if (!detectSupport()) {
	showAlert("Your browser does not support the proper APIs. Please update your browser.");
}
function showAlert(message: string) {
	$(".page-header").before(
		"<div id=\"script-alert\" role=\"alert\" class=\"alert alert-danger alert-dismissable\" style=\"margin: 10px 0 5px 0;\">" +
		"<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
			message +
		"</div>");
}
function hideAlert() {
	$("#script-alert").remove();
}

declare var Data: {
	Maps: VariantLib.GameMap[];
	Types: VariantLib.GameType[];
}