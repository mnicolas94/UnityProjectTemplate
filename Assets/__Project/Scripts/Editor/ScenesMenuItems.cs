using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Editor.GenericSearchWindow;

namespace TGM.Editor
{
    public static class ScenesMenuItems
    {

        [MenuItem("Scenes/Open")]
        private static void Open()
        {
            OpenScenesMenu(sceneName => OpenScene(sceneName, false));
        }
        
        [MenuItem("Scenes/Open additively")]
        private static void OpenAdditively()
        {
            OpenScenesMenu(sceneName => OpenScene(sceneName, true));
        }

        private static void OpenScene(string sceneName, bool additive)
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            }
            else
            {
                EditorSceneManager.OpenScene(sceneName, additive ? OpenSceneMode.Additive : OpenSceneMode.Single);
            }
        }

        private static void OpenScenesMenu(Action<string> onSceneSelected)
        {
            var entries = new List<SearchEntry<string>>();
            
            // add scenes in build settings
            var scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            for (int i = 0; i < scenes.Count; i++)
            {
                var scene = scenes[i];
                var sceneName = Path.GetFileNameWithoutExtension(scene);
                sceneName = $"({i}) {sceneName}";
                var entry = new SearchEntry<string>(sceneName, scene);
                entries.Add(entry);
            }

            // add other scenes
            var scenesOthers = AssetDatabase.FindAssets("t:scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(scenePath => !scenes.Contains(scenePath));
            foreach (var scenePath in scenesOthers)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                sceneName = $"Others/{sceneName}";
                var entry = new SearchEntry<string>(sceneName, scenePath);
                entries.Add(entry);
            }

            var position = GetMousePosition();
            GenericSearchWindow<string>.Create(position, "Scenes", entries, onSceneSelected);
        }

        private static Vector2 GetMousePosition()
        {
            var field = typeof(Event).GetField("s_Current", BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                if (field.GetValue(null) is Event current)
                {
                    var position = current.mousePosition;
                    var screenPosition = GUIUtility.GUIToScreenPoint(position);
                    
                    return screenPosition;
                }
            }

            return Vector2.zero;
        }
    }
}