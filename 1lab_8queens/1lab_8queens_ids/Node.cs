using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1lab_8queens_ids
{
    public sealed class Node
    {
        public int[,] State { get; set; } //информация о том, как расставлены фишки
        public Node parentNode { get; set; } //из чего мы пришли к текущему состоянию

        public List<Node> children = new List<Node>();

        // public string Action { get; set; } //действия, которое было применено к нашему узлу

        public int Depth { get; set; } //количество этапов пути от начального состояние (глубина)

        public Node(int[,] State, Node parentNode, int Depth)
        {
            this.State = State;
            this.parentNode = parentNode;
            this.Depth = Depth;
        }
    }
}
