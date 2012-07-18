var currentState = 0;

function setProgressBar() {
    setTimeout(function () {
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
                    seconds = Math.round((new Date() - Date.parse(state.StartTime)) / 1000);
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