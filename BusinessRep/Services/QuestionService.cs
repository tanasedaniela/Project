using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.ServiceInterfaces.Models.QuestionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessRep.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        private readonly IAnswerRepository _answerRepository;

        public QuestionService(IQuestionRepository questionRepository, IAnswerRepository answerRepository){
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public List<Answer> GetAllAnswersForQuestion(Guid id)
        {
            return _answerRepository.GetAllAnswersForGivenQuestion(id);
        }

        public List<Question> GetAllQuestions()
        {
            return _questionRepository.GetAllQuestions().OrderByDescending(x => x.CreatedDate).ToList();
        }

        public Question GetQuestionById(Guid id)
        {
            return _questionRepository.GetQuestionById(id);
        }

        public void CreateQuestion(Question question)
        {
            _questionRepository.CreateQuestion(question);
        }

        public void EditQuestion(Question question)
        {
            _questionRepository.EditQuestion(question);
        }

        public void DeleteQuestion(Question question)
        {
            List<Answer> answersForThisQuestion = _answerRepository.GetAllAnswersForGivenQuestion(question.Id);

            foreach(Answer ans in answersForThisQuestion)
            {
                _answerRepository.DeleteAnswer(ans);
            }

            _questionRepository.DeleteQuestion(question);
        }

        public void CreateQuestion(QuestionCreateModel questionCreateModel)
        {
            _questionRepository.CreateQuestion(
                Question.CreateQuestion(
                    questionCreateModel.UserId,
                    questionCreateModel.Text
                )
            );
        }

        public bool CheckIfQuestionExists(Guid id)
        {
            return _questionRepository.GetAllQuestions().Any(question => question.Id == id);
        }

        public int GetNumberOfPagesForQuestions()
        {
            int rez = 0;
            int count = _questionRepository.GetNumberOfQuestions();
            rez = count / 5;

            if (rez * 5 < count)
            {
                rez++;
            }

            return rez;
        }

        public List<Question> Get5QuestionsByIndex(int index)
        {
            List<Question> rez = new List<Question>() { };
            List<Question> allQuestions = GetAllQuestions();
            int start = (index - 1) * 5;
            int finish = start + 5;
            if (allQuestions.Count > 0)
            {
                for (int i = start; i < finish; i++)
                {
                    if (i < allQuestions.Count)
                    {
                        rez.Add(allQuestions[i]);
                    }
                }
            }

            return rez;
        }
        
    }
}
