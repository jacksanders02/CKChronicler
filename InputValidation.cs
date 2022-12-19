using System.Text.RegularExpressions;

namespace CKChronicler; 

public static class InputValidation {
    private static readonly Regex NumberRegex = new Regex("[0-9]+");

    public static bool IsNumeric(string input) {
        return NumberRegex.IsMatch(input);
    }
}