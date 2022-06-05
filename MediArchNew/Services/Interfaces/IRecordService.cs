using Data.Domain.Entities;
using MediArch.Models.AccountViewModels;
using System.Collections.Generic;

namespace MediArch.Services.Interfaces
{
    public interface IRecordService
    {
        List<UserRecordViewModel> GetAllUsers();
        List<Consult> GetAllConsults();
        List<Question> GetAllQuestions();
        List<Answer> GetAllAnswers();
    }
}
