var currentState = 0;

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
                    //alert((Date.parse(state.CurrentTime) - Date.parse(state.StartTime)) + " " + state.RoundeTime);
                    seconds = Math.round((state.RoundeTime + requsetTime) / 1000);
                    currentState = state.State;
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
        }
    });
}