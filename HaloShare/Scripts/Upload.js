var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
$(".file-select").on('dragenter', function (e) {
    $(".file-select").addClass("active");
});
$(".file-select").on('dragleave', function (e) {
    $(".file-select").removeClass("active");
});
$(".file-select").on('mouseleave', function () {
    $(".file-select").removeClass("active");
});
var variant;
var $inputForm = document.getElementById("input-form");
var $inputFile = document.getElementById("input-file");
$("#input-file").change(function () {
    hideAlert();
    var file = $inputFile.files[0];
    XboxInternals.IO.FileIO.LoadFromFile(file, function (io) {
        // Make sure we are using the correct Endian type.
        io.SetEndian(1 /* LittleEndian */);
        var detected = VariantLib.VariantDetector.Detect(io);
        switch (detected) {
            case 1 /* ForgeVariant */:
                loadForgeVariant(io);
                break;
            case 2 /* GameVariant */:
                loadGameVariant(io);
                break;
            default:
                $inputFile.value = "";
                if (file.name.indexOf(".frg") != -1) {
                    showAlert("<b>Pardon our Dust!</b> It looks like you've selected an older, unsupported save format. " + "Please load the save using Darkc0de's Forge Tool and re-save it from within the game.");
                }
                else {
                    showAlert("<b>Keep it Clean!</b> The selected file is not supported.");
                }
                break;
        }
    });
});
$("#input-cancel").click(function () {
    $(".page-header").text("Upload");
    $inputFile.value = "";
    $("#input-container").fadeOut(1);
    $("#details-container").fadeOut(1);
    $(".file-select").fadeIn();
});
$("#input-submit").click(function () {
    $("#input-submit").prop("disabled", true);
    $("#input-submit").text("Saving...");
    if (!checkValidity())
        return;
    var formData = new FormData($inputForm);
    var url;
    switch (variant.Type) {
        case 1 /* ForgeVariant */:
            url = "/upload/forge";
            break;
        case 2 /* GameVariant */:
            url = "/upload/gametype";
            break;
    }
    $.ajax({
        url: url,
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            window.location.href = response;
        },
        error: function (response) {
            showAlert(response.responseText);
            $("#input-submit").prop("disabled", false);
            $("#input-submit").text("Save & Upload");
        }
    });
});
function loadForgeVariant(io) {
    $(".page-header").text("Upload - Forge Variant");
    $(".file-select").fadeOut(1);
    variant = new VariantLib.ForgeVariant(io);
    var map = Data.Maps.filter(function (n) { return n.InternalId == variant.MapId; })[0];
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
function loadGameVariant(io) {
    $(".page-header").text("Upload - Game Variant");
    $(".file-select").fadeOut(1);
    variant = new VariantLib.GameVariant(io);
    var gt = Data.Types.filter(function (n) { return n.InternalId == variant.TypeId; })[0];
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
    var titleValidity = document.getElementById("input-title").validity;
    var descriptionValidity = document.getElementById("input-description").validity;
    var contentValidity = document.getElementById("input-content").validity;
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
var VariantLib;
(function (VariantLib) {
    var Variant = (function () {
        function Variant(io) {
            io.SetPosition(0x48);
            this.VariantName = io.ReadWString(32);
            io.SetPosition(0x68);
            this.VariantDescription = io.ReadString(128);
            io.SetPosition(0xE8);
            this.VariantAuthor = io.ReadString(16);
        }
        return Variant;
    })();
    VariantLib.Variant = Variant;
    var ForgeVariant = (function (_super) {
        __extends(ForgeVariant, _super);
        function ForgeVariant(io) {
            _super.call(this, io);
            this.Type = 1 /* ForgeVariant */;
            io.SetPosition(0x120);
            this.MapId = io.ReadInt32();
        }
        return ForgeVariant;
    })(Variant);
    VariantLib.ForgeVariant = ForgeVariant;
    var GameVariant = (function (_super) {
        __extends(GameVariant, _super);
        function GameVariant(io) {
            _super.call(this, io);
            this.Type = 2 /* GameVariant */;
            io.SetPosition(0xF8);
            this.TypeId = io.ReadInt32();
        }
        return GameVariant;
    })(Variant);
    VariantLib.GameVariant = GameVariant;
    var VariantDetector = (function () {
        function VariantDetector() {
        }
        VariantDetector.Detect = function (io) {
            io.SetPosition(0x0);
            if (io.ReadString(4) !== "_blf")
                return 0 /* Invalid */;
            io.SetPosition(0x138);
            switch (io.ReadString(4)) {
                case "mapv":
                    return 1 /* ForgeVariant */;
                case "mpvr":
                    return 2 /* GameVariant */;
                default:
                    return 0 /* Invalid */;
            }
        };
        return VariantDetector;
    })();
    VariantLib.VariantDetector = VariantDetector;
    (function (VariantType) {
        VariantType[VariantType["Invalid"] = 0] = "Invalid";
        VariantType[VariantType["ForgeVariant"] = 1] = "ForgeVariant";
        VariantType[VariantType["GameVariant"] = 2] = "GameVariant";
    })(VariantLib.VariantType || (VariantLib.VariantType = {}));
    var VariantType = VariantLib.VariantType;
})(VariantLib || (VariantLib = {}));
function detectSupport() {
    if (File && FileReader && FileList && Blob) {
        return true;
    }
    return false;
}
if (!detectSupport()) {
    showAlert("Your browser does not support the proper APIs. Please update your browser.");
}
function showAlert(message) {
    $(".page-header").before("<div id=\"script-alert\" role=\"alert\" class=\"alert alert-danger alert-dismissable\" style=\"margin: 10px 0 5px 0;\">" + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" + message + "</div>");
}
function hideAlert() {
    $("#script-alert").remove();
}
//# sourceMappingURL=Upload.js.map