using System;

namespace DiceRoller.Models
{
    public class Dice : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Name { get; set; }
        public string Equation { get; set; }
        public string Category { get; set; }

        public static Dice Create(string name, string equation, string category) =>
            new Dice
            {
                Name = name,
                Equation = equation,
                Category = category,
            };
    }
}
