using JgLibHelper;
using System;
using System.Messaging;

namespace JgDienstScannerMaschine
{
    public class JgOptionenCraddle
    {
        private MessageQueue _Queue { get; }

        public JgOptionen JgOpt { get; }

        public string CraddleIpAdresse { get; set; } = "";
        public int CraddlePort { get; set; } = 0;
        public string TextVerbinungOk { get; set; } = "";

        public string TextBeiFehler { get; } = "#FehlerCraddle";

        public string Info { get => $"Craddle Ip: {CraddleIpAdresse} Port: {CraddlePort}"; } 

        public JgOptionenCraddle(JgOptionen MyOptionen)
        {
            JgOpt = MyOptionen;
            _Queue = new MessageQueue(JgOpt.PathQueue, QueueAccessMode.Send);
        }

        public void QueueSend(string MyLabel, JgMaschineStamm Maschine, JgMeldung Meldung)
        {
            QueueSendAusfuehren(MyLabel, new ServiceRef.JgWcfMeldung()
            {
                Aenderung = Meldung.Aenderung,
                Id = Meldung.Id,
                Anzahl = Meldung.Anzahl,
                IdBediener = Meldung.IdBediener,
                IdMaschine = Maschine.Id
            });
        }

        public void QueueSend(string MyLabel, JgBauteil Bauteil)
        {
            QueueSendAusfuehren(MyLabel, Bauteil);
        }

        public void QueueSendAusfuehren(string MyLabel, object SendObjekt)
        {
            try
            {
                var trans = new MessageQueueTransaction();
                trans.Begin();
                _Queue.Send(SendObjekt, MyLabel, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                JgLog.Set($"Daten konnten nicht an MessageQueue übergeben werden !\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }
    }
}
