using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
// enes ozkan sen 
namespace server_rn
{
    public partial class Form1 : Form
    {
        private Socket serverSocket;
        private Dictionary<string, ClientInfo> clientSocketsDictionary = new Dictionary<string, ClientInfo>();
        private byte[] buffer = new byte[4096];
        // Record all user name and divide them based on their choice of the room
        private HashSet<string> user_SPS101 = new HashSet<string>();
        private HashSet<string> user_IF100 = new HashSet<string>();

        private volatile bool acceptingClients = true;

        public Form1()
        {
            InitializeComponent();
            // Initially, the Stop button is disabled
            stop.Enabled = false;

            
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void StartServer()
        {
           
            int portNum;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if(Int32.TryParse(port.Text, out portNum))
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, portNum)); 
                serverSocket.Listen(10);
                MessageBox.Show("Server started!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Start accepting client connections in a separate thread
                Thread acceptThread = new Thread(AcceptClients);
                acceptThread.Start(); // Disable the Start button when the server is started
                start.Enabled = false;
                // Enable the Stop button
                stop.Enabled = true;
                port.Enabled = false;
                screen.AppendText($"Server Listening on port:{port.Text}\n"); // Showing the port number
            }
            else
            {
                screen.AppendText("Check your port\n");
            }
            

            
        }

