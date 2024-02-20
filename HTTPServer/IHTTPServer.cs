using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace HTTPServer
{
    public delegate void RequestReceived(string ID, string URL);

    interface IHTTPServer
    {
        event RequestReceived RequestReceived;

        void StartListening(string IP, string Port);

        void AnswerRequest(string ID, string Message);

        void StopListening();
    }
}