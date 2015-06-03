using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitTheRisks
{
    class SingleMatchBetInfo
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
    }
}
