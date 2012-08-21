var stakes = new Array();
var isFormValid = true;

function ContainStakesId(id, type) {
    for (var i = 0; i < stakes.length; i++) {
        if (stakes[i].Id === id && stakes[i].Type === type) {
            return true;
        }
    }
    return false;
}

function BetComplect(number) {
    if (!IsValidConditionForBet()) return false;
    var bet = $(".selected").data("value");
    var Pieces = 0;
    number = parseInt(number);

    // Straight Up	
    StraightUp(number, "SingleElement", bet); Pieces += 1;

    // Corners and 0 streets
    if (number - 3 >= 1) {
        if (number % 3 != 0) {
            StraightUp(number - 2, "Corner", 4 * bet); Pieces += 4;
        }
        if (number % 3 != 1) {
            StraightUp(number - 3, "Corner", 4 * bet); Pieces += 4;
        }
    } else {
        switch (number) {
            case 0:
                StraightUp([0, 3, 2], "Street", 3 * bet); Pieces += 3;
                StraightUp([0, 2, 1], "Street", 3 * bet); Pieces += 3;
                break;
            case 1:
                StraightUp([0, 2, 1], "Street", 3 * bet); Pieces += 3;
                break;
            case 2:
                StraightUp([0, 3, 2], "Street", 3 * bet); Pieces += 3;
                StraightUp([0, 2, 1], "Street", 3 * bet); Pieces += 3;
                break;
            case 3:
                StraightUp([0, 3, 2], "Street", 3 * bet); Pieces += 3;
                break;
        }
        StraightUp([0, 3, 2, 1], "Corner", 4 * bet); Pieces += 4;        
    }
    if (number + 3 <= 36 && number > 0) {
        if (number % 3 != 0) {
            StraightUp(number + 1, "Corner", 4 * bet); Pieces += 4;
        }
        if (number % 3 != 1) {
            StraightUp(number, "Corner", 4 * bet); Pieces += 4;
        }
    }

    // splits
    if (number % 3 > 0) {
        StraightUp([number + 1, number], "Split", 2 * bet); Pieces += 2;
    }
    if (number % 3 != 1 && number != 0) {
        StraightUp([number, number - 1], "Split", 2 * bet); Pieces += 2;
    }
    if (number - 3 > 0) {
        StraightUp([number - 3, number], "Split", 2 * bet); Pieces += 2;
    } else {
        switch (number) {
        case 0:
            StraightUp([0, 1], "Split", 2 * bet); Pieces += 2;
            StraightUp([0, 2], "Split", 2 * bet); Pieces += 2;
            StraightUp([0, 3], "Split", 2 * bet); Pieces += 2;
            break;
        case 1:
            StraightUp([0, 1], "Split", 2 * bet); Pieces += 2;
            break;
        case 2:
            StraightUp([0, 2], "Split", 2 * bet); Pieces += 2;
            break;
        case 3:
            StraightUp([0, 3], "Split", 2 * bet); Pieces += 2;
            break;
    }
    }
    if (number + 3 <= 36 && number > 0) {
        StraightUp([number, number + 3], "Split", 2 * bet); Pieces += 2;
    }

    // streets
    if (number > 0) {
        var firstInStreet = number - number % 3 + ((number % 3) != 0 ? 3 : 0);
        StraightUp([firstInStreet--, firstInStreet--, firstInStreet], "Street", 3 * bet); Pieces += 3;
    }

    // Sixline
    if (number - 3 > 0) {
        var leftNumber = number - 3;
        var firstInStreet = leftNumber - leftNumber % 3 + ((leftNumber % 3) != 0 ? 3 : 0);
        StraightUp(firstInStreet, "Sixline", 6 * bet); Pieces += 6;
    }
    if (number + 3 <= 36 && number > 0) {
        var firstInStreet = number - number % 3 + ((number % 3) != 0 ? 3 : 0);
        StraightUp(firstInStreet, "Sixline", 6 * bet); Pieces += 6;
    }

    alert(Pieces*bet);

}

function BetNeighbors(element) {
    if (!IsValidConditionForBet()) return false;
    var bet = $(".selected").data("value");
    bet = Math.round(bet / 5 - 0.49);
    var index = element.data("index");
    var betFrom = index - 2;
    var batTo = index + 2;
    for (var i = betFrom; i <= batTo; i++) {
        index = (37 + i) % 37;
        StraightUp(NUMBERS[index], "SingleElement", bet);
    }
}

function BetJeuZero() {
    if (!IsValidConditionForBet()) return false;
    var bet = $(".selected").data("value");
    bet = Math.round(bet / 4 - 0.49);
    StraightUp([12, 15], "Split", bet);
    StraightUp([32, 35], "Split", bet);
    StraightUp([0, 3], "Split", bet);
    StraightUp([26], "SingleElement", bet);
}

