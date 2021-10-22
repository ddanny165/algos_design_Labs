namespace _2lab_denseIndex.dbRecords
{
    public class IndexRegDBRecord
    {
        public int employeeID { get; set; } //primary key
        public int recordNumPointer { get; set; } //pointer to the main reg

        public int blockNum { get; set; }

        public IndexRegDBRecord(MainRegDBRecord mainRegDBRecord)
        {
            this.employeeID = mainRegDBRecord.employeeID;
            this.recordNumPointer = mainRegDBRecord.increment;
            this.blockNum = Algorithms.getBlockNumByAPrimaryKey(this.employeeID);
        }
        public IndexRegDBRecord(int employeeID, int recordNumPointer)
        {
            this.employeeID = employeeID;
            this.recordNumPointer = recordNumPointer;
            this.blockNum = Algorithms.getBlockNumByAPrimaryKey(this.employeeID);
        }

        public override string ToString()
        {
            return "\n[PK]Employee ID: " + employeeID + "\nRecordNum: " + recordNumPointer;
        }
    }
}
