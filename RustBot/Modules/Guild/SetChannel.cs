﻿using Discord.Commands;
using System;
using System.Threading.Tasks;
using RustBot;
using RustBot.Permissions;
using System.Linq;
using Discord;
using RustBot.Users.Guilds;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
public class SetChannel : ModuleBase<SocketCommandContext>
{
    [Command("setchannel", RunMode = RunMode.Async)]
    [Summary("Used by server owners/admins to set the bots channel. Mention a channel to set. To set back to default, run r!setchannel null")]
    [Remarks("Guild")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task SetCommandsChannel([Remainder]string channelMention)
    {
        

        if (channelMention == "null") 
        { 
            GuildUtils.SetBotChannel(GuildUtils.GetSettings(Context.Guild.Id), default(ulong));
            await ReplyAsync("", false, Utilities.GetEmbedMessage("Set Channel", "Channel set", $"Bot channel is now: any", Context.User));
        }
        else 
        { 
            GuildUtils.SetBotChannel(GuildUtils.GetSettings(Context.Guild.Id), Context.Message.MentionedChannels.First().Id);
            await ReplyAsync("", false, Utilities.GetEmbedMessage("Set Channel", "Channel set", $"Bot channel is now: <#{Context.Message.MentionedChannels.First().Id}>", Context.User));
        }

    }
}