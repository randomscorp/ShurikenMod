using System;
using InControl;
using Modding.Converters;
using Newtonsoft.Json;

namespace Shuriken
{
    public class Settings
    {

        public class SaveModSettings
        {
            public bool startupSelection { get; set; } = false;

        }

        public class GlobalModSettings
        {
            public string Version { get; set; } = "";

            [JsonConverter(typeof(PlayerActionSetConverter))]
            public KeyBinds keybinds = new KeyBinds();
            public int shurikenLevel = 0;
            public bool shurikenRando = false;
        }

        public class KeyBinds : PlayerActionSet
        {
            public PlayerAction ShurikenKey;

            public KeyBinds()
            {
                ShurikenKey = CreatePlayerAction("ShurikenKey");
                DefaultBinds();
            }

            private void DefaultBinds()
            {
                ShurikenKey.AddDefaultBinding(Key.V);
            }
        }


    }
}
