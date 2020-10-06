using LiteDB;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Route
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public List<ControlPoint> ControlPoints { get; set; }
    }
}
