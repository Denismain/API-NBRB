using Firstapp.DBContext;
using Firstapp.Entities;
using Firstapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Firstapp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index(ViewModel Currencies)
        {
            return View(Currencies);
        }

        [HttpPost]
        public async Task<ActionResult> CurrenciesButton(TimeModel model)
        {
            using HttpClient httpClient = new HttpClient();
            //ViewModel viewModel = new ViewModel();
            string dateCurrencies = model.DateTimeCurrencies.Year.ToString() + "-" + model.DateTimeCurrencies.Month.ToString() + "-" + model.DateTimeCurrencies.Day.ToString();
            var response = await httpClient.GetAsync($"https://api.nbrb.by/exrates/rates?ondate={dateCurrencies}&periodicity=0");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<CurrencyResponse>? cont = JsonConvert.DeserializeObject<List<CurrencyResponse>>(content);
                //viewModel.ListCurrency = cont;
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
                    //ViewBag.Message = "Download data base";
                }
                else ViewBag.Message = "В базе уже есть данные на эту дату";//ViewBag.Message = "Data with this date is already in the database";
                //return View("Index", viewModel);
            }
            return View("Index");
        }
        
        [HttpPost]
        public async Task<ActionResult> CurrencyButton(TimeModel model)
        {
            using HttpClient httpClient = new HttpClient();
            ViewModel viewModel = new ViewModel();
            string dateCurrency = model.DateTimeCurrency.Year.ToString() + "-" + model.DateTimeCurrency.Month.ToString() + "-" + model.DateTimeCurrency.Day.ToString();
            //var response = await httpClient.GetAsync($"https://api.nbrb.by/exrates/rates/{model.CurrencyCode}?ondate={dateCurrency}");
            var response = await httpClient.GetAsync($"https://api.nbrb.by/exrates/rates/{model.CodeName}?ondate={dateCurrency}&parammode=2");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                CurrencyResponse? cont = JsonConvert.DeserializeObject<CurrencyResponse>(content);
                viewModel.ListCurrency.Add(cont);
                return View("Index", viewModel);
            }
            return View("Index");
        }
    }
}
