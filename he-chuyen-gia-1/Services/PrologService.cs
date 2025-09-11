using SbsSW.SwiPlCs;
using he_chuyen_gia_1.Entity;

public class PrologService
{
    public PrologService()
    {
        if (!PlEngine.IsInitialized)
        {
            string[] param = { "-q", "-s", @"c:/users/huy/onedrive/documents/documents/prolog/benh-o-nguoi.pl" };
            PlEngine.Initialize(param);
        }
    }
    public List<string> GetSymptoms()
    {
        var results = new List<string>();

        using (var q = new PlQuery("symptom(X)"))
        {
            foreach (var sol in q.SolutionVariables)
            results.Add(sol["X"].ToString());
        }
        PlEngine.PlCleanup();
        return results;
    }

    public List<Disease> GetDiseases(string[] symptoms)
    {
        var results = new List<Disease>();
        var symptomList = string.Join(", ", symptoms);

        // Query gọi sang Prolog
        var query = $"diagnose_from_list([{symptomList}], Disease, Score, Matched, Unmatched)";

        using (var q = new PlQuery(query))
        {
            foreach (PlQueryVariables sol in q.SolutionVariables)
            {
                var disease = new Disease
                {
                    Name = sol["Disease"].ToString(),
                    Score =double.Parse(sol["Score"].ToString()),

                    // Matched là list
                    MatchedSymptoms = sol["Matched"]
                        .ToString()
                        .Trim('[', ']')
                        .Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList(),

                    // Unmatched là list
                    UnmatchedSymptoms = sol["Unmatched"]
                        .ToString()
                        .Trim('[', ']')
                        .Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList()
                };
                results.Add(disease);
            }
        }
        PlEngine.PlCleanup();
        return results;
    }

}
