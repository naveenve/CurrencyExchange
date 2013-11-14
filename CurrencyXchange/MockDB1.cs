using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyXchange
{
    public class MockDB1
    {
        public string SrcISDCode { get; set; }
        public string SrcCountry { get; set; }
        public string SrcCurrencyCode { get; set; }
        public string SrcCurrencyDescription { get; set; }

        public string TargetISDCode { get; set; }
        public string TargetCountry { get; set; }
        public string TargetCurrencyCode { get; set; }
        public string TargetCurrencyDescription { get; set; }

        public string ConvertUrl { get; set; }

        public MockDB1()
        {
           
        }

    }
}