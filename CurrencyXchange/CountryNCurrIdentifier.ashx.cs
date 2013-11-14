using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;


namespace CurrencyXchange
{
    /// <summary>
    /// Summary description for CountryNCurrIdentifier
    /// </summary>
    public class CountryNCurrIdentifier : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Added this for local machine debug
                    string res = GetCountryAndCurrency(context);
                    context.Response.ContentType = "text/plain";                    
                    context.Response.ContentType = "application/json";
                    context.Response.Write(res);
                
         }           
        
        public bool ValidateNos(string no, out string errorCode)
        {
            errorCode = string.Empty;
            long num = 0;
            bool isNum = false;

            //Check if it is a numeral
            isNum = long.TryParse(no, out num);
            if (!isNum)
            {
                errorCode = "Invalid number";
                return false;
            }
            
            //Differentiate ISD code from nos

            //Check if the numbers don't repeat

            //Use regex, but don't believe there is a regex for validating all Intl nos

            return false;
        }

        public string GetCountryAndCurrency(HttpContext context)
        {


            string errorCode = string.Empty;
            string res = string.Empty;
            string[] SrcCodeAndNo = new string[2];
            string[] TargetCodeAndNo = new string[2];


            if (context.Request.RequestType == "GET")
            {

                //RawUrl=http://localhost/Services.xch/?1-8572349876/91-6789095673
                string RawUrl = context.Request.RawUrl;
                string splitter = "/?";
                string SubRawUrl = RawUrl.Substring(RawUrl.IndexOf(splitter) + splitter.Length);
                string[] phoneNos = SubRawUrl.Split('/');

                Match m = Regex.Match(SubRawUrl.ToString(), @"[0-9-]");
                if (!m.Success)
                    return "Invalid Input";


                SrcCodeAndNo = phoneNos[0].Split('-');
                TargetCodeAndNo = phoneNos[1].Split('-');


                foreach (var item in SrcCodeAndNo)
                {
                    bool val = ValidateNos(item, out errorCode);
                    if (!val)
                        return errorCode;

                }

                foreach (var item in TargetCodeAndNo)
                {
                    bool val = ValidateNos(item, out errorCode);
                    if (!val)
                        return errorCode;

                }

                res = GetMockValues();


            }

            return res;
          
        }

        public string GetMockValues()
        {
              MockDB1 mock = new MockDB1();

            mock.SrcISDCode = "1";
            mock.SrcCountry = "USA";
            mock.SrcCurrencyCode = "USD";
            
            mock.TargetISDCode = "44";
            mock.TargetCountry = "UK";
            mock.TargetCurrencyCode = "GBP";

            mock.ConvertUrl = @"http://localhost/convert.aspx?USD2GBP";
            StringBuilder sb = new StringBuilder();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            ser.Serialize(mock, sb);
            return sb.ToString();
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