var Rating = (function () {
    function Rating(id, value, voteAction, itemId) {
        var _this = this;
        this.rating = $(id);
        this.default = value;
        this.action = voteAction;
        this.itemId = itemId;
        $('.fa', this.rating).on('mouseover', function (e) {
            _this.setRatingStar(parseInt($(e.currentTarget).attr('data-rating')));
        });
        $('.fa', this.rating).on('click', function (e) {
            _this.processRating(parseInt($(e.currentTarget).attr('data-rating')));
        });
        $(this.rating).on('mouseleave', function () {
            _this.setRatingStar(_this.default);
        });
        this.setRatingStar(this.default);
    }
    Rating.prototype.processRating = function (star) {
        var _this = this;
        $.ajax({
            url: this.action,
            method: "POST",
            data: {
                id: this.itemId,
                rating: star
            },
            success: function (data) {
                _this.default = data.Rating;
                _this.setRatingStar(data.Rating);
                $("#rating-count").text(data.Count);
            }
        });
        return;
        var request = new XMLHttpRequest();
        request.open("POST", this.action);
        request.onload = function () {
            var response = JSON.parse(request.responseText);
            _this.default = response.Rating;
            _this.setRatingStar(response.Rating);
            $("#rating-count").text(response.Count);
        };
        request.send({
            id: this.itemId,
            rating: this.rating
        });
    };
    Rating.prototype.setRatingStar = function (stars) {
        $(".fa", this.rating).each(function (i, e) {
            if (stars >= parseInt($(e).data('rating'))) {
                return $(e).removeClass('fa-star-o').addClass('fa-star');
            }
            else {
                return $(e).removeClass('fa-star').addClass('fa-star-o');
            }
        });
    };
    return Rating;
})();
function dwnlNag(type) {
    localStorage.setItem("dwnlMethod", type);
    $("#download-link").click();
}
$("#download-link").on('click', function () {
    var dwnlMethod = localStorage.getItem("dwnlMethod");
    if (dwnlMethod == undefined) {
        event.preventDefault();
        $("#modal-dwnlnag").modal({
            keyboard: true
        });
    }
    else if (dwnlMethod == "blam") {
        $("#download-link").attr("href", $("#download-link").data('blam'));
    }
    $("#download-link").addClass("disabled");
});
function Report(url) {
    var description = prompt("Please provide short a report description:");
    console.log("Reporting:", url, description);
    $.ajax({
        url: '/report/submit',
        method: "POST",
        data: {
            url: url,
            description: description
        },
        success: function (data) {
            if (data.Code == 0) {
                alert("Succefully Reported!");
            }
            else if (data.Code == 1) {
                alert("Something went wrong while reporting, try again.");
            }
        }
    });
    return;
    var request = new XMLHttpRequest();
    request.open("POST", "/report/submit");
    request.onload = function () {
        var response = JSON.parse(request.responseText);
        if (response.Code == 0) {
            alert("Succefully Reported!");
        }
        else if (response.Code == 1) {
            alert("Something went wrong while reporting, try again.");
        }
    };
    request.send({
        url: url,
        description: description
    });
}
{
    $("#theme-selector").on('change', function () {
        console.log("ey");
        $.cookie('Style', $("#theme-selector").val(), {
            expires: 20 * 365,
            path: '/'
        });
        location.reload();
    });
}
//# sourceMappingURL=Scripts.js.map