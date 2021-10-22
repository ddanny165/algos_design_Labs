using System.Collections.Generic;
using System.Linq;

namespace _2lab_denseIndex.dbRecords
{
    public class RegionsInitializator
    {
        public List<MainRegDBRecord> initiateMainReg()
        {
            List<MainRegDBRecord> mainRegDBRecords = new();

            //file entry point
            MainRegDBRecord firstMainReg = new MainRegDBRecord(1, 1, "Brown");
            mainRegDBRecords.Add(firstMainReg);

            setMainRegWithCycle(111, 150, mainRegDBRecords);
            setMainRegWithCycle(331, 380, mainRegDBRecords);
            setMainRegWithCycle(441, 490, mainRegDBRecords);
            setMainRegWithCycle(2, 50, mainRegDBRecords);
            setMainRegWithCycle(271, 320, mainRegDBRecords);
            setMainRegWithCycle(491, 540, mainRegDBRecords);
            setMainRegWithCycle(151, 210, mainRegDBRecords);
            setMainRegWithCycle(51, 100, mainRegDBRecords);
            setMainRegWithCycle(221, 270, mainRegDBRecords);
            setMainRegWithCycle(381, 430, mainRegDBRecords);

            return mainRegDBRecords;
        }

        public List<IndexRegDBRecord> initiateIndexReg(List<MainRegDBRecord> mainRegDBRecords)
        {
            List<IndexRegDBRecord> indexRegDBRecords = new();

            foreach(var record in mainRegDBRecords)
            {
                IndexRegDBRecord one = new(record);
                indexRegDBRecords.Add(one);
            }

            return indexRegDBRecords.OrderBy(b => b.blockNum).ThenBy(id => id.employeeID).ToList();
        }

        private void setMainRegWithCycle(int initPos, int endPos, List<MainRegDBRecord> mainRegDBRecords)
        {
            for (int i = initPos; i <= endPos; i++)
            {
                string randomName = Algorithms.getRandomSurname();
                int employeeID = i;
                int recordNumPointer = mainRegDBRecords[mainRegDBRecords.Count - 1].increment + 1;

                MainRegDBRecord one = new MainRegDBRecord(recordNumPointer, employeeID, randomName);
                mainRegDBRecords.Add(one);
            }
        }
    }
}
