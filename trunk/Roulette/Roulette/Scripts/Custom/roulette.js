var rouletteAngularSpeed = -0.4;
var delta = 0.015;
var s;
var c;
var ballAngularSpeed;

var rouletteAngle = 0;
var ballAngle;
var ballRotateStarted = false;
var winnerHighlighted = false;
var number = 0;
var cycleNumber = 9;

var bouncingRadius = 0;
var isRadiusGrowing = undefined;
var bouncingSteps = 30;
var bouncingAmplitude = 30; 
var bouncingCurrentStep = 0;
var bouncingAcceleration;
var stack288 = new Array();
var bollStopSteps = 0;


var switchToNextStep = true;
var bounsingType = false;
var bounsingState = 0;


var CENTERX = 218; //center X of roulette roulette-board R
var CENTERY = 218; //center Y of roulette
var ballHeight = 12;
var ballWidth = 12;
var initialR = 189;
var r = initialR;
var NUMBERS = [26, 3, 35, 12, 28, 7, 29, 18, 22, 9, 31, 14, 20, 1, 33, 16, 24, 5, 10, 23, 8, 30, 11, 36, 13, 27, 6, 34, 17, 25, 2, 21, 4, 19, 15, 32, 0];



var wheel;
var board;
var ball;
var canvas;
var context;
var wheelTopLeft = 64;
var wheelSize = 309;

Array.prototype.findIndex = function(value){
    var ctr = "";
    for (var i=0; i < this.length; i++) {
    if (this[i] === value) {
        return i;
    }
    }
    return ctr;
};

function CalculateRotateNumber(rouletteAngularSpeed, s, delta) {
    //Assuming that n=c/delta, we must return the solution of s = n*c - delta * (n * (n+1) /2)
    return (delta + Math.sqrt(delta * delta + 8 * delta * s)) / 2 - delta;
}

function CalculatePath() {
    return 360 * cycleNumber + 99 + NUMBERS.findIndex(number) * 360/37;
}

function getRadian(degree) {
    return degree * Math.PI / 180;
}

function Sin(degree) {
    return Math.sin(getRadian(degree));
}

function Cos(degree) {
    return Math.cos(getRadian(degree));
}

function Start() {
    $.ajax({
        type: "POST",
        url: '/Stake/NextNumber',
        datatype: 'json',
        success: function (json) {
            if (json.nextNumber === null) Start();
            else {
                number = json.nextNumber;
                s = CalculatePath();
                r = initialR;
                c = CalculateRotateNumber(rouletteAngularSpeed, s, delta);
                ballAngularSpeed = c + rouletteAngularSpeed;
                ballAngle = rouletteAngle;
                winnerHighlighted = false;
                ballRotateStarted = true;

                bollStopSteps = 0;
                bounsingState = 0;

                bounsingType = Math.random() > 0.3;
            }
        }
    });
}


function rotateWheel(d) {
    setTimeout(function () {

        context.clearRect(0, 0, canvas.width, canvas.height);
        context.drawImage(board, 0, 0);
        context.translate(wheelTopLeft, wheelTopLeft);
        context.translate(wheelSize / 2, wheelSize/2);
        context.rotate(getRadian(d));
        context.drawImage(wheel, -wheelSize / 2, -wheelSize/2);
        context.rotate(-getRadian(d));
        context.translate(-wheelSize / 2, -wheelSize / 2); 
        context.translate(-wheelTopLeft, -wheelTopLeft);

        if (ballAngularSpeed == rouletteAngularSpeed) {
            ballRotateStarted = false;
            if (!winnerHighlighted) {
                $("#board .round").each(function () {
                    if ($(this).html() === number.toString()) {

                        var td;

                        if (number == 0) {
                            td = $(this).parents("#zero").clone().removeClass("reverse");
                            $(this).parents("#zero").addClass("highlighted");
                        }
                        else {
                            td = $(this).parents("td");
                            td.addClass("highlighted");
                        }

                        var classes = td.attr("class");

                        $("#history").animate({ "margin-top": 150 }, 500, function () {
                            $(".right").prepend($('<div class="selected-number" style="display:none">' + td.html() + '</div>').addClass(classes));
                            $(".right .selected-number").fadeIn(1000);
                        });
                    }
                    winnerHighlighted = true;
                });
            }

            if (currentState == 1) {
                context.drawImage(ball, CENTERX + (r * Cos(ballAngle)) - ballWidth / 2, CENTERY + (r * Sin(ballAngle)) - ballHeight / 2, ballWidth, ballHeight);
                //$("#ball").css("top", CENTERY + (r * Sin(ballAngle)) - $("#ball").height() / 2).css("left", CENTERX + (r * Cos(ballAngle)) - $("#ball").width() / 2);
                ballAngle = (ballAngle - rouletteAngularSpeed) % 360;
            }
        }

        if (ballRotateStarted) {
            if (ballAngularSpeed > rouletteAngularSpeed) {

                ballAngle = (ballAngle - ballAngularSpeed) % 360;
                if (c - delta > 0) c -= delta;
                else c = 0;
                ballAngularSpeed = c + rouletteAngularSpeed;

                //ANOTHER HARD CODE for boll bounsingmnm,

                if ((r >= 176 && r < 179) && bounsingState == 0 && !bounsingType && switchToNextStep) {
                    isRadiusGrowing = false;

                    bouncingAmplitude = 25 + Math.floor((Math.random() * 20) + 1);
                    bouncingSteps = 30 + Math.floor((Math.random() * 10) + 1);

                    bouncingAcceleration = bouncingAmplitude / ((1 + bouncingSteps) * bouncingSteps / 2);
                    bouncingRadius = -bouncingSteps * bouncingAcceleration;

                    switchToNextStep == false;
                }

                // ANOTHER BOUNSING
                if ((r >= 120 && r < 121.5) && bounsingState == 0 && bounsingType && switchToNextStep) {
                    isRadiusGrowing = false;

                    bouncingAmplitude = 20;
                    bouncingSteps = 40;

                    bouncingAcceleration = bouncingAmplitude / ((1 + bouncingSteps) * bouncingSteps / 2);
                    bouncingRadius = bouncingSteps * bouncingAcceleration;

                    bouncingCurrentStep = 0;

                    switchToNextStep = false;
                }

                if (bounsingState == 1 && switchToNextStep) {
                    isRadiusGrowing = false;

                    bouncingAmplitude = bounsingType ? 20 : 6;
                    bouncingSteps = Math.floor((c / 0.02) / 2) - (bounsingType ? 16 : 0);

                    bouncingAcceleration = bouncingAmplitude / ((1 + bouncingSteps) * bouncingSteps / 2);
                    bouncingRadius = bouncingSteps * bouncingAcceleration * (bounsingType ? -1 : 1);

                    bouncingCurrentStep = 0;

                    switchToNextStep = false;
                }

                if (bounsingState == 2 && switchToNextStep && bounsingType) {
                    isRadiusGrowing = false;

                    bouncingAmplitude = 4;
                    bouncingSteps = 16;

                    bouncingAcceleration = bouncingAmplitude / ((1 + bouncingSteps) * bouncingSteps / 2);
                    bouncingRadius = bouncingSteps * bouncingAcceleration * (bounsingType ? 1 : -1);

                    bouncingCurrentStep = 0;

                    switchToNextStep = false;
                }





                bounseBall();

                //HARD CODE
                if (c < 3.5)
                    if (bollStopSteps < (initialR - 105) / 1.5) {
                        bollStopSteps += 1;
                        r -= 1.5;
                    }
                context.drawImage(ball, CENTERX + (r * Cos(ballAngle)) - ballWidth / 2, CENTERY + (r * Sin(ballAngle)) - ballHeight / 2, ballWidth, ballHeight);
            }
        }

        rouletteAngle = (d - rouletteAngularSpeed) % 360;
        rotateWheel(rouletteAngle);

    }, 8);
}

function bounseBall() {
    if (isRadiusGrowing == false && bouncingCurrentStep < bouncingSteps) {
        bouncingRadius += bouncingAcceleration;
        r += bouncingRadius;
        bouncingCurrentStep += 1;

        stack288.push(bouncingRadius);

        if (bouncingCurrentStep == bouncingSteps) {
            isRadiusGrowing = true;
            bouncingRadius = 0;
            bouncingCurrentStep = 0;
        }
    }
    if (isRadiusGrowing == true && bouncingCurrentStep < bouncingSteps) {
        r -= stack288.pop();
        bouncingCurrentStep += 1;
        if (bouncingCurrentStep == bouncingSteps) {
            isRadiusGrowing = undefined;
            bouncingRadius = 0;
            bouncingCurrentStep = 0;

            switchToNextStep = true;
            bounsingState += 1;
        }
    }
}

$(function () {
    canvas = document.getElementById("roulette");
    context = canvas.getContext("2d");

    board = new Image();

    board.onload = function () {
        context.drawImage(board, 0, 0);
        wheel = new Image();

        wheel.onload = function () {
            context.drawImage(wheel, wheelTopLeft, wheelTopLeft);
            ball = new Image();
            ball.onload = function () {
                rotateWheel(0);
            }
            ball.src = "/img/ball.png";
        };

        wheel.src = "/img/roulette-wheel.png";
    };

    board.src = "/img/roulette-board.png";

});
