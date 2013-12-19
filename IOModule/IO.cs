using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KPP.Core.Debug;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Drawing;
using System.ComponentModel;
using Phidgets;
using Phidgets.Events;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace IOModule {


    public enum AvaibleCommands {NoCommand,XBeeSETCOM,XBeeSendTO,XBeeListDevices,XBeeSetDefaults }


    public static class IOFunctions {

        private static KPPLogger log = new KPPLogger(typeof(IOFunctions));

        public static string SerializeObject<T>(this T toSerialize) {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            StringWriter textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
        public static object DeserializeFromString(string objectData, Type type) {
            try {

                var serializer = new XmlSerializer(type);
                object result;

                using (TextReader reader = new StringReader(objectData)) {
                    result = serializer.Deserialize(reader);
                }
                return result;
            } catch (Exception exp) {
                log.Error(exp);
                Console.WriteLine(exp.Message);
                return null;
            }


        }

    }


    #region TCP Client


    public enum ClientStates { ClientConnected, ClientDisConnected }

    public class TCPClientConnection {

        private static KPPLogger log = new KPPLogger(typeof(TCPClientConnection));

        public delegate void ServerMessage(String[] Commands);
        public event ServerMessage OnServerMessage;

        //public delegate void ServerImage(String Request, String Inspection, Image<Bgr, Byte> NewImage);
        public delegate void ServerImage(String Request, String Inspection, Object NewImage);
        public event ServerImage OnServerImage;

        public enum ConnectionState { Connected, Connecting, Disconnected };

        public delegate void ConnectionStateChanged(TCPClientConnection client, ConnectionState NewState);

        public event ConnectionStateChanged OnConnectionStateChanged;

        private void DnsResolveCallback(IAsyncResult result) {
            try {

                IPAddress[] addr = Dns.EndGetHostAddresses(result);
                if (addr.Count() > 0) {
                    if (addr[0].AddressFamily == AddressFamily.InterNetwork) {
                        ipAddress = new IPAddress[] { addr[0] };
                        return;
                    }
                }
                ipAddress = null;

            } catch (Exception exp) {
                ipAddress = null;
                //if (!StaticObjects.isLoading) {
                    log.Error("Invalid Host:" + _address);
               // }
            }
        }

        string _address = "127.0.0.1";
        [XmlAttribute]
        public String Address {
            get {
                return _address;
            }
            set {

                if (_address != value) {
                    _address = value;
                }
            }
        }

        private int _Port = 7000;
        [XmlAttribute]
        public int Port {
            get { return _Port; }
            set { _Port = value; }
        }

        private Boolean m_Start=false;
        [XmlIgnore]
        public Boolean Start {
            get { return m_Start; }
            set {
                if (m_Start!=value) {

                    if (value) {
                        if (this.State == ConnectionState.Disconnected) {
                            this.Connect();
                        } 
                    }
                    else {
                        if (State!= ConnectionState.Connected) {
                            this.Disconnect();
                        }
                    }


                    m_Start = value;
                }
            }
        }

        ConnectionState _State = ConnectionState.Disconnected;
        [XmlIgnore]
        [ReadOnly(true)]
        public ConnectionState State {
            get {
                return _State;
            }
            set {
                if (_State != value) {
                    _State = value;
                    if (OnConnectionStateChanged != null) {
                        OnConnectionStateChanged(this, _State);
                    }
                }



            }
        }

        IPAddress[] ipAddress = null;
        private TcpClient tcpClient;
        private int failedConnectionCount;



        public TCPClientConnection() {
            State = ConnectionState.Disconnected;
            this.tcpClient = new TcpClient();
            this.Encoding = Encoding.Default;
            

        }


        public override string ToString() {
            return "Remote Connection";
        }

        /// <summary>
        /// The endoding used to encode/decode string when sending and receiving.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Attempts to connect to one of the specified IP Addresses
        /// </summary>
        public void Connect() {
            try {

                if (State == ConnectionState.Disconnected || State == ConnectionState.Connecting) {
                    //Set the failed connection count to 0


                    Interlocked.Exchange(ref failedConnectionCount, 0);
                    //Start the async connect operation

                    if (tcpClient != null) {
                        if (tcpClient.Client != null) {
                            tcpClient.Client.Close();
                        }
                        tcpClient = null;

                    }

                    try {
                        ipAddress = new IPAddress[] { System.Net.IPAddress.Parse(_address) };

                    } catch (FormatException exp) {
                        try {
                            //if (!StaticObjects.isLoading) {

                            log.Debug("Invalid IP (" + _address + ")");
                            log.Debug("Checking Host Name (" + _address + ")");
                            //}
                            ipAddress = Dns.GetHostAddresses(_address);
                            //ipAddress = Dns.GetHostAddresses(value);


                        } catch (Exception exp2) {

                            log.Error("Invalid HOST (" + _address + ")");

                            return;
                        }
                    }

                    tcpClient = new TcpClient();

                    //addresses[0] = System.Net.IPAddress.Parse(Address);
                    State = ConnectionState.Connecting;
                    Thread.Sleep(10);
                    tcpClient.BeginConnect(ipAddress, Port, ConnectCallback, null);


                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        public static byte[] Combine(byte[] first, byte[] second) {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public string WriteReadSync(string data) {
            //   NetworkStream networkStream = tcpClient.GetStream();


            //   return line;

            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.


                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9602);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    Byte[] totalbytes = new Byte[0];
                    String decoded = "";
                    int bytesRec = 0;
                    do {
                        bytesRec = sender.Receive(bytes);

                        decoded = decoded + Encoding.UTF8.GetString(bytes);
                        if (bytesRec < 1024) {
                            break;
                        }
                        //totalbytes=Combine(bytes, totalbytes);
                    } while (bytesRec > 0);
                    // Receive the response from the remote device.



                    Byte[] bytesdecoded = Convert.FromBase64String(decoded);
                    using (MemoryStream stream = new MemoryStream(bytesdecoded)) {
                        //Bitmap bmp = new Bitmap(stream);
                        //bmp.Save("img.bmp");
                        //Image<Bgr, Byte> teste = ;
                    }
                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                } catch (ArgumentNullException ane) {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                } catch (SocketException se) {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                } catch (Exception e) {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }



            return "";
        }

        /// <summary>
        /// Writes a string to the network using the defualt encoding.
        /// </summary>
        /// <param name="data">The string to write</param>
        /// <returns>A WaitHandle that can be used to detect
        /// when the write operation has completed.</returns>
        public void Write(string data) {
            byte[] bytes = Encoding.GetBytes(data);
            Write(bytes);
        }

        /// <summary>
        /// Writes an array of bytes to the network.
        /// </summary>
        /// <param name="bytes">The array to write</param>
        /// <returns>A WaitHandle that can be used to detect
        /// when the write operation has completed.</returns>
        public bool Write(byte[] bytes) {
            if (State == ConnectionState.Connected) {

                NetworkStream networkStream = tcpClient.GetStream();
                //Start async write operation
                networkStream.BeginWrite(bytes, 0, bytes.Length, WriteCallback, null);
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Callback for Write operation
        /// </summary>
        /// <param name="result">The AsyncResult object</param>
        private void WriteCallback(IAsyncResult result) {
            try {
                NetworkStream networkStream = tcpClient.GetStream();
                networkStream.EndWrite(result);
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        /// <summary>
        /// Callback for Connect operation
        /// </summary>
        /// <param name="result">The AsyncResult object</param>
        private void ConnectCallback(IAsyncResult result) {
            try {

                tcpClient.EndConnect(result);
            } catch (Exception exp) {
                //Increment the failed connection count in a thread safe way
                Interlocked.Increment(ref failedConnectionCount);
                if (failedConnectionCount >= ipAddress.Length) {
                    //We have failed to connect to all the IP Addresses
                    //connection has failed overall.
                    Disconnect();
                    log.Warn(exp.Message.ToString());
                    return;
                }
            }

            try {

                ////We are connected successfully.
                //NetworkStream networkStream = tcpClient.GetStream();
                //byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                ////Now we are connected start asyn read operation.
                //networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
                Thread.Sleep(10);
                State = ConnectionState.Connected;
                StateObject ClientObject = new StateObject();
                ClientObject.workSocket = tcpClient.Client;
                tcpClient.Client.BeginReceive(ClientObject.buffer, 0, StateObject.BufferSize, 0,
                           new AsyncCallback(ReadCallback), ClientObject);

                this.Write("");
            } catch (Exception exp) {
                Disconnect();
                log.Error(exp);
            }
        }



        public class StateObject {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();

            public void ClearArray() {
                buffer = new byte[BufferSize];
                sb = new StringBuilder();

            }
        }

        void ProcessCommand(Socket handler, String Command) {
            try {
                String[] Commands = Command.Split('|');
                if (Commands.Count() > 0) {


                    if (OnServerMessage != null) {
                        OnServerMessage(Commands);

                    }
                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }



        private void ReadCallback(IAsyncResult ar) {

            try {
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0) {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();
                    if (content.IndexOf("\n") > -1) {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",content.Length, content);
                        // Echo the data back to the client.

                        ProcessCommand(handler, content);
                        state.ClearArray();
                    }
                    // else {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                    // }
                }
                else if (bytesRead == 0) {

                    try {
                        State = ConnectionState.Disconnected;


                    } catch (Exception exp) {

                        log.Error(exp);
                    }

                }

            } catch (Exception exp) {
                Disconnect();
                log.Warn(exp.Message);
            }

        }

        public void Disconnect() {
            if (State == ConnectionState.Connected || State == ConnectionState.Connecting) {
                if (tcpClient != null) {
                    tcpClient.Close();
                    tcpClient = null;
                }
                State = ConnectionState.Disconnected;
            }

            OnConnectionStateChanged = null;
        }


    }

    #endregion


    #region TCP Server

    

    //public class TCPServerClient {

    //    public delegate void ClientStateChanged(TCPServerClient sender, ServerClientState NewState);
    //    public event ClientStateChanged OnClientStateChanged;

    //    private static KPPLogger log = new KPPLogger(typeof(TCPServerClient));

    //    [XmlIgnore]
    //    internal Thread ClientThread = null;

    //    public delegate void Message(TCPServerClient ServerClient, String[] Args);
    //    public event Message OnMessage;
    //    [XmlIgnore]
    //    public object locker = new object();

    //    ManualResetEvent sendDone = new ManualResetEvent(true);
    //    ManualResetEvent receiveDone = new ManualResetEvent(true);

    //    [XmlIgnore]
    //    public ManualResetEvent WaitNewClient = new ManualResetEvent(false);

    //    public void Send(String data) {

    //        try {
    //            if (_WorkSocket == null) {
    //                return;
    //            }
    //            sendDone.WaitOne();
    //            // Convert the string data to byte data using ASCII encoding.
    //            data += "|\n";
    //            byte[] byteData = Encoding.ASCII.GetBytes(data);

    //            // Begin sending the data to the remote device.
    //            _WorkSocket.BeginSend(byteData, 0, byteData.Length, 0,
    //               new AsyncCallback(SendCallback), _WorkSocket);
    //        } catch (SocketException exp) {

    //            this.Disconnect(false);
    //        }
    //    }

    //    private ServerClientState _state = ServerClientState.Waiting;

    //    [XmlIgnore]
    //    public ServerClientState State {
    //        get {
    //            return _state;
    //        }
    //        internal set {
    //            _state = value;
    //            if (OnClientStateChanged != null) {
    //                OnClientStateChanged(this, value);
    //            }
    //        }
    //    }

    //    private void SendCallback(IAsyncResult ar) {
    //        try {
    //            // Retrieve the socket from the state object.
    //            Socket handler = (Socket)ar.AsyncState;

    //            // Complete sending the data to the remote device.
    //            int bytesSent = handler.EndSend(ar);
    //            //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

    //            // handler.Shutdown(SocketShutdown.Both);
    //            //handler.Close();
    //            sendDone.Set();

    //        } catch (Exception e) {
    //            log.Warn(e.Message);
    //        }
    //    }


    //    // Client  socket.
    //    private Socket _WorkSocket = null;
    //    [XmlIgnore]
    //    public Socket WorkSocket {
    //        get { return _WorkSocket; }
    //    }


    //    //}
    //    // Size of receive buffer.
    //    private const int BufferSize = 1024;
    //    // Receive buffer.
    //    [XmlIgnore]
    //    public byte[] buffer = new byte[BufferSize];
    //    // Received data string.
    //    [XmlIgnore]
    //    public StringBuilder sb = new StringBuilder();

    //    public void ClearArray() {
    //        buffer = new byte[BufferSize];
    //        sb = new StringBuilder();

    //    }

    //    private Boolean _DoDisconnect = false;

    //    internal Boolean DoDisconnect {
    //        get { return _DoDisconnect; }

    //    }

    //    internal Boolean Disconnect(Boolean exit) {


    //        try {
    //            _DoDisconnect = exit;
                
                    
                

    //            if (WorkSocket != null) {
    //                if (WorkSocket.RemoteEndPoint != null) {
    //                    IPEndPoint remoteIpEndPoint = WorkSocket.RemoteEndPoint as IPEndPoint;

    //                    if (remoteIpEndPoint != null) {
    //                        log.Info("TCP Client Disconnected:" + remoteIpEndPoint.Address);
    //                    }
    //                } 
    //                WorkSocket.Close();
    //            }
    //            if (MainSocket != null) {
    //                //Clients[i].MainSocket.Shutdown(SocketShutdown.Both);
    //                MainSocket.Close();
    //            }
    //            OnMessage = null;

    //            WaitNewClient.Set();
    //        } catch (Exception exp) {

    //            log.Error(exp);
    //        }
    //        this.State = ServerClientState.Disconnected;
    //        return true;
    //    }






    //    public void Close() {
    //        _WorkSocket.Close();
    //        _WorkSocket = null;
    //        ClearEvents();
    //    }


    //    void ProcessCommand(Socket handler, String Command) {
    //        try {
    //            Command = Command.Replace("\n", "");
    //            Command = Command.Replace("\r", "");
    //            Command = Command.Replace("\b", "");
    //            String[] Commands = Command.Split('|');
    //            if (Commands.Count() > 0) {

    //                if (OnMessage != null) {
    //                    OnMessage(this, Commands.Where(a => a != null && a != "").ToArray());
    //                }

    //            }
    //        } catch (Exception exp) {

    //            log.Error(exp);
    //        }
    //    }



    //    internal void ReadCallback(IAsyncResult ar) {
    //        try {


    //            String content = String.Empty;

    //            // Retrieve the state object and the handler socket
    //            // from the asynchronous state object.
    //            TCPServerClient state = (TCPServerClient)ar.AsyncState;
    //            Socket handler = state.WorkSocket;

    //            // Read data from the client socket. 
    //            int bytesRead = handler.EndReceive(ar);

    //            if (bytesRead > 0) {
    //                // There  might be more data, so store the data received so far.
    //                state.sb.Append(Encoding.ASCII.GetString(
    //                    state.buffer, 0, bytesRead));

    //                // Check for end-of-file tag. If it is not there, read 
    //                // more data.
    //                content = state.sb.ToString();
    //                if (content.IndexOf("\n") > -1) {
    //                    // All the data has been read from the 
    //                    // client. Display it on the console.
    //                    //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",content.Length, content);
    //                    // Echo the data back to the client.
    //                    ProcessCommand(handler, content);
    //                    state.ClearArray();
    //                }
    //                // else {
    //                // Not all data received. Get more.
    //                handler.BeginReceive(state.buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), state);
    //                // }
    //            }
    //            else if (bytesRead == 0) {

    //                try {
    //                    Disconnect(false);


    //                } catch (Exception exp) {

    //                    log.Error(exp);
    //                }

    //            }

    //        } catch (Exception exp) {
    //            Disconnect(false);
    //            log.Warn(exp.Message);
    //        }
    //    }

    //    [XmlAttribute]
    //    public Boolean ProcessCommands { get; set; }


    //    private Boolean _LogInfo = false;
    //    [XmlAttribute]
    //    public Boolean LogInfo {
    //        get { return _LogInfo; }
    //        set { _LogInfo = value; }
    //    }

    //    private Boolean _LogDebug = false;
    //    [XmlAttribute]
    //    public Boolean LogDebug {
    //        get { return _LogDebug; }
    //        set { _LogDebug = value; }
    //    }

    //    private int _Port = 0;
    //    [XmlAttribute]
    //    public int Port {
    //        get { return _Port; }
    //        set { _Port = value; }
    //    }

    //    private String _Name = "";
    //    [XmlAttribute]
    //    public String Name {
    //        get { return _Name; }
    //        set {
    //            _Name = value;
    //        }
    //    }

    //    private Boolean _IsRemote = false;
    //    [XmlAttribute]
    //    public Boolean IsRemote {
    //        get { return _IsRemote; }
    //        set {
    //            if (_IsRemote != value) {
    //                _IsRemote = value;
    //            }
    //        }
    //    }

    //    Socket _MainSocket = null;
    //    [XmlIgnore]
    //    public Socket MainSocket {
    //        get { return _MainSocket; }
    //        internal set {
    //            _MainSocket = value;
    //        }
    //    }



    //    public TCPServerClient() {


    //    }


    //    public TCPServerClient(int port, string name) {
    //        Port = port;
    //        Name = name;

    //    }


    //    public void SetClientSocket(Socket workSocket) {
    //        _WorkSocket = workSocket;
    //        _WorkSocket.BeginReceive(buffer, 0, BufferSize, 0,
    //                        new AsyncCallback(ReadCallback), this);

    //    }



    //    public void ClearEvents() {
    //        OnMessage = null;

    //    }

    //}

    //public class TCPServerConnection {



    //    private static KPPLogger log = new KPPLogger(typeof(TCPServerConnection));



    //    public delegate void ClientMessage(TCPServerClient ServerClient, String[] Args);
    //    public event ClientMessage OnClientMessage;

    //    public delegate void ServerClientConnected(TCPServerClient ServerClient);
        
    //    //public event ServerClientConnected OnServerClientConnected;
    //    private event ServerClientConnected _OnServerClientConnected;
    //    public event ServerClientConnected OnServerClientConnected {
    //        add {
    //            if (_OnServerClientConnected == null || !_OnServerClientConnected.GetInvocationList().Contains(value)) {
    //                _OnServerClientConnected += value;
    //            }
    //        }
    //        remove {
    //            _OnServerClientConnected -= value;
    //        }
    //    }




    //    public delegate void ServerClientDisconnected(TCPServerClient ServerClient);

    //    private event ServerClientDisconnected _OnServerClientDisconnected;
    //    public event ServerClientDisconnected OnServerClientDisconnected {
    //        add {
    //            if (_OnServerClientDisconnected == null || !_OnServerClientDisconnected.GetInvocationList().Contains(value)) {
    //                _OnServerClientDisconnected += value;
    //            }
    //        }
    //        remove {
    //            _OnServerClientDisconnected -= value;
    //        }
    //    }

        




    //    [XmlIgnore]
    //    ManualResetEvent allDone = new ManualResetEvent(false);

    //    //[XmlAttribute]
    //    //public List<int> Ports { get; set; }



    //    [XmlAttribute]
    //    public Boolean StartOnLoad { get; set; }





    //    void DisconnectClients(Boolean disposeClients) {

    //        try {

    //            for (int i = 0; i < Clients.Count; i++) {
    //                if (disposeClients) {
    //                    if (Clients[i].Disconnect(true))

    //                        if (Clients[i].ClientThread != null) {
    //                            Clients[i].ClientThread.Join(1000);
    //                        }
    //                    Clients.RemoveAt(i); 
    //                } else {
    //                    Clients[i].Disconnect(false);
    //                }
    //            }
    //            OnClientMessage = null;
                

    //        } catch (Exception exp) {

    //            log.Error(exp);
    //        }

    //    }






    //    List<IPEndPoint> localEndPoints = new List<IPEndPoint>();

    //    private List<TCPServerClient> _Clients = new List<TCPServerClient>();

    //    public List<TCPServerClient> Clients {
    //        get { return _Clients; }
    //        set { _Clients = value; }
    //    }

    //    Boolean ServerStopped = false;

    //    public void threadListen(object objs) {



    //        TCPServerClient ServerClient = (TCPServerClient)(objs);
    //        ServerClient.OnClientStateChanged += new TCPServerClient.ClientStateChanged(ServerClient_OnClientStateChanged);

    //        do {
    //            try {
    //                Socket scon = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //                if (ServerClient.MainSocket == null) {
    //                    ServerClient.MainSocket = scon;
    //                }


    //                IPEndPoint sender = new IPEndPoint(IPAddress.Any, ServerClient.Port);
    //                EndPoint Remote = (EndPoint)(sender);


    //                scon.Bind(sender);
    //                scon.Listen(100);

    //                ServerClient.State = ServerClientState.Waiting;
    //                Socket handler = scon.Accept();


    //                // Get the socket that handles the client request.

                    

    //                ServerClient.SetClientSocket(handler);
    //                IPEndPoint remoteIpEndPoint = handler.RemoteEndPoint as IPEndPoint;
    //                log.Info("TCP Client connected:" + remoteIpEndPoint.Address);

    //                ServerClient.OnMessage += new TCPServerClient.Message(ServerClient_OnMessage);
    //                if (_OnServerClientConnected != null) {                        

    //                    _OnServerClientConnected(ServerClient);
    //                }
    //                ServerClient.State = ServerClientState.Connected;

    //                //Clients.Add(ServerClient);
    //                //  ServerClient.MainSocket.Disconnect(true);
    //                ServerClient.MainSocket = null;
    //                scon.Dispose();//= null;

    //                ServerClient.WaitNewClient.WaitOne();
    //                ServerClient.WaitNewClient.Reset();

    //            } catch (SocketException ex) {

    //                if (true) {

    //                }

    //            }

    //        } while (!ServerClient.DoDisconnect);


    //    }

    //    void ServerClient_OnClientStateChanged(TCPServerClient sender, ServerClientState NewState) {
    //        try {
                

    //            if (NewState== ServerClientState.Disconnected) {
                    
    //                if (_OnServerClientDisconnected!=null) {

                        
                      

    //                    _OnServerClientDisconnected(sender);
    //                }
    //            } else if (NewState== ServerClientState.Connected) {
                    
    //            }
    //        } catch (Exception exp) {

    //            log.Error(exp);
    //        }
    //    }



    //    Boolean StopServer = false;



    //    public void Dispose() {
    //        DisconnectClients(true);
    //    }

    //    void StartListening() {
    //        // Data buffer for incoming data.
    //        byte[] bytes = new Byte[1024];

    //        for (int i = 0; i < Clients.Count; i++) {
    //            localEndPoints.Add(new IPEndPoint(IPAddress.Any, Clients[i].Port));
    //        }





    //        // Create a TCP/IP sockets.

    //        for (int i = 0; i < Clients.Count; i++) {

    //            Thread thread = new Thread(threadListen);
    //            thread.IsBackground = true;
    //            Clients[i].ClientThread = thread;
    //            thread.Start(Clients[i]);
    //        }

    //        //State = ServerState.Started;


    //    }

    //    void ServerClient_OnMessage(TCPServerClient ServerClient, string[] Args) {
    //        if (OnClientMessage != null) {
    //            OnClientMessage(ServerClient, Args);
    //        }
    //    }

    //    void ClearEvents() {
    //        OnClientMessage = null;
    //        _OnServerClientConnected = null;
    //    }

    //    public void Stop() {
    //        DisconnectClients(false);

    //    }

    //    //Thread m_clientListenerThread;

    //    public void Start() {
    //        StartListening();
    //    }


    //    public TCPServerConnection(List<TCPServerClient> clients)
    //    :this(){
    //        Clients.AddRange(clients);
    //    }

    //    public TCPServerConnection() {

    //        StartOnLoad = true;
    //    }
    //}


    #endregion

    #region Console


    public class ConsoleCommand {

        private static KPPLogger log = new KPPLogger(typeof(ConsoleCommand));

        List<ConsoleCommand> _commandList = new List<ConsoleCommand>();

        public List<ConsoleCommand> CommandList {
            get { return _commandList; }
            private set { _commandList = value; }
        }

        Boolean _RaiseEvent = false;

        public Boolean RaiseEvent {
            get { return _RaiseEvent; }
            set { _RaiseEvent = value; }
        }

        AvaibleCommands _commandName = AvaibleCommands.NoCommand;

        public AvaibleCommands CommandName {
            get { return _commandName; }
            private set { _commandName = value; }
        }

        String _commandParam = "";

        public String CommandParam {
            get { return _commandParam; }
            private set { _commandParam = value; }
        }

        public ConsoleCommand(AvaibleCommands commandName, String commandParam, List<ConsoleCommand> commands) {
            CommandName = commandName;
            CommandParam = commandParam;
            CommandList = commands;
        }

        public ConsoleCommand(AvaibleCommands commandName, String commandParam, Boolean Raiseevent) {
            CommandName = commandName;
            CommandParam = commandParam;
            RaiseEvent = Raiseevent;

        }

        public override string ToString() {
            return CommandName + "|" + CommandParam;
        }
        public ConsoleCommand() {
        }

    }

    public class CommandParser {

        private static KPPLogger log = new KPPLogger(typeof(CommandParser));

        public delegate void CommandParsed(AvaibleCommands CommandName, List<String> Commandargs);
        public event CommandParsed OnCommandParsed;

        private List<ConsoleCommand> _RootCommands = new List<ConsoleCommand>();

        public List<ConsoleCommand> RootCommands {
            get { return _RootCommands; }
            private set { _RootCommands = value; }
        }


        public void ParseCommands(String CmdInput) {
            try {
                List<String> Args = CmdInput.Split('|').ToList();

                if (Args.Count == 0) {
                    return;
                }

                List<ConsoleCommand> cmds = RootCommands;
                int paramcount = 0;
                while (cmds != null) {
                    if (paramcount == Args.Count || cmds.Count == 0) {
                        break;
                    }
                    String paramstr = Args[paramcount];
                    ConsoleCommand cmd = cmds.Find(param => param.CommandParam == paramstr);
                    if (cmd != null) {

                        if (cmd.RaiseEvent) {
                            paramcount++;
                            if (OnCommandParsed != null)


                                OnCommandParsed(cmd.CommandName, Args.GetRange(paramcount, Args.Count - paramcount));
                        }
                        paramcount++;
                        cmds = cmd.CommandList;

                    } else {
                        break;
                    }
                }
            } catch (Exception exp) {
                log.Debug(exp.Message);
                log.Error(exp);

            }

        }

        public CommandParser() {

        }
    }


    #endregion

    #region Serial


    public enum SerialState { Opened, Closed };


    public class SerialConnectionServer {


        private List<SerialConnection> _Serials = new List<SerialConnection>();

        public List<SerialConnection> Serials {
            get { return _Serials; }
            set { _Serials = value; }
        }

        public void Dispose() {
            foreach (SerialConnection item in Serials) {
                item.Dispose();
            }
        }

        public SerialConnectionServer() {
        }
    }

    public class SerialConnection {

        private static KPPLogger log = new KPPLogger(typeof(SerialConnectionServer));

        public delegate void Message(SerialConnection serial, String[] Args);
        public static event Message OnMessage;


        private SerialState _State = SerialState.Closed;
        [XmlIgnore]
        public SerialState State {
            get { return _State; }
            set { _State = value; }
        }

        [XmlAttribute]
        public Boolean ProcessCommands { get; set; }

        private String _Port = "";
        [XmlAttribute]
        public String Port {
            get { return _Port; }
            set {
                if (!_ComPort.IsOpen) {
                    _Port = value;
                    _ComPort.PortName = value;
                }
            }
        }

        private String _Name = "New Serial";
        [XmlAttribute]
        public String Name {
            get { return _Name; }
            set { _Name = value; }
        }

        bool doexit = false;
        ManualResetEvent _event = new ManualResetEvent(true);
        private void ReadThread() {


            while (!doexit) {
                try {
                    _event.WaitOne();
                    if (_ComPort.BytesToRead > 0) {
                        Thread.Sleep(1);

                        string data = _ComPort.ReadLine();


                        data = data.Replace("\n", "");
                        data = data.Replace("\r", "");
                        data = data.Replace("\b", "");
                        String[] Commands = data.Split('|');
                        if (Commands.Count() > 0) {
                            Console.WriteLine("Command Received:" + data);
                            if (OnMessage != null) {
                                OnMessage(this, Commands);
                            }

                        }
                    }
                    else {
                        Thread.Sleep(200);
                    }
                } catch (Exception exp) {

                    //                    throw;
                }
            }
            Console.WriteLine("Serial Thread exit");
        }

        Thread readThread = null;
        public void Connect() {
            try {
                try {
                    if (!_ComPort.IsOpen) {

                        _ComPort.ReadTimeout = 1000;
                        _ComPort.NewLine = "\n";
                        _ComPort.Open();
                        Thread.Sleep(10);
                        if (readThread == null) {
                            readThread = new Thread(new ThreadStart(ReadThread));
                            readThread.Start();
                            readThread.IsBackground = true;
                        }
                        else {
                            _event.Set();
                        }



                        State = SerialState.Opened;
                    }
                } catch (Exception exp) {

                    log.Error(exp);
                }
            } catch (Exception exp) {

                log.Error(exp);

            }
        }

        public void Disconnect() {
            try {
                if (_ComPort.IsOpen) {
                    _event.Reset();
                    _ComPort.Close();
                    State = SerialState.Closed;
                }
            } catch (Exception exp) {

                log.Error(exp);

            }
        }

        public void Dispose() {
            Disconnect();
            doexit = true;
            _event.Set();
            if (readThread != null) {
                readThread.Join(1500);
            }

        }

        public void Send(String line) {
            if (_ComPort.IsOpen) {
                _ComPort.WriteLine(line);
            }
        }

        SerialPort _ComPort = new SerialPort();

        private Boolean _Active = false;
        [XmlAttribute]
        public Boolean Active {
            get { return _Active; }
            set {
                if (_Active != value) {
                    _Active = value;
                    if (Active) {
                        Connect();
                    }
                    else {
                        Disconnect();
                    }
                }

            }
        }

        private Boolean _IsRemote = false;
        [XmlAttribute]
        public Boolean IsRemote {
            get { return _IsRemote; }
            set {
                if (_IsRemote != value) {
                    _IsRemote = value;
                }
            }
        }

        public SerialConnection() {


        }

        void _ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {

        }

        public override string ToString() {
            return Name;
        }
    }

    #endregion

    #region PHidget





    #region Phidgets selector

    public class PhidgetsSelector : System.Drawing.Design.UITypeEditor {
        // this is a container for strings, which can be 
        // picked-out
        ListBox Box1 = new ListBox();
        IWindowsFormsEditorService edSvc;
        // this is a string array for drop-down list
        //internal static List<CameraInfo> RemoteCameras = new List<CameraInfo>();



        public PhidgetsSelector() {
            Box1.BorderStyle = BorderStyle.None;
            // add event handler for drop-down box when item 
            // will be selected
            Box1.Click += new EventHandler(Box1_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {
            Box1.Items.Clear();


            foreach (InterfaceKit  item in PhidgetsIO.BoardsAvaible) {
                Box1.Items.Add(item.SerialNumber);
            }

            

            Box1.Height = Box1.PreferredHeight;

           



            // window.
            edSvc =
               (IWindowsFormsEditorService)provider.
               GetService(typeof
               (IWindowsFormsEditorService));

            if (edSvc != null) {



                edSvc.DropDownControl(Box1);
                if (Box1.SelectedItem == null) {
                    return value;
                } else {
                    return Box1.SelectedItem;
                }

            }
            return value;
        }



        private void Box1_Click(object sender, EventArgs e) {

            edSvc.CloseDropDown();
        }
    }

    #endregion
    
    public sealed class PhidgetsIO  {

        public static List<InterfaceKit> BoardsAvaible = new List<InterfaceKit>();

        public delegate void BordAvaible(int boardSerial);
        private static event BordAvaible _OnBordAvaible;
        public static event BordAvaible OnBordAvaible {
            add {
                if (_OnBordAvaible == null || !_OnBordAvaible.GetInvocationList().Contains(value)) {
                    _OnBordAvaible += value;
                }
            }
            remove {
                _OnBordAvaible -= value;
            }
        }

        private static InterfaceKit m_ifkit = new InterfaceKit();
        private static KPPLogger log = new KPPLogger(typeof(PhidgetsIO));

        private Int32 _SerialNumber = -1;
        [DisplayName("Serial Number"), XmlAttribute, EditorAttribute(typeof(PhidgetsSelector), typeof(UITypeEditor))]
        public Int32 SerialNumber {
            get { return _SerialNumber; }
            set {
                if (_SerialNumber!=value) {
                    _SerialNumber = value;
                    IfKit = PhidgetsIO.BoardsAvaible.Find(phi => phi.SerialNumber == value);
                     
                }
            }
        }

        private  Int32 IoInMinCount { get; set; }

        private  Int32 IoInMaxCount { get; set; }
        private  Int32 IoOutMinCount { get; set; }
        private  Int32 IoOutMaxCount { get; set; }

        private InterfaceKit _IfKit = null;
        [XmlIgnore,Browsable(false)]
        private InterfaceKit IfKit {
            get { return _IfKit; }
            set { 
                _IfKit = value;
                if (IfKit!=null) {
                    this.Connect();
                }
            }
        }

        private  AttachEventHandler _ev_attach = null;
        private  DetachEventHandler _ev_dettach = null;
        private  InputChangeEventHandler _ev_inputChanged = null;
        private  OutputChangeEventHandler _ev_outputChanged = null;
        private  Phidgets.Events.ErrorEventHandler _ev_error = null;
        private  Boolean IsConnected = false;
        private  Boolean IsInitialized = false;


        private String _Name;
        [XmlAttribute,ReadOnly(true)]
        public String Name {
            get { return _Name; }
            set { _Name = value; }
        }

        [XmlAttribute,DisplayName("Pre Inspection Output")]
        public int PreInspectionOutput { get; set; }

        public static void InitializePhidgets() {
            m_ifkit.Attach += new AttachEventHandler(m_ifkit_Attach);
            m_ifkit.open();
            
            //m_ifkit.Detach += new DetachEventHandler(ifKit_Detach);
            //m_ifkit.Error += new ErrorEventHandler(ifKit_Error);

        }

        static void m_ifkit_Attach(object sender, AttachEventArgs e) {
            InterfaceKit ifKit = (InterfaceKit)sender;
            if (!BoardsAvaible.Contains(ifKit)) {
                BoardsAvaible.Add(ifKit);
                if (_OnBordAvaible != null) {
                    _OnBordAvaible(ifKit.SerialNumber);
                }
            }
        }

        public PhidgetsIO() {
            
            SerialNumber = -1;
            PreInspectionOutput = -1;
            PhidgetsIO.OnBordAvaible += new BordAvaible(PhidgetsIO_OnBordAvaible);
        }

        void PhidgetsIO_OnBordAvaible(int boardSerial) {
            IfKit = PhidgetsIO.BoardsAvaible.Find(board => board.SerialNumber == boardSerial);

        }

        public override string ToString() {
            return "Phidgets IO";
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <returns></returns>
        public bool Connect() {
            if (!IsConnected) {
                try {
                    Name = IfKit.Name;
                    IfKit.Attach += _ev_attach = new AttachEventHandler(_ifKit_Attach);
                    IfKit.Detach += _ev_dettach = new DetachEventHandler(_ifKit_Detach);
                    //_ifKit.Error += _ev_error = new Phidgets.Events.ErrorEventHandler(_ifKit_Error);
                    //_ifKit.InputChange += _ev_inputChanged = new InputChangeEventHandler(_ifKit_InputChange);
                    //_ifKit.OutputChange += _ev_outputChanged = new OutputChangeEventHandler(_ifKit_OutputChange);

                    //if (SerialNumber > -1) {
                    //    _ifKit.open(SerialNumber);
                    //} else {
                    IfKit.open(SerialNumber);
                    //}
                    IsInitialized = false;
                    IsConnected = true;
                    //RaiseOnIoModuleConnected();
                } catch (Exception exp) {
                    log.Error("Unable to connect to Phidget interface", exp);
                    if (IfKit != null) {
                        IfKit.close();
                    }
                    IfKit = null;
                }

                return IsConnected;
            }
            return false;
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        /// <returns></returns>
        public bool Disconnect() {
            if (IsConnected) {
                
                if (IfKit != null) {
                    try {
                       // IfKit.close();

                        IfKit.Attach -= _ev_attach;
                        _ev_attach = null;
                        IfKit.Detach -= _ev_dettach;
                        _ev_dettach = null;
                        IfKit.Error -= _ev_error;
                        _ev_error = null;
                        IfKit.InputChange -= _ev_inputChanged;
                        _ev_inputChanged = null;
                        IfKit.OutputChange -= _ev_outputChanged;
                        _ev_outputChanged = null;

                        IsConnected = false;
                        //RaiseOnIoModuleDisconnected();
                    } catch (Exception exp) {
                        log.Error("Error disconnecting from Phidget interface", exp);
                    }
                    IfKit = null;
                    return !IsConnected;
                }
            }
            return false;
        }

        ///// <summary>
        ///// Reads the data.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public override IoModuleOperationResult PerformOperation(string id) {
        //    return new IoModuleOperationResult(id, false, null);
        //}

        /// <summary>
        /// Handles the Attach event of the _ifKit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Phidgets.Events.AttachEventArgs"/> instance containing the event data.</param>
        private void _ifKit_Attach(object sender, AttachEventArgs e) {
            InterfaceKit ifKit = sender as InterfaceKit;
            if (ifKit != null) {
                IsInitialized = true;
                if (SerialNumber < 0 || ifKit.SerialNumber == SerialNumber) {
                    IsInitialized = true;
                    //RaiseOnIoModuleHardwareInitialized();
                }
            }
        }

        /// <summary>
        /// Handles the Detach event of the _ifKit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Phidgets.Events.DetachEventArgs"/> instance containing the event data.</param>
        private void _ifKit_Detach(object sender, DetachEventArgs e) {
            InterfaceKit ifKit = sender as InterfaceKit;
            if (ifKit != null) {
                IsInitialized = false;
                Disconnect();
            }
        }

        private void _ifKit_Error(object sender, Phidgets.Events.ErrorEventArgs e) {

        }

        private void _ifKit_InputChange(object sender, InputChangeEventArgs e) {
            NotifyInputChange(e.Index, e.Value);
        }

        private void NotifyInputChange(int number, bool status) {
            //RaiseOnIoModuleHardwareStatus(number.ToString(), "IN_" + (status ? "ON" : "OFF"), null, IoModuleHardwareStatusEnum.Running);
        }

        private void _ifKit_OutputChange(object sender, OutputChangeEventArgs e) {
            NotifyOutputChange(e.Index, e.Value);
        }

        private void NotifyOutputChange(int number, bool status) {
            //RaiseOnIoModuleHardwareStatus(number.ToString(), "OUT_" + (status ? "ON" : "OFF"), null, IoModuleHardwareStatusEnum.Running);

           
        }

        public Boolean SendCommand(string comm, params string[] args) {
            if (IfKit==null) {
                this.Connect();
            }
            if (IfKit != null) {
                if (comm == "SET_OUT" && args.Length > 1) {
                    int idx = -1;
                    if (int.TryParse(args[0], out idx)) {
                        if (idx > -1 && idx < IfKit.outputs.Count) {
                            IfKit.outputs[idx] = args[1] == "ON" || args[1] == "1";
                        }
                        return true;
                    }
                }
            }
            return false;
        }



        
    }


    #endregion



    #region BASE IO

    
    [XmlInclude(typeof(PhidgetsIO))]
    public abstract class BaseIO {

        private String _IOName;
        [XmlAttribute]
        public String IOName {
            get { return _IOName; }
            set { _IOName = value; }
        }

        

        public BaseIO() {

        }

    }


    #endregion
}
