#region Copyright
//
// Copyright(C) Eric Singh., 2017.
//
#endregion

#region Using Declarations
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace Bio.Library
{
    /// <summary>
    /// Class to handle client requests asynchronously
    /// </summary>
    public class HandleClient
    {
        #region Fields/Properties

        /// <summary>
        /// The list of clients currently managed by the server
        /// </summary>
        public ConcurrentDictionary<long, HandleClient> Clients
        {
            get;
            private set;
        }

        /// <summary>
        /// This client
        /// </summary>
        public TcpClient Client
        {
            get;
            private set;
        }

        /// <summary>
        /// The identifier for this client
        /// </summary>
        public long Identifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates if this client has already performed the handshake
        /// </summary>
        public bool Handshake
        {
            get;
            protected set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Asynchronously starts listening for client messages
        /// </summary>
        /// <param name="clients">All of the clients currently connected to the server</param>
        /// <param name="client">The client</param>
        /// <param name="identifier">The identfier for this client</param>
        public HandleClient(ConcurrentDictionary<long, HandleClient> clients, TcpClient client, long identifier)
        {
            Clients = clients;
            Client = client;
            Identifier = identifier;

            // Handle this client asynchronously
            Task.Run(() => HandleMessages())
                .ContinueWith(task =>
                {
                    // Show error message if task is faulted
                    if (task.IsFaulted)
                        Console.WriteLine($"Client {Identifier} Error: {task.Exception.Message}");
                });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle the message from the client
        /// </summary>
        /// <remarks>Child class can override this method to add additional functionality</remarks>
        protected virtual void HandleMessages()
        {
            while (true)
            {
                // If the client isn't connected anymore, exit the listening process
                // This will occur if the client sent TERMINATE
                if (!Client.Connected)
                    break;

                // Translate the received data to a string
                byte[] data = new byte[1024];
                int size = Client.GetStream().Read(data, 0, data.Length);
                string receivedData = Encoding.ASCII.GetString(data, 0, size);

                // If the received data is empty, the client lost connection
                if (string.IsNullOrEmpty(receivedData))
                {
                    Console.WriteLine($"Client {Identifier} lost connection");
                    RemoveAndCleanupClient();
                    break;
                }

                // Perform handshake if it hasn't been done yet or handle the command
                if (!Handshake)
                {
                    if (receivedData == ClientMessageConstants.Helo)
                        PerformHandshake();
                    //else
                    // This line can be added if the server adds a configuration property for verbose logging
                    // Console.WriteLine($"Client {Identifier} requires handshake");
                }
                else
                    HandleCommands(receivedData);
            }
        }

        /// <summary>
        /// Handle incoming commands from the client
        /// </summary>
        /// <param name="receivedData">The string data received from the client</param>
        /// <remarks>Child class can override this method to add additional functionality</remarks>
        protected virtual void HandleCommands(string receivedData)
        {
            // Action depending on received data. If the actions were
            // more complex, this can be broken out to separate methods
            switch (receivedData)
            {
                case ClientMessageConstants.Count:
                    var count = Clients.Values.Count(i => i.Handshake).ToString();
                    SendData(count);
                    Console.WriteLine(count);
                    break;

                case ClientMessageConstants.Connections:
                    var connections = Clients.Count.ToString();
                    SendData(connections);
                    Console.WriteLine(connections);
                    break;

                case ClientMessageConstants.Prime:
                    var primeNumber = PrimeNumber.GetRandomPrimeNumber().ToString();
                    SendData(primeNumber);
                    Console.WriteLine(primeNumber);
                    break;

                case ClientMessageConstants.Terminate:
                    SendData("BYE");
                    Console.WriteLine($"BYE Client {Identifier}");
                    RemoveAndCleanupClient();
                    break;
                default:
                    // This line can be added if the server adds a configuration property for verbose logging
                    // Console.WriteLine($"Command {receivedData} not recognized");
                    break;
            }
        }

        /// <summary>
        /// Sends data back to the client
        /// </summary>
        /// <param name="data">The data to send</param>
        protected virtual void SendData(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            Client.GetStream().Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Perform the handshake
        /// </summary>
        /// <remarks>Child class can override this method to add additional functionality</remarks>
        protected virtual void PerformHandshake()
        {
            SendData("HI");
            Console.WriteLine($"HI Client {Identifier}");

            // Block this thread for 5 seconds
            Thread.Sleep(5000);

            // This client has performed the handshake
            Handshake = true;
        }

        /// <summary>
        /// Remove client from list of active clients
        /// and close the connection
        /// </summary>
        protected virtual void RemoveAndCleanupClient()
        {
            // Remove this client
            Clients.TryRemove(Identifier, out var client);
            Client.Close();
        }

        #endregion
    }
}
