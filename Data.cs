﻿using System;
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
        public List<BetInfo> AllBets = new List<BetInfo>();
       // public List<Match> allMatches = new List<Match>();
      //  public List<SingleMatchBetInfo> OnePlayerBet = new List<SingleMatchBetInfo>();
       // public List<List<SingleMatchBetInfo>> allBetsList = new List<List<SingleMatchBetInfo>>();
        private const int matchesNum = 6;
        private const double R = 0.05;
        private const int NumberOfBets = 10;
        public List<MatchParams> ProbsMarathon = new List<MatchParams>();
        public List<MatchParams> ProbsOtherCo = new List<MatchParams>();
        public List<MatchParams> CoefsMarathon = new List<MatchParams>();
        public List<MatchParams> CoefsOtherCo = new List<MatchParams>();
        private Random RandomNum = new Random();   // the way to make this variable global is the only possible as random numbers are equal to the first random number 


        public Data()
        {
            ObtainData();
            SubTree tree = new SubTree(ProbsMarathon,CoefsMarathon,AllBets);
        }
        private  void ObtainData()
        {
            GenProbsMarathon();
            GenProbsOtherCo();
            CoefsMarathon = GetCoefs(ProbsMarathon);
            CoefsOtherCo = GetCoefs(ProbsOtherCo);
            GenAllBetsOfAllPlayers();
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
            BetInfo OnePlayerBet;
            for (int i = 0; i < NumberOfBets; i++)
            {
                OnePlayerBet = GenerateBet();
                AllBets.Add(OnePlayerBet);
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
                    ProbsOtherCo.Add(matchParams);
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


     /*   private  List<string> ReadMatchesDataFromFile()
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
      */

        private  BetInfo GenerateBet()
        {
            
            var matchesNumber = RandomNum.Next(1, 10); // randomize the number of matches in express
            List<int> chosenMatches = new List<int>();
            List<int> outcomes = new List<int>();
            int betSize = RandomNum.Next(1, 10000); //randomize  the size of bet
            for (int i = 0; i < matchesNumber; i++)
            {
                int matchNum = RandomNum.Next(0, matchesNum - 1); //randomize the choice of match 
                while (chosenMatches.Contains(matchNum))
                {
                    matchNum = RandomNum.Next(0, matchesNum - 1); //randomize the choice of match
                }
                chosenMatches.Add(matchNum);
                
                int matchResult = RandomNum.Next(0, 2); // randomize match result  0 -> x1;  1 -> x; 2 -> x2;
                outcomes.Add(matchResult);
                
            }

            double coef = 1;
            for (int i = 0; i < chosenMatches.Count; i++)
            {
                switch (outcomes[i])
                {
                    case 0:
                        coef *= CoefsMarathon[chosenMatches[i]].X1;

                        break;
                    case 1:
                        coef *= CoefsMarathon[chosenMatches[i]].X;
                        break;
                    case 2:
                        coef *= CoefsMarathon[chosenMatches[i]].X2;
                        break;
                }
            }
            chosenMatches.Sort();
            BetInfo betinfo = new BetInfo(chosenMatches, outcomes, betSize, coef);
            coef = 1;

            /*
            // sort by the match number criterium
            OnePlayerBet.Sort(delegate(SingleMatchBetInfo x, SingleMatchBetInfo y)
            {
                if (x.MatchNum == null && y.MatchNum == null) return 0;
                else if (x.MatchNum == null) return -1;
                else if (y.MatchNum == null) return 1;
                else return x.MatchNum.CompareTo(y.MatchNum);
            });
            OnePlayerBet.Sort();
             */
            return betinfo;
        }
    }
}