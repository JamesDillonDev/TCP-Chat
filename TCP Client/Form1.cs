﻿using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

//WARNING any changes that are made may cause Errors

namespace TCP_Client
{
    public partial class Form1 : Form
    {
        string nickname;
        TcpClient tcpClient;
        StreamWriter sWriter;
        StreamReader sReader;
        Thread thread;

        public Form1()
        {
            InitializeComponent();
        }

        public void Button_Connect(object sender, EventArgs e)
        {
            Server_Connect();        
        }

        private void Server_Connect()
        {
            TabTerminal.Clear();
            TerminalWindow.Clear();

            string host = HostEntry.Text.Trim();
            int port = int.Parse(PortEntry.Text.Trim());
            nickname = NicknameEntry.Text.Trim();
            nickname = NicknameEntry.Text.Trim();

            nickname = nickname.Replace(" ", "_");
            NicknameEntry.Text = nickname;

            if (nickname == "")
            {
                MessageBox.Show("Invalid nickname try Again", "TCP Client");
                return;
            }
            try 
            {
                tcpClient = new TcpClient(host, port);

                thread = new Thread(Message_Recv);
                thread.Start();

                sWriter = new StreamWriter(tcpClient.GetStream());
                sWriter.WriteLine(nickname);
                sWriter.Flush();
            }
            catch 
            {
                MessageBox.Show("Server Closed");            
            }
        }

        public void Button_Disconnect(object sender, EventArgs e)
        {
            Server_Disconnect();
        }

        private void Server_Disconnect() 
        {
            if (tcpClient != null) 
            {
                if (tcpClient.Connected)
                {
                    try
                    {
                        sReader.Close();
                        sWriter.Close();
                        tcpClient.Close();

                        TerminalWindow.Clear();
                        TabTerminal.Clear();
                        TabTerminal.AppendText($"Online Clients\n{nickname} ");
                        TerminalWindow.AppendText($"Client Disconnected\n>> {nickname} left the Chat");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else 
                {
                    MessageBox.Show("You are not Connected to a Server", "TCP Client", MessageBoxButtons.OK);
                }
            }
        }

        private void Message_Recv()
        {
            while (true)
            {
                try 
                {
                    sReader = new StreamReader(tcpClient.GetStream());
                    string message = sReader.ReadLine();
                    if(message != null)
                    {
                        SetText(message);
                    }
                    
                }
                catch (Exception)
                {
                    if (!tcpClient.Connected) 
                    {
                        return;
                    }
                }
            }
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (this.TerminalWindow.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                
                text = text.Replace("%EOL%", "\n");
                if (text.StartsWith("PASSWORD"))
                {
                    string password = PasswordEntry.Text.Trim();
                    sWriter.WriteLine($"{password}");
                    sWriter.Flush();
                }
                else if (text.StartsWith("MESSAGE"))
                {
                    string message_arg = text.Replace("MESSAGE ", "");
                    MessageBox.Show(message_arg, "TCP Client");
                }
                else if (text.StartsWith("ADD")) 
                {
                    string add_arg = text.Replace("ADD ", "");
                    TabTerminal.AppendText(add_arg);
                }
                else if (text.StartsWith("REMOVE")) 
                {
                    string remov_arg = text.Replace("REMOVE ", "");
                    TabTerminal.Text = TabTerminal.Text.Replace(remov_arg, "");
                }
                else if (text.StartsWith("DISCONNECT")) 
                {
                    Server_Disconnect();               
                }
                else
                {
                    TerminalWindow.AppendText($"{text}");
                }
            }
        }
        
        public void Button_Send(object sender, EventArgs e)
        {
            string message_send = MessageEntry.Text.Trim();
            MessageEntry.Clear();
            if (tcpClient.Connected)
            {
                if (message_send == "") 
                {
                    return;
                }
                else if (message_send.StartsWith("/help"))
                {
                    TerminalWindow.AppendText("\n---------------HELP---------------");
                    TerminalWindow.AppendText("\n/help --> Displays list of Commands");
                    TerminalWindow.AppendText("\n/clear --> Clears terminal Window");
                    sWriter.WriteLine($"/help");
                    sWriter.Flush();
                }
                else if (message_send.EndsWith("/clear"))
                {
                    TerminalWindow.Clear();
                    TerminalWindow.AppendText(">> Chat has been Cleared");
                }
                else
                {
                    sWriter.WriteLine($"{message_send}");
                    sWriter.Flush();
                }
            }
            else 
            {
                MessageBox.Show("Connect to a server to send messages");
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void About_Press(object sender, EventArgs e)
        {
            MessageBox.Show($"Version: 1.0.0\nDeveloper: James Dillon\nGithub: https://github.com/JamesDillonDev/", "TCP Client");
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            if(tcpClient != null)
            {
                sReader.Close();
                sWriter.Close();
                tcpClient.Close();
            }
        }

        private void TerminalWindow_TextChanged(object sender, EventArgs e)
        {

        }
    }
}