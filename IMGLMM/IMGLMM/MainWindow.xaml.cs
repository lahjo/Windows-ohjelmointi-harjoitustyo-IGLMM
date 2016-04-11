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


            listView1.ItemsSource = null;
            listView1.ItemsSource = Matchs;
        }
        // }
    }

    public class LolData
    {
        private string date, tournamentMatchIdentifieTable, matchs, teams, teamMatchIdentifieTable;

        private List<String> tournamentsList = new List<String>();
        private List<String> tournamentsInfoList = new List<String>();

        private List<String> Matchs = new List<String>();
        private List<String> MatchsInfoList = new List<String>();

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
            get { return teamBlueName + " vs " + teamRedName + " - " + matchDate; }
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
                var teamPerformances = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/teamperformance.php");

                teamPerformances = @"{""teamperformance"" :" + teamPerformances + "}";
                TeamPerformanceData(teamPerformances);
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

                /*Console.Write("tournament" + ":" + data.tournamentTitle + "\n");
                Console.Write("ID" + ":" + data.identifier + "\n");*/
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

                /*Console.Write("match hash" + ":" + data.matchHash + "\n");
                Console.Write("match date" + ":" + data.date + "\n");
                Console.Write("ID" + ":" + data.identifier + "\n");*/
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

                string[] matchdate = MatchsInfoList[MatchsInfoListIndex].Split(';');
                this.matchDate = matchdate[2];
                Matchs.Add(teamInfo);

                /*Console.Write("team name" + ":" + data.teamName + "\n");
                Console.Write("team argonym" + ":" + data.teamArgonym + "\n");
                Console.Write("ID" + ":" + data.identifier + "\n");*/
            }
        }

        public void TeamPerformanceData(string json)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.teamperformance)
            {
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
                this.matchHash = data.matchHash;
                this.teamID = data.team_identifier;

                /*Console.Write("firs tBlood" + ":" + data.firstBlood + "\n");
                Console.Write("first Tower" + ":" + data.firstTower + "\n");
                Console.Write("first Inhibitor" + ":" + data.firstInhibitor + "\n");
                Console.Write("first Baron" + ":" + data.firstBaron + "\n");
                Console.Write("first Dragon" + ":" + data.firstDragon + "\n");
                Console.Write("firstRiftHerald" + ":" + data.firstRiftHerald + "\n");
                Console.Write("tower Takedowns" + ":" + data.towerTakedowns + "\n");
                Console.Write("inhibitor Takedowns" + ":" + data.inhibitorTakedowns + "\n");
                Console.Write("baron kills" + ":" + data.baronKills + "\n");
                Console.Write("dragon Kills" + ":" + data.dragonKills + "\n");
                Console.Write("riftHerald Kills" + ":" + data.riftHeraldKills + "\n");
                Console.Write("Match Hash" + ":" + data.matchHash + "\n");
                Console.Write("ID" + ":" + data.team_identifier + "\n");*/
            }
        }

        public void PlayerData(string json)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.player)
            {
                this.playerName = data.playerName;
                this.playerID = data.identifier;

                /*Console.Write("player name" + ":" + data.playerName + "\n");
                Console.Write("ID" + ":" + data.identifier + "\n");*/
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

                /* Console.Write("kills" + ":" + data.kills + "\n");
                 Console.Write("deathsr" + ":" + data.deaths + "\n");
                 Console.Write("assists" + ":" + data.assist + "\n");
                 Console.Write("playerID" + ":" + data.player_identifier + "\n");*/
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

            for (int i = 0; i < Matchs.Count; i++) {
                Console.WriteLine(Matchs[i]);
            }

            return this.Matchs;
            //return this.Matchs;
        }
    }
}
