using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Services
{
    internal class CommandHandler
    {
        private  CommandService _commands;
        private  DiscordSocketClient _discord;
        private  ConfigurationService _config;
        private  IServiceProvider _services;

        public CommandHandler(IServiceProvider services, DiscordSocketClient client)
        {
            _discord = client;
            _services = services;
            _commands = services.GetRequiredService<CommandService>();
            
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var prefPos = 0;
            if (!message.HasCharPrefix(char.Parse("+"), ref prefPos)) return;

            var context = new SocketCommandContext(_discord, message);

            await _commands.ExecuteAsync(context, prefPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified) return;

            if (result.IsSuccess) return;
        }
    }
}
