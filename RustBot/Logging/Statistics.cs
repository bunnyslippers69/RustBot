﻿using Discord.Commands;
using RustBot;
using System;
using System.Collections.Generic;
using System.Text;

namespace RustBot.Logging
{
    class Statistics
    {
        //Bot statistics
        public static readonly DateTime startupDate = DateTime.Now;
        public static readonly DateTime creationDate = DateTime.Parse("Wed Jul 15 2020, 16:23:51");
        public static int runCommands = 0;
        public static int messagesRead = 0;
        public static int guildChanges = 0;
        public static string lastGuildJoined = "";
        public static Dictionary<string, int> commandStats = new Dictionary<string, int> { };

        public static void UpdateCommandStats(string c)
        {
            commandStats.TryGetValue(c, out var currentCount);
            commandStats[c] = currentCount + 1;
        }

        public static int GetTotalUsers()
        {
            int totalUsers = 0;
            foreach (var guild in Program._client.Guilds)
            {
                totalUsers += guild.MemberCount;
            }

            return totalUsers;
        }

        public static int GetTotalChannels()
        {
            int totalChannels = 0;
            int totalCategories = 0;
            foreach (var guild in Program._client.Guilds)
            {
                totalChannels += guild.Channels.Count;
                totalCategories += guild.CategoryChannels.Count;
            }

            return totalChannels - totalCategories;
        }
    }
}
