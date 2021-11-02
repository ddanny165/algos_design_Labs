using System;

namespace _3lab_vertexColoringABC
{
    public class Graph
    {
        public int[,] graphAdjacencyMatrix { get; private set;}

        public Graph (int verticesNum, int minVertexDegree, int maxVertexDegree)
        {
            GenerateRandomAdjacencyMatrix(verticesNum, minVertexDegree, maxVertexDegree);
        }

        private void GenerateRandomAdjacencyMatrix(int verticesNum, int minVertexDegree, int maxVertexDegree)
        {
            if (verticesNum <= 2 || minVertexDegree < 1 || maxVertexDegree > 30)
            {
                //maxDegree is 30 according to the given task
                throw new ArgumentException("VerticlesNum can't be less than 2, " +
                    "minVertexDegree less than 1 and maxVertexDegree more than 30.");
            }

            graphAdjacencyMatrix = new int[verticesNum, verticesNum];

            InitializeAdjMatrixOfGraph(verticesNum, minVertexDegree, maxVertexDegree);
        }

        private void InitializeAdjMatrixOfGraph(int verticesNum, int minVertexDegree, int maxVertexDegree)
        {
            Random ran = new();
            bool foundPosToInitialize = false;

            for (int i = 0; i < verticesNum; i++)
            {
                
                int numOfPositionsToInitialize = ran.Next(minVertexDegree, maxVertexDegree);
                
                for (int j = 0; j < numOfPositionsToInitialize; j++)
                {
                    while (!foundPosToInitialize)
                    {
                        int rndPos = ran.Next(0, verticesNum - 1);
                        var selectedVertex = graphAdjacencyMatrix[i, rndPos];

                        if (CheckIfNotMoreThanLimit(verticesNum, rndPos, maxVertexDegree) && selectedVertex == 0 && i != rndPos)
                        {
                            graphAdjacencyMatrix[i, rndPos] = 1;
                            graphAdjacencyMatrix[rndPos, i] = 1;
                            foundPosToInitialize = true;
                        }
                        else
                        {
                            foundPosToInitialize = false;
                        }
                    }
                    foundPosToInitialize = false;
                }
                foundPosToInitialize = false;
            }
        }
        private bool CheckIfNotMoreThanLimit(int verticesNum, int vertexNum, int maxDegreeNum)
        {
            bool notMoreAdjThanLimit = true;
            int adjVerticesCounter = 0;

            for (int i = 0; i < verticesNum; i++)
            {
                if (graphAdjacencyMatrix[vertexNum, i] == 1)
                {
                    adjVerticesCounter++;
                }
            }

            if (adjVerticesCounter >= maxDegreeNum)
            {
                notMoreAdjThanLimit = false;
            }

            return notMoreAdjThanLimit;
        }
    }
}
