using System;
using NewCore.Domain;

namespace NewCore.Data
{
    [Serializable]
    public class GameSettingsState : IModel
    {
        public int MusicVolume;
        public int SfxVolume;
    }
}