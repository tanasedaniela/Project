using Data.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces
{
    public interface IQuestionRepository
    {
        List<Question> GetAllQuestions();
        Question GetQuestionById(Guid id);
        void CreateQuestion(Question question);
        void EditQuestion(Question question);
        void DeleteQuestion(Question question);
        int GetNumberOfQuestions();
    }
}
