using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Lab03.Server
{
    public partial class Server : Form
    {
        TcpListener server;
        List<TcpClient> clients = new List<TcpClient>();
        List<string> connectedClients = new List<string>();
        Dictionary<int, TcpClient> connectedClientsDict = new Dictionary<int, TcpClient>(); // Lưu trữ danh sách các cổng kết nối và TcpClient tương ứng
        Dictionary<TcpClient, string> clientNames = new Dictionary<TcpClient, string>(); // Lưu thông tin tên người dùng

        public Server()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(700, 0);
        }

        private void Server_Load(object sender, EventArgs e)
        {
            server = new TcpListener(IPAddress.Any, 9494);
            server.Start();
            richTextBox1.AppendText("Server started.\n");

            Thread listenThread = new Thread(Listen);
            listenThread.Start();
        }

        private void Listen()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                int clientPort = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                connectedClientsDict.Add(clientPort, client); // Thêm cổng kết nối và TcpClient tương ứng vào connectedClientsDict
                connectedClients.Add(client.Client.RemoteEndPoint.ToString());
                UpdateConnectedClients();

                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            string clientName = "";

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(message + "\n")));
                if (message.StartsWith("[file]"))
                {
                    // Tin nhắn chứa tập tin được gửi từ client
                    string fileName = message.Substring(6);
                    byte[] fileData = new byte[1024];
                    int totalBytesRead = 0;
                    int bytesToRead;

                    // Đọc dữ liệu tập tin từ client
                    while ((bytesToRead = stream.Read(fileData, 0, fileData.Length)) > 0)
                    {
                        // Ghi dữ liệu vào tập tin trên server
                        using (FileStream fileStream = new FileStream(fileName, FileMode.Append))
                        {
                            fileStream.Write(fileData, 0, bytesToRead);
                        }

                        totalBytesRead += bytesToRead;
                        if (totalBytesRead >= bytesRead)
                        {
                            break;
                        }
                    }

                    // Gửi tập tin đến các client khác
                    foreach (TcpClient otherClient in clients)
                    {
                        if (otherClient != client)
                        {
                            NetworkStream otherStream = otherClient.GetStream();

                            // Gửi tin nhắn chứa tên tập tin đến các client khác
                            byte[] response = Encoding.Unicode.GetBytes(message);
                            otherStream.Write(response, 0, response.Length);

                            // Gửi dữ liệu tập tin đến các client khác
                            byte[] fileBuffer = File.ReadAllBytes(fileName);
                            otherStream.Write(fileBuffer, 0, fileBuffer.Length);
                        }
                    }
                }
                else
                {
                    string[] test = message.Split(' ');  // Tin nhắn được gửi từ client có format "{[ip]}{Name}: {@recipientPort} {message} " nếu là chat riêng
                    string check = test[1]; // Ta cắt các khoảng trắng trong tin nhắn từ client và lấy phần thứ hai là {@recipientPort} 
                    if (check.StartsWith("@")) // Kiểm tra xem tin nhắn có phải là riêng tư hay không
                    {
                        string[] parts = message.Split(' '); // Cắt tin nhắn được gửi từ client 
                        string senderName = parts[0]; // Lấy ip và tên người gửi từ phần tử đầu tiên của mảng parts {[ip]}{Name}
                        int recipientPort = int.Parse(parts[1].Substring(1)); // Lấy cổng (port) người nhận {@recipientPort}
                        string privateMessage = string.Join(" ", parts.Skip(2)); // Lấy nội dung tin nhắn riêng tư {message}

                        TcpClient recipientClient;
                        if (connectedClientsDict.TryGetValue(recipientPort, out recipientClient))
                        {
                            string senderPort = ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString();
                            string privateLog = $"private from {senderPort} to {recipientPort}: {privateMessage}";
                            // Ghi thông điệp từ client lên server vào richTextBox
                            richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(privateLog + "\n")));

                            // Gửi tin nhắn riêng đến người nhận
                            NetworkStream recipientStream = recipientClient.GetStream();
                            privateMessage = senderName + " " + privateMessage;
                            byte[] privateResponse = Encoding.Unicode.GetBytes(privateMessage);
                            recipientStream.Write(privateResponse, 0, privateResponse.Length);
                        }
                    }
                    else
                    {
                        if (clientName == "") // Nếu tên người dùng chưa được đặt, sử dụng tin nhắn đầu tiên làm tên
                        {
                            clientName = message;
                            clientNames.Add(client, clientName);
                        }

                        foreach (TcpClient otherClient in clients)
                        {
                            if (otherClient != client)
                            {
                                NetworkStream otherStream = otherClient.GetStream();
                                byte[] response = Encoding.Unicode.GetBytes(message);
                                otherStream.Write(response, 0, response.Length);
                            }
                        }
                    }
                }

            }

            clients.Remove(client);
            int disconnectedPort = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
            connectedClientsDict.Remove(disconnectedPort); // Xóa cổng kết nối khi client ngắt kết nối
            connectedClients.Remove(client.Client.RemoteEndPoint.ToString());
            clientNames.Remove(client); // Xóa thông tin người dùng khi client ngắt kết nối
            UpdateConnectedClients();
        }


        private void UpdateConnectedClients()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string client in connectedClients)
            {
                sb.AppendLine(client);
            }
            richTextBox3.Invoke(new Action(() => richTextBox3.Text = sb.ToString()));

        }
    }
}