using System;
using System.IO;

namespace BrainfuckTranspiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = args[0];
            var displayCode = args[1];

            var @namespace = "Brainfuck";
            var @class = "BrainfuckCode";
            var method = "Run";

            using (var brainfuckCode = File.OpenRead(filePath))
            {
                var codeInCSharp = BrainfuckIntoCSharp.Transpile(brainfuckCode, @namespace, @class, method);

                if (displayCode == "verbose")
                {
                    Console.Write(codeInCSharp);
                    Console.WriteLine();
                }

                CompileAndExecute.CompileAndRun(codeInCSharp, @namespace, @class, method);
            }

            Console.ReadLine();
        }
    }
}
