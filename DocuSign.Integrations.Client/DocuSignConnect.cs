using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocuSign.Integrations.Client
{
    public class DocuSignConnect
    {
        public Account Login { get; set; }
        public WebProxy Proxy { get; set; }
        public string RestTrace { get; private set; }
        public Error RestError { get; private set; }
        protected void Trace(RequestBuilder utils, ResponseInfo response)
        {
            if (RestSettings.Instance.RestTracing == true)
            {
                StringBuilder sb = new StringBuilder(this.RestTrace);
                sb.AppendLine();
                sb.AppendLine(utils.Dump());
                sb.AppendLine();
                sb.AppendLine("Response:");
                sb.AppendLine(response.ResponseText);
                sb.AppendLine();
                this.RestTrace = sb.ToString();
            }
        }
        protected void ParseErrorResponse(ResponseInfo response)
        {
            try
            {
                this.RestError = Error.FromJson(response.ResponseText);
            }
            catch
            {
                this.RestError = new Error { message = response.ResponseText };
            }

            this.RestError.httpStatusCode = response.StatusCode;
        }


        public JObject Get(string accountId)
        {
            RequestBuilder builder = new RequestBuilder();
            RequestInfo req = new RequestInfo();
            List<RequestBody> requestBodies = new List<RequestBody>();

            req.RequestContentType = "application/json";
            req.AcceptContentType = "application/json";
            req.HttpMethod = "GET";
            req.LoginEmail = this.Login.Email;
            req.ApiPassword = this.Login.ApiPassword;
            req.DistributorCode = RestSettings.Instance.DistributorCode;
            req.DistributorPassword = RestSettings.Instance.DistributorPassword;
            req.IntegratorKey = RestSettings.Instance.IntegratorKey;
            req.Uri = string.Format("{0}/accounts/{1}/connect", this.Login.BaseUrl, accountId);

            builder.Request = req;
            builder.Proxy = this.Proxy;

            ResponseInfo response = builder.MakeRESTRequest();
            this.Trace(builder, response);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.ParseErrorResponse(response);
                return null;
            }
            JObject json = JObject.Parse(response.ResponseText);

            //var names = new List<string>();
            //var signers = json["signers"];
            //foreach (var signer in signers)
            //    names.Add((string)signer["name"]);
            //var ccs = json["carbonCopies"];
            //foreach (var cc in ccs)
            //    names.Add((string)cc["name"]);
            //var certifiedDeliveries = json["certifiedDeliveries"];
            //foreach (var cd in certifiedDeliveries)
            //    names.Add((string)cd["name"]);
            //return names;
            return json;
        }
    }
}
