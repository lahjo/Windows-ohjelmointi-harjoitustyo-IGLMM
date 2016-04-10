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
        private List<String> torunaments = new List<String>();
        private List<String> Matchs = new List<String>();
        User user = new User();

        //private IWebDriver driver;

        //private string root = "odds-browser";

        public MainWindow()
        {
            InitializeComponent();

            using (var webClient = new System.Net.WebClient())
            {
                string tournaments = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/tournament.php");

                tournaments = @"{""tournament"" :" + tournaments + "}";

                
                user.TournamentData(tournaments);

            }

            using (var webClient = new System.Net.WebClient())
            {
                var matchs = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/match.php");

                matchs = @"{""match"" :" + matchs + "}";
                user.MatchData(matchs);
            }

            using (var webClient = new System.Net.WebClient())
            {
                var tournamentMatchIdentidieTable = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/tournamentmatch.php");

            }

            using (var webClient = new System.Net.WebClient())
            {
                var teams = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/team.php");

                teams = @"{""team"" :" + teams + "}";
                user.TeamData(teams);
            }

            using (var webClient = new System.Net.WebClient())
            {
                var teamMatchIdentifieTable = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/teammatch.php");

            }

            using (var webClient = new System.Net.WebClient())
            {
                var teamPerformances = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/teamperformance.php");

                teamPerformances = @"{""teamperformance"" :" + teamPerformances + "}";
                user.TeamPerformanceData(teamPerformances);
            }

            using (var webClient = new System.Net.WebClient())
            {
                var players = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/player.php");

                players = @"{""player"" :" + players + "}";
                user.PlayerData(players);
            }

            using (var webClient = new System.Net.WebClient())
            {
                var playerTeamIdentifieTable = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/playerteam.php");

            }

            using (var webClient = new System.Net.WebClient())
            {
                var playerPerformances = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/playerperformance.php");

                playerPerformances = @"{""playerperformance"" :" + playerPerformances + "}";
                user.PlayerPerformanceData(playerPerformances);
            }
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            torunaments = user.TorunamentList();
            Matchs = user.MatchsList();

            listView.ItemsSource = null;
            listView1.ItemsSource = null;

            listView.ItemsSource = torunaments;
            listView1.ItemsSource = Matchs;
        }
        // }
    }

    public class User
    {
        private string date;

        private List<String> torunaments = new List<String>();
        private List<String> Matchs = new List<String>();

        public string tournamentTitle { get; set; }
        public string torunamentHash { get; set; }
        public string torunamentID { get; set; }

        public string matchDate
        {
            get { return date; }
            set { string[] datesplit = value.Split('T'); date = datesplit[0]; }
        }
        public string matchID { get; set; }
        public string matchHash { get; set; }

        public string teamName { get; set; }
        public string teamArgonym { get; set; }
        public string teamID { get; set; }

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

        public void TournamentData(string json) {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.tournament)
            {
                this.tournamentTitle = data.tournamentTitle;
                this.torunamentHash = data.tournamentHash;
                this.torunamentID = data.identifier;

                torunaments.Add(this.tournamentTitle);

                /*Console.Write("tournament" + ":" + data.tournamentTitle + "\n");
                Console.Write("torunamenthash" + ":" + data.tournamentHash + "\n");
                Console.Write("ID" + ":" + data.identifier + "\n");*/
            }
        }

        public void MatchData(string json)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.match)
            {
                this.matchHash = data.matchHash;
                this.matchDate = data.date;
                this.matchID = data.identifier;

                Matchs.Add(this.matchHash);

                /*Console.Write("match hash" + ":" + data.matchHash + "\n");
                Console.Write("match date" + ":" + data.date + "\n");
                Console.Write("ID" + ":" + data.identifier + "\n");*/
            }
        }

        public void TeamData(string json)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(json);

            foreach (var data in dynObj.team)
            {
                this.teamName = data.teamName;
                this.teamArgonym = data.teamArgonym;
                this.teamID = data.identifier;

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

        public List<String> TorunamentList() {
            return this.torunaments;
        }

        public List<String> MatchsList()
        {
            return this.Matchs;
        }
    }
}
