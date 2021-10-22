using System;
using System.Linq;
using System.Collections.Generic;
using _2lab_denseIndex.dbRecords;

namespace _2lab_denseIndex
{
    public static class Algorithms
    {
        public static int UniformBinarySearch(int[] sortedArrOfAllPKs, int target)
        {
            FilesRepository files = new();
            var addedValuesPK = files.readIndexReg();

            int[] valuesInFreeBlocks = numOfElemInFreeSpaceBlocks(addedValuesPK);

            int blockNum = getBlockNumByAPrimaryKey(target);
            int[] pkValuesByBlock = getArrayAccordingToBlockNum(sortedArrOfAllPKs, target, blockNum);

            //algo by itself
            int N = pkValuesByBlock.Length;

            int index = N / 2 + 1;
            double delta = (double)(N / 2);

            int resultOfSearch = 0;
            int notFoundFlag = 0;

            while (delta != 0 && index > 0)
            {
                if (index >= N)
                {
                    index = N - 1;
                    notFoundFlag++;
                }

                if (notFoundFlag == 2)
                {
                    return -1; //not found flag, 
                }

                if (pkValuesByBlock[index] == target)
                {
                    resultOfSearch = index;
                    break;
                }
                else if (pkValuesByBlock[index] < target)
                {
                    // Ключ (елемент масиву) менше шуканого
                    index += (int)(Math.Round(delta / 2) + 1);
                    delta /= 2;
                }
                else
                {
                    // Ключ (елемент масиву) більше шуканого
                    index -= (int)(Math.Round(delta / 2) + 1);
                    delta /= 2;
                }
            }

            if (resultOfSearch == 0 && target != 1)
            {
                return -1; //not found case
            }

            if (blockNum != 6)
            {
                resultOfSearch += (blockNum - 1) * 100;
            }

            switch (blockNum)
            {
                case 2:
                    resultOfSearch += 2 * valuesInFreeBlocks[0];
                    break;
                case 3:
                    resultOfSearch += 2 * (valuesInFreeBlocks[1] + valuesInFreeBlocks[0]);
                    break;
                case 4:
                    resultOfSearch += 2 * (valuesInFreeBlocks[2] + valuesInFreeBlocks[1] + valuesInFreeBlocks[0]);
                    break;
                case 5:
                    resultOfSearch += 2 * (valuesInFreeBlocks[3] + valuesInFreeBlocks[2] + valuesInFreeBlocks[1] + valuesInFreeBlocks[0]);
                    break;
                case 6:
                    
                    break;
            }

            return resultOfSearch;
        }

        //just to test my UniformBinarySearch
        public static int binarySearch(int[] sortedArr, int target)
        {
            if (target > sortedArr[sortedArr.Length - 1])
            {
                return -1;
            }

            int debug = UniformBinarySearch(sortedArr, target);

            int blockNum = getBlockNumByAPrimaryKey(target);
            int[] borders = getBlockBordersByBlockNum(blockNum, sortedArr.Length - 1);

            int left = borders[0];
            int right = borders[1];

            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (target == sortedArr[mid])
                {
                     return mid;
                }
                else if (target < sortedArr[mid])
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            return -1; //target is not found case
        }

        public static int[] getArrayAccordingToBlockNum(int[] sortedArr, int target, int blockNum)
        {
            List<int> pkValuesByBlock = new();
            int[] borders = getBlockBordersByBlockNum(blockNum, sortedArr.Length - 1);

            int left = borders[0];
            int right = borders[1];

            for (int i = left; i < right; i++)
            {
                pkValuesByBlock.Add(sortedArr[i]);
            }

            return pkValuesByBlock.ToArray();
        }

        public static int getBlockNumByAPrimaryKey(int primaryKey)
        {
            if (primaryKey <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(primaryKey));
            }


            if (primaryKey > 0 && primaryKey <= 110)
            {
                return 1;
            }
            else if (primaryKey > 110 && primaryKey <= 220)
            {
                return 2;
            }
            else if (primaryKey > 220 && primaryKey <= 330)
            {
                return 3;
            }
            else if (primaryKey > 330 && primaryKey <= 440)
            {
                return 4;
            }
            else if (primaryKey > 440 && primaryKey <= 550)
            {
                return 5;
            }
            else
            {
                return 6; //overflow reg (block of index reg)
            }
        }

