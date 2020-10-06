/*      Orienteering software
 * 
 * author:  Mariusz Dobrowolski
 * created: 08.2013
 * 
 * modifications:
 * 
 * */

using System;

namespace DAL.Models
{
    /// <summary>
    /// Klasa bazowa dla wszystkich pozostałych modeli
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Ścieżka do pliku z lokalną bazą danych
        /// </summary>
        protected string _connectionString;

        /// <summary>
        /// Konstruktor klasy,
        /// aby utworzyć instancję klasy BaseModel
        /// należy zdefiniować ścieżkę pliku z lokalną bazą danych
        /// </summary>
        /// <param name="aConnectionString">Ścieżka do pliku z lokalną bazą danych</param>
        public BaseRepository(string aConnectionString)
        {
            _connectionString = aConnectionString;
        }
    }
}
