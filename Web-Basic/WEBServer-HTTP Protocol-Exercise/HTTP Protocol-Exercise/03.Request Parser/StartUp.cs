using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _03.Request_Parser
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var requests = new Dictionary<string, HashSet<string>>();
            StringBuilder sb = new StringBuilder();

            string input = string.Empty;
            while ((input = Console.ReadLine().ToLower()) != "end")
            {
                var correctInput = input.Split('/', StringSplitOptions.RemoveEmptyEntries);

                var path = correctInput[0];
                var method = correctInput[1];

                if (!requests.ContainsKey(path))
                {
                    requests.Add(path, new HashSet<string>());
                }

                requests[path].Add(method);
            }

            string httpRequest = Console.ReadLine().ToLower();

            var parsedHttpRequest = httpRequest.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var parsedPath = parsedHttpRequest[1].Trim('/');
            var parsedMethod = parsedHttpRequest[0];

            if (requests.ContainsKey(parsedPath))
            {
                var getRequest = requests[parsedPath].FirstOrDefault(r => r == parsedMethod);

                if (getRequest != null)
                {
                    sb.AppendLine("HTTP/1.1 200 OK");
                    sb.AppendLine("Content-Length: 2");
                    sb.AppendLine("Content-Type: text/plain");
                    sb.AppendLine();
                    sb.AppendLine("OK");
                }
                else
                {
                    sb.AppendLine("HTTP/1.1 404 NotFound");
                    sb.AppendLine("Content-Length: 9");
                    sb.AppendLine("Content-Type: text/plain");
                    sb.AppendLine();
                    sb.AppendLine("NotFound");
                }
            }

            string result = sb.ToString().Trim();

            Console.WriteLine(result);
        }
    }
}