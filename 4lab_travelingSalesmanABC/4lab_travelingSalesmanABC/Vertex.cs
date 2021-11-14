using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4lab_travelingSalesmanABC
{
    public class Vertex
    {
        public int VertexNum { get; private set; }
        public int NumOfNectar { get; set; }

        public List<OnLookerBee> onLookerBeesAtThisVertex { get; set; }

        public Vertex(int VertexNum, Cities cities)
        {
            this.VertexNum = VertexNum;
            this.NumOfNectar = cities.Solution.Length / 
                cities.GetCityNumWithTheLowestPathFromChosen(VertexNum).Values.First();
        }
    }
}
