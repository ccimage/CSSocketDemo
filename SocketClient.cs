/*
 * File: /home/che/Work/test/csharpSocket/SocketClient.cs
 * Project: /home/che/Work/test/csharpSocket
 * Created Date: Thursday, April 12th 2018, 9:44:56 am
 * Author: ccimage
 * -----
 * Last Modified: Thu Apr 12 2018
 * Modified By: ccimage
 * -----
 * Copyright (c) 2018 <<company>>
 * 
 */

using System;
using System.Net;  
using System.Net.Sockets;
using System.Text;
using System.Threading;  

namespace csharpSocket
{  
    class SocketClient  
    {  
        private static byte[] result = new byte[1024];  
        public static void Start(string ipStr, int port)  
        {  
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse(ipStr);  
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
            try  
            {  
                clientSocket.Connect(new IPEndPoint(ip, port)); //配置服务器IP与端口  
                Console.WriteLine("Connect to server {0}:{1}", ipStr, port);  
            }  
            catch  
            {  
                Console.WriteLine("(Error)Failed connect to server.");  
                return;  
            }  
            //通过clientSocket接收数据  
            int receiveLength = clientSocket.Receive(result);  
            Console.WriteLine("received from server, message：{0}",Encoding.UTF8.GetString(result,0,receiveLength));  
            //通过 clientSocket 发送数据  
            for (int i = 0; i < 10; i++)  
            {  
                try  
                {  
                    Thread.Sleep(1000);    //等待1秒钟  
                    string sendMessage = "test message datetime=" + DateTime.Now;  
                    clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));  
                    Console.WriteLine("Send to server message：{0}" , sendMessage);  
                }  
                catch  
                {  
                    clientSocket.Shutdown(SocketShutdown.Both);  
                    clientSocket.Close();  
                    break;  
                }  
            }  
            Console.WriteLine("Finish. \nPress ENTER exit.");  
            Console.ReadLine();  
        }  
    }  
}  