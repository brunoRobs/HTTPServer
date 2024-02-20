using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace HTTPServer
{
    public partial class Form1 : Form
    {
        clsHTTPServer HttpServer = new clsHTTPServer();

        clsLog log = new clsLog();

        string path;

        Thread serverThread;

        public Form1()
        {
            InitializeComponent();
        }

        private void HttpServer_RequestReceived(string ID, string URL)
        {
            log.Log($"{DateTime.Now} - Requisição recebida de {ID}: {URL}\n", path);

            URL = txtURL.Text + URL;

            Process.Start(URL);

            log.Log($"{DateTime.Now} - Requisição respondida\n", path);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = true;

            btnEnd.Enabled = true;

            string IP = "";

            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(address))
                {
                    IP = address.ToString();
                    break;
                }
            }

            txtIP.Text = "localhost";

            HttpServer.RequestReceived += HttpServer_RequestReceived;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            serverThread = new Thread(StartServer);

            serverThread.Start();

            btnStart.Enabled = false;

            btnEnd.Enabled = false;

            btnStop.Enabled = true;
        }

        private void StartServer()
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{txtLog.Text}.txt");

            log.Init(path, Int32.Parse(txtLength.Text));

            try
            {
                HttpServer.StartListening(txtIP.Text, txtPort.Text);

                log.Log($"{DateTime.Now} - Servidor {txtIP.Text}:{txtPort.Text} inicializado\n", path);
            }
            catch(Exception error)
            {
                log.Log($"{DateTime.Now} - {error.Message}\n", path);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;

            btnStart.Enabled = true;

            btnEnd.Enabled = true;

            serverThread.Abort();

            try
            {
                HttpServer.StopListening();

                log.Log($"{DateTime.Now} - Servidor {txtIP.Text}:{txtPort.Text} encerrado\n", path);
            }
            catch (Exception error)
            {
                log.Log($"{DateTime.Now} - {error.Message}\n", path);
            }
        }
        
        private void btnEnd_Click(object sender, EventArgs e)
        {
            HttpServer.StopListening();
            Application.Exit();
        }
    }
}