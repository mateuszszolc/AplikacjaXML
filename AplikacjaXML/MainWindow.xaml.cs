using AplikacjaXML.ViewModel;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;
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

namespace AplikacjaXML
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string xmlFileName = "";
        public MainWindow()
        {
            InitializeComponent();
            xmlFileTextBox.IsEnabled = false;
            browseButton.IsEnabled = false;
            operationTypeComboBox.ItemsSource = SD.operationTypes;
            tableComboBox.ItemsSource = SD.tables;
            operationTypeComboBox.SelectedIndex = 0;
            tableComboBox.SelectedIndex = 0;
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (operationTypeComboBox.SelectedItem.ToString() == SD.operationTypes[0])
            {
                switch (tableComboBox.SelectedItem.ToString())
                {
                    case "Kluby":
                        XMLHelper.ExportDbToXML("Kluby");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Hale sportowe":
                        XMLHelper.ExportDbToXML("Hale_sportowe");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Sponsorzy":
                        XMLHelper.ExportDbToXML("Sponsorzy");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Sztaby drużyn":
                        XMLHelper.ExportDbToXML("Sztaby_druzyn");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Adresy":
                        XMLHelper.ExportDbToXML("Adresy");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Zawodnicy":
                        XMLHelper.ExportDbToXML("Zawodnicy");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Statystyki":
                        XMLHelper.ExportDbToXML("Statystyki");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Zarząd hali":
                        XMLHelper.ExportDbToXML("Zarzad_hali");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Kolejka":
                        XMLHelper.ExportDbToXML("Kolejka");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Mecze":
                        XMLHelper.ExportDbToXML("Mecze");
                        MessageBox.Show("Eksport do pliku XML zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    default:
                        break;
                }
            }
            else
            {

                switch (tableComboBox.SelectedItem.ToString())
                {
                    case "Kluby":
                        XMLHelper.ExportXMLToDbClubs(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Hale sportowe":
                        XMLHelper.ExportXMLToDbGyms(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Sponsorzy":
                        XMLHelper.ExportXMLToDbSponsors(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Sztaby drużyn":
                        XMLHelper.ExportXMLToDbClubStuffs(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Adresy":
                        XMLHelper.ExportXMLToDbAddresses(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Zawodnicy":
                        XMLHelper.ExportXMLToDbPlayers(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Statystyki":
                        XMLHelper.ExportXMLToDbPlayerStatistics(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Zarząd hali":
                        XMLHelper.ExportXMLToDbGymStuffs(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Kolejka":
                        XMLHelper.ExportXMLToDbGames(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Mecze":
                        XMLHelper.ExportXMLToDbMatches(xmlFileName);
                        MessageBox.Show("Eksport do DB zakończony", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    default:
                        break;
                }
            }
        }
        catch(OracleException)
        {
            MessageBox.Show(SD.oracleErrorMessage, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch(Exception)
        {
            MessageBox.Show("Nie udało się wyeksportować plików", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

}


//XMLHelper.ExportDbToXML("Mecze");
//MessageBox.Show("Eksport zakończony");



private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfileDialog = new OpenFileDialog();
            openfileDialog.Filter = "XML Files(*.xml)| *.xml";
            openfileDialog.Multiselect = false;
            openfileDialog.Title = "Wybierz plik XML";
            openfileDialog.InitialDirectory = Environment.CurrentDirectory;

            if (openfileDialog.ShowDialog() == true)
            {
                xmlFileName = openfileDialog.FileName;
                xmlFileTextBox.Text = xmlFileName;
            }
        }

        private void operationTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(operationTypeComboBox.SelectedItem.ToString() == SD.operationTypes[0])
            {
                xmlFileTextBox.Text = "";
                xmlFileTextBox.IsEnabled = false;
                browseButton.IsEnabled = false;
                generateButton.IsEnabled = true;
            }
            else
            {
                xmlFileTextBox.IsEnabled = true;
                browseButton.IsEnabled = true;
                generateButton.IsEnabled = false;
            }
        }

        private void xmlFileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if((string.IsNullOrWhiteSpace(xmlFileTextBox.Text)) && operationTypeComboBox.SelectedItem.ToString() == SD.operationTypes[1])
            {
                generateButton.IsEnabled = false;
            }
            else
            {
                generateButton.IsEnabled = true;
            }
        }
    }
}
