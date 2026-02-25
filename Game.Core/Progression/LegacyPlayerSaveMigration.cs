using System;
using System.Collections.Generic;
using System.Text.Json;
using Game.Core.Combat;

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

            var roster = CollectHeroRoster(root);
            if (roster.Count == 0)
            {
                return false;
            }

            var snapshot = new ProgressState
            {
                EncounterCounter = 1,
                Gold = ExtractGold(root),
                HeroRoster = roster.ToArray(),
                Inventory = CollectItems(root, "Storage"),
                Shop = CollectItems(root, "Shop")
            };

            state = snapshot.Normalize();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static int ExtractGold(JsonElement root)
    {
        if (!root.TryGetProperty("Gold", out var goldElement) || !goldElement.TryGetInt32(out var gold))
        {
            return 0;
        }

        return Math.Max(0, gold);
    }

    private static InventoryItemSnapshot[] CollectItems(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var array) || array.ValueKind != JsonValueKind.Array)
        {
            return Array.Empty<InventoryItemSnapshot>();
        }

        var snapshots = new List<InventoryItemSnapshot>();
        foreach (var entry in array.EnumerateArray())
        {
            var snapshot = new InventoryItemSnapshot
            {
                Name = entry.TryGetProperty("Name", out var nameProperty) ? nameProperty.GetString() ?? string.Empty : string.Empty,
                Effects = ExtractEffects(entry),
                BuffName = ExtractBuffName(entry)
            };

            snapshots.Add(snapshot);
        }

        return snapshots.ToArray();
    }

    private static Dictionary<Characteristic, int> ExtractEffects(JsonElement entry)
    {
        var result = new Dictionary<Characteristic, int>();

        if (!entry.TryGetProperty("Actions", out var actions) || actions.ValueKind != JsonValueKind.Array)
        {
            return result;
        }

        foreach (var action in actions.EnumerateArray())
        {
            if (!action.TryGetProperty("Item1", out var charElement) || !charElement.TryGetInt32(out var charIndex))
            {
                continue;
            }

            if (!Enum.IsDefined(typeof(Characteristic), charIndex))
            {
                continue;
            }

            if (!action.TryGetProperty("Item2", out var valueElement) || !valueElement.TryGetInt32(out var effectValue))
            {
                continue;
            }

            result[(Characteristic)charIndex] = effectValue;
        }

        return result;
    }

    private static string ExtractBuffName(JsonElement entry)
    {
        if (!entry.TryGetProperty("Buff", out var buff) || buff.ValueKind != JsonValueKind.Object)
        {
            return string.Empty;
        }

        if (!buff.TryGetProperty("Name", out var nameProperty))
        {
            return string.Empty;
        }

        return nameProperty.GetString() ?? string.Empty;
    }

    private static List<HeroProgressInfo> CollectHeroRoster(JsonElement root)
    {
        var roster = new List<HeroProgressInfo>();
        var seenKeys = new HashSet<string>();
        var slot = 0;

        AddTeamLine(root, roster, seenKeys, ref slot, "ActiveTeam", "FirstLine");
        AddTeamLine(root, roster, seenKeys, ref slot, "ActiveTeam", "SecondLine");
        AppendList(root, roster, seenKeys, ref slot, "Heroes");
        AppendList(root, roster, seenKeys, ref slot, "Mercenaries");

        return roster;
    }

    private static void AddTeamLine(JsonElement root, List<HeroProgressInfo> roster, HashSet<string> seenKeys,
        ref int slot, string teamProperty, string lineProperty)
    {
        if (!root.TryGetProperty(teamProperty, out var team) || team.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (!team.TryGetProperty(lineProperty, out var line) || line.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        foreach (var hero in line.EnumerateArray())
        {
            var info = ParseHero(hero, true, slot);
            var key = $"{info.Name}:{info.Level}:{info.Specialization}";
            if (seenKeys.Add(key))
            {
                roster.Add(info);
                slot++;
            }
        }
    }

    private static void AppendList(JsonElement root, List<HeroProgressInfo> roster, HashSet<string> seenKeys,
        ref int slot, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var list) || list.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        foreach (var heroElement in list.EnumerateArray())
        {
            var info = ParseHero(heroElement, false, slot);
            var key = $"{info.Name}:{info.Level}:{info.Specialization}";
            if (seenKeys.Add(key))
            {
                roster.Add(info);
                slot++;
            }
        }
    }

    private static HeroProgressInfo ParseHero(JsonElement hero, bool isActive, int slotIndex)
    {
        var name = hero.TryGetProperty("Name", out var nameProperty)
            ? nameProperty.GetString() ?? "Hero"
            : "Hero";

        var level = 1;
        if (hero.TryGetProperty("Level", out var levelProperty) && levelProperty.TryGetInt32(out var parsedLevel))
        {
            level = parsedLevel;
        }

        var specialization = Specialization.Wizard;
        if (hero.TryGetProperty("Specialization", out var specProperty) &&
            specProperty.TryGetInt32(out var parsedSpec) &&
            Enum.IsDefined(typeof(Specialization), parsedSpec))
        {
            specialization = (Specialization)parsedSpec;
        }

        return new HeroProgressInfo
        {
            Name = name,
            Level = level,
            Specialization = specialization,
            IsActive = isActive,
            SlotIndex = slotIndex
        };
    }
}
