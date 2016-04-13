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

        public MainWindow()
        {
            InitializeComponent();
            data.ApiData();

            tournaments = data.TournamentList();

            tournamentView.ItemsSource = null;
            tournamentView.ItemsSource = tournaments;

        }

        private void tournamentView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Matchs = data.MatchsList(tournamentView.SelectedIndex);


            MatchView.ItemsSource = null;
            MatchView.ItemsSource = Matchs;
        }

        private void MatchView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            data.TeamPerformanceData(MatchView.SelectedIndex);

            matchInfo window = new matchInfo(data);
            window.ShowDialog();

            data.TeamBlueprobability(MatchView.SelectedIndex);
            data.TeamRedprobability(MatchView.SelectedIndex);
        }
    }
}
