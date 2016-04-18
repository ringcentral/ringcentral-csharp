using ManyConsole;
using System;

namespace RingCentral.CodeGen
{
    public class GenerateCommand : ConsoleCommand
    {
        public string Language;

        public GenerateCommand()
        {
            this.IsCommand("g", "generate source code");
            this.HasOption("l|lang=", "lang", l => Language = l);
        }

        public override int Run(string[] remainingArguments)
        {
            if (Language == null)
            {
                Language = "csharp";
            }
            Console.WriteLine("Language is " + Language);
            return 0;
        }
    }
}
