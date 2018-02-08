using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JgMonitor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MessageQueue _Message = null;
        private bool _FlagInArbeit = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void SchreibText(string Wert)
        {
            TxtBlock.Text += Wert;
            SbView.ScrollToEnd();
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            SchreibText("\n\nMessageQueue gestartet ......\n");

            _FlagInArbeit = true;
            _Message = new MessageQueue(Properties.Settings.Default.AdresseMessageQueue, QueueAccessMode.Receive)
            {
                Formatter = new XmlMessageFormatter(new String[] { "System.String, mscorlib" })
            };

            while (_FlagInArbeit)
            {
                var erg = await WarteAufNachricht(_Message);

                if (erg != "#TimeOut")
                    SchreibText(erg);
            }

            SchreibText("\n\nMessageQueue beendet ......\n");
            _Message.Close();
        }

        private static Task<string> WarteAufNachricht(MessageQueue MyQueue)
        {
            var t = new Task<string>((myQueue) =>
            {
                var mq = (MessageQueue)myQueue;
                var msg = "";

                try
                {
                    var erg = MyQueue.Receive(new TimeSpan(0, 0, 4));
                    msg = $"{erg.Label}\n{erg.Body.ToString()}";
                }
                catch (MessageQueueException ex)
                {
                    if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                        msg = "#TimeOut";
                }
                catch (InvalidOperationException ex)
                {
                    msg = $"\nFehler MessageQueue.\nGrund: {ex.Message}";
                }

                return msg;
            }, MyQueue);

            t.Start();
            return t;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _FlagInArbeit = false;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtBlock.Clear();
        }
    }
}
