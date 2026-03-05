namespace PracticeProjectWebAPI.Models.DTO
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public double Rating { get; set; }
    }
}
