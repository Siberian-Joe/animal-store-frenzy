using System;
using Game.World.GameTime;
using UnityEngine;

namespace Game.World.Shop.Customers.Flow
{
    [Serializable]
    public struct CustomerTrafficPeriod
    {
        [Range(0, 23)] public int StartHour;
        [Range(0, 59)] public int StartMinute;
        [Min(0.01f)] public float MinSpawnIntervalMinutes;
        [Min(0.01f)] public float MaxSpawnIntervalMinutes;

        public CustomerTrafficPeriod(
            int startHour,
            int startMinute,
            float minSpawnIntervalMinutes,
            float maxSpawnIntervalMinutes)
        {
            StartHour = startHour;
            StartMinute = startMinute;
            MinSpawnIntervalMinutes = minSpawnIntervalMinutes;
            MaxSpawnIntervalMinutes = maxSpawnIntervalMinutes;
        }

        public int StartMinuteOfDay => StartHour * 60 + StartMinute;

        public float GetRandomSpawnIntervalMinutes() =>
            UnityEngine.Random.Range(MinSpawnIntervalMinutes, MaxSpawnIntervalMinutes);
    }

    [CreateAssetMenu(
        fileName = "CustomerTrafficProfile",
        menuName = "Game/Shop/Customer Traffic Profile")]
    public sealed class CustomerTrafficProfile : ScriptableObject
    {
        [SerializeField] private CustomerTrafficPeriod[] _periods;

        public CustomerTrafficPeriod Resolve(GameTimeSnapshot time)
        {
            if (_periods == null || _periods.Length == 0)
                throw new InvalidOperationException($"{nameof(CustomerTrafficProfile)} requires at least one period.");

            var minuteOfDay = time.Hour * 60 + time.Minute;

            for (var index = _periods.Length - 1; index >= 0; index--)
            {
                if (_periods[index].StartMinuteOfDay <= minuteOfDay)
                    return _periods[index];
            }

            throw new InvalidOperationException(
                $"{nameof(CustomerTrafficProfile)} has no period covering {time.Hour:00}:{time.Minute:00}.");
        }

        public void Validate()
        {
            if (_periods == null || _periods.Length == 0)
                throw new InvalidOperationException($"{nameof(CustomerTrafficProfile)} requires at least one period.");

            var previousStartMinute = -1;

            for (var index = 0; index < _periods.Length; index++)
            {
                var period = _periods[index];

                if (period.StartHour < 0 || period.StartHour > 23)
                {
                    throw new InvalidOperationException(
                        $"{nameof(CustomerTrafficProfile)} period {index} hour must be in range 0..23.");
                }

                if (period.StartMinute < 0 || period.StartMinute > 59)
                {
                    throw new InvalidOperationException(
                        $"{nameof(CustomerTrafficProfile)} period {index} minute must be in range 0..59.");
                }

                if (period.MinSpawnIntervalMinutes <= 0f)
                {
                    throw new InvalidOperationException(
                        $"{nameof(CustomerTrafficProfile)} period {index} minimum interval must be greater than zero.");
                }

                if (period.MaxSpawnIntervalMinutes < period.MinSpawnIntervalMinutes)
                {
                    throw new InvalidOperationException(
                        $"{nameof(CustomerTrafficProfile)} period {index} maximum interval must not be below its minimum.");
                }

                var startMinute = period.StartMinuteOfDay;

                if (index == 0 && startMinute != 0)
                {
                    throw new InvalidOperationException(
                        $"{nameof(CustomerTrafficProfile)} first period must start at 00:00.");
                }

                if (startMinute <= previousStartMinute)
                {
                    throw new InvalidOperationException(
                        $"{nameof(CustomerTrafficProfile)} period start times must be strictly ascending.");
                }

                previousStartMinute = startMinute;
            }
        }
    }
}
