using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using DiscordBot.Services;

namespace DiscordBot
{
    internal class Program
    {
        public static Task Main() => new Program().MainAsync();

        public async Task MainAsync()
        {
            // More info about "using" statement
            // https://www.c-sharpcorner.com/article/the-using-statement-in-C-Sharp/
            using (var services = ConfigureServices())                             
            {
                var client = services.GetRequiredService<DiscordSocketClient>();             
                var logger = services.GetRequiredService<Loggers>();                
                var commandHandler = services.GetRequiredService<CommandHandler>();

                await client.LoginAsync(TokenType.Bot, "");
                await client.StartAsync();

                await Task.Delay(-1);
            }

        }

        /*  
         * Here we make and configure classes that are gonna be used throught
         * the bot program and modules with commands or other services.
         
         * Singleton is a creational design pattern that lets you ensure that a class
         * has only one instance, while providing a global access point to this instance.
         * More info on how that works here: https://refactoring.guru/design-patterns/singleton
         
         * Firstly we create a new Singleton instance of DiscordSocketClient which allows us to use
         * DiscordSocketConfig class to put in a constructor to define what we want our bot to have. 
         * Possible configurations: https://discordnet.dev/api/Discord.WebSocket.DiscordSocketConfig.html
         
         * Secondly we make a new CommandService which is used to implement CommandHandlerService to 
         * execute commands from command modules.
         
         * Thirdly we add our newly made Loggers service that works the same as those above but we
         * don't put anything to the class contructor, that's because it automatically puts created 
         * and existing instances of DiscordSocketClient and CommandService to Loggers cnotructor 
         * so we can use them to get Logs messages and show them to the console.
        */
        private ServiceProvider ConfigureServices() => new ServiceCollection()
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.All,
                LogLevel = LogSeverity.Debug
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug
            }))
            .AddSingleton<Loggers>()
            .AddSingleton<CommandHandler>()
            .BuildServiceProvider();
        
    }
}
