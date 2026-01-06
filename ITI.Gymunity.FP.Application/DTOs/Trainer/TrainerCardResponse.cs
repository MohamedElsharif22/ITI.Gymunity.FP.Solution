namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
    public class TrainerCardDto
    {

        public string Id { get; set; }
        public string FullName { get; set; }
        public string Handle { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string Bio { get; set; }
        public bool IsVerified { get; set; }
        public decimal RatingAverage { get; set; }
        public int TotalReviews { get; set; }
        public int TotalClients { get; set; }
        public int YearsExperience { get; set; }
        public List<string> Specializations { get; set; }
        public decimal? StartingPrice { get; set; }
        public string Currency { get; set; }
        public bool HasActiveSubscription { get; set; }
    }
}
