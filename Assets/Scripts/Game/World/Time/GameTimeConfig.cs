using System;
using UnityEngine;

namespace Game.World.GameTime
{
    [CreateAssetMenu(fileName = "GameTimeConfig", menuName = "Game/World/Game Time Config")]
    public sealed class GameTimeConfig : ScriptableObject
    {
        private const int MinutesPerHour = 60;
        private const int MinutesPerDay = 24 * MinutesPerHour;

        [Header("Fresh Game")]
        [SerializeField, Min(1)] private int _startDay = 1;
        [SerializeField, Range(0, 23)] private int _startHour = 8;
        [SerializeField, Range(0, 59)] private int _startMinute;

        [Header("Progression")]
        [SerializeField, Min(0.001f)] private float _realSecondsPerGameMinute = 1f;
        [SerializeField, Min(0.1f)] private float _saveIntervalRealSeconds = 5f;

        public float RealSecondsPerGameMinute => _realSecondsPerGameMinute;
        public float SaveIntervalRealSeconds => _saveIntervalRealSeconds;

        public long StartTotalMinutes =>
            ((long)_startDay - 1L) * MinutesPerDay +
            (long)_startHour * MinutesPerHour +
            _startMinute;

        public void Validate()
        {
            if (_startDay < 1)
                throw new InvalidOperationException($"{nameof(GameTimeConfig)} start day must be at least 1.");

            if (_startHour < 0 || _startHour > 23)
                throw new InvalidOperationException($"{nameof(GameTimeConfig)} start hour must be in range 0..23.");

            if (_startMinute < 0 || _startMinute > 59)
                throw new InvalidOperationException($"{nameof(GameTimeConfig)} start minute must be in range 0..59.");

            if (_realSecondsPerGameMinute <= 0f)
                throw new InvalidOperationException(
                    $"{nameof(GameTimeConfig)} real seconds per game minute must be greater than zero.");

            if (_saveIntervalRealSeconds <= 0f)
                throw new InvalidOperationException(
                    $"{nameof(GameTimeConfig)} save interval must be greater than zero.");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _startDay = Mathf.Max(1, _startDay);
            _startHour = Mathf.Clamp(_startHour, 0, 23);
            _startMinute = Mathf.Clamp(_startMinute, 0, 59);
            _realSecondsPerGameMinute = Mathf.Max(0.001f, _realSecondsPerGameMinute);
            _saveIntervalRealSeconds = Mathf.Max(0.1f, _saveIntervalRealSeconds);
        }
#endif
    }
}
