using System.CommandLine;

namespace Furnace
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("Furnace mod manager for modrinth.");
            
            // run id*
            var runCommand = new Command("run", "Run a modpack");
            var runArgument = new Argument<string>("id", "The packs modrinth id.");
            runCommand.AddArgument(runArgument);
            
            runCommand.SetHandler((id) =>
            {
                Console.WriteLine($"Run");
            });
            rootCommand.AddCommand(runCommand);
            
            // list*
            var listPacksCommand = new Command("list", "List all packs");
            var listVerboseOption = new Option<bool>("verbose", "Show additional pack information");
            listPacksCommand.AddOption(listVerboseOption);
            rootCommand.AddCommand(listPacksCommand);
            
            // options
            var optionsCommand = new Command("options", "Application options");
            var listOptionsCommand = new Command("list", "List all options");
            var editOptionsCommand = new Command("edit", "Edit options file");
            var editOptionsEditorOption = new Option<string>("editor", "JSON editor program.");
            var setOptionsCommand = new Command("set", "Set a value");
            var setOptionsKeyArgument = new Argument<string>("key");
            var setOptionsValueArgument = new Argument<string>("value");
            var getOptionsCommand = new Command("get", "Get a value");
            var getOptionsKeyArgument = new Argument<string>("key");
            setOptionsCommand.AddArgument(setOptionsKeyArgument);
            setOptionsCommand.AddArgument(setOptionsValueArgument);
            getOptionsCommand.AddArgument(getOptionsKeyArgument);
            editOptionsCommand.AddOption(editOptionsEditorOption);
            optionsCommand.AddCommand(editOptionsCommand);
            optionsCommand.AddCommand(listOptionsCommand);
            optionsCommand.AddCommand(setOptionsCommand);
            optionsCommand.AddCommand(getOptionsCommand);
            rootCommand.AddCommand(optionsCommand);
            
            // modrinth
            var modrinthCommand = new Command("modrinth");
            var modrinthLogin = new Command("login");
            var modrinthLogout = new Command("login");
            
            var modrinthInfo = new Command("info");
            modrinthCommand.AddCommand(modrinthLogin);
            modrinthCommand.AddCommand(modrinthLogout);
            modrinthCommand.AddCommand(modrinthInfo);
            rootCommand.AddCommand(modrinthCommand);
            
            // mojang
            var mojangCommand = new Command("mojang");
            var mojangLogin = new Command("login");
            var mojangLogout = new Command("login");
            var mojangInfo = new Command("info");
            mojangCommand.AddCommand(mojangLogin);
            mojangCommand.AddCommand(mojangLogout);
            mojangCommand.AddCommand(mojangInfo);
            rootCommand.AddCommand(mojangCommand);
            
            // install
            var installCommand = new Command("install", "Install a modpack by id or URL");
            var installMultiArgument = new Argument<string>("id");
            installCommand.AddArgument(installMultiArgument);
            rootCommand.AddCommand(installCommand);
            
            // update
            var updateCommand = new Command("update", "Update a modpack by id");
            var updateIdArgument = new Argument<string>("id");
            installCommand.AddArgument(updateIdArgument);
            rootCommand.AddCommand(updateCommand);

            return await rootCommand.InvokeAsync(args);
        }
    }
}