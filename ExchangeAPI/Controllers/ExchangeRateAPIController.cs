using ExchangeAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ExchangeAPI.Controllers
{
    public class ExchangeRateAPIController : ApiController
    {
        static HttpClient client;
        

        public ExchangeRateAPIController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(RequestConstant.BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        
        
        [System.Web.Http.HttpPost]
        public async Task<ResultInfo> GetAPI([FromBody] ExchangeInfo model)
        {
            ResultInfo resultInfo = new ResultInfo();
            if (ModelState.IsValid)
            {
                ResultExchangeInfo[] resultExchangeInfos = new ResultExchangeInfo[model.dates.Count];
                for (int i = 0; i < model.dates.Count; i++)
                {
                    string responseData = "";
                    var httpResponseMessage = await GetExchangeAPI(model.dates.ToArray()[i], model.baseCurrency, model.targetCurrency);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        responseData = httpResponseMessage.Content.ReadAsStringAsync().Result;

                        JObject jObj = JObject.Parse(responseData);
                        string exchangeRate = jObj["rates"][model.targetCurrency].ToString();
                        resultExchangeInfos[i] = new ResultExchangeInfo();
                        resultExchangeInfos[i].date = model.dates.ToArray()[i];
                        resultExchangeInfos[i].exchangeRate = Convert.ToDouble(jObj["rates"][model.targetCurrency]);
                    }
                    else
                    {
                        responseData = httpResponseMessage.ReasonPhrase;
                    }
                }

                if (resultExchangeInfos.Any())
                {
                    resultInfo.minExchangeRate = resultExchangeInfos.Min(m => m.exchangeRate);
                    resultInfo.minDate = resultExchangeInfos.First(m => m.exchangeRate == resultInfo.minExchangeRate).date;

                    resultInfo.maxExchangeRate = resultExchangeInfos.Max(m => m.exchangeRate);
                    resultInfo.maxDate = resultExchangeInfos.First(m => m.exchangeRate == resultInfo.maxExchangeRate).date;

                    resultInfo.avgRate = resultExchangeInfos.Average(m => m.exchangeRate);
                }
                else
                {
                    resultInfo.Error = "Bad Request";
                }
            }
            else
            {
                resultInfo.Error = "Bad Request";
            }
                return resultInfo; ;
            
        }
        // GET: EchnageRateInfo
        [System.Web.Mvc.Route("{dates}/{baseCurrency}/{targetCurrency}")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetExchangeAPI(DateTime dates,string baseCurrency, string targetCurrency)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                string responseData = "";
                // model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExchangeInfo>(JsonConvert.SerializeObject(strData));

                responseMessage = await client.GetAsync($"{dates.ToString("yyyy-MM-dd")}?base={baseCurrency}&symbols={targetCurrency}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    responseData = responseMessage.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    responseData = responseMessage.ReasonPhrase;
                }
                
            }
            catch (Exception e)
            {                
                responseMessage.ReasonPhrase= "Error calling API. Please do manual lookup."+ e.Message;
            }

            return responseMessage;
        }
    }
}
