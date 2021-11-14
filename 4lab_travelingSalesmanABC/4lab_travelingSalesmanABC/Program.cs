using System;

namespace _4lab_travelingSalesmanABC
{
    class Program
    {
        static void Main(string[] args)
        {
            int verticesNum = 250;
            int numOfEmployedBees = 1;
            int numOfOnLookerBees = 100;
            int minDistance = 5;
            int maxDistance = 150;

            Graph graph = new Graph(verticesNum, minDistance, maxDistance);
            Algorithms.ArtificialBeeColonyTSP(graph.graphAdjacencyMatrix, numOfEmployedBees, numOfOnLookerBees);
        }
    }
}
