using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text;

namespace Zork.Scripts
{
    public class Game
    {
        [JsonIgnore]
        public static Game Instance { get; private set; }

        public World World { get; private set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        private bool IsRunning { get; }

        [JsonIgnore]
        public CommandManager CommandManager { get; }

        public Game() => CommandManager = new CommandManager();

        public static void Start(string gameFileName) 
        {
            if (!File.Exists(gameFileName)) 
            {
                throw new FileNotFoundException("Expected file.", gameFileName);
            }

            while (Instance == null || Instance.mIsRestarting) 
            {
                Instance = Load(gameFileName);
                Instance.LoadCommands();
                Instance.LoadScripts();
                Instance.DisplayWelcomeMessage();
                Instance.Run();
            }
        }
        
        public void Run()
        {
            mIsRunning = true;
            Room previousRoom = null;
            LoadCommands();
            while (mIsRunning)
            {
                Console.WriteLine(Player.Location);
                if (previousRoom != Player.Location)
                {
                    CommandManager.PerformCommand(this, "LOOK");
                    previousRoom = Player.Location;
                }

                Console.Write("\n> ");
                if (CommandManager.PerformCommand(this, Console.ReadLine().Trim()))
                {
                    Player.Moves++;
                }
                else
                {
                    Console.WriteLine("That's not a verb I recognize.");
                }
            }
        }

        public void Restart() 
        {
            mIsRunning = false;
            mIsRestarting = true;
            Console.Clear();
        }

        public void => mIsRunning = false;

        public static Game Load(string filename)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();

            return game;
        }

        private void LoadCommands()
        {
            var commandMethods = (from type in Assembly.GetExecutingAssembly().GetTypes()
                                  from method in type.GetMethods()
                                  let attribute = method.GetCustomAttribute<CommandAttribute>()
                                  where type.IsClass && type.GetCustomAttribute<CommandAttribute>() != null
                                  where attribute != null
                                  select new Command(attribute.CommandName, attribute.Verbs,
                                  (Action<Game, CommandContext>)Delegate.CreateDelegate(typeof(Action<Game, CommandContext>)
                                  method)));
            
            CommandManager.AddCommands(commandMethods);
        }

        private void LoadScripts()
        {
            foreach (string file in Directory.EnumerateFiles(ScriptDictionary, ScriptFileExtension))
            {
                try
                {
                    var scriptOptions = ScriptOptions.Default.AddReferences(Assembly.GetExecutingAssembly());
#if(DEBUG)
                    scriptOptions.WithEmitDebugInformation(true)
                        .WithFilePath(new FileInfo(file).FullName)
                        .withFileEncoding(Encoding.UTF8);
#endif
                    string script = File.ReadAllText(file);
                    CSharpScript.RunAsync(script, scriptOptions).Wait();
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Error compiling script: {file} Error: {ex.Message}");
                }
            }
        }

        public bool ConfirmAction(string prompt) 
        {
            Console.Write(prompt);

            while (true) 
            {
                string response = Console.ReadLine().Trim().ToUpper();
                if (response == "YES" || response == "Y")
                {
                    return true;
                }
                else if (response == "NO" || response == "N")
                {
                    return false;
                }
                else 
                {
                    Console.Write("Please answer yes or no.> ");
                }
            }
        }
        private void DisplayWelcomeMessage() => Console.WriteLine(WelcomeMessage);

        public static readonly Random Random = new Random();
        private static readonly string ScriptDirectory = "Scripts";
        private static readonly string ScriptFileExtension = "*.csx";

        [JsonProperty]
        private string WelcomeMessage = null;
        private bool mIsRunning;
        private bool mIsRestarting;
    }
}
