namespace kiwi.Export
{
    internal class StartExport
    {
        private static bool Zip { get; set; } = false;

        private static void CopyDirectory(string SourceDir, string DestDir, JsonData JsonObject)
        {
            DirectoryInfo dir = new(SourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"directory to export not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(DestDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                if (JsonObject.Export != null && JsonObject.Export.FilesToIgnore.Contains(file.Name))
                {
                    continue;
                }
                string TargetFilePath = Path.Combine(DestDir, file.Name);
                file.CopyTo(TargetFilePath);
            }

            foreach (DirectoryInfo SubDir in dirs)
            {
                if (JsonObject.Export != null && JsonObject.Export.DirectoriesToIgnore.Contains(SubDir.Name))
                {
                    continue;
                }
                string NewDestinationDir = Path.Combine(DestDir, SubDir.Name);
                CopyDirectory(SubDir.FullName, NewDestinationDir, JsonObject);
            }
        }

        private static JsonData ParseJson()
        {
            string @string = File.ReadAllText("kiwi.project.json");
            JsonData? JsonObject = JsonConvert.DeserializeObject<JsonData>(@string);

            if (JsonObject != null && JsonObject.Export != null && JsonObject.Export.DirectoriesToExport == null)
            {
                Console.WriteLine("Error, directories to export are null.");
                Environment.Exit(1);
            }

            if (JsonObject == null || JsonObject.Export == null) Environment.Exit(1);

            Zip = JsonObject.Export.Zip;
            return JsonObject;
        }

        private static void Export(JsonData JsonObject)
        {
            if (JsonObject.Export != null && JsonObject.Project != null)
            {
                foreach (string dir in JsonObject.Export.DirectoriesToExport)
                {
                    CopyDirectory(dir, $"kiwi/export/{JsonObject.Project.Name}", JsonObject);
                    Console.WriteLine($"Copied {dir}");
                }

                if (Zip)
                {
                    Console.WriteLine($"Zipping {JsonObject.Project.Name}...");
                    ZipFile.CreateFromDirectory($"kiwi/export/{JsonObject.Project.Name}", $"kiwi/export/{JsonObject.Project.Name}{JsonObject.Export.ZipExtension}");
                    Console.WriteLine($"Zipped {JsonObject.Project.Name}.");
                }
            }
        }

        internal static void Start()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Exporting...\n");
                if (!File.Exists("kiwi.project.json"))
                {
                    Console.WriteLine("'kiwi.project.json' not found.");
                    Environment.Exit(1);
                }
                JsonData JsonObject = ParseJson();

                if (Directory.Exists("kiwi/export"))
                {
                    Directory.Delete("kiwi/export", true);
                    Directory.CreateDirectory("kiwi/export");
                }
                
                Export(JsonObject);
                Console.WriteLine("Exported.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
