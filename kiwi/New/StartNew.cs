namespace kiwi.New;

internal class StartNew
{
    private static bool Venv { get; set; } = false;
    private static bool Main { get; set; } = true;
    private static bool ReadMe { get; set; } = true;
    private static string? Config { get; set; } = null;
    private static string ProjectName { get; set; } = "Project";
    private static String[] Directories { get; set; } = Array.Empty<string>();

    private static void ParseArgs()
    {
        ProjectName = Data.Args[1];
        Data.Interpreter = Data.Args[2];

        if (Data.Args.Contains("--venv"))
        {
            try
            {
                Venv = Convert.ToBoolean(Data.Args[Array.IndexOf(Data.Args, "--venv") + 1].ToLower());
            }
            catch { }
        }
        else if (Data.Args.Contains("-v"))
        {
            try
            {
                Venv = Convert.ToBoolean(Data.Args[Array.IndexOf(Data.Args, "-v") + 1].ToLower());
            }
            catch { }
        }

        if (Data.Args.Contains("--readme"))
        {
            try
            {
                ReadMe = Convert.ToBoolean(Data.Args[Array.IndexOf(Data.Args, "--readme") + 1].ToLower());
            }
            catch { }
        }
        else if (Data.Args.Contains("-r"))
        {
            try
            {
                ReadMe = Convert.ToBoolean(Data.Args[Array.IndexOf(Data.Args, "-r") + 1].ToLower());
            }
            catch { }
        }

        if (Data.Args.Contains("--main"))
        {
            try
            {
                Main = Convert.ToBoolean(Data.Args[Array.IndexOf(Data.Args, "--main") + 1].ToLower());
            }
            catch { }
        }
        else if (Data.Args.Contains("-m"))
        {
            try
            {
                Main = Convert.ToBoolean(Data.Args[Array.IndexOf(Data.Args, "-m") + 1].ToLower());
            }
            catch { }
        }

        if (Data.Args.Contains("--config"))
        {
            try
            {
                if (File.Exists(Data.Args[Array.IndexOf(Data.Args, "--config") + 1]))
                {
                    Config = Data.Args[Array.IndexOf(Data.Args, "--config") + 1];
                }
            }
            catch { }
        }
        else if (Data.Args.Contains("-c"))
        {
            try
            {
                if (File.Exists(Data.Args[Array.IndexOf(Data.Args, "-c") + 1]))
                {
                    Config = Data.Args[Array.IndexOf(Data.Args, "-c") + 1];
                }
            }
            catch { }
        }
    }

    private static void Create()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine($"Building {ProjectName}...");
        if (Config != null)
        {
            string JsonString = File.ReadAllText(Config);
            JsonData? JsonObject = JsonConvert.DeserializeObject<JsonData>(JsonString);

            if (JsonObject != null && (JsonObject.Directories == Array.Empty<string>() || JsonObject.Directories == null))
            {
                HttpClient client = new();
                var r = client.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/json/NewConfig.json");
                r.Wait();

                JsonData? JsonObjectHttp = JsonConvert.DeserializeObject<JsonData>(r.Result);
                if (JsonObjectHttp != null ) Directories = JsonObjectHttp.Directories;
            }
            else
            {
                if (JsonObject != null) Directories = JsonObject.Directories;
            }
        }
        else
        {
            HttpClient client = new();
            var r = client.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/json/NewConfig.json");
            r.Wait();

            JsonData? JsonObjectHttp = JsonConvert.DeserializeObject<JsonData>(r.Result);
            if (JsonObjectHttp != null) Directories = JsonObjectHttp.Directories;
        }

        Console.WriteLine("Creating project directories...");
        foreach (string directory in Directories)
        {
            if (directory == "%%ProjectName%%")
            {
                Directory.CreateDirectory(ProjectName);
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
        }
        Console.WriteLine("Created project directories.");

        if (Main)
        {
            Console.WriteLine("Creating 'main.py' file.");
            HttpClient client2 = new();
            var r2 = client2.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/templates/main.py");
            r2.Wait();

            File.WriteAllText($"{ProjectName}/main.py", r2.Result);
            Console.WriteLine("Created 'main.py' file.");
        }

        if (ReadMe)
        {
            Console.WriteLine("Creating 'README.md' file...");
            File.Create($"{ProjectName}/README.md");
            Console.WriteLine("Created 'README.md' file.");
        }

        if (Venv)
        {
            Console.WriteLine("Creating 'venv'...");
            ProcessStartInfo psi = new(Data.Interpreter, "-m venv venv")
            {
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
            };
            var process = Process.Start(psi);
            if (process != null)
            {
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(process.StandardError.ReadToEnd()))
                {
                    Console.WriteLine("Error while creating 'venv'.");
                }
                else
                {
                    Console.WriteLine("Created 'venv'.");
                }
            }
        }

        Console.WriteLine("Creating 'kiwi.project.json' file...");
        HttpClient client3 = new();
        var r3 = client3.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/json/kiwi.project.json");
        r3.Wait();

        string @string = r3.Result.Replace("%%ProjectName%%", ProjectName);
        @string = @string.Replace("%%ProjectPath%%", Data.Path.Replace('\\', '/'));
        @string = @string.Replace("%%Interpreter%%", Data.Interpreter);

        File.WriteAllText("kiwi.project.json", @string);
        Console.WriteLine("Created 'kiwi.project.json' file.");

        Console.WriteLine("Creating 'kiwi/plugins/__init__.py'...");
        File.Create("kiwi/plugins/__init__.py");
        Console.WriteLine("Created 'kiwi/plugins/__init__.py'.");

        Console.WriteLine("Creating 'piwi.py'...");
        HttpClient client4 = new();
        var r4 = client4.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/templates/piwi.py");
        r4.Wait();

        File.WriteAllText("piwi.py", r4.Result);
        Console.WriteLine("Created 'piwi.py'.\n");

        Console.WriteLine($"Finished building {ProjectName}!");

        Console.ForegroundColor = ConsoleColor.White;
        Environment.Exit(0);
    }

    internal static void Start()
    {
        if (Data.Args.Length < 3)
        {
            Console.WriteLine("Invalid args:");
            Console.WriteLine("Usage:\nkiwi new [name] [interpreter] [args]");
            Environment.Exit(1);
        }

        try
        {
            ParseArgs();
            Create();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
}
