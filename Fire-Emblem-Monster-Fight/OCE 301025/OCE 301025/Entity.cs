using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        private int dexterity;
        private int luck;
        public int HitRate;
        public int CritRate;

        private int healPower;
        private string EntityName;
        private Random random;

        public int Hp { get { return currentHp; } }
             
        public bool IsDead { get { return currentHp <= 0; } }

        public Entity(int maxHp, int attackPower, int healPower, string EntityName, int dexterity, int luck, int speed, int HitRate, int CritRate)
        {
            this.maxHp = maxHp;
            this.currentHp = maxHp;
            this.attackPower = attackPower;
            this.dexterity = dexterity;
            this.luck = luck;
            this.speed = speed;
            this.healPower = healPower;
            this.EntityName = EntityName;
            this.random = new Random();
            this.HitRate = HitRate;
            this.CritRate = CritRate;
        }
        protected string name;

        public Entity(string name) 
        { 
            this.name = name;
        }

        /*public void Hit(Entity EntitToAttack)
            int HitRate = (int)(skill) * 2 + (EntityWeapon) + (other abilities) - (EnemyAvoid) ; */

        //int Avoid = (int)(speed) * 2 + (other abilities);

       
        public void Attack(Entity EntityToAttack)
        {
            // generate random number between 1-100
            // if (Entity is getting Hit --> The Roll is equal or lower than the HitRate);
            //int Critical Rate = (int)(luck) / 2 + (other abilities) + 1 --> if hit roll is eqal or lower than the Critical Rate its a crit;
            // else Misses and nothing happensbe 
            {
                int Damage = (int)(attackPower);
                EntityToAttack.TakeDamage(Damage);
                //if Critical Hit --> Damage = Damage * 3;
                Console.WriteLine($" (EntityName), +  attacks" + "(EntityToAttack.unitName)" + "and deals"  + "Damage" + "damage!");

            }

        }

        public void TakeDamage(int DamageTaken)
        {
            currentHp -= DamageTaken - defense;
            // if defence is higher than attack power, deal 1 damage

            if (IsDead)
                Console.WriteLine($" (EntityName), + has got their ass kicked!");
        }
    

     public int Gethp()
     {
         return maxHp;

     }
/// <summary>
/// Berechnet die HitRate nach der Formel:
/// HitRate = dexterity * 2 + weaponAccuracy - enemyAvoid
/// Wert wird auf 0..100 geklippt und in das Feld HitRate geschrieben.
/// </summary>
public int CalculateHitRate(int Dexterity, int weaponAccuracy, int enemyAvoid)
{
    int calculated = Dexterity * 2 + weaponAccuracy - enemyAvoid;
    calculated = Math.Clamp(calculated, 0, 100);
    this.HitRate = calculated;
    return calculated;
}
/// <summary>
/// Berechnet die Ausweichrate: Avoid = speed * 2
/// Steht für alle abgeleiteten Klassen (z.B. Enemy) zur Verfügung.
/// </summary>
public virtual int enemyAvoid => speed * 2;
}
}
/*while schleife-- solange die bedinung true ist wird der code ausgeführt. 
            for schleife -- wiederhole den code so oft wie angegeben /variable)
            do-while schleife -- while schleife aber macht erst prüft danach.
            for each scbleife -- funktioniert in einer liste, wiederholt den code für jedes element in der liste.
            do a fire emblem type battle system */