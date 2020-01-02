using System;
using SimpleREST.Routing;

namespace TrustablePerimeter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string[] prefixes = { "http://localhost:12345/" };
            Server server = new Server(prefixes);
            server.Run();
        }
    }
}
