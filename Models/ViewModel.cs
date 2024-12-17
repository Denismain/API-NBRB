using System.ComponentModel.DataAnnotations;

namespace Firstapp.Models
{
    public class ViewModel
    {
        public List<CurrencyResponse> ListCurrency { get; set; } = [];

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DateTimeCurrencies { get; set; } = DateTime.Today;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DateTimeCurrency { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Введите буквенный код валюты, к примеру USD, RUB, UAH")]
        public string CodeName { get; set; }
    }
}
