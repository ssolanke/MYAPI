using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeAPI.Models
{
    public class ExchangeInfo
    {
        [Required]
        [DataType(DataType.Date)]
        public ICollection<DateTime> dates { get; set; }

        [Required]
        public string baseCurrency { get; set; }

        [Required]
        public string targetCurrency { get; set; }
       // double exchangeRate { get; set; }
    }
}