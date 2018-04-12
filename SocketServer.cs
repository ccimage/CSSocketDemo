/*
 * File: /home/che/Work/test/csharpSocket/SocketServer.cs
 * Project: /home/che/Work/test/csharpSocket
 * Created Date: Thursday, April 12th 2018, 9:43:14 am
 * Author: ccimage
 * -----
 * Last Modified: Thu Apr 12 2018
 * Modified By: ccimage
 * -----
 * Copyright (c) 2018 <<company>>
 * 
 */

using System;
using System.Net.Sockets;  
using System.Net;  
using System.Threading;
using System.Text;

namespace csharpSocket
{  
    class SocketServer  
    {  
        private static byte[] result = new byte[1024]; 
        static Socket serverSocket;  
        public static void Start(string ipStr, int port)
        {  
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse(ipStr);  
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
            serverSocket.Bind(new IPEndPoint(ip, port));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  
            Console.WriteLine("Start listening on {0}", serverSocket.LocalEndPoint.ToString());  
            //通过Clientsoket发送数据  
            Thread myThread = new Thread(ListenClientConnect);  
            myThread.Start();  
            Console.ReadLine();  
        }  
  
        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private static void ListenClientConnect()  
        {  
            //while (true)  
            {  
                Console.WriteLine("Listen thread start.");
                Socket clientSocket = serverSocket.Accept();  
                clientSocket.Send(Encoding.UTF8.GetBytes("Server Say Hello"));  
                Thread receiveThread = new Thread(ReceiveMessage);  
                receiveThread.Start(clientSocket);  
            }  
        }  
  
        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private static void ReceiveMessage(object clientSocket)  
        {  
            Socket myClientSocket = (Socket)clientSocket;  
            while (true)  
            {  
                try  
                {  
                    
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);  
                    if(receiveNumber<=0){
                        break;
                    }
                    Console.WriteLine("received  from {0}, message {1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(result, 0, receiveNumber));  
                }  
                catch(Exception ex)  
                {  
                    Console.WriteLine(ex.Message);  
                    myClientSocket.Shutdown(SocketShutdown.Both);  
                    myClientSocket.Close();  
                    break;  
                }  
            }  

            Console.WriteLine("Listen thread terminated. \nPress ENTER exit.");  
        }  
    }  
}  