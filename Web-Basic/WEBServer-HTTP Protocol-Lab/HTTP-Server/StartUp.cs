using System;

namespace HTTP_Server
{
    class StartUp
    {
        static void Main(string[] args)
        {
            IHttpServer httpServer = new HttpServer();

            httpServer.Start();
        }
    }
}