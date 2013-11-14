using System;
using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CurrencyXchange
{
    /// <summary>
    /// Using HTTP Handler for REST API rather than using WCF or ASMX in .Net. This light weight and easy to implement.
    /// </summary>
    public class CurrencyConverter : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Added this for local machine debug
            System.Diagnostics.Debugger.Break();
            string result = ConvertCurrency(context);
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public string ConvertCurrency(HttpContext context)
        {

            string errorCode = string.Empty;
            string res = string.Empty;
            string SourceCountryName = string.Empty;
            string TargetCountryName = string.Empty;
            string InputAmt = string.Empty;
            string OutputAmt = string.Empty;
            string jsonRes = string.Empty;
            string ConversionResult = string.Empty;

            Dictionary<string, string> values = new System.Collections.Generic.Dictionary<string, string>();


           // string RawUrlx = "http://localhost/currency/?from=USD&to=EUR&q=1";
            string RawUrlx = context.Request.RawUrl;
            NameValueCollection nm = HttpUtility.ParseQueryString(RawUrlx.Substring(RawUrlx.IndexOf("/?") + 2));


            // Populating the key-value object just for validation purposes
            SourceCountryName = nm.Get("from");
            TargetCountryName = nm.Get("to");
            InputAmt = nm.Get("q");
            nm.Add("ConvertedAmt", string.Empty);

            //Do All the good stuff here - validation

            // Here I could rather copy the request url and use it for the API post
            var requestUri = string.Format("http://rate-exchange.appspot.com/currency?from={0}&to={1}&q={2}", Uri.EscapeDataString(SourceCountryName), Uri.EscapeDataString(TargetCountryName), Uri.EscapeDataString(InputAmt));


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Reading the API call result, parsing the JSON object and populating the key-value object
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    jsonRes = reader.ReadToEnd();
                    values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonRes);
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                }
                throw;
            }

            //Validating Conversion API result
            values.TryGetValue("err", out errorCode);
            values.TryGetValue("warning", out errorCode);

            if (!String.IsNullOrEmpty(errorCode))
                return errorCode;


            values.TryGetValue("v", out OutputAmt);
                return OutputAmt;

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}