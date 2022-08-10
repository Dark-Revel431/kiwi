namespace kiwi;

internal class Parsers
{
    internal static void ParseArgs()
    {
        switch (Data.Args[0])
        {
            case "new":
                New.New.Start();
                break;
            case "run":
                break;
            case "export":
                break;
            case "newfile":
                break;
            case "newinit":
                break;
            case "package":
                break;
            default:
                break;
        }
    }
}
