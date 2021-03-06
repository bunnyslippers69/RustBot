﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using DiscordBotsList.Api;
using DiscordBotsList;
using DiscordBotsList.Api.Objects;
using Discord.Webhook;
using System.Threading;
using System.Net;

namespace RustBot
{
    class LoggingUtils
    {
        public static SocketGuild destGuild = Program._client.GetGuild(701178110485463152);
        public static SocketGuild livelogGuild;
        public static SocketTextChannel livelogOutput;

        public static string apiKey;

        public static async Task Log(SocketUserMessage message, DateTime date, bool isPrivate)
        {
            if (isPrivate) { return; }

            var chnl = message.Channel as SocketGuildChannel;

            string sourceGuild = chnl.Guild.Name;

            string log = $"{date} | Username/ID: {message.Author.Username}/{message.Author.Id} | Message: \"{message.Content.Replace("\n", "\\n")}\"";

            if (!Directory.Exists("Logging")) { Directory.CreateDirectory("Logging"); }
            if (!Directory.Exists($"Logging/{sourceGuild}")) { Directory.CreateDirectory($"Logging/{sourceGuild}"); }
            if (!File.Exists($"Logging/{sourceGuild}/{message.Channel.Name}.log")) { File.Create($"Logging/{sourceGuild}/{message.Channel.Name}.log").Close(); }

            await FileWriteAsync($"Logging/{sourceGuild}/{message.Channel.Name}.log", log, true);
        }

        public static async Task FileWriteAsync(string filePath, string messaage, bool append = true)
        {
            using (FileStream stream = new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteLineAsync(messaage);
            }
        }

        public static async Task GuildJoinedAlert(SocketGuild g)
        {
            destGuild = Program._client.GetGuild(701178110485463152);
            SocketTextChannel destChannel = destGuild.GetTextChannel(701178110933991466);
            await destChannel.SendMessageAsync("", false, Utilities.GetEmbedMessage("Guild Joined", $"Joined Guild: {g.Name}", $"Owner: {g.Owner.Username}\nGuild ID: {g.Id}\nUsers: {g.MemberCount}", null));
        }

        public static async Task UpdateStats(ulong selfId)
        {
            AuthDiscordBotListApi DblApi = new AuthDiscordBotListApi(selfId, apiKey);

            var me = await DblApi.GetMeAsync();

            // Update stats           guildCount
            await me.UpdateStatsAsync(Program._client.Guilds.Count);
        }
    }
}
