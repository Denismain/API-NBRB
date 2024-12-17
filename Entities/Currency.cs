namespace Firstapp.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public string Abbreviation { get; set; }
        public int Scale { get; set; }
        public string Name { get; set; }
        public float OfficialRate { get; set; }
    }
}
