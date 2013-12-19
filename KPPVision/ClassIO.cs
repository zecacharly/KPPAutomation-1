using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using KPP.Core.Debug;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
//using DejaVu;
using System.Collections.Concurrent;
using System.Reflection;
using RaspberryPiDotNet;
using System.IO.Ports;
using ClassInspection;


namespace ClassInspection {





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

    public class TCPServerClient {

        public delegate void ClientStateChanged(TCPServerClient sender, ServerClientState NewState);
        public event ClientStateChanged OnClientStateChanged;

        private static KPPLogger log = new KPPLogger(typeof(TCPServerClient));

        [XmlIgnore]
        internal Thread ClientThread=null;

        public delegate void Message(TCPServerClient ServerClient, String[] Args);
        public event Message OnMessage;
        [XmlIgnore]
        public object locker = new object();

        ManualResetEvent sendDone = new ManualResetEvent(true);
        ManualResetEvent receiveDone = new ManualResetEvent(true);

        [XmlIgnore]
        public ManualResetEvent WaitNewClient = new ManualResetEvent(false);

        public void Send(String data) {

            if (_WorkSocket==null) {
                return;
            }
            sendDone.WaitOne();
            // Convert the string data to byte data using ASCII encoding.
            data += "|\n";
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            _WorkSocket.BeginSend(byteData, 0, byteData.Length, 0,
               new AsyncCallback(SendCallback), _WorkSocket);
        }

        private ServerClientState _state = ServerClientState.Waiting;

        [XmlIgnore]
        public ServerClientState State {
            get {
                return _state;
            }
            internal set {
                _state = value;
                if (OnClientStateChanged != null) {
                    OnClientStateChanged(this, value);
                }
            }
        }

        private void SendCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                // handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
                sendDone.Set();

            } catch (Exception e) {
                log.Warn(e.Message);
            }
        }


        // Client  socket.
        private Socket _WorkSocket = null;
        [XmlIgnore]
        public Socket WorkSocket {
            get { return _WorkSocket; }
        }


        //}
        // Size of receive buffer.
        private const int BufferSize = 1024;
        // Receive buffer.
        [XmlIgnore]
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        [XmlIgnore]
        public StringBuilder sb = new StringBuilder();

        public void ClearArray() {
            buffer = new byte[BufferSize];
            sb = new StringBuilder();

        }

        private Boolean _DoDisconnect = false;

        internal Boolean DoDisconnect {
            get { return _DoDisconnect; }

        }

        internal Boolean Disconnect(Boolean exit) {


            try {
                _DoDisconnect = exit;
                if (WorkSocket != null) {
                    // Clients[i].WorkSocket.Shutdown(SocketShutdown.Both);
                    WorkSocket.Close();
                }
                if (MainSocket != null) {
                    //Clients[i].MainSocket.Shutdown(SocketShutdown.Both);
                    MainSocket.Close();
                }
                OnMessage = null;

                WaitNewClient.Set();
            } catch (Exception exp) {

                log.Error(exp);
            }
            this.State = ServerClientState.Disconnected;
            return true;
        }






        public void Close() {
            _WorkSocket.Close();
            _WorkSocket = null;
            ClearEvents();
        }


        void ProcessCommand(Socket handler, String Command) {
            try {
                Command = Command.Replace("\n", "");
                Command = Command.Replace("\r", "");
                Command = Command.Replace("\b", "");
                String[] Commands = Command.Split('|');
                if (Commands.Count() > 0) {

                    if (OnMessage != null) {
                        OnMessage(this, Commands.Where(a => a != null && a != "").ToArray());
                    }

                }
            } catch (Exception exp) {

                log.Error(exp);
            }
        }

        

        internal void ReadCallback(IAsyncResult ar) {
            try {

               
                    String content = String.Empty;

                    // Retrieve the state object and the handler socket
                    // from the asynchronous state object.
                    TCPServerClient state = (TCPServerClient)ar.AsyncState;
                    Socket handler = state.WorkSocket;

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
                        handler.BeginReceive(state.buffer, 0, BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        // }
                    }
                    else if (bytesRead == 0) {

                        try {
                            Disconnect(false);


                        } catch (Exception exp) {

                            log.Error(exp);
                        }

                    } 
           
            } catch (Exception exp) {
                Disconnect(false);
                log.Warn(exp.Message);
            }
        }

        [XmlAttribute]
        public Boolean ProcessCommands { get; set; }

        private int _Port = 0;
        [XmlAttribute]
        public int Port {
            get { return _Port; }
            set { _Port = value; }
        }

        private String _Name = "";
        [XmlAttribute]
        public String Name {
            get { return _Name; }
            set { 
                _Name = value; 
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

        Socket _MainSocket = null;
        [XmlIgnore]
        public Socket MainSocket {
            get { return _MainSocket; }
            internal set {
                _MainSocket = value;
            }
        }



        public TCPServerClient() {


        }


        public TCPServerClient(int port, string name) {
            Port = port;
            Name = name;

        }


        public void SetClientSocket(Socket workSocket) {
            _WorkSocket = workSocket;
            _WorkSocket.BeginReceive(buffer, 0, BufferSize, 0,
                            new AsyncCallback(ReadCallback), this);

        }



        public void ClearEvents() {
            OnMessage = null;

        }

    }

    public enum ClientStates { ClientConnected, ClientDisConnected }

    public enum ServerClientState { Waiting, Connected, Disconnected };




    public class TCPClientConnection {

        private static KPPLogger log = new KPPLogger(typeof(TCPClientConnection));

        public delegate void ServerMessage(String[] Commands);
        public event ServerMessage OnServerMessage;

        public delegate void ServerImage(String Request, String Inspection, Image<Bgr, Byte> NewImage);
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
                if (!StaticObjects.isLoading) {
                    log.Error("Invalid Host:" + _address);
                }
            }
        }

        string _address = "";
        [XmlAttribute]
        public String Address {
            get {
                return _address;
            }
            set {

                if (_address != value) {
                    _address = value;
                    try {
                        ipAddress = new IPAddress[] { System.Net.IPAddress.Parse(value) };

                    } catch (FormatException exp) {
                        try {
                            if (!StaticObjects.isLoading) {

                                log.Debug("Invalid IP (" + value + ")");
                                log.Debug("Check Host Name (" + value + ")");
                            }
                            Dns.BeginGetHostAddresses(_address, DnsResolveCallback, null);
                            //ipAddress = Dns.GetHostAddresses(value);


                        } catch (Exception exp2) {
                            if (!StaticObjects.isLoading) {
                                log.Error("Invalid HOST (" + value + ")");
                            }
                            //                            log.Error(exp2);
                            ipAddress = null;
                        }
                    }

                }
            }
        }
        [XmlAttribute]
        public int Port { get; set; }

        [XmlAttribute]
        public Boolean AutoStart { get; set; }

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
            AutoStart = false;

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
                    if (ipAddress != null) {

                        Interlocked.Exchange(ref failedConnectionCount, 0);
                        //Start the async connect operation

                        if (tcpClient != null) {
                            if (tcpClient.Client != null) {
                                tcpClient.Client.Close();
                            }
                            tcpClient = null;

                        }


                        tcpClient = new TcpClient();

                        //addresses[0] = System.Net.IPAddress.Parse(Address);
                        State = ConnectionState.Connecting;
                        Thread.Sleep(10);
                        tcpClient.BeginConnect(ipAddress, Port, ConnectCallback, null);

                    }
                    else {
                        log.Debug("Invalid adress : " + Address);
                    }
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
                        Bitmap bmp = new Bitmap(stream);
                        bmp.Save("img.bmp");
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

    public class TCPServerConnection {



        private static KPPLogger log = new KPPLogger(typeof(TCPServerConnection));



        public delegate void ClientMessage(TCPServerClient ServerClient, String[] Args);
        public event ClientMessage OnClientMessage;

        public delegate void ServerClientConnected(TCPServerClient ServerClient);
        public event ServerClientConnected OnServerClientConnected;





        [XmlIgnore]
        ManualResetEvent allDone = new ManualResetEvent(false);

        //[XmlAttribute]
        //public List<int> Ports { get; set; }



        [XmlAttribute]
        public Boolean StartOnLoad { get; set; }





        void DisconnectClients() {

            try {

                for (int i = 0; i < Clients.Count; i++) {
                    if (Clients[i].Disconnect(true))

                        if (Clients[i].ClientThread!=null) {
                            Clients[i].ClientThread.Join(1000); 
                        }
                        Clients.RemoveAt(i);
                }


            } catch (Exception exp) {

                log.Error(exp);
            }

        }






        List<IPEndPoint> localEndPoints = new List<IPEndPoint>();

        private List<TCPServerClient> _Clients = new List<TCPServerClient>();

        public List<TCPServerClient> Clients {
            get { return _Clients; }
            set { _Clients = value; }
        }

        Boolean ServerStopped = false;

        public void threadListen(object objs) {



            TCPServerClient ServerClient = (TCPServerClient)(objs);
            ServerClient.OnClientStateChanged += new TCPServerClient.ClientStateChanged(ServerClient_OnClientStateChanged);

            do {
                try {
                    Socket scon = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (ServerClient.MainSocket == null) {
                        ServerClient.MainSocket = scon;
                    }


                    IPEndPoint sender = new IPEndPoint(IPAddress.Any, ServerClient.Port);
                    EndPoint Remote = (EndPoint)(sender);


                    scon.Bind(sender);
                    scon.Listen(100);

                    ServerClient.State = ServerClientState.Waiting;
                    Socket handler = scon.Accept();


                    // Get the socket that handles the client request.

                    ServerClient.State = ServerClientState.Connected;

                    ServerClient.SetClientSocket(handler);
                    ServerClient.OnMessage += new TCPServerClient.Message(ServerClient_OnMessage);
                    if (OnServerClientConnected != null) {
                        OnServerClientConnected(ServerClient);
                    }
                    //Clients.Add(ServerClient);
                    //  ServerClient.MainSocket.Disconnect(true);
                    ServerClient.MainSocket = null;
                    scon.Dispose();//= null;

                    ServerClient.WaitNewClient.WaitOne();
                    ServerClient.WaitNewClient.Reset();

                } catch (SocketException ex) {

                    if (true) {

                    }

                }

            } while (!ServerClient.DoDisconnect);


        }

        void ServerClient_OnClientStateChanged(TCPServerClient sender, ServerClientState NewState) {
            try {
                // Clients.Remove(sender);
            } catch (Exception exp) {

                log.Error(exp);
            }
        }



        Boolean StopServer = false;



        public void Dispose() {
            DisconnectClients();
        }

        void StartListening() {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            for (int i = 0; i < Clients.Count; i++) {
                localEndPoints.Add(new IPEndPoint(IPAddress.Any, Clients[i].Port));
            }





            // Create a TCP/IP sockets.

            for (int i = 0; i < Clients.Count; i++) {

                Thread thread = new Thread(threadListen);
                thread.IsBackground = true;
                Clients[i].ClientThread=thread;
                thread.Start(Clients[i]);
            }

            //State = ServerState.Started;


        }

        void ServerClient_OnMessage(TCPServerClient ServerClient, string[] Args) {
            if (OnClientMessage != null) {
                OnClientMessage(ServerClient, Args);
            }
        }

        void ClearEvents() {
            OnClientMessage = null;
            OnServerClientConnected = null;
        }

        public void Stop() {
            DisconnectClients();

        }

        //Thread m_clientListenerThread;

        public void Start() {
            StartListening();
        }

        public TCPServerConnection() {

            StartOnLoad = true;
        }
    }

    public enum SerialState { Opened, Closed };



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
                            if (OnMessage!=null) {
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
                        if (readThread==null) {
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
            if (readThread!=null) {
                readThread.Join(1500);
            }

        }

        public void Send(String line) {
            if (_ComPort.IsOpen) {
                _ComPort.WriteLine(line);
            }
        }

        SerialPort _ComPort = new SerialPort();

        private Boolean _Active=false;
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



    public enum IOType { Trigger, Status};
    public enum IOOutMode { Pulse, On,Off};

     
    public class IO {

        private static KPPLogger log = new KPPLogger(typeof(IO));

        internal GPIOMem MemIO;

        public delegate void IOStateChanged(IO sender, Boolean NewState);

        private event IOStateChanged m_OnIOStateChanged;

        public event IOStateChanged OnIOStateChanged {

            add {
                if (m_OnIOStateChanged==null) {
                    m_OnIOStateChanged += value;
                   // Console.WriteLine("Adding event handler for IO: "+this.Name);
                }
                
            }
            remove {
                m_OnIOStateChanged -= value;
            }
        }


        private Boolean _State = false;
        [XmlIgnore]
        public Boolean State {
            get { return _State; }
            internal set {
                if (_State != value) {
                    _State = value;
                    if (PinType == IOType.Trigger) {
                        if (m_OnIOStateChanged != null) {                            
                            m_OnIOStateChanged(this, _State);
                        }

                        Console.WriteLine("IO (" + this.Name + ") changed:" + _State.ToString());
                    }
                }
            }
        }

       

        private Boolean _Active = false;
        [XmlAttribute]
        public Boolean Active {
            get { return _Active; }
            set {
                if (_Active != value) {
                    _Active = value;
                }
            }
        }
       

        private IOType _PinType = IOType.Status;
        [XmlAttribute]
        public IOType PinType {
            get { return _PinType; }
            set {
                if ( _PinType != value) {
                     _PinType = value; 
                }
            }
        }

        private GPIOPins _Pin =  GPIOPins.GPIO_NONE;

        [XmlAttribute]
        public GPIOPins Pin {
            get { return _Pin; }
            set {
                if (_Pin != value) {
                    _Pin = value;
                }
            }
        }

        private String _Name = "New IO";
        [XmlAttribute]
        public String Name {
            get { return _Name; }
            set { _Name = value; }
        }


        public bool Write(Boolean value) {
            try {
                if (PinType == IOType.Status) {
                    if (MemIO != null) {
                        MemIO.Write(value);
                        State = value;
                    }
                }
                return true;
            } catch (Exception exp) {

                return false;
            }
        }

        public bool Read() {
            try {
                if (PinType == IOType.Trigger) {
                    if (MemIO != null) {
                        
                        return MemIO.Read();
                        
                    }
                }
                return false;
            } catch (Exception exp) {

                return false;
            }
        }

        public IO(String name,GPIOPins PIN,IOType pinType) {
            Name = name;
            Pin = PIN;
            PinType = pinType;
        }


        public IO() {
            
            

        }

       

        public override string ToString() {
            return Name;
        }
    }

    public class IOList  {

        private List<IO> _IOs = new List<IO>();

        public List<IO> IOPins {
            get { return _IOs; }
            set { _IOs = value; }
        }


        

        public IOList() {
           
        }

        public override string ToString() {
            return base.ToString();
        }
    }

    

    public static class GPIOObject {

        private static KPPLogger log = new KPPLogger(typeof(GPIOObject));

        public static IOList IOs;
        static Thread IOThread;
        public static int RefreshTime=100;

        public static Boolean Stop = false;

        internal static void MonitorIOS() {

            while (!Stop) {

                foreach (IO item in IOs.IOPins) {
                    if (item.Active) {
                        if (item.PinType== IOType.Trigger) {
                            bool iostate = item.MemIO.Read();                            
                            item.State = iostate;
                        }
                        
                    }
                }
                Thread.Sleep(RefreshTime);
            }
        }

        public static void Init(IOList ios) {
            try {
                IOs = ios;
                if (StaticObjects.isRPI) {
                    foreach (IO item in ios.IOPins) {
                        
                        if (item.Active) {
                            IO sameIO = ios.IOPins.Find(io => (io.Pin == item.Pin && io.MemIO != null));
                            if (sameIO!=null) {
                                item.MemIO = sameIO.MemIO;
                            }
                            else {
                                switch (item.PinType) {
                                    case IOType.Trigger:
                                        item.MemIO = new GPIOMem(item.Pin, GPIODirection.In);
                                        break;
                                    case IOType.Status:

                                        item.MemIO = new GPIOMem(item.Pin, GPIODirection.Out);
                                        item.MemIO.Write(true);
                                        Thread.Sleep(100);
                                        item.MemIO.Write(false);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            

                            Console.WriteLine("Setting IO:" + item.Pin.ToString());
                        }
                    }


                    IOThread = new Thread(MonitorIOS);
                    IOThread.Start();
                }
            } catch (Exception exp) {
                Console.WriteLine("Error setting IOs:"+ exp.Message);
                log.Error(exp);
            }

        }
    }

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
}
