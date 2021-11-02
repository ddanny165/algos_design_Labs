using System;
using System.Collections.Generic;
using System.Linq;

namespace _3lab_vertexColoringABC
{
    public static class Algorithms
    {
        private static int IterationsCounter = 0;
        public static int ArtificialBeeColonyGraphColoring(int[,] AdjMatrixOfAGraph, int numOfEmployedBees, int numOfOnLookerBees)
        {
            int numberOfVerticesOfAGivenGraph = AdjMatrixOfAGraph.GetUpperBound(0) + 1;
            List<Vertex> allVertices = new List<Vertex>(numberOfVerticesOfAGivenGraph);

            //Ініціалізуємо вершини
            for (int i = 0; i < numberOfVerticesOfAGivenGraph; i++)
            {
                Vertex vertex = new(i, AdjMatrixOfAGraph);
                allVertices.Add(vertex);
            }

            List<EmployedBee> employedBees = new();

            // число пофарбованих вершин 
            int coloredVertices = 0;
            
            // Initializing all colors (from 1 to 50)
            int[] AllColors = initializeColors();
            List<int> UsedColors = new();

            // Поки не будуть пофарбовані усі вершини
            while (coloredVertices != numberOfVerticesOfAGivenGraph)
            {
                IterationsCounter++;
                int[] iterationsToCheck = { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400,
                420, 440, 460, 480, 500, 520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900,
                920, 940, 960, 980, 1000};
                
                for (int i = 0; i < iterationsToCheck.Length; i++)
                {
                    if (IterationsCounter == iterationsToCheck[i])
                    {
                        Console.WriteLine($"iteration #{IterationsCounter}: usedColors = {UsedColors.Count}, numOfColored = {coloredVertices}");
                    }
                }

                coloredVertices = 1;
                if (employedBees.Count > 0)
                {
                    employedBees.Clear();
                }

                //Initializing each of employed bees
                for (int i = 0; i < numOfEmployedBees; i++)
                {
                    EmployedBee employedBee = new();
                    employedBees.Add(employedBee);
                }

                // Задаємо кожній зайнятій бджілці номер вершини та її кількість нектару (тобто степінь вершини) 
                foreach (var bee in employedBees)
                {
                    bee.numOfSelectedVertex =
                        bee.ChooseARandomVertex(numberOfVerticesOfAGivenGraph, employedBees);

                    bee.amountOfNectarInSelectedVertex =
                        bee.CountAmountOfNectarInAGivenVertex(bee.numOfSelectedVertex, AdjMatrixOfAGraph);
                }

                // Визначаємо скільки фуражирів відправимо в усі вибрані розвідниками вершини  
                Dictionary<int, double> PiValuesForEachVertex =
                   CountPiForEachSelectedVertex(employedBees);

                Dictionary<int, int> amountOfOnLookerBeesForEachVertex =
                    CountNumOfOnLookerBeesForEachSelectedVertex(PiValuesForEachVertex, numOfOnLookerBees);

                foreach (var bee in employedBees)
                {
                    var vertex = allVertices.Single(v => v.VertexNum == bee.numOfSelectedVertex);

                    // Согласно формуле Pi распределяем фуражиров по вершинам,
                    // по ключу отримуємо кількість фуражирів для обраної розвідником вершини
                    int numOfOnLookerBeesForThisVertex = amountOfOnLookerBeesForEachVertex[vertex.VertexNum];
                    vertex.onLookerBeesAtThisVertex = new();

                    // Ініціалізуємо усіх фуражирів, присваюємо їм номер суміжної вершини поточної
                    for (int i = 0; i < numOfOnLookerBeesForThisVertex; i++)
                    {
                        OnLookerBee onLookerBee = new(vertex.AdjacentVertexes[i]);
                        vertex.onLookerBeesAtThisVertex.Add(onLookerBee);
                    }

                    // Розмальовуємо усі суміжні вершини із вибраними вершинами робочими бджмілками 
                    foreach (var onLookerBee in vertex.onLookerBeesAtThisVertex)
                    {
                        // Якщо у вершини фуражира є нектар = красимо її в перший доступний колір
                        if (allVertices[onLookerBee.VertexNum].NumOfNectar != 0)
                        {
                            allVertices[onLookerBee.VertexNum].ColorNum = AllColors[0];
                        }

                        bool foundAColorFromUsedColors = false;

                        // Перевіряємо присвоєні кольора із сусідніми вершинами,
                        // додаємо їх до масиву UsedColors, якщо вони будуть допустимі
                        if (IsSafe(allVertices[onLookerBee.VertexNum], allVertices))
                        {
                            if (CheckIfNotContainsColor(allVertices[onLookerBee.VertexNum].ColorNum, UsedColors))
                            {
                                UsedColors.Add(allVertices[onLookerBee.VertexNum].ColorNum);
                            }
                            foundAColorFromUsedColors = true;
                        }

                        // Якщо не допустимі кольора, пробуємо присвоїти інший колір із масиву UsedColors
                        else if (!IsSafe(allVertices[onLookerBee.VertexNum], allVertices))
                        {
                            foreach (var color in UsedColors)
                            {
                                allVertices[onLookerBee.VertexNum].ColorNum = color;
                                if (IsSafe(allVertices[onLookerBee.VertexNum], allVertices))
                                {
                                    if (CheckIfNotContainsColor(allVertices[onLookerBee.VertexNum].ColorNum, UsedColors))
                                    {
                                        UsedColors.Add(allVertices[onLookerBee.VertexNum].ColorNum);
                                    }

                                    foundAColorFromUsedColors = true;
                                    break;
                                }
                            }
                        }

                        // Якщо попередні 2 етапи не допомогли, вибираємо перший доступний колір із AllColors
                        // та додаємо його до масиву використаних кольорів (UsedColors)
                        if (!foundAColorFromUsedColors)
                        {
                            allVertices[onLookerBee.VertexNum].ColorNum += 1;

                            if (CheckIfNotContainsColor(allVertices[onLookerBee.VertexNum].ColorNum, UsedColors))
                            {
                                UsedColors.Add(allVertices[onLookerBee.VertexNum].ColorNum);
                            }
                        }
                    }

                    // Після розмалювання усіх суміжних - розмалюємо самі вершини і встановимо їх нектар нулем
                    bool foundAColorForVertex = false;

                    foreach (var color in UsedColors)
                    {
                        vertex.ColorNum = color;
                        if (IsSafe(vertex, allVertices))
                        {
                            vertex.NumOfNectar = 0;
                            foundAColorForVertex = true;
                            break;
                        }
                    }

                    if (!foundAColorForVertex)
                    {
                        vertex.ColorNum = AllColors[UsedColors.Count];
                        UsedColors.Add(vertex.ColorNum);
                        vertex.NumOfNectar = 0;
                    }

                }

                // Зменшуємо кількість фуражирів, оскільки 5 (згідно з варіантом) із них умовно стали розвідниками
                numOfOnLookerBees -= numOfEmployedBees;

                // Підраховуємо кількість усіх пофарбованих вершин 
                foreach (var vertex in allVertices)
                {
                    if (vertex.ColorNum != 0)
                    {
                        coloredVertices++;
                    }
                }
            }

            Console.WriteLine($"Total iterations: {IterationsCounter}\n");

            // Перевіряємо чи всі safe (дві суміжні вершини не мають спільного кольору)
            bool allColoredAreSafe = CheckIfAllColoredVerticesAreSafe(allVertices);
            
            if (allColoredAreSafe)
            {
                Console.WriteLine("All colored vertices are safe.");
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }

            return UsedColors.Count;
        }


