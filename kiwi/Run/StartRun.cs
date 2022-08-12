namespace kiwi.Run;

internal class StartRun
{
    private static JsonData ParseJson()
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
    internal static void Start()
    {
        try
        {
            if (!File.Exists("kiwi.project.json"))
            {
                Console.WriteLine("'kiwi.project.json' not found.");
                Environment.Exit(1);
            }

            JsonData JsonObject = ParseJson();

            var process = Process.Start(JsonObject.Run.Interpreter, $"{JsonObject.Run.FileToRun} {JsonObject.Run.Args}");
            process.WaitForExit();
            Environment.Exit(0);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
