namespace Dntc.Common;

public static class Utils
{
    public static string IlOffsetToLabel(int ilOffset) => $"IL_{ilOffset:x4}";
}