#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
using System;
using System.Collections;
using System.Reflection;
using Game.World.GameTime;
using NUnit.Framework;
using UnityEngine;

namespace Game.World.Shop.Customers.Flow
{
    public sealed class CustomerTrafficProfileTests
    {
        [TestCase(0, 0, 0)]
        [TestCase(5, 59, 0)]
        [TestCase(6, 0, 6)]
        [TestCase(7, 59, 6)]
        [TestCase(8, 0, 8)]
        [TestCase(11, 59, 8)]
        [TestCase(12, 0, 12)]
        [TestCase(16, 59, 12)]
        [TestCase(17, 0, 17)]
        [TestCase(20, 59, 17)]
        [TestCase(21, 0, 21)]
        [TestCase(23, 59, 21)]
        public void Resolve_UsesLatestPeriodStart(int hour, int minute, int expectedStartHour)
        {
            var profile = CreateProfile(ValidPeriods());

            try
            {
                profile.Validate();

                var resolved = profile.Resolve(new GameTimeSnapshot(hour * 60L + minute));

                Assert.That(resolved.StartHour, Is.EqualTo(expectedStartHour));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(profile);
            }
        }

        [TestCaseSource(nameof(InvalidProfiles))]
        public void Validate_RejectsInvalidPeriods(CustomerTrafficPeriod[] periods)
        {
            var profile = CreateProfile(periods);

            try
            {
                Assert.Throws<InvalidOperationException>(profile.Validate);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(profile);
            }
        }

        private static IEnumerable InvalidProfiles()
        {
            yield return new TestCaseData(Array.Empty<CustomerTrafficPeriod>()).SetName("Empty");
            yield return new TestCaseData(new[] { Period(6, 0) }).SetName("Missing midnight");
            yield return new TestCaseData(new[] { Period(0, 0), Period(0, 0) }).SetName("Duplicate start");
            yield return new TestCaseData(new[] { Period(0, 0), Period(8, 0), Period(6, 0) })
                .SetName("Unordered start");
            yield return new TestCaseData(new[] { Period(0, 0), Period(24, 0) }).SetName("Invalid hour");
            yield return new TestCaseData(new[] { Period(0, 0), Period(1, 60) }).SetName("Invalid minute");
            yield return new TestCaseData(new[] { new CustomerTrafficPeriod(0, 0, 0f, 1f) })
                .SetName("Non-positive minimum");
            yield return new TestCaseData(new[] { new CustomerTrafficPeriod(0, 0, 2f, 1f) })
                .SetName("Maximum below minimum");
        }

        private static CustomerTrafficPeriod[] ValidPeriods() =>
            new[]
            {
                new CustomerTrafficPeriod(0, 0, 30f, 45f),
                new CustomerTrafficPeriod(6, 0, 12f, 20f),
                new CustomerTrafficPeriod(8, 0, 4f, 7f),
                new CustomerTrafficPeriod(12, 0, 3f, 6f),
                new CustomerTrafficPeriod(17, 0, 2f, 4f),
                new CustomerTrafficPeriod(21, 0, 12f, 20f)
            };

        private static CustomerTrafficPeriod Period(int hour, int minute) =>
            new(hour, minute, 1f, 2f);

        private static CustomerTrafficProfile CreateProfile(CustomerTrafficPeriod[] periods)
        {
            var profile = ScriptableObject.CreateInstance<CustomerTrafficProfile>();
            typeof(CustomerTrafficProfile)
                .GetField("_periods", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(profile, periods);
            return profile;
        }
    }
}
#endif
