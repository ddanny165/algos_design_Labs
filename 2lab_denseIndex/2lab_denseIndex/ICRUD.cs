namespace _2lab_denseIndex
{
    interface ICRUD
    {
        void createRecord(string name, int employeeID);
        string readRecord(int employeeID);
        void updateRecord(int employeeID, string newName);
        void deleteRecord(int employeeID);
    }
}
