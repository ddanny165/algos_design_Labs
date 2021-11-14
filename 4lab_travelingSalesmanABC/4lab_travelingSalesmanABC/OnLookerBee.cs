using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4lab_travelingSalesmanABC
{
    public class OnLookerBee
    {
        public int currentCityNum { get; set; }
        // 
        public List<int> VisitedCities = new();

        public OnLookerBee(int citiesNum, List<int> VisitedCities, int curCity)
        {
            this.currentCityNum = curCity;

            for (int i = 0; i < VisitedCities.Count; i++)
            {
                this.VisitedCities.Add(VisitedCities[i]);
            }
            this.VisitedCities.Add(curCity);
        }
    }
}
