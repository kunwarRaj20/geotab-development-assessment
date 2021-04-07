using JokesGenerator.Helpers;
using JokesGenerator.Services.Jokes;
using JokesGenerator.Services.Names;
using JokesGenerator.Services.Printer;
using Microsoft.Extensions.Logging;
using System;

namespace JokesGenerator.Services.Runner
{
    public class RunnerService : IRunnerService
    {
        private readonly IJokesService _jokeService;
        private readonly INameService _nameService;
        private readonly ILogger<RunnerService> _runnerLog;
        private readonly IPrinterService _printerService;
        private readonly AppSession _appSession;
        public RunnerService(IJokesService jokeService,
            INameService nameService,
            ILogger<RunnerService> runnerLog,
            AppSession appSession,
            IPrinterService printerService)
        {
            _jokeService = jokeService;
            _nameService = nameService;
            _runnerLog = runnerLog;
            _appSession = appSession;
            _printerService = printerService;
        }

        /// <summary>
        /// triggers the application
        /// </summary>
        public void Run()
        {

            Console.ForegroundColor = ConsoleColor.Cyan;
            _printerService.PrintBanner("Welcome to Jokes Generator");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            _printerService.PrintMessage("This app allows you to get random " + AppConsts.DefaultTypes.DefaultName + " jokes. You can also choose the category or choice of a random name");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ReadFromConsole("Press any key to get instructions, x to exit the application.");
                if (_appSession.Key == 'x')
                    Environment.Exit(0);
                break;
            }
            Execute();
        }

        /// <summary>
        /// initalize the app with the main menu and triggers the workflows 
        /// </summary>
        private void Execute()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                _printerService.PrintMessage("> Main Menu");
                ReadFromConsole("1. Press c to get categories \n2. Press r to get random jokes \n3. Press x to exit the application");
                if (_appSession.Key == 'c')
                {
                    ExecuteCategories(_jokeService);
                    if (ExecuteJokes(_jokeService))
                        ExecuteRandomJokes(_jokeService, _nameService);
                }
                if ((_appSession.Key == 'r'))
                    ExecuteRandomJokes(_jokeService, _nameService);

                else if (_appSession.Key == 'x')
                    EndApplication();
                else if (_appSession.Key != 'n' && _appSession.Key != 'y')
                {
                    InvalidEntry();
                    continue;
                }
                ReadFromConsole("Press any key to go back to main menu, x to exit the application");
                if (_appSession.Key == 'x')
                    EndApplication();
            }
        }

        /// <summary>
        /// triggers the random jokes workflow
        /// </summary>
        /// <param name="jokeService"></param>
        /// <param name="nameService"></param>
        private void ExecuteRandomJokes(IJokesService jokeService, INameService nameService)
        {

            Tuple<string, string> names = ExecuteRandonName(nameService);
            ExecuteJokeDisplay(names, jokeService);
        }

        /// <summary>
        /// Prints the number of jokes requested by the user 
        /// </summary>
        /// <param name="names"></param>
        /// <param name="jokeService"></param>
        private void ExecuteJokeDisplay(Tuple<string, string> names, IJokesService jokeService)
        {
            while (true)
            {
                _printerService.PrintMessage("Q> Specify how many jokes you want from 1-9, please press enter after to continue");
                if (int.TryParse(Console.ReadLine(), out int numberOfJokes))
                {
                    if (jokeService.IsValidJokeCount(numberOfJokes))
                    {
                        GetJokes(jokeService, numberOfJokes, names);
                        break;
                    }
                    else
                        InvalidEntry("The numbered enterd is not within the range. Please enter a between 1-9 to continue.");
                }
                else
                    InvalidEntry("Invalid Entry, please choose a number between 1-9 to conitnue");
            }
        }

        /// <summary>
        /// Asks the user if they want to specify a random name for the jokes
        /// </summary>
        /// <param name="nameService"></param>
        /// <returns></returns>
        private Tuple<string, string> ExecuteRandonName(INameService nameService)
        {
            Tuple<string, string> names = null;
            while (true)
            {
                ReadFromConsole("Q> Do you want to use a random name for the Joke(s)? Press y for yes, n for no, x to exit the application");
                if (_appSession.Key == 'y')
                {
                    names = GetNameForRandomJokes(nameService);
                    _printerService.PrintMessage(names.Item1 + " " + names.Item2 + " will now be used for the jokes");
                    break;
                }
                else if (_appSession.Key == 'x')
                    EndApplication();
                else if (_appSession.Key == 'n')
                {
                    _printerService.PrintMessage(AppConsts.DefaultTypes.DefaultName + " will be used for the jokes");
                    break;
                }
                else
                    InvalidEntry("Invalid Entry, please entry a valid entry to continue.");
            }

            return names;
        }

        /// <summary>
        /// gets the first name and last name randomly and saves it to a tuple
        /// </summary>
        /// <param name="nameService"></param>
        /// <returns></returns>
        private Tuple<string, string> GetNameForRandomJokes(INameService nameService)
        {
            _printerService.PrintMessage("***Loading Name................");
            var result = nameService.GetNames().Result;
            return Tuple.Create(ConsoleHelper.RemoveDiacritics(result.name.ToString()), ConsoleHelper.RemoveDiacritics(result.surname.ToString()));
        }

        /// <summary>
        /// gets and prints the jokes as per the selections made by the user
        /// </summary>
        /// <param name="jokeService"></param>
        /// <param name="numberOfJokes"></param>
        /// <param name="names"></param>
        private void GetJokes(IJokesService jokeService, int numberOfJokes, Tuple<string, string> names)
        {
            _printerService.PrintMessage("***Loading Jokes................");
            var retVal = jokeService.GetRandomJokes(names, _appSession.Category, numberOfJokes).Result;
            if (retVal.Length <= numberOfJokes)
                _printerService.PrintMessage("Sorry, we could only find the following jokes, please try a different category");
            _printerService.PrettyPrintArray(retVal, true);
        }

        /// <summary>
        /// gets and prints the categories of the jokes
        /// </summary>
        /// <param name="jokeService"></param>
        private void ExecuteCategories(IJokesService jokeService)
        {
            _printerService.PrintMessage("> Categories");
            _printerService.PrintMessage("***Loading Joke Catgories................");

            var retVal = jokeService.GetCategories().Result;
            _printerService.PrettyPrintArray(retVal, true);
        }

        /// <summary>
        /// triggers the jokes workflow, if the user wants to get a jokes it continues otherwise goes to a different flow 
        /// </summary>
        /// <param name="jokeService"></param>
        /// <returns></returns>
        private bool ExecuteJokes(IJokesService jokeService)
        {
            while (true)
            {
                ReadFromConsole("Q> Do you want to get a joke? Press y for yes, n for no, x to exit the application.");
                if (_appSession.Key == 'y')
                {
                    while (true)
                    {
                        _printerService.PrintMessage("Q> Please enter a category name from options above, press x to exit the application");
                        _appSession.Category = Console.ReadLine();
                        if (_appSession.Category.Length == 1 && _appSession.Category.ToLower() == "x")
                            EndApplication();
                        _printerService.PrintMessage("***Validating Entered Category................");
                        if (jokeService.IsCategoryValid(_appSession.Category).Result)
                        {
                            _printerService.PrintMessage("***Valid Category Detected................");
                            return true;
                        }
                        else
                            InvalidEntry("Invalid category detected, please choose a valid category name from the list above.");
                    }
                }
                else if (_appSession.Key == 'n')
                    return false;
                else if (_appSession.Key == 'x')
                    EndApplication();
                else
                    InvalidEntry("Invalid Entry, please entry a valid entry to continue.");
            }
        }

        /// <summary>
        /// //show a prompt message and get a input key 
        /// </summary>
        /// <param name="message"></param>
        private void ReadFromConsole(string message)
        {
            _printerService.PrintMessage(message);
            _appSession.Key = ConsoleHelper.GetEnteredKey(Console.ReadKey());
        }

        /// <summary>
        ///  print a welcome banner message 
        /// </summary>
        private void Banner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            _printerService.PrintBanner("Welcome to Jokes Generator");
        }

        /// <summary>
        /// displays a prommpt message and exits the application 
        /// </summary>
        /// <returns></returns>
        private void EndApplication()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            _printerService.PrintMessage("Exiting the application. See you soon :)");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(0);
        }

        /// <summary>
        /// prints a prompts message for invalid entry 
        /// </summary>
        /// <param name="message"></param>
        private void InvalidEntry(string message = "Invalid Entry. Please enter a valid entry.Back to Main Menu")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            _printerService.PrintMessage(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
