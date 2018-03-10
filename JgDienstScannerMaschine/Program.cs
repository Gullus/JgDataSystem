using JgLibHelper;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetLogWriter(new LogWriterFactory().Create());
            ExceptionPolicy.SetExceptionManager(new ExceptionPolicyFactory().CreateManager(), false);
            // Bsp.: ExceptionPolicy.HandleException(ex, "Policy");

            Console.WriteLine("Scanner Gestartet");
            JgLog.Set(null, "Programm wird gestartet!", JgLog.LogArt.Info);

            var pr = Properties.Settings.Default;

            var jgOpt = new JgOptionen()
            {
                IdStandort = pr.IdStandort,
            };

            // Bediener und Maschine local laden, danach mit Server abgeleichen

            var init = new JgInit(jgOpt);
            init.BedienerLocalLaden();
            init.MaschinenLocalLaden();

            try
            {
                using (var dienst = new ServiceRef.WcfServiceClient())
                {
                    var tt = dienst.WcfTest("OK?");

                    if (tt == "OK?")
                    {
                        JgLog.Set(null, "Verbindung Server OK!", JgLog.LogArt.Info);

                        if (init.BedienerVonServer(dienst))
                            init.BedienerLocalSpeichern();

                        if (init.MaschinenVonServer(dienst))
                            init.MaschinenLocalSpeichern();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Policy");
            }

            // Status Maschinen laden, wenn vorhanden

            foreach (var maStatus in jgOpt.ListeMaschinen.Values)
               JgMaschinenStatus.LoadStatusMaschineLocal(maStatus, jgOpt.PfadDaten);

            // Optionen für jedes Craddel laden

            var listeCraddleOpt = new List<JgOptionenCraddle>();
            for (int i = 0; i < 5; i++)
            {
                var s = Properties.Settings.Default["Craddel_" + i.ToString()].ToString();
                var crad = new JgOptionenCraddle(jgOpt);
                Helper.PropStringInOnjekt<JgOptionenCraddle>(crad, s);
                if (crad.CraddleIpAdresse != "")
                    listeCraddleOpt.Add(crad);
            }

#if DEBUG

            var lTaskMaschine = new List<JgScannerMaschinen>();

            foreach (var crad in listeCraddleOpt)
            {
                var sm = new JgScannerMaschinen();
                sm.TaskScannerMaschineStarten(crad);
                lTaskMaschine.Add(sm);
            }

            var datenZumServer = new JgDatenZumServer(jgOpt);
            datenZumServer.DatenabgleichStarten();

            Console.WriteLine("Fertig");
            Console.ReadKey();


#else

      var ServiceToRun = new ServiceBase[] { new JgMaschineServiceScanner(scOptionen) };
      ServiceBase.Run(ServiceToRun);

#endif

        }
    }

    public class JgMaschineServiceScanner : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            var msg = "Scannerservice starten!";
            Logger.Write(msg, "Service", 0, 0, System.Diagnostics.TraceEventType.Information);
            base.OnStart(args);
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
            var msg = "Scannerservice herunterfahren!";
            Logger.Write(msg, "Service", 0, 0, System.Diagnostics.TraceEventType.Information);
        }
    }

    [RunInstaller(true)]
    public class Installation : Installer
    {
        private ServiceInstaller _serviceInstall;
        private ServiceProcessInstaller _prozessInstaller;

        public Installation()
        {
            _serviceInstall = new ServiceInstaller()
            {
                ServiceName = "JgMaschine - Scanner",
                DisplayName = "JgMaschine - Scanner",
                Description = "Dienst zum erfassen von Daten und dem Steuern von Baustahl-Maschinen.",
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true,
            };

            _prozessInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalSystem,
            };

            Installers.Add(_serviceInstall);
            Installers.Add(_prozessInstaller);
        }
    }
}
