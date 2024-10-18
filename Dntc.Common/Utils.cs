namespace Dntc.Common;

public static class Utils
{
    public static string IlOffsetToLabel(int ilOffset) => $"IL_{ilOffset:x4}";

    public static string LocalName(int localIndex) => $"__local_{localIndex}";

    public static string MakeValidCName(string name)
    {
        return name.Replace('<', '_')
            .Replace('>', '_');
    }
}