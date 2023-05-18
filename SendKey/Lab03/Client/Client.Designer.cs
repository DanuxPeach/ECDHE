namespace Lab03.Client
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBoxName = new System.Windows.Forms.RichTextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSendFile = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // richTextBoxName
            // 
            this.richTextBoxName.Location = new System.Drawing.Point(263, 28);
            this.richTextBoxName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxName.Name = "richTextBoxName";
            this.richTextBoxName.Size = new System.Drawing.Size(225, 22);
            this.richTextBoxName.TabIndex = 26;
            this.richTextBoxName.Text = "";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(509, 21);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(84, 33);
            this.buttonConnect.TabIndex = 25;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(543, 442);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(60, 33);
            this.buttonSend.TabIndex = 23;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Location = new System.Drawing.Point(15, 444);
            this.richTextBoxMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(473, 30);
            this.richTextBoxMessage.TabIndex = 22;
            this.richTextBoxMessage.Text = "";
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(15, 66);
            this.richTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(587, 366);
            this.richTextBox.TabIndex = 21;
            this.richTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 526);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(19, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 20);
            this.label2.TabIndex = 28;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(200, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 29;
            this.label3.Text = "Name";
            // 
            // buttonSendFile
            // 
            this.buttonSendFile.Location = new System.Drawing.Point(492, 442);
            this.buttonSendFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSendFile.Name = "buttonSendFile";
            this.buttonSendFile.Size = new System.Drawing.Size(48, 33);
            this.buttonSendFile.TabIndex = 30;
            this.buttonSendFile.Text = "File";
            this.buttonSendFile.UseVisualStyleBackColor = true;
            this.buttonSendFile.Click += new System.EventHandler(this.buttonSendFile_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(62, 28);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(100, 22);
            this.textBoxIP.TabIndex = 32;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 487);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.buttonSendFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxName);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.richTextBoxMessage);
            this.Controls.Add(this.richTextBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxName;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSendFile;
        private System.Windows.Forms.TextBox textBoxIP;
    }
}