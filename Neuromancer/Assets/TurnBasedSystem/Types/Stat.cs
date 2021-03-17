namespace TurnBasedSystem {
    public struct Stat {
        public int health { get; set;}
        public int energy { get; set;}
        public int speed  { get; set;} //stride,movement
        public int range  { get; set;} //How far they can attack
        public int attackdmg { get; set; } //How much damage they do
    }
}