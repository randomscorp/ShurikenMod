using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Modules;

namespace Shuriken.Rando
{
    class ShurikenModule : Module
    {
        public override void Initialize()
        {
            ShurikenMod.settings.shurikenLevel = 0;
        }

        public override void Unload()
        {
        }

        public void GiveShuriken()
        {
            int level = ShurikenMod.settings.shurikenLevel;
            Modding.Logger.Log("oi");
            if (level < 2)
            {
                ShurikenMod.settings.shurikenLevel += 1;
            }


        }
    }
}
