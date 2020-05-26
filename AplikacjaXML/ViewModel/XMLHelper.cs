using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AplikacjaXML.ViewModel
{
    public class XMLHelper
    {
        public static void ExportDbToXML(string tableName)
        {

            string commandText = "SELECT * FROM " + tableName;

            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {
                OracleCommand command = new OracleCommand(commandText, connection);
                DataSet dataSet = new DataSet(tableName);

                connection.Open();

                OracleDataAdapter dataAdapter = new OracleDataAdapter(command);

                DataTable dataTable = new DataTable(tableName);
                dataAdapter.Fill(dataTable);
                connection.Close();
                connection.Dispose();

                dataSet.Tables.Add(dataTable);

                dataSet.Tables[0].WriteXml(tableName + ".xml");

            }
        }

        public static void ExportXMLToDbClubs(string file)
        {
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);


            int id_klubu = 0;
            string nazwa_klubu = null;
            int punkty = 0;
            int zwyciestwa = 0;
            double wsp_setow = 0.00;
            int miejsce = 0;

            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_klubu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    nazwa_klubu = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                    punkty = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[2]);
                    zwyciestwa = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[3]);
                    wsp_setow = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[4], CultureInfo.InvariantCulture);
                    miejsce = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[5]);
                    string commandText = $@"insert into Kluby (id_klubu, nazwa_klubu, punkty, zwyciestwa, wspolczynnik_setow, miejsce_w_tabeli)  values({id_klubu}, '{nazwa_klubu}', {punkty}, {zwyciestwa}, {wsp_setow.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, {miejsce})";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbGames(string file)
        {
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);


            int id_kolejki = 0;
            int numer_kolejki = 0;


            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_kolejki = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    numer_kolejki = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);

                    string commandText = $@"insert into Kolejka (id_kolejki, numer_kolejki)  values({id_kolejki}, {numer_kolejki})";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbGyms(string file)
        {
            List<int> foreignKeys = new List<int>();
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);

            foreignKeys = GetIds("Kluby_id_klubu", "Hale_sportowe");

            int id_hali = 0;
            int Kluby_id_klubu = 0;
            string nazwa_hali = null;
            int liczba_miejsc = 0;


            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_hali = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    Kluby_id_klubu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);
                    if (foreignKeys.Contains(Kluby_id_klubu))
                    {
                        continue;
                    }
                    nazwa_hali = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();
                    liczba_miejsc = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[3]);

                    string commandText = $@"insert into Hale_sportowe (id_hali, Kluby_id_klubu, nazwa_hali, liczba_miejsc)  values({id_hali}, {Kluby_id_klubu}, '{nazwa_hali}', {liczba_miejsc})";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbGymStuffs(string file)
        {
            List<int> foreignKeys = new List<int>();
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);

            foreignKeys = GetIds("Hale_sportowe_id_hali", "Zarzad_hali");


            int id_zarzadu = 0;
            int Hale_sportowe_id_hali = 0;
            string wozny = null;
            string kierownik = null;


            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_zarzadu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    Hale_sportowe_id_hali = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);
                    if (foreignKeys.Contains(Hale_sportowe_id_hali))
                    {
                        continue;
                    }
                    wozny = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();
                    kierownik = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();

                    string commandText = $@"insert into Zarzad_hali (id_zarzadu, Hale_sportowe_id_hali, wozny, kierownik)  values({id_zarzadu}, {Hale_sportowe_id_hali}, '{wozny}', '{kierownik}')";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbSponsors(string file)
        {
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);


            int id_sponsora = 0;
            int Kluby_id_klubu = 0;
            string nazwa_sponsora = null;
            int wklad_pieniezny;


            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_sponsora = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    Kluby_id_klubu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);
                    nazwa_sponsora = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();
                    wklad_pieniezny = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[3]);

                    string commandText = $@"insert into Sponsorzy (id_sponsora, Kluby_id_klubu, nazwa_sponsora, wklad_pieniezny)  values({id_sponsora}, {Kluby_id_klubu}, '{nazwa_sponsora}', {wklad_pieniezny})";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbClubStuffs(string file)
        {
            List<int> foreignKeys = new List<int>();
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);

            foreignKeys = GetIds("Kluby_id_klubu", "Sztaby_druzyn");


            int id_sztabu = 0;
            int Kluby_id_klubu = 0;
            string trener = null;
            string drugi_trener = null;
            string prezes = null;
            string menadzer = null;
            string dyrektor_sportowy = null;

            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_sztabu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    Kluby_id_klubu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);
                    if (foreignKeys.Contains(Kluby_id_klubu))
                    {
                        continue;
                    }
                    trener = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();
                    drugi_trener = dataSet.Tables[0].Rows[i].ItemArray[3].ToString();
                    prezes = dataSet.Tables[0].Rows[i].ItemArray[4].ToString();
                    menadzer = dataSet.Tables[0].Rows[i].ItemArray[5].ToString();
                    dyrektor_sportowy = dataSet.Tables[0].Rows[i].ItemArray[6].ToString();
                    string commandText = $@"insert into Sztaby_druzyn (id_sztabu, Kluby_id_klubu, trener, drugi_trener, prezes, menadzer, dyrektor_sportowy)  values({id_sztabu}, {Kluby_id_klubu}, '{trener}', '{drugi_trener}', '{prezes}', '{menadzer}', '{dyrektor_sportowy}')";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbMatches(string file)
        {
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);

            int id_meczu = 0;
            int Kolejka_id_kolejki = 0;
            int Kluby_id_klubu = 0;
            int id_gosc = 0;
            string data_i_godzina = null;
            string wynik = null;

            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_meczu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    id_gosc = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);
                    data_i_godzina = DateTime.Parse(dataSet.Tables[0].Rows[i].ItemArray[2].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    Kluby_id_klubu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[3]);
                    wynik = dataSet.Tables[0].Rows[i].ItemArray[4].ToString();
                    Kolejka_id_kolejki = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[5]);
                    string commandText = $@"insert into Mecze (id_meczu, Kolejka_id_kolejki, Kluby_id_klubu, id_gosc, data_i_godzina, wynik)  values({id_meczu}, {Kolejka_id_kolejki}, {Kluby_id_klubu}, {id_gosc}, '{data_i_godzina}', '{wynik}')";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbPlayers(string file)
        {
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);


            int id_zawodnika = 0;
            string imie = null;
            string nazwisko = null;
            string data_urodzenia = null;
            int Kluby_id_klubu = 0;
            int wzrost = 0;
            int waga = 0;
            string pozycja = null;
            int numer = 0;
            int zasieg = 0;


            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_zawodnika = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    imie = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                    nazwisko = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();
                    data_urodzenia = DateTime.Parse(dataSet.Tables[0].Rows[i].ItemArray[3].ToString()).ToString("dd/MM/yyyy");
                    Kluby_id_klubu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[4]);
                    wzrost = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[5]);
                    waga = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[6]);
                    pozycja = dataSet.Tables[0].Rows[i].ItemArray[7].ToString();
                    numer = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[8]);
                    zasieg = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[9]);
                    string commandText = $@"insert into Zawodnicy (id_zawodnika, imie, nazwisko, data_urodzenia, Kluby_id_klubu, wzrost, waga, pozycja, numer, zasieg)  values({id_zawodnika}, '{imie}', '{nazwisko}', TO_DATE('{data_urodzenia}','DD/MM/YYYY'), {Kluby_id_klubu}, {wzrost}, {waga}, '{pozycja}', {numer}, {zasieg})";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbAddresses(string file)
        {
            List<int> foreignKeys = new List<int>();
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);

            foreignKeys = GetIds("Kluby_id_klubu", "Adresy");

            int id_adresu = 0;
            int Kluby_id_klubu = 0;
            string kod_pocztowy = null;
            string ulica = null;
            string miejscowosc = null;
            int nr_budynku = 0;
            int nr_lokalu = 0;
            string strona_internetowa = null;
            string email = null;
            string telefon = null;


            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_adresu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    Kluby_id_klubu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);
                    if (foreignKeys.Contains(Kluby_id_klubu))
                    {
                        continue;
                    }
                    kod_pocztowy = dataSet.Tables[0].Rows[i].ItemArray[2].ToString();
                    ulica = dataSet.Tables[0].Rows[i].ItemArray[3].ToString();
                    miejscowosc = dataSet.Tables[0].Rows[i].ItemArray[4].ToString();
                    nr_budynku = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[5]);
                    nr_lokalu = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[6]);
                    strona_internetowa = dataSet.Tables[0].Rows[i].ItemArray[7].ToString();
                    email = dataSet.Tables[0].Rows[i].ItemArray[8].ToString();
                    telefon = dataSet.Tables[0].Rows[i].ItemArray[9].ToString();
                    string commandText = $@"insert into Adresy (id_adresu, Kluby_id_klubu, kod_pocztowy, ulica, miejscowosc, nr_budynku, nr_lokalu, strona_internetowa, email, telefon)  values({id_adresu}, {Kluby_id_klubu}, '{kod_pocztowy}', '{ulica}', '{miejscowosc}', {nr_budynku}, {nr_lokalu}, '{strona_internetowa}', '{email}', '{telefon}')";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        public static void ExportXMLToDbPlayerStatistics(string file)
        {
            List<int> foreignKeys = new List<int>();
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            DataSet dataSet = new DataSet();
            XmlReader xmlReader = XmlReader.Create(file, new XmlReaderSettings());
            dataSet.ReadXml(xmlReader);

            foreignKeys = GetIds("Zawodnicy_id_zawodnika", "Statystyki");


            int id_statystyki = 0;
            int Zawodnicy_id_zawodnika = 0;
            int liczba_sezonow = 0;
            int liczba_meczow = 0;
            int punkty = 0;
            int asy_serwisowe = 0;
            int bloki = 0;
            double skutecznosc = 0.00;



            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    id_statystyki = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]);
                    Zawodnicy_id_zawodnika = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[1]);
                    if (foreignKeys.Contains(Zawodnicy_id_zawodnika))
                        liczba_sezonow = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[2]);
                    liczba_meczow = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[3]);
                    punkty = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[4]);
                    asy_serwisowe = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[5]);
                    bloki = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[6]);
                    skutecznosc = Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[7], CultureInfo.InvariantCulture);
                    string commandText = $@"insert into Statystyki (id_statystyki, Zawodnicy_id_zawodnika, liczba_sezonow, liczba_meczow, punkty, asy_serwisowe, bloki, skutecznosc)  values({id_statystyki}, {Zawodnicy_id_zawodnika}, {liczba_sezonow}, {liczba_meczow}, {punkty}, {asy_serwisowe}, {bloki}, {skutecznosc.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)})";

                    OracleCommand command = new OracleCommand(commandText, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }

            }
        }

        private static List<int> GetIds(string columnName, string tableName)
        {
            List<int> ids = new List<int>();
            using (OracleConnection connection = new OracleConnection(OracleConnectionElements.connectionString))
            {
                OracleCommand command = connection.CreateCommand();
                command.CommandText = "SELECT " + columnName + " FROM " + tableName;
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ids.Add(Convert.ToInt32(reader[columnName]));
                }
                connection.Close();
            }

            return ids;
        }
    }
}
