using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LimitTheRisks
{
    internal class Data
    {
        public List<Match> allMatches = new List<Match>();
        private List<SingleMatchBetInfo> OnePlayerBet = new List<SingleMatchBetInfo>();
        private List<List<SingleMatchBetInfo>> allBetsList = new List<List<SingleMatchBetInfo>>();
        private const int matchesNum = 6;
        private const double R = 0.05;
        private const int NumberOfBets = 10;
        public List<MatchParams> ProbsMarathon = new List<MatchParams>();
        public List<MatchParams> ProbsOtherCo = new List<MatchParams>();
        public List<MatchParams> CoefsMarathon = new List<MatchParams>();
        public List<MatchParams> CoefsOtherCo = new List<MatchParams>();


        public Data()
        {
            ObtainData();
            SubTree tree = new SubTree(ProbsMarathon,CoefsMarathon);
        }
        private  void ObtainData()
        {
            GenProbsMarathon();
            GenProbsOtherCo();
            CoefsMarathon = GetCoefs(ProbsMarathon);
            CoefsOtherCo = GetCoefs(ProbsOtherCo);
        }

        /*   public  void FillInTheInputList()
        {
            string homeTeamName = String.Empty;
            string arrivalTeamName = String.Empty;
            List<double> coefList = new List<double>();
            List<string> linesList = ReadMatchesDataFromFile();
            for (int i = 0; i < linesList.Count; i++)
            {
                if (i % 5 == 0)
                    homeTeamName = linesList[i];
                if (i % 5 == 1)
                    arrivalTeamName = linesList[i];
                if (i % 5 == 2)
                    coefList.Add(Convert.ToDouble(linesList[i]));
                if (i % 5 == 3)
                    coefList.Add(Convert.ToDouble(linesList[i]));
                if (i % 5 == 4)
                {
                    coefList.Add(Convert.ToDouble(linesList[i]));
                    var match = new Match(coefList, homeTeamName, arrivalTeamName);
                    allMatches.Add(match);
                    coefList.Clear();
                }
            }
        }
     */

        private void GenAllBetsOfAllPlayers()
        {
            for (int i = 0; i < NumberOfBets; i++)
            {
                OnePlayerBet = GenerateBet();
                allBetsList.Add(OnePlayerBet);
            }
        }

        private void GenProbsMarathon()
        {
            double minProb = 0.01;
            Random random = new Random();
            for (int i = 0; i < matchesNum; i++)
            {
                double x1 = random.NextDouble()*(1 - 2*minProb) + minProb;
                    //random.NextDouble() * (maximum - minimum) + minimum;
                double x = random.NextDouble()*(1 - x1 - 2*minProb) + minProb;
                double x2 = 1 - x1 - x;
                MatchParams matchParams = new MatchParams(x1, x2, x);
                ProbsMarathon.Add(matchParams);
            }
        }

        private  void GenProbsOtherCo()
        {
            double delta = 0.005;
            Random random = new Random();
            try
            {
                for (int i = 0; i < matchesNum; i++)
                {
                    double x1 = ProbsMarathon[i].X1 + random.NextDouble()*(2*delta) - delta;
                    double x2 = ProbsMarathon[i].X2 + random.NextDouble()*(2*delta) - delta;
                    double x = ProbsMarathon[i].X + random.NextDouble()*(2*delta) - delta;

                    MatchParams matchParams = new MatchParams(x1, x2, x);
                    ProbsMarathon.Add(matchParams);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

        }

        private  List<MatchParams> GetCoefs(List<MatchParams> probsList)
        {
            var coefList = new List<MatchParams>();
            try
            {
                for (int i = 0; i < probsList.Count; i++)
                {
                    double x1 = (1 - R)/probsList[i].X1;
                    double x2 = (1 - R)/probsList[i].X2;
                    double x = (1 - R)/probsList[i].X;

                    MatchParams matchParams = new MatchParams(x1, x2, x);
                    coefList.Add(matchParams);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            return coefList;
        }


        private  List<string> ReadMatchesDataFromFile()
        {
            List<string> linesList = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader("Matches.txt"))
                {
                    while (sr.Peek() >= 0)
                    {
                        linesList.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }
            string pattern = @"\.";
            string replacement = @",";
            Regex rgx = new Regex(pattern, RegexOptions.None);
            rgx.Replace(linesList[2], replacement);
            for (int i = 0; i < linesList.Count; i++)
            {
                linesList[i] = rgx.Replace(linesList[i], replacement);
            }
            return linesList;
        }

        private  List<SingleMatchBetInfo> GenerateBet()
        {
            Random random = new Random();
            var matchesNumber = random.Next(2, 10); // randomize the number of matches in express
            List<int> chosenMatches = new List<int>();
            for (int i = 0; i < matchesNumber; i++)
            {
                int matchNum = random.Next(0, matchesNum - 1); //randomize the choice of match
                while (chosenMatches.Contains(matchNum))
                {
                    matchNum = random.Next(0, matchesNum - 1); //randomize the choice of match
                }
                chosenMatches.Add(matchNum);

                int betSize = random.Next(1, 10000); //randomize  the size of bet
                int matchResult = random.Next(0, 2); // randomize match result
                double chosenCoef = 0;
                switch (matchResult)
                {
                    case 0:
                        chosenCoef = CoefsMarathon[matchNum].X1;
                        break;
                    case 1:
                        chosenCoef = CoefsMarathon[matchNum].X2;
                        break;;
                    case 2:
                        chosenCoef = CoefsMarathon[matchNum].X;
                        break;
                }
                
                SingleMatchBetInfo singleMatchBetInfo = new SingleMatchBetInfo(matchNum, betSize, matchResult,
                    chosenCoef);
                OnePlayerBet.Add(singleMatchBetInfo);
            }
            return OnePlayerBet;
        }
    }
}