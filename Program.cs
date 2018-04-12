using System;

namespace csharpSocket
{
    class Program
    {
        static readonly string[] serverKey = {"-s","Server"};
        static readonly string[] clientKey = {"-c","Client"};
        static readonly string[] portKey = {"-p","--port"};
        static readonly string[] ipKey = {"-i","--ip"};

        static readonly string defaultIp = "127.0.0.1";
        static readonly int defaultPort = 8888;
        static void Main(string[] args)
        {
            if(args.Length <= 0){
                ShowHelp();
                return;
            }
            string firstArg = args[0];
            if(firstArg == "-h" || firstArg == "--help"){
                ShowHelp();
                return;
            }
            
            string ip = defaultIp;
            int port = defaultPort;
            int ipIndex = FindParamIndex(ipKey,args);
            if(ipIndex >= 0){
                ip = FindNextParam(ipIndex,args);
            }
            int portIndex = FindParamIndex(portKey,args);
            if(portIndex >= 0){
                port = GetNumber(FindNextParam(ipIndex,args));
            }
            
            if(FindParamIndex(serverKey,args)>=0){
                StartServer(ip, port);
                return;
            }

            if(FindParamIndex(clientKey,args)>=0){
                StartClient(ip, port);
                return;
            }
            
            Console.WriteLine("[Fatal] Unknown Options");
            ShowHelp();
            return;
        }
        static void ShowHelp(){
            Console.WriteLine("Usage:  dotnet csharpSocket.dll OPTIONS [PARAMS]");
            Console.WriteLine("Usage:  dotnet run \"OPTIONS [PARAMS]\" (in source directory,compile and run)");
            Console.WriteLine("OPTIONS:  \n\t-c|Client \t\tStart as client");
            Console.WriteLine("\t-s|Server \t\tStart as server");
            Console.WriteLine("\t-p|--port 1234 \t\tPort listen or connect to");
            Console.WriteLine("\t-i|--ip 1.2.3.4 \t\tIP address");

            Console.WriteLine("\t-h|--help \t\tHelp");



            Console.WriteLine("PARAMS:  \n\tdefault port is 8888");
            Console.WriteLine("\tdefault ip address is 127.0.0.1");
        }

        static void StartServer(string ip, int port){
            if(ip.Length<=0){
                ip = defaultIp;
            }
            if(port == 0 || port > 65535){
                port = defaultPort;
            }
            SocketServer.Start(ip,port);
        }
        static void StartClient(string ip, int port){
            if(ip.Length<=0){
                ip = defaultIp;
            }
            if(port == 0 || port > 65535){
                port = defaultPort;
            }
            SocketClient.Start(ip, port);
        }

        static int FindParamIndex(string[] keys,string[] args){
            for(int x = 0; x < keys.Length; x++){
                for(int i = 0; i < args.Length; i++){
                    if(args[i] == keys[x]){
                        return i;
                    }
                }
            }
            
            return -1;
        }
        static string FindNextParam(int index,string[] args){
            if(args.Length <= index+1){
                return "";
            }
            return args[index+1];
        }

        static int GetNumber(string s){
            if(s.Length <= 0){
                return 0;
            }
            try{
                return Convert.ToInt32(s);
            }
            catch(Exception ex){
                Console.WriteLine(ex.ToString());
            }
            return 0;
        }
    }
}
