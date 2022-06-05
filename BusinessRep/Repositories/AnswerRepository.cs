using System;
using System.Collections.Generic;
using System.Linq;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistence;

namespace BusinessRep.Repositories
{
    public class AnswerRepository: IAnswerRepository
    {
        private readonly DatabaseContext _databaseService;

        public AnswerRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Answer> GetAllAnswers()
        {
            List<Answer> rez = _databaseService.Answers.ToList();
            foreach (Answer ans in rez)
            {
                ans.Text = ans.Text.Decrypt();
            }
            return rez;
        }

        public List<Answer> GetAllAnswersForGivenQuestion(Guid qid)
        {
            List<Answer> rez = _databaseService.Answers.Where(answer => answer.QuestionId == qid).OrderBy(x=>x.Created_Date).ToList();
            foreach (Answer ans in rez)
            {
                ans.Text = ans.Text.Decrypt();
            }
            return rez;
            
        }

        public List<Answer> GetAllAnswersForGivenUserId(Guid uid)
        {
            List < Answer > rez = _databaseService.Answers.Where(answer => answer.UserId == uid).ToList();
            foreach( Answer ans in rez)
            {
                ans.Text = ans.Text.Decrypt();
            }
            return rez;
        }

        public Answer GetAnswerById(Guid id)
        {
            Answer rez = _databaseService.Answers.SingleOrDefault(answer => answer.Id == id);

            rez.Text = rez.Text.Decrypt();

            return rez;
        }

        public void CreateAnswer(Answer answer)
        {
            answer.Text = answer.Text.Encrypt();

            _databaseService.Answers.Add(answer);

            _databaseService.SaveChanges();
        }

        public void CreateAnswerForGivenQuestion(Guid qid, Answer answer)
        {
            answer.QuestionId = qid;

            answer.Text = answer.Text.Encrypt();

            _databaseService.Answers.Add(answer);

            _databaseService.SaveChanges();
        }

        public void EditAnswer(Answer answer)
        {

            answer.Text = answer.Text.Encrypt();

            _databaseService.Answers.Update(answer);

            _databaseService.SaveChanges();
        }

        public void DeleteAnswer(Answer answer)
        {
            _databaseService.Answers.Remove(answer);

            _databaseService.SaveChanges();
        }
    }
}
