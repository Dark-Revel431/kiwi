namespace kiwi.New;

internal class New
{
    private static bool Venv { get; set; } = false;
    private static bool Main { get; set; } = true;
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
        Console.WriteLine($"Building {ProjectName}...");
        if (Config != null)
        {
            string JsonString = File.ReadAllText(Config);
            JsonData? JsonObject = JsonConvert.DeserializeObject<JsonData>(JsonString);

            if (JsonObject != null && (JsonObject.Directories == Array.Empty<string>() || JsonObject.Directories == null))
            {
                HttpClient client = new();
                var r = client.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/.kiwi/json/NewConfig.json");
                r.Wait();

                JsonData? JsonObjectHttp = JsonConvert.DeserializeObject<JsonData>(r.Result);
                if (JsonObjectHttp != null ) Directories = JsonObjectHttp.Directories;
            }
            else
            {
                if (JsonObject != null) Directories = JsonObject.Directories;
            }
        }

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
            HttpClient client2 = new();
            var r2 = client2.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/.kiwi/templates/main.py");
            r2.Wait();

            File.WriteAllText($"{ProjectName}/main.py", "");
            Console.WriteLine("Created 'main.py' file.");
        }

        if (Venv)
        {
            ProcessStartInfo psi = new(Data.Interpreter, "-m venv venv")
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false,
            };
            var process = Process.Start(psi);
            if (process != null)
            {
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(process.StandardError.ReadToEnd()))
                {
                    Console.WriteLine("Error while creating 'venv':");
                    Console.WriteLine(process.StandardError.ReadToEnd());
                }
                else
                {
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                }
            }

            Console.WriteLine("Created 'venv'.");
        }
    }

    internal static void Start()
    {
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
