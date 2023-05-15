using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Old data gathering script that uses local Sockets.
/// This has been replaced by HiveMQ cloud connectivity through the HiveMQConnector.cs script
/// </summary>
public class SocketTest : MonoBehaviour, IDataPublisher
{
    [SerializeField] private int _socket = 12000;

    // TODO: MOVE THIS TO A SEPARATE CLASS!!!!!
    // ABSOLUTELY NO REASON TO HAVE THIS ON THE SAME SCRIPT THAT'S IN CHARGE OF SOCKET CONNECTIONS
    // WHY IS THIS HERE
    [SerializeField] private DataStructure[] dataStructures;
    /*
    [Range(1,10)]
    [SerializeField] private int number_of_outputs;
    [SerializeField] private List<AbsValuelistener> _readers;
    */

    //PubSub to other parts of Unity code --------------------------------
    private Action<string[]> PublishStringArray;


    public DataStructure[] GetDataStructures()
    {
        return dataStructures;
    }

    void PublishString(string outString)
    {
        string[] string_array = outString.Split(',');
        PublishStringArray(string_array);
    }

    public void Subscribe(Action<string[]> function)
    {
        PublishStringArray += function;
    }

    //Socket connection code ---------------------------------------------
    System.Threading.Thread SocketThread;
    volatile bool keepReading = false;

    // Use this for initialization
    void Start()
    {
        // !!!!!!!!!!! FOLLOWING CODE WAS COMMENTED TO DISABLE SOCKET CONNECTIONS, !!!!!!!!!!! 
        // !!!!!!!!!!! AS HIVEMQ IS BEING USED INSTEAD                             !!!!!!!!!!!

        // UNCOMMENT IF THIS NEEDS TO BE REUSED AGAIN
        // OTHERWISE, THIS SCRIPT SHOULD BE REPURPOSED OR OUTRIGHT REMOVED FROM ANY GAMEOBJECTS
        // '[SerializeField] private DataStructure[] dataStructures;' SHOULD BE MOVED SOMEWHERE ELSE
        // BEFORE FULLY DISCARTING THIS CODE
        // AS THIS IS BEING USED BY 'DisplayDataContainer.cs'

        /*
        Application.runInBackground = true;
        startServer();
        */
    }

    void startServer()
    {
        SocketThread = new System.Threading.Thread(networkCode);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }



    private string getIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        //host = Dns.GetHostEntry(Dns.GetHostName());
        host = Dns.GetHostEntry("localhost");
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
            }

        }
        return localIP;
    }


    Socket listener;
    Socket handler;

    void networkCode()
    {
        string data;

        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // host running the application.
        Debug.Log("Ip " + getIPAddress().ToString());
        IPAddress[] ipArray = Dns.GetHostAddresses(getIPAddress());
        IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], _socket);
        Debug.Log(ipArray[0]);

        // Create a TCP/IP socket.
        listener = new Socket(ipArray[0].AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and 
        // listen for incoming connections.

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.
            while (true)
            {
                try
                {

                    keepReading = true;

                    // Program is suspended while waiting for an incoming connection.
                    Debug.Log("Waiting for Connection");     //It works

                    handler = listener.Accept();
                    Debug.Log("Client Connected");     //It doesn't work
                    data = null;

                    // An incoming connection needs to be processed.
                    while (keepReading)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        //Debug.Log("Received from Server");

                        if (bytesRec <= 0)
                        {
                            keepReading = false;
                            handler.Disconnect(true);
                            break;
                        }
                        //Use (data += ...) if a connection should send one big message and then disconnect
                        //data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        //Use (data = ...) if a connection should constantly send independent messages
                        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        //Debug.Log(data);
                        PublishString(data);
                        //setOutString(data);

                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(1);
                    }
                    //PublishString(data);

                    System.Threading.Thread.Sleep(1);
                }
                catch (SocketException se)
                {
                    Debug.Log("Connection Lost.");
                    Debug.Log(se.ToString());
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            Debug.Log("Terminating Socket.");
        }
    }

    void stopServer()
    {
        keepReading = false;

        //stop thread
        if (SocketThread != null)
        {
            SocketThread.Abort();
        }

        if (handler != null && handler.Connected)
        {
            handler.Disconnect(false);
            Debug.Log("Disconnected!");
        }
    }

    void OnDisable()
    {
        stopServer();
    }
}
