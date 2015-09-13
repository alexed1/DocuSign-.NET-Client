using Newtonsoft.Json;
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
        public ConnectProfile Get()
        {
            return MakeRequest<ConnectProfile>("GET", string.Empty, string.Empty);
        }

        public Configuration Create(Configuration configurations)
        {
            return MakeRequest<Configuration>("POST", JsonConvert.SerializeObject(configurations), string.Empty);
        }

        public Configuration Update(Configuration configurations)
        {
            return MakeRequest<Configuration>("PUT", JsonConvert.SerializeObject(configurations), string.Empty);
        }

        public void Delete(string connectId)
        {
            MakeRequest<String>("DELETE", string.Empty, string.Format("/{0}",connectId));
        }

        private T MakeRequest<T>(string method, string requestBody, string connectId)
        {
            RequestBuilder builder = new RequestBuilder();
            RequestInfo req = new RequestInfo();
            List<RequestBody> requestBodies = new List<RequestBody>();

            req.RequestContentType = "application/json";
            req.AcceptContentType = "application/json";
            req.HttpMethod = method;
            req.LoginEmail = this.Login.Email;
            req.ApiPassword = this.Login.ApiPassword;
            req.DistributorCode = RestSettings.Instance.DistributorCode;
            req.DistributorPassword = RestSettings.Instance.DistributorPassword;
            req.IntegratorKey = RestSettings.Instance.IntegratorKey;
            req.Uri = string.Format("{0}/connect{1}", this.Login.BaseUrl,connectId);

            builder.Request = req;
            builder.Proxy = this.Proxy;

            if (!string.IsNullOrEmpty(requestBody))
            {
                RequestBody rb = new RequestBody();
                rb.Text = requestBody;
                requestBodies.Add(rb);
                req.RequestBody = requestBodies.ToArray();
                builder.Request = req;
            }
            ResponseInfo response = builder.MakeRESTRequest();
            this.Trace(builder, response);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.ParseErrorResponse(response);
               // return null;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            return JsonConvert.DeserializeObject<T>(response.ResponseText);
        }

    }
}
