using System;
using System.Net;
using System.Text;

namespace _02.Validate_URL
{
    class StartUp
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            StringBuilder sb = new StringBuilder();

            var url = WebUtility.UrlDecode(input);

            var urlParts = new Uri(url);

            var protocol = urlParts.Scheme;
            var host = urlParts.Host;
            int? port = urlParts.Port;
            var path = urlParts.AbsolutePath;
            var query = urlParts.Query;
            var fragment = urlParts.Fragment;

            if (protocol == null || host == null || port == null || path == null)
            {
                Console.WriteLine("Invalid URL");
                return;
            }
            if (protocol == "http")
            {
                if (port == 80 || port == null)
                {
                    sb.AppendLine($"Protocol: {protocol}");
                    sb.AppendLine($"Host: {host}");
                    sb.AppendLine("Port: 80");
                    sb.AppendLine($"Path: {path}");

                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        sb.AppendLine($"Query: {query}");
                    }
                    if (!string.IsNullOrWhiteSpace(fragment))
                    {
                        sb.AppendLine($"Fragment: {fragment}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid URL");
                    return;
                }
            }
            else if (protocol == "https")
            {
                if (port == 443 || port == null)
                {
                    sb.AppendLine($"Protocol: {protocol}");
                    sb.AppendLine($"Host: {host}");
                    sb.AppendLine("Port: 443");
                    sb.AppendLine($"Path: {path}");
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        sb.AppendLine($"Query: {query}");
                    }
                    if (!string.IsNullOrWhiteSpace(fragment))
                    {
                        sb.AppendLine($"Fragment: {fragment}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid URL");
                    return;
                }
            }

            string result = sb.ToString().Trim();
            Console.WriteLine(result);
        }
    }
}