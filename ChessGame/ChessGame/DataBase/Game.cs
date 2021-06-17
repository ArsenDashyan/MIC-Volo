using System;

#nullable disable

namespace ChessGame
{
    public partial class Game
    {
        public int Id { get; set; }
        public string GameCondition { get; set; }
        public string GameStory { get; set; }
        public int UserId { get; set; }
        public DateTime? DateTime { get; set; }
        public virtual User User { get; set; }

        public string CurrentColor { get; set; }
    }
}
