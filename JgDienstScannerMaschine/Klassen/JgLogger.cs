using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace JgDienstScannerMaschine
{
    public static class JgLog
    {
        public enum LogArt
        {
            Unbedeutend,
            Info,
            Warnung,
            Fehler,
            Krittisch,
            Start,
            Stop
        }

        public static void Set(string LogText, LogArt Art)
        {
            System.Diagnostics.TraceEventType art = System.Diagnostics.TraceEventType.Verbose;

            switch (Art)
            {
                case LogArt.Info:
                    art = System.Diagnostics.TraceEventType.Information;
                    break;
                case LogArt.Warnung:
                    art = System.Diagnostics.TraceEventType.Warning;
                    break;
                case LogArt.Fehler:
                    art = System.Diagnostics.TraceEventType.Error;
                    break;
                case LogArt.Krittisch:
                    art = System.Diagnostics.TraceEventType.Critical;
                    break;
                case LogArt.Start:
                    art = System.Diagnostics.TraceEventType.Start;
                    break;
                case LogArt.Stop:
                    art = System.Diagnostics.TraceEventType.Stop;
                    break;
            }

            Logger.Write(LogText, "Service", 0, 0, art);
        }
    }
}
