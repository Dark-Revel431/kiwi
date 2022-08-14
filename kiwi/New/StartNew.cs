namespace kiwi.New;

internal class StartNew
{
    private static bool Venv { get; set; } = false;
    private static bool Main { get; set; } = true;
    private static bool ReadMe { get; set; } = true;
    private static string ProjectName { get; set; } = string.Empty;
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
    }

    private static void Create()
    {
        Console.WriteLine($"BUILDING '{ProjectName}' > VERSION: {Data.Version}\n\n");

        try
        {
            Console.WriteLine("Connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/json/NewConfig.json\n");
            HttpClient client = new();
            var r = client.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/json/NewConfig.json");
            r.Wait();

            JsonData? JsonObjectHttp = JsonConvert.DeserializeObject<JsonData>(r.Result);
            if (JsonObjectHttp != null) Directories = JsonObjectHttp.Directories;
            Console.WriteLine("Done.");
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error while connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/json/NewConfig.json");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }

        try
        {
            Console.WriteLine("Building project directories...");
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
            Console.WriteLine("Done.\n");
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error while building project directories.");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }

        if (Main)
        {
            try
            {
                Console.WriteLine("Connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/templates/main.py\n");
                HttpClient client2 = new();
                var r2 = client2.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/templates/main.py");
                r2.Wait();
                Console.WriteLine("Done.\n");

                Console.WriteLine("Building 'main.py' file.");
                File.WriteAllText($"{ProjectName}/main.py", r2.Result);
                Console.WriteLine("Done.");
            }
            catch (HttpRequestException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error while connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/templates/main.py");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(1);
            }
            catch 
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error while building 'main.py' file.");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(1);
            }
        }

        if (ReadMe)
        {
            try
            {
                Console.WriteLine("Building 'README.md' file...");
                File.Create($"{ProjectName}/README.md");
                Console.WriteLine("Done.");
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error while building 'README.md' file.");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Environment.Exit(1);
            }
        }

        if (Venv)
        {
            Console.WriteLine("Building 'venv'...");
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
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Error while building 'venv'.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine("Done.");
                }
            }
        }

        try
        {
            Console.WriteLine("Connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/json/kiwi.project.json\n");
            HttpClient client3 = new();
            var r3 = client3.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/json/kiwi.project.json");
            r3.Wait();
            Console.WriteLine("Done.");

            string @string = r3.Result.Replace("%%ProjectName%%", ProjectName);
            @string = @string.Replace("%%ProjectPath%%", Data.Path.Replace('\\', '/'));
            @string = @string.Replace("%%Interpreter%%", Data.Interpreter);

            Console.WriteLine("Building 'kiwi.project.json' file...");
            File.WriteAllText("kiwi.project.json", @string);
            Console.WriteLine("Done.");
        }
        catch (HttpRequestException)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error while connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/json/kiwi.project.json");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error while building 'kiwi.project.json' file.");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }

        try
        {
            Console.WriteLine("Building 'kiwi/plugins/__init__.py'...");
            File.Create("kiwi/plugins/__init__.py");
            Console.WriteLine("Done.");
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error while building 'kiwi/plugins/__init__.py' file.");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Environment.Exit(1);
        }

        try
        {
            Console.WriteLine("Connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/templates/piwi.py\n");
            HttpClient client4 = new();
            var r4 = client4.GetStringAsync("https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/templates/piwi.py");
            r4.Wait();
            Console.WriteLine("Done.");

            Console.WriteLine("Building 'piwi.py'...");
            File.WriteAllText("piwi.py", r4.Result);
            Console.WriteLine("Done.\n\n");
        }
        catch (HttpRequestException)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error while connecting with:\n > https://raw.githubusercontent.com/Dark-Revel431/kiwi/master/kiwi/kiwi/1.0/templates/piwi.py");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Error while building 'piwi.py' file.");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }

        Console.WriteLine($"BUILDED '{ProjectName}' > VERSION: {Data.Version}.");

        Environment.Exit(0);
    }

    internal static void Start()
    {
        if (Data.Args.Length < 3)
        {
            Console.WriteLine("Invalid args, usage:");
            Console.WriteLine(" > kiwi new [name] [interpreter] [args]");
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
