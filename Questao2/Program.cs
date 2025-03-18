using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        using (HttpClient client = new())
        {
            // team1
            int currentPage = 1;
            int totalPages = 1;
            while (currentPage <= totalPages)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}";
                string response = client.GetStringAsync(url).Result;
                var result = JsonConvert.DeserializeObject<FootballMatchesResponse>(response);
                totalPages = result!.Total_pages;
                foreach (var match in result.Data!)
                {
                    totalGoals += int.Parse(match.Team1goals!);
                }
                currentPage++;
            }

            // team2
            currentPage = 1;
            totalPages = 1;
            while (currentPage <= totalPages)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPage}";
                string response = client.GetStringAsync(url).Result;
                var result = JsonConvert.DeserializeObject<FootballMatchesResponse>(response);
                totalPages = result!.Total_pages;
                foreach (var match in result.Data!)
                {
                    totalGoals += int.Parse(match.Team2goals!);
                }
                currentPage++;
            }
        }
        return totalGoals;
    }
}

public class FootballMatchesResponse
{
    public int Page { get; set; }
    public int Per_page { get; set; }
    public int Total { get; set; }
    public int Total_pages { get; set; }
    public List<FootballMatch>? Data { get; set; }
}

public class FootballMatch
{
    public string? Team1 { get; set; }
    public string? Team2 { get; set; }
    public string? Team1goals { get; set; }
    public string? Team2goals { get; set; }
}
