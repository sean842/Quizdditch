using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleProject.Shared.Models.Questions;

namespace TriangleProject.Shared.Models.Game
{
    public class UnityGame
    {

        public int id { get; set; }
        public string game_name { get; set; }
        public int time_to_answer { get; set; }
        public bool is_publish { get; set; }
        public int game_code { get; set; }
        public List<GetQuestionClass> game_questions { get; set; }


    }
}
