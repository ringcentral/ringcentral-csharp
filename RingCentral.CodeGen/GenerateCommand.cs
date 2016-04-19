using ManyConsole;
using System;
using System.Diagnostics;
using System.IO;

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

            // generate code
            Process.Start("cmd.exe", "/C java -jar swagger-codegen.jar generate -i swagger.json -l csharp -o tmp/test");

            // rename namespace
            foreach (var filePath in Directory.GetFiles(@"tmp\test\src\main\csharp\IO\Swagger\Model"))
            {
                Debug.WriteLine(filePath);
                var content = "";
                using (var sr = new StreamReader(filePath))
                {
                    content = sr.ReadToEnd();
                }
                content = content.Replace("namespace IO.Swagger.Model", "namespace RingCentral.Model");

                // write to destination directly
                var fileName = Path.GetFileName(filePath);
                using (var sw = new StreamWriter(@"..\..\..\RingCentral\model\" + fileName))
                {
                    sw.Write(content);
                }
            }

            return 0;
        }
    }
}
