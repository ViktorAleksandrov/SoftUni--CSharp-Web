using System;
using System.Net;
using System.Text;

namespace P2.ValidateURL
{
    class Program
    {
        static void Main(string[] args)
        {
            string encodedUrl = Console.ReadLine();

            string decodedUrl = WebUtility.UrlDecode(encodedUrl);

            var sb = new StringBuilder();

            try
            {
                var uri = new Uri(decodedUrl);

                if (uri.Port < 0 || !uri.IsDefaultPort)
                {
                    Console.WriteLine("Invalid URL");
                    return;
                }

                string protocol = uri.Scheme;
                string host = uri.Host;
                int port = uri.Port;
                string path = uri.LocalPath;

                sb.AppendLine($"Protocol: {protocol}");
                sb.AppendLine($"Host: {host}");
                sb.AppendLine($"Port: {port}");
                sb.AppendLine($"Path: {path}");

                string query = uri.Query.TrimStart('?');

                if (query != string.Empty)
                {
                    sb.AppendLine($"Query: {query}");
                }

                string fragment = uri.Fragment.TrimStart('#');

                if (uri.Fragment != string.Empty)
                {
                    sb.AppendLine($"Fragment: {fragment}");
                }
            }
            catch (UriFormatException)
            {
                sb.AppendLine("Invalid URL");
            }

            string output = sb.ToString().TrimEnd();

            Console.WriteLine(output);
        }
    }
}
