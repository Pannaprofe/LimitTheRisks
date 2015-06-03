using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitTheRisks
{
    public class MatchParams
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X { get; set; }

        public MatchParams(double x1, double x2, double x)
        {
            this.X1 = x1;
            this.X2 = x2;
            this.X = x;
        }
    }
}
