using System;

namespace Game.World.GameTime
{
    public readonly struct GameTimeSnapshot : IEquatable<GameTimeSnapshot>
    {
        private const int MinutesPerHour = 60;
        private const int MinutesPerDay = 24 * MinutesPerHour;

        public GameTimeSnapshot(long totalMinutes)
        {
            if (totalMinutes < 0L)
                throw new ArgumentOutOfRangeException(nameof(totalMinutes));

            TotalMinutes = totalMinutes;

            var minuteOfDay = (int)(totalMinutes % MinutesPerDay);
            Day = totalMinutes / MinutesPerDay + 1L;
            Hour = minuteOfDay / MinutesPerHour;
            Minute = minuteOfDay % MinutesPerHour;
        }

        public long TotalMinutes { get; }
        public long Day { get; }
        public int Hour { get; }
        public int Minute { get; }

        public bool Equals(GameTimeSnapshot other) => TotalMinutes == other.TotalMinutes;

        public override bool Equals(object obj) => obj is GameTimeSnapshot other && Equals(other);

        public override int GetHashCode() => TotalMinutes.GetHashCode();

        public static bool operator ==(GameTimeSnapshot left, GameTimeSnapshot right) => left.Equals(right);

        public static bool operator !=(GameTimeSnapshot left, GameTimeSnapshot right) => left.Equals(right) == false;
    }
}
