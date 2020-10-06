using LiteDB;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Contest
    {
        [BsonId]
        public Guid Id { get; set; }
        public bool IsOpen { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }
        public List<User> UsersList { get; set; }
        public List<ControlPoint> ControlPoints { get; set; }

        public override string ToString()
        {
            return Name + " (" + Date.ToShortDateString() + ")";
        }
    }
}
