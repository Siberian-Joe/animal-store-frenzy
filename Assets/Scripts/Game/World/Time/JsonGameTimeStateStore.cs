using System;
using System.IO;
using UnityEngine;

namespace Game.World.GameTime
{
    public sealed class JsonGameTimeStateStore : IGameTimeStateStore
    {
        private readonly string _path;

        public JsonGameTimeStateStore(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Game time state path cannot be empty.", nameof(path));

            _path = path;
        }

        public bool TryLoad(out GameTimeState state)
        {
            state = null;

            if (File.Exists(_path) == false)
                return false;

            var json = File.ReadAllText(_path);
            if (string.IsNullOrWhiteSpace(json))
                return false;

            state = JsonUtility.FromJson<GameTimeState>(json);
            return state != null;
        }

        public void Save(GameTimeState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var directory = Path.GetDirectoryName(_path);
            if (string.IsNullOrWhiteSpace(directory) == false)
                Directory.CreateDirectory(directory);

            File.WriteAllText(_path, JsonUtility.ToJson(state, true));
        }
    }
}
