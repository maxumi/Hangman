using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HangmanClient
{
    internal class HangmanClientClass
    {
        private static readonly int port = 11000;

        public HangmanClientClass()
        {
            IPAddress serverIp = GetServerIpAddress();
            IPEndPoint endPoint = new IPEndPoint(serverIp, port);
            Console.WriteLine($"Connecting to {serverIp} on port {port}...");
            StartClient(endPoint);
        }

        private void StartClient(IPEndPoint endPoint)
        {
            Socket sender = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(endPoint);
                Console.WriteLine("Connected successfully!");

                while (true)
                {
                    // Receive the message from the server
                    byte[] buffer = new byte[1024];
                    int received = sender.Receive(buffer);
                    string msg = Encoding.ASCII.GetString(buffer, 0, received);
                    Console.WriteLine(msg);

                    // If the game has ended, exit the loop
                    if (msg.Contains("win") || msg.Contains("lose"))
                    {
                        break;
                    }

                    // Send the user's guess to the server
                    string guess = SendGuess();
                    byte[] byteArray = Encoding.ASCII.GetBytes(guess);
                    sender.Send(byteArray);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
            finally
            {
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
        }

        private string SendGuess()
        {
            Console.Write("Input guess: ");
            return Console.ReadLine();
        }

        private IPAddress GetServerIpAddress()
        {
            Console.Write("Connect to server IP: ");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return IPAddress.Parse("127.0.0.1");
            }
            return IPAddress.Parse(input);
        }
    }
}
