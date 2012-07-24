using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Helper
{
    public class Constant
    {
        public static Dictionary<string,int> Coeffecent = new Dictionary<string,int>(){
            {"SingleElement",36},
            {"HorizontalPair",18},
            {"VerticalPair",18},
            {"HorizontalWithZeroPair",18},
            {"HorizontalWithZeroTrips",12},
            {"Quads",9},
            {"TwelveElements",3},
            {"EvenOrOdd",2},
            {"BlackOrRed",2},
            {"EighteenElements",2},
            {"HorizontalLine",3},
            {"VerticalTrips",12},
            {"TwoVerticalTrips",6}
        };
    }
}