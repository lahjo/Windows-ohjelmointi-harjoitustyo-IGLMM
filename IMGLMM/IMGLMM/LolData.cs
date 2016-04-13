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
        private string date, tournamentMatchIdentifieTable, matchs, teams, teamMatchIdentifieTable, teamPerformances, matchTitle, playerPerformances, teamPlayerIdentifieTable, player;
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

        private List<String> playerList = new List<String>();
        private List<String> teamBluePlayerList = new List<String>();
        private List<String> teamRedPlayerList = new List<String>();

        private List<String> RedTeamRosterAndStats = new List<String>();
        private List<String> BlueTeamRosterAndStats = new List<String>();


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

        public int kill { get; set; }
        public int deadth { get; set; }
        public int assist { get; set; }

        public string TeamRoster
        {
            get { return playerName + ";" + this.kill + ";" + this.deadth + ";" + this.assist; }
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
                var playerApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/player.php");

                player = @"{""players"" :" + playerApi + "}";
                //PlayerPerformanceData(playerPerformances);
            }

            using (var webClient = new System.Net.WebClient())
            {
                var teamPlayerApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/playerteam.php");

                teamPlayerIdentifieTable = @"{""teamPlayer"" :" + teamPlayerApi + "}";
            }

            using (var webClient = new System.Net.WebClient())
            {
                var playerPerformancesApi = webClient.DownloadString("http://datamining-esportlol.rhcloud.com/api/playerperformance.php");

                playerPerformances = @"{""playerperformance"" :" + playerPerformancesApi + "}";
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
            playerList.Clear();
            RedTeamRosterAndStats.Clear();
            BlueTeamRosterAndStats.Clear();
            teamBluePlayerList.Clear();
            teamRedPlayerList.Clear();


            dynamic dynObj = JsonConvert.DeserializeObject(teamPerformances);
            matchdata = Matchs[MatchsInfoListPosition].Split(';');
            tournamentMatchTitle = matchdata[0];

            string[] teamname = matchdata[0].Split(' ');

            int teamsFounded = 0;

            // get all player whos have this matchID
            // Players
            dynamic dynObjrosters = JsonConvert.DeserializeObject(playerPerformances);

            foreach (var rosterdata in dynObjrosters.playerperformance)
            {
                if (rosterdata.matchHash == matchdata[1])
                {
                    string player = rosterdata.player_identifier;
                    playerList.Add(player);
                }

            }

           dynamic dynObjrosterIdentification = JsonConvert.DeserializeObject(teamPlayerIdentifieTable);

            for(int playerListIndex = 0; playerListIndex < playerList.Count; playerListIndex++) { 
                foreach (var playerRosterdata in dynObjrosterIdentification.teamPlayer)
                {
                    if (playerRosterdata.team_identifier == matchdata[2] & playerRosterdata.player_identifier == playerList[playerListIndex])
                    {
                        string blueplayer = playerList[playerListIndex];
                        teamBluePlayerList.Add(blueplayer);
                    }
                    else if (playerRosterdata.team_identifier == matchdata[3] & playerRosterdata.player_identifier == playerList[playerListIndex]) {
                        string redplayer = playerList[playerListIndex];
                        teamRedPlayerList.Add(redplayer);
                    }

                }
            }

            dynamic dynObjPlayer = JsonConvert.DeserializeObject(player);

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

                    for (int playerListIndex = 0; playerListIndex < teamBluePlayerList.Count; playerListIndex++) { 
                        dynamic dynObjPlayersStats = JsonConvert.DeserializeObject(playerPerformances);

                        foreach (var playerdata in dynObjPlayersStats.playerperformance)
                        {
                            string playerId = playerdata.player_identifier;
                            if (playerdata.matchHash == matchdata[1] & teamBluePlayerList[playerListIndex] == playerId)
                            {
                                foreach (var findBluePlayerName in dynObjPlayer.players)
                                {
                                    if (findBluePlayerName.identifier == teamBluePlayerList[playerListIndex])
                                    {
                                        playerName = findBluePlayerName.playerName;
                                    }
                                }

                                string playerKills = playerdata.kills;
                                string playerDeaths = playerdata.deaths;
                                string playerAssists = playerdata.assist;

                                this.kill = int.Parse(playerKills);
                                this.deadth = int.Parse(playerDeaths);
                                this.assist = int.Parse(playerAssists);

                                BlueTeamRosterAndStats.Add(TeamRoster);
                            }
                        }
                    }
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

                    for (int playerListIndex = 0; playerListIndex < teamRedPlayerList.Count; playerListIndex++)
                    {
                        dynamic dynObjPlayersStats = JsonConvert.DeserializeObject(playerPerformances);

                        foreach (var playerdata in dynObjPlayersStats.playerperformance)
                        {
                            string playerId = playerdata.player_identifier;
                            if (playerdata.matchHash == matchdata[1] & teamRedPlayerList[playerListIndex] == playerId)
                            {
                                foreach (var findRedPlayerName in dynObjPlayer.players)
                                {
                                    if (findRedPlayerName.identifier == teamRedPlayerList[playerListIndex]) {
                                        playerName = findRedPlayerName.playerName;
                                    }
                                }

                                string playerKills = playerdata.kills;
                                string playerDeaths = playerdata.deaths;
                                string playerAssists = playerdata.assist;

                                this.kill = int.Parse(playerKills);
                                this.deadth = int.Parse(playerDeaths);
                                this.assist = int.Parse(playerAssists);
                                RedTeamRosterAndStats.Add(TeamRoster);
                            }
                        }
                    }
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

        public List<String> teamBlueStats()
        {
            return BlueTeamRosterAndStats;
        }

        public List<String> teamRedStats()
        {
            return RedTeamRosterAndStats;
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
                string[] matchlist = Matchs[i].Split(';');
                MatchsListviewList.Add(matchlist[0]);
            }

            return MatchsListviewList;

        }
    }
}
