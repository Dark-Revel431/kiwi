namespace kiwi;

class Program
{
    static void Main(String[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Invalid args:");
            Console.WriteLine("Usage:\nkiwi [command] [args]");
            Environment.Exit(1);
        }

        Data.Args = args;
        Parsers.ParseArgs();
    }
}