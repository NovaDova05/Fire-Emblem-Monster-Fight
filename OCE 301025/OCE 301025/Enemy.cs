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
 * Critical Hit Range --> 100 - Luck +- skills