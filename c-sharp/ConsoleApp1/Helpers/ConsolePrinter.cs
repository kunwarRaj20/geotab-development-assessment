using System;

namespace JokesGenerator.Helpers
{
    public class ConsolePrinter
    {
        public static object PrintValue;

        public ConsolePrinter Value(string value, bool isNewLineReq = true)
        {
            PrintValue = (isNewLineReq ? "\n" : "") + value;
            return this;
        }

        public override string ToString()
        {
            Console.WriteLine(PrintValue);
            return null;
        }
    }
}
