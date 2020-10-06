using LiteDB;
using System;

namespace DAL.Models
{
    /// <summary>
    /// Enumerator stanu biegu
    /// </summary>
    public enum RunStatus
    {
        InProgress,
        Correct,
        WrongPath,
        Cheat
    }

    public class User
    {
        [BsonId]
        public Guid Id { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public string Info { get; set; }

		public bool IsActive { get; set; }

        /// <summary>
        /// Numer startowy danego uczestnika zawodów
        /// </summary>
        public int Identifier { get; set; }
        /// <summary>
        /// Ranking uczestnika zawodów,
        /// jego pozycja w rankingu
        /// liczona wg. czasów biegu
        /// </summary>
        public int? Rank { get; set; }
        /// <summary>
        /// Czas w jakim uczestnik przebiegł wyznaczoną trasę
        /// </summary>
        public TimeSpan Time { get; set; }
        /// <summary>
        /// Status zakończonego przez uczestnika biegu
        /// </summary>
        public RunStatus Status { get; set; }

        public string FullName
        {
            get { return this.Name + " " + this.Surname; }
        }
    }
}
