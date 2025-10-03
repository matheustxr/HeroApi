namespace HeroApi.DTOs
{
    public class RequestHeroJson
    {
        public string Name { get; set; } = string.Empty;
        public string HeroName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<int> SuperpowerIds { get; set; } = new();
    }
}
