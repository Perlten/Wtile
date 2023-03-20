using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtile.Core.Keybind;

namespace Wtile.Core.Keybind
{
    public class WtileKeybind : IComparable<WtileKeybind>
    {
        public WtileModKey ModKey { get; }
        private List<WtileKey> _keys;
        public Action Action { get; }

        public bool Blocking = true;


        public WtileKeybind(List<WtileKey> keys, WtileModKey modKey, Action action)
        {
            _keys = keys;
            Action = action;
            ModKey = modKey;
        }

        internal bool ShouldTrigger(Dictionary<int, bool> keymap, int keypressCounter)
        {
            if (_keys.Count == keypressCounter)
            {
                foreach (var key in _keys)
                {
                    var keyCode = (int)key;
                    if (!keymap[keyCode])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }


        public int CompareTo(WtileKeybind? other)
        {
            if (other == null) return 1;
            return _keys.Count.CompareTo(other._keys.Count);
        }
    }
}
