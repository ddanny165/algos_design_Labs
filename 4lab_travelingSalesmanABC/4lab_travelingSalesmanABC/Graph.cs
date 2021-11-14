using System;

namespace _4lab_travelingSalesmanABC
{
    public class Graph
    {
        public int[,] graphAdjacencyMatrix { get; private set; }

        public Graph(int verticesNum, int minDistance, int maxDistance)
        {
            GenerateRandomAdjacencyMatrix(verticesNum, minDistance, maxDistance);
        }

        private void GenerateRandomAdjacencyMatrix(int verticesNum, int minDistance, int maxDistance)
        {
            if (verticesNum <= 2)
            {
                throw new ArgumentException("VerticlesNum can't be less than 2, ");
            }

            graphAdjacencyMatrix = new int[verticesNum, verticesNum];
            InitializeAdjMatrixOfGraph(verticesNum, minDistance, maxDistance);
        }

        private void InitializeAdjMatrixOfGraph(int verticesNum, int minDistance, int maxDistance)
        {
            Random ran = new Random();
            int distance = 0;
            

            for (int i = 0; i < verticesNum; i++)
            {
                for (int j = 0; j < verticesNum; j++)
                { 
                    if (i != j && graphAdjacencyMatrix[i, j] == 0 && 
                        graphAdjacencyMatrix[i, j] == graphAdjacencyMatrix[j, i])
                    {
                        distance = ran.Next(minDistance, maxDistance);

                        graphAdjacencyMatrix[i, j] = distance;
                        graphAdjacencyMatrix[j, i] = graphAdjacencyMatrix[i, j];
                    }
                }
            }
        }
    }
}
