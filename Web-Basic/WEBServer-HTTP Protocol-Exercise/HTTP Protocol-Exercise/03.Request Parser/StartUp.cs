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

                var method = correctInput[0];
                var requestInput = correctInput[1];

                if (!requests.ContainsKey(method))
                {
                    requests.Add(method, new HashSet<string>());
                }

                requests[method].Add(requestInput);
            }

            string httpRequest = Console.ReadLine().ToLower();

            var parsedHttpRequest = httpRequest.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var parsedMethod = parsedHttpRequest[1].Trim('/');
            var parsedRequest = parsedHttpRequest[0];

            if (requests.ContainsKey(parsedMethod))
            {
                var getRequest = requests[parsedMethod].FirstOrDefault(r => r == parsedRequest);

                if (getRequest != null)
                {
                    sb.AppendLine("HTTP/1.1 200 OK");
                    sb.AppendLine("Content-Length: 2");
                    sb.AppendLine("Content-Type: text/plain");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine("OK");
                }
                else
                {
                    sb.AppendLine("HTTP/1.1 404 NotFound");
                    sb.AppendLine("Content-Length: 9");
                    sb.AppendLine("Content-Type: text/plain");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine("NotFound");
                }
            }

            string result = sb.ToString().Trim();

            Console.WriteLine(result);
        }
    }
}