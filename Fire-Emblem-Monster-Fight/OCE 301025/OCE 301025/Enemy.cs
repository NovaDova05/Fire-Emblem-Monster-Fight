using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OCE_301025
{
    internal class Enemy : Entity
    {
        public Enemy(string name) : base(name) { }

    }

    // Monster-Templates mit Stat-Modifiern und Skill-Beschreibungen (in den Kommentaren).
    // Diese Templates sind als Referenz gedacht; das Erstellen konkreter Entity-Instanzen
    // erfolgt separat, sobald Entity Setzer/Builder verfügbar sind.

    internal sealed record MonsterTemplate(
        string Name,
        int HpModifier,
        int AttackModifier,
        int DefenseModifier,
        int SpeedModifier,
        int DexterityModifier,
        int LuckModifier,
        string[] Skills);

    internal static class MonsterLibrary
    {
        private static readonly List<MonsterTemplate> templates = new()
        {
            // Goblin
            // Skills:
            // - "Sneak Attack": Angriff wird zweimal geckeckt, wenn beide treffen macht er double damage.
            new MonsterTemplate(
                Name: "Goblin",
                HpModifier: -10,
                AttackModifier: 0,
                DefenseModifier: -1,
                SpeedModifier: +5,
                DexterityModifier: +2,
                LuckModifier: 0,
                Skills: new[] { "Sneaky Attack" }
            ),

            // Troll
            // Skills:
            // - "Regenerate": Heilt pro Runde einen kleinen Betrag an hp (3-10).

            new MonsterTemplate(
                Name: "Troll",
                HpModifier: +10,
                AttackModifier: +2,
                DefenseModifier: 0,
                SpeedModifier: 0,
                DexterityModifier: 0,
                LuckModifier: -10,

                Skills: new[] { "Regenerate" }
            ),

            // Orc
            // Skills:
            // - "Adrenaline Rush = 50% chance noch einmal anzugreifen.
            new MonsterTemplate(
                Name: "Orc",
                HpModifier: +4,
                AttackModifier: +5,
                DefenseModifier: 0,
                SpeedModifier: 0,
                DexterityModifier: 0,
                LuckModifier: 0,

                Skills: new[] { "Adrenaline Rush" }
            ),

            // Vampire
            // Skills:
            // - "Bloodsucker": Heilt einen Teil des verursachten Schadens (50%).
            new MonsterTemplate(
                Name: "Vampire",
                HpModifier: -10,
                AttackModifier: 0,
                DefenseModifier: 0,
                SpeedModifier: +2,
                DexterityModifier: +5,
                LuckModifier: 0,

                Skills: new[] { "Bloodsucker"}
            ),

            // Golem
            // Skills:
            // - "Stone Skin": Immunität gegen kritische Treffer.
            new MonsterTemplate(
                Name: "Golem",
                HpModifier: +5,
                AttackModifier: -5,
                DefenseModifier: +5,
                SpeedModifier: 0,
                DexterityModifier: 0,
                LuckModifier: 0,

                Skills: new[] { "Stone Skin" }
            ),

            // Gambler
            // Skills:
            // - "Lucky Bastard": +10% auf kritische Treffer, und der schaden ist vervierfacht bei einem kritischen Treffer.
            new MonsterTemplate(
                Name: "Gambler",
                HpModifier: -5,
                AttackModifier: 0,
                DefenseModifier: -2,
                SpeedModifier: +2,
                DexterityModifier: +3,
                LuckModifier: +5,
                Skills: new[] { "Lucky Bastard" }
            )
        };

        public static IReadOnlyList<MonsterTemplate> GetAll() => templates.AsReadOnly();

        public static MonsterTemplate? GetByName(string name) =>
            templates.FirstOrDefault(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));
    }
}

/* Monster and their skills
 * Goblin --| dexteritx related skills > dodgy or sneaky attacks
 * Troll --| Constitution related skills > classic regeneration
 * Orc  --| Strength based skrills > extra attack or maybe critical attack
 * Vampire --> heal half damage
 * 
 * 
 * Fire Emblem Combat
 * stats and each monster has their own "weapon" -- > same attack function different parameters
 *
 * attack = Skill * 2 + weapon hit - Enemy Avoid +- special skills
 * Avoid = Speed * 2
 * Random Number Generator to calculate if attack hits
 * Luck stat --> makes critical hits easier
 * Critical Hit Range -->  Luck /2  +- skills + 1 */
