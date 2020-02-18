using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExchangeAPI.Models
{
    public class ResultInfo
    {
        public double minExchangeRate { get; set; }
        public DateTime minDate { get; set; }
        public double maxExchangeRate { get; set; }
        public DateTime maxDate { get; set; }
        public double avgRate { get; set; }
        public string Error { get; set; }

    }
}