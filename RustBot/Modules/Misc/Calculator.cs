﻿using Discord.Commands;
using System;
using System.Threading.Tasks;
using RustBot;
using RustBot.Permissions;
using System.Data;
using Discord;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
public class Calculator : ModuleBase<SocketCommandContext>
{
    [Command("calculate", RunMode = RunMode.Async)]
    [Alias("calc")]
    [Summary("Returns the value of the calculation specified")]
    [Remarks("Misc")]
    public async Task SendCalc([Remainder]string math)
    {
        

        math = math.Replace("x", "*").Replace(",", "");

        EmbedBuilder eb = new EmbedBuilder();
        EmbedFooterBuilder fb = new EmbedFooterBuilder();


        fb.WithText(PremiumUtils.SelectFooterEmbedText(Context.User));;
        fb.WithIconUrl(Context.Message.Author.GetAvatarUrl());

        eb.WithTitle($"{new DataTable().Compute(math, null).ToString()}");
        eb.WithColor(Color.Blue);
        eb.WithFooter(fb);



        await ReplyAsync("", false, eb.Build());
    }
}