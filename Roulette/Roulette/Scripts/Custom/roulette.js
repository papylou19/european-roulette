var ROULETTE_SPEED = 12.5;
var CENTERX = 190;
var CENTERY = 190;
var ACCELERATION = 0.00005;
var NUMBERS = [26, 3, 35, 12, 28, 7, 29, 18, 22, 9, 31, 14, 20, 1, 33, 16, 24, 5, 10, 23, 8, 30, 11, 36, 13, 27, 6, 34, 17, 25, 2, 21, 4, 19, 15, 32, 0];
var TRANSFORM;

var contactForce = -0.00015;
var angle = 0;
var firstTimeCount = false;
var v = 0.15 + 0.05 * Math.random();
var r = 140;


function Start() {
    contactForce = -0.00015;
    angle = 0;
    firstTimeCount = false;
    v = 0.15 + 0.05 * Math.random();
    r = 140;
    $("#ball").move();
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

function getDegree(radian) {
    return radian * 180 /  Math.PI;
}

function getDifference(rouletteAngle, ballAngle) {
    //adding 90 degrees to make start point as roulette's start point(begining of second quarter(top...by default zero is left point))
    var dif = (rouletteAngle % 360 - (getDegree(ballAngle) + 90) % 360);
    dif = dif > 0 ? dif % 360 : (360 - dif) % 360;
    return dif * 37 / 360;
}

function rotateWheel(d) {
    setTimeout(function () {
        $('.roulette-wheel')[0].style[TRANSFORM] = 'rotate(' + (d % 360) + 'deg)';

        if (v <= 0) {
            if (!firstTimeCount) {

                var dif = getDifference(d, angle);
                var decimalPart = dif - Math.floor(dif);

                var winner;
                if (decimalPart <= 0.5 && decimalPart > 0.1) {
                    angle += (decimalPart - 0.1) * 37 / 360;
                    winner = NUMBERS[Math.floor(dif + (decimalPart - 0.1) * 37 / 360) - 1];
                }
                else if (decimalPart > 0.5 && decimalPart < 0.9) {
                    angle -= (0.9 - decimalPart) * 37 / 360;
                    winner = NUMBERS[Math.floor(dif - (0.9 - decimalPart) * 37 / 360)];
                }
                else {
                    winner = dif - Math.floor(dif) > 0.9 ? NUMBERS[Math.floor(dif)] : NUMBERS[Math.floor(dif) - 1];
                }

                $("#board .round").each(function () {
                    if ($(this).text() == winner) {
                        $(this).parents("td").addClass("highlighted");
                    }
                });

                firstTimeCount = true;
            }

            $("#ball").css("top", CENTERY + (r * Math.sin(angle)) - $("#ball").height() / 2).css("left", CENTERX + (r * Math.cos(angle)) - $("#ball").width() / 2);
        }

        angle += getRadian(0.5);
        rotateWheel(d + 0.5);

    }, ROULETTE_SPEED);
}

$.fn.move = function () {
    angle -= v;

    if (v > 0) {
        $(this).animate({
            top: CENTERY + (r * Math.sin(angle)) - $(this).height() / 2,
            left: CENTERX + (r * Math.cos(angle)) - $(this).width() / 2
        }, 10, function () {

            v += contactForce;
            if (v * v / ACCELERATION > 140) {
                r = 140;
            }
            else if (v * v / ACCELERATION < 100) {
                r = 100;
            }
            else {
                r = v * v / ACCELERATION;
            }
            if (r < 120) {
                contactForce = -0.002;
            }
            $("#ball").show().move();
        });
    }

}

$(function () {
    TRANSFORM = getTransformProperty($('.roulette-wheel')[0]);
    if (TRANSFORM) {
        rotateWheel(0);
    }
});