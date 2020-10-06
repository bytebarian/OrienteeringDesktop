/*      Orienteering software
 * 
 * author:  Mariusz Dobrowolski
 * created: 08.2013
 * 
 * modifications:
 * 
 * */

using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Models
{
    /// <summary>
    /// Klasa zawierająca wszystkie metody potrzebne do
    /// zarządzania zawodami
    /// <example>
    /// przykład pokazuje obsługę klasy ContestRepository
    /// <code>
    /// ContestRepository model = new ContestRepository("C\\ProgramFiles\\Orienteering\\Database.sdf");
    /// </code>
    /// </example>
    /// </summary>
    public class ContestRepository : BaseRepository
    {
        #region Constructors

        /// <summary>
        /// Kontruktor klasy ContestRepository.
        /// Jedynym dopuszczalnym konstruktorem dla tej klasy jest
        /// konstruktor parametrowy, w którym należy podać
        /// connection string ze ścieżką do lokalnej bazy danych
        /// </summary>
        /// <param name="aConnectionString">ścieżka do lokalnej bazy danych w formacie .sdf</param>
        public ContestRepository(string aConnectionString)
            : base(aConnectionString)
        {

        }

        #endregion

        #region Public Methodes

        /// <summary>
        /// Metoda pozwala na pobranie wszystkich obiektów zawodów,
        /// które nie zostały jescze zakończone
        /// </summary>
        /// <returns>lista obiektów zawodów</returns>
        public List<Contest> GetAllActiveContests()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<Contest>("contests")
                    .Query()
                    .Where(c => c.IsOpen)
                    .ToList();
            }
        }

        /// <summary>
        /// Metoda pozwala na dodanie uczestników do zawodów
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <param name="aUsers">lista uczestników zawodów</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddUsersToContest(Contest aContest, IEnumerable<User> aUsers)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<Contest>("contests");

                var contest = collection
                    .FindById(aContest.Id);
                contest.UsersList = aUsers.ToList();

                return collection.Update(contest);
            }
        }

        /// <summary>
        /// Metoda pozwala zapisanie na trasy zawodów
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <param name="aControlPoints">lista punktów kontrolnych</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AddControlPointsToContest(Contest aContest, IEnumerable<ControlPoint> aControlPoints)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<Contest>("contests");

                var contest = collection
                    .FindById(aContest.Id);
                contest.ControlPoints = aControlPoints.ToList();

                return collection.Update(contest);
            }
        }

        /// <summary>
        /// Metoda pozwala na zapis rozpoczętych zawodów do bazy danych
        /// obiekt zawodów zostaje zapisany ze statusem "Open", w bazie
        /// przechowywane są na bierząco dane odnośnie zawodów,
        /// aby w wypadku awarii programu możliwe było odtworzenie stanu poprzedniego
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool SaveContest(Contest aContest)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var result = db.GetCollection<Contest>("contests").Insert(aContest);
                return !result.IsNull;
            }
        }

        /// <summary>
        /// Metoda pozwala na zakończenie danych zawodów
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool CloseContest(Contest aContest)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<Contest>("contests");

                var contest = collection.FindById(aContest.Id);
                contest.IsOpen = false;

                return collection.Update(contest);
            }
        }

        /// <summary>
        /// Metoda pozwala na nadanie miejsc poszczególnym uczestnikom zawodów
        /// zgodnie z ich czasami biegu
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <param name="aUsers">lista uczestników</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool AllocateRankToUsers(Contest aContest, IEnumerable<User> aUsers)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var contests = db.GetCollection<Contest>("contests");

                var contest = contests
                    .FindById(aContest.Id);
                contest.UsersList.ForEach(u =>
                {
                    var user = aUsers.First(x => x.Id == u.Id);
                    u.Rank = user.Rank;
                });

                return contests.Update(contest);
            }
        }

        /// <summary>
        /// Metoda pozwala na zapisaniu w bazie danych
        /// czasu danego uczestnika w jakim ukończył on swój bieg
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <param name="aUser">uczestnik zawodów</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool SaveUserTime(Contest aContest, User aUser)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var contests = db.GetCollection<Contest>("contests");

                var contest = contests
                    .FindById(aContest.Id);
                contest.UsersList.First(u => u.Id == aUser.Id).Time = aUser.Time; 

                return contests.Update(contest);
            }
        }

        /// <summary>
        /// Metoda pozwala na zapisanie w bazie danych
        /// statusu ukończenia biegu uczestnika
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <param name="aUser">uczestnik zawodów</param>
        /// <returns>metoda zwraca wartość logiczną:
        /// true - jeżeli operacja się powiodła
        /// false - jeżeli operacja zakończyła się niepowodzeniem</returns>
        public bool SaveUserRunStatus(Contest aContest, User aUser)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var contests = db.GetCollection<Contest>("contests");

                var contest = contests
                    .FindById(aContest.Id);
                contest.UsersList.First(u => u.Id == aUser.Id).Status = aUser.Status;

                return contests.Update(contest);
            }
        }

        /// <summary>
        /// Metoda zwraca uczestnikó danych zawodów
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <returns>lista uczestników zawodów</returns>
        public IEnumerable<User> GetContestParticipants(Contest aContest)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<Contest>("contests")
                    .FindById(aContest.Id)
                    .UsersList;
            }
        }

        /// <summary>
        /// Metoda zwraca punkty kontrolne na trasie danych zawodów
        /// </summary>
        /// <param name="aContest">obiekt zawodów</param>
        /// <returns>lista punktów kontrolnych na trasie zawodów</returns>
        public IEnumerable<ControlPoint> GetContestControlPoints(Contest aContest)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                return db.GetCollection<Contest>("contests")
                    .FindById(aContest.Id)
                    .ControlPoints;
            }
        }

        #endregion
    }
}
