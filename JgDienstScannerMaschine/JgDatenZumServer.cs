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
                        Formatter = new XmlMessageFormatter(new Type[] { typeof(ServiceRef.JgWcfMeldung), typeof(ServiceRef.JgWcfBauteil) })
                    };

                    while (true)
                    {
                        try
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
                                            var erg = queue.Receive(new TimeSpan(0, 10, 0), myTransaction);
                                            sendObj = erg.Body;
                                        }
                                        catch (MessageQueueException ex)
                                        {
                                            myTransaction.Abort();

                                            if (ex.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                                                throw new Exception($"Fehler lesen Meldung aus MessageQueue!", ex);
                                        }

                                        // Wenn nach 10 Minute keine Aktion vorhanden ist, wird die Verbindung für 10 Minuten 
                                        // geschlossen und danach wieder neu gestartet.

                                        if (sendObj == null) 
                                        {
                                            JgLog.Set(null, $"Verbindung wird wegen Inaktivität für 10 Minuten geschlossen !", JgLog.LogArt.Info);
                                            //todo Verbindungsauszeit eintragen
                                            Thread.Sleep(new TimeSpan(0, 10, 0));
                                        }
                                        else
                                        {
                                            if (sendObj is ServiceRef.JgWcfMeldung wcfMeldung)
                                            {
                                                var maschine = JgInit.GetMaschine(optSenden.ListeMaschinen, wcfMeldung.IdMaschine);
                                                var maStatus = new JgMaschinenStatus(maschine, optSenden.PfadDaten);
                                                maStatus.SaveStatusMaschineLocal();

                                                var antwortServer = verb.SendeMeldung(wcfMeldung, maStatus.GetStatusAsXmlByte());

                                                if (antwortServer.Substring(0, 2) == "OK")
                                                {
                                                    myTransaction.Commit();
                                                    if (antwortServer.Length > 2)
                                                        JgLog.Set(null, antwortServer.Substring(2), JgLog.LogArt.Fehler);
                                                    else
                                                        JgLog.Set(null, $"Wcf Meldung {wcfMeldung.Meldung} mit Id {wcfMeldung.Id} gesendet", JgLog.LogArt.Info);
                                                }
                                                else
                                                {
                                                    myTransaction.Abort();
                                                    JgLog.Set(null, $"Wpf 'Meldung' Fehler durch Server!\nGrund: {antwortServer}", JgLog.LogArt.Fehler);
                                                }
                                            }
                                            else if (sendObj is ServiceRef.JgWcfBauteil wcfBauteil)
                                            {
                                                var maschine = JgInit.GetMaschine(optSenden.ListeMaschinen, wcfBauteil.IdMaschine);
                                                var maStatus = new JgMaschinenStatus(maschine, optSenden.PfadDaten);
                                                maStatus.SaveStatusMaschineLocal();

                                                var antwortServer = verb.SendeBauteil(wcfBauteil, maStatus.GetStatusAsXmlByte());

                                                if (antwortServer.Substring(0, 2) == "OK")
                                                {
                                                    myTransaction.Commit();
                                                    if (antwortServer.Length > 2)
                                                        JgLog.Set(null, antwortServer.Substring(2), JgLog.LogArt.Fehler);
                                                    else
                                                        JgLog.Set(null, $"Wcf Bauteil mit Id {wcfBauteil.Id} gesendet", JgLog.LogArt.Info);
                                                }
                                                else
                                                {
                                                    myTransaction.Abort();
                                                    JgLog.Set(null, $"Wpf 'Bauteil' Fehler durch Server!\nGrund: {antwortServer}", JgLog.LogArt.Fehler);
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            catch (TimeoutException ex)
                            {
                                if (myTransaction != null)
                                    myTransaction.Abort();

                                throw new TimeoutException("Wpf Zeitüberschreitung", ex);
                            }
                            catch (FaultException faultEx)
                            {
                                if (myTransaction != null)
                                    myTransaction.Abort();
                                switch (faultEx.Code.Name)
                                {
                                    case "InternalServiceFault":
                                        JgLog.Set(null, faultEx.Message, JgLog.LogArt.Info);
                                        Thread.Sleep(10000);
                                        break;
                                    default:
                                        var msg = (faultEx.Code.SubCode == null) ? "" : " -> Subcode: " + faultEx.Code.SubCode.Name;
                                        msg = $"WCF Faultexeption -> {faultEx.Code.Name} Message: {faultEx.Message} {msg}";
                                        JgLog.Set(null, msg, JgLog.LogArt.Info);
                                        throw new Exception(msg, faultEx);
                                }
                            }
                            catch (CommunicationException ex)
                            {
                                if (myTransaction != null)
                                    myTransaction.Abort();

                                JgLog.Set(null, $"Verbindung zum Server konnte nicht aufgebaut werden !\nGrund: {ex.Message}", JgLog.LogArt.Warnung);
                                Thread.Sleep(10000);
                            }
                            catch (Exception ex)
                            {
                                if (myTransaction != null)
                                    myTransaction.Abort();
                                throw new Exception("WCF - Unbekannter Fehler", ex);
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
