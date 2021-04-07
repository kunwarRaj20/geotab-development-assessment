namespace JokesGenerator.Services.Printer
{
    public interface IPrinterService
    {
        void PrettyPrintArray(string[] arr, bool bulletList = false);

        void PrintBanner(string message);

        void PrintMessage(string message, bool newLine = true);
    }
}
