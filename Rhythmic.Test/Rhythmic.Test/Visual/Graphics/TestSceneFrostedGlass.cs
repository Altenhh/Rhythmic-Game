﻿using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Screens;
using Rhythmic.Screens.Backgrounds;

namespace Rhythmic.Test.Visual.Graphics
{
    public class TestSceneFrostedGlass : TestScene
    {
        [TestCase(false)]
        [TestCase(true)]
        public void TestNoEffects(bool originalEffects) => createTest(0, 0, originalEffects);

        [Test]
        public void TestSubtractOriginalBlur() => createTest(10, 0, false);

        [Test]
        public void TestCopyOriginalBlur() => createTest(10, 0, true);

        [TestCase(false)]
        [TestCase(true)]
        public void TestBlurViewOnly(bool originalEffects) => createTest(0, 20, originalEffects);

        [TestCase(false)]
        [TestCase(true)]
        public void TestBlurBoth(bool originalEffects) => createTest(10, 20, originalEffects);

        [Test]
        public void TestNonSynchronisedQuad() => createTest(10, 0, false, false);

        private void createTest(float originalBlur, float viewBlur, bool originalEffects, bool synchronisedQuad = true)
        {
            AddStep("create container", () =>
            {
                Clear();

                BufferedContainer container = new BufferedContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    BlurSigma = new Vector2(originalBlur),
                    Children = new Drawable[]
                    {
                        new BackgroundScreenStack(new BackgroundScreenDefault()),
                        new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Colour = Color4.Aqua,
                            Size = new Vector2(100)
                        }
                    }
                };

                Children = new Drawable[]
                {
                    container,
                    new BlurView(container, viewBlur, originalEffects, synchronisedQuad)
                    {
                        Position = new Vector2(100, 100)
                    }
                };
            });
        }

        private class BlurView : CompositeDrawable
        {
            public BlurView(BufferedContainer buffer, float blur, bool displayEffects, bool synchronisedQuad)
            {
                Size = new Vector2(200);
                Masking = true;
                CornerRadius = 20;
                BorderColour = Color4.Magenta;
                BorderThickness = 2;

                InternalChildren = new Drawable[]
                {
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Colour = Color4.Magenta
                                        },
                                        new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = "You can drag this view.",
                                            Font = new FontUsage(size: 16),
                                        }
                                    }
                                }
                            },
                            new Drawable[]
                            {
                                new BufferedContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    BackgroundColour = Color4.Black,
                                    BlurSigma = new Vector2(blur),
                                    Child = buffer.CreateView().With(d =>
                                    {
                                        d.RelativeSizeAxes = Axes.Both;
                                        d.SynchronisedDrawQuad = synchronisedQuad;
                                        d.DisplayOriginalEffects = displayEffects;
                                    })
                                }
                            },
                        },
                        RowDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Absolute, 20),
                        }
                    }
                };
            }

            protected override bool OnDrag(DragEvent e)
            {
                Position += e.Delta;
                return true;
            }

            protected override bool OnDragEnd(DragEndEvent e) => true;
            protected override bool OnDragStart(DragStartEvent e) => true;
        }
    }
}