        public static int[] getBlockBordersByBlockNum(int blockNum, int lastPK)
        {
            if (blockNum <= 0 || blockNum > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(blockNum));
            }

            FilesRepository fileManip = new();
            List<IndexRegDBRecord> indexRegDBRecords = fileManip.readIndexReg();

            int[] numOfElemInFreeSpace = getNumOfElementsInFreeSpaceBlock(indexRegDBRecords);
            int[] borders = new int[2];

            switch (blockNum)
            {
                case 1:
                    borders[0] = 0;

                    borders[1] = getAmountOfRecordsInAGivenBlock(blockNum, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 1];
                    break;


                case 2:
                    borders[0] = getAmountOfRecordsInAGivenBlock(blockNum - 1, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 2];

                    borders[1] = borders[0] + getAmountOfRecordsInAGivenBlock(blockNum, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 1];
                    break;


                case 3:
                    borders[0] = getAmountOfRecordsInAGivenBlock(blockNum - 2, indexRegDBRecords) +
                        getAmountOfRecordsInAGivenBlock(blockNum - 1, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 3] + numOfElemInFreeSpace[blockNum - 2];

                    borders[1] = borders[0] + getAmountOfRecordsInAGivenBlock(blockNum, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 1];
                    break;


                case 4:
                    borders[0] = getAmountOfRecordsInAGivenBlock(blockNum - 3, indexRegDBRecords) + 
                        getAmountOfRecordsInAGivenBlock(blockNum - 2, indexRegDBRecords) + 
                        getAmountOfRecordsInAGivenBlock(blockNum - 1, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 4] + 
                        numOfElemInFreeSpace[blockNum - 3] + numOfElemInFreeSpace[blockNum - 2];

                    borders[1] = borders[0] + getAmountOfRecordsInAGivenBlock(blockNum, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 1];
                    break;


                case 5:
                    borders[0] = getAmountOfRecordsInAGivenBlock(blockNum - 4, indexRegDBRecords) + 
                        getAmountOfRecordsInAGivenBlock(blockNum - 3, indexRegDBRecords) +
                        getAmountOfRecordsInAGivenBlock(blockNum - 2, indexRegDBRecords) + 
                        getAmountOfRecordsInAGivenBlock(blockNum - 1, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 5] + 
                        numOfElemInFreeSpace[blockNum - 4] + numOfElemInFreeSpace[blockNum - 3] + numOfElemInFreeSpace[blockNum - 2];

                    borders[1] = borders[0] + getAmountOfRecordsInAGivenBlock(blockNum, indexRegDBRecords) + numOfElemInFreeSpace[blockNum - 1];
                    break;


                case 6:
                    borders[0] = 0;
                    borders[1] = lastPK + 1;
                    break;

                default:
                    throw new ArgumentException(nameof(blockNum));
            }

