namespace CMS.Core.Generators;

public class NameGenerator
{
    public static string GenerateCode()
    {
        return Guid.NewGuid().ToString();
    }
}