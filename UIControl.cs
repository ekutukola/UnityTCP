using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;




public class UIControl : MonoBehaviour
{

    //objects
    public Text textIP;
    public Text textPORT;
    public Text textPASSWORD;
    public Text textDeneme;




    //data
    public string ip;
    public string port;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
 

    }


    private static byte[] MessageToByteArray(string message, Encoding encoding)
    {
        var byteCount = encoding.GetByteCount(message);
        if (byteCount > byte.MaxValue)
            throw new ArgumentException("Message size is greater than 255 bytes in the provided encoding");
        var byteArray = new byte[byteCount + 1];
        byteArray[0] = (byte)byteCount;
        encoding.GetBytes(message, 0, message.Length, byteArray, 1);
        return byteArray;
    }

    
    public void connectBtn()
    {

        string message = textDeneme.text;
        var byteArray = MessageToByteArray(message, Encoding.ASCII);
        using (var tcpClient = new TcpClient())
        {
            tcpClient.Connect("192.168.1.133", 5000);
            using (var networkStream = tcpClient.GetStream())
            using (var bufferedStream = new BufferedStream(networkStream))
            {
                // Send three exactly the same messages.


                Console.WriteLine("1st");
                bufferedStream.Write(byteArray, 0, byteArray.Length);
                Console.WriteLine("2nd");
                bufferedStream.Write(byteArray, 0, byteArray.Length);
                Console.WriteLine("3rd");
                bufferedStream.Write(byteArray, 0, byteArray.Length);
               
            }
        }


    } 
}
