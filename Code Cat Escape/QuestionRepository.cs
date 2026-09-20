using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Code_Cat_Escape
{
    public class QuestionRepository
    {
        public List<QuizQuestion> Load()
        {
            byte[] data =Properties.Resources.questions;

            string json = Encoding.UTF8.GetString(data);

            List<QuizQuestion>? questions = JsonSerializer.Deserialize<List<QuizQuestion>>(json,
            new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true
            });

            if (questions == null ||questions.Count == 0)
            {
                throw new InvalidOperationException(
                    "No questions were found.");
            }

            return questions;
        }
    }
}
