using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleProject.Shared.Models.Answers;

namespace TriangleProject.Shared.Models.Questions
{
    public class GetQuestionClass
    {
        public int id { get; set; }
        public int game_id { get; set; }
        public bool imag_or_text { get; set; }
        public string content { get; set; }
        public List<GetAnswerClass> answers { get; set; }

    }
}