        // Pi -> Possibility of moving to one of the vertices for one of the onLooker bees
        private static Dictionary<int, double> CountPiForEachSelectedVertex(List<EmployedBee> employedBees)
        {
            int AmountOfNectarInAllSelectedVerteces = 0;
            foreach (var bee in employedBees)
            {
                AmountOfNectarInAllSelectedVerteces += bee.amountOfNectarInSelectedVertex;
            }

            Dictionary<int, double> PiValues = new Dictionary<int, double>(employedBees.Count);

            foreach (var bee in employedBees)
            {
                double Pi = (double)bee.amountOfNectarInSelectedVertex / (double)AmountOfNectarInAllSelectedVerteces;
                PiValues.Add(bee.numOfSelectedVertex, Pi);
            }

            return PiValues;
        }
        
        private static Dictionary<int, int> CountNumOfOnLookerBeesForEachSelectedVertex(Dictionary<int, double> piValuesForEachVertex, int numOfOnLookerBees)
        {
            Dictionary<int, int> NumOfOnLookerBeesForEachVertex = new Dictionary<int, int>(piValuesForEachVertex.Count);

            foreach (var keyValue in piValuesForEachVertex)
            {
                int numOfOnLookerBeesForGivenVertex = (int)Math.Round(numOfOnLookerBees * keyValue.Value);

                NumOfOnLookerBeesForEachVertex.Add(keyValue.Key, numOfOnLookerBeesForGivenVertex);
            }

            int checkingNumOfOnLookerBees = 0;
            foreach (var keyValue in NumOfOnLookerBeesForEachVertex)
            {
                checkingNumOfOnLookerBees += keyValue.Value;
            }
            
            return NumOfOnLookerBeesForEachVertex;
        }
        