function BetVoisinsduZero() {
    if (!IsValidConditionForBet()) return false;
    var bet = $(".selected").data("value");
    bet = Math.round(bet / 9 - 0.49);
    StraightUp([4,7], "Split", bet);
    StraightUp([12,15], "Split", bet);
    StraightUp([18,21], "Split", bet);
    StraightUp([19,22], "Split", bet);
    StraightUp([32,35], "Split", bet);
    StraightUp([26, 25, 28, 29], "Corner", 2 * bet);
    StraightUp([0,3,2], "Street", 2*bet);
}

function BetOrphelins() {
    if (!IsValidConditionForBet()) return false;
    var bet = $(".selected").data("value");
    bet = Math.round(bet / 5 - 0.49);
    StraightUp([6, 9], "Split", bet);
    StraightUp([14, 17], "Split", bet);
    StraightUp([17, 20], "Split", bet);
    StraightUp([31, 34], "Split", bet);
    StraightUp([1], "SingleElement", bet);
}

function BetTiersduCylindre() {
    if (!IsValidConditionForBet()) return false;
    var bet = $(".selected").data("value");
    bet = Math.round(bet / 6 - 0.49);
    StraightUp([5, 8], "Split", bet);
    StraightUp([11, 10], "Split", bet);
    StraightUp([13, 16], "Split", bet);
    StraightUp([24, 23], "Split", bet);
    StraightUp([27, 30], "Split", bet);
    StraightUp([33, 36], "Split", bet);
}

function StraightUp(numbers, betType, bet) {
    if (numbers.length == undefined) {
        numbers = [numbers];
    }

    if ($(".selected").length === 0) {
        return false;
    }

    var cell = new Array();

    for (var i = 0; i < numbers.length; i++) {
        $(".round:contains('" + numbers[i] + "')").each(function (idx, val) {
            if ($(val).html() == numbers[i]) {
                cell.push($(val).parents().eq(1));
            }
        })
    }


    var element = $(".selected").clone().removeClass("selected").addClass("onboard");
    element.find("div.text").html(bet);
    var absHeight;
    var absWidth;
    var type;


    if (betType == "SingleElement" && cell.length == 1) {
        if (!ContainStakesId(numbers[0], "SingleElement")) {
            absTop = (cell[0].outerHeight() - $(".selected").outerHeight()) / 2;
            absLeft = (cell[0].outerWidth() - $(".selected").outerWidth()) / 2;
            type = "SingleElement";
        }
    } else if (betType == "Split" && cell.length == 2) {
        if (Math.abs(numbers[0] - numbers[1]) == 1 && numbers[0] != 0) {
            if (!ContainStakesId(numbers[0], "VerticalPair")) {
                absTop = cell[0].outerHeight() - $(".selected").outerHeight() / 2;
                absLeft = (cell[0].outerWidth() - $(".selected").outerWidth()) / 2;
                type = "VerticalPair";
            }
        }
        else if (numbers[0] == 0) {
            numbers[0] = numbers[1];
            if (!ContainStakesId(numbers[0], "HorizontalWithZeroPair")) {
                if (numbers[1] == 3) {
                    absTop = -$(".selected").outerHeight() / 2;
                }
                else if (numbers[1] == 2) {
                    absTop = (cell[0].outerHeight() - $(".selected").outerHeight()) / 2;
                }
                else {
                    absTop = cell[0].outerHeight() - $(".selected").outerHeight() / 2; ;
                }
                absLeft = cell[0].outerWidth() - $(".selected").outerWidth() / 2;
                type = "HorizontalWithZeroPair";
            }
        }
        else {
            if (!ContainStakesId(numbers[0], "HorizontalPair")) {
                absTop = (cell[0].outerHeight() - $(".selected").outerHeight()) / 2;
                absLeft = cell[0].outerWidth() - $(".selected").outerWidth() / 2;
                type = "HorizontalPair";
            }
        }
    } else if (betType == "Street" && cell.length == 3) {
        if (numbers[0] == 0) {
            var temp = numbers[1];
            numbers[1] = numbers[0];
            numbers[0] = temp;
            if (!ContainStakesId(numbers[0], "HorizontalWithZeroTrips")) {
                absTop = -$(".selected").outerHeight() / 2;
                absLeft = -$(".selected").outerWidth() / 2;
                type = "HorizontalWithZeroTrips";
            }
        }
        else {
            if (!ContainStakesId(numbers[0], "VerticalTrips")) {
                absTop = cell[0].outerHeight() * 2 - $(".selected").outerHeight() / 2;
                absLeft = (cell[0].outerWidth() - $(".selected").outerWidth()) / 2;
                type = "VerticalTrips";
            }
        }
    } else if (betType == "Corner"/* && cell.length == 4*/) {
        if (!ContainStakesId(numbers[0], "Quads")) {
            if (numbers[0] != 0) {
                absTop = cell[0].outerHeight() - $(".selected").outerHeight() / 2;
                absLeft = cell[0].outerWidth() - $(".selected").outerWidth() / 2;
            } else {
                absTop = cell[1].outerHeight() - $(".selected").outerHeight() / 2;
                absLeft = - $(".selected").outerWidth() / 2;
            }
            type = "Quads";
        }
    } else if (betType == "Sixline") {
        if (!ContainStakesId(numbers[0], "TwoVerticalTrips")) {
            absTop = cell[0].outerHeight()*3 - $(".selected").outerHeight() / 2;
            absLeft = cell[0].outerWidth() - $(".selected").outerWidth() / 2;
            type = "TwoVerticalTrips";
        }
    }

    if (type !== undefined) {
        element.css({
            "position": "absolute",
            "z-index": 1,
            "top": absTop,
            "left": absLeft
        });

        stakes.push({ Id: numbers[0], Price: bet, Type: type });

        if (type == "SingleElement" || type == "HorizontalPair" || type == "VerticalPair" || type == "HorizontalWithZeroPair") {
            cell[0].children(".push-item").eq(0).append(element);
        } else if (type == "HorizontalWithZeroTrips" || type == "VerticalTrips") {
            if (numbers[1] != 0) {
                cell[1].children(".push-item").eq(0).append(element);
            }
            else {
                cell[2].children(".push-item").eq(0).append(element);
            }
        } else if (type == "Quads") {
            if (numbers[0] != 0)
            {
                cell[0].children(".push-item").eq(0).append(element);
            } else {
                cell[3].children(".push-item").eq(0).append(element); 
            }
        } else if (type == "TwoVerticalTrips") {
            cell[0].children(".push-item").eq(0).append(element);
        }

        $.ajax({
            type: "POST",
            url: '/Stake/RememberCurrentState',
            data: { currentState: $("#centered-div").html() }
        });
    }
}

