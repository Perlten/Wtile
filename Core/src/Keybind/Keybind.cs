using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtile.Core.Keybind;

namespace Wtile.Core.Keybind
{
    public class WtileKeybind
    {
        private List<WtileKey> _keys;
        private Action _action;

        public bool Blocking = true;


        public WtileKeybind(List<WtileKey> keys, Action action)
        {
            _keys = keys;
            _action = action;
        }


        internal bool HandleTriggering(Dictionary<int, bool> keymap)
        {
            if (ShouldTrigger(keymap))
            {
                _action();
                return true;
            }
            return false;
        }

        private bool ShouldTrigger(Dictionary<int, bool> keymap)
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
    }
}
