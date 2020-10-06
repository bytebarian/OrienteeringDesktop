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

namespace DAL.Models
{
    /// <summary>
    /// Klasa zawierająca wszystkie metody potrzebne do
    /// zarządzania kodami punktów kontrolnych w bazie danych
    /// <example>
    /// przykład pokazuje obsługę klasy QRCodeRepository
    /// <code>
    /// QRCodeRepository model = new QRCodeRepository("C\\ProgramFiles\\Orienteering\\Database.sdf");
    /// 
    /// foreach(var item in model.GetAllControlPoints())
    /// {
    ///     // Porcess data 
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class QRCodeRepository : BaseRepository
    {
        #region Constructors

        /// <summary>
        /// Kontruktor klasy QRCodeRepository.
        /// Jedynym dopuszczalnym konstruktorem dla tej klasy jest
        /// konstruktor parametrowy, w którym należy podać
        /// connection string ze ścieżką do lokalnej bazy danych
        /// </summary>
        /// <param name="aConnectionString">ścieżka do lokalnej bazy danych w formacie .sdf</param>
        public QRCodeRepository(string aConnectionString)
            : base(aConnectionString)
        {

        }

        #endregion

        #region Public Methodes

        /// <summary>
        /// Metoda pobiera wszystkie kody QR z bazy danych
        /// </summary>
        /// <returns>lista punktów kontrolnych / kodów QR</returns>
        public IEnumerable<ControlPoint> GetAllControlPoints()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<ControlPoint>("controlpoints").Query().ToList();
            }
        }

        /// <summary>
        /// Metoda pozwala na dodanie nowego punktu kontrolnego / kodu QR
        /// do bazy danych
        /// </summary>
        /// <param name="aControlPoint">obiekt nowego punktu kontrolnego / kodu QR</param>
        ///<returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddNewControlPoint(ControlPoint aControlPoint)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var result = db.GetCollection<ControlPoint>("controlpoints").Insert(aControlPoint);
                return !result.IsNull;
            }
        }

        /// <summary>
        /// Metoda pozwala na usunięcie punktu kontrolnego / kodu QR
        /// z bazy danych
        /// </summary>
        /// <param name="aControlPoint">obiekt punktu kontrolnego / kodu QR do usunięcia</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool DeleteControlPoint(ControlPoint aControlPoint)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<ControlPoint>("controlpoints").Delete(new BsonValue(aControlPoint.Id));
            }
        }

        /// <summary>
        /// Metoda pozwala określić czy w bazie danych
        /// znajduje się już punkt kontrolny, który
        /// zawiera ten sam zakodowany ciąg znaków
        /// </summary>
        /// <param name="aText">ciąg znaków do zakodowania</param>
        /// <returns>true - jeżeli w bazie istnienieje już kod o tym samym zakodowanym ciągu znaków co przekazany w parametrze
        /// false - jeżeli w bazie nie ma takiego kodu</returns>
        public bool IsAnotherCodeWithSameText(string aText)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ControlPoint>("controlpoints");
                collection.EnsureIndex(x => x.Text);
                return collection.FindOne(x => x.Text.Equals(aText)) != null;
            }
        }

        #endregion
    }
}
