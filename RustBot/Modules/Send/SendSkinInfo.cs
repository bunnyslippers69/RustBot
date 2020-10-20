using Discord.Commands;
using System;
using System.Threading.Tasks;
using RustBot;
using RustBot.Permissions;
using System.Collections.Generic;
using Discord;
using Discord.Addons.Interactive;
using System.Linq;
using System.Text;
using System.Diagnostics;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
[Group("wip")]
public class Skin : InteractiveBase
{
    string[] sortType = { "new", "price", "discount", "exclusive", "market", "name", "item"};
    string[] sortDir = { "asc", "desc"};

    [Command("skin", RunMode = RunMode.Async)]
    [Summary("Sends skin info | Currently not working correctly")]
    [Remarks("Admin")]
    public async Task SendHelpMessage()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        

        string orderBy;
        string orderDirection;

        //Asks user for the desired order type
        await ReplyAndDeleteAsync("", false, Utilities.GetEmbedMessage("Skin Information", "Order By", Language.SkinInfo_Order_By, Context.Message.Author), TimeSpan.FromSeconds(20));
        var orderByResponse = await NextMessageAsync();
        
        if(!Int32.TryParse(Utilities.GetNumbers(orderByResponse.Content), out int order)) { await ReplyAsync("Make sure to type just the number of your choice."); return; }
        orderBy = sortType[order - 1];

        //Asks the user for the desired sort direction - asc, desc
        await ReplyAndDeleteAsync("", false, Utilities.GetEmbedMessage("Skin Information", "Order Direction", Language.SkinInfo_Order_Direction, Context.Message.Author), TimeSpan.FromSeconds(20));
        var sortDirectionResponse = await NextMessageAsync();

        if (!Int32.TryParse(Utilities.GetNumbers(sortDirectionResponse.Content), out int sortDirection)) { await ReplyAsync("Make sure to type just the number of your choice."); return; }
        orderDirection = sortDir[sortDirection - 1];

        //Asks the user for their search query
        await ReplyAndDeleteAsync("", false, Utilities.GetEmbedMessage("Skin Information", "Awaiting Search", Language.SkinInfo_Awaiting_Search, Context.Message.Author), TimeSpan.FromSeconds(20));
        var searchQueryResponse = await NextMessageAsync();

        List<SkinInfo> skinList = await Utilities.GetSkinInfo(searchQueryResponse.Content, orderBy, orderDirection);

        EmbedBuilder eb = new EmbedBuilder();
        
        eb.WithTitle($"Search Results");

        if (skinList.Count > 10) { eb.WithDescription($"Showing 10 of {skinList.Count} results. Narrow your search to view other results."); }

        for (int i = 0; i < 10; i++)
        {
            SkinInfo si = skinList[i];

            if (si.SkinUsualPrice == "0") { eb.AddField($"{si.SkinName}", $"Current Price: {si.SkinPrice}\n Link: [{si.SkinName}]({si.SkinURL})"); }
            else { eb.AddField($"{si.SkinName}", $"Current Price: {si.SkinPrice}\n Non-Discounted Price: {si.SkinUsualPrice}\n Link: [{si.SkinName}]({si.SkinURL})"); }
        }

        sw.Stop();
        eb.WithFooter(Utilities.GetFooter(Context.User, sw));
        await ReplyAsync("", false, eb.Build());
    }
}