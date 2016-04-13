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
using System.Windows.Shapes;

namespace IMGLMM
{
    /// <summary>
    /// Interaction logic for matchInfo.xaml
    /// </summary>
    public partial class matchInfo : Window
    {
        private LolData data;
        private List<String> teamBluePerformanceList = new List<String>();
        private List<String> teamRedPerformanceList = new List<String>();

        private List<String> teamBlueProbabilityList = new List<String>();
        private List<String> teamRedProbabilityList = new List<String>();

        private List<String> teamBlueRoster = new List<String>();
        private List<String> teamBluePlayerNames = new List<String>();
        private List<String> teamBluePlayerKills = new List<String>();
        private List<String> teamBluePlayerDeaths = new List<String>();
        private List<String> teamBluePlayerAssists = new List<String>();

        private List<String> teamRedRoster = new List<String>();
        private List<String> teamRedPlayerNames = new List<String>();
        private List<String> teamRedPlayerKills = new List<String>();
        private List<String> teamRedPlayerDeaths = new List<String>();
        private List<String> teamRedPlayerAssists = new List<String>();
        private int teamBlueKillsAmount, teamRedKillsAmount, teamBlueDeathsAmount, teamRedDeathsAmount;

        public matchInfo(LolData data)
        {
            
            InitializeComponent();

            this.data = data;
            teamBluePerformanceList.Clear();
            teamRedPerformanceList.Clear();

            teamBlueProbabilityList.Clear();
            teamRedProbabilityList.Clear();

            teamBlueRoster.Clear();
            teamBluePlayerNames.Clear();
            teamBluePlayerKills.Clear();
            teamBluePlayerDeaths.Clear();
            teamBluePlayerAssists.Clear();

            teamRedRoster.Clear();
            teamRedPlayerNames.Clear();
            teamRedPlayerKills.Clear();
            teamRedPlayerDeaths.Clear();
            teamRedPlayerAssists.Clear();

            teamBluePlayerListview.ItemsSource = null;
            teamBlueKillsListview.ItemsSource = null;
            teamBlueDeathsListview.ItemsSource = null;
            teamBlueAssistsListview.ItemsSource = null;

            teamRedPlayerListview.ItemsSource = null;
            teamRedKillsListview.ItemsSource = null;
            teamRedDeathsListview.ItemsSource = null;
            teamRedAssistsListview.ItemsSource = null;

            teamBluePerformanceList = data.teamBlue();
            teamRedPerformanceList = data.teamRed();

            teamBlueProbabilityList = data.teamBlueProbability();
            teamRedProbabilityList = data.teamRedProbability();

            teamBlueRoster = data.teamBlueStats();
            teamRedRoster = data.teamRedStats();

            teamBlueKillsAmount = 0;
            teamRedKillsAmount = 0;

            teamBlueDeathsAmount = 0;
            teamRedDeathsAmount = 0;

            MatchTitle.Text = data.tournamentMatchTitle;

            try
            {
                teamBlueName.Text = teamBluePerformanceList[0];
                if (teamBluePerformanceList[1].Contains('1')) { teamBlueFirstBlood.IsChecked = true; }
                if (teamBluePerformanceList[2].Contains('1')) { teamBlueFirstTower.IsChecked = true; }
                if (teamBluePerformanceList[3].Contains('1')) { teamBlueFirstInhibitor.IsChecked = true; }
                if (teamBluePerformanceList[4].Contains('1')) { teamBlueFirstBaron.IsChecked = true; }
                if (teamBluePerformanceList[5].Contains('1')) { teamBlueFirstDragon.IsChecked = true; }
                if (teamBluePerformanceList[6].Contains('1')) { teamBlueFirstRiftHerald.IsChecked = true; }
                teamBlueTowers.Text = teamBluePerformanceList[7];
                teamBlueInhibitors.Text = teamBluePerformanceList[8];
                teamBlueBarons.Text = teamBluePerformanceList[9];
                teamBlueDragons.Text = teamBluePerformanceList[10];
                teamBlueRiftHerals.Text = teamBluePerformanceList[11];
            }
            catch (Exception)
            {

                teamBlueName.Text = "unknown";
                teamBlueTowers.Text = "0";
                teamBlueInhibitors.Text = "0";
                teamBlueBarons.Text = "0";
                teamBlueDragons.Text = "0";
                teamBlueRiftHerals.Text = "0";
            }

            try
            {
                if (teamRedPerformanceList[1].Contains('1')) { teamRedFirstBlood.IsChecked = true; }
                if (teamRedPerformanceList[2].Contains('1')) { teamRedFirstTower.IsChecked = true; }
                if (teamRedPerformanceList[3].Contains('1')) { teamRedFirstInhibitor.IsChecked = true; }
                if (teamRedPerformanceList[4].Contains('1')) { teamRedFirstBaron.IsChecked = true; }
                if (teamRedPerformanceList[5].Contains('1')) { teamRedFirstDragon.IsChecked = true; }
                if (teamRedPerformanceList[6].Contains('1')) { teamRedFirstRiftHerald.IsChecked = true; }
                teamRedName.Text = teamRedPerformanceList[0];
                teamRedTowers.Text = teamRedPerformanceList[7];
                teamRedInhibitors.Text = teamRedPerformanceList[8];
                teamRedBarons.Text = teamRedPerformanceList[9];
                teamRedDragons.Text = teamRedPerformanceList[10];
                teamRedRiftHerals.Text = teamRedPerformanceList[11];
            }
            catch (Exception)
            {

                teamRedName.Text = "unknown";
                teamRedTowers.Text = "0";
                teamRedInhibitors.Text = "0";
                teamRedBarons.Text = "0";
                teamRedDragons.Text = "0";
                teamRedRiftHerals.Text = "0";
            }

            try
            {
                teamBlueFirstBloodProbability.Text = teamBlueProbabilityList[0];
                teamBlueFirstTowerProbability.Text = teamBlueProbabilityList[1];
                teamBlueFirstInhibitorProbability.Text = teamBlueProbabilityList[2];
                teamBlueFirstBaronProbability.Text = teamBlueProbabilityList[3];
                teamBlueFirstDragonProbability.Text = teamBlueProbabilityList[4];
                teamBlueFirstRiftHeraldProbability.Text = teamBlueProbabilityList[5];
            }
            catch (Exception)
            {

                teamBlueFirstBloodProbability.Text = "";
                teamBlueFirstTowerProbability.Text = "";
                teamBlueFirstInhibitorProbability.Text = "";
                teamBlueFirstBaronProbability.Text = "";
                teamBlueFirstDragonProbability.Text = "";
                teamBlueFirstRiftHeraldProbability.Text = "";
            }

            try
            {
                teamRedFirstBloodProbability.Text = teamRedProbabilityList[0];
                teamRedFirstTowerProbability.Text = teamRedProbabilityList[1];
                teamRedFirstInhibitorProbability.Text = teamRedProbabilityList[2];
                teamRedFirstBaronProbability.Text = teamRedProbabilityList[3];
                teamRedFirstDragonProbability.Text = teamRedProbabilityList[4];
                teamRedFirstRiftHeraldProbability.Text = teamRedProbabilityList[5];
            }
            catch (Exception)
            {

                teamRedFirstBloodProbability.Text = "";
                teamRedFirstTowerProbability.Text = "";
                teamRedFirstInhibitorProbability.Text = "";
                teamRedFirstBaronProbability.Text = "";
                teamRedFirstDragonProbability.Text = "";
                teamRedFirstRiftHeraldProbability.Text = "";
            }

            try
            {
                string[] rosterstats = null;
                for (int i = 0; i < teamBlueRoster.Count; i++)
                {
                    rosterstats = teamBlueRoster[i].Split(';');
                    teamBluePlayerNames.Add(rosterstats[0]);
                    teamBluePlayerKills.Add(rosterstats[1]);
                    teamBluePlayerDeaths.Add(rosterstats[2]);
                    teamBluePlayerAssists.Add(rosterstats[3]);

                    this.teamBlueKillsAmount += int.Parse(rosterstats[1]);
                    this.teamBlueDeathsAmount += int.Parse(rosterstats[2]);
                }

                rosterstats = null;
                for (int i = 0; i < teamRedRoster.Count; i++)
                {
                    rosterstats = teamRedRoster[i].Split(';');
                    teamRedPlayerNames.Add(rosterstats[0]);
                    teamRedPlayerKills.Add(rosterstats[1]);
                    teamRedPlayerDeaths.Add(rosterstats[2]);
                    teamRedPlayerAssists.Add(rosterstats[3]);

                    this.teamRedKillsAmount += int.Parse(rosterstats[1]);
                    this.teamRedDeathsAmount += int.Parse(rosterstats[2]);
                }

                teamBluePlayerListview.ItemsSource = teamBluePlayerNames;
                teamBlueKillsListview.ItemsSource = teamBluePlayerKills;
                teamBlueDeathsListview.ItemsSource = teamBluePlayerDeaths;
                teamBlueAssistsListview.ItemsSource = teamBluePlayerAssists;

                teamRedPlayerListview.ItemsSource = teamRedPlayerNames;
                teamRedKillsListview.ItemsSource = teamRedPlayerKills;
                teamRedDeathsListview.ItemsSource = teamRedPlayerDeaths;
                teamRedAssistsListview.ItemsSource = teamRedPlayerAssists;

                teamBlueKills.Text = teamBlueKillsAmount.ToString();
                teamRedKills.Text = teamRedKillsAmount.ToString();
                teamBlueDeaths.Text = teamBlueDeathsAmount.ToString();
                teamRedDeaths.Text = teamRedDeathsAmount.ToString();
            }
            catch (Exception)
            {

                
            }

        }
    }
}