            return borders; 
        }

        public static int getAmountOfRecordsInAGivenBlock(int blockNum, List<IndexRegDBRecord> indexRegDBRecords)
        {
            int AmountOfRecordsCounter = 0;

            for (int i = 0; i < indexRegDBRecords.Count; i++)
            {
                if (indexRegDBRecords[i].blockNum == blockNum)
                {
                    AmountOfRecordsCounter++;
                }
            }

            return AmountOfRecordsCounter;
        }

        public static int[] numOfElemInFreeSpaceBlocks(List<IndexRegDBRecord> indexRegDBRecords)
        {
            int[] numOfElem = new int[6];

            for (int i = 0; i < indexRegDBRecords.Count; i++)
            {
                if (indexRegDBRecords[i].employeeID >= 101 && indexRegDBRecords[i].employeeID <= 110)
                {
                    numOfElem[0]++;
                }

                if (indexRegDBRecords[i].employeeID >= 211 && indexRegDBRecords[i].employeeID <= 220)
                {
                    numOfElem[1]++;
                }

                if (indexRegDBRecords[i].employeeID >= 321 && indexRegDBRecords[i].employeeID <= 330)
                {
                    numOfElem[2]++;
                }

                if (indexRegDBRecords[i].employeeID >= 431 && indexRegDBRecords[i].employeeID <= 440)
                {
                    numOfElem[3]++;
                }

                if (indexRegDBRecords[i].employeeID >= 541 && indexRegDBRecords[i].employeeID <= 550)
                {
                    numOfElem[4]++;
                }

                if (indexRegDBRecords[i].employeeID >= 551 && indexRegDBRecords[i].employeeID <= 2000)
                {
                    numOfElem[5]++;
                }
            }

            return numOfElem;
        }

        public static int[] getNumOfElementsInFreeSpaceBlock(List<IndexRegDBRecord> indexRegDBRecords)
        {
            int[] numOfElem = new int[5];

            for (int i = 0; i < indexRegDBRecords.Count; i++)
            {
                if (indexRegDBRecords[i].employeeID >= 101 && indexRegDBRecords[i].employeeID <= 110)
                {
                    numOfElem[0]++;
                }

                if (indexRegDBRecords[i].employeeID >= 211 && indexRegDBRecords[i].employeeID <= 220)
                {
                    numOfElem[1]++;
                }

                if (indexRegDBRecords[i].employeeID >= 321 && indexRegDBRecords[i].employeeID <= 330)
                {
                    numOfElem[2]++;
                }

                if (indexRegDBRecords[i].employeeID >= 431 && indexRegDBRecords[i].employeeID <= 440)
                {
                    numOfElem[3]++;
                }

                if (indexRegDBRecords[i].employeeID >= 541 && indexRegDBRecords[i].employeeID <= 5500)
                {
                    numOfElem[4]++;
                }
            }

            return numOfElem;
        }

        public static string getRandomSurname()
        {
            Random ran = new();
            int selector = ran.Next(0, 20);

            switch (selector)
            {
                case 0:
                    return "Moskaliuk";
                case 1:
                    return "Yaremchuk";
                case 2:
                    return "Romaniuk";
                case 3:
                    return "Miller";
                case 4:
                    return "Brownlow";
                case 5:
                    return "Kehrer";
                case 6:
                    return "Trump";
                case 7:
                    return "Durov";
                case 8:
                    return "Scott";
                case 9:
                    return "Riedel";
                case 10:
                    return "Wilson";
                case 11:
                    return "Elliot";
                case 12:
                    return "Smith";
                case 13:
                    return "Lambert";
                case 14:
                    return "El-Masri";
                case 15:
                    return "Flanigan";
                case 16:
                    return "Grohe";
                case 17:
                    return "Schantz";
                case 18:
                    return "Kennon";
                case 19:
                    return "Perez";
                case 20:
                    return "Franklin";
                default:
                    throw new ArgumentException(nameof(selector));
            }
        }

        public static MainRegDBRecord convertStringToMainRegRecord(string fileLine)
        {
            if (fileLine == null)
            {
                throw new ArgumentNullException(nameof(fileLine));
            }

            string[] mainRegDBRecordValues = fileLine.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

            if (mainRegDBRecordValues.Length != 4)
            {
                throw new ArgumentException(nameof(fileLine));
            }

            int increment = int.Parse(mainRegDBRecordValues[0]);
            int employeeId = int.Parse(mainRegDBRecordValues[1]);
            string surname = mainRegDBRecordValues[2];
            int isAvailable = int.Parse(mainRegDBRecordValues[3]);

            MainRegDBRecord mainRegRecord = new(increment, employeeId, surname, isAvailable);
            return mainRegRecord;
        }

        public static IndexRegDBRecord convertStringToIndexRegRecord(string fileLine)
        {
            if (fileLine == null)
            {
                throw new ArgumentNullException(nameof(fileLine));
            }

            string[] IndexRegDBRecordValues = fileLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (IndexRegDBRecordValues.Length != 3)
            {
                throw new ArgumentException(nameof(fileLine));
            }

            int employeeID = int.Parse(IndexRegDBRecordValues[0]);
            int recordNumPointer = int.Parse(IndexRegDBRecordValues[1]);

            IndexRegDBRecord indexRegRecord = new(employeeID, recordNumPointer);
            return indexRegRecord;
        }

        public static int convertStringToPrimaryKeyIntValue(string fileLine)
        {
            if (fileLine == null)
            {
                throw new ArgumentNullException(nameof(fileLine));
            }

            string[] IndexRegDBRecordValues = fileLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (IndexRegDBRecordValues.Length != 3)
            {
                throw new ArgumentException(nameof(fileLine));
            }

            return int.Parse(IndexRegDBRecordValues.FirstOrDefault());
        }
    }
}
