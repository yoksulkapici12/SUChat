using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace chat_client
{
    public partial class Form1 : Form
    {
        private Socket clientSocket;
        private byte[] buffer = new byte[4096];
        private bool connected = false;
        private bool terminating = false;


        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            // Initially, arrange the all buttons
            SPS101.Enabled = false;
            IF100.Enabled = false;
            Leave_it.Enabled = false;
            send.Enabled = false;
            ip.Enabled = true;
            port.Enabled = true;
            user_name.Enabled = true;
            disconnect.Enabled = false;
        }

        private void connect_Click(object sender, EventArgs e)
        {
            string serverIP = ip.Text;
            string Port = port.Text;
            int serverPort;

            string userName = user_name.Text;
            if(userName != "")
            {
                if (Int32.TryParse(Port, out serverPort))// get the port number
                {
                    if(serverIP != "")
                    {
                         try
                         {
                            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            clientSocket.Connect(serverIP, serverPort);

                            string str_port = serverPort.ToString();

                            // sending username and other stuff to server. Server side should get userName and not need to send IP ort port
                            string clientInfo = $"{serverIP}|{str_port}|{userName}";
                            byte[] clientInfoBytes = Encoding.ASCII.GetBytes(clientInfo);
                            clientSocket.Send(clientInfoBytes);

                            // Get the message from the server
                            int bytesRead = clientSocket.Receive(buffer);
                            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                            // Process the message coming from the server
                            screen.AppendText(response);

                            // Enable buttons if the success message is received
                            if (response.Contains("Connected!"))
                            {
                                connected = true;
                                // Use Invoke to update UI controls from a different thread
                                Invoke(new Action(() =>
                                {
                                    ip.Enabled = false;
                                    port.Enabled = false;
                                    user_name.Enabled = false;
                                    SPS101.Enabled = true;
                                    IF100.Enabled = true;
                                    connect.Enabled = false;
                                    disconnect.Enabled = true;

                                }));
                                // getting message flow
                                Thread receiveThread = new Thread(Receive);
                                receiveThread.Start();

                            }
                            
                           
                         }
                         catch (Exception ex)
                         {
                            MessageBox.Show($"Error connecting to server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                    }
                    else
                    {
                        screen.AppendText("Server IP Address cannot be empty\n");
                    }
                }
                else
                {
                    screen.AppendText("Check your Port\n");
                }
            }
            else
            {
                screen.AppendText("User name cannot be empty\n");
            }

           

        }

        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    int bytesRead = clientSocket.Receive(buffer);

                    // Convert the received bytes directly to string
                    string incomingMessage = Encoding.Default.GetString(buffer, 0, bytesRead);

                    // Use Invoke to update UI controls from a different thread
                    Invoke(new Action(() =>
                    {
                        if (incomingMessage == "!!! Hello Server is down!!!")// if the server is stoped close and make the GUI equals to initial situation
                        {
                            // Take appropriate action, e.g., disable UI controls or show a message
                            screen.AppendText("Server is shutting down. Disconnecting...\n");
                            connected = false;

                            SPS101.Enabled = false;
                            IF100.Enabled = false;
                            Leave_it.Enabled = false;
                            send.Enabled = false;
                            ip.Enabled = true;
                            port.Enabled = true;
                            user_name.Enabled = true;
                            connect.Enabled = true;
                            disconnect.Enabled = false;
                            clientSocket.Close();

                        }
                        // getting know who are in the room 
                        else if (incomingMessage.Contains("server_sendingparticipantinfo") )
                        {
                            string[] messagePart = incomingMessage.Split('|');
                            if (messagePart.Length >= 2 && messagePart[0] == "server_sendingparticipantinfo")
                            {
                                participants.Clear();
                                // Extract participant information from the message (excluding the first element)
                                string[] participantss = new string[messagePart.Length - 1];
                                Array.Copy(messagePart, 1, participantss, 0, participantss.Length);

                                // Now 'participants' array contains the participant information
                                foreach (string participant in participantss)
                                {
                                    if (participants.InvokeRequired)
                                    {
                                        participants.Invoke(new Action(() =>
                                        {
                                            participants.AppendText($"Participant: {participant}\n");
                                        }));
                                    }
                                    else
                                    {
                                       
                                        participants.AppendText($"Participant: {participant}\n");

                                    }
                                    
                                }
                            }
                            
                        }
                        else
                        {

                            screen.AppendText(incomingMessage + "\n");
                        }
                    }));
                }
                catch
                {
                    if (!terminating)
                    {

                        connect.Enabled = true;

                        send.Enabled = false;
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }


        // Sending room info based on the client choice
        private void SPS101_Click(object sender, EventArgs e)
        {
            screen.Clear();
            SendRoomInfo("SPS101 joined...");
            IF100.Enabled = false;
            SPS101.Enabled = false;
            Leave_it.Enabled = true;
            send.Enabled = true;

        }

        private void IF100_Click(object sender, EventArgs e)
        {
            screen.Clear();
            SendRoomInfo("IF100 joined...");
            SPS101.Enabled = false;
            IF100.Enabled = false;
            Leave_it.Enabled = true;
            send.Enabled = true; ;
        }

        private void SendRoomInfo(string roomInfo)
        {
            try
            {
                byte[] roomInfoBytes = Encoding.ASCII.GetBytes(roomInfo);
                clientSocket.Send(roomInfoBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending room info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // send messages to server with the username 
        private void send_Click(object sender, EventArgs e)
        {
            try
            {
                // Include the username along with the message
                string message = $"{user_name.Text}:{msg.Text}";
                byte[] messages = Encoding.ASCII.GetBytes(message);
                clientSocket.Send(messages);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending the message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Leave the client from the room which the client was in 
        private void Leave_it_Click(object sender, EventArgs e)
        {
            SendLeaveMessage();
            SPS101.Enabled = true;
            IF100.Enabled = true;
            Leave_it.Enabled = false;
            send.Enabled = false;
            participants.Clear();
            screen.Clear();
        }


        // Close the GUI and inform the server 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected)
            {
                string leaveMessage = $"{user_name.Text}:// closed the app";
                byte[] leaveMessageBytes = Encoding.ASCII.GetBytes(leaveMessage);
                clientSocket.Send(leaveMessageBytes);
                clientSocket.Close();

            }
            
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }
        // informing the server about the leaving room
        private void SendLeaveMessage()
        {
            try
            {
                string leaveMessage = $"{user_name.Text}:// want to leave";
                byte[] leaveMessageBytes = Encoding.ASCII.GetBytes(leaveMessage);
                clientSocket.Send(leaveMessageBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending leave message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                string leaveMessage = $"{user_name.Text}:// closed the app";
                byte[] leaveMessageBytes = Encoding.ASCII.GetBytes(leaveMessage);
                clientSocket.Send(leaveMessageBytes);
                clientSocket.Close();

            }

            connected = false;
            terminating = true;
            screen.Clear();
            participants.Clear();
            SPS101.Enabled = false;
            IF100.Enabled = false;
            Leave_it.Enabled = false;
            send.Enabled = false;
            ip.Enabled = true;
            port.Enabled = true;
            user_name.Enabled = true;
            disconnect.Enabled = false;
            connect.Enabled = true;

        }
    }
}
