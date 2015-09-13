using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSign.Integrations.Client
{
    [Serializable]
    public class ConnectProfile
    {
        public List<Configuration> configurations { get; set; }
        public string totalRecords { get; set; }
    }

    [Serializable]
    public class Configuration
    {
        public string connectId { get; set; }
        public string configurationType { get; set; }
        public string urlToPublishTo { get; set; }
        public string name { get; set; }
        public string allowEnvelopePublish { get; set; }
        public string enableLog { get; set; }
        public string includeDocuments { get; set; }
        public string includeCertificateOfCompletion { get; set; }
        public string requiresAcknowledgement { get; set; }
        public string signMessageWithX509Certificate { get; set; }
        public string useSoapInterface { get; set; }
        public string includeTimeZoneInformation { get; set; }
        public string includeEnvelopeVoidReason { get; set; }
        public string includeSenderAccountasCustomField { get; set; }
        public string envelopeEvents { get; set; }
        public string recipientEvents { get; set; }
        public string userIds { get; set; }
        public string soapNamespace { get; set; }
        public string allUsers { get; set; }
        public string includeCertSoapHeader { get; set; }
        public string includeDocumentFields { get; set; }
    }
}
