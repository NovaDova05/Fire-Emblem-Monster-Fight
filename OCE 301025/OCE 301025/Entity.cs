using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCE_301025
{
    internal abstract class Entity
    {
        private int currentHp;
        private int maxHp;
        private int attackPower;
        private int defense;
        private int speed;
        private int skill;
        private int luck;
      
        private int healPower;
        private string EntityName;
        private Random random;

        public int Hp { get { return currentHp; } }
        public string EntityName { get { return EntityName; } }
        public bool IsDead { get { return currentHp <= 0; } }

        public Unit(int maxHp, int attackPower, int healPower, string EntityName, int skill, int luck, int HitRate)
        {
            this.maxHp = maxHp;
            this.currentHp = maxHp;
            this.attackPower = attackPower;
            this.skill = skill;
            this.luck = luck;
            this.healPower = healPower;
            this.EntityName = EntityName;
            this.random = new Random();
            this.HitRate = HitRate;
        }
        protected string name;

        public Entity(string name) 
        { 
            this.name = name;
        }

        /*public void Hit(Entity EntitToAttack)
            int HitRate = (int)(skill) * 2 + (EntityWeapon) - (EnemyAvoid) ; */

        public void Attack(Entity EntityToAttack)
        {
            // if (Entity is getting Hit --> The Roll is equal or lower than the HitRate);
            {
                int Damage = (int)(attackPower);
                EntityToAttack.TakeDamage(Damage);
                Console.WriteLine(EntityName + " attacks " + EntityToAttack.unitName + " and deals " + Damage + " damage!");
            }

            }

        public void TakeDamage(int DamageTaken)
        {
            currentHp -= DamageTaken - defense;

            if (IsDead)
                Console.WriteLine(EntityName + " has got their ass kicked!");
        }
        public void Heal()
        {
            double rng = random.NextDouble();
            rng = rng / 2 + 0.75f;
            int heal = (int)(rng * healPower);
            currentHp = heal + currentHp > maxHp ? maxHp : currentHp + heal;
            Console.WriteLine(UnitName + " heals " + heal);
        }
    }

    /* public int Gethp()
     {
         return _HP;

     } */
}
}
