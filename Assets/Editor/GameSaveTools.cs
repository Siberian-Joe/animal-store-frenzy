using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.EditorTools
{
    public static class GameSaveTools
    {
        private static readonly string[] GameplaySaveFileNames =
        {
            "world-state.json",
            "quest-progress.json"
        };

        [MenuItem("Tools/Animal Store Frenzy/Clear Gameplay Save")]
        private static void ClearGameplaySave()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("Stop Play Mode before clearing gameplay save files.");
                return;
            }

            var deletedCount = 0;

            foreach (var fileName in GameplaySaveFileNames)
            {
                var path = Path.Combine(Application.persistentDataPath, fileName);
                if (File.Exists(path) == false)
                    continue;

                File.Delete(path);
                deletedCount++;
                Debug.Log($"Deleted gameplay save: {path}");
            }

            Debug.Log(deletedCount > 0
                ? $"Gameplay save cleared ({deletedCount} file(s))."
                : $"Gameplay save is already clear: {Application.persistentDataPath}");
        }

        [MenuItem("Tools/Animal Store Frenzy/Open Gameplay Save Folder")]
        private static void OpenGameplaySaveFolder()
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}