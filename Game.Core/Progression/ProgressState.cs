using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Combat;

namespace Game.Core.Progression;

public sealed record ProgressState
{
    private static readonly string[] DefaultHeroNames = ["Alden", "Mira", "Tess"];
    private static readonly Specialization[] DefaultSpecializations =
        [Specialization.Wizard, Specialization.Warrior, Specialization.Archer];

    public int EncounterCounter { get; init; } = 1;
    public int Gold { get; init; }
    public int[] HeroLevels { get; init; } = DefaultHeroLevels;
    public HeroProgressInfo[] HeroRoster { get; init; } = Array.Empty<HeroProgressInfo>();
    public InventoryItemSnapshot[] Inventory { get; init; } = Array.Empty<InventoryItemSnapshot>();
    public InventoryItemSnapshot[] Shop { get; init; } = Array.Empty<InventoryItemSnapshot>();

    public static int[] DefaultHeroLevels => [1, 1, 1];

    public IReadOnlyList<int> GetHeroLevels()
    {
        if (HeroRoster.Length > 0)
        {
            return HeroRoster.Select(hero => Math.Max(1, hero.Level)).ToArray();
        }

        if (HeroLevels.Length > 0)
        {
            return HeroLevels.Select(level => Math.Max(1, level)).ToArray();
        }

        return DefaultHeroLevels;
    }

    public ProgressState Normalize()
    {
        var roster = (HeroRoster ?? Array.Empty<HeroProgressInfo>())
            .Select(NormalizeHero)
            .Where(info => info != null)
            .Cast<HeroProgressInfo>()
            .ToArray();

        if (roster.Length == 0)
        {
            roster = BuildDefaultRoster(HeroLevels);
        }

        var normalizedLevels = roster.Select(hero => Math.Max(1, hero.Level)).ToArray();

        return this with
        {
            EncounterCounter = Math.Max(1, EncounterCounter),
            Gold = Math.Max(0, Gold),
            HeroLevels = normalizedLevels.Length > 0 ? normalizedLevels : DefaultHeroLevels,
            HeroRoster = roster,
            Inventory = NormalizeSnapshots(Inventory),
            Shop = NormalizeSnapshots(Shop)
        };
    }

    private static HeroProgressInfo[] BuildDefaultRoster(int[] heroLevels)
    {
        var list = new List<HeroProgressInfo>();
        for (var i = 0; i < heroLevels.Length; i++)
        {
            list.Add(new HeroProgressInfo
            {
                Name = DefaultHeroNames[i % DefaultHeroNames.Length],
                Specialization = DefaultSpecializations[i % DefaultSpecializations.Length],
                Level = Math.Max(1, heroLevels[i]),
                IsActive = i < Math.Min(3, heroLevels.Length)
            });
        }

        return list.ToArray();
    }

    private static HeroProgressInfo NormalizeHero(HeroProgressInfo hero)
    {
        var safeName = string.IsNullOrWhiteSpace(hero.Name) ? "Hero" : hero.Name;
        return hero with
        {
            Name = safeName,
            Level = Math.Max(1, hero.Level)
        };
    }

    private static InventoryItemSnapshot[] NormalizeSnapshots(InventoryItemSnapshot[] snapshots)
    {
        var safeSnapshots = snapshots ?? Array.Empty<InventoryItemSnapshot>();
        return safeSnapshots.Select(snapshot => snapshot with
        {
            Name = snapshot.Name ?? string.Empty,
            Effects = snapshot.Effects == null
                ? new Dictionary<Characteristic, int>()
                : new Dictionary<Characteristic, int>(snapshot.Effects),
            BuffName = snapshot.BuffName ?? string.Empty
        }).ToArray();
    }
}

public sealed record HeroProgressInfo
{
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }
    public Specialization Specialization { get; init; }
    public bool IsActive { get; init; }
    public int SlotIndex { get; init; }
}

public sealed record InventoryItemSnapshot
{
    public string Name { get; init; } = string.Empty;
    public Dictionary<Characteristic, int> Effects { get; init; } = new();
    public string BuffName { get; init; } = string.Empty;
}
