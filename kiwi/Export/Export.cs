namespace kiwi.Export;

internal class Export
{
    public string[] DirectoriesToExport { get; set; } = Array.Empty<string>();
    public string[] FilesToIgnore { get; set; } = Array.Empty<string>();
    public string[] DirectoriesToIgnore { get; set; } = Array.Empty<string>();
    public bool Zip { get; set; } = false;
    public string ZipExtension { get; set; } = ".zip";
}
