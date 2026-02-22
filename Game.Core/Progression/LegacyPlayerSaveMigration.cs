using System.Collections.Generic;
using System.Text.Json;

namespace Game.Core.Progression;

public static class LegacyPlayerSaveMigration
{
    public static bool TryMigrateFromLegacyPlayerJson(string legacyJson, out ProgressState state)
    {
        state = new ProgressState();

        if (string.IsNullOrWhiteSpace(legacyJson))
        {
            return false;
        }

        try
        {
            using var doc = JsonDocument.Parse(legacyJson);
            var root = doc.RootElement;

            var levels = new List<int>();
            CollectHeroLevels(root, levels);

            if (levels.Count == 0)
            {
                return false;
            }

            state = new ProgressState
            {
                EncounterCounter = 1,
                HeroLevels = levels.ToArray()
            }.Normalize();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static void CollectHeroLevels(JsonElement root, List<int> levels)
    {
        if (root.TryGetProperty("Heroes", out var heroes) && heroes.ValueKind == JsonValueKind.Array)
        {
            foreach (var hero in heroes.EnumerateArray())
            {
                AddLevel(hero, levels);
            }
        }

        if (root.TryGetProperty("ActiveTeam", out var activeTeam) && activeTeam.ValueKind == JsonValueKind.Object)
        {
            AddTeamLineLevels(activeTeam, "FirstLine", levels);
            AddTeamLineLevels(activeTeam, "SecondLine", levels);
        }

        if (levels.Count == 0 && root.TryGetProperty("Mercenaries", out var mercs) && mercs.ValueKind == JsonValueKind.Array)
        {
            foreach (var hero in mercs.EnumerateArray())
            {
                AddLevel(hero, levels);
            }
        }
    }

    private static void AddTeamLineLevels(JsonElement team, string lineName, List<int> levels)
    {
        if (!team.TryGetProperty(lineName, out var line) || line.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        foreach (var hero in line.EnumerateArray())
        {
            AddLevel(hero, levels);
        }
    }

    private static void AddLevel(JsonElement hero, List<int> levels)
    {
        if (hero.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (!hero.TryGetProperty("Level", out var levelElement) || levelElement.ValueKind != JsonValueKind.Number)
        {
            return;
        }

        if (levelElement.TryGetInt32(out var level))
        {
            levels.Add(level < 1 ? 1 : level);
        }
    }
}
