namespace _2lab_denseIndex.dbRecords
{
    public class MainRegDBRecord
    {
        public string surname { get; set; } //surname of an employee
        public int employeeID { get; set; } //unique id of an employee
        public int increment { get; set; } //
        public int isAvailable { get; set; } //can accept values 1 or 0 (1 - if record available, 0 - if not available) 

        public MainRegDBRecord(int increment, int primaryKey, string dataValue)
        {
            this.surname = dataValue;
            this.employeeID = primaryKey;
            this.increment = increment;
            this.isAvailable = 1; //1 if available, 0 if deleted
        }

        public MainRegDBRecord(int increment, int primaryKey, string dataValue, int isAvailable)
        {
            this.surname = dataValue;
            this.employeeID = primaryKey;
            this.increment = increment;
            this.isAvailable = isAvailable;
        }

        public override string ToString()
        {
            return "\nSurname: " + surname + "\n[PK]Employee ID: " + employeeID + "\nRecordNum: " + increment;
        }
    }
}
