using Firstapp.DBContext;
using Firstapp.Entities;
using Firstapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace Firstapp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        private bool isLoad =false;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index(ViewModel Currencies)
        {
            if (!isLoad)
            {
                LoadData();
                isLoad = true;
            }
            return View(Currencies);
        }

        [HttpPost]
        public async Task<ActionResult> CurrenciesButton(TimeModel model)
        {
            using HttpClient httpClient = new HttpClient();
            string dateCurrencies = model.DateTimeCurrencies.Year.ToString() + "-" + model.DateTimeCurrencies.Month.ToString() + "-" + model.DateTimeCurrencies.Day.ToString();
            var response = await httpClient.GetAsync($"https://api.nbrb.by/exrates/rates?ondate={dateCurrencies}&periodicity=0");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<CurrencyResponse>? cont = JsonConvert.DeserializeObject<List<CurrencyResponse>>(content);
                if (!db.Currencies.Any(c => c.Date == model.DateTimeCurrencies))
                {
                    List<Currency> currencies = cont.Select(c => new Currency
                    {
                        Abbreviation = c.CurAbbreviation,
                        Date = c.Date,
                        CurrencyId = c.CurID,
                        Name = c.CurName,
                        OfficialRate = c.CurOfficialRate,
                        Scale = c.CurScale
                    }).ToList();
                    db.Currencies.AddRange(currencies);
                    await db.SaveChangesAsync();
                    ViewBag.Message = "Данные о валютах загружены в базу данных";
                }
                else ViewBag.Message = "В базе уже есть данные на эту дату";
            }
            else ViewBag.Message = "Нету информации на такую дату о валютах";
            LoadData();
            return View("Index");
        }
        
        [HttpPost]
        public async Task<ActionResult> CurrencyButton(TimeModel model)
        {
            using HttpClient httpClient = new HttpClient();
            ViewModel viewModel = new ViewModel();
            string dateCurrency = model.DateTimeCurrency.Year.ToString() + "-" + model.DateTimeCurrency.Month.ToString() + "-" + model.DateTimeCurrency.Day.ToString();
            var response = await httpClient.GetAsync($"https://api.nbrb.by/exrates/rates/{model.CodeName}?ondate={dateCurrency}&parammode=2");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                CurrencyResponse? cont = JsonConvert.DeserializeObject<CurrencyResponse>(content);
                viewModel.ListCurrency.Add(cont);
                LoadData();
                return View("Index", viewModel);
            }
            else 
            {
                ViewBag.Message = "Нету информации на такую дату о валюте ";
                LoadData();
            }
            return View("Index");
        }

        private void LoadData()
        {
            var client = new HttpClient();
            HttpResponseMessage responseCurrencies = client.GetAsync("https://api.nbrb.by/exrates/currencies").Result;
            if (responseCurrencies.IsSuccessStatusCode)
            {
                var data = responseCurrencies.Content.ReadAsStringAsync().Result;
                List<AllCurrencies>? currencies = JsonConvert.DeserializeObject<List<AllCurrencies>>(data);
                ViewBag.Currencies = new SelectList(currencies, "Cur_Abbreviation", "Cur_Abbreviation");
            }
        }
    }
}
