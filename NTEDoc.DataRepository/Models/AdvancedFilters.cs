namespace NTEDoc.DataRepository.Models
{
    public class AdvancedFilters
    {
        public int[] Recorders { get; set; }
        public DateRange CreatedAtDate { get; set; } 
        public DateRange DocumentDate { get; set; } 
        public int Status { get; set; }
        public int DocumentType { get; set; }
        public AmountRange Amount { get; set; }
        public int DeliveryType { get; set; }
    }
}