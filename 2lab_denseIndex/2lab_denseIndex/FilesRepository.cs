using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using _2lab_denseIndex.dbRecords;

namespace _2lab_denseIndex
{
    class FilesRepository :ICRUD
    {
        //helper methods, which interact with files
        public void initialDatabase()
        {
            RegionsInitializator regInit = new();

            List<MainRegDBRecord> mainRegDBRecords = regInit.initiateMainReg();
            List<IndexRegDBRecord> indexRegDBRecords = regInit.initiateIndexReg(mainRegDBRecords);

            try
            {
                using (StreamWriter sw = new StreamWriter(Constants.mainRegPath, false, System.Text.Encoding.Default))
                {
                    foreach (var record in mainRegDBRecords)
                    {
                        string toWrite =
                            $"{record.increment},{record.employeeID},{record.surname},{record.isAvailable}";
                        sw.WriteLine(toWrite);
                    }
                }

                using (StreamWriter sw = new StreamWriter(Constants.indexRegPath, false, System.Text.Encoding.Default))
                {
                    foreach (var record in indexRegDBRecords)
                    {
                        string toWrite =
                            $"{record.employeeID},{record.recordNumPointer},{record.blockNum}";
                        sw.WriteLine(toWrite);
                    }
                }

                Console.WriteLine("Database initialization completed successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<MainRegDBRecord> readMainReg()
        {
            List<MainRegDBRecord> mainRegDBRecords = new();

            try
            {
                using (StreamReader sr = new StreamReader(Constants.mainRegPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        MainRegDBRecord mainRegRecord = Algorithms.convertStringToMainRegRecord(line);
                        mainRegDBRecords.Add(mainRegRecord);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return mainRegDBRecords;
        }

        public List<IndexRegDBRecord> readIndexReg()
        {
            List<IndexRegDBRecord> indexRegDBRecords = new();

            try
            {
                using (StreamReader sr = new StreamReader(Constants.indexRegPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        IndexRegDBRecord indexRegRecord = Algorithms.convertStringToIndexRegRecord(line);
                        indexRegDBRecords.Add(indexRegRecord);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (indexRegDBRecords.Count == 0)
            {
                throw new NullReferenceException(nameof(indexRegDBRecords));
            }

            return indexRegDBRecords;
        }

        private int[] getAllPrimaryKeys()
        {
            List<int> primaryKeys = new();

            try
            {
                using (StreamReader sr = new StreamReader(Constants.indexRegPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        int primaryKey = Algorithms.convertStringToPrimaryKeyIntValue(line);
                        primaryKeys.Add(primaryKey);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (primaryKeys.Count == 0)
            {
                throw new NullReferenceException(nameof(primaryKeys));
            }

            int[] arrOfPrimaryKeys = primaryKeys.ToArray();
            Array.Sort(arrOfPrimaryKeys);

            return arrOfPrimaryKeys;
        }

        private void writeToMainRegFile(List<MainRegDBRecord> mainRegDBRecords)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Constants.mainRegPath, false, System.Text.Encoding.Default))
                {
                    foreach (var record in mainRegDBRecords)
                    {
                        string toWrite =
                            $"{record.increment},{record.employeeID},{record.surname},{record.isAvailable}";
                        sw.WriteLine(toWrite);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void writeToIndexRegFile(List<IndexRegDBRecord> indexRegDBRecords)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Constants.indexRegPath, false, System.Text.Encoding.Default))
                {
                    foreach (var record in indexRegDBRecords)
                    {
                        string toWrite =
                            $"{record.employeeID},{record.recordNumPointer},{record.blockNum}";
                        sw.WriteLine(toWrite);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //adding a record
        public void createRecord(string name, int employeeID)
        {
            int[] primaryKeys = getAllPrimaryKeys();
            int pkIndex = Algorithms.UniformBinarySearch(primaryKeys, employeeID);

            if (pkIndex == -1)
            {
                int mainRegNumOfElem = addToMainRegAndRetNumOfElem(name, employeeID);
                addToIndexReg(mainRegNumOfElem, employeeID);
            }
            else
            {
                throw new ArgumentException("Defined employee ID already exists in our system. PK must be unique.");
            }
        }

        private int addToMainRegAndRetNumOfElem(string name, int employeeID)
        {
            List<MainRegDBRecord> mainRegDBRecords = readMainReg();
            int recordNumPointer = mainRegDBRecords[mainRegDBRecords.Count - 1].increment + 1;

            MainRegDBRecord elemToAdd = new MainRegDBRecord(recordNumPointer, employeeID, name);
            mainRegDBRecords.Add(elemToAdd);

            writeToMainRegFile(mainRegDBRecords);

            return recordNumPointer;
        }

        private void addToIndexReg(int mainRegPointer, int employeeID)
        {
            List<IndexRegDBRecord> indexRegDBRecords = readIndexReg();

            IndexRegDBRecord elemToAdd = new IndexRegDBRecord(employeeID, mainRegPointer);
            indexRegDBRecords.Add(elemToAdd);

            indexRegDBRecords = indexRegDBRecords.
                OrderBy(b => b.blockNum).ThenBy(id => id.employeeID).ToList();

            writeToIndexRegFile(indexRegDBRecords);
        }


        //reading a record
        public string readRecord(int employeeID)
        {
            int numOfRecordInMainReg = getMainRegPointerByEmployeeIDInIndexReg(employeeID);
            return getDataFromMainRegByRecordNum(numOfRecordInMainReg);
        }

        private int getMainRegPointerByEmployeeIDInIndexReg(int employeeID)
        {
            List<IndexRegDBRecord> indexRegDBRecords = readIndexReg();
            var primaryKeys = getAllPrimaryKeys();

            int indexOfRequiredRecord = Algorithms.UniformBinarySearch(primaryKeys, employeeID);

            if (indexOfRequiredRecord == -1)
            {
                throw new ArgumentException("Given PK is not present in our database.");
            }

            return indexRegDBRecords[indexOfRequiredRecord].recordNumPointer;
        }

        private string getDataFromMainRegByRecordNum(int recordNum)
        {
            List<MainRegDBRecord> mainRegDBRecords = readMainReg();
            
            var requiredRecord = mainRegDBRecords.Single(rec => rec.increment == recordNum);
            return requiredRecord.surname;
        }


        //updating a record
        public void updateRecord(int employeeID, string newName)
        {
            int numOfRecordInMainReg = getMainRegPointerByEmployeeIDInIndexReg(employeeID);
            updateInMainRegByRecordNum(numOfRecordInMainReg, newName);
        }

        private void updateInMainRegByRecordNum(int recordNum, string newName)
        {
            List<MainRegDBRecord> mainRegDBRecords = readMainReg();

            var neededRecord = mainRegDBRecords.SingleOrDefault(rec => rec.increment == recordNum);
            neededRecord.surname = newName;

            writeToMainRegFile(mainRegDBRecords);
        }


        //deleting a record
        public void deleteRecord(int employeeID)
        {
            deleteFromMainReg(employeeID);
            deleteFromIndexReg(employeeID);
        }

        private void deleteFromMainReg(int employeeID)
        {
            int recordNum = getMainRegPointerByEmployeeIDInIndexReg(employeeID);

            List<MainRegDBRecord> mainRegDBRecords = readMainReg();
            var neededRecord = mainRegDBRecords.SingleOrDefault(rec => rec.increment == recordNum);
            neededRecord.isAvailable = 0;

            writeToMainRegFile(mainRegDBRecords);
        }

        private void deleteFromIndexReg(int employeeID)
        {
            List<IndexRegDBRecord> indexRegDBRecords = readIndexReg();
            var neededRecord = indexRegDBRecords.SingleOrDefault(rec => rec.employeeID == employeeID);

            if (neededRecord != null)
            {
                indexRegDBRecords.Remove(neededRecord);

                writeToIndexRegFile(indexRegDBRecords);
            }
            else
            {
                throw new NullReferenceException("Record to delete is not found");
            }
        }
    }
}
