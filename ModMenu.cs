using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using UnityEngine;
using System.Collections;

using static Modding.Logger;


namespace Shuriken
{
    public class ModMenu
    {

        public static MenuScreen Screen;
        public static MenuScreen modListMenu;
        private static MappableKey ShuKey = null;

        public static bool skipPauseMenu = false;


        public static void saveModsMenuScreen(MenuScreen screen)
        {
            modListMenu = screen;
        }


        public static void BackSetting()
        {
            Log("Back Setting");
            if (!ShurikenMod.saveSettings.startupSelection && skipPauseMenu)
            {
                startPlaying();
            }
            else
            {
                UIManager.instance.UIGoToDynamicMenu(modListMenu);
            }
        }

        public static void startPlaying()
        {
            Log("start playing");
            ShurikenMod.saveSettings.startupSelection = true;
            skipPauseMenu = false;
            GameManager.instance.StartCoroutine(closePauseMenu());
        }

        public static IEnumerator closePauseMenu()
        {
            //UIManager.instance.UIGoToDynamicMenu(modListMenu); 
            yield return null;
            //UIManager.instance.UIGoToPauseMenu();
            //yield return new WaitForSeconds(0.1f);
            //yield return UIManager.instance.UIClosePauseMenu();
            UIManager.instance.TogglePauseGame();
        }
        private static void addMenuOptions(ContentArea area)
        {

            area.AddTextPanel("HelpText2",
                    new RelVector2(new Vector2(850f, 105f)),
                    new TextPanelConfig
                    {
                        Text = "Set your key for spawning shuriken",
                        Size = 30,
                        Font = TextPanelConfig.TextFont.TrajanRegular,
                        Anchor = TextAnchor.MiddleCenter
                    });

            area.AddKeybind(
                    "ShurikenBind",
                    ShurikenMod.settings.keybinds.ShurikenKey,
                    new KeybindConfig
                    {
                        Label = "Shuriken",
                        CancelAction = _ => { BackSetting(); },
                    }, out ShuKey
                );


        }

            public static MenuScreen CreatemenuScreen()
            {
                var builder = new MenuBuilder(UIManager.instance.UICanvas.gameObject, "SKMenu")
                    .CreateTitle("Shuriken", MenuTitleStyle.vanillaStyle)
                    .CreateContentPane(RectTransformData.FromSizeAndPos(
                        new RelVector2(new Vector2(1920f, 903f)),
                        new AnchoredPosition(
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0f, -60f)
                        )
                    ))
                    .CreateControlPane(RectTransformData.FromSizeAndPos(
                        new RelVector2(new Vector2(1920f, 250f)),
                        new AnchoredPosition(
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0f, -502f)
                        )
                    ))
                    .SetDefaultNavGraph(new ChainedNavGraph())

                    .AddControls(
                        new SingleContentLayout(new AnchoredPosition(
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0f, 32f)
                        )),
                        c => c.AddMenuButton(
                            "ApplyButton",
                                new MenuButtonConfig
                                {
                                    Label = "Apply",
                                    CancelAction = _ => { BackSetting(); },
                                    SubmitAction = _ => { ApplySetting(); },
                                    Style = MenuButtonStyle.VanillaStyle,
                                    Proceed = true
                                }
                            )
                    ).AddControls(
                        new SingleContentLayout(new AnchoredPosition(
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0f, -64f)
                        )),
                        c => c.AddMenuButton(
                            "BackButton",
                                new MenuButtonConfig
                                {
                                    Label = "Back",
                                    CancelAction = _ => { BackSetting(); },
                                    SubmitAction = _ => { BackSetting(); },
                                    Style = MenuButtonStyle.VanillaStyle,
                                    Proceed = true
                                }
                            )
                    );
                builder.AddContent(new NullContentLayout(), c => c.AddScrollPaneContent(
                new ScrollbarConfig
                {
                    CancelAction = _ => { BackSetting(); },

                    Navigation = new Navigation
                    {
                        mode = Navigation.Mode.Explicit,
                    },
                    Position = new AnchoredPosition
                    {
                        ChildAnchor = new Vector2(0f, 1f),
                        ParentAnchor = new Vector2(1f, 1f),
                        Offset = new Vector2(-310f, 0f)
                    }
                },
                    new RelLength(1600f),
                    RegularGridLayout.CreateVerticalLayout(125f),
                    contentArea => addMenuOptions(contentArea)
                ));
                Screen = builder.Build();

                return Screen;

            }
        public static void ApplySetting()
        {
            Log("Apply Setting");
            if (!ShurikenMod.saveSettings.startupSelection && skipPauseMenu)
            {
                startPlaying();
            }
            else
            {
                UIManager.instance.UIGoToDynamicMenu(modListMenu);
            }
        }

    }
}