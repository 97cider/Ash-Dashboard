
using ElectronCgi.DotNet;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ConnectionBuilder()
                .WithLogging()
                .Build();

            connection.On<string, string>("cameraCheck", name => "Og " + name);

            connection.Listen();
        }
    }
}