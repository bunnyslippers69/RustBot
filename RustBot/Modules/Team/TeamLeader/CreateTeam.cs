﻿using Discord.Commands;
using System;
using System.Threading.Tasks;
using RustBot;
using RustBot.Permissions;
using System.Linq;
using System.Collections.Generic;
using Discord.Addons.Interactive;
using Discord;
using RustBot.Users.Teams;
using System.Net.Sockets;
using Discord.WebSocket;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!
public class CreateTeam : InteractiveBase
{
    [Command("createteam", RunMode = RunMode.Async)]
    [Summary("Creates a team. You can invite members to your team with r!teaminvite.")]
    [Remarks("Team Leader")]
    [RequireBotPermission(GuildPermission.ManageRoles)]
    public async Task SendRoll()
    {
        

        bool notifications;

        //Asks the user whether or not to enable notifications
        await ReplyAndDeleteAsync("", false, Utilities.GetEmbedMessage("Team Creation", "Notifications", Language.Team_Creation_Notifications, Context.User));
        var notifResponse = await NextMessageAsync();

        if (notifResponse.Content == "1") { notifications = true; }
        else if (notifResponse.Content == "2") { notifications = false; }
        else { await ReplyAsync("", false, Utilities.GetEmbedMessage("Team Creation", "Error", Language.Team_Creation_Error_Invalid, Context.User)); return; }

        //Creates the team role
        IRole teamRole = await Context.Guild.CreateRoleAsync($"{Context.User.Username}'s Team", null, null, false, null);

        if (!TeamUtils.CreateTeam(Context.User.Id, new ulong[] { }, teamRole, Context.Guild, notifications)) { await ReplyAsync("", false, Utilities.GetEmbedMessage("Team Creation", "Unsuccessful", Language.Team_Error_Has_Team, Context.User)); await Context.Guild.GetRole(teamRole.Id).DeleteAsync(); return; }

        await (Context.User as IGuildUser).AddRoleAsync(teamRole);
        await ReplyAsync("", false, Utilities.GetEmbedMessage("Team Creation", "Success", $"Team successfully created (<#{teamRole.Id}>). Invite people using r!teaminvite", Context.User));
    }
}