using System;
using System.Linq;
using System.Collections.Generic;


namespace _4lab_travelingSalesmanABC
{
    public static class Algorithms
    {
        public static void ArtificialBeeColonyTSP(int[,] AdjMatrixOfAGraph, int numOfEmployedBees, int numOfOnLookerBees)
        {
            int numOfCities = AdjMatrixOfAGraph.GetUpperBound(0) + 1;
            Cities cities = new Cities(numOfCities, AdjMatrixOfAGraph);

            List<Vertex> allVertices = new List<Vertex>(numOfCities);

            //Ініціалізуємо вершини
            for (int i = 0; i < numOfCities; i++)
            {
                Vertex vertex = new(i, cities);
                allVertices.Add(vertex);
            }

            List<EmployedBee> employedBees = new();
            List<EmployedBee> tempEmployedBees = new();
            List<int> visitedCities = new();

            // Initializing each of employed bees
            // Задаємо кожній зайнятій бджілці номер вершини та
            // її кількість нектару (к-сть вершин : найкоротший шлях від вибраної вершини до будь-якої доступної)
            for (int i = 0; i < numOfEmployedBees; i++)
            {
                EmployedBee employedBee = new(cities, numOfCities, visitedCities);
                employedBees.Add(employedBee);
            }

            int numOfEmployedBeesThatVisitedAllCities = 0;

            while (numOfEmployedBeesThatVisitedAllCities != numOfEmployedBees)
            {
                foreach (var bee in tempEmployedBees)
                {
                    employedBees.Add(bee);
                }
                tempEmployedBees.Clear();

                // Визначаємо скільки фуражирів відправимо в усі вибрані розвідниками вершини  
                List<EmployedBee> MostProfitableEmployedBees = GetTheMostProfitableEmployedBees(employedBees, numOfCities, AdjMatrixOfAGraph, numOfEmployedBees);

                var PiValuesForEachVertex =
                   CountPiForEachSelectedVertex2(MostProfitableEmployedBees);

                var amountOfOnLookerBeesForEachVertex =
                    CountNumOfOnLookerBeesForEachSelectedVertex(PiValuesForEachVertex, numOfOnLookerBees);
                
                foreach (var bee in MostProfitableEmployedBees)
                {
                    var vertex = allVertices.Single(v => v.VertexNum == bee.numOfSelectedVertex);

                    // Согласно формуле Pi распределяем фуражиров по вершинам,
                    // по ключу отримуємо кількість фуражирів для обраної розвідником вершини
                    int numOfOnLookerBeesForThisVertex = GetNumOfOnLookerBeesBasedOnVertexNum(vertex.VertexNum, amountOfOnLookerBeesForEachVertex);
                    vertex.onLookerBeesAtThisVertex = new();

                    var citiesToVisitForOnLookerBees =
                        GetCititesToGoForOnLookerBees(numOfOnLookerBeesForThisVertex, vertex.VertexNum, AdjMatrixOfAGraph);

                    // Ініціалізуємо усіх фуражирів, присваюємо їм номер суміжної вершини поточної
                    for (int i = 0; i < numOfOnLookerBeesForThisVertex; i++)
                    {
                        OnLookerBee onLookerBee = new(numOfCities, bee.VisitedCities, citiesToVisitForOnLookerBees[i]);
                        vertex.onLookerBeesAtThisVertex.Add(onLookerBee);
                    }

                    //Умови закінчення роботи алгоритму
                    if (bee.VisitedCities.Count + 1 >= numOfCities)
                    {
                        numOfEmployedBeesThatVisitedAllCities++;
                    }
                    if (numOfEmployedBeesThatVisitedAllCities == numOfEmployedBees)
                    {
                        tempEmployedBees.Clear();
                        
                        foreach (var mpBee in MostProfitableEmployedBees)
                        {
                            tempEmployedBees.Add(mpBee);
                        }
                        break;
                    }

                    foreach (var onLookerBee in vertex.onLookerBeesAtThisVertex)
                    {
                        for (int i = 0; i < numOfEmployedBees; i++)
                        {
                            EmployedBee employedBee = new(cities, numOfCities, onLookerBee.VisitedCities);
                            tempEmployedBees.Add(employedBee);
                        }
                    }

                    vertex.onLookerBeesAtThisVertex.Clear();
                    bee.isActive = false;
                }

                if (numOfEmployedBeesThatVisitedAllCities != numOfEmployedBees)
                {
                    for (int i = 0; i < employedBees.Count; i++)
                    {
                        if (!employedBees[i].isActive)
                        {
                            employedBees.Remove(employedBees[i]);
                            i = i - 1;
                        }
                    }
                }
            }

            EmployedBee solution = GetEmployedBeeWithTheBestSolution(tempEmployedBees, cities);
            cities.Solution = solution.VisitedCities.ToArray();
            
            Console.WriteLine(cities);
            Console.WriteLine($"\nFull route distance: {solution.FullRouteDistance}");
        }


        private static List<EmployedBee> GetTheMostProfitableEmployedBees(List<EmployedBee> employedBees, int citiesNum, int[,] AdjMatrix, int numOfEmployedBees)
        {
            Cities cities = new Cities(citiesNum, AdjMatrix);
            List<EmployedBee> mostProfitableEmployedBees = new List<EmployedBee>(numOfEmployedBees);

            Dictionary<int, int> vertexNumAndFullDistance = new Dictionary<int, int>(employedBees.Count);
            int RouteTotalLength = 0;

            foreach(var bee in employedBees)
            {
                if (vertexNumAndFullDistance.ContainsKey(bee.numOfSelectedVertex))
                {
                    continue;
                }

                for (int i = 0; i < bee.VisitedCities.Count - 1; i++)
                {
                    RouteTotalLength += cities.CountDistanceBetweenTwoCities(bee.VisitedCities[i], bee.VisitedCities[i + 1]);
                }
                vertexNumAndFullDistance.Add(bee.numOfSelectedVertex, RouteTotalLength);
                RouteTotalLength = 0;
            }

            Dictionary<int, int> sortedDict = new();

            foreach (KeyValuePair<int, int> vertexAndFullDistance in vertexNumAndFullDistance.OrderBy(key => key.Value))
            {
                sortedDict.Add(vertexAndFullDistance.Key, vertexAndFullDistance.Value);
            }

            int arrCounter = 0;

            foreach (var keyValue in sortedDict)
            {
                if (arrCounter == mostProfitableEmployedBees.Capacity)
                {
                    break;
                }

                mostProfitableEmployedBees.Add(employedBees.First(v => v.numOfSelectedVertex == keyValue.Key));
                arrCounter++;
            }

            employedBees.Clear();
            return mostProfitableEmployedBees;
        }

        private static EmployedBee GetEmployedBeeWithTheBestSolution(List<EmployedBee> employedBees, Cities cities)
        {
            Dictionary<int, int> vertexNumFullDistance = new(employedBees.Count);

            foreach (var bee in employedBees)
            {
                vertexNumFullDistance.Add(bee.numOfSelectedVertex, CountFullRouteDistance(bee.VisitedCities.ToArray(), cities));
            }

            Dictionary<int, int> sortedDict = new();

            foreach (KeyValuePair<int, int> vertexAndFullDistance in vertexNumFullDistance.OrderBy(key => key.Value))
            {
                sortedDict.Add(vertexAndFullDistance.Key, vertexAndFullDistance.Value);
            }

            EmployedBee employedBeeToReturn = employedBees.First(b => b.numOfSelectedVertex == sortedDict.First().Key);
            employedBeeToReturn.FullRouteDistance = sortedDict.First().Value;

            return employedBeeToReturn;
        }

        private static int CountFullRouteDistance(int[] solution, Cities cities)
        {
            int RouteTotalLength = 0;

            for (int i = 0; i < solution.Length - 1; i++)
            {
                RouteTotalLength += cities.CountDistanceBetweenTwoCities(solution[i], solution[i + 1]);
            }

            RouteTotalLength += cities.CountDistanceBetweenTwoCities(solution[solution.Length - 1], solution[0]);

            return RouteTotalLength;
        }

