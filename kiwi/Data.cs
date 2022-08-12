namespace kiwi;

internal class Data
{
    internal static string Version { get; } = "1.0";
    internal static string Interpreter { get; set; } = string.Empty;
    internal static String[] Args { get; set; } = Array.Empty<string>();
    internal static string Path { get; } = Directory.GetCurrentDirectory();
}
