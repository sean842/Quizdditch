using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleProject.Shared.Models.Answers
{
    public class GetAnswerClass
    {
        public int id { get; set; }
        public int question_id { get; set; }
        public bool is_correct { get; set; }
        public string content { get; set; }
        public bool imag_or_text { get; set; }
    }
}