        private static int GetNumOfOnLookerBeesBasedOnVertexNum(int VertexNum, int[] amountOfOnLookerBeesForEachVertex)
        {
            int NumOfOnLookerBees = 0;

            for (int i = 0; i < amountOfOnLookerBeesForEachVertex.Length; i++)
            {
                if (VertexNum == amountOfOnLookerBeesForEachVertex[i] && (i % 2 == 0))
                {
                    NumOfOnLookerBees = amountOfOnLookerBeesForEachVertex[i + 1];
                }
            }

            return NumOfOnLookerBees;
        }

        // Pi -> Possibility of moving to one of the vertices for one of the onLooker bees
        private static double[] CountPiForEachSelectedVertex2(List<EmployedBee> employedBees)
        {
            int counter = 0;
            int AmountOfNectarInAllSelectedVerteces = 0;
            foreach (var bee in employedBees)
            {
                AmountOfNectarInAllSelectedVerteces += bee.amountOfNectarInSelectedVertex;
            }

            double[] PiValues = new double[employedBees.Count * 2];

            foreach (var bee in employedBees)
            {
                double Pi = (double)bee.amountOfNectarInSelectedVertex / (double)AmountOfNectarInAllSelectedVerteces;
                PiValues[counter] = bee.numOfSelectedVertex; 
                counter++;

                PiValues[counter] = Pi;
                counter++;
            }

            return PiValues;
        }

        private static int[] CountNumOfOnLookerBeesForEachSelectedVertex(double[] piValuesForEachVertex, int numOfOnLookerBees)
        {
            int[] NumOfOnLookerBeesForEachVertex = new int[piValuesForEachVertex.Length];

            for (int i = 0; i < piValuesForEachVertex.Length; i++)
            {
                NumOfOnLookerBeesForEachVertex[i] = (int)piValuesForEachVertex[i];
                i++;

                NumOfOnLookerBeesForEachVertex[i] = (int)Math.Round(numOfOnLookerBees * piValuesForEachVertex[i]);
            }

            return NumOfOnLookerBeesForEachVertex;
        }

        private static int[] GetCititesToGoForOnLookerBees(int numOfOnLookerBees, int cityNum, int[,] adjMatrix)
        {
            int[] possibleRoutes = ConvertMatrixRowToArray(cityNum, adjMatrix);
            int[] distancesOfNeededRoutes = ConvertMatrixRowToArray(cityNum, adjMatrix);
            Array.Sort(distancesOfNeededRoutes);

            int[] numsOfCitiesToGo = new int[numOfOnLookerBees];


            for (int i = 1; i < numsOfCitiesToGo.Length + 1; i++)
            {
                for (int j = 0; j < distancesOfNeededRoutes.Length; j++)
                {
                    if (distancesOfNeededRoutes[i] == possibleRoutes[j] && j != cityNum && !isAlreadyAdded(numsOfCitiesToGo, j))
                    {
                        numsOfCitiesToGo[i - 1] = j;
                        break;
                    }
                }
            }

            return numsOfCitiesToGo;
        } 

        private static bool isAlreadyAdded(int[] currentArray, int vertexToAdd)
        {
            bool isAdded = false;
            for (int i = 0; i < currentArray.Length; i++)
            {
                if (vertexToAdd == currentArray[i])
                {
                    isAdded = true;
                    break;
                }
            }

            return isAdded;
        }

        public static int[] ConvertMatrixRowToArray(int rowNum, int[,] AdjMatrix)
        {
            int arrLength = AdjMatrix.GetUpperBound(0) + 1;

            int[] arr = new int[arrLength];
            for (int i = 0; i < arrLength; i++)
            {
                arr[i] = AdjMatrix[rowNum, i];
            }

            return arr;
        }
    }
}
