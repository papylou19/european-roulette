using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roulette.Models
{
    public class ColorField
    {
        public int Number {get;set;}
        public bool Color { get; set; }

        public ColorField(int number, bool color)
        {
            this.Number = number;
            this.Color = color;
        }
    }

    public class TableModel
    {
        public ColorField[] Colors { get; set; }
    }
}