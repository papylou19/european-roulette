var currentState = 0;
var currentRoundNumber;

function setProgressBar() {
    setTimeout(function () {
        var requestStartAt = new Date();
        $.ajax({
            type: "POST",
            url: '/Home/GetCurrentState',
            dataType: 'json',
            success: function (state) {
                var seconds;
                if (state == null) {
                    seconds = 0;
                }
                else {
                    var requestEndAt = new Date();
                    var requsetTime = requestEndAt - requestStartAt;
                    seconds = Math.round((state.RoundeTime + requsetTime) / 1000);
                    currentState = state.State;
                    if (currentState == 1 && !$("#main").hasClass("disabled")) {
                        $("#admin-board").parents("#main").addClass("disabled");
                    }
                    else if (currentState == 0 && $("#main").hasClass("disabled")) {
                        $("#admin-board").parents("#main").removeClass("disabled");
                    }
                }
                $(".progress-bar progress").val(seconds);
            }
        });
        setProgressBar();
    }, 1000);
}

function getRoundNumber() {
    $.ajax({
        type: "POST",
        url: '/Home/GetCurrentRound',
        success: function (round) {
            $("#round-number").html(round);

            if (currentRoundNumber != round) {

                currentRoundNumber = round;
                //checkSum.val("0");
                $("#cancel-button").click();
            }
        }
    });
}