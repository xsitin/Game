using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public static class Bot
    {
        public static void MakeAMove(Game game)
        {
            var current = (EnemyHero) game.CurrentCreature;
            var random = new Random();
            var skill = current.Skills[random.Next(0, current.Skills.Count - 1)];
            var targets = new List<BasicCreature>();
            switch (skill.Range)
            {
                case SkillRange.All:
                    targets.AddRange(game.Heroes.GetTeamList().Concat(game.Enemy.GetTeamList()));
                    break;
                case SkillRange.Enemies:
                    targets = game.Heroes.GetTeamList();
                    break;
                case SkillRange.Friendly:
                    targets = game.Enemy.GetTeamList();
                    break;
                case SkillRange.Single:
                    if (skill.Effect.Any(x => x.characteristic == Characteristics.Health && x.value < 0))
                        if (DateTime.Now.Minute % 2 == 0)
                            targets.Add(current.Position == Position.Melee
                                ? game.Heroes.FirstLine.OrderBy(x => x.Characteristics[Characteristics.Health]).First()
                                : game.Heroes.GetTeamList().OrderBy(x => x.Characteristics[Characteristics.Health])
                                    .First());
                        else
                            targets.Add(game.Heroes.GetTeamList()[random.Next(0, game.Heroes.GetTeamList().Count - 1)]);
                    //todo add using heal
                    break;
            }

            current.UseSkill(skill, targets.ToArray());
            game.NextStep();
        }
    }
}