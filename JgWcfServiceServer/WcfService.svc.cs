using JgWcfServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace JgWcfServiceServer
{
    public class WcfService : IWcfService
    {
        public List<JgDbBediener> GetBediener()
        {
            throw new NotImplementedException();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public List<JgDbMaschine> GetMaschinen()
        {
            throw new NotImplementedException();
        }

        public JgWcfOptionen GetOptionen(Guid IdStanort)
        {
            throw new NotImplementedException();
        }

        public bool SendeBauteil(JgWcfBauteil Bauteil)
        {
            throw new NotImplementedException();
        }

        public bool SendeMaschinenanmeldung(JgWcfMaschinenanmeldung Anmeldung)
        {
            throw new NotImplementedException();
        }

        public bool SendeMaschinenStatus(JgWcfMaschine Maschine)
        {
            throw new NotImplementedException();
        }

        public bool SendeMeldung(JgWcfMeldung Meldung)
        {
            throw new NotImplementedException();
        }

        public string SendeTest(string value)
        {
            throw new NotImplementedException();
        }
    }
}
