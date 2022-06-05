using Data.Domain.Entities;
using Data.Domain.ServiceInterfaces.Models.QuestionViewModels;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces
{
    public interface IQuestionService
    {
        List<Question> GetAllQuestions();
        List<Answer> GetAllAnswersForQuestion(Guid id);
        Question GetQuestionById(Guid id);
        void CreateQuestion(Question question);
        void EditQuestion(Question question);
        void DeleteQuestion(Question question);
        void CreateQuestion(QuestionCreateModel questionCreateModel);
        bool CheckIfQuestionExists(Guid id);

        int GetNumberOfPagesForQuestions();
        List<Question> Get5QuestionsByIndex(int index);
    }
}
