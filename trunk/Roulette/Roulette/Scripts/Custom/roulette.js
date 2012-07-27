﻿var rouletteAngularSpeed = -0.35;
var delta = 0.03;
var s;
var c;
var ballAngularSpeed;

var rouletteAngle = 0;
var ballAngle;
var ballRotateStarted = false;
var winnerHighlighted = false;
var number = 0;
var cycleNumber = 10;

var CENTERX = 190; //center X of roulette
var CENTERY = 190; //center Y of roulette
var ballheight;
var ballWidth;
var r = 140;
var NUMBERS = [26, 3, 35, 12, 28, 7, 29, 18, 22, 9, 31, 14, 20, 1, 33, 16, 24, 5, 10, 23, 8, 30, 11, 36, 13, 27, 6, 34, 17, 25, 2, 21, 4, 19, 15, 32, 0];
var TRANSFORM;

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

function getTransformProperty(element) {
    var properties = [
                        'transform',
                        'WebkitTransform',
                        'msTransform',
                        'MozTransform',
                        'OTransform'
                     ];
    var p;
    while (p = properties.shift()) {
        if (typeof element.style[p] != 'undefined') {
            return p;
        }
    }
    return false;
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
                r = 140;
                c = CalculateRotateNumber(rouletteAngularSpeed, s, delta);
                ballAngularSpeed = c + rouletteAngularSpeed;
                ballAngle = rouletteAngle;
                $("#ball").show();
                winnerHighlighted = false;
                ballRotateStarted = true;
            }
        }
    });
}

function rotateWheel(d) {
    setTimeout(function () {
        $('.roulette-wheel')[0].style[TRANSFORM] = 'rotate(' + (d % 360) + 'deg)';

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
            if ($("#ball").is(":visible")) {
                $("#ball").css("top", CENTERY + (r * Sin(ballAngle)) - $("#ball").height() / 2).css("left", CENTERX + (r * Cos(ballAngle)) - $("#ball").width() / 2);
                ballAngle = (ballAngle - rouletteAngularSpeed) % 360;
            }
        }

        if (ballRotateStarted) {
            if (ballAngularSpeed > rouletteAngularSpeed) {

                ballAngle = (ballAngle - ballAngularSpeed) % 360;
                if (c - delta > 0) c -= delta;
                else c = 0;
                ballAngularSpeed = c + rouletteAngularSpeed;

                //HARD CODE
                if (c < 3.5)
                    if (r > 80) r -= 1.5;

                $("#ball").css({
                    top: CENTERY + (r * Sin(ballAngle)) - ballHeight / 2,
                    left: CENTERX + (r * Cos(ballAngle)) - ballWidth / 2
                });
            }
        }

        rouletteAngle = (d - rouletteAngularSpeed) % 360;
        rotateWheel(rouletteAngle);

    }, 8);
}

$(function () {
    ballHeight = $("#ball").height();
    ballWidth = $("#ball").width();
    TRANSFORM = getTransformProperty($('.roulette-wheel')[0]);
    if (TRANSFORM) {
        rotateWheel(0);
    }
});
