using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeadSpin.Core.Utilities.Logging
{
    public class FileLog
    {
        public static void LogMsg(string message)
        {
            (new FileLog(ConfigHelper.GetAppSettingValue("file-log"))).Log(message);
        }

        private string _Filepath = "";

        public FileLog(string filepath)
        {
            _Filepath = filepath;
        }

        public void Log(string msg)
        {
            LogMessage(msg, _Filepath);
        }

        internal void LogMessage(string msg, string logFile)
        {

            if (!string.IsNullOrEmpty(logFile))
            {

                lock (this)
                {
                    StreamWriter writer = null;

                    try
                    {
                        if (!File.Exists(logFile))
                        {
                            writer = File.CreateText(logFile);
                        }
                        else
                        {
                            FileStream fs = File.OpenWrite(logFile);
                            fs.Seek(0, SeekOrigin.End);
                            writer = new StreamWriter(fs);
                        }

                        DateTime dt = DateTime.Now;

                        string sDateTime = dt.ToString();
                        sDateTime += " ";
                        sDateTime += dt.Millisecond.ToString();

                        writer.WriteLine(sDateTime);
                        writer.WriteLine(msg);

                        Console.WriteLine(sDateTime + " " + msg);


                        //writer.Flush();

                        writer.WriteLine(" ");
                    }
                    catch (Exception e)
                    {
                        // probably don;t want to re-log this msg b/c wer
                        // could wind up back in here w/a recursion issue.
                        throw e;
                    }
                    finally
                    {
                        if (writer != null)
                        {
                            writer.Close();
                        }
                    }
                }
            }
        }
    }
}
