namespace LibraryWebAPI.Dto.Book
{
    public class CreateBookDto
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }
    }
}
