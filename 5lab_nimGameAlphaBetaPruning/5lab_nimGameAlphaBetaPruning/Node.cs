using System.Collections.Generic;

namespace _5lab_nimGameAlphaBetaPruning
{
    public sealed class Node
    {
        // state --> Информация о текущем состоянии
        public readonly int[] State;
        
        public readonly Node ParentNode;

        public readonly int Depth;

        public List<Node> ChildrenNodes;

        // значение функции оценки
        public readonly int StaticEvaluation;

        public Node(int[] State, Node ParentNode, int Depth, int StaticEvaluation) 
        {
            ChildrenNodes = new List<Node>();
            
            this.State = State;
            this.ParentNode = ParentNode;
            this.Depth = Depth;
            this.StaticEvaluation = StaticEvaluation;
        }
    }
}
