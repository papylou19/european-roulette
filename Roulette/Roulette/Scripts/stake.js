var stakes = new Array();

function ContainStakesId(id, type) {
    for (var i = 0; i < stakes.length; i++) {
        if (stakes[i].Id === id && stakes[i].Type === type) {
            return true;
        }
    }
    return false;
}

$(function () {
    $('div.chip').click(function () {
        $(this).siblings().removeClass("selected");
        if ($(this).hasClass("selected"))
            $(this).removeClass("selected");
        else
            $(this).addClass("selected");
    });

    $("#zero").click(function () {
        $(this).setStake();
    });

    $(".roulette-board td").click(function () {
        $(this).setStake();
    });

    $.fn.setStake = function () {
        if ($(".selected").length === 0) {
            return false;
        }

        var element = $(".selected").clone().removeClass("selected");
        var absHeight;
        var absWidth;

        var id = parseInt($(".highlighted").eq(0).find(".round").html());
        var type;

        if ($(".highlighted").length == 1) {
            if (!ContainStakesId(id, "SingleElement")) {

                absTop = ($(this).outerHeight() - $(".selected").outerHeight()) / 2;
                absLeft = ($(this).outerWidth() - $(".selected").outerWidth()) / 2;
                type = "SingleElement";
            }
        }
        else if ($(".highlighted").length == 2) {

            if (Math.abs($(".highlighted").eq(0).find(".round").html() - $(".highlighted").eq(1).find(".round").html()) == 1 && $(".highlighted").eq(0).attr("id") != "zero") {
                if (!ContainStakesId(id, "VerticalPair")) {

                    absTop = $(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight() / 2;
                    absLeft = ($(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth()) / 2;
                    type = "VerticalPair";

                }
            }
            else if ($(".highlighted").eq(0).attr("id") == "zero") {

                id = parseInt($(".highlighted").eq(1).find(".round").html());
                if (!ContainStakesId(id, "HorizontalWithZeroPair")) {
                    if (id == 3) {
                        absTop = -$(".selected").outerHeight() / 2;
                    }
                    else if (id == 2) {
                        absTop = ($(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight()) / 2;
                    }
                    else {
                        absTop = $(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight() / 2; ;
                    }
                    absLeft = $(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth() / 2;
                    type = "HorizontalWithZeroPair";

                }
            }
            else {
                if (!ContainStakesId(id, "HorizontalPair")) {

                    absTop = ($(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight()) / 2;
                    absLeft = $(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth() / 2;
                    type = "HorizontalPair";

                }
            }
        }
        else if ($(".highlighted").length == 3) {
            if ($(".highlighted").eq(0).attr("id") == "zero") {
                id = parseInt($(".highlighted").eq(1).find(".round").html());
                if (!ContainStakesId(id, "HorizontalWithZeroTrips")) {

                    absTop = -$(".selected").outerHeight() / 2;
                    absLeft = -$(".selected").outerWidth() / 2;
                    type = "HorizontalWithZeroTrips";

                }

            }
            else {
                if (!ContainStakesId(id, "VerticalTrips")) {

                    absTop = $(".highlighted").eq(0).outerHeight() * 2 - $(".selected").outerHeight() / 2;
                    absLeft = ($(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth()) / 2;
                    type = "VerticalTrips";

                }
            }
        }
        else if ($(".highlighted").length == 4) {
            if (!ContainStakesId(id, "Quads")) {

                absTop = $(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight() / 2;
                absLeft = $(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth() / 2;
                type = "Quads";

            }
        }
        else if ($(".highlighted").length == 6) {
            if (!ContainStakesId(id, "TwoVerticalTrips")) {

                absTop = $(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight() / 2;
                absLeft = $(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth() / 2;
                type = "TwoVerticalTrips";

            }
        }
        else {

            absLeft = ($(this).outerWidth() - $(".selected").outerWidth()) / 2;
            absTop = ($(this).outerHeight() - $(".selected").outerHeight()) / 2;

            if ($(".highlighted").length == 12) {
                if (Math.abs($(".highlighted").eq(0).find(".round").html() - $(".highlighted").eq(4).find(".round").html()) == 12) {
                    if (!ContainStakesId(id, "HorizontalLine")) {
                        type = "HorizontalLine";
                    }
                }
                else {
                    if (!ContainStakesId(id, "TwelveElements")) {
                        type = "TwelveElements";

                    }
                }
            }
            else if ($(".highlighted").length == 18) {
                if (!$(".highlighted.black").length || !$(".highlighted.red").length) {
                    if (!ContainStakesId(id, "BlackOrRed")) {
                        type = "BlackOrRed";

                    }
                }
                else if (Math.abs($(".highlighted").eq(0).find(".round").html() - $(".highlighted").eq(1).find(".round").html()) == 3) {
                    if (!ContainStakesId(id, "EighteenElements")) {
                        type = "EighteenElements";

                    }
                }
                else {
                    if (!ContainStakesId(id, "EvenOrOdd")) {
                        type = "EvenOrOdd";

                    }
                }
            }
        }

        if (type !== undefined) {
            element.css({
                "position": "absolute",
                "z-index": 1,
                "top": absTop,
                "left": absLeft
            });

            stakes.push({ Id: id, Price: $(".selected").data("value"), Type: type });

            if ($(this).hasClass("number") && $(".highlighted").length != 3 && $(".highlighted").length != 6) {
                $(".highlighted").eq(0).append(element);
            }
            else if ($(this).hasClass("number") && $(".highlighted").length == 3) {
                if ($(".highlighted").eq(0).attr("id") != "zero") {
                    $(".highlighted").eq(1).append(element);
                }
                else {
                    $(".highlighted").eq(2).append(element);
                }
            }
            else if ($(this).hasClass("number") && $(".highlighted").length == 6) {
                $(".highlighted").eq(4).append(element);
            }
            else {
                $(this).append(element);
            }

            $.ajax({
                type: "POST",
                url: '/Stake/RememberCurrentState',
                data: {currentState : $("#centered-div").html()}
            });


        }
    }


});