namespace kiwi;

internal class Data
{
    internal static string Interpreter { get; set; } = string.Empty;
    internal static String[] Args { get; set; } = Array.Empty<string>();
    internal static string Path { get; } = Directory.GetCurrentDirectory();
}
