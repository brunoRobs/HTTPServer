using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServer
{
    class clsLog : ILog
    {
        int MaxLength;

        public void Init(string DefaultLogFile, int MaxLogLen)
        {
            MaxLength = MaxLogLen;

            if (!File.Exists(DefaultLogFile))
            {
                var file = new FileStream(DefaultLogFile, FileMode.Create);

                file.Close();
            }
        } 
            
        public void Log(string fpMessage, string LogFile)
        {
            var log = new FileStream(LogFile, FileMode.Open);

            if (log.Length >= MaxLength)
            {
                log.Close();

                File.Move(log.Name, Path.GetFileNameWithoutExtension(log.Name) + "_old.txt");

                log.SetLength(0);

                log = new FileStream(LogFile, FileMode.Append);
            }

            byte[] message = Encoding.UTF8.GetBytes(fpMessage);

            log.Write(message, 0, message.Length);

            log.Close();
        }
    }
}