using System;
using System.IO;
using UnityEngine;

namespace Game.World.Quests.Progress
{
    public sealed class JsonQuestProgressStateStore : IQuestProgressStateStore
    {
        private readonly string _path;

        public JsonQuestProgressStateStore(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Quest progress state path cannot be empty.", nameof(path));

            _path = path;
        }

        public QuestProgressState Load()
        {
            if (File.Exists(_path) == false)
                return new QuestProgressState();

            var json = File.ReadAllText(_path);
            if (string.IsNullOrWhiteSpace(json))
                return new QuestProgressState();

            return JsonUtility.FromJson<QuestProgressState>(json) ?? new QuestProgressState();
        }

        public void Save(QuestProgressState state)
        {
            var directory = Path.GetDirectoryName(_path);
            if (string.IsNullOrWhiteSpace(directory) == false)
                Directory.CreateDirectory(directory);

            File.WriteAllText(_path, JsonUtility.ToJson(state ?? new QuestProgressState(), true));
        }
    }
}