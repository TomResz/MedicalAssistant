using System.Text.RegularExpressions;

namespace MedicalAssistant.Infrastructure.DAL;

internal static class ToTsQueryConverter
{
    public static string ToTsQuery(this string query)
    {
        var cleaned = Regex.Replace(query, @"[^\w\s]", "");
        cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();
        var tsQuery = cleaned.Replace(" ", " & ");
        tsQuery = Regex.Replace(tsQuery, @"(&\s*){2,}", "&"); 
        return tsQuery + ":*"; 
    }
}