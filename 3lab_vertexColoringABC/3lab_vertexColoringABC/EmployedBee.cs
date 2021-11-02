using System;
using System.Collections.Generic;

namespace _3lab_vertexColoringABC
{
    public class EmployedBee
    {
        public int numOfSelectedVertex { get; set; }
        public int amountOfNectarInSelectedVertex { get; set; }

        public int ChooseARandomVertex(int numOfGraphVertices, List<EmployedBee> otherEmployedBees)
        {
            bool foundAUniqueVertex = false;
            int vertexToChoose = (new Random()).Next(0, numOfGraphVertices - 1);

            while (!foundAUniqueVertex)
            {
                foreach (var otherBee in otherEmployedBees)
                {
                    if (vertexToChoose == otherBee.numOfSelectedVertex)
                    {
                        vertexToChoose = (new Random()).Next(0, numOfGraphVertices - 1);
                    }
                }

                foreach (var otherBee in otherEmployedBees)
                {
                    if (vertexToChoose == otherBee.numOfSelectedVertex)
                    {
                        vertexToChoose = (new Random()).Next(0, numOfGraphVertices - 1);
                    }
                    else
                    {
                        foundAUniqueVertex = true;
                    }
                }
            }
            

            return vertexToChoose;
        }

        public int CountAmountOfNectarInAGivenVertex(int vertexNum, int[,] adjMatrix)
        {
            if (vertexNum < 0)
            {
                throw new ArgumentException("Vertex can't be less than 0");
            }

            int columns = adjMatrix.GetUpperBound(1) + 1;
            int degreeCounter = 0;

            for (int i = 0; i < columns; i++)
            {
                if (adjMatrix[vertexNum, i] == 1)
                {
                    degreeCounter++;
                }
            }

            return degreeCounter;
        }
    }
}
