namespace BookManagement.Models
{
    public class BookDetails
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? PublishedYear { get; set; }
        public decimal? Price { get; set; }
        public string? Discount { get; set; }
        public decimal? FinalPrice { get; set; }
        public string? Ratings { get; set; }
        
    }
}
