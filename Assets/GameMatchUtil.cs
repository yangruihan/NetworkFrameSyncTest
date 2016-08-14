using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Ruihanyang.Game
{
    public class GameMatchUtil : MonoBehaviour
    {
        private static int GAME_SERVER_PORT = 8998;
        private static string GAME_SERVER_IP = "10.66.218.174";

        private static int ACCEPT_PORT = 8999;

        [HideInInspector]
        public static string HostIp = "";
        [HideInInspector]
        public static string LocalIp = "";

        /// <summary>
        /// 得到本地IP
        /// </summary>
        /// <returns></returns>
        private static string GetLocalIps()
        {
            string ipStream = "";

            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);

            foreach (var localAddr in localhost.AddressList)
            {
                ipStream += localAddr.ToString() + ";";
            }

            return ipStream;
        }

        private static string GetIpv4Ip()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);

            foreach (var localAddr in localhost.AddressList)
            {
                string ip = localAddr.ToString();
                string pattern = @"\d+\.\d+\.\d+\.\d+";
                if (Regex.IsMatch(ip, pattern))
                {
                    return ip;
                }
            }

            return "";
        }

        /// <summary>
        /// 发送本地IP到服务器
        /// </summary>
        public static void SendLocalIpMessageToServer()
        {
            try
            {
                IPAddress ip = IPAddress.Parse(GAME_SERVER_IP);
                IPEndPoint ipe = new IPEndPoint(ip, GAME_SERVER_PORT);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                Debug.Log("连接服务器中...");

                socket.Connect(ipe);
                // 获得本地IP，并发送给服务器
                string sendStr = GetLocalIps();
                byte[] bs = Encoding.ASCII.GetBytes(sendStr);

                Debug.Log("发送消息...");

                socket.Send(bs, bs.Length, 0);
                socket.Close();

                Debug.Log("消息发送成功！");
            }
            catch (ArgumentNullException e)
            {
                Debug.Log("argumentNullException: " + e.ToString());
            }
            catch (SocketException e)
            {
                Debug.Log("SocketException: " + e.ToString());
            }
        }

        public static void AcceptHostIpMessageFromServer()
        {
            IPAddress ip = IPAddress.Parse(GetIpv4Ip());
            IPEndPoint ipe = new IPEndPoint(ip, ACCEPT_PORT);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipe);
            socket.Listen(0);

            Debug.Log("正在等待游戏服务器返回主机IP信息...");

            Socket temp = socket.Accept();

            Debug.Log("与游戏服务器建立连接...");

            string recvStr = "";
            byte[] recvBytes = new byte[1024];
            int bytes;
            bytes = temp.Receive(recvBytes, recvBytes.Length, 0);
            recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);

            LocalIp = recvStr.Split(';')[0];
            HostIp = recvStr.Split(';')[1];
            Debug.Log("主机信息：" + recvStr);

            string SendStr = "Accept OK";
            byte[] bs = Encoding.ASCII.GetBytes(SendStr);
            temp.Send(bs, bs.Length, 0);
            temp.Close();
            socket.Close();
        }
    }
}
