namespace Orienteering.Helpers
{
    /// <summary>
    /// Klasa zawierająca metody pomocnicze do komunikacji z lokalną bazą danych
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// Ścieżka do lokalnej bazy danych.
        /// Plik Database.sdf znajduję się bezpośrednio w folderze aplikacji,
        /// zależnie od tego, gdzie użytkownik zainstalował aplikacje
        /// </summary>
        internal static string ConnectionString = "OrienteeringDatabase.db";
        /// <summary>
        /// Flaga określa czy plik z bazą danych aplikacji istnieje
        /// i czy jest dostępny z poziomu aplikacji
        /// </summary>
        internal static bool IsSelfTestCompleted = true;
    }
}
