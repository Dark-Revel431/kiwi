namespace kiwi.Export
{
    internal class StartExport
    {
        private static bool Zip { get; set; } = false;

        private static void CopyDirectory(string SourceDir, string DestDir, JsonData JsonObject)
        {
            DirectoryInfo dir = new(SourceDir);

            if (!dir.Exists) throw new DirectoryNotFoundException($"directory to export not found: {dir.FullName}");

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
            try
            {
                string @string = File.ReadAllText("kiwi.project.json");
                JsonData? JsonObject = JsonConvert.DeserializeObject<JsonData>(@string);

                if (JsonObject != null && JsonObject.Export != null && JsonObject.Export.DirectoriesToExport == null)
                {
                    throw new Exception("Directories to export are null.");
                }

                if (JsonObject == null || JsonObject.Export == null) Environment.Exit(1);

                Zip = JsonObject.Export.Zip;
                return JsonObject;
            }
            catch
            {
                throw new Exception("Error while parsing 'kiwi.project.json'.");
            }
        }

        private static void Export(JsonData JsonObject)
        {
            Console.WriteLine("EXPORTING THE PROJECT...");
            if (JsonObject.Export != null && JsonObject.Project != null)
            {
                foreach (string dir in JsonObject.Export.DirectoriesToExport)
                {
                    try
                    {
                        Console.WriteLine($"Exporting {dir}...");
                        CopyDirectory(dir, $"kiwi/export/{JsonObject.Project.Name}/{dir}", JsonObject);
                        Console.WriteLine("Done.");
                    }
                    catch
                    {
                        throw new Exception($"Error while exporting {dir}.");
                    }
                }

                if (Zip)
                {
                    try
                    {
                        Console.WriteLine($"Zipping {JsonObject.Project.Name}...");
                        ZipFile.CreateFromDirectory($"kiwi/export/{JsonObject.Project.Name}", $"kiwi/export/{JsonObject.Project.Name}{JsonObject.Export.ZipExtension}");
                        Console.WriteLine("Done.");
                    }
                    catch
                    {
                        throw new Exception($"Error while zipping {JsonObject.Project.Name}.");
                    }
                }
            }
            Console.WriteLine("PROJECT EXPORTED.");

            Environment.Exit(0);
        }

        internal static void Start()
        {
            if (!File.Exists("kiwi.project.json"))
            {
                throw new FileNotFoundException("'kiwi.project.json' not found.");
            }

            JsonData JsonObject = ParseJson();

            if (JsonObject.Export != null && JsonObject.Export.DirectoriesToExport.Length == 0)
            {
                Console.WriteLine("PROJECT EXPORTED.");
                Environment.Exit(1);
            }
            
            if (Directory.Exists("kiwi/export"))
            {
                try
                {
                    Directory.Delete("kiwi/export", true);
                    Directory.CreateDirectory("kiwi/export");
                }
                catch
                {
                    throw new Exception("Error while deleting and re-building 'kiwi/export'");
                }
            }

            Export(JsonObject);
        }
    }
}
