using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMGLMM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<String> tournaments = new List<String>();
        private List<String> Matchs = new List<String>();
        private List<String> performance = new List<String>();

        LolData data = new LolData();

        //private IWebDriver driver;

        //private string root = "odds-browser";

        public MainWindow()
        {
            InitializeComponent();
            data.ApiData();

            tournaments = data.TournamentList();

            listView.ItemsSource = null;
            listView.ItemsSource = tournaments;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            /* driver = new FirefoxDriver();
             INavigation nav = driver.Navigate();
             nav.GoToUrl("http://www.pinnaclesports.com/fi/odds/match/e-sports/league-of-legends/league-of-legends-championship-series");

             IList<IWebElement> selectElements = driver.FindElements(By.ClassName("ng-scope"));

             Console.WriteLine(selectElements.Count);*/


            /* IList<IWebElement> selectElements = driver.FindElements(By.ClassName(root));
             IList<IWebElement> match = driver.FindElements(By.CssSelector("table.odds-browser td.market_title a.event_name"));



             //for (int j = 0; j < match.Count; j++) {
                 selectElements[1].Click();
                 match[1].Click();

                 WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                 wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("oddsbody")));

                 IWebElement oddsTable = driver.FindElement(By.ClassName("oddsbody"));

                 // driver.Close();

                 string odds = oddsTable.GetAttribute("innerHTML");

                 //Console.Write(odds);
                 //Console.Write("<!-- -------------------------------------------------------------------------------------------- -->");
                 driver.Navigate().Back();
                 */
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Matchs = data.MatchsList(listView.SelectedIndex);


            listView3.ItemsSource = null;
            listView3.ItemsSource = Matchs;
        }

        private void listView3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            data.TeamPerformanceData(listView3.SelectedIndex);
            listView2.ItemsSource = null;
            listView4.ItemsSource = null;
            listView5.ItemsSource = null;
            listView6.ItemsSource = null;

            listView2.ItemsSource = data.teamBlue();
            listView4.ItemsSource = data.teamRed();

            listView5.ItemsSource = data.TeamBlueprobability(listView3.SelectedIndex);
            listView6.ItemsSource = data.TeamRedprobability(listView3.SelectedIndex);
        }
        // }
    }

    public class LolData
    {
        private string date, tournamentMatchIdentifieTable, matchs, teams, teamMatchIdentifieTable, teamPerformances;

        private List<String> tournamentsList = new List<String>();
        private List<String> tournamentsInfoList = new List<String>();

        private List<String> Matchs = new List<String>();
        private List<String> MatchsInfoList = new List<String>();

        private List<String> teamBluePerformanceList = new List<String>();
        private List<String> teamRedPerformanceList = new List<String>();

        private List<String> teamBlueProbabilityList = new List<String>();
        private List<String> teamRedProbabilityList = new List<String>();

        /*private List<String> MatchsEu2015Summer = new List<String>();
          private List<String> MatchsEu2015SummerPlayoffs = new List<String>();
          private List<String> MatchsEu2015Regionals = new List<String>();
          private List<String> MatchsEu2016Spring = new List<String>();*/

        public string tournamentTitle { get; set; }

        public string tournamentInfo
        {
            get { return tournamentTitle + ";" + torunamentID; }
        }

        public string torunamentID { get; set; }

        public string matchDate
        {
            get { return date; }
            set { string[] datesplit = value.Split('T'); date = datesplit[0]; }
        }
        public string matchID { get; set; }
        public string matchHash { get; set; }
        public string matchInfo
        {
            get { return matchHash + ";" + matchID + ";" + matchDate; }
        }

        public string teamName { get; set; }
        public string teamArgonym { get; set; }
        public string teamID { get; set; }

        public string teamBlueName { get; set; }
        public string teamBlueArgonym { get; set; }
        public string teamBlueID { get; set; }

        public string teamRedName { get; set; }
        public string teamRedArgonym { get; set; }
        public string teamRedID { get; set; }

        public string teamInfo
        {
            get { return teamBlueName + " vs " + teamRedName + " - " + matchDate + ";" + matchHash + ";" + teamBlueID + ";" + teamRedID ; }
        }

        public int firstBlood { get; set; }
        public int firstTower { get; set; }
        public int firstInhibitor { get; set; }
        public int firstBaron { get; set; }
        public int firstDragon { get; set; }
        public int firstRiftHerald { get; set; }
        public int towerTakedowns { get; set; }
        public int inhibitorTakedowns { get; set; }
        public int baronKills { get; set; }
        public string dragonKills { get; set; }
        public string riftHeraldKills { get; set; }
        public string team_identifier { get; set; }


        public string playerName { get; set; }
        public string playerID { get; set; }

        public string kills { get; set; }
        public string deaths { get; set; }
        public string assists { get; set; }
        
        // Read api data to json files;
        public void ApiData()
        {
            using (var webClient = new System.Net.WebClient())
            {
                string tournaments = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/tournament.php");

                tournaments = @"{""tournament"" :" + tournaments + "}";
                TournamentData(tournaments);

            }

            using (var webClient = new System.Net.WebClient())
            {
                var matchsApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/match.php");
                var tournamentMatchIdentifieTableApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/tournamentmatch.php");

                tournamentMatchIdentifieTable = @"{""dataStruct"" :" + tournamentMatchIdentifieTableApi + "}";
                matchs = @"{""match"" :" + matchsApi + "}";


            }

            using (var webClient = new System.Net.WebClient())
            {
                var teamMatchIdentifieTableApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/teammatch.php");
                var teamsApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/team.php");

                teamMatchIdentifieTable = @"{""dataStruct"" :" + teamMatchIdentifieTableApi + "}";
                teams = @"{""team"" :" + teamsApi + "}";
            }

            using (var webClient = new System.Net.WebClient())
            {
                var teamPerformancesApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/teamperformance.php");

                teamPerformances = @"{""teamperformance"" :" + teamPerformancesApi + "}";
            }

            using (var webClient = new System.Net.WebClient())
            {
                var players = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/player.php");

                players = @"{""player"" :" + players + "}";
                PlayerData(players);
            }

            using (var webClient = new System.Net.WebClient())
            {
                var playerTeamIdentifieTable = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/playerteam.php");

            }

            using (var webClient = new System.Net.WebClient())
            {
                var playerPerformances = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/playerperformance.php");

                playerPerformances = @"{""playerperformance"" :" + playerPerformances + "}";
                PlayerPerformanceData(playerPerformances);
            }
        }

        public void TournamentData(string json)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.tournament)
            {
                this.tournamentTitle = data.tournamentTitle;
                this.torunamentID = data.identifier;

                tournamentsInfoList.Add(tournamentInfo);
            }
        }

        public void MatchData(string json, string structTable, string tournamentIdentifier)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);
            dynamic dynObjStructTable = JsonConvert.DeserializeObject(structTable);

            foreach (var data in dynObj.match)
            {
                this.matchHash = data.matchHash;
                this.matchDate = data.date;
                this.matchID = data.identifier;

                // find right tournament
                foreach (var datastruct in dynObjStructTable.dataStruct)
                {
                    if (datastruct.tournament_identifier == tournamentIdentifier & datastruct.tournamentMatch_identifier == this.matchID)
                    {
                        MatchsInfoList.Add(matchInfo);
                        break;
                    }
                }
            }

            TeamData(teams, teamMatchIdentifieTable);
        }

        public void TeamData(string json, string structTable)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);
            dynamic dynObjStructTable = JsonConvert.DeserializeObject(structTable);

            int teamIndex = 0;

            // Find teams [ teamBlue vs teamRed]
            for (int MatchsInfoListIndex = 0; MatchsInfoListIndex < MatchsInfoList.Count; MatchsInfoListIndex++)
            {

                foreach (var data in dynObj.team)
                {
                    this.teamName = data.teamName;
                    this.teamArgonym = data.teamArgonym;
                    this.teamID = data.identifier;

                    string[] matchdentifier = MatchsInfoList[MatchsInfoListIndex].Split(';');

                    foreach (var datastruct in dynObjStructTable.dataStruct) {
                        if (datastruct.tournamentMatch_identifier == matchdentifier[1] & datastruct.team_identifier == this.teamID) {
                            // Get TeamBlue
                                if (teamIndex == 0) {
                                    this.teamBlueName = this.teamName;
                                    this.teamBlueID = this.teamID;
                                    teamIndex++;
                                    break;
                                }
                            // Get TeamRed
                                else if (teamIndex == 1) {
                                    this.teamRedName = this.teamName;
                                    this.teamRedID = this.teamID;
                                    teamIndex = 0;
                                    break;
                                }
                            }
                        }
                    }

                string[] matchinfo = MatchsInfoList[MatchsInfoListIndex].Split(';');
                this.matchDate = matchinfo[2];
                this.matchHash = matchinfo[0];

                Matchs.Add(teamInfo);
            }
        }

        public void TeamPerformanceData(int MatchsInfoListPosition)
        {
            teamBluePerformanceList.Clear();
            teamRedPerformanceList.Clear();

            dynamic dynObj = JsonConvert.DeserializeObject(teamPerformances);
            string[] matchdata = Matchs[MatchsInfoListPosition].Split(';');

            int teamsFounded = 0;

            foreach (var data in dynObj.teamperformance)
            {
                if (data.matchHash == matchdata[1] & data.team_identifier == matchdata[2]) {
                    this.firstBlood = data.firstBlood;
                    this.firstTower = data.firstTower;
                    this.firstInhibitor = data.firstInhibitor;
                    this.firstBaron = data.firstBaron;
                    this.firstDragon = data.firstDragon;
                    this.firstRiftHerald = data.firstRiftHerald;
                    this.towerTakedowns = data.towerTakedowns;
                    this.inhibitorTakedowns = data.inhibitorTakedowns;
                    this.baronKills = data.baronKills;
                    this.dragonKills = data.dragonKills;
                    this.riftHeraldKills = data.riftHeraldKills;

                    teamBluePerformanceList.Add(this.firstBlood.ToString());
                    teamBluePerformanceList.Add(this.firstTower.ToString());
                    teamBluePerformanceList.Add(this.firstInhibitor.ToString());
                    teamBluePerformanceList.Add(this.firstBaron.ToString());
                    teamBluePerformanceList.Add(this.firstDragon.ToString());
                    teamBluePerformanceList.Add(this.firstRiftHerald.ToString());
                    teamBluePerformanceList.Add(this.towerTakedowns.ToString());
                    teamBluePerformanceList.Add(this.inhibitorTakedowns.ToString());
                    teamBluePerformanceList.Add(this.baronKills.ToString());
                    teamBluePerformanceList.Add(this.dragonKills.ToString());
                    teamBluePerformanceList.Add(this.riftHeraldKills.ToString());

                    teamsFounded++;

                } else if (data.matchHash == matchdata[1] & data.team_identifier == matchdata[3]) {
                    this.firstBlood = data.firstBlood;
                    this.firstTower = data.firstTower;
                    this.firstInhibitor = data.firstInhibitor;
                    this.firstBaron = data.firstBaron;
                    this.firstDragon = data.firstDragon;
                    this.firstRiftHerald = data.firstRiftHerald;
                    this.towerTakedowns = data.towerTakedowns;
                    this.inhibitorTakedowns = data.inhibitorTakedowns;
                    this.baronKills = data.baronKills;
                    this.dragonKills = data.dragonKills;
                    this.riftHeraldKills = data.riftHeraldKills;

                    teamRedPerformanceList.Add(this.firstBlood.ToString());
                    teamRedPerformanceList.Add(this.firstTower.ToString());
                    teamRedPerformanceList.Add(this.firstInhibitor.ToString());
                    teamRedPerformanceList.Add(this.firstBaron.ToString());
                    teamRedPerformanceList.Add(this.firstDragon.ToString());
                    teamRedPerformanceList.Add(this.firstRiftHerald.ToString());
                    teamRedPerformanceList.Add(this.towerTakedowns.ToString());
                    teamRedPerformanceList.Add(this.inhibitorTakedowns.ToString());
                    teamRedPerformanceList.Add(this.baronKills.ToString());
                    teamRedPerformanceList.Add(this.dragonKills.ToString());
                    teamRedPerformanceList.Add(this.riftHeraldKills.ToString());

                    teamsFounded++;
                }

                // No need to search anymore
                if(teamsFounded == 2) { break; }
            }
        }

        public List<String> TeamBlueprobability(int MatchsInfoListPosition) {
            dynamic dynObj = JsonConvert.DeserializeObject(teamPerformances);
            string[] matchdata = Matchs[MatchsInfoListPosition].Split(';');

            teamBlueProbabilityList.Clear();

            double teamfirstBlood = 0, teamfirstTower = 0, teamfirstInhibitor = 0, teamfirstBaron = 0, teamfirstDragon = 0, teamfirstRiftHerald = 0;
            double allfirstBlood = 0, allfirstTower = 0, allfirstInhibitor = 0, allfirstBaron = 0, allfirstDragon = 0, allfirstRiftHerald = 0;

            foreach (var data in dynObj.teamperformance)
            {
                if (data.team_identifier == matchdata[2])
                {
                    if (data.firstBlood != 0) { teamfirstBlood++; }
                    if (data.firstTower != 0) { teamfirstTower++; }
                    if (data.firstInhibitor != 0) { teamfirstInhibitor++; }
                    if (data.firstBaron != 0) { teamfirstBaron++; }
                    if (data.firstDragon != 0) { teamfirstDragon++; }
                    if (data.firstRiftHerald != 0) { teamfirstRiftHerald++; }

                }
                else
                {
                    if (data.firstBlood != 0) { allfirstBlood++; }
                    if (data.firstTower != 0) { allfirstTower++; }
                    if (data.firstInhibitor != 0) { allfirstInhibitor++; }
                    if (data.firstBaron != 0) { allfirstBaron++; }
                    if (data.firstDragon != 0) { allfirstDragon++; }
                    if (data.firstRiftHerald != 0) { allfirstRiftHerald++; }
                }
            }

            teamBlueProbabilityList.Add((teamfirstBlood / (teamfirstBlood + allfirstBlood)).ToString());
            teamBlueProbabilityList.Add((teamfirstTower / (teamfirstTower + allfirstTower)).ToString());
            teamBlueProbabilityList.Add((teamfirstInhibitor / (teamfirstInhibitor + allfirstInhibitor)).ToString());
            teamBlueProbabilityList.Add((teamfirstBaron / (teamfirstBaron + allfirstBaron)).ToString());
            teamBlueProbabilityList.Add((teamfirstDragon / (teamfirstDragon + allfirstDragon)).ToString());
            teamBlueProbabilityList.Add((teamfirstRiftHerald / (teamfirstRiftHerald + allfirstRiftHerald)).ToString());

            return teamBlueProbabilityList;
        }

        public List<String> TeamRedprobability(int MatchsInfoListPosition)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(teamPerformances);
            string[] matchdata = Matchs[MatchsInfoListPosition].Split(';');

            teamRedProbabilityList.Clear();

            float teamfirstBlood = 0, teamfirstTower = 0, teamfirstInhibitor = 0, teamfirstBaron = 0, teamfirstDragon = 0, teamfirstRiftHerald = 0;
            float allfirstBlood = 0, allfirstTower = 0, allfirstInhibitor = 0, allfirstBaron = 0, allfirstDragon = 0, allfirstRiftHerald = 0;

            foreach (var data in dynObj.teamperformance)
            {
                if (data.team_identifier == matchdata[3])
                {
                    if (data.firstBlood != 0) { teamfirstBlood++; }
                    if (data.firstTower != 0) { teamfirstTower++; }
                    if (data.firstInhibitor != 0) { teamfirstInhibitor++; }
                    if (data.firstBaron != 0) { teamfirstBaron++; }
                    if (data.firstDragon != 0) { teamfirstDragon++; }
                    if (data.firstRiftHerald != 0) { teamfirstRiftHerald++; }

                }
                else
                {
                    if (data.firstBlood != 0) { allfirstBlood++; }
                    if (data.firstTower != 0) { allfirstTower++; }
                    if (data.firstInhibitor != 0) { allfirstInhibitor++; }
                    if (data.firstBaron != 0) { allfirstBaron++; }
                    if (data.firstDragon != 0) { allfirstDragon++; }
                    if (data.firstRiftHerald != 0) { allfirstRiftHerald++; }
                }
            }

            teamRedProbabilityList.Add((teamfirstBlood / (teamfirstBlood + allfirstBlood)).ToString());
            teamRedProbabilityList.Add((teamfirstTower / (teamfirstTower + allfirstTower)).ToString());
            teamRedProbabilityList.Add((teamfirstInhibitor / (teamfirstInhibitor + allfirstInhibitor)).ToString());
            teamRedProbabilityList.Add((teamfirstBaron / (teamfirstBaron + allfirstBaron)).ToString());
            teamRedProbabilityList.Add((teamfirstDragon / (teamfirstDragon + allfirstDragon)).ToString());
            teamRedProbabilityList.Add((teamfirstRiftHerald / (teamfirstRiftHerald + allfirstRiftHerald)).ToString());

            // Debug only replace this
            return teamRedProbabilityList;
        }

        public List<String> teamBlue()
        {
            return teamBluePerformanceList;
        }

        public List<String> teamRed()
        {
            return teamRedPerformanceList;
        }

        public void PlayerData(string json)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.player)
            {
                this.playerName = data.playerName;
                this.playerID = data.identifier;
            }
        }

        public void PlayerPerformanceData(string json)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.playerperformance)
            {
                this.kills = data.kills;
                this.deaths = data.deaths;
                this.assists = data.assist;
                this.playerID = data.player_identifier;
            }
        }

        public List<String> TournamentList()
        {
            for (int tournamentListIndex = 0; tournamentListIndex < tournamentsInfoList.Count; tournamentListIndex++)
            {
                string[] tournamentTitleName = tournamentsInfoList[tournamentListIndex].Split(';');

                tournamentsList.Add(tournamentTitleName[0]);
            }

            return this.tournamentsList;
        }

        public List<String> MatchsList(int index)
        {
            Matchs.Clear();
            MatchsInfoList.Clear();

            string[] tournamentIdentifier = tournamentsInfoList[index].Split(';');

            MatchData(matchs, tournamentMatchIdentifieTable, tournamentIdentifier[1]);

            return this.Matchs;
        }
    }
}
