﻿using Discord.Commands;
using System;
using System.Threading.Tasks;
using SSRPBalanceBot;
using SSRPBalanceBot.Permissions;
using Discord;
using RustBot.Logging;
using System.Diagnostics;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
public class BotStats : ModuleBase<SocketCommandContext>
{
    [Command("botstats", RunMode = RunMode.Async)]
    [Summary("Returns various bot statistics.")]
    [Remarks("Info")]
    public async Task SendBotStats()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        if (PermissionManager.GetPerms(Context.Message.Author.Id) < PermissionConfig.User) { await Context.Channel.SendMessageAsync("Not authorised to run this command."); return; }

        int totalUsers = 0;
        foreach (var guild in Program._client.Guilds)
        {
            var temp = guild.MemberCount;
            totalUsers += guild.MemberCount;
        }

        Embed msg = Utilities.GetEmbedMessage("Bot Statistics", $"Stats", 
            $"```js\n" +
            $"Creation Date: {Statistics.creationDate}\n" +
            $"Days Since Created: {Math.Floor((DateTime.Now - Statistics.creationDate).TotalDays)}\n" +
            $"Up-Time: {DateTime.Now.Subtract(Statistics.startupDate).ToString(@"dd\:hh\:mm\:ss")}\n" +
            $"Total Guilds: {Program._client.Guilds.Count}\n" +
            $"Total DMs: {Program._client.DMChannels.Count}\n" +
            $"Total Channels: {Statistics.GetTotalChannels()}\n" +
            $"Total Users: {totalUsers}\n" +
            $"Messages Handled: {Statistics.messagesRead}\n" +
            $"Commands Executed: {Statistics.runCommands}\n" +
            $"Guilds Since Boot: {Statistics.guildChanges}\n" +
            $"Last Guild Joined: {Statistics.lastGuildJoined}\n" +
            $"Latency: {Program._client.Latency}ms\n" +
            $"```", Context.User, Color.Red, Utilities.GetFooter(Context.User, sw));

        await ReplyAsync("", false, msg);
    }
}
