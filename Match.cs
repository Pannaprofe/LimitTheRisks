using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitTheRisks
{
    class Match
    {
        public List<double>  matchResultsCoefList;
        public string homeTeamName;
        public string arrivalTeamName;

        public Match(List<double> coefList,string hmtmNm,string arTmNm)
        {
            this.matchResultsCoefList = coefList;
            this.homeTeamName = hmtmNm;
            this.arrivalTeamName = arTmNm;
        }
    }
}
