var NUMBERS = [26, 3, 35, 12, 28, 7, 29, 18, 22, 9, 31, 14, 20, 1, 33, 16, 24, 5, 10, 23, 8, 30, 11, 36, 13, 27, 6, 34, 17, 25, 2, 21, 4, 19, 15, 32, 0]

function Highlight(index) {
    index = (37 + index) % 37;  
    var number = NUMBERS[index];
    $(".round:contains('" + number + "')").each(function (idx, val) {
        if ($(val).html() == number) {
            $(val).parents().eq(1).highlight();
            cells[index][0].attr("fill-opacity", 0.3);
            cells[index][1].attr("fill", "#FFFF00");
        }
    })
}

function HighlightNeighbors(element) {
    ClearHighlight();
    var index = element.data("index");
    var betFrom = index - 2;
    var batTo = index + 2;
    for (var i = betFrom; i <= batTo; i++) {              
        Highlight(i);
    }
}

function HighlightJeuZero() {
    ClearHighlight();
    for (var i = 34; i <= 36; i++) {
        Highlight(i);
    }
    for (var i = 0; i <= 3; i++) {
        Highlight(i);
    }
}

function HighlightVoisinsduZero() {
    ClearHighlight();
    for (var i = 29; i <= 36; i++) {
        Highlight(i);
    } 
    for (var i = 0; i <= 8; i++) {
        Highlight(i);
    }
}

function HighlightOrphelins() {
    ClearHighlight();
    for (var i = 26; i <= 28; i++) {
        Highlight(i);
    }
    for (var i = 9; i <= 13; i++) {
        Highlight(i);
    }
}

function HighlightTiersduCylindre() {
    ClearHighlight();
    for (var i = 14; i <= 25; i++) {
        Highlight(i);
    }
}

function ClearHighlight() {
    $(".highlighted").removeClass("highlighted");

    for (var i = 0; i < cells.length; i++) {
        cells[i][0].attr("fill-opacity", 1);
        cells[i][1].attr("fill", "#FFF");
    }
}


$(function () {
    var cellWidth = $(".roulette-board tr:first").find("td").outerWidth();
    var cellHeight = $(".roulette-board tr:first").find("td").outerHeight();

    $.fn.highlight = function () {
        $(this).addClass("highlighted");
    };

    function HighlightCellsWithColor(color) {
        color == "red" ? $(".roulette-board td.red").highlight() : $(".roulette-board td.black").highlight();
    }

    function HighlightCellsGroup(elem, start, end) {
        elem.find("td").slice(start, end).highlight();
    }

    function HighlightCells(elem, start, end, pointX, pointY, index, rowCount) {
        if (parseInt(pointX) < rowCount) {
            elem.find("td").slice(start, end).highlight();
            if (pointY > 0.8) {
                if (index < 2) {
                    elem.parent().find("tr").eq(index + 1).find("td").slice(start, end).highlight();
                }
                else if (index == 2) {
                    elem.parent().find("tr").slice(0, 3).each(function () {
                        $(this).find("td").slice(start, end).highlight();
                    });
                }
            }
            else if (pointY < 0.2 && index > 0) {
                elem.parent().find("tr").eq(index - 1).find("td").slice(start, end).highlight();
            }
        }
    }

    $(".roulette-board").mouseleave(function () {
        ClearHighlight();
    });

    $("#zero").mouseleave(function (e) {
        ClearHighlight();
    });
    
    $("#sectors-container").mouseleave(function (e) {
        ClearHighlight();
    });

    $("#zero").mousemove(function (e) {
        $("#zero").highlight();
    });

    $(".roulette-board tr").mousemove(function (e) {
        var x = e.pageX - $(this).offset().left;
        var y = e.pageY - $(this).offset().top;
        var index = $(this).index();
        var pointX = x / cellWidth;
        var pointY = y / cellHeight;

        ClearHighlight();

        if (index == 0 || index == 1 || index == 2) {
            if (parseInt(pointX) == 12) {
                HighlightCellsGroup($(this), 0, 12);
            }
            else if (Math.abs(pointX - parseInt(pointX)) > 0.8) {
                HighlightCells($(this), parseInt(pointX), parseInt(pointX) + 2, pointX, pointY, index, 11);
            }
            else if (Math.abs(pointX - parseInt(pointX)) < 0.2) {
                HighlightCells($(this), parseInt(pointX) - 1 < 0 ? 0 : parseInt(pointX) - 1, parseInt(pointX) + 1, pointX, pointY, index, 11);
                if (parseInt(pointX) == 0) $("#zero").highlight();
            }
            else {
                HighlightCells($(this), parseInt(pointX), parseInt(pointX) + 1, pointX, pointY, index, 12);
            }
        }
        else if (index == 3) {
            var ind = parseInt(pointX / 4);
            for (i = 0; i < 3; i++) {
                HighlightCellsGroup($(this).parent().find("tr").eq(i), ind * 4, ind * 4 + 4);
            }
        }
        else {
            var ind = parseInt(pointX / 2);
            switch (ind) {
                case 0:
                    {
                        for (i = 0; i < 3; i++) {
                            HighlightCellsGroup($(this).parent().find("tr").eq(i), 0, 6);
                        }
                        break;
                    }
                case 1:
                    {
                        for (i = 0; i < 3; i++) {
                            for (j = 0; j < 12; j++) {
                                if ((j % 2 == 0 && i % 2 == 1) || (j % 2 == 1 && i % 2 == 0)) {
                                    HighlightCellsGroup($(this).parent().find("tr").eq(i), j, j + 1);
                                }
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        HighlightCellsWithColor("black");
                        break;
                    }
                case 3:
                    {
                        HighlightCellsWithColor("red");
                        break;
                    }
                case 4:
                    {
                        for (i = 0; i < 3; i++) {
                            for (j = 0; j < 12; j++) {
                                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 == 1 && i % 2 == 1)) {
                                    HighlightCellsGroup($(this).parent().find("tr").eq(i), j, j + 1);
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        for (i = 0; i < 3; i++) {
                            HighlightCellsGroup($(this).parent().find("tr").eq(i), 6, 12);
                        }
                        break;
                    }

            }
        }


    });
});