using System.Text;

string RemoveLeadingAndTrailingParentheses(string puzzleString)
{
    int firstOpenParenthesesOccursAt = puzzleString.IndexOf('(');
    if(firstOpenParenthesesOccursAt != -1)
    {
        puzzleString = puzzleString.Remove(firstOpenParenthesesOccursAt, 1);
    }    

    int lastClosedParenthesesOccursAt = puzzleString.LastIndexOf(')');
    if(lastClosedParenthesesOccursAt != -1)
    {
        puzzleString = puzzleString.Remove(lastClosedParenthesesOccursAt, 1);
    }

    return puzzleString;
}

string GetFieldNameThatContainsInnerFields(string puzzleString)
{
    string fieldThatContainsInnerFields = string.Empty;
    string[] splitPuzzleStringWithInnerFields = puzzleString.Split(',');
    for (int i =0; i < splitPuzzleStringWithInnerFields.Length; i++)
    {
        if(splitPuzzleStringWithInnerFields[i].Contains('('))
        {
            splitPuzzleStringWithInnerFields[i] = splitPuzzleStringWithInnerFields[i].Remove(splitPuzzleStringWithInnerFields[i].IndexOf('(')).Trim();
            fieldThatContainsInnerFields = splitPuzzleStringWithInnerFields[i];
            break;
        }
    }

    return fieldThatContainsInnerFields;
}

string GetSubFields(string puzzleString, int firstOpenParenthesesOccursAt, int lastClosedParenthesesOccursAt) => 
    puzzleString.Substring(firstOpenParenthesesOccursAt, lastClosedParenthesesOccursAt - firstOpenParenthesesOccursAt + 1);

string RemoveSubFields(string puzzleString, int firstOpenParenthesesOccursAt, int lastClosedParenthesesOccursAt) =>
    puzzleString.Remove(firstOpenParenthesesOccursAt, lastClosedParenthesesOccursAt - firstOpenParenthesesOccursAt + 1);

string[] SplitAndSortString(string puzzleString, bool sortFieldsAlphabetically)
{
    string[] splitPuzzleString = puzzleString.Split(',');
    if(sortFieldsAlphabetically)
    {
        splitPuzzleString = splitPuzzleString
            .ToList()
            .Select(s => s.Trim())
            .OrderBy(x => x)
            .ToArray();
    }
    else
    {
        splitPuzzleString = splitPuzzleString
            .ToList()
            .Select(s => s.Trim())
            .ToArray();
    }

    return splitPuzzleString;
}

string FormatStringWithDashesAndTabs(
    string[] splitPuzzleString, 
    int tabOffset, 
    string fieldThatContainsInnerFields, 
    string formattedInnerFields)
{
    StringBuilder puzzleSolutionStringBuilder = new StringBuilder();

    string tab = tabOffset != 0
            ? new string('\t', tabOffset)
            : string.Empty;

    for (int i =0; i < splitPuzzleString.Length; i++)
    {
        puzzleSolutionStringBuilder
            .Append(tab)
            .Append($"- {splitPuzzleString[i]}\n");

        if(!string.IsNullOrWhiteSpace(formattedInnerFields) && string.Equals(splitPuzzleString[i], fieldThatContainsInnerFields))
        {
            puzzleSolutionStringBuilder.Append(formattedInnerFields);
        }
    }

    return puzzleSolutionStringBuilder.ToString();
}

string ConvertPuzzleFormattedString(string puzzleString, int tabOffset = 0, bool sortFieldsAlphabetically = false)
{
    puzzleString = RemoveLeadingAndTrailingParentheses(puzzleString);
    
    int firstOpenParenthesesOccursAt = puzzleString.IndexOf('(');
    int lastClosedParenthesesOccursAt = puzzleString.LastIndexOf(')');

    if (firstOpenParenthesesOccursAt != -1 && lastClosedParenthesesOccursAt != -1)
    {
        string fieldThatContainsInnerFields = GetFieldNameThatContainsInnerFields(puzzleString);

        string subFields = GetSubFields(puzzleString, firstOpenParenthesesOccursAt, lastClosedParenthesesOccursAt);
        puzzleString = RemoveSubFields(puzzleString, firstOpenParenthesesOccursAt, lastClosedParenthesesOccursAt);

        string formattedInnerFields = ConvertPuzzleFormattedString(subFields, tabOffset + 1, sortFieldsAlphabetically);

        string[] splitPuzzleString = SplitAndSortString(puzzleString, sortFieldsAlphabetically);

        return FormatStringWithDashesAndTabs(splitPuzzleString, tabOffset, fieldThatContainsInnerFields, formattedInnerFields);
    }
    else
    {
        string[] splitPuzzleString = SplitAndSortString(puzzleString, sortFieldsAlphabetically);
        return FormatStringWithDashesAndTabs(splitPuzzleString, tabOffset, string.Empty, string.Empty);
    }
}

string puzzleString = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";

string result = ConvertPuzzleFormattedString(puzzleString);
Console.WriteLine("1) Unsorted result:\n");
Console.WriteLine(result);

result = ConvertPuzzleFormattedString(puzzleString, sortFieldsAlphabetically: true);
Console.WriteLine("2) Sorted result:\n");
Console.WriteLine(result);