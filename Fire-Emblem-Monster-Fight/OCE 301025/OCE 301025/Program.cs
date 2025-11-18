using System;

namespace OCE_301025
{
    internal class Program
    {
        /*
         * Ausführlicher Plan in Pseudocode:
         *
         * 1) Definiere Datentypen:
         *    - enum GameSpeedMode { Fast, Slow }
         *    - enum HitMode { Hitrate, GuaranteedHit }
         *    - class Combatant:
         *        - Felder: Name, MaxHp, CurrentHp, Attack, Defense, Speed, Dexterity, Luck
         *        - Methode Gethp() -> gibt CurrentHp zurück
         *        - Methode ResetHp() -> setzt CurrentHp = MaxHp
         *        - Methode IsAlive() -> CurrentHp > 0
         *        - Methode TakeDamage(int dmg) -> CurrentHp -= dmg
         *        - Methode Attack(Combatant target, HitMode hitMode, Random rng) -> berechnet Treffer, Crit, Schaden; ruft target.TakeDamage()
         *
         * 2) Setup:
         *    - Erzeuge einen Spieler und einen Gegner (feste Werte oder zufällig zwischen 10-30)
         *    - HP-Wert wird verdoppelt wie gewünscht (MaxHp = baseHp * 2)
         *
         * 3) Spielmodi:
         *    - Geschwindigkeit (Fast/Slow) steuert kurze Pausen und Anzeige-Level
         *    - Treffer-Modus: Hitrate (berechne Trefferchance) oder GuaranteedHit (immer treffen)
         *
         * 4) Kampfrunde (Haupt-Gameloop):
         *    - Solange beide lebendig:
         *        - Bestimme Angriffsreihenfolge nach Speed (bei Gleichstand: Spieler zuerst)
         *        - Für jeden Angreifer:
         *            - Bestimme Anzahl Attacken: wenn Attacker.Speed >= Defender.Speed + 5 => 2 Angriffe
         *            - Für jede Attacke:
         *                - Wenn HitMode == GuaranteedHit => Treffer
         *                - Sonst berechne hitChance = Baseline + (attacker.Dex - defender.Dex) * Faktor; clamp zwischen 5 und 95
         *                - Würfle Random 1-100, vergleiche
         *                - Kritisch: Würfel gegen Luck (z.B. luck %)
         *                - Schaden = Max(1, attacker.Attack - defender.Defense + Varianz)
         *                - Wenn kritisch => Schaden *= 3
         *                - Wende Schaden auf Ziel an und zeige Ergebnis
         *            - Wenn Ziel tot => breche Runden ab
         *    - Nach Rundenende: Sieger anzeigen, frage nach Neustart
         *
         * 5) Utility:
         *    - Funktionen zur Ausgabe, Pausen nur wenn Slow-Mode
         */

        enum GameSpeedMode { Fast, Slow }
        enum HitMode { Hitrate, GuaranteedHit }

        class Enemy
        {
            public string Name { get; }
            public int MaxHp { get; }
            public int CurrentHp { get; private set; }
            public int Attack { get; }
            public int Defense { get; }
            public int Speed { get; }
            public int Dexterity { get; }
            public int Luck { get; }

            public Enemy(string name, int baseHp, int attack, int defense, int speed, int dex, int luck)
            {
                Name = name;
                MaxHp = Math.Max(1, baseHp);
                CurrentHp = MaxHp;
                Attack = attack;
                Defense = defense;
                Speed = speed;
                Dexterity = dex;
                Luck = luck;
            }

            public int Gethp() => CurrentHp;
            public bool IsAlive() => CurrentHp > 0;
            public void ResetHp() => CurrentHp = MaxHp;
            public void TakeDamage(int dmg) => CurrentHp = Math.Max(0, CurrentHp - dmg);

            // Führt eine einzelne Attacke gegen target durch, return true wenn ded

            public bool AttackTarget(Enemy target, HitMode hitMode, Random rng, GameSpeedMode speedMode, int weaponAccuracy = 80)
            {
                bool hit;
                if (hitMode == HitMode.GuaranteedHit)
                {
                    hit = true;
                }
                else
                {
                    // Konsistente Formel: Hit = attacker.Dexterity*2 + weaponAccuracy - defender.Avoid (avoid = speed)
                    int enemyAvoid = target.Speed;
                    int hitChance = Math.Clamp(this.Dexterity * 2 + weaponAccuracy - enemyAvoid, 0, 100);
                    int roll = rng.Next(1, 101); // korrekt: 1..100
                    Console.WriteLine($"{Name} versucht zu treffen: HitChance={hitChance}% | Roll={roll}");
                    hit = roll <= hitChance;
                    Console.WriteLine(hit ? "Treffer!" : "Verfehlt!");
                }

                if (!hit)
                {
                    if (speedMode == GameSpeedMode.Slow) System.Threading.Thread.Sleep(600);
                    return false;
                }

                int critChance = Math.Clamp(this.Luck / 2, 0, 100);
                int critRoll = rng.Next(1, 101);
                bool critical = critRoll <= critChance;
                Console.WriteLine($"CritChance={critChance}% | CritRoll={critRoll} -> {(critical ? "Crit!" : "Kein Crit")}");

                int raw = Attack - target.Defense;
                int damage = Math.Max(1, raw);
                if (critical) damage *= 3;

                target.TakeDamage(damage);
                Console.WriteLine($"{Name} trifft {target.Name} für {damage} Schaden{(critical ? " (Critisch!)" : "")}. \n {target.Name} HP: {target.CurrentHp}/{target.MaxHp}");
                if (speedMode == GameSpeedMode.Slow) System.Threading.Thread.Sleep(500);

                return !target.IsAlive();
            }
        }

        static void Main(string[] args)
        {
            var rng = new Random();

            // Interaktive Eingabe für Spieler- und Gegner-Stats (ersetzt die vorherige Zufallserzeugung)
            Console.WriteLine("Erstelle Spieler-Charakter. Leere Eingabe verwendet den vorgeschlagenen Wert.");

            int ReadStat(string label, int min, int max, int def)
            {
                while (true)
                {
                    Console.Write($"{label} ({min}-{max}, Vorschlag {def}): ");
                    string? s = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(s)) return def;
                    if (int.TryParse(s, out int v))
                    {
                        if (v < min) v = min;
                        if (v > max) v = max;
                        return v;
                    }
                    Console.WriteLine("Ungültige Eingabe — bitte Zahl eingeben.");
                }
            }

            Console.Write("Name des Spielers (Standard: Milky): ");
            string? playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName)) playerName = "Milky";

            int pBaseHp = ReadStat("Basis-HP", 10, 100, rng.Next(10, 101));
            int pAtk = ReadStat("Attack", 1, 30, rng.Next(10, 31));
            int pDef = ReadStat("Defense", 0, 30, rng.Next(5, 31));
            int pSpd = ReadStat("Speed", 0, 30, rng.Next(5, 31));
            int pDex = ReadStat("Dexterity", 0, 30, rng.Next(1, 31));
            int pLuck = ReadStat("Luck", 0, 30, rng.Next(1, 31));

            Enemy player = new Enemy(playerName, baseHp: pBaseHp, attack: pAtk, defense: pDef, speed: pSpd, dex: pDex, luck: pLuck);

            Console.WriteLine();
            Console.WriteLine("Gegner erstellen: 1) Zufall  2) Aus Monster-Bibliothek  3) Manuell");
            Console.Write("Auswahl (1/2/3, Standard 2): ");
            string? choice = Console.ReadLine()?.Trim() ?? "2";

            Enemy enemy;
            var templates = MonsterLibrary.GetAll();

            if (choice == "1")
            {
                // vollständig zufällig
                enemy = new Enemy("Monster", baseHp: rng.Next(10, 31), attack: rng.Next(10, 31), defense: rng.Next(5, 21), speed: rng.Next(5, 30), dex: rng.Next(1, 20), luck: rng.Next(1, 30));
                Console.WriteLine("Stats zufällig erzeugt.");
            }
            else if (choice == "3")
            {
                // Manuelle Eingabe (wie zuvor)
                Console.Write("Name des Gegners (Standard: Goblin): ");
                string? enName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(enName)) enName = "Goblin";

                int eBaseHp = ReadStat("Basis-HP", 10, 100, rng.Next(10, 101));
                int eAtk = ReadStat("Attack", 1, 30, rng.Next(10, 31));
                int eDef = ReadStat("Defense", 0, 30, rng.Next(5, 31));
                int eSpd = ReadStat("Speed", 0, 30, rng.Next(5, 31));
                int eDex = ReadStat("Dexterity", 0, 30, rng.Next(1, 31));
                int eLuck = ReadStat("Luck", 0, 30, rng.Next(1, 31));

                enemy = new Enemy(enName, baseHp: eBaseHp, attack: eAtk, defense: eDef, speed: eSpd, dex: eDex, luck: eLuck);
            }
            else
            {
                // Auswahl aus MonsterLibrary (Standard)
                Console.WriteLine("Verfügbare Monstervorlagen:");
                for (int i = 0; i < templates.Count; i++)
                {
                    var t = templates[i];
                    Console.WriteLine($"{i + 1}) {t.Name}  (HPMod:{t.HpModifier}, ATKMod:{t.AttackModifier}, DEFMod:{t.DefenseModifier}, SPDMod:{t.SpeedModifier}, DEXMod:{t.DexterityModifier}, LCKMod:{t.LuckModifier})");
                }
                Console.Write("Wähle Nummer (ENTER = zufällig aus Bibliothek): ");
                string? sel = Console.ReadLine()?.Trim();

                MonsterTemplate chosen;
                if (!string.IsNullOrEmpty(sel) && int.TryParse(sel, out int idx) && idx >= 1 && idx <= templates.Count)
                {
                    chosen = templates[idx - 1];
                }
                else
                {
                    chosen = templates[rng.Next(templates.Count)];
                    Console.WriteLine($"Zufällig gewählt: {chosen.Name}");
                }

                // Erzeuge Basiswerte und wende Template-Modifier an (Clamp auf sinnvolle Bereiche)
                int baseHp = Math.Clamp(rng.Next(50, 91) + chosen.HpModifier, 1, 200);
                int atk = Math.Clamp(rng.Next(10, 21) + chosen.AttackModifier, 0, 99);
                int def = Math.Clamp(rng.Next(5, 11) + chosen.DefenseModifier, 0, 99);
                int spd = Math.Clamp(rng.Next(5, 10) + chosen.SpeedModifier, 0, 99);
                int dex = Math.Clamp(rng.Next(1, 10) + chosen.DexterityModifier, 0, 99);
                int luck = Math.Clamp(rng.Next(1, 30) + chosen.LuckModifier, 0, 100);

                enemy = new Enemy(chosen.Name, baseHp: baseHp, attack: atk, defense: def, speed: spd, dex: dex, luck: luck);
                Console.WriteLine($"Gegner erstellt: {chosen.Name} (HP:{baseHp} ATK:{atk} DEF:{def} SPD:{spd} DEX:{dex} LUCK:{luck})");
            }

            Console.WriteLine("Einstellungen wählen:");
            Console.WriteLine("Wähle Spielgeschwindigkeit: 1) Fast  2) Slow");
            GameSpeedMode speedMode = Console.ReadLine() == "2" ? GameSpeedMode.Slow : GameSpeedMode.Fast;
            Console.WriteLine("Wähle Treffer-Modus: \n  1) Hitrate (die Monster haben eine bestimmte hit Chance) \n  2) GuaranteedHit (alle Angriffe Treffen)");
            HitMode hitMode = Console.ReadLine() == "2" ? HitMode.GuaranteedHit : HitMode.Hitrate;

            bool playAgain = true;
            while (playAgain)
            {
                player.ResetHp();
                enemy.ResetHp();

                Console.WriteLine();
                Console.WriteLine("== Konfiguration ==");
                PrintStats(player);
                PrintStats(enemy);
                Console.WriteLine("===================\n");
                if (speedMode == GameSpeedMode.Slow) System.Threading.Thread.Sleep(700);

                // Kampfloop
                while (player.IsAlive() && enemy.IsAlive())
                {
                    // Reihenfolge nach Speed (Verbleib: Spieler zuerst bei Gleichstand)
                    Enemy first = player, second = enemy;
                    if (enemy.Speed > player.Speed)
                    {
                        first = enemy;
                        second = player;
                    }

                    // Anzahl Angriffe: +1 wenn Speed >= other + 5
                    int attacksFirst = (first.Speed >= second.Speed + 5) ? 2 : 1;
                    for (int i = 0; i < attacksFirst && second.IsAlive(); i++)
                    {
                        bool died = ResolveSingleAttack(first, second, hitMode, rng, speedMode);
                        if (died) break;
                    }
                    if (!second.IsAlive()) break;

                    int attacksSecond = (second.Speed >= first.Speed + 5) ? 2 : 1;
                    for (int i = 0; i < attacksSecond && first.IsAlive(); i++)
                    {
                        bool died = ResolveSingleAttack(second, first, hitMode, rng, speedMode);
                        if (died) break;
                    }
                }

                // Ergebnis
                Console.WriteLine();
                if (player.IsAlive() && !enemy.IsAlive())
                {
                    Console.WriteLine("Player gewinnt!");
                }
                else if (!player.IsAlive() && enemy.IsAlive())
                {
                    Console.WriteLine("Enemy gewinnt!");
                }
                else
                    {
                        Console.WriteLine("Beide sind gefallen!");
                    }

                    Console.WriteLine("Nochmal spielen? (j/n)");
                    string input = Console.ReadLine()?.Trim().ToLower() ?? "n";
                    playAgain = input == "j" || input == "y";
                    if (playAgain)
                    {
                        // Option: neue Gegner/Stats zufällig erzeugen oder die gleichen verwenden
                        enemy = new Enemy("Monster", baseHp: rng.Next(10, 31), attack: rng.Next(10, 31), defense: rng.Next(5, 21), speed: rng.Next(5, 30), dex: rng.Next(1, 20), luck: rng.Next(1, 30));
                        // Spieler bleibt gleich, HP wird zurückgesetzt
                    }
                }

                Console.WriteLine("Programm beendet. Drücke eine Taste.");
                Console.ReadKey();
            }

            static void PrintStats(Enemy c)
            {
                Console.WriteLine($"{c.Name} | HP: {c.MaxHp} | ATK: {c.Attack} | DEF: {c.Defense} | SPD: {c.Speed} | DEX: {c.Dexterity} | LUCK: {c.Luck}");
            }

            // Kampfloop (ersetzt die bisherige while-Schleife) — nutzt Formeln aus Entity.cs:
            // HitRate = dexterity*2 + weaponAccuracy - enemyAvoid (enemyAvoid = speed*2)
            // CritChance = luck / 2
            static int CalculateHitChance(int attackerDex, int weaponAccuracy, int defenderSpeed)
            {
                int enemyAvoid = defenderSpeed * 2;
                int calculated = attackerDex * 2 + weaponAccuracy - enemyAvoid;
                return Math.Clamp(calculated, 0, 100);
            }

            static bool ResolveSingleAttack(Enemy attacker, Enemy defender, HitMode hitMode, Random rng, GameSpeedMode speedMode, int weaponAccuracy = 80)
            {
                // Treffer prüfen (Anzeige von HitChance und tatsächlichem Würfelwurf)
                bool hit;
                if (hitMode == HitMode.GuaranteedHit)
                {
                    hit = true;
                }
                else
                {
                    int hitChance = CalculateHitChance(attacker.Dexterity, weaponAccuracy, defender.Speed);
                    int roll = rng.Next(1, 101); // Würfel 1..100
                    Console.WriteLine($"{attacker.Name} versucht zu treffen: HitChance={hitChance}% | Roll={roll}");
                    hit = roll <= hitChance;
                    Console.WriteLine(hit ? "Treffer!" : "Verfehlt!");
                }

                if (!hit)
                {
                    if (speedMode == GameSpeedMode.Slow) System.Threading.Thread.Sleep(600);
                    return false;
                }

                // Kritische Chance: luck / 2 (Anzeige optional)
                int critChance = Math.Clamp(attacker.Luck / 2, 0, 100);
                int critRoll = rng.Next(1, 101);
                bool critical = critRoll <= critChance;
                Console.WriteLine($"CritChance={critChance}% | CritRoll={critRoll} -> {(critical ? "Crit!" : "Kein Crit")}");

                // Schaden: attack - defense (mind. 1). Bei Crit x3
                int rawDamage = attacker.Attack - defender.Defense;
                int damage = Math.Max(1, rawDamage);
                if (critical) damage *= 3;

                // Anwenden
                defender.TakeDamage(damage);
                Console.WriteLine($"{attacker.Name} trifft {defender.Name} für {damage} Schaden{(critical ? " (Crit!)" : "")}. {defender.Name} HP: {defender.CurrentHp}/{defender.MaxHp}");
                if (speedMode == GameSpeedMode.Slow) System.Threading.Thread.Sleep(500);

                return !defender.IsAlive();
            }
        }
    }
