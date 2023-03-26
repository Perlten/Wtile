namespace Wtile.Core.Keybind;

public class WtileKeybind : IComparable<WtileKeybind>
{
    public List<WtileModKey> ModKeys { get; }
    public WtileKey Key { get; }
    public Action Action { get; }

    public bool Blocking = true;


    public WtileKeybind(WtileKey key, List<WtileModKey> modKeys, Action action)
    {
        Key = key;
        Action = action;
        ModKeys = modKeys;
    }

    public int CompareTo(WtileKeybind? other)
    {
        return other?.ModKeys.Count.CompareTo(ModKeys.Count) ?? 0;
    }
}
