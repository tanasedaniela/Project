using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.ServiceInterfaces.Models.QuestionViewModels
{
    public class QuestionEditModel
    {
        public QuestionEditModel()
        {
            // EF
        }
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        public Guid UserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate = DateTime.Now;
        
        [Required(ErrorMessage = "Answer Text is required.")]
        [MinLength(1, ErrorMessage = "Answer must have at least 1 character.")]
        [MaxLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters.")]
        public string Text { get; set; }

        public QuestionEditModel(Guid id, Guid userId, string text)
        {
            Id = id;
            UserId = userId;
            Text = text;
        }
    }
}
