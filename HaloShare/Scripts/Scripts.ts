
class Rating {

	private rating: JQuery;
	private default: number;
	private action: string;
	private itemId: any;

	constructor(id: string, value: number, voteAction: string, itemId: any) {
		this.rating = $(id);
		this.default = value;
		this.action = voteAction;
		this.itemId = itemId;

		$('.fa', this.rating).on('mouseover', (e) => {
			this.setRatingStar(parseInt($(e.currentTarget).attr('data-rating')));
		});

		$('.fa', this.rating).on('click',(e) => {
			this.processRating(parseInt($(e.currentTarget).attr('data-rating')));
		});

		$(this.rating).on('mouseleave',() => {
			this.setRatingStar(this.default);
		});

		this.setRatingStar(this.default);
	}

	private processRating(star: number) {
		$.ajax({
			url: this.action,
			method: "POST",
			data: {
				id: this.itemId,
				rating: star
			},
			success: (data) => {

				this.default = data.Rating;
				this.setRatingStar(data.Rating);

				$("#rating-count").text(data.Count);
			}
		});
		return;
		var request = new XMLHttpRequest();
		request.open("POST", this.action);
		request.onload = () => {
			var response = JSON.parse(request.responseText);

			this.default = response.Rating;
			this.setRatingStar(response.Rating);

			$("#rating-count").text(response.Count);
		}
		request.send({
			id: this.itemId,
			rating: this.rating
		});
	}

	private setRatingStar(stars: number) {
		$(".fa", this.rating).each((i, e) => {
			if (stars >= parseInt($(e).data('rating'))) {
				return $(e).removeClass('fa-star-o').addClass('fa-star');
			} else {
				return $(e).removeClass('fa-star').addClass('fa-star-o');
			}
		});
	}
}

function dwnlNag(type: string) {
    localStorage.setItem("dwnlMethod", type);
    $("#download-link").click();
}

$("#download-link").on('click', () => {
    var dwnlMethod = localStorage.getItem("dwnlMethod");

    if (dwnlMethod == undefined) {
        event.preventDefault();

        $("#modal-dwnlnag").modal({
            keyboard: true
        });
    } else {
        $("#download-link").addClass("disabled");
    }/* else if (dwnlMethod == "blam") {
        $("#download-link").attr("href", $("#download-link").data('blam'));       
    }*/

});

function Report(url: string) {
	var description = prompt("Please provide short a report description:");
	console.log("Reporting:", url, description);

	$.ajax({
		url: '/report/submit',
		method: "POST",
		data: {
			url: url,
			description: description
		},
		success: (data) => {
			if (data.Code == 0) {
				alert("Succefully Reported!");
			} else if (data.Code == 1) {
				alert("Something went wrong while reporting, try again.");
			}
		}
	});
	return;
	var request = new XMLHttpRequest();
	request.open("POST", "/report/submit");
	request.onload = () => {
		var response = JSON.parse(request.responseText);

		if (response.Code == 0) {
			alert("Succefully Reported!");
		} else if (response.Code == 1) {
			alert("Something went wrong while reporting, try again.");
		}
	}
	request.send({
		url: url,
		description: description
	});
}

// Theme Selector
{
	$("#theme-selector").on('change', () => {
		console.log("ey");
		$.cookie('Style', $("#theme-selector").val(), {
			expires: 20 * 365,
			path: '/'
		});
		location.reload();
	});
}