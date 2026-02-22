using System;
using System.Collections.Generic;
using System.IO;
using Game.Core.Combat;
using Game.Core.Progression;
using Xunit;

namespace Game.Core.Tests;

public sealed class JsonFileProgressStorageTests
{
    [Fact]
    public void Load_ReturnsDefault_WhenFileDoesNotExist()
    {
        var path = Path.Combine(Path.GetTempPath(), $"game-core-progress-{Guid.NewGuid():N}.json");
        var storage = new JsonFileProgressStorage(path);

        var state = storage.Load();

        Assert.Equal(1, state.EncounterCounter);
        Assert.Equal(0, state.Gold);
        Assert.Equal(3, state.HeroRoster.Length);
        Assert.Equal("Alden", state.HeroRoster[0].Name);
        Assert.Empty(state.Inventory);
        Assert.Empty(state.Shop);
    }

    [Fact]
    public void SaveThenLoad_RoundTripsExtendedProgress()
    {
        var path = Path.Combine(Path.GetTempPath(), $"game-core-progress-{Guid.NewGuid():N}.json");
        var storage = new JsonFileProgressStorage(path);

        var saved = new ProgressState
        {
            EncounterCounter = 7,
            Gold = 120,
            HeroRoster = new[]
            {
                new HeroProgressInfo
                {
                    Name = "Test",
                    Level = 4,
                    Specialization = Specialization.Wizard,
                    IsActive = true,
                    SlotIndex = 0
                }
            },
            Inventory = new[]
            {
                new InventoryItemSnapshot
                {
                    Name = "heal",
                    Effects = new Dictionary<Characteristic, int>
                    {
                        [Characteristic.Health] = 5
                    }
                }
            },
            Shop = new[]
            {
                new InventoryItemSnapshot
                {
                    Name = "bolt",
                    Effects = new Dictionary<Characteristic, int>
                    {
                        [Characteristic.Mana] = -10
                    },
                    BuffName = "Spark"
                }
            }
        };

        storage.Save(saved);
        var loaded = storage.Load();

        Assert.Equal(7, loaded.EncounterCounter);
        Assert.Equal(120, loaded.Gold);
        Assert.Single(loaded.HeroRoster);
        Assert.Equal("Test", loaded.HeroRoster[0].Name);
        Assert.Equal(5, loaded.Inventory[0].Effects[Characteristic.Health]);
        Assert.Equal("Spark", loaded.Shop[0].BuffName);

        File.Delete(path);
    }
}
