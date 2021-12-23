using System;
using System.Collections.Generic;

namespace _6lab_nochkaGame
{
    public sealed class Node
    {
        // four rows -> current suit and taken values
        public readonly List<(String, int[])> State;

        public readonly Node ParentNode;

        public readonly int Depth;

        public List<Node> ChildrenNodes;

        public readonly int StaticEvaluation;

        public Node(List<(String, int[])> State, Node ParentNode, int Depth, int StaticEvaluation)
        {
            ChildrenNodes = new List<Node>();

            this.State = State;
            this.ParentNode = ParentNode;
            this.Depth = Depth;
            this.StaticEvaluation = StaticEvaluation;
        }
    }
}
