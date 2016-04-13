using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGLMM
{
    public class LolData
    {
        private string date, tournamentMatchIdentifieTable, matchs, teams, teamMatchIdentifieTable, teamPerformances, matchTitle, playerPerformances;
        private int kills, deaths, assists;

        private List<String> tournamentsList = new List<String>();
        private List<String> tournamentsInfoList = new List<String>();

        private List<String> Matchs = new List<String>();
        private List<String> MatchsInfoList = new List<String>();
        private List<String> MatchsListviewList = new List<String>();

        private List<String> teamBluePerformanceList = new List<String>();
        private List<String> teamRedPerformanceList = new List<String>();

        private List<String> teamBlueProbabilityList = new List<String>();
        private List<String> teamRedProbabilityList = new List<String>();

        private string[] matchdata;

        public string tournamentTitle { get; set; }

        public string tournamentInfo
        {
            get { return tournamentTitle + ";" + torunamentID; }
        }

        public string tournamentMatchTitle
        {
            set { matchTitle = value; }
            
            get { return matchTitle; }
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
            get { return teamBlueName + " vs " + teamRedName + " - " + matchDate + ";" + matchHash + ";" + teamBlueID + ";" + teamRedID; }
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
                var playerPerformancesApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/playerperformance.php");

                playerPerformances = @"{""playerperformance"" :" + playerPerformancesApi + "}";
                //PlayerPerformanceData(playerPerformances);
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

                    foreach (var datastruct in dynObjStructTable.dataStruct)
                    {
                        if (datastruct.tournamentMatch_identifier == matchdentifier[1] & datastruct.team_identifier == this.teamID)
                        {
                            // Get TeamBlue
                            if (teamIndex == 0)
                            {
                                this.teamBlueName = this.teamName;
                                this.teamBlueID = this.teamID;
                                teamIndex++;
                                break;
                            }
                            // Get TeamRed
                            else if (teamIndex == 1)
                            {
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
            matchdata = Matchs[MatchsInfoListPosition].Split(';');
            tournamentMatchTitle = matchdata[0];

            string[] teamname = matchdata[0].Split(' ');

            int teamsFounded = 0;

            foreach (var data in dynObj.teamperformance)
            {
                if (data.matchHash == matchdata[1] & data.team_identifier == matchdata[2])
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

                    
                    teamBluePerformanceList.Add(teamname[0]);
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

                }
                else if (data.matchHash == matchdata[1] & data.team_identifier == matchdata[3])
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

                    teamRedPerformanceList.Add(teamname[2]);
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

                    // Players
                    // DATABASE problem
                    /*dynamic dynObjPlayers = JsonConvert.DeserializeObject(playerPerformances);
                    foreach (var playerdata in dynObjPlayers.playerperformance) {
                        if (playerdata.matchHash == matchdata[1]) {
                            kills = 0;
                            deaths = 0;
                            assists = 0;
                            kills =+ playerdata.kills;
                            deaths =+ playerdata.deaths;
                            assists =+ playerdata.assist;
                        }
                            
                    }*/
                }

                // No need to search anymore
                if (teamsFounded == 2) { break; }
            }
        }

        public void TeamBlueprobability(int MatchsInfoListPosition)
        {
            dynamic dynObj = JsonConvert.DeserializeObject(teamPerformances);
            matchdata = Matchs[MatchsInfoListPosition].Split(';');

            teamBlueProbabilityList.Clear();

            float teamfirstBlood = 0, teamfirstTower = 0, teamfirstInhibitor = 0, teamfirstBaron = 0, teamfirstDragon = 0, teamfirstRiftHerald = 0;
            float allfirstBlood = 0, allfirstTower = 0, allfirstInhibitor = 0, allfirstBaron = 0, allfirstDragon = 0, allfirstRiftHerald = 0;

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

            teamBlueProbabilityList.Add(((teamfirstBlood / (teamfirstBlood + allfirstBlood)) * 100).ToString());
            teamBlueProbabilityList.Add(((teamfirstTower / (teamfirstTower + allfirstTower)) * 100).ToString());
            teamBlueProbabilityList.Add(((teamfirstInhibitor / (teamfirstInhibitor + allfirstInhibitor)) * 100).ToString());
            teamBlueProbabilityList.Add(((teamfirstBaron / (teamfirstBaron + allfirstBaron)) * 100).ToString());
            teamBlueProbabilityList.Add(((teamfirstDragon / (teamfirstDragon + allfirstDragon)) * 100).ToString());
            teamBlueProbabilityList.Add(((teamfirstRiftHerald / (teamfirstRiftHerald + allfirstRiftHerald)) * 100).ToString());
        }

        public void TeamRedprobability(int MatchsInfoListPosition)
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

            teamRedProbabilityList.Add(((teamfirstBlood / (teamfirstBlood + allfirstBlood)) * 100).ToString());
            teamRedProbabilityList.Add(((teamfirstTower / (teamfirstTower + allfirstTower)) * 100).ToString());
            teamRedProbabilityList.Add(((teamfirstInhibitor / (teamfirstInhibitor + allfirstInhibitor)) * 100).ToString());
            teamRedProbabilityList.Add(((teamfirstBaron / (teamfirstBaron + allfirstBaron)) * 100).ToString());
            teamRedProbabilityList.Add(((teamfirstDragon / (teamfirstDragon + allfirstDragon)) * 100).ToString());
            teamRedProbabilityList.Add(((teamfirstRiftHerald / (teamfirstRiftHerald + allfirstRiftHerald)) * 100).ToString());
        }

        public List<String> teamBlue()
        {
            return teamBluePerformanceList;
        }

        public List<String> teamRed()
        {
            return teamRedPerformanceList;
        }

        public List<String> teamBlueProbability()
        {
            return teamBlueProbabilityList;
        }

        public List<String> teamRedProbability()
        {
            return teamRedProbabilityList;
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
            MatchsListviewList.Clear();

            string[] tournamentIdentifier = tournamentsInfoList[index].Split(';');

            MatchData(matchs, tournamentMatchIdentifieTable, tournamentIdentifier[1]);

            for (int i = 0; i < Matchs.Count; i++)
            {
                string[] a = Matchs[i].Split(';');
                MatchsListviewList.Add(a[0]);
            }

            return MatchsListviewList;

        }
    }
}
