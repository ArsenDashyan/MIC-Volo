using System;
using System.Collections.Generic;

#nullable disable

namespace ChessGame
{
    public partial class UserDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Opponent { get; set; }

        public virtual GameDetail GameDetail { get; set; }
    }
}
