using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Firstapp.Models
{
    public class CurrencyResponse
    {
        [JsonProperty("Cur_ID")]
        public int CurID { get; set; }
        
        [JsonProperty("Date", ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTime Date { get; set; }
        
        [JsonProperty("Cur_Abbreviation")]
        public string CurAbbreviation { get; set; }
        
        [JsonProperty("Cur_Scale")]
        public int CurScale { get; set; }
        
        [JsonProperty("Cur_Name")]
        public string CurName { get; set; }
        
        [JsonProperty("Cur_OfficialRate")]
        public float CurOfficialRate { get; set; }
    }
}