function IsValidConditionForBet() {
    return ($(".selected").length != 0 && currentState == 0);
}

function IsComplectBet() {
    return ($("#complect-button").hasClass("clicked") && !$("#cancel-any-button").hasClass("clicked"));
}

$(function () {
    $("td.number").click(function () {
        if (IsComplectBet()) {
            var number = $(this).find("div.round").html();
            BetComplect(number);
        }
    });

    $("#zero").click(function () {
        if (IsComplectBet()) {
            BetComplect(0);
        }
    })

    $('.bet').change(function () {
        if ($(this).val() > 0) {
            $('.selected').find('.text').text($(this).val());
            $('.selected').data("value", $(this).val());
            $(this).css("border-color", "#000000");
            isFormValid = true;
        } else {
            $(this).css("border-color", "#FF0000");
            isFormValid = false;
        }
    });

    $('div.chip').live("click", function () {
        if ($("#cancel-any-button").hasClass("clicked")) {
            if ($(this).hasClass("onboard")) {
                $(this).removeStake();
                $(this).remove();
                $.ajax({
                    type: "POST",
                    url: '/Stake/RememberCurrentState',
                    data: { currentState: $("#centered-div").html() }
                });
            }
        }
        else {
            if (!$(this).hasClass("onboard")) {
                $('.bet').val($(this).find('.text').text());
                $(this).siblings().removeClass("selected");
                if ($(this).hasClass("selected"))
                    $(this).removeClass("selected");
                else
                    $(this).addClass("selected");
            }
        }
    });

    $("#zero").click(function () {
        if (!$("#cancel-any-button").hasClass("clicked")) {
            $(this).setStake();
        }
    });

    $(".roulette-board td").click(function () {
        if (!$("#cancel-any-button").hasClass("clicked")) {
            $(this).setStake();
        }
    });


    $.fn.setStake = function () {

        if (!IsValidConditionForBet() || $("#complect-button").hasClass("clicked")) {
            return false;
        }

        var element = $(".selected").clone().removeClass("selected").addClass("onboard");
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

                    absTop = $(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight() / 2;
                    absLeft = ($(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth()) / 2;
                    type = "VerticalTrips";

                }
            }
        }
        else if ($(".highlighted").length == 4) {
            if (!ContainStakesId(id, "Quads")) {
                if ($(".highlighted").eq(0).attr("id") != "zero") {
                    absTop = $(".highlighted").eq(0).outerHeight() - $(".selected").outerHeight() / 2;
                    absLeft = $(".highlighted").eq(0).outerWidth() - $(".selected").outerWidth() / 2;
                } else {
                    absTop = $(".highlighted").eq(1).outerHeight() - $(".selected").outerHeight() / 2;
                    absLeft = - $(".selected").outerWidth() / 2;
                }
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
            if ($(this).hasClass("number") && $(".highlighted").length != 3 && $(".highlighted").length != 6 && $(".highlighted").length != 4) {
                $(".highlighted").eq(0).children(".push-item").eq(0).append(element);
            }
            else if ($(this).hasClass("number") && $(".highlighted").length == 3) {
                if ($(".highlighted").eq(0).attr("id") != "zero") {
                    $(".highlighted").eq(2).children(".push-item").eq(0).append(element);
                }
                else {
                    $(".highlighted").eq(2).children(".push-item").eq(0).append(element);
                }
            }
            else if ($(this).hasClass("number") && $(".highlighted").length == 4) {
                if ($(".highlighted").eq(0).attr("id") != "zero") {
                    $(".highlighted").eq(0).children(".push-item").eq(0).append(element);
                }
                else {
                    $(".highlighted").eq(3).children(".push-item").eq(0).append(element);
                }
            }
            else if ($(this).hasClass("number") && $(".highlighted").length == 6) {
                $(".highlighted").eq(4).children(".push-item").eq(0).append(element);
            }
            else {
                $(this).children(".push-item").eq(0).append(element);
            }

            $.ajax({
                type: "POST",
                url: '/Stake/RememberCurrentState',
                data: { currentState: $("#centered-div").html() }
            });
        }
    }

    $.fn.removeStake = function () {

        var id = parseInt($(".highlighted").eq(0).find(".round").html());
        var type;

        if ($(".highlighted").length == 1) {
            if (ContainStakesId(id, "SingleElement")) {
                type = "SingleElement";
            }
        }
        else if ($(".highlighted").length == 2) {

            if (Math.abs($(".highlighted").eq(0).find(".round").html() - $(".highlighted").eq(1).find(".round").html()) == 1 && $(".highlighted").eq(0).attr("id") != "zero") {
                if (ContainStakesId(id, "VerticalPair")) {
                    type = "VerticalPair";

                }
            }
            else if ($(".highlighted").eq(0).attr("id") == "zero") {

                id = parseInt($(".highlighted").eq(1).find(".round").html());
                if (ContainStakesId(id, "HorizontalWithZeroPair")) {
                    type = "HorizontalWithZeroPair";
                }
            }
            else {
                if (ContainStakesId(id, "HorizontalPair")) {
                    type = "HorizontalPair";

                }
            }
        }
        else if ($(".highlighted").length == 3) {
            if ($(".highlighted").eq(0).attr("id") == "zero") {
                id = parseInt($(".highlighted").eq(1).find(".round").html());
                if (ContainStakesId(id, "HorizontalWithZeroTrips")) {
                    type = "HorizontalWithZeroTrips";

                }

            }
            else {
                if (ContainStakesId(id, "VerticalTrips")) {
                    type = "VerticalTrips";

                }
            }
        }
        else if ($(".highlighted").length == 4) {
            if (ContainStakesId(id, "Quads")) {
                type = "Quads";

            }
        }
        else if ($(".highlighted").length == 6) {
            if (ContainStakesId(id, "TwoVerticalTrips")) {
                type = "TwoVerticalTrips";

            }
        }
        else {
            if ($(".highlighted").length == 12) {
                if (Math.abs($(".highlighted").eq(0).find(".round").html() - $(".highlighted").eq(4).find(".round").html()) == 12) {
                    if (ContainStakesId(id, "HorizontalLine")) {
                        type = "HorizontalLine";
                    }
                }
                else {
                    if (ContainStakesId(id, "TwelveElements")) {
                        type = "TwelveElements";

                    }
                }
            }
            else if ($(".highlighted").length == 18) {
                if (!$(".highlighted.black").length || !$(".highlighted.red").length) {
                    if (ContainStakesId(id, "BlackOrRed")) {
                        type = "BlackOrRed";

                    }
                }
                else if (Math.abs($(".highlighted").eq(0).find(".round").html() - $(".highlighted").eq(1).find(".round").html()) == 3) {
                    if (ContainStakesId(id, "EighteenElements")) {
                        type = "EighteenElements";

                    }
                }
                else {
                    if (ContainStakesId(id, "EvenOrOdd")) {
                        type = "EvenOrOdd";

                    }
                }
            }
        }

        if (type !== undefined) {

            var index = -1;
            for (var i = 0; i < stakes.length; i++) {
                if (stakes[i].Id == id && stakes[i].Type == type) {
                    index = i;
                }
            }

            stakes.splice(index, 1);
        }

    }


});