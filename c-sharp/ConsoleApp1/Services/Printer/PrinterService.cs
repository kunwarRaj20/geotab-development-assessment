using JokesGenerator.Helpers;
using Microsoft.Extensions.Logging;
using System;

namespace JokesGenerator.Services.Printer
{
    public class PrinterService : IPrinterService
    {
        private readonly ConsolePrinter _printer;
        private readonly ILogger<PrinterService> _printerServiceLog;
        public PrinterService(ConsolePrinter printer,
            ILogger<PrinterService> printerServiceLog)
        {
            _printer = printer;
            _printerServiceLog = printerServiceLog;
        }

        /// <summary>
        /// prints message to the console 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="newLine">default to true, if passed adds a new line after the message</param>

        public void PrintMessage(string message, bool newLine = true)
        {
            _printer.Value(message, newLine).ToString();
        }

        /// <summary>
        /// print the array in the console 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="bulletList">default to false, if passed, prints the array in bulleted form</param>
        public void PrettyPrintArray(string[] arr, bool bulletList = false)
        {
            try
            {
                if (bulletList)
                {
                    for (int index = 0; index < arr.Length; index++)
                    {
                        var item = arr[index];
                        if (!String.IsNullOrEmpty(item))
                        {
                            var message = string.Format("{0}. {1}", index + 1, item);

                            _printer.Value(message).ToString();
                        }
                    }
                }
                else
                    PrintMessage(String.Join("", arr));
            }
            catch (Exception ex)
            {
                _printerServiceLog.LogError($"PrinterService - PrettyPrintArray : {ex.Message} + '\n' + {ex.StackTrace}");
                PrintMessage("");
            }
        }

        /// <summary>
        /// prints banner message to the console with the input text
        /// </summary>
        /// <param name="message"></param>
        public void PrintBanner(string message)
        {
            _printer.Value("*****************************************************************************************************", false).ToString();
            _printer.Value("").ToString();
            _printer.Value("                                       " + message + "                                               ", false).ToString();
            _printer.Value("").ToString();
            _printer.Value("*****************************************************************************************************", false).ToString();
        }
    }
}
