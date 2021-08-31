

namespace AnotherDbTest
{
    class Book
    {
        public string TitleId { get; private set; }
        public string Title { get; private set; }
        public string Type { get; private set; }
        public string PubId { get; private set; }
        public decimal Price { get; private set; }
        public decimal Advance { get; private set; }

        public Book(string id, string bookTitle, string bookType, string publisherId, decimal bookPrice, decimal advanceQty)
        {
            TitleId = id;
            Title = bookTitle;
            Type = bookType;
            PubId = publisherId;
            Price = bookPrice;
            Advance = advanceQty;
        }
        public override string ToString()
            => $"{TitleId,10} {Title,-65}{Type,-10}{PubId,6}{Price,10}{Advance,12}";

    }
}
