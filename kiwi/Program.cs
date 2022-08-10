namespace kiwi;

class Program
{
    static void Main(String[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Invalid args:");
            Console.WriteLine("Usage:\nkiwi [command] [name] [interpreter] [args]");
            Environment.Exit(1);
        }

        Data.Args = args;
        Parsers.ParseArgs();
    }
}