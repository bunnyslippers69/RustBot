﻿using Discord.Commands;
using System;
using System.Threading.Tasks;
using SSRPBalanceBot;
using SSRPBalanceBot.Permissions;
using System.Collections.Generic;
using Discord;
using System.Linq;
using System.Text;
using System.Diagnostics;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
public class VoteLink : ModuleBase<SocketCommandContext>
{
    [Command("vote", RunMode = RunMode.Async)]
    [Summary("Sends a link to vote for the bot.")]
    [Remarks("Misc")]
    public async Task SendVoteLink()
    {
        if (PermissionManager.GetPerms(Context.Message.Author.Id) < PermissionConfig.User) { await Context.Channel.SendMessageAsync("Not authorised to run this command."); return; }

        EmbedBuilder eb = new EmbedBuilder();
        EmbedFooterBuilder fb = new EmbedFooterBuilder();


        fb.WithIconUrl(Context.Message.Author.GetAvatarUrl());

        eb.WithTitle($"Vote");
        eb.WithDescription("https://top.gg/bot/732215647135727716/vote");
        eb.WithThumbnailUrl("https://top.gg/images/logotrans.png");
        eb.WithColor(Color.Red);
        eb.WithFooter(fb);

        await ReplyAsync($"{Context.Message.Author.Mention}\n", false, eb.Build());

    }
}