using System;

namespace _2lab_denseIndex
{
    public static class UserInteraction
    {
        public static void interactWithUser()
        {
            FilesRepository actionsWithFile = new();

            while (true)
            {
                Console.Write("\nAvailable actions:\n 1. Find a record by its primary key;\n" +
                " 2. Add a new record to the database;\n" +
                " 3. Update a record by its primary key;\n" +
                " 4. Delete a record by its primary key.\n" +
                " 5. Exit.\n\n" +
                "Your choice [1|2|3|4|5]: ");
                try
                {
                    int usersChoice = int.Parse(Console.ReadLine());

                    switch (usersChoice)
                    {
                        case 1:
                            try
                            {
                                Console.Write("\nDefine the ID (primary key): ");
                                int idToLookFor = int.Parse(Console.ReadLine());

                                string recordValue = actionsWithFile.readRecord(idToLookFor);
                                Console.WriteLine($"Value for the given PK is |{recordValue}|");
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("\nDefined ID is not found in our database. Try again.");
                                interactWithUser();
                            }
                            break;

                        case 2:
                            try
                            {
                                Console.Write("\nDefine the name of a new employee (value): ");
                                string valueToAdd = Console.ReadLine();

                                Console.Write("\nDefine the ID (primary key) of a new employee: ");
                                int idToAdd = int.Parse(Console.ReadLine());

                                actionsWithFile.createRecord(valueToAdd, idToAdd);
                                Console.WriteLine("Successfully added.");
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("\nDefined ID already exists in our database. Try again.");
                                interactWithUser();
                            }
                            break;

                        case 3:
                            try
                            {
                                Console.Write("Define an employee ID (PK), whom you want to find: ");
                                int idOfRecordToChange = int.Parse(Console.ReadLine());

                                Console.Write("Define a new name of an employee: ");
                                string newName = Console.ReadLine();

                                actionsWithFile.updateRecord(idOfRecordToChange, newName);
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("\nDefined ID is not found in our database. Try again.");
                                interactWithUser();
                            }
                            break;

                        case 4:
                            try
                            {
                                Console.Write("Define a primary key of a record, which you want to delete: ");
                                int idToDelete = int.Parse(Console.ReadLine());

                                actionsWithFile.deleteRecord(idToDelete);
                            }
                            catch
                            {
                                Console.WriteLine("\nDefined ID is not found in our database. Try again.");
                                interactWithUser();
                            }
                            break;

                        case 5:
                            Console.WriteLine("\nSee you later!");
                            return;

                        default:
                            Console.WriteLine("Unknown action. Please try again.");
                            interactWithUser();
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect input format. Try again.");
                    interactWithUser();
                }
            }
        }
    }
}
