using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Shouter.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserEmail { get; set; }

        public virtual ICollection<Shout> Shouts { get; set; }
    }

    public class Shout
    {
        public int ShoutID { get; set; }
        public string ShoutText { get; set; }
        public DateTime ShoutCreationTime { get; set; }
        public int UserID { get; set; }

        public virtual User User { get; set; }
    }

    public class ShouterDBContext : DbContext
    {
        public DbSet<Shout> Shouts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}