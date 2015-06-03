using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace LimitTheRisks
{
    public class SubTree
    {
        private List<MatchParams> Probs;
        private List<MatchParams> Coefs;
        public Node Tree;
        public Node Top;
        public Node CurNode;
        private int TreeLevels;
        private double LocalCoef;
        private double level;
        private StringBuilder stringBuilder = new StringBuilder();
        //private StreamWriter streamWriter = new StreamWriter("Output.txt");

        public SubTree(List<MatchParams> probs, List<MatchParams> coefs)
        {
            Tree = new Node();
            this.Probs = probs;
            this.Coefs = coefs;
            Tree.LocalCoef = 1;
            Tree.LocalProb = 1;
            TreeLevels = 2;
            BuildTheTree(Tree);
            PassTheTree(Tree);
            var str = stringBuilder.ToString();
        }

        private void BuildTheTree(Node tree)
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
                        level++;
                    }
                }
                else
                {
                    Tree = Tree.Parent;
                    level--;
                }
                if (level == TreeLevels)
                {
                    if (nodesNum == Math.Pow(3, TreeLevels))
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
          /*  using (StreamWriter streamWriter = new StreamWriter("123.txt"))
            {
                streamWriter.WriteLine("_____________________________________");
                streamWriter.WriteLine(tree.LocalCoef + " " + tree.LocalProb);
            }
           */
            stringBuilder.AppendLine(tree.LocalCoef + " " + tree.LocalProb);
            if (tree.Win1 == null)  // the level is the last
            {
                return;
            }
            PassTheTree(tree.Win1);
            PassTheTree(tree.Draw);
            PassTheTree(tree.Win2);
        }
    }
}