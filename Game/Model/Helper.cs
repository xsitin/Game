using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

namespace Game.Model
{
    public class Helper //класс отвечающий за доступ к данным в проекте, к примеру, за получение необходимых для view картинок
    {
        public static readonly Dictionary<Specialization, Position> Transfer = new Dictionary<Specialization, Position>
        {
            
            {Specialization.Wizard, Position.Range},
            {Specialization.Warrior, Position.Melee},
            {Specialization.Archer, Position.Range}
        };

        public static readonly Dictionary<Specialization, Image> ImageTransfer = new Dictionary<Specialization, Image>
        {
            {Specialization.Wizard, Properties.Resources.Wizard},
            {Specialization.Warrior, Properties.Resources.Warrior},
            {Specialization.Archer, Properties.Resources.Archer}
        };
        
        public static readonly Dictionary<Specialization, Image> EnemyImageTransfer = new Dictionary<Specialization, Image>
        {
            {Specialization.Wizard, Properties.Resources.EnemyWizard},
            {Specialization.Warrior, Properties.Resources.EnemyWarrior},
            {Specialization.Archer, Properties.Resources.EnemyArcher}
        };
        public static readonly Dictionary<Specialization, Image> DeadImageTransfer = new Dictionary<Specialization, Image>
        {
            {Specialization.Wizard, Properties.Resources.DeadMag},
            {Specialization.Warrior, Properties.Resources.DeadWarrior},
            {Specialization.Archer, Properties.Resources.DeadArcher}
        };
        
        public static readonly Dictionary<Specialization, Image> DeadEnemyImageTransfer = new Dictionary<Specialization, Image>
        {
            {Specialization.Wizard, Properties.Resources.DeadEnemyMag},
            {Specialization.Warrior, Properties.Resources.DeadEnemyWarrior},
            {Specialization.Archer, Properties.Resources.DeadEnemyArcher}
        };

        public static Dictionary<Specialization, List<Skill>> BasicSkills = new Dictionary<Specialization, List<Skill>>
        {
            {
                Specialization.Archer,
                new List<Skill>
                {
                    new Skill(20, new[] {(Characteristics.Health, -45)}, SkillRange.Single, "Power Shoot", null),
                    new Skill(60, new[] {(Characteristics.Health, -30)}, SkillRange.Enemies, "Power Multi Shoot", null),
                    new Skill(0, new[] {(Characteristics.Health, -20)}, SkillRange.Single, "Broking arrow", 
                        new Buff(2, "Poison", (Characteristics.PhysicalProtection, -15))),
                    new Skill(0, new[] {(Characteristics.Health, -40)}, SkillRange.Single, "Hitting below the belt", null)
                }
            },
            {
                Specialization.Wizard,
                new List<Skill>
                {
                    new Skill(20, new[] {(Characteristics.Health, -30)}, SkillRange.Single, "Fire Boll",
                        new Buff(2, "Burning", (Characteristics.Initiative, -10))) {IsMagic = true},
                    new Skill(60, new[] {(Characteristics.Health, -30)}, SkillRange.Enemies, "Magical Arrows", null) {IsMagic = true},
                    new Skill(30, new[] {(Characteristics.Health, +30)}, SkillRange.Friendly, "Healing hands", null) {IsMagic = true},
                    new Skill(100, new[] {(Characteristics.Health, -90)}, SkillRange.All, "Armageddon", null) {IsMagic = true}
                }
            },
            {
                Specialization.Warrior,
                new List<Skill>
                {
                    new Skill(20, new[] {(Characteristics.Health, -20)}, SkillRange.Single, "OraOra",
                        new Buff(2, "Stan", (Characteristics.Initiative, -100))),
                    new Skill(30, new[] {(Characteristics.Health, -20)}, SkillRange.Enemies, "OraTeam", null),
                    new Skill(10, new (Characteristics characteristic, int value)[0], SkillRange.Single, "Close the shield", 
                        new Buff(3, "Shield", new []{(Characteristics.PhysicalProtection, +15), (Characteristics.MagicalProtection, +15)})),
                    new Skill(30, new[] {(Characteristics.Health, +10), (Characteristics.PhysicalDamage, +10), (Characteristics.Evasion, +5)},
                        SkillRange.Single, "Rage", null),
                }
            }
        };

        private static int previous;

        public static string GetName()
        {
            var Names = Properties.Resources.Names.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var r = new Random();
            var val = r.Next(0, Names.Length - 1);
            while (val == previous)
                val = r.Next(0, Names.Length - 1);
            previous = val;
            return Names[val];
        }

        public static void SaveGame(Player player)
        {
            var serialized = JsonConvert.SerializeObject(player);
            var way = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            way = Path.Combine(way, "Game");
            var sw = File.CreateText(Path.Combine(way,player.PlayerName));
            sw.Write(serialized);
            sw.Close();
        }

        public static Player LoadGame(string playerName)
        {
            var way = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            way = Path.Combine(way, "Game");
            way = Path.Combine(way, playerName);
            if (File.Exists(way))
            {
                var text = File.ReadAllText(way);
                return
                    JsonConvert.DeserializeObject<Player>(text);
            }

            return new Player();
        }
        //private string Save = @"{"Gold":100,"PlayerName":"Player1","Heroes":[{"Inventory":{"Heap":[],"Size":0},"Buffs":[],"StandardChars":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"UpgradePoints":0,"Exp":0,"Name":"Lyur","Characteristics":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"Level":1,"Location":0,"Specialization":1,"Skills":[{"Range":3,"IsMagic":false,"Name":"Base Hit","Level":1,"ManaCost":0,"Effect":[{"Item1":0,"Item2":-15}],"Buff":null}],"Position":0},{"Inventory":{"Heap":[],"Size":0},"Buffs":[],"StandardChars":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"UpgradePoints":0,"Exp":0,"Name":"Soks","Characteristics":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"Level":1,"Location":0,"Specialization":2,"Skills":[{"Range":3,"IsMagic":false,"Name":"Base Hit","Level":1,"ManaCost":0,"Effect":[{"Item1":0,"Item2":-15}],"Buff":null}],"Position":1}],"Mercenaries":[{"Inventory":{"Heap":[],"Size":0},"Buffs":[],"StandardChars":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"UpgradePoints":0,"Exp":0,"Name":"Lyur","Characteristics":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"Level":1,"Location":0,"Specialization":1,"Skills":[{"Range":3,"IsMagic":false,"Name":"Base Hit","Level":1,"ManaCost":0,"Effect":[{"Item1":0,"Item2":-15}],"Buff":null}],"Position":0},{"Inventory":{"Heap":[],"Size":0},"Buffs":[],"StandardChars":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"UpgradePoints":0,"Exp":0,"Name":"Soks","Characteristics":{"Health":200,"Mana":100,"Initiative":30,"PhysicalDamage":15,"PhysicalProtection":20,"Evasion":10,"MagicalProtection":10},"Level":1,"Location":0,"Specialization":2,"Skills":[{"Range":3,"IsMagic":false,"Name":"Base Hit","Level":1,"ManaCost":0,"Effect":[{"Item1":0,"Item2":-15}],"Buff":null}],"Position":1}],"Storage":[{"Actions":[{"Item1":0,"Item2":20}],"Buff":null,"Action":null,"Name":"heal"}],"Shop":[]}";
    }
}