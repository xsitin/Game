using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Game.Model
{
    public class
        Helper //класс отвечающий за доступ к данным в проекте, к примеру, за получение необходимых для view картинок
    {
        public static readonly Dictionary<Specialization, Position> Transfer = new Dictionary<Specialization, Position>
        {
            {Specialization.Wizard, Position.Range},
            {Specialization.Warrior, Position.Melee},
            {Specialization.Archer, Position.Range}
        };

        public static Dictionary<Specialization, List<Skill>> BasicSkills = new Dictionary<Specialization, List<Skill>>
        {
            {
                Specialization.Archer,
                new List<Skill>
                {
                    new Skill(20, new[] {(Characteristics.Health, -45)}, SkillRange.Single, "Power Shoot", null),
                    new Skill(60, new[] {(Characteristics.Health, -30)}, SkillRange.Enemies, "Power Multi Shoot", null)
                }
            },
            {
                Specialization.Wizard,
                new List<Skill>
                {
                    new Skill(20, new[] {(Characteristics.Health, -30)}, SkillRange.Single, "Fire Boll",
                        new Buff(2, "Burning", (Characteristics.Initiative, -10))),
                    new Skill(60, new[] {(Characteristics.Health, -30)}, SkillRange.Enemies, "Magical Arrows", null)
                }
            },
            {
                Specialization.Warrior,
                new List<Skill>
                {
                    new Skill(20, new[] {(Characteristics.Health, -20)}, SkillRange.Single, "OraOraOraOra",
                        new Buff(2, "Stan", (Characteristics.Initiative, -100))),
                    new Skill(50, new[] {(Characteristics.Health, -40)}, SkillRange.Enemies, "OraOraOraOra OnTeam",
                        null)
                }
            }
        };

        private static int previous;

        public static string GetName()
        {
            var w = Directory.GetParent(Path.GetFullPath(Assembly.GetExecutingAssembly().Location)).Parent.Parent.Parent
                .FullName + @"\Game\Properties\Names.txt";
            var Names = File.ReadAllLines(w);
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
            File.WriteAllText(Directory.GetParent(Path.GetFullPath(Assembly.GetExecutingAssembly().Location)).Parent
                .Parent.Parent
                .FullName + @"\Game\Saves", serialized);
        }

        public static Player LoadGame(string playerName)
        {
            var text = File.ReadAllText(Directory.GetParent(Path.GetFullPath(Assembly.GetExecutingAssembly().Location))
                .Parent.Parent.Parent
                .FullName + @"\Game\Saves");
            return JsonConvert.DeserializeObject<Player>(text);
        }
    }
}