namespace HeroApi.Entities
{
    public class Superpower
    {
        public int Id { get; set; }
        public string SuperPower { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public ICollection<HeroSuperpower> HeroSuperpowers { get; set; } = new List<HeroSuperpower>();
    }
}
