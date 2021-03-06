﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.everest_minimalist_wallpaper_blue;//Set bg
            panel1.Location = new Point(
            this.ClientSize.Width / 2 - panel1.Size.Width / 2,
            this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = Color.FromArgb(50, Color.White); // panel en blanc transparent 
        }



        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream(); //On récup le flux du serveur
                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, inStream.Length);
                string returndata = System.Text.Encoding.UTF8.GetString(inStream); //On le convert en string UTF8
                readData = "" + returndata;
                msg();
                
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                tb_printmessages.Text = tb_printmessages.Text + Environment.NewLine + " >> " + readData; //On Ajout le message à la tb
        }

        private void button2_Click(object sender, EventArgs e)
        {

            clientSocket.Connect("127.0.0.1", 8888); //On se connecte au serveur socket
            readData = "--------------Connecté au serveur Chat !--------------";
            msg();
            serverStream = clientSocket.GetStream();

            byte[] outStream = System.Text.Encoding.UTF8.GetBytes(tb_pseudo.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage); 
            ctThread.Start(); //On lance un thread qui tourne en boucle qui récup les messages du flux
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.UTF8.GetBytes(tb_sendmessage.Text + "$"); //Stream to UTF8
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            tb_sendmessage.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (tb_printmessages.Lines.Count() > 23)
            {
                tb_printmessages.Text = "";
            }
        }
    }
}
