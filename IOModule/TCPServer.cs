using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KPP.Core.Debug;
using System.IO;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Net;
using System.Threading;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace IOModule {

    /// <summary>
    /// Implements a custom EventArgs class for passing connection state information.
    /// </summary>
    public class TCPServerEventArgs : EventArgs {
        protected ConnectionState connectionState;
        public ConnectionState ConnectionState {
            get { return connectionState; }
        }


        public TCPServerEventArgs(ConnectionState cs) {
            connectionState = cs;
            
        }
    }

    /// <summary>
    /// Implements a custom EventArgs class for passing connection state information.
    /// </summary>
    public class TCPServerClientEventArgs : EventArgs {
        protected ConnectionState connectionState;
        public ConnectionState ConnectionState {
            get { return connectionState; }
        }

        protected String m_MessageReceived;
        public String MessageReceived{
            get { return m_MessageReceived; }
        }

        public TCPServerClientEventArgs(String messageReceived, ConnectionState cs) {
            connectionState = cs;
            m_MessageReceived = messageReceived;
        }
    }


    /// <summary>
    /// Implements a custom EventArgs class for passing an application exception back
    /// to the application for processing.
    /// </summary>
    public class TCPServerApplicationExceptionEventArgs : EventArgs {
        protected Exception e;

        public Exception Exception {
            get { return e; }
        }

        public TCPServerApplicationExceptionEventArgs(Exception e) {
            this.e = e;
        }
    }

    [Serializable]
    public class TcpServerException : Exception {
        public TcpServerException()
            : base() { }

        public TcpServerException(string message)
            : base(message) { }

        public TcpServerException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public TcpServerException(string message, Exception innerException)
            : base(message, innerException) { }

        public TcpServerException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected TcpServerException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }


    /// <span class="code-SummaryComment"><summary></span>
    /// Buffers the socket connection and TcpServer instance.
    /// <span class="code-SummaryComment"></summary></span>
    public class ConnectionState {

        private static KPPLogger log = new KPPLogger(typeof(ConnectionState));

        protected Socket connection;
        protected TCPServer server;

        /// <span class="code-SummaryComment"><summary></span>
        /// Gets the TcpServer instance. Throws an exception if the connection
        /// has been closed.
        /// <span class="code-SummaryComment"></summary></span>
        public TCPServer Server {
            get {
                if (server == null) {
                    throw new TcpServerException("Connection is closed.");
                }

                return server;
            }
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Gets the socket connection. Throws an exception if the connection
        /// has been closed.
        /// <span class="code-SummaryComment"></summary></span>
        private Socket Connection {
            get {
                if (server == null) {
                    throw new TcpServerException("Connection is closed.");
                }

                return connection;
            }
        }

        public void Write(String Message) {
            try {

                try {
                    lock (this) {

                        if (connection != null) {
                            connection.Send(Encoding.ASCII.GetBytes(Message.Replace("\n", "") + "\n"));
                        } 
                    }
                } catch (SocketException exp) {
                    log.Debug(exp.Message);
                }
            } catch (Exception exp) {

                log.Error(this, exp);
            }
        }

        public const int BufferSize = 1024*2;
        // Receive buffer.

        protected StringBuilder sb = new StringBuilder();

        protected byte[] buffer = new byte[BufferSize];

        /// <span class="code-SummaryComment"><summary></span>
        /// Constructor.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="connection">The socket connection.</param></span>
        /// <span class="code-SummaryComment"><param name="server">The TcpServer instance.</param></span>
        public ConnectionState(Socket connection, TCPServer server) {
            this.connection = connection;
            this.connection.BeginReceive(buffer, 0, BufferSize, 0,
                            new AsyncCallback(ReadCallback), this);
            this.server = server;

        }



        private void ReadCallback(IAsyncResult ar) {
            
            ConnectionState state = (ConnectionState)ar.AsyncState;

            try {
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                
                Socket handler = state.Connection;

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0) {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();

                    if (content[content.Length-1]=='\n') {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",content.Length, content);
                        // Echo the data back to the client.

                        //ProcessCommand(handler, content);
                        content=content.Replace("\r", "");
                        String[] StrArray = content.Split(new String[] { "\n" }, StringSplitOptions.None);

                        foreach (String item in StrArray) {
                            server.OnServerClientMessage(new TCPServerClientEventArgs(item, this));
                        }

                        //server.OnServerClientMessage(new TCPServerClientEventArgs(content,this));

                        buffer = new byte[BufferSize];
                        sb = new StringBuilder();
                    }
                    // else {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, ConnectionState.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                    // }
                }
                else if (bytesRead == 0) {

                    try {
                        state.Close();


                    } catch (Exception exp) {
                        log.Info(exp);
                
                    }

                }

            } catch (Exception exp) {
                if (exp is SocketException) {
                    if (state!=null) {
                        state.Close();
                    }
                } else {
                    log.Error(this,exp);
                }
                
            }

        }
        /// <span class="code-SummaryComment"><summary></span>
        /// This is the prefered manner for closing a socket connection, as it
        /// nulls the internal fields so that subsequently referencing a closed
        /// connection throws an exception. This method also throws an exception 
        /// if the connection has already been shut down.
        /// <span class="code-SummaryComment"></summary></span>
        public void Close() {
            if (server == null) {
                throw new TcpServerException("Connection already is closed.");
            }
            lock (this) {
                server.OnServerClientDisconnected(new TCPServerEventArgs(this));

                connection.Shutdown(SocketShutdown.Both);
                connection.Close();
                connection = null;
                server = null; 
            }
        }
    }




    public enum ServerState { Started, Stopped};
  
    public class TCPServer {



        private static KPPLogger log = new KPPLogger(typeof(TCPServer));


         private String m_ID;
        [XmlAttribute]
        public String ID {
            get {
                return m_ID;
            }
            set {
                m_ID = value;
            }
        }

        private int m_Port = -1;
        [XmlAttribute]
        public int Port {
            get { return m_Port; }
            set {
                if (m_Port!=value) {
                    m_Port = value;
                    endPoint = new IPEndPoint(IPAddress.Any, value);
                }
            }
        }

        public delegate void TcpServerClientMessageEvent(object sender,
                                              TCPServerClientEventArgs e);

        public delegate void TcpServerEventDlgt(object sender,
                                              TCPServerEventArgs e);

        public delegate void ApplicationExceptionDlgt(object sender,
              TCPServerApplicationExceptionEventArgs e);

        /// <span class="code-SummaryComment"><summary></span>
        /// Event fires when a connection is accepted. Being multicast, this 
        /// allows you to attach not only your application's event handler, but
        /// also other handlers, such as diagnostics/monitoring, to the event.
        /// <span class="code-SummaryComment"></summary></span>
        
        public event TcpServerEventDlgt Connected;
        
        public event TcpServerEventDlgt Disconnected;
        
        public event TcpServerClientMessageEvent ServerClientMessage;

        /// <span class="code-SummaryComment"><summary></span>
        /// This event fires when *your* application throws an exception 
        /// that *you* do not handle in the 
        /// interaction with the client. You can hook this event to log 
        /// unhandled exceptions, more as a 
        /// tool to aid development rather than a suggested approach 
        /// for handling your application errors.
        /// <span class="code-SummaryComment"></summary></span>
        
        public event ApplicationExceptionDlgt HandleApplicationException;
        [XmlIgnore, Browsable(false)]
        protected IPEndPoint endPoint;
        [XmlIgnore]
        internal Socket listener;
        [XmlIgnore]
        protected int pendingConnectionQueueSize;

        /// <span class="code-SummaryComment"><summary></span>
        /// Gets/sets pendingConnectionQueueSize. The default is 100.
        /// <span class="code-SummaryComment"></summary></span>
        /// 
        [XmlIgnore, Browsable(false)]
        public int PendingConnectionQueueSize {
            get { return pendingConnectionQueueSize; }
            set {
                if (listener != null) {
                    throw new TcpServerException("Listener has already started. Changing the pending queue size is not allowed.");
                }

                pendingConnectionQueueSize = value;
            }
        }


        ConnectionState m_Client;
        [XmlIgnore, Browsable(false)]
        public ConnectionState Client {
            get { return m_Client; }
            
        }

        ServerState m_State= ServerState.Stopped;
        [XmlIgnore]
        public ServerState State {
            get { return m_State; }
            
        }
        /// <span class="code-SummaryComment"><summary></span>
        /// Gets listener socket.
        /// <span class="code-SummaryComment"></summary></span>
        /// 
        [XmlIgnore]
        internal Socket Listener {
            get { return listener; }
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Gets/sets endPoint
        /// <span class="code-SummaryComment"></summary></span>
        /// 
        [XmlIgnore,Browsable(false)]
        public IPEndPoint EndPoint {
            get { return endPoint; }
            private set {
                if (listener != null) {
                    throw new TcpServerException("Listener has already started. Changing the endpoint is not allowed.");
                }

                endPoint = value;
            }
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Default constructor.
        /// <span class="code-SummaryComment"></summary></span>
        public TCPServer() {
            pendingConnectionQueueSize = 1;
            ID = "No ID";
            m_Port = -1;
        }

        private Boolean m_Enabled = true;
        [XmlAttribute]
        public Boolean Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }


        /// <span class="code-SummaryComment"><summary></span>
        /// Initializes the server with a port, the endpoint is initialized
        /// with IPAddress.Any.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="port"></param></span>
        public TCPServer(String clientID, int port) {
            //endPoint = new IPEndPoint(IPAddress.Any, port);
            Port = port;
            ID = clientID;
            pendingConnectionQueueSize = 1;
        }

       

        /// <span class="code-SummaryComment"><summary></span>
        /// Begins listening for incoming connections.
        /// This method returns immediately.
        /// Incoming connections are reported using the Connected event.
        /// <span class="code-SummaryComment"></summary></span>
        public void StartListening() {
            try
            {
                if (!Enabled)
                {
                    log.Info("Server ("+this.ID+") not enabled");
                    return;
                }
                if (endPoint == null)
                {
                    throw new TcpServerException("EndPoint not initialized.");
                }

                if (listener != null)
                {
                    return;
                    //throw new TcpServerException("Already listening.");
                }

                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                      ProtocolType.Tcp);
                listener.Bind(endPoint);
                listener.Listen(10);
                listener.BeginAccept(AcceptConnection, this);
                m_State = ServerState.Started;
            }
            catch (Exception exp)
            {

                log.Error(exp);
                    
            }
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Shuts down the listener.
        /// <span class="code-SummaryComment"></summary></span>
        public void StopListening() {
            // Make sure we're not accepting a connection.
            lock (this) {
                if (listener!=null)
                {
                    listener.Close();
                    listener = null; 
                }
                if (Client!=null) {
                    Client.Close();
                }
            }
            m_State = ServerState.Stopped;
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Accepts the connection and invokes any Connected event handlers.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="res"></param></span>
        protected void AcceptConnection(IAsyncResult res) {
            Socket connection;

            TCPServer server = res.AsyncState as TCPServer;

            // Make sure listener doesn't go null on us.
            lock (this) {
                if (listener == null) {
                    return;
                }

                connection = listener.EndAccept(res);

                if (server.Client == null) {

                    
                    ConnectionState cs = new ConnectionState(connection, this);
                    m_Client = cs;
                    this.OnConnected(new TCPServerEventArgs(cs));
                }
                else {
                
                    connection.Send(Encoding.ASCII.GetBytes("Only one client can connect to server port\n"));
                    connection.Close();
                }


                listener.BeginAccept(AcceptConnection, server);
            }


            // Close the connection if there are no handlers to accept it!




        }

        internal virtual void OnServerClientDisconnected(TCPServerEventArgs e) {

            if (Disconnected != null) {
                try {                    
                    Disconnected(this,e);
                } catch (Exception exp) {

                    log.Error(exp);
                }
            }

            m_Client = null;

        }


        internal virtual void OnServerClientMessage(TCPServerClientEventArgs e) {

            if (ServerClientMessage!=null) {
                try {
                    ServerClientMessage(this,e);
                } catch (Exception exp) {

                    log.Error(exp);
                }
            }
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Fire the Connected event if it exists.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="e"></param></span>
        protected virtual void OnConnected(TCPServerEventArgs e) {
            if (Connected != null) {
                try {
                    Connected(this, e);
                } catch (Exception ex) {
                    // Close the connection if the application threw an exception that
                    // is caught here by the server.
                    e.ConnectionState.Close();
                    TCPServerApplicationExceptionEventArgs appErr =
                         new TCPServerApplicationExceptionEventArgs(ex);

                    try {
                        OnHandleApplicationException(appErr);
                    } catch (Exception ex2) {
                        // Oh great, the exception handler threw an exception!
                        System.Diagnostics.Trace.WriteLine(ex2.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Invokes the HandleApplicationException handler, if exists.
        /// </summary>
        /// <param name="e">The exception event args instance.</param>
        protected virtual void OnHandleApplicationException(TCPServerApplicationExceptionEventArgs e) {
            if (HandleApplicationException != null) {
                HandleApplicationException(this, e);
            }
        }

       

    }
}
