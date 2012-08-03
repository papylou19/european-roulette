using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Helpers
{
    public class StakeDTO
    {
        public byte Id { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
    }

    public class Report
    {
        public int Stake { get; set; }
        public int CountStake { get; set; }
        public double WinSum { get; set; }
        public double WaitingSum { get; set;}
        public List<Stake> Stakes {get;set;}
    }

    public enum TypesEnum
    {
        SingleElement = 1,
        HorizontalPair = 2,
        VerticalPair = 3,
        VerticalTrips = 4,
        HorizontalLine = 5,
        Quads = 6,
        TwoVerticalTrips = 7,
        TwelveElements = 8,
        EvenOrOdd = 9,
        BlackOrRed = 10,
        EighteenElements = 11,
        HorizontalWithZeroPair = 12,
        HorizontalWithZeroTrips = 13
    }
}
