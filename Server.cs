﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Socket = System.Net.Sockets.Socket;

namespace GadgeteerApp1
{
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

    public class Server
    {
        public const int DEFAULT_SERVER_PORT = 8080;
        private Socket socket;
        private int port;

        public event DataReceivedEventHandler DataReceived;


        public Server()
            : this(DEFAULT_SERVER_PORT)
        { }

        public Server(int port)
        {
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(localEndPoint);
            socket.Listen(Int32.MaxValue);
            Debug.Print("Server Listening");

            new Thread(StartServerInternal).Start();
        }

        private void StartServerInternal()
        {
            while (true)
            {
                // Wait for a request from a client.
                Socket clientSocket = socket.Accept();
                Debug.Print("Client Accepted");
                // Process the client request.
                var request = new ProcessClientRequest(this, clientSocket);
                request.Process();
            }
        }

        private void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(this, e);
        }
        public static string BytesToString(byte[] bytes)
        {
            string str = string.Empty;
            for (int i = 0; i < bytes.Length; ++i)
                str += (char)bytes[i];

            return str;
        }

        private class ProcessClientRequest
        {
            private Socket clientSocket;
            private Server socket;

            public ProcessClientRequest(Server socket, Socket clientSocket)
            {
                this.socket = socket;
                this.clientSocket = clientSocket;
            }

            public void Process()
            {
                // Handle the request in a new thread.
                new Thread(ProcessRequest).Start();
            }

            private void ProcessRequest()
            {
                const int c_microsecondsPerSecond = 1000000;

                using (clientSocket)
                {
                    while (true)
                    {
                        try
                        {
                            if (clientSocket.Poll(5 * c_microsecondsPerSecond,
                                                                    SelectMode.SelectRead))
                            {
                                // If the buffer is zero-length, the connection has been closed
                                // or terminated.
                                if (clientSocket.Available == 0)
                                    break;
                                byte[] buffer = new byte[clientSocket.Available];
                                int bytesRead = clientSocket.Receive(buffer, clientSocket.Available,
                                                                             SocketFlags.None);
                                Debug.Print(buffer.ToString());
                                byte[] data = new byte[bytesRead];
                                buffer.CopyTo(data, 0);

                                DataReceivedEventArgs args = new DataReceivedEventArgs(
                                                              clientSocket.LocalEndPoint,
                                                              clientSocket.RemoteEndPoint, data);
                                socket.OnDataReceived(args);

                                if (args.ResponseData != null)
                                    clientSocket.Send(args.ResponseData);

                                if (args.Close)
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}