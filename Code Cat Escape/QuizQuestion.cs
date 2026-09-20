using System;
using System.Collections.Generic;
using System.Text;

namespace Code_Cat_Escape
{
    public class QuizQuestion
    {
        public string Question { get; set; } = "";
        public string Code { get; set; } = "";
        public string[] Answers { get; set; } = Array.Empty<string>();
        public int CorrectAnswer { get; set; }
        public string Category { get; set; } = "";
        public string Explanation { get; set; } = "";
    }
}
