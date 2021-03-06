﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Catch.Beatmaps;
using osu.Game.Rulesets.Catch.UI;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Catch.Objects
{
    public abstract class CatchHitObject : HitObject, IHasXPosition, IHasComboInformation
    {
        public const float OBJECT_RADIUS = 64;

        private float x;

        /// <summary>
        /// The horizontal position of the fruit between 0 and <see cref="CatchPlayfield.WIDTH"/>.
        /// </summary>
        public float X
        {
            get => x + XOffset;
            set => x = value;
        }

        /// <summary>
        /// Whether this object can be placed on the catcher's plate.
        /// </summary>
        public virtual bool CanBePlated => false;

        /// <summary>
        /// A random offset applied to <see cref="X"/>, set by the <see cref="CatchBeatmapProcessor"/>.
        /// </summary>
        internal float XOffset { get; set; }

        public double TimePreempt = 1000;

        public int IndexInBeatmap { get; set; }

        public virtual FruitVisualRepresentation VisualRepresentation => (FruitVisualRepresentation)(IndexInBeatmap % 4);

        public virtual bool NewCombo { get; set; }

        public int ComboOffset { get; set; }

        public Bindable<int> IndexInCurrentComboBindable { get; } = new Bindable<int>();

        public int IndexInCurrentCombo
        {
            get => IndexInCurrentComboBindable.Value;
            set => IndexInCurrentComboBindable.Value = value;
        }

        public Bindable<int> ComboIndexBindable { get; } = new Bindable<int>();

        public int ComboIndex
        {
            get => ComboIndexBindable.Value;
            set => ComboIndexBindable.Value = value;
        }

        /// <summary>
        /// Difference between the distance to the next object
        /// and the distance that would have triggered a hyper dash.
        /// A value close to 0 indicates a difficult jump (for difficulty calculation).
        /// </summary>
        public float DistanceToHyperDash { get; set; }

        public Bindable<bool> LastInComboBindable { get; } = new Bindable<bool>();

        /// <summary>
        /// The next fruit starts a new combo. Used for explodey.
        /// </summary>
        public virtual bool LastInCombo
        {
            get => LastInComboBindable.Value;
            set => LastInComboBindable.Value = value;
        }

        public float Scale { get; set; } = 1;

        /// <summary>
        /// Whether this fruit can initiate a hyperdash.
        /// </summary>
        public bool HyperDash => HyperDashTarget != null;

        /// <summary>
        /// The target fruit if we are to initiate a hyperdash.
        /// </summary>
        public CatchHitObject HyperDashTarget;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = (float)BeatmapDifficulty.DifficultyRange(difficulty.ApproachRate, 1800, 1200, 450);

            Scale = (1.0f - 0.7f * (difficulty.CircleSize - 5) / 5) / 2;
        }

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }

    /// <summary>
    /// Represents a single object that can be caught by the catcher.
    /// </summary>
    public abstract class PalpableCatchHitObject : CatchHitObject
    {
        public override bool CanBePlated => true;
    }

    public enum FruitVisualRepresentation
    {
        Pear,
        Grape,
        Pineapple,
        Raspberry,
        Banana // banananananannaanana
    }
}
