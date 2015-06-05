using System;
using System.Collections.Generic;  // List<>
using System.Data;
using System.IO;   // StreamWriter
using System.Text;  //StringBuilder

namespace LimitTheRisks
{
    public class SubTree
    {
        private List<MatchParams> Probs;
        private List<MatchParams> Coefs;
        private List<BetInfo> Bets;
        private List<int> tmp = new List<int>();

        private Node Tree;
        private Node Top;
        private Node CurNode;

        private int TreeLevels;
        private StringBuilder stringBuilder = new StringBuilder();
        private int CriticalNodeNumber;
        private bool isOrderedSublist = true;

        public SubTree(List<MatchParams> probs, List<MatchParams> coefs, List<BetInfo> bets)
        {
            this.Probs = probs;
            this.Coefs = coefs;
            this.Bets = bets;
            //initialize tree top
            Tree = new Node();
            Tree.LocalCoef = 1;
            Tree.LocalProb = 1;
            TreeLevels = probs.Count;
            GetCriticalNodeNumber();
            Top = Tree;
            CurNode = Top;

            BuildTheTree(ref Tree);          
            PassTheTree(Top);
            Output();

            var output = stringBuilder.ToString();
            using (StreamWriter streamWriter = new StreamWriter("123.txt"))
            {
                streamWriter.WriteLine(output);
            }
        }


        private void Output()
        {
            foreach (BetInfo playerbet in Bets)
            {
                PassTheTreeSelectively(Top,playerbet);
            }
        }
        //returns number of nodes in the tree, excluding top element
        private void GetCriticalNodeNumber()
        {
            for (int i=0; i<TreeLevels;i++)
            {
                CriticalNodeNumber = CriticalNodeNumber * 3 + 3; 
            }
        }

        private void BuildTheTree(ref Node tree)
        {
            bool stop = false;
            int level = 0;
            int nodesNum = 0;

            while (!stop)
            {
                if (level < TreeLevels)
                {
                    if (tree.Win1 == null)
                    {
                        Node node = new Node();
                        node.Coef = Coefs[level].X1;
                        node.Prob = Probs[level].X1;
                        node.LocalCoef = tree.LocalCoef*node.Coef;
                        node.LocalProb = tree.LocalProb*node.Prob;
                        tree.Win1 = node;
                        node.Parent = tree;
                        tree = node;
                        nodesNum++;
                        tree.NodeNum = nodesNum;
                        level++;
                    }
                    else if (tree.Draw == null)
                    {
                        Node node = new Node();
                        node.Coef = Coefs[level].X;
                        node.Prob = Probs[level].X;
                        node.LocalCoef = tree.LocalCoef * node.Coef;
                        node.LocalProb = tree.LocalProb * node.Prob;
                        tree.Draw = node;
                        node.Parent = tree;
                        tree = node;
                        nodesNum++;
                        tree.NodeNum = nodesNum;
                        level++;
                    }
                    else if (tree.Win2 == null)
                    {
                        Node node = new Node();
                        node.Coef = Coefs[level].X2;
                        node.Prob = Probs[level].X2;
                        node.LocalCoef = tree.LocalCoef * node.Coef;
                        node.LocalProb = tree.LocalProb * node.Prob;
                        tree.Win2 = node;
                        node.Parent = tree;
                        tree = node;
                        nodesNum++;
                        tree.NodeNum = nodesNum;
                        level++;
                    }
                    else
                    {
                        Tree = Tree.Parent;
                        level--;
                    }
                }
                
                if (level == TreeLevels)
                {
                    if (nodesNum == CriticalNodeNumber)
                        stop = true;
                    else
                    {
                        level--;
                        tree = tree.Parent;
                    }
                }
            }
        }

        private void PassTheTree(Node tree)
        {
            stringBuilder.AppendLine(tree.NodeNum.ToString() + " " + tree.LocalProb);
            if (tree.Win1 == null)  // the level is the last
            {
                return;
            }
            PassTheTree(tree.Win1);
            PassTheTree(tree.Draw);
            PassTheTree(tree.Win2);
        }

        //non recursive pass of the tree
        private void PassTheTreeSelectively(Node tree, BetInfo playerBet)
        {
           // stringBuilder.AppendLine(tree.NodeNum.ToString() + " " + tree.LocalProb);
            tmp.Add(tree.Level);
            if (tree.Win1 == null)  // the level is the last
            {
                foreach (int elem in playerBet.MatchesAndOutcomes.MatchList)
                {
                    if (!tmp.Contains(elem))
                    {
                        isOrderedSublist = false;
                        break;
                    }
                }
                if (isOrderedSublist)
                {
                    foreach (int elem in playerBet.MatchesAndOutcomes.MatchList)
                    {
                        stringBuilder.AppendLine(elem.ToString());
                    }
                }
                tmp.RemoveAt(tmp.Count - 1);
                return;
            }
            PassTheTreeSelectively(tree.Win1,playerBet);
            PassTheTreeSelectively(tree.Draw,playerBet);
            PassTheTreeSelectively(tree.Win2,playerBet);
        }
    }
}