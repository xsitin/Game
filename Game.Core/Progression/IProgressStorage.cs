namespace Game.Core.Progression;

public interface IProgressStorage
{
    ProgressState Load();
    void Save(ProgressState state);
}
