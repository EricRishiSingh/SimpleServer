using Bio.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bio.Server.Tests
{
    /// <summary>
    /// Unit tests for the Server class
    /// </summary>
    [TestClass()]
    public class ServerTests
    {
        /// <summary>
        /// Test the server for the correct output
        /// </summary>
        [TestMethod()]
        public void RunServerTest()
        {
            // Make sure the server starts and can be canceled
            Server.StopServerToken.CancelAfter(15000);

            // Start the server
            Task.Run(() => Server.RunServer());

            // Start 2 clients
            var socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket1.Connect(IPAddress.Loopback, Server.Port);

            var socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket2.Connect(IPAddress.Loopback, Server.Port);

            // Send messages
            // Look at the test output to verify the server responses
            socket1.Send(Encoding.ASCII.GetBytes(ClientMessageConstants.Helo));
            Thread.Sleep(6000);
            socket1.Send(Encoding.ASCII.GetBytes(ClientMessageConstants.Count));
            Thread.Sleep(1000);
            socket1.Send(Encoding.ASCII.GetBytes(ClientMessageConstants.Connections));
            Thread.Sleep(1000);
            socket1.Send(Encoding.ASCII.GetBytes(ClientMessageConstants.Prime));
            Thread.Sleep(1000);
            socket1.Send(Encoding.ASCII.GetBytes(ClientMessageConstants.Terminate));
            Thread.Sleep(1000);

            // Wait for server to stop before exiting
            Server.StopServerToken.Token.WaitHandle.WaitOne();
        }
    }
}