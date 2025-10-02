namespace HeroApi.Entities
{
    public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HeroName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }

        public ICollection<HeroSuperpower> HeroSuperpowers { get; set; } = new List<HeroSuperpower>();
    }
}
