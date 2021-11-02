using System;

namespace _3lab_vertexColoringABC
{
    class Program
    {
        static void Main(string[] args)
        {
			// According to the lab task (12th num)
            int numOfGraphVertices = 300;
            int minVertexDegree = 1;
            int maxVertexDegree = 30;
			int numOfEmployedBees = 5;
			int numOfOnLookerBees = 55;

            Graph graph = new Graph(numOfGraphVertices, minVertexDegree, maxVertexDegree);

			// Solution (number of colors required for coloring the whole graph)
			int chromaticNumber = Algorithms.ArtificialBeeColonyGraphColoring(graph.graphAdjacencyMatrix, numOfEmployedBees, numOfOnLookerBees);
            
			Console.WriteLine($"Chromatic number after coloring the graph with a help of ABC algorithm equals to |{chromaticNumber}|.");
		}
	}
}
