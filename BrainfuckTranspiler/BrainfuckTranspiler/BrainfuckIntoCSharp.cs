using System.IO;
using System.Text;

namespace BrainfuckTranspiler
{
    public static class BrainfuckIntoCSharp
    {
        public static string Transpile(Stream code, string @namespace, string @class, string method)
        {
            var template = @"using System;

namespace {0}
{{
    public class {1}
    {{
        public void {2}()
        {{
{3}
        }}
    }}
}}";

            var sb = new StringBuilderWithIndentation(12);
            sb.AppendLine("var array = new int[30000];");
            sb.AppendLine("var index = 0;");

            using (var sr = new StreamReader(code))
            {
                while (!sr.EndOfStream)
                {
                    var command = (char)sr.Read();
                    switch (command)
                    {
                        case '>':
                            sb.AppendLine("index++;");
                            break;
                        case '<':
                            sb.AppendLine("index--;");
                            break;
                        case '+':
                            sb.AppendLine("array[index]++;");
                            break;
                        case '-':
                            sb.AppendLine("array[index]--;");
                            break;
                        case '.':
                            sb.AppendLine("Console.Write((char)array[index]);");
                            break;
                        case ',':
                            sb.AppendLine("array[index] = (int)Console.ReadKey().KeyChar;");
                            break;
                        case '[':
                            sb.AppendLine("while (array[index] != 0)");
                            sb.AppendLine("{");
                            sb.MoveRight();
                            break;
                        case ']':
                            sb.MoveLeft();
                            sb.AppendLine("}");
                            break;
                        default: //ignore any other characters
                            break;
                    }
                }
            }

            return string.Format(template, @namespace, @class, method, sb.ToString());
        }
    }

    public class StringBuilderWithIndentation
    {
        public int Indentation { get; set; }
        private readonly StringBuilder stringBuilder;

        public StringBuilderWithIndentation(int initialIndentation)
        {
            Indentation = initialIndentation;
            stringBuilder = new StringBuilder();
        }

        public void AppendLine(string value)
        {
            stringBuilder.Append(' ', Indentation);
            stringBuilder.AppendLine(value);
        }

        public void MoveRight()
        {
            Indentation += 4;
        }

        public void MoveLeft()
        {
            Indentation -= 4;
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }
}