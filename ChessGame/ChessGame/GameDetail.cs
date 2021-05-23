using System;
using System.Collections.Generic;

#nullable disable

namespace ChessGame
{
    public partial class GameDetail
    {
        public int GameId { get; set; }
        public string GameCondition { get; set; }
        public string GameStory { get; set; }
        public DateTime DateTime { get; set; }

        public virtual UserDetail Game { get; set; }
    }
}
