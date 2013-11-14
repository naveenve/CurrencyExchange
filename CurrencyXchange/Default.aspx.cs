using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.IO;



namespace CurrencyXchange
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string errorCode = string.Empty;
            string res = string.Empty;
            string SourceCountryName = string.Empty;
            string TargetCountryName = string.Empty;
            string InputAmt = string.Empty;
            string OutputAmt = string.Empty;
            string jsonRes = string.Empty;

            Dictionary<string, string> values = new System.Collections.Generic.Dictionary<string,string>();


                string RawUrlx = "http://localhost/currency/?from=USD&to=EUR&q=1";
                NameValueCollection nm = HttpUtility.ParseQueryString(RawUrlx.Substring(RawUrlx.IndexOf("/?") + 2));
          
                SourceCountryName = nm.Get("from");
                TargetCountryName = nm.Get("to");
                InputAmt = nm.Get("q");
                nm.Add("ConvertedAmt",string.Empty);

                //Do All the good stuff here - validation


                var requestUri = string.Format("http://rate-exchange.appspot.com/currency?from={0}&to={1}&q={2}", Uri.EscapeDataString(SourceCountryName), Uri.EscapeDataString(TargetCountryName), Uri.EscapeDataString("1.23x"));


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                try
                {
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
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


                values.TryGetValue("err", out errorCode);
                values.TryGetValue("warning", out errorCode);
               // if (error)
                  //  return errorCode;



                bool successfulConv = values.TryGetValue("v", out OutputAmt);    
        }

      


    
    }
}