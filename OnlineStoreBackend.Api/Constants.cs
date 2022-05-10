using System.Text.RegularExpressions;

namespace OnlineStoreBackend.Api;

public static class Constants
{
    public static Regex ProductCodeRegex =
        new Regex("[0-9_]+");
}