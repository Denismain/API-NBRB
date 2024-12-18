using System.ComponentModel.DataAnnotations;

namespace Firstapp.Models
{
    public class ViewModel
    {
        public List<CurrencyResponse> ListCurrency { get; set; } = [];
        public List<AllCurrencies> ListAllCurrencies { get; set; } = [];

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DateTimeCurrencies { get; set; } = DateTime.Today;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DateTimeCurrency { get; set; } = DateTime.Today;
        public string CodeName { get; set; }
    }
}
