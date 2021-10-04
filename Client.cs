using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;


public class Client : MonoBehaviour
{
    private TcpClient tcpClient;

    public async Task Initialize(string ip, int port)
    {
        try
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port);
        }
        catch { }
    }

    public void Disconnect()
    {
        if (tcpClient.Connected)
            tcpClient.Close();
    }

    public bool Connected()
    {
        if (tcpClient != null)
            if (tcpClient.Connected)
                return true;

        return false;
    }

    public async Task Read()
    {
        var buffer = new byte[4096];
        var ns = tcpClient.GetStream();
        MemoryStream ms = new MemoryStream();
        while (true)
        {
            var bytesRead = await ns.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead <= 0) break; // Stream was closed
            ms.Write(buffer, 0, bytesRead);

            //while (!Form1.MessageFromOmron.Equals("")) Thread.Sleep(100);

            //Form1.MessageFromOmron = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            ms.Seek(0, SeekOrigin.Begin);
        }
        Debug.Log("connection closed");
    }


    public void BeginSend(string msg)
    {
        try
        {
            var bytes = Encoding.ASCII.GetBytes(msg + "\n");
            var ns = tcpClient.GetStream();
            ns.BeginWrite(bytes, 0, bytes.Length, EndSend, bytes);
        }
        catch { }
    }

    public void EndSend(IAsyncResult result)
    {
        var bytes = (byte[])result.AsyncState;
    }
}
