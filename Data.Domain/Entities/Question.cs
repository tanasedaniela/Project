using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        public Guid UserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        
        [Required(ErrorMessage = "Answer Text is required.")]
        [MinLength(1, ErrorMessage = "Answer must have at least 1 character.")]
        [MaxLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters.")]
        public string Text { get; set; }

        public IList<Answer> Answers { get; set; }

        public static Question CreateQuestion(Guid userId, string text)
        {
            var question = new Question
            {
                Id = Guid.NewGuid(),
                Answers = new List<Answer>()
            };

            question.UpdateQuestion(userId,text);

            return question;
        }

        private void UpdateQuestion(Guid userId, string text)
        {
            UserId = userId;
            CreatedDate = DateTime.Now;
            Text = text;
        }
    }
}
