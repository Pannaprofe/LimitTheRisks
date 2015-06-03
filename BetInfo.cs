using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitTheRisks
{
    class BetInfo
    {
        public Matches MatchesAndOutcomes { get; set; }
        public int BetSize { get; set; }
        public double Coef { get; set; }

        public BetInfo(List<int> matchList, List<int> outcomes, int betSize, double coef)
        {
            this.MatchesAndOutcomes.MatchList = matchList;
            this.MatchesAndOutcomes.Outcomes = outcomes;
            this.BetSize = betSize;
            this.Coef = coef;
        }
    }
}
