using System.IO;
using System.Text.Json;

namespace Game.Core.Progression;

public sealed class JsonFileProgressStorage : IProgressStorage
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _path;

    public JsonFileProgressStorage(string path)
    {
        _path = path;
    }

    public ProgressState Load()
    {
        try
        {
            if (!File.Exists(_path))
            {
                return new ProgressState();
            }

            var json = File.ReadAllText(_path);
            var state = JsonSerializer.Deserialize<ProgressState>(json);
            return (state ?? new ProgressState()).Normalize();
        }
        catch
        {
            return new ProgressState();
        }
    }

    public void Save(ProgressState state)
    {
        var normalized = state.Normalize();
        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrWhiteSpace(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(normalized, SerializerOptions);
        File.WriteAllText(_path, json);
    }
}
