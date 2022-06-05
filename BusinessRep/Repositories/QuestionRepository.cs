using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessRep.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DatabaseContext _databaseService;

        public QuestionRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }
        
        public List<Question> GetAllQuestions()
        {

            List<Question> rez = _databaseService.Questions.ToList();
            
            foreach (Question quest in rez)
            {
                quest.Text = quest.Text.Decrypt();
            }

            return rez;
            
        }

        public Question GetQuestionById(Guid id)
        {
            Question rez = _databaseService.Questions.SingleOrDefault(question => question.Id == id);

            rez.Text = rez.Text.Decrypt();

            return rez;
        }

        public void CreateQuestion(Question question)
        {
            question.Text = question.Text.Encrypt();

            _databaseService.Questions.Add(question);

            _databaseService.SaveChanges();
        }

        public void EditQuestion(Question question)
        {
            question.Text = question.Text.Encrypt();

            _databaseService.Questions.Update(question);

            _databaseService.SaveChanges();
        }

        public void DeleteQuestion(Question question)
        {
            _databaseService.Questions.Remove(question);

            _databaseService.SaveChanges();

        }

        public int GetNumberOfQuestions()
        {
            return _databaseService.Questions.Count();
        }
    }
}
