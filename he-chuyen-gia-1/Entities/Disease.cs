namespace he_chuyen_gia_1.Entity
{
    public class Disease
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public List<string> MatchedSymptoms { get; set; }
        public List<string> UnmatchedSymptoms { get; set; }
    }
}
