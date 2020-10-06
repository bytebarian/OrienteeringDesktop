using LiteDB;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class UsersList
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public List<User> Users { get; set; }
    }
}
