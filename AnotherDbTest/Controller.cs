using System;

namespace AnotherDbTest
{
    class Controller
    {
        public static UserInterface ui;
             
        private static string dbName;
        private static string input;
        private static int inputInt;
        private static string dataSource;
        private static int voiceIndex;

        static void Main(string[] args)
        {
            dataSource = args[0];
            voiceIndex = int.Parse(args[1]);

            ui = new UserInterface(voiceIndex, 1);
            ui.Greet();
            
            while (true)
            {
                SelectUserFunctionNumber();
                ExecuteFunction();                                           
            }
        }

        private static void ExecuteFunction()
        {
            switch (input)
            {
                case "1":
                case "2":
                    DbUtil.SelectQuery(input);
                    DisplayDatabase();
                    break;
                case "3":
                    AddPerson();
                    break;
                case "4":
                    ModifyPerson();
                    break;
                case "5":
                    ui.SetVoice();
                    break;
                case "6":
                    DeletePerson();
                    break;
            }
        }

        internal static void ThingsWentWrong(Exception e)
        {
            ui.ExceptionMessage(e);
        }

        private static void DeletePerson()
        {
            string entry = ui.DeleteSelectMessage();
            if (int.TryParse(entry, out int id)) { id = int.Parse(entry); }
            if (id > 0)
            {
                DbUtil.DeleteRow(id);
               
                ui.DeletedMessage(id);
            }
            else
            {
                ui.ErrorMessage();
            }
        }

        private static void DisplayDatabase()
        {
            ui.RetrievingMessage();

            if (DbUtil.ReturnTable(input) == 0) { ui.RetrievedMessage(dbName); }
            else { ui.NoDataMessage(); }            
        }

        private static void ModifyPerson()
        {
            string idString = ui.ModifyIdMessage();
            string category = ui.ModifyCategoryMessage();
            string newData = ui.ModifyNewDataMessage();
            string dataOption = string.Empty;
            
            switch (category)
            {
                case "1":
                    dataOption = "FirstName";
                    break;
                case "2":
                    dataOption = "LastName";
                    break;
                case "3":
                    dataOption = "YearOfBirth";                    
                    break;
            }
            try
            {
                int id = int.Parse(idString);
                DbUtil.ModifyEntry(id, dataOption, newData);              
                ui.ModifiedMessage(dataOption, id, newData);
            }
            catch (Exception)
            {
                ui.ErrorMessage();
            }
        }

        private static void SelectUserFunctionNumber()
        {
            input = ui.SelectFunctionMessage();
            
            if (int.TryParse(input, out inputInt)) { inputInt = int.Parse(input); }

            if (input.Equals("Q", StringComparison.OrdinalIgnoreCase)) { QuitRoutine(); }
            if (inputInt < 1 || inputInt > 6)
            {
                ui.ErrorMessage();
                SelectUserFunctionNumber();
                return;
            }
            SelectDatabase();
        }

        private static void AddPerson()
        {
            string first = ui.GetPersonData ("first name");
            string last = ui.GetPersonData ("last name");
            string yearString = ui.GetPersonData("year of birth");

            if (int.TryParse(yearString, out int year)) { year = int.Parse(yearString); }

            try
            {
                if (first.Length > 0 && last.Length > 0 && year > 0)
                {
                    DbUtil.AddRow(first, last, year);
                    ui.AddedMessage(first, last, year);
                }
                else
                {
                    ui.ErrorMessage();
                }
            }
            catch (Exception e)
            {
                ui.ExceptionMessage(e);
            }
        }
        
        private static void QuitRoutine()
            {
                ui.QuitMessage();
                Environment.Exit(0);
            }

        private static void SelectDatabase()
        {
            if (input.Equals("1")) { dbName = "pubs"; }
            if (inputInt > 1 && inputInt < 7) { dbName = "Dave_Db_Test"; }
            DbUtil.SetConnectionString(dataSource, dbName);
        }

        internal static void BookRetrieved(Book book)
        {
            ui.DisplayBook(book);
        }
        internal static void PersonRetrieved(Person person)
        {
            ui.DisplayPerson(person);
        }
    }
}

