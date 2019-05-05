﻿using osu.Framework.Allocation;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using System.IO;
using static System.Environment;

namespace Rhythmic.Screens.MainMenu
{
    public class MainMenu : Screen
    {
        [Resolved]
        private BeatmapCollection collection { get; set; }

        [Resolved]
        private BeatmapAPI API { get; set; }

        protected override void LoadComplete()
        {
            LoadAllBeatmaps();
            System.Console.WriteLine(collection.Beatmaps.Count);

            base.LoadAsyncComplete();
        }

        private void LoadAllBeatmaps()
        {
            var path = GetFolderPath(SpecialFolder.ApplicationData) + @"\Rhythmic\Database\Beatmaps\";

            foreach (var file in Directory.EnumerateDirectories(path))
            {
                int Length = path.Length;
                API.GetBeatmapFromZip(file);
                var level = API.ParseBeatmap(File.ReadAllText(file + @"\level.json"));
                collection.Beatmaps.Add(level);
            }
        }
    }
}