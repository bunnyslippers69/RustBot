using Discord.Commands;
using System;
using System.Threading.Tasks;
using RustBot;
using RustBot.Permissions;
using System.Collections.Generic;
using Discord;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
public class ItemStore : ModuleBase<SocketCommandContext>
{
    [Command("itemstore", RunMode = RunMode.Async)]
    [Summary("Sends the current item store")]
    [Remarks("Tools")]
    public async Task SendITemStore()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        

        List<ItemStoreItem> itemStore = await Utilities.GetItemStore();

        if (itemStore == null) { await ReplyAsync("The item store is yet to refresh. Please wait a while and try again."); }

        EmbedBuilder eb = new EmbedBuilder();
        EmbedFooterBuilder fb = new EmbedFooterBuilder();

        fb.WithIconUrl(Context.Message.Author.GetAvatarUrl());
        eb.WithColor(PremiumUtils.SelectEmbedColour(Context.User));
        eb.WithTitle($"Item Store");
        eb.WithFooter(fb);

        foreach(var item in itemStore)
        {
            eb.AddField($"{item.ItemName}", $"Price: {Regex.Replace(item.ItemPrice, @"\t|\n|\r", "")}\nLink: [{item.ItemName}]({item.ItemURL})");
        }

        sw.Stop();
        fb.WithText(PremiumUtils.SelectFooterEmbedText(Context.User, sw));;

        await ReplyAsync("", false, eb.Build());
    }
}