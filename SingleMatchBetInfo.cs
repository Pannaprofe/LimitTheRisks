using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitTheRisks
{
    class SingleMatchBetInfo: IEquatable<SingleMatchBetInfo>,IComparable<SingleMatchBetInfo>
    {
        public int MatchNum { get; set; }
        public int BetSize { get; set; }
        public int MatchResult { get; set; }  // 0 - X1, 1 - Draw, 2 - X2
        public double Coef { get; set; }

        public SingleMatchBetInfo(int matchNum, int betsize, int matchResult, double coef)
        {
            this.MatchNum = matchNum;
            this.BetSize = betsize;
            this.MatchResult = matchResult;
            this.Coef = coef;
        }


        public override string ToString()
        {
            return "Match Number: " + MatchNum + "   BetSize: " + BetSize + "   MatchResult: " + MatchResult + "   Coef: " + Coef;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            SingleMatchBetInfo objAsPart = obj as SingleMatchBetInfo;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int SortByNameAscending(string name1, string name2)
        {

            return name1.CompareTo(name2);
        }

        // Default comparer for Part type.
        public int CompareTo(SingleMatchBetInfo comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            else
                return this.MatchNum.CompareTo(comparePart.MatchNum);
        }
        public override int GetHashCode()
        {
            return MatchNum;
        }
        public bool Equals(SingleMatchBetInfo other)
        {
            if (other == null) return false;
            return (this.MatchNum.Equals(other.MatchNum));
        }
    }
}
