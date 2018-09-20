using System;
using System.Net;

namespace _01.URL_Decode
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();

            var decodedUrl = WebUtility.UrlDecode(input);

            Console.WriteLine(decodedUrl);
        }
    }
}