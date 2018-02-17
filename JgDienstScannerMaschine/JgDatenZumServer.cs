using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Messaging;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace JgDienstScannerMaschine
{
    public class JgDatenZumServer
    {
        private JgOptionen _JgOpt;

        public JgDatenZumServer(JgOptionen Optionen)
        {
            _JgOpt = Optionen;
        }

        public void DatenabgleichStarten()
        {
            var ctDatenabfrage = _JgOpt.CraddelTokensSource.Token;
            var task = new Task((JgOptionenSenden) =>
            {
                var optSenden = (JgOptionen)JgOptionenSenden;

                while (true)
                {
                    var queue = new MessageQueue(optSenden.PathQueue, QueueAccessMode.Receive)
                    {
                        Formatter = new XmlMessageFormatter(new Type[] { typeof(ServiceRef.JgWcfMeldung), typeof( ServiceRef.JgWcfBauteil) })
                    };

                    while (true)
                    {
                        var queuHatDaten = false;

                        try
                        {
                            try
                            {
                                var msg = queue.Peek(new TimeSpan(0, 0, 5));
                                queuHatDaten = true;
                            }
                            catch (MessageQueueException ex)
                            {
                                if (ex.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                                    throw new Exception("Fehler bei Kontrolle auf Daten im Queue", ex);
                            }

                            if (queuHatDaten)
                            {
                                MessageQueueTransaction myTransaction = null;

                                try
                                {
                                    using (var verb = new ServiceRef.WcfServiceClient())
                                    {
                                        while (true)
                                        {
                                            object sendObj = null;
                                            myTransaction = new MessageQueueTransaction();

                                            try
                                            {
                                                myTransaction.Begin();
                                                var erg = queue.Receive(new TimeSpan(0, 0, 10), myTransaction);
                                                sendObj = erg.Body;
                                            }
                                            catch (MessageQueueException ex)
                                            {
                                                myTransaction.Abort();

                                                if (ex.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                                                    throw new Exception($"Fehler lesen Meldung aus MessageQueue!", ex);
                                            }

                                            if (sendObj != null)
                                            {
                                                if (sendObj is ServiceRef.JgWcfMeldung wcfMeldung)
                                                {
                                                    var maschine = JgInit.GetMaschine(optSenden.ListeMaschinen, wcfMeldung.IdMaschine);
                                                    var maStatus = new JgMaschinenStatus(maschine, optSenden.PfadDaten);
                                                    maStatus.Save();

                                                    var stat = maStatus.GetAsWcfMaschinenStatus();

                                                    if (verb.SendeMeldung(wcfMeldung, maStatus.GetAsWcfMaschinenStatus()))
                                                    {
                                                        myTransaction.Commit();
                                                        JgLog.Set($"Wcf Meldung {wcfMeldung.Meldung} mit Id {wcfMeldung.Id} gesendet", JgLog.LogArt.Info);
                                                    }
                                                    else
                                                        myTransaction.Abort();
                                                }

                                            }
                                        }
                                    }
                                }
                                catch (TimeoutException ex)
                                {
                                    if (myTransaction != null)
                                        myTransaction.Abort();
                                    JgLog.Set($"WCF Zeitüberschreitung {ex.Message}", JgLog.LogArt.Fehler);
                                    throw new TimeoutException("Wpf Zeitüberschreitung", ex);
                                }
                                catch (FaultException faultEx)
                                {
                                    if (myTransaction != null)
                                        myTransaction.Abort();
                                    switch (faultEx.Code.Name)
                                    {
                                        case "InternalServiceFault":
                                            JgLog.Set(faultEx.Message, JgLog.LogArt.Info);
                                            Thread.Sleep(10000);
                                            break;
                                        default:
                                            var msg = (faultEx.Code.SubCode == null) ? "" : " -> Subcode: " + faultEx.Code.SubCode.Name;
                                            msg = $"WCF Faultexeption -> {faultEx.Code.Name} Message: {faultEx.Message} {msg}";
                                            JgLog.Set(msg, JgLog.LogArt.Info);
                                            throw new Exception(msg, faultEx);
                                    }
                                }
                                catch (CommunicationException ex)
                                {
                                    if (myTransaction != null)
                                        myTransaction.Abort();
                                    JgLog.Set($"WCF Kommunikationsproblem {ex.Message} - {ex.StackTrace}", JgLog.LogArt.Fehler);
                                    throw new CommunicationException("WCF Kommunikationsproblem", ex);
                                }
                                catch (Exception ex)
                                {
                                    if (myTransaction != null)
                                        myTransaction.Abort();
                                    JgLog.Set($"WCF - Unbekannter Fehler. \nGrund: {ex.Message} - {ex.StackTrace}", JgLog.LogArt.Fehler);
                                    throw new Exception("WCF - Unbekannter Fehler", ex);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            ExceptionPolicy.HandleException(ex, "Policy");
                            Thread.Sleep(10000);
                            break;
                        }
                    }
                }
            }, _JgOpt, ctDatenabfrage, TaskCreationOptions.LongRunning);

            task.Start();
        }
    }
}
