using System.Net.Mime;
using System.Security.Cryptography;

namespace OCE_301025
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player01 = new Player("Horst");
            Enemy enemy01 = new Enemy("Torsten");

            /* if statement --> if player & enemy hp > 0 fight again +
            find a way to make speed meaningful --> attack order or miss mechanic
             make a UI if the game isnt auto battle, or make a random chance for using different abilitie
            split the actions of the fighting enemies? - make special monster attacks inside the class?
             while schleife -- solange die bedinung true ist wird der code ausgeführt. 
            for schleife -- wiederhole den code so oft wie angegeben /variable)
            do-while schleife -- while schleife aber macht erst prüft danach.
            for each scbleife -- funktioniert in einer liste, wiederholt den code für jedes element in der liste.
            do a fire emblem type battle system

            */
            do  
            {
                player01.Attack(enemy01);
                enemy01.Attack(player01);
            } while (player01.Gethp() > 0 && enemy01.Gethp() > 0);
            

            if (player01.Gethp() < 0 || enemy01.Gethp() < 0)
            {
                Console.Write("He dead lol");
                
                //close programm - maybe wait a second or two
            }
            
            
            // Spells und Potions einbauen. Optional mit Effekten.

            // Tobi-Task: Denk über die Semantik von _dmg, dmg und Damage nach !!!
        }
    }
}
