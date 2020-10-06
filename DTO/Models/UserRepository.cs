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
    /// zarządzania użytkownikami w bazie danych
    /// <example>
    /// przykład pokazuje obsługę klasy UserRepository
    /// <code>
    /// UserRepository model = new UserRepository("C\\ProgramFiles\\Orienteering\\Database.db");
    /// 
    /// foreach(var item in model.GetAllUsers())
    /// {
    ///     // Porcess data 
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class UserRepository : BaseRepository
    {
        /// <summary>
        /// Kontruktor klasy UserModel.
        /// Jedynym dopuszczalnym konstruktorem dla tej klasy jest
        /// konstruktor parametrowy, w którym należy podać
        /// connection string ze ścieżką do lokalnej bazy danych
        /// </summary>
        /// <param name="aConnectionString">ścieżka do lokalnej bazy danych w formacie .sdf</param>
        public UserRepository(string aConnectionString)
            : base(aConnectionString)
        {

        }

        /// <summary>
        /// Metoda zwraca listę wszystkich użytkowników / zawodników dostępnych w bazie
        /// </summary>
        /// <returns>lista zawodników</returns>
        public IEnumerable<User> GetAllUsers()
        {
            using(var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<User>("users").Query().ToList();
            }
        }

        /// <summary>
        /// Metoda pozwala na pobranie uczestników dla zadanej listy
        /// </summary>
        /// <param name="aUsersList">obiekt listy uczestników</param>
        /// <returns>uczestnicy na danej liście</returns>
        public IEnumerable<User> GetUsersForUsersList(UsersList aUsersList)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<UsersList>("userslists")
                    .FindById(aUsersList.Id)
                    .Users;
            }
        }

        /// <summary>
        /// Metoda zapisuje nowego zawodnika do bazy danych
        /// </summary>
        /// <param name="aUser">obiekt nowego zawodnika</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddNewUser(User aUser)
        {
            using(var db = new LiteDatabase(_connectionString))
            {
                var result = db.GetCollection<User>("users").Insert(aUser);
                return !result.IsNull;
            }
        }

        /// <summary>
        /// Metoda pozwala na usunięcie z bazy wybranego użytkownika
        /// </summary>
        /// <param name="aUser">obiekt użytkownika do usunięcia</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool DeleteUser(User aUser)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<User>("users").Delete(new BsonValue(aUser.Id));
            }
        }

        /// <summary>
        /// Metoda umożliwia pobranie wszystkich
        /// list uczestników z bazy danych
        /// </summary>
        /// <returns>lista list uczestników</returns>
        public IEnumerable<UsersList> GetAllUsersLists()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<UsersList>("userslists").Query().ToList();
            }
        }

        /// <summary>
        /// Metoda pozwala na dodanie nowej listy uczestników
        /// </summary>
        /// <param name="aUsersList">obiekt nowej listy uczestników</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddNewUsersList(UsersList aUsersList)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var result = db.GetCollection<UsersList>("userslists").Insert(aUsersList);
                return !result.IsNull;
            }
        }

        /// <summary>
        /// Metoda pozwala na usunięcie listy użytkowników z bazy
        /// </summary>
        /// <param name="aUsersList">obiekt listy użytkowników do usunięcia</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool DeleteUsersList(UsersList aUsersList)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<UsersList>("userslists").Delete(new BsonValue(aUsersList.Id));
            }
        }

        /// <summary>
        /// Metoda pozwala na dodanie uczestników do listy
        /// </summary>
        /// <param name="aUsersList">lista uczestników</param>
        /// <param name="aUsers">uczestnicy, którzy mają do tej listy zostać dodani</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddUsersToList(UsersList aUsersList, IEnumerable<User> aUsers)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<UsersList>("userslists");

                var list = collection
                    .FindById(aUsersList.Id);
                if (list.Users == null) list.Users = aUsers.ToList();
                else list.Users.AddRange(aUsers);

                return collection.Update(list);
            }
        }
    }
}
