using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleProject.Shared.Models.Answers;
using TriangleProject.Shared.Models.Questions;

namespace TriangleProject.Shared.Models.Game
{
    public class Game
    {
        public int ID { get; set; }
        public string GameName { get; set; }
        public int GameCode { get; set; }
        public int TimeToAnswer { get; set; }
        public int UserID { get; set; }
        public bool IsPublish { get; set; }
        public bool CanPublish { get; set; }
        public List<GetQuestionClass> Questions { get; set; }

    }
}