        private void AcceptClients()
        {
            while (acceptingClients)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept();

                    // Create a copy of clientSocket for the thread
                    Socket clientCopy = clientSocket;

                    // Handle client communication in a separate thread with the copy
                    Thread clientThread = new Thread(() => HandleClient(clientCopy));
                    clientThread.Start();
                }
                catch (SocketException ex)
                {
                    // Check if the operation was interrupted by WSACancelBlockingCall
                    if (ex.ErrorCode == 10004)
                    {
                        // This exception can be ignored since it's expected when stopping the server
                        break;
                    }
                    else
                    {
                        // Handle other socket exceptions
                        MessageBox.Show($"SocketException during Accept: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void HandleClient(Socket clientSocket)
        {
            
            try
            {
                int bytesRead = clientSocket.Receive(buffer);
                string clientInfo = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Validate the client's input as a first message and then send extract message 
                if (IsValidClientInput(clientInfo))
                {
                    // Extract username from clientInfo
                    string[] infoParts = clientInfo.Split('|');
                    string username = infoParts[2];
                    if (clientSocketsDictionary.ContainsKey(username))
                    {
                        // Send an error message back to the client
                        string errorMessage = "Username is already taken. Connection rejected.\n";
                        byte[] errorMessageBytes = Encoding.ASCII.GetBytes(errorMessage);
                        clientSocket.Send(errorMessageBytes);
                        return; // Exit the method without adding to the connectedUsernames
                    }

                    // Add the username and socket to the dictionary
                    clientSocketsDictionary.Add(username, new ClientInfo { Socket = clientSocket, Username = username });

                    // Send a success message back to the client with the username
                    string successMessage = $"Connected! Hey, {username}! Are you ready to Join SPS101 or IF100?\n";
                    byte[] successMessageBytes = Encoding.ASCII.GetBytes(successMessage);
                    clientSocket.Send(successMessageBytes);

                    

                    try
                    {
                        while (true)
                        {
                            int byt = clientSocket.Receive(buffer);
                            string message = Encoding.ASCII.GetString(buffer, 0, byt);
                            if(message.Contains("// want to leave"))// if the client wants to leave the room 
                            {
                                string[] messageParts = message.Split(':');
                                string name = messageParts[0];
                                HandleLeaveAction(name);

                                
                            }
                            else if(message.Contains("SPS101"))// client joining the room
                            {
                                DisplayClickedButton(username, message);
                                BroadcastMessageInRoom(username, message, user_SPS101);
                                BroadcastJoinMessage( user_SPS101);

                            }
                            else if (message.Contains("IF100"))// client joining the room
                            {
                                DisplayClickedButton(username, message);
                                BroadcastMessageInRoom(username, message, user_IF100);
                                BroadcastJoinMessage(user_IF100);
                            }
                            else if(message.Contains("// closed the app"))// client closed the app
                            {
                                string[] messageParts = message.Split(':');
                                string name = messageParts[0];
                                HandleLeaveAction(name);
                                ClientInfo my_client = clientSocketsDictionary[name];
                                my_client.Socket.Close();

                                // Remove the user from the dictionary
                                clientSocketsDictionary.Remove(name);

                            }
                            else// it is conversational message
                            {
                                // Parse the username and message
                                string[] messageParts = message.Split(':');
                                string name = messageParts[0];
                                string userMessage = messageParts[1];

                                // Process the message as needed
                                DisplayMessage(username, userMessage);
                            }

                            

                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions or client disconnection
                        Console.WriteLine($"Error handling client messages: {ex.Message}");
                    }
                }
             
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error handling client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void DisplayMessage(string senderUsername, string message)
        {
            
            if (screen.InvokeRequired)
            {
                screen.Invoke(new Action(() =>
                {
                    byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                    if (user_SPS101.Contains(senderUsername))
                    {
                        SPS101.AppendText($"{senderUsername}: {message}\n");
                        BroadcastMessageInRoom(senderUsername, message, user_SPS101);
                    }
                    else
                    {
                        IF100.AppendText($"{senderUsername}: {message}\n");
                        BroadcastMessageInRoom(senderUsername, message, user_IF100);

                    }
                }));
            }
            else
            {
                if (user_SPS101.Contains(senderUsername))
                {
                    SPS101.AppendText($"{senderUsername}: {message}\n");
                    BroadcastMessageInRoom(senderUsername, message, user_SPS101);
                }
                else
                {
                    IF100.AppendText($"{senderUsername}: {message}\n");
                    BroadcastMessageInRoom(senderUsername, message, user_IF100);
                }
            }
        }
        private string participant_Info_Sender(HashSet<string> roomUsers)// send all participant info which share the same room with the client to client 
        {
            string dummy = "server_sendingparticipantinfo";
            foreach(string recipient in roomUsers)
            {
                dummy += $"|{recipient}";
            }
            return dummy;
           
        }
        private void BroadcastMessageInRoom(string senderUsername, string message, HashSet<string> roomUsers)
        {
            foreach (string recipientUsername in roomUsers)
            {
                if (clientSocketsDictionary.TryGetValue(recipientUsername, out ClientInfo recipientClient) && recipientClient.Socket != null)
                {
                    try
                    {
                        // Sending the message to each recipient in the room
                        Byte[] buffer = Encoding.Default.GetBytes($"{senderUsername}: {message}\n");
                        recipientClient.Socket.Send(buffer);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, log, or notify the user
                        Console.WriteLine($"Error sending message to {recipientUsername}: {ex.Message}");
                    }
                }
            }
        }
        private bool IsUserInRoom(string username, HashSet<string> room)
        {
            return room.Contains(username);
        }

        private void HandleLeaveAction(string username)
        {
            if (screen.InvokeRequired)
            {
                screen.Invoke(new Action(() =>
                {
                    screen.AppendText($"{username} Leaved...\n");
                }));
                
            }
            else
            {
                screen.AppendText($"{username} Leaved...\n");
            }
            
            // Remove the user from the room
            if (IsUserInRoom(username, user_IF100))
            {
                user_IF100.Remove(username);
                BroadcastLeaveMessage(username, user_IF100);
                
                if (IF100.InvokeRequired)
                {
                    IF100.Invoke(new Action(() =>
                    {
                        IF100.AppendText($"{username} Leaved...\n");
                    }));
                }
                else
                {
                    IF100.AppendText($"{username} Leaved...\n");
                }
                

            }
            else
            {
                user_SPS101.Remove(username);
                BroadcastLeaveMessage(username, user_SPS101);
                
                if (SPS101.InvokeRequired)
                {
                    SPS101.Invoke(new Action(() =>
                    {
                        SPS101.AppendText($"{username} Leaved...\n");
                    }));
                }
                else
                {
                    SPS101.AppendText($"{username} Leaved...\n");
                }

            }

        }

        private void BroadcastJoinMessage( HashSet<string> users)// informing the every client sharing the same room
        {
            string list = participant_Info_Sender(users);
            Byte[] my_buf = Encoding.Default.GetBytes(list);
            foreach (string recipientUsername in users)
            {
                if (clientSocketsDictionary.TryGetValue(recipientUsername, out ClientInfo recipientClient) && recipientClient.Socket != null)
                {
                    try
                    {
                        
                        recipientClient.Socket.Send(my_buf);

                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, log, or notify the user
                        Console.WriteLine($"Error sending leave message to {recipientUsername}: {ex.Message}");
                    }
                }
            }
        }
        private void BroadcastLeaveMessage(string username, HashSet<string> roomUsers)
        {               
            string list = participant_Info_Sender(roomUsers);
            Byte[] my_buf = Encoding.Default.GetBytes(list);
            foreach (string recipientUsername in roomUsers)
            {
                if (clientSocketsDictionary.TryGetValue(recipientUsername, out ClientInfo recipientClient) && recipientClient.Socket != null)
                {
                    try
                    {
                        // Sending the leave message to each recipient in the room
                        Byte[] buffer = Encoding.Default.GetBytes($"{username}: leaved\n");
                        recipientClient.Socket.Send(buffer);
                        //update the new information about participant of the room                        
                        recipientClient.Socket.Send(my_buf);

                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, log, or notify the user
                        Console.WriteLine($"Error sending leave message to {recipientUsername}: {ex.Message}");
                    }
                }
            }
        }

        private bool IsValidClientInput(string clientInfo)
        {
            
            string[] infoParts = clientInfo.Split('|');
            return infoParts.Length == 3  ;
        }

        private void start_Click(object sender, EventArgs e)
        {
            StartServer();
            
        }

        private void StopServer()
        {
            
            Invoke(new Action(() =>
            {
                screen.Clear();
                IF100.Clear();
                SPS101.Clear();
                // Enable the Start button when the server is stopped
                start.Enabled = true;
                // Disable the Stop button
                stop.Enabled = false;
                port.Enabled = true;
            }));

            

             Byte[] buffer = Encoding.Default.GetBytes("!!! Hello Server is down!!!");
            // Inform each client 
            foreach (ClientInfo clientInfo in clientSocketsDictionary.Values)
            {
                try
                {
                    if (clientInfo.Socket != null && clientInfo.Socket.Connected)
                    {
                        clientInfo.Socket.Send(buffer);
                        clientInfo.Socket.Shutdown(SocketShutdown.Both);
                        clientInfo.Socket.Close();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log, or notify the user
                    Console.WriteLine($"Error sending to client or closing client socket: {ex.Message}");
                }
            }            
            // Close all connected client sockets and inform each client

            foreach (ClientInfo clientInfo in clientSocketsDictionary.Values)
            {

                try
                {
                    
                    clientInfo.Socket?.Shutdown(SocketShutdown.Both);
                    clientInfo.Socket?.Close();
                    
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log, or notify the user
                    Console.WriteLine($"Error closing client socket: {ex.Message}");
                }
            }
           

            // Stop the server socket
            serverSocket?.Close();
            acceptingClients = false;

            // Clear the dictionary of client sockets
            clientSocketsDictionary.Clear();
        }

        private void stop_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to stop the server?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopServer();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit from the ChatSU SERVER?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopServer();
                
            }
            else
            {
                // If the user selected "No", cancel the form closing
                e.Cancel = true;
            }
        }

        // Display the clicked button in the rich text box
        private void DisplayClickedButton(string username, string button)
        {
            if (screen.InvokeRequired)
            {
                screen.Invoke(new Action(() =>
                {
                    screen.AppendText($"{username} joined: {button}\n");
                    if (button.Contains("SPS"))
                    {
                        user_SPS101.Add(username);
                        SPS101.AppendText($"{username} joined: {button}\n");

                    }
                    else
                    {
                        user_IF100.Add(username);
                        IF100.AppendText($"{username} joined: {button}\n");

                    }
                }));
            }
            else
            {
                screen.AppendText($"{username} joined: {button}\n");
                if (button.Contains("SPS"))
                {
                    user_SPS101.Add(username);
                    SPS101.AppendText($"{username} joined: {button}\n");
                }
                else
                {
                    user_IF100.Add(username);
                    IF100.AppendText($"{username} joined: {button}\n");

                }
            }
        }
    }

    //  to hold client information
    public class ClientInfo
    {
        public Socket Socket { get; set; }
        public string Username { get; set; }
    }

}

