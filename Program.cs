using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace servertcp_deneme
{
    class Program
    {
        public static void Main(string[] args)
        {   
            // Create a TCP/IP listener.
            var localAddress = IPAddress.Parse("192.168.1.133");
            var tcpListener = new TcpListener(localAddress, 5000);

            // Start listening for connections.
            tcpListener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");

                // Program is suspended while waiting for an incoming connection.
                var tcpClient = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client has been accepted!");

                // An incoming connection needs to be processed.
                Thread thread = new Thread(() => ClientSession(tcpClient))
                {
                    IsBackground = true     
                };
                thread.Start();
                Console.WriteLine("Client session thread has been started!");
            }
        }

        private static bool TryReadExact(Stream stream, byte[] buffer, int offset, int count)
        {
            int bytesRead;
            while (count > 0 && ((bytesRead = stream.Read(buffer, offset, count)) > 0))
            {
                offset += bytesRead;
                count -= bytesRead;
            }

            return count == 0;
        }

        private static void ClientSession(TcpClient tcpClient)
        {
            const int totalByteBuffer = 4096;
            byte[] buffer = new byte[256];

            using (var networkStream = tcpClient.GetStream())
            using (var bufferedStream = new BufferedStream(networkStream, totalByteBuffer))
                while (true)
                {
                    // Receive header - byte length.
                    if (!TryReadExact(bufferedStream, buffer, 0, 1))
                    {
                        break;
                    }
                    byte messageLen = buffer[0];

                    // Receive the ASCII bytes.
                    if (!TryReadExact(bufferedStream, buffer, 1, messageLen))
                    {
                        break;
                    }

                    var message = Encoding.ASCII.GetString(buffer, 1, messageLen);
                    Console.WriteLine("Message received: {0}", message);
                    Console.WriteLine("HAM MESAJ:" + message.Split("\n").ToString());
                    
                }
            Console.WriteLine("Client session completed!");
        }
    }
}
