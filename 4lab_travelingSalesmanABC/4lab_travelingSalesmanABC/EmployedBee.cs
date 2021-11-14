using System;
using System.Linq;
using System.Collections.Generic;


namespace _4lab_travelingSalesmanABC
{
    public class EmployedBee
    {
        public bool isActive { get; set; }
        public int numOfSelectedVertex { get; set; }
        public int amountOfNectarInSelectedVertex { get; set; }
        public List<int> VisitedCities { get; private set; }
        public int FullRouteDistance { get; set; }
        public List<EmployedBee> otherEmployedBees { get; private set; }

        public int ChooseARandomVertex(int numOfGraphVertices, List<int> VisitedCities)
        {
            bool foundAUniqueVertexAmongVisited = false;

            int notVisitedCount = 0;
            int vertexToChoose = (new Random()).Next(0, numOfGraphVertices - 1);

            if (VisitedCities != null && VisitedCities.Count != 0)
            {
                while (!foundAUniqueVertexAmongVisited)
                {
                    foreach (var city in VisitedCities)
                    {
                        if (vertexToChoose == city)
                        {
                            vertexToChoose = (new Random()).Next(0, numOfGraphVertices - 1);
                        }
                    }

                    foreach (var city in VisitedCities)
                    {
                        if (vertexToChoose != city)
                        {
                            notVisitedCount++;
                        }
                    }

                    if (notVisitedCount == VisitedCities.Count)
                    {
                        foundAUniqueVertexAmongVisited = true;
                    }

                    notVisitedCount = 0;
                }
            }

            return vertexToChoose;
        }

        public EmployedBee()
        {

        }

        public EmployedBee(Cities cities, int citiesNum, List<int> VisitedCities)
        {
            this.isActive = true;
            this.otherEmployedBees = otherEmployedBees;
            this.VisitedCities = VisitedCities;
            this.numOfSelectedVertex = ChooseARandomVertex(citiesNum, this.VisitedCities);
            this.amountOfNectarInSelectedVertex = cities.Solution.Length / cities.GetCityNumWithTheLowestPathFromChosen(this.numOfSelectedVertex).Values.First();

            this.VisitedCities.Add(this.numOfSelectedVertex);
        }
    }
}
