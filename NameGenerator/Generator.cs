namespace NameGenerator;

public static class Generator
{
    public static string GenerateRandomFirstName() =>
        GetRandomLineFromFile("FirstNames.txt", "Jimbo");

    public static string GenerateRandomSurname() =>
        GetRandomLineFromFile("Surnames.txt", "Jones");

    public static string GenerateRandomFullName() =>
        $"{GenerateRandomFirstName()} {GenerateRandomSurname()}";

    private static string GetRandomLineFromFile(string filename, string @default)
    {
        string[] names = File.ReadAllLines(filename) ?? new[] { @default };
        return names[Random.Shared.Next(names.Length)];
    }
}