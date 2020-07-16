using Discord.Commands;
using System;
using System.Threading.Tasks;
using SSRPBalanceBot;
using SSRPBalanceBot.Permissions;
using System.Collections.Generic;
using Discord;
using System.Linq;
using System.Text;
using Discord.Addons.Interactive;
using System.Diagnostics;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
public class ItemInfo : InteractiveBase
{
    Stopwatch sw = new Stopwatch();

    [Command("item", RunMode = RunMode.Async)]
    [Summary("Sends item info")]
    public async Task SendItemInfo([Remainder]string item)
    {
        sw.Start();
        if (PermissionManager.GetPerms(Context.Message.Author.Id) < PermissionConfig.User) { await Context.Channel.SendMessageAsync("Not authorised to run this command."); return; }

        //Stops people spamming characters as to lag the bot
        if (item.Length > 100) { await ReplyAsync("Please use a shorter item name."); return; }
        if (item.Length < 3) { await ReplyAsync("Please use a longer item name."); return; }

        //Grabbing item info
        var itemInfo = await Utilities.SearchForItem(item);

        if (itemInfo == null) { await ReplyAsync($"Item `{item}` not found."); await Utilities.StatusMessage("item", Context); return; }
        else
        {
            if(itemInfo.Count > 1)
            {
                StringBuilder si = new StringBuilder();
                int count = 0;

                foreach(SearchItem s in itemInfo)
                {
                    count++;
                    si.Append($"{count}. {s.ItemName}\n");
                }

                si.Append("\n**Please type the number of the item you are looking for.**");
                sw.Stop();

                await ReplyAsync("", false, Utilities.GetEmbedMessage("Search Results", "Multiple Results", si.ToString(), Context.Message.Author, Color.Red));

                
                var response = await NextMessageAsync();
                sw.Start();

                if(response != null)
                {
                    Item i = await Utilities.GetItemInfo(itemInfo[Convert.ToInt32(response.Content) - 1]);

                    await ReplyAsync("", false, GenMessage(i));
                }
                
            }
            else
            {
                Item i = await Utilities.GetItemInfo(itemInfo[0]);

                await ReplyAsync("", false, GenMessage(i));
            }

            await Utilities.StatusMessage("item", Context);
        }
    }

    private Embed GenMessage(Item i)
    {
        EmbedBuilder eb = new EmbedBuilder();
        EmbedFooterBuilder fb = new EmbedFooterBuilder();


        fb.WithIconUrl(Context.Message.Author.GetAvatarUrl());

        eb.WithThumbnailUrl(i.Icon);
        eb.WithUrl(i.URL);
        eb.WithTitle($"{i.ItemName}");
        eb.AddField("Description", $"{i.Description}");

        //Info table builder
        if (i.ItemInfoTable.Count != 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var info in i.ItemInfoTable)
            {
                sb.Append($"**{info.Stat.Replace("\n", "")}** - {info.Value.Replace("\n", "")}\n");
            }

            eb.AddField("Item Info", $"{sb.ToString()}", true);
        }

        //Drop chance info builder
        if (i.DropChances.Count != 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"__*Container - Amount - Chance*__\n");

            foreach (var d in i.DropChances)
            {
                sb.Append($"{d.Container} - {d.Amount} - {d.Chance}%\n");
            }

            eb.AddField("Drop Chances", $"{sb.ToString()}", true);
        }

        //Crafting info builder
        if(i.Ingredients.Count != 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"__*Ingredient - Amount*__\n");

            foreach (var d in i.Ingredients)
            {
                sb.Append($"{d.IngredientName} {d.IngredientAmount}\n");
            }

            eb.AddField("Ingredients", $"{sb.ToString()}", false);
        }

        sw.Stop();
        fb.WithText($"Called by {Context.Message.Author.Username} | Completed in {sw.ElapsedMilliseconds}ms");

        eb.WithColor(Color.Red);
        eb.WithFooter(fb);

        return eb.Build();
    }
}