using System;
using System.Collections.Generic;
using System.Text;

namespace _4lab_travelingSalesmanABC
{
    public class Cities
    {
     
        public int[] Solution { get; set; }
        
        private readonly int citiesNum;
        private readonly int[,] AdjMatrix;

        public Cities(int citiesNum, int[,] AdjMatrix)
        {
            this.citiesNum = citiesNum;
            this.AdjMatrix = AdjMatrix;
            this.Solution = new int[citiesNum];
        }


        // key equals to the city (vertex) num, value equals to the distance
        public Dictionary<int,int> GetCityNumWithTheLowestPathFromChosen(int chosenCityNum)
        {
            Dictionary<int, int> cityWithTheLowestPathToGo = new Dictionary<int, int>(1);

            int tempCityNum = this.AdjMatrix[chosenCityNum, 0] == 0 ? 1 : 0; 
            int tempDistance = this.AdjMatrix[chosenCityNum, tempCityNum];

            for (int i = 0; i < this.citiesNum - 1; i++)
            { 
                if (this.AdjMatrix[chosenCityNum, tempCityNum] > this.AdjMatrix[chosenCityNum, i + 1] && chosenCityNum != i + 1)
                {
                    tempCityNum = i + 1;
                    tempDistance = this.AdjMatrix[chosenCityNum, i + 1];
                }
            }

            cityWithTheLowestPathToGo.Add(tempCityNum, tempDistance);
            return cityWithTheLowestPathToGo;
        }

        public int CountDistanceBetweenTwoCities(int FirstCity, int SecondCity)
        {
            if (AdjMatrix[FirstCity, SecondCity] != AdjMatrix[SecondCity, FirstCity])
            {
                throw new Exception("Algorithm is not working properly.");
            }

            return AdjMatrix[FirstCity, SecondCity];
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder("Path: ");

            for (int i = 0; i < Solution.Length; i++)
            {
                if (i == Solution.Length - 1)
                {
                    output.Append($"{this.Solution[i]}");
                    continue;
                }
                output.Append($"{this.Solution[i]}" + "-> ");
            }

            return output.ToString(); 
        }
    }
}
