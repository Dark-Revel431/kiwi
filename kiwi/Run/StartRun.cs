namespace kiwi.Run;

internal class StartRun
{
    private static JsonData ParseJson()
    {
        try
        {
            string @string = File.ReadAllText("kiwi.project.json");
            JsonData? JsonObject = JsonConvert.DeserializeObject<JsonData>(@string);

            if (JsonObject != null && JsonObject.Run.FileToRun == null)
            {
                Console.WriteLine("Error, file to run is null.");
                Environment.Exit(1);
            }

            if (JsonObject == null) Environment.Exit(1); ;

            return JsonObject;
        }
        catch
        {
            throw new Exception("Error while parsing 'kiwi.project.json'.");
        }
    }
    internal static void Start()
    {
        if (!File.Exists("kiwi.project.json"))
        {
            throw new FileNotFoundException("'kiwi.project.json' not found.");
        }

        JsonData JsonObject = ParseJson();

        var process = Process.Start(JsonObject.Run.Interpreter, $"{JsonObject.Run.FileToRun} {JsonObject.Run.Args}");
        process.WaitForExit();

        Environment.Exit(0);
    }
}