        private static int[] initializeColors()
        {
            int[] colors = new int[50];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = i + 1;
            }

            return colors;
        }

        private static bool IsSafe(Vertex vertex, List<Vertex> allVertices)
        {
            bool isSafe = true;
            int adjVerticesNum = vertex.AdjacentVertexes.Count;

            List<Vertex> adjVertices = new(adjVerticesNum);

            // adding all adjacent vertices to adjVertices
            for (int i = 0; i < adjVerticesNum; i++)
            {
                var adjVertex = allVertices.Single(v => v.VertexNum == vertex.AdjacentVertexes[i]);
                adjVertices.Add(adjVertex);
            }

            for (int i = 0; i < adjVerticesNum; i++) 
            {
                //checking if neighbour has same color.
                if (vertex.ColorNum == adjVertices[i].ColorNum) 
                {
                    isSafe = false; 
                    break;
                }
            }

            return isSafe;
        }

        private static bool CheckIfNotContainsColor(int colorNum, List<int> usedColors)
        {
            bool notContainsColor = true;

            for (int i = 0; i < usedColors.Count; i++)
            {
                if (colorNum == usedColors[i])
                {
                    notContainsColor = false;
                    break;
                }
            }

            return notContainsColor;
        }

        private static bool CheckIfAllAdjacentVerticesAreColored(Vertex vertex, List<Vertex> allVertices)
        {
            bool AreColored = true;
            int adjVerticesNum = vertex.AdjacentVertexes.Count;

            List<Vertex> adjVertices = new(adjVerticesNum);

            // adding all adjacent vertices to adjVertices
            for (int i = 0; i < adjVerticesNum; i++)
            {
                var adjVertex = allVertices.Single(v => v.VertexNum == vertex.AdjacentVertexes[i]);
                adjVertices.Add(adjVertex);
            }


            for (int i = 0; i < adjVerticesNum; i++)
            {
                //checking if all neighbours are colored.
                if (adjVertices[i].ColorNum == 0)
                {
                    AreColored = false;
                    break;
                }
            }

            return AreColored;
        }

        public static bool CheckIfAllColoredVerticesAreSafe(List<Vertex> coloredVertices)
        {
            bool allSafe = true;

            foreach (var vertex in coloredVertices)
            {
                if (vertex.VertexNum != 299 && !IsSafe(vertex, coloredVertices))
                {
                    allSafe = false;
                }
            }

            return allSafe;
        }

        public static List<int> GetAdjacentVertexes(int vertexNum, int[,] adjMatrix)
        {
            if (vertexNum < 0)
            {
                throw new ArgumentException("Vertex can't be less than 0");
            }

            List<int> adjacentVertexes = new();

            int columns = adjMatrix.GetUpperBound(1) + 1;

            for (int i = 0; i < columns; i++)
            {
                if (adjMatrix[vertexNum, i] == 1)
                {
                    adjacentVertexes.Add(i);
                }
            }

            return adjacentVertexes;
        }
    }
}
