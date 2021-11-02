using System.Collections.Generic;

namespace _3lab_vertexColoringABC
{
    public class Vertex
    {
        public int VertexNum { get; private set;}

        public List<int> AdjacentVertexes { get; private set; }

        public int ColorNum { get; set; }

        public int NumOfNectar { get; set; }

        public List<OnLookerBee> onLookerBeesAtThisVertex { get; set; }

        public Vertex(int VertexNum, int[,] AdjMatrix)
        {
            this.VertexNum = VertexNum;
            this.AdjacentVertexes = Algorithms.GetAdjacentVertexes(VertexNum, AdjMatrix);
            this.NumOfNectar = this.AdjacentVertexes.Count; 
        }
    }
}
