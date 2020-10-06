/*      Orienteering software
 * 
 * author:  Mariusz Dobrowolski
 * created: 08.2013
 * 
 * modifications:
 * 
 * */

using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Models
{
    /// <summary>
    /// Klasa zawierająca wszystkie metody potrzebne do
    /// zarządzania trasami w bazie danych
    /// <example>
    /// przykład pokazuje obsługę klasy RouteRepository
    /// <code>
    /// RouteRepository model = new RouteRepository("C\\ProgramFiles\\Orienteering\\Database.db");
    /// 
    /// foreach(var item in model.GetAllRoutes())
    /// {
    ///     // Porcess data 
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class RouteRepository :BaseRepository
    {
        #region Constructors

        /// <summary>
        /// Kontruktor klasy RouteRepository.
        /// Jedynym dopuszczalnym konstruktorem dla tej klasy jest
        /// konstruktor parametrowy, w którym należy podać
        /// connection string ze ścieżką do lokalnej bazy danych
        /// </summary>
        /// <param name="aConnectionString">ścieżka do lokalnej bazy danych w formacie .sdf</param>
        public RouteRepository(string aConnectionString)
            : base(aConnectionString)
        {

        }

        #endregion

        #region Public Methodes

        /// <summary>
        /// Metoda pozwala na pobranie wszystkich tras znajdujących się w bazie danych
        /// </summary>
        /// <returns>lista tras</returns>
        public IEnumerable<Route> GetAllRoutes()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<Route>("routes").Query().ToList();
            }
        }

        /// <summary>
        /// Metoda pozwala na pobranie listy punktów kontrolnych
        /// bez uwzględnienia punktów startowych i końcowych
        /// </summary>
        /// <returns>lista punktów kontrolnych</returns>
        public IEnumerable<ControlPoint> GetControlPoints()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<ControlPoint>("controlpoints").Query().ToList();
            }
        }

        /// <summary>
        /// Metoda pozwala na pobranie listy punktów kontrolnych dla danej trasy
        /// </summary>
        /// <param name="aRoute">obiekt trasy</param>
        /// <returns>lista punktów kontrolnych danej trasy</returns>
        public IEnumerable<ControlPoint> GetRouteControlPoints(Route aRoute)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<Route>("routes")
                    .FindById(aRoute.Id)
                    .ControlPoints;
            }
        }

        /// <summary>
        /// Metoda pozwala na dodanie nowej trasy do bazy danych
        /// </summary>
        /// <param name="aRoute">obiekt nowej trasy</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddNewRoute(Route aRoute)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var result = db.GetCollection<Route>("routes").Insert(aRoute);
                return !result.IsNull;
            }
        }

        /// <summary>
        /// Metoda pozwala na usnięcie trasy z bazy danych
        /// </summary>
        /// <param name="aRoute">obiekt trasy</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool DeleteRoute(Route aRoute)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<Route>("routes").Delete(new BsonValue(aRoute.Id));
            }
        }

        /// <summary>
        /// Metoda pozwala na dodanie listy punktów kontrolnych dla danej trasy
        /// </summary>
        /// <param name="aRoute">obiekt trasy</param>
        /// <param name="aControlPoints">lista punktów kontrolnych</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddControlPointsToRoute(Route aRoute, IEnumerable<ControlPoint> aControlPoints)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<Route>("routes");

                var route = collection
                    .FindById(aRoute.Id);
                if (route.ControlPoints == null) route.ControlPoints = aControlPoints.ToList();
                else route.ControlPoints.AddRange(aControlPoints);

                return collection.Update(route);
            }
        }

        #endregion
    }
}
