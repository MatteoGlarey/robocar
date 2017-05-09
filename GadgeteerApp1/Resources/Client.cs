using System;
using Microsoft.SPOT;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace GadgeteerApp1
{
    class SocketClient
    {
        Socket socket;

        Thread ReceiveThread;

        byte[] receiveBuffer = new byte[4096];

        public void Connect(string server, int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse(server), port));

                ReceiveThread = new Thread(Receive);
                ReceiveThread.Start();

                Send("Hello World!");

            }
            catch (Exception x)
            {
                Debug.Print(x.ToString());
            }


        }

        public void Send(string message)
        {
            socket.Send(new System.Text.UTF8Encoding().GetBytes(message));
        }

        public void Receive()
        {
            socket.Receive(receiveBuffer);

            char[] chars = new System.Text.UTF8Encoding().GetChars(receiveBuffer);

            //depends on how you want to process the chars, here we just concatenate a string with them.
            string data = "";

            for (int i = 0; i < chars.Length; i++)
            {
                data += chars[i];
            }

            Debug.Print(data);
        }
    }
}