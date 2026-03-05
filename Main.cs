using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.IO;

[assembly: MelonInfo(typeof(SSD_Demo_Fixer.Main), "SSD Demo Fixer", "1.0.0", "luckycdev")]
[assembly: MelonGame(null, null)]

namespace SSD_Demo_Fixer
{
    public class Main : MelonMod
    {
        private int _targetScene = 0;
        private string _flagPath = Path.Combine(MelonEnvironment.UserDataDirectory, "restart_target.txt"); // for when you press R
        private bool _isAutoLoading = false;
        
        private bool _showGui = true;

        private GUIStyle headerStyle;
        private GUIStyle buttonStyle;
        private GUIStyle helpStyle;

        public override void OnInitializeMelon()
        {
            if (File.Exists(_flagPath)) // read restart_target.txt
            {
                if (int.TryParse(File.ReadAllText(_flagPath), out int sceneID))
                {
                    _targetScene = sceneID;
                    _isAutoLoading = true;
                    LoggerInstance.Msg($"Restarted, targeting {GetSceneName(sceneID)} (ID: {sceneID})");
                }
                File.Delete(_flagPath); // delete restart_target.txt after its used
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex == 0) // if main menu, show buttons
            {
                if (_isAutoLoading) // if true, switch scenes to the right one after a restart
                {
                    _isAutoLoading = false; // reset to not loop
                    SceneManager.LoadScene(_targetScene);
                    return;
                }
                ShowMenuButtons();
            }

            if (buildIndex == 1) // if pregame lobby, remove black screen
            {
                RemoveBlackScreen();
            }
        }

        public override void OnUpdate() // once per frame
        {
            if (Input.GetKeyDown(KeyCode.F1)) _showGui = !_showGui; // main menu f1 toggles gui

            if (Input.GetKeyDown(KeyCode.R)) // r restarts game
            {
                _showGui = true;
                RestartGame();
            }

            if (Input.GetKeyDown(KeyCode.Alpha0)) // 0 sends you to main menu
            {
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    _showGui = false;
                    _targetScene = 0;
                    RestartGame();
                }
            }
        }

        private void RemoveBlackScreen()
        {
            GameObject blackPanel = GameObject.Find("BlackPanel"); // remove the black screen in the pregame lobby
            if (blackPanel != null)
            {
                UnityEngine.Object.Destroy(blackPanel);
                LoggerInstance.Msg("Successfully removed the pregame lobby black screen");
            }
            else
            {
                LoggerInstance.Warning("Could not find black screen to remove in pregame lobby");
            }
        }

        private void ShowMenuButtons()
        {
            bool found = false;
            foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.name == "UIAnimCtrl") // the parent of the buttons is disabled by default so we enable it
                {
                    obj.SetActive(true);
                    found = true;
                    break;
                }
            }
            if (found) LoggerInstance.Msg("Enabled main menu buttons");
            else LoggerInstance.Warning("Could not find UIAnimCtrl to enable main menu buttons");
        }

        public override void OnGUI()
        {
            float boxW = Screen.width * 0.4f;
            float boxH = Screen.height * 0.8f;
            float centerX = (Screen.width - boxW) / 2;
            float centerY = (Screen.height - boxH) / 2;

            InitGUIStyles(boxH); // initialize styles (once instead of every frame)

            if (SceneManager.GetActiveScene().buildIndex != 0 || !_showGui) return; // if not on main menu or if gui is toggled off, return

            GUI.color = Color.white;
            GUI.contentColor = Color.white;

            GUI.Box(new Rect(centerX, centerY, boxW, boxH), "");

            GUI.Label(new Rect(centerX, centerY + 40f, boxW, 50), "Game Selector", headerStyle);

            float padding = boxW * 0.05f;
            float startX = centerX + padding;
            float startY = centerY + (boxH * 0.15f);
            float btnW = (boxW - (padding * 3)) / 2;
            float btnH = boxH * 0.06f;
            float spacing = boxH * 0.015f;

            for (int i = 0; i <= 12; i++)
            {
                float x = (i % 2 == 0) ? startX : startX + btnW + padding;
                float y = startY + (Mathf.Floor(i / 2) * (btnH + spacing));

                GUI.color = (_targetScene == i) ? Color.yellow : Color.white;

                if (GUI.Button(new Rect(x, y, btnW, btnH), $"<b>{GetSceneName(i)}</b>", buttonStyle))
                {
                    _targetScene = i;
                }
            }

            GUI.Label(new Rect(centerX, centerY + (boxH * 0.68f), boxW, 120),
                "<b>[R]</b> Reload | <b>[0]</b> Return to Menu | <b>[F1]</b> Hide GUI\n\n" +
                "<b>Quick/Slow/Custom Races <color=red>DO NOT WORK</color>,\nyou must pick one of these.</b>", helpStyle);

            if (GUI.Button(new Rect(centerX + (boxW * 0.15f), centerY + (boxH * 0.85f), boxW * 0.7f, boxH * 0.1f), "<b>Load Game</b>", buttonStyle))
            {
                _showGui = false;
                LoggerInstance.Msg($"Loading scene {GetSceneName(_targetScene)} (ID: {_targetScene})");
                SceneManager.LoadScene(_targetScene);
            }
        }

        private void InitGUIStyles(float boxH)
        {
            if (headerStyle != null) return; // only init once

            headerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(boxH * 0.05f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperCenter,
                richText = true
            };
            headerStyle.normal.textColor = Color.white;

            buttonStyle = new GUIStyle(GUI.skin.button) { richText = true };
            buttonStyle.normal.textColor = Color.white;

            helpStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.RoundToInt(boxH * 0.025f),
                richText = true
            };
            helpStyle.normal.textColor = Color.white;
        }

        private string GetSceneName(int id)
        {
            return id switch { 0 => "Main Menu", 1 => "Pregame Lobby", 2 => "The Square", 3 => "Pop The Ball", 4 => "Ice Track", 5 => "Turbo Soccer", 6 => "Slow Race I", 7 => "Sumo Spray", 8 => "Statues", 9 => "Shark Ducks", 10 => "Pass The Bomb", 11 => "Slow Race II", 12 => "UFO Lights", _ => $"Scene {id}" };
        }

        private void RestartGame()
        {
            LoggerInstance.Msg("Restarting game");
            File.WriteAllText(_flagPath, _targetScene.ToString()); // write which game in restart_target.txt
            Process.Start(new ProcessStartInfo { FileName = Process.GetCurrentProcess().MainModule.FileName, UseShellExecute = true }); // start game
            Process.GetCurrentProcess().Kill(); // kill old game
        }
    }
}