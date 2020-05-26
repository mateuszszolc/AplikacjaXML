using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaXML.ViewModel
{
    public static class SD
    {
        public static List<string> operationTypes { get; } = new List<string>()
        {
            "Eksport do XML",
            "Import do DB"
        };

        public static List<string> tables { get; } = new List<string>() {
            "Kluby",
            "Hale sportowe",
            "Sponsorzy" ,
            "Sztaby drużyn",
            "Adresy",
            "Zawodnicy",
            "Statystyki",
            "Zarząd hali",
            "Kolejka",
            "Mecze"};

        public static string oracleErrorMessage { get; } = "Podano plik dotyczący innej tabeli lub zmieniono dane w pliku XML w taki sposób, iż maruszają teraz więzy unikatowe w bazie danych";
    }
}
