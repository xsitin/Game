using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Game.Model
{
    public class
        Database //класс отвечающий за доступ к данным в проекте, к примеру, за получение необходимых для view картинок
    {
        public static Dictionary<Specialization,List<Skill>> BasicSkills = new Dictionary<Specialization, List<Skill>>()
        {
            {Specialization.Archer,
                new List<Skill>()
                {
                    new Skill(20, new[] {(Characteristics.Health, -45)}, SkillRange.Single, "Power Shoot", null),
                    new Skill(60, new[] {(Characteristics.Health, -30)}, SkillRange.Enemies, "Power Multi Shoot", null),
                }
            },
            {Specialization.Wizard,
                new List<Skill>()
                {
                    new Skill(20, new[] {(Characteristics.Health, -30)}, SkillRange.Single, "Fire Boll", 
                        new Buff(2,"Burning",(Characteristics.Initiative, -10))),
                    new Skill(60, new[] {(Characteristics.Health, -30)}, SkillRange.Enemies, "Magical Arrows", null),
                }
            },
            {Specialization.Warrior,
                new List<Skill>()
                {
                    new Skill(20, new[] {(Characteristics.Health, -20)}, SkillRange.Single, "OraOraOraOra", 
                        new Buff(2,"Stan",(Characteristics.Initiative, -100))),
                    new Skill(50, new[] {(Characteristics.Health, -40)}, SkillRange.Enemies, "OraOraOraOra OnTeam", null),
                }
            }
            
        };
        public static string GetName()
        {
            return "1";
        }
        public static void SaveGame(Player player)
        {
            var opt = new JsonSerializerOptions();
            opt.Converters.Add(
                new DictionaryTKeyEnumTValueConverter().CreateConverter(typeof(Dictionary<Characteristics, int>),
                    new JsonSerializerOptions()));
            var serialized = JsonSerializer.Serialize(player, opt);
            Console.WriteLine(serialized);
            var stream = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), player.PlayerName));
            stream.WriteLine(serialized);
            stream.Close();
        }

        public static Player LoadGame(string playerName)
        {
            var opt = new JsonSerializerOptions();
            opt.Converters.Add(
                new DictionaryTKeyEnumTValueConverter().CreateConverter(typeof(Dictionary<Characteristics, int>),
                    new JsonSerializerOptions()));
            var text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), playerName));
            return JsonSerializer.Deserialize<Player>(text, opt);
        }
    }
}