namespace HeroApi.Entities
{
    public class HeroSuperpower
    {
        public int HeroId { get; set; }
        public int SuperpowerId { get; set; }
        public Hero Hero { get; set; } = null!;
        public Superpower Superpower { get; set; } = null!;
    }
}
