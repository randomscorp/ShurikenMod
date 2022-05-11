using UObject = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GlobalEnums;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using TMPro;
using MonoMod.RuntimeDetour;
namespace Shuriken
{
   
    public class ShurikenMod : Mod, ICustomMenuMod, IGlobalSettings<Settings.GlobalModSettings>, ILocalSettings<Settings.SaveModSettings>
    {
        internal static ShurikenMod Instance;

        public static Settings.GlobalModSettings settings { get; set; } = new Settings.GlobalModSettings();
        public void OnLoadGlobal(Settings.GlobalModSettings s) => settings = s;
        public Settings.GlobalModSettings OnSaveGlobal() => settings;

        public static Settings.SaveModSettings saveSettings { get; set; } = new Settings.SaveModSettings();
        public void OnLoadLocal(Settings.SaveModSettings s) => saveSettings = s;
        public Settings.SaveModSettings OnSaveLocal() => saveSettings;


        public bool ToggleButtonInsideMenu => false;

        public static IEnumerator HideCurrentMenu(On.UIManager.orig_HideCurrentMenu orig, UIManager self)
        {
            Modding.Logger.Log(self.menuState);
            if (self.menuState == MainMenuState.DYNAMIC_MENU &&
             self.currentDynamicMenu == ModMenu.Screen &&
             !saveSettings.startupSelection && ModMenu.skipPauseMenu)
            {
                ModMenu.startPlaying();
                yield return self.HideMenu(ModMenu.Screen);
                yield return null;
            }
            else
            {
                yield return orig(self);
            }
        }
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            ModMenu.saveModsMenuScreen(modListMenu);
            return ModMenu.CreatemenuScreen();
        }





        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");
            Instance = this;
            ShurikenControl.shurikenW = LoadSprite("tiso");
            ShurikenControl.shurikenW.transform.localScale /= 10;//4;//2.7f;
            ShurikenControl.shurikenW.SetActive(false);

            ShurikenControl.shurikenB = LoadSprite("tiso");
            ShurikenControl.shurikenB.transform.localScale /= 4;//2.7f;
            ShurikenControl.shurikenB.SetActive(false);


            ShurikenControl.DontDestroyOnLoad(ShurikenControl.shurikenW);
            ShurikenControl.DontDestroyOnLoad(ShurikenControl.shurikenB);
            ShurikenControl.DontDestroyOnLoad(ShurikenControl.shuriken);

            //ShurikenControl.shuriken = ShurikenControl.shurikenW;

            Hooks();
            
            
        }

        private static void Hooks() {

            On.HeroController.Start += HeroController_Start;
            On.HeroController.TakeDamage += HeroController_TakeDamage;

            bool rando = ModHooks.GetMod("Randomizer 4") is Mod;
            bool ic = ModHooks.GetMod("ItemChangerMod") is Mod;

            if (rando) Rando.MenuHolder.Hook();
            Rando.ShurikenRando.Hook(rando,ic);
        }

        private static void HeroController_TakeDamage(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, GlobalEnums.CollisionSide damageSide, int damageAmount, int hazardType)
        {
            if (ShurikenControl.shurikenInstance) { ShurikenControl.Destroy(ShurikenControl.shurikenInstance); }
            orig(self, go, damageSide, damageAmount, hazardType);
        }

        private static void HeroController_Start(On.HeroController.orig_Start orig, HeroController self)
        {
            HeroController.instance.gameObject.AddComponent<ShurikenControl>();
            orig(self);

        }

        private GameObject LoadSprite(string name)
        {
            GameObject shuriken = new GameObject("shuriken");
            Texture2D texture;
            using (FileStream s = File.Open((Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + $"/static/{name}.png").ToString(), FileMode.Open))
            {

                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                s.Dispose();
                texture = new Texture2D(1, 1);
                texture.LoadImage(buffer);

                texture.Apply();
            }
            SpriteRenderer spriteRenderer = shuriken.AddComponent<SpriteRenderer>();
            spriteRenderer
                .sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            //DamageEnemies damage = shuriken.AddComponent<DamageEnemies>();

            return shuriken;
        }
    }
}
