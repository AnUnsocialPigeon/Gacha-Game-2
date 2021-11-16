using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Gacha_Game_2.GameData {
    class Networking {
        /// <summary>
        /// Sends a request to a given network
        /// </summary>
        /// <param name="messageHeader"></param>
        /// <param name="messageBody"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string Send(int messageHeader, string messageBody, IPAddress ip) {
            // End of packet bytes
            string sendingMessage = messageHeader.ToString() + "<BDY>" + messageBody + "<EOF>";
            byte[] bytes = new byte[1024];

            // Connect to a Remote server  
            IPHostEntry client = Dns.GetHostEntry(ip);
            IPAddress ipAddress = client.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.    
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.    
            // Connect to Remote EndPoint + send  
            sender.Connect(remoteEP);
            byte[] msg = Encoding.ASCII.GetBytes(sendingMessage);
            sender.Send(msg);

            // Receive the response from the remote server.    
            int bytesRec = sender.Receive(bytes);
            string response = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            // Decode + split the sent data
            string header = response.Substring(0, response.IndexOf("<BDY>"));
            response = response.Substring(header.Length + 5, (response.Length - 10 - header.Length));

            // Release the socket.    
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

            return response;
        }
    }
    public enum NetworkHeaders {
        RequestCards
    }
}
