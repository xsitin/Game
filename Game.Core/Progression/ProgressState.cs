using System.Linq;

namespace Game.Core.Progression;

public sealed class ProgressState
{
    public int EncounterCounter { get; init; } = 1;
    public int[] HeroLevels { get; init; } = [1, 1, 1];

    public ProgressState Normalize()
    {
        var counter = EncounterCounter < 1 ? 1 : EncounterCounter;
        var levels = HeroLevels.Length == 0 ? [1, 1, 1] : HeroLevels.Select(x => x < 1 ? 1 : x).ToArray();
        return new ProgressState
        {
            EncounterCounter = counter,
            HeroLevels = levels
        };
    }
}
