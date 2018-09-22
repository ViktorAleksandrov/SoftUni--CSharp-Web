using System;
using System.Net;

namespace P1.URLDecode
{
    class Program
    {
        static void Main(string[] args)
        {
            string encodedUrl = Console.ReadLine();

            string decodedUrl = WebUtility.UrlDecode(encodedUrl);

            Console.WriteLine(decodedUrl);
        }
    }
}
