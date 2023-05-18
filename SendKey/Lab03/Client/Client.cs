using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab03.Client
{
    public partial class Client : Form
    {
        TcpClient client;
        NetworkStream stream;
        string clientName;
        public Client()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            client = new TcpClient(textBoxIP.Text, 9494);
            stream = client.GetStream();

            richTextBox.AppendText("Connected to server.\n");
            richTextBox.AppendText("You've joined.\n");
            clientName = richTextBoxName.Text;

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = richTextBoxMessage.Text;
            if (message.StartsWith("@")) // Kiểm tra xem tin nhắn có phải là riêng tư hay không
            {
                string[] parts = message.Split(' ');
                int recipientPort = int.Parse(parts[0].Substring(1)); // Lấy cổng (port) người nhận
                string privateMessage = string.Join(" ", parts.Skip(1)); // Lấy nội dung tin nhắn riêng tư

                message = "@" + recipientPort + " " + privateMessage;

            }
            message = $"[{client.Client.LocalEndPoint}]{clientName}: {message}"; // Thêm ip:port của client vào message
            richTextBox.AppendText(message + "\n");
            richTextBoxMessage.Clear();

            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);

        }

        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);

                    
                    if (message.StartsWith("[file]")) // Kiểm tra xem tin nhắn gửi đi có phải file hay không
                    {
                        // Tin nhắn chứa tập tin được gửi từ server
                        string fileName = message.Substring(6);
                        string filePath = Path.Combine(Application.StartupPath, fileName);

                        // Lưu tập tin vào thư mục của ứng dụng
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fileStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                    else
                    {
                        if (message.StartsWith("@")) // Kiểm tra xem tin nhắn có phải là riêng tư hay không
                        {
                            string[] parts = message.Split(' ');
                            int senderPort = int.Parse(parts[0].Substring(1)); // Lấy cổng (port) người gửi
                            string privateMessage = string.Join(" ", parts.Skip(1)); // Lấy nội dung tin nhắn riêng tư

                            richTextBox.Invoke(new Action(() => richTextBox.AppendText($"[{senderPort}] {privateMessage}\n")));

                        }
                        else
                        {
                            richTextBox.Invoke(new Action(() => richTextBox.AppendText(message + "\n")));
                        }
                    }
                }
            }
            catch
            {

            }


        }

        private void buttonSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // Kiểm tra xem người dùng đã chọn tập tin hay chưa
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                // Đọc tập tin cần gửi
                byte[] fileData = File.ReadAllBytes(openFileDialog.FileName);

                // Gửi tập tin đến server
                string fileName = Path.GetFileName(openFileDialog.FileName);
                string message = $"[file]{fileName}";
                byte[] messageData = Encoding.Unicode.GetBytes(message);
                stream.Write(messageData, 0, messageData.Length);
                stream.Write(fileData, 0, fileData.Length);
            }
            catch
            {

            }
        }
    }
}
