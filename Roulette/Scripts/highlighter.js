$(function () {

    var cellWidth = $(".roulette-board tr:first").find("td").outerWidth();
    var cellHeight = $(".roulette-board tr:first").find("td").outerHeight();


    $.fn.highlight = function () {
        $(this).find("div.round").css("color", "yellow");
        $(this).css("background", "#228F22");
    };


    function ClearHighlight() {
        $(".roulette-board td").find("div.round").css("color", "White");
        $(".roulette-board td").css("background", "url('/img/green-texture.jpg')");
        //$(".roulette-board td.black").find("div.round").css("background-color", "black");
       // $(".roulette-board td.red").find("div.round").css("background-color", "red");
    }

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
            else if (Math.abs(pointX - parseInt(pointX)) > 0.85) {
                HighlightCells($(this), parseInt(pointX), parseInt(pointX) + 2, pointX, pointY, index, 11);
            }
            else if (Math.abs(pointX - parseInt(pointX)) < 0.15) {
                HighlightCells($(this), parseInt(pointX) - 1 < 0 ? 0 : parseInt(pointX) - 1, parseInt(pointX) + 1, pointX, pointY, index, 11);
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