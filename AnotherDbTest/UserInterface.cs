using System;
using System.Speech.Synthesis;
using System.Globalization;
using System.Threading;
using static System.Console;

namespace AnotherDbTest
{
    class UserInterface
    {
        private static SpeechSynthesizer speaker;
        private static VoiceInfo info;

        public UserInterface (int voice, int rate)
        {
            speaker = new SpeechSynthesizer();
            speaker.SelectVoice(speaker.GetInstalledVoices()[voice].VoiceInfo.Name);
            speaker.Rate = rate;
        }

        internal void Say(string message)
        {
            ClearVoice();
            speaker.SpeakAsync(message);
            WriteLine(message);
        }
        internal string SayAndRead (string message)
        {
            ClearVoice();
            speaker.SpeakAsync(message);
            WriteLine(message);
            return ReadLine();
        }
        private void ClearVoice()
        {
            Prompt current = speaker.GetCurrentlySpokenPrompt();
            if (current != null) { speaker.SpeakAsyncCancel(current); }
        }
        internal void SetVoice()
        {
            speaker.Speak("Please select a voice: press space for the next voice. Press enter to select.");

            foreach (InstalledVoice voice in speaker.GetInstalledVoices())
            {
                info = voice.VoiceInfo;
                WriteLine($"  Name: {info.Name}, culture: {info.Culture}, gender: {info.Gender}, age: {info.Age}.");
                WriteLine($"    Description: {info.Description}");

                ClearVoice();
                speaker.SelectVoice(info.Name);
                speaker.SpeakAsync($"My name is {info.Name}. I am {GetCulture(info.Culture)}.");
                if (ReadKey().Key == ConsoleKey.Enter) { break; }
            }
            ClearVoice();
            speaker.Speak($"{info.Name} selected.");
        }

        private object GetCulture(CultureInfo culture)
        {
            string nationality = string.Empty;
            switch (culture.ToString())
            {
                case "en-US":
                    nationality = "American";
                    break;
                case "en-CA":
                    nationality = "Canadian";
                    break;
                case "en-IE":
                    nationality = "Irish";
                    break;
                case "en-IN":
                    nationality = "Indian";
                    break;
                case "en-GB":
                    nationality = "British";
                    break;
                case "en-AU":
                    nationality = "Australian";
                    break;
            }
            return nationality;
        }
        internal void Greet()
        {
            speaker.SpeakAsync("Welcome to the database program!");
            WriteLine("Dave's first database app, A.K.A. \"A Load Of C.R.U.D.\"\n");
        }
        internal string SelectFunctionMessage()
        {
            WriteLine("\n\nEnter the number of your choice:\n\t1 for Books\n\t2 for People\n\t"
               + "3 to add a person to the database\n\t4 to update the database\n\t5 to change the voice"
               + "\n\t6 to delete a person from the database\n\t'Q' to quit");
            speaker.SpeakAsync("Please choose an option.");
            return ReadLine();
        }

        internal void RetrievedMessage(string dbName)
            => Say($"\nData retrieved from {(dbName.Equals("pubs") ? "books" : "people")} database.");


        internal string ModifyIdMessage() => SayAndRead("Please select ID number of the person to modify:");

        internal string ModifyCategoryMessage() 
            => SayAndRead ("Please select data to modify: 1 for first name, 2 for last name, 3 for year of birth");

        internal string ModifyNewDataMessage() => SayAndRead("Please enter new data:");

        internal void ModifiedMessage(string dataOption, int id, string newData)
            => Say($"{dataOption} of person number {id} changed to {newData}");


        internal string GetPersonData(string category) => SayAndRead ("Enter " + category);

        internal void AddedMessage(string first, string last, int year)
            => Say($"{first} {last}, year of birth {year}, successfully added to the database.");


        internal string DeleteSelectMessage() => SayAndRead ("Please select ID number of the person to Delete:");

        internal void DeletedMessage(int id) => Say($"Person number {id} deleted.");


        internal void NoDataMessage()
        {
            Say ("No data available.");
        }

        internal void RetrievingMessage()
        {
            Say("Retrieving data.\n");
        }

        internal void ErrorMessage()
        {
            Random random = new Random();
            int i = random.Next(1, 4);
            string message = string.Empty;
            switch (i)
            {
                case 1:
                    message = "Are you taking the piss? Cop on.";
                    break;
                case 2:
                    message = "Invalid data. Cop on, you eedjit.";
                    break;
                case 3:
                    message = "Well, you've ballsed it up again.";
                    break;
            }

            speaker.SpeakAsync(message);
            WriteLine("Invalid data.");
        }

        internal void DisplayPerson(Person person)
        {
            WriteLine(person);
        }

        internal void DisplayBook(Book book)
        {
            WriteLine(book);
        }

        internal void ExceptionMessage(Exception e)
        {
            WriteLine("Error: " + e.StackTrace);
            speaker.SpeakAsync("Crash and burn, baby!");
        }

        internal void QuitMessage()
        {
            ClearVoice();            
            speaker.SpeakAsync ("Goodbye, and good riddance.");
            Thread.Sleep(2000);
            WriteLine("Closing program...");
        }
    }
}
