using LiteDB;
using System;

namespace DAL.Models
{
    public class ControlPoint
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// numer identyfikacyjny punktu zakodowany w kodzie QR
        /// cała zakodowana w kodzie informacja ma postać CH/x,
        /// kod punktu to wartość x
        /// </summary>
        public int Ident { get; set; }
    }
}
