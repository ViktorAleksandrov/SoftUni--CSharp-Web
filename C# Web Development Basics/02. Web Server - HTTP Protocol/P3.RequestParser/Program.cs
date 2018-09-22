using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace P3.RequestParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath;

            var paths = new HashSet<string>();

            while ((inputPath = Console.ReadLine()) != "END")
            {
                paths.Add(inputPath);
            }

            string[] requestArgs = Console.ReadLine().Split();

            string method = requestArgs[0].ToLower();
            string path = requestArgs[1];
            string protocol = requestArgs[2];

            string pathMethod = $"{path}/{method}";

            HttpStatusCode statusCode = paths.Contains(pathMethod) ? HttpStatusCode.OK : HttpStatusCode.NotFound;

            var sb = new StringBuilder();

            sb.AppendLine($"{protocol} {(int)statusCode} {statusCode}");
            sb.AppendLine($"Content-Length: {statusCode.ToString().Length}");
            sb.AppendLine("Content-Type: text/plain");
            sb.AppendLine();
            sb.Append(statusCode);

            Console.WriteLine(sb);
        }
    }
}
