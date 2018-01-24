using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Evaluation
{
    public static class logger
    {
        static logger()
        {
            TextWriterTraceListener tr1 = new TextWriterTraceListener(System.Console.Out);
            //Debug.Listeners.Add(tr1);
            Trace.Listeners.Add(tr1);

            //TextWriterTraceListener tr2 = new TextWriterTraceListener(System.IO.File.CreateText("Output.txt"));
            //Debug.Listeners.Add(tr2);
        }

        public static void write(string message)
        {
            Info(message, "Application");
        }

        public static void ExportToFile(string fileName, string content)
        {
            FileStream fs;
            StreamWriter sw;
            try
            {
                using (fs = new FileStream(fileName, FileMode.Create))
                {
                    using (sw = new StreamWriter(fs))
                    {
                        sw = new StreamWriter(fs);
                        sw.Write(content);
                        sw.Flush();
                    }
                }
                logger.write("Export to " + fileName + " complete.");
            }
            catch (Exception ex)
            {
                logger.write("Error in file export to " + fileName + "\n" + ex.Message);
            }
        }

        #region New

        public static void Error(string message, string module)
        {
            WriteEntry(message, "error", module);
        }

        public static void Error(Exception ex, string module)
        {
            WriteEntry(ex.Message, "error", module);
        }

        public static void Warning(string message, string module)
        {
            WriteEntry(message, "warning", module);
        }

        public static void Info(string message, string module)
        {
            WriteEntry(message, "info", module);
        }

        private static void WriteEntry(string message, string type, string module)
        {
            string LogEntry = 
                String.Format("{0} # {1} # {2} # {3}",
                DateTime.Now.ToString("hh:mm:ss:fff"),
                type,
                module,
                message
                );

            Trace.WriteLine(LogEntry);

        }
        #endregion
    }
}
