using System;

namespace _2lab_denseIndex
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //FilesRepository actionsWithFile = new();
            //actionsWithFile.initialDatabase();

            UserInteraction.interactWithUser();
           
            // we have 5 blocks in index: from 1 to 100, from 101 to 200, from 201 to 300, from 301 to 400, from 401 to 500 
            // each block in index reg contains 100 records + 10% (10 records is an empty space)
            // in general, index reg consists of 550 records + infinite number of records num in overflow block
        }
    }
}
