using System;
using System.Collections.Generic;

#nullable disable

namespace ChessGame
{
    public partial class User
    {
        public User()
        {
            Games = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Opponent { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
