﻿using osu.Framework.Bindables;
using System;
using System.Linq;

namespace Rhythmic.Screens.Edit
{
    public class BindableBeatDivisor : BindableNumber<int>
    {
        public static readonly int[] VALID_DIVISORS = { 1, 2, 3, 4, 6, 8, 12, 16 };

        public BindableBeatDivisor(int value = 1)
            : base(value)
        {
        }

        public void Next() => Value = VALID_DIVISORS[Math.Min(VALID_DIVISORS.Length - 1, Array.IndexOf(VALID_DIVISORS, Value) + 1)];

        public void Previous() => Value = VALID_DIVISORS[Math.Max(0, Array.IndexOf(VALID_DIVISORS, Value) - 1)];

        public override int Value
        {
            get => base.Value;
            set
            {
                if (!VALID_DIVISORS.Contains(value))
                    throw new ArgumentOutOfRangeException($"Provided divisor is not in {nameof(VALID_DIVISORS)}");

                base.Value = value;
            }
        }

        protected override int DefaultMinValue => VALID_DIVISORS.First();
        protected override int DefaultMaxValue => VALID_DIVISORS.Last();
        protected override int DefaultPrecision => 1;
    }
}
