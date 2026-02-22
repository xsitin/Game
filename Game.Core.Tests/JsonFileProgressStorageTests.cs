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
        Assert.Equal([1, 1, 1], state.HeroLevels);
    }

    [Fact]
    public void SaveThenLoad_RoundTripsProgress()
    {
        var path = Path.Combine(Path.GetTempPath(), $"game-core-progress-{Guid.NewGuid():N}.json");
        var storage = new JsonFileProgressStorage(path);

        var saved = new ProgressState
        {
            EncounterCounter = 7,
            HeroLevels = [2, 5, 9]
        };

        storage.Save(saved);
        var loaded = storage.Load();

        Assert.Equal(7, loaded.EncounterCounter);
        Assert.Equal([2, 5, 9], loaded.HeroLevels);

        File.Delete(path);
    }
}
