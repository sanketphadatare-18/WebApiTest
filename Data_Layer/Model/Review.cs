namespace Web_Api_Core_.Model
{
    public class Review
    {
        public int id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public Reviewer Reviewer { get; set; }

        public Pokemon Pokemon { get; set; }
    }
}
