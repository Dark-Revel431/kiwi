namespace kiwi;

internal class Parsers
{
    internal static void ParseArgs()
    {
        switch (Data.Args[0])
        {
            case "new":
                New.StartNew.Start();
                break;
            case "run":
                Run.StartRun.Start();
                break;
            case "export":
                break;
            case "package":
                break;
            case "reload":
                break;
            case "plugin":
                break;
            case "--version":
                break;
            default:
                break;
        }
    }
}
