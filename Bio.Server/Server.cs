#region Copyright
//
// Copyright(C) Eric Singh., 2017.
//
#endregion

#region Using Declarations
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Bio.Library;
#endregion

namespace Bio.Server
{
    /// <summary>
    /// Asynchronous server used to listen for incoming connections and handle commands from multiple clients
    /// </summary>
    public class Server
    {
        #region Fields/Properties

        /// <summary>
        /// CancellationToken to stop the server
        /// </summary>
        public static CancellationTokenSource StopServerToken = new CancellationTokenSource();

        /// <summary>
        /// Port used for connecting to server
        /// </summary>
        public static int Port = 55555;

        #endregion

        #region Methods

        /// <summary>
        /// Main method. Server execution starts here
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            RunServer();
        }

        /// <summary>
        /// Start running the server to listen for connections
        /// </summary>
        public static void RunServer()
        {
            // Thread safe dictionary to hold the active clients
            // Thread safe dictionary must be used because the clients may access it
            // on different threads
            var clients = new ConcurrentDictionary<long, HandleClient>();
            long counter = 0;

            try
            {
                // Start listening
                // SSL encryption can be added later to improve security
                // We can also add some logic to limit the number of connections if required
                var serverSocket = new TcpListener(IPAddress.Any, Port);
                var clientSocket = default(TcpClient);
                serverSocket.Start();

                // Stop listening if the token is triggered
                StopServerToken.Token.Register(() => serverSocket.Stop());

                Console.WriteLine($"{nameof(Server)} started");

                while (true)
                {
                    // Wait for client to connect
                    clientSocket = serverSocket.AcceptTcpClient();
                    counter++;
                    Console.WriteLine($"{nameof(Server)} connected to Client {counter}");

                    // The constructor is asynchronous. This will not block and we can handle more incoming connections
                    clients[counter] = new HandleClient(clients, clientSocket, counter);
                }
            }
            catch (Exception ex)
            {
                // Only display the error message if the cancellation token wasn't triggered
                Console.WriteLine(StopServerToken.IsCancellationRequested ?
                    $"{nameof(Server)} disconnected" : $"Error: {ex.Message}");
            }
        }

        #endregion
    }
}

