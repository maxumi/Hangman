using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HangmanServer
{
    internal class HangmanServerClass
    {
        public static int count = 0;
        public static List<char> CorrectChars = new List<char>();
        public HangmanServerClass()
        {
            IPEndPoint endpoint = GetServerEndpoint();
            Socket listener = new(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endpoint);
            listener.Listen(10);
            Console.WriteLine($"Server Listening on: {listener.LocalEndPoint}");
            StartServer(listener);
            
        }
        private void StartServer(Socket listener)
        {
            Socket handler = listener.Accept();
            Console.WriteLine($"Accepting connection from {handler.RemoteEndPoint}");
            GameClass game = new(handler);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

        }
            private IPEndPoint GetServerEndpoint()
        {
            //Gets the hostname of the machine
            string strHostName = Dns.GetHostName();
            //Uses hostnape to get host entry
            IPHostEntry host = Dns.GetHostEntry(strHostName);
            //Host entry contains all IP addressses
            //We create a list of IPv4 addresses
            List<IPAddress> addrList = new();
            int counter = 0;
            foreach (var item in host.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetworkV6) continue;
                Console.WriteLine($"{counter++} {item.ToString()}");
                addrList.Add(item);
            }
            //Selects the IP from the list. If input is number and is within the range of the list,
            //If list contains 1 endpoint use that instead of asking.
            if (addrList.Count == 1) return new IPEndPoint(addrList[0], 11000);

            int temp;
            do Console.Write("Select server IP: ");
            while (!int.TryParse(Console.ReadLine(), out temp) || temp > addrList.Count || temp < 0);

            return new IPEndPoint(addrList[temp], 11000);
        }
    }
}
