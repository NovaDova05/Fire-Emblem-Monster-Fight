using System;
using System.Collections.Generic;
using System.Linq;

namespace OCE_301025
{
    // Einfacher Weapon-Typ
    internal record Weapon(string Name, int Accuracy, int Power, int CriticalBonus);

    // Zentrales Repository mit verfügbaren Waffen
    internal static class WeaponRepository
    {
        private static readonly List<Weapon> weapons = new()
        {
            new Weapon("Goblin Dagger", 80, 6 , 1),
            new Weapon("Troll Claw", 70, 7, 1),
            new Weapon("Orc Waraxe", 65, 13, 1),
            new Weapon("Vampire Fangs", 90, 8, 1),
            new Weapon("Gambling Flame", 80, 9, 10),
            new Weapon("Golem Fist", 55, 20, 1)
        };

        public static IReadOnlyList<Weapon> GetAll() => weapons.AsReadOnly();

        public static Weapon? GetByName(string name) =>
            weapons.FirstOrDefault(w => string.Equals(w.Name, name, StringComparison.OrdinalIgnoreCase));

        public static bool TryGet(string name, out Weapon? weapon)
        {
            weapon = GetByName(name);
            return weapon is not null;
        }
    }
}
