using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Http;

namespace HTTPServer
{
    class clsHTTPServer : IHTTPServer
    {
        HttpListener server = new HttpListener();

        HttpListenerContext request;

        public event RequestReceived RequestReceived;

        public void StartListening(string IP, string Port)
        {
            server.Prefixes.Add($"http://{IP}:{Port}/");

            server.Start();

            while (true)
            {
                request = server.GetContext();

                if(request != null)
                {
                    string URL = request.Request.RawUrl.Substring(1);

                    string ID = request.Request.UserHostAddress;

                    RequestReceived.Invoke(ID, URL);
                }
            }
        }

        public void AnswerRequest(string ID, string Message)
        {
            HttpListenerResponse response = request.Response;

            response.StatusCode = (int)HttpStatusCode.OK;

            response.AddHeader("Location", Message);

            response.Close();
        }

        public void StopListening()
        {
            if (server.IsListening)
            {
                server.Stop();
            }
        }
    }
}