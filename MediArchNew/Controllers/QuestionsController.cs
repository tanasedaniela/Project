using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Data.Domain.Interfaces;
using Data.Domain.ServiceInterfaces.Models.QuestionViewModels;

namespace MediArch.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _service;

        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }

        // GET: Questions
        public IActionResult Index()
        {
            foreach (var item in _service.GetAllQuestions())
            {
                item.Answers = _service.GetAllAnswersForQuestion(item.Id);
            }

            return View(_service.GetAllQuestions());
        }

        public IActionResult QuestionsPaginated(int noPage)
        {
            
            if (noPage < 1)
            {
                noPage = 1;
            }

            if (noPage > _service.GetNumberOfPagesForQuestions())
            {
                noPage = _service.GetNumberOfPagesForQuestions();
            }
            TempData["NoPage"] = noPage;
            return View(_service.Get5QuestionsByIndex(noPage));
        }



        // GET: Questions/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var question = _service.GetQuestionById(id.Value);
            if (question == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(question);
        }

        // GET: Questions/AnswerList/5
        public IActionResult Answers(Guid? id)
        {
            return RedirectToAction("Index", "Answers", new { questionId = id });
        }

        // GET: Questions/Create
        public IActionResult Create(Guid? uid)
        {
            TempData["UId"] = uid;
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid? uid, [Bind("UserId,CreatedDate,Text")] QuestionCreateModel questionCreateModel)
        {
            TempData["UId"] = uid;
            if (!ModelState.IsValid)
            {
                return View(questionCreateModel);
            }

            _service.CreateQuestion(questionCreateModel);

            return RedirectToAction(nameof(QuestionsPaginated));
        }

        // GET: Questions/Edit/5
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var question = _service.GetQuestionById(id);

            if (question == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var questionEditModel = new QuestionEditModel(
                id,
                question.UserId,
                question.Text
            );

            return View(questionEditModel);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,UserId,CreatedDate,Text")] QuestionEditModel questionEditModel)
        {
            var questionToBeEdited = _service.GetQuestionById(id);

            if (questionToBeEdited == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(questionEditModel);
            }

            questionToBeEdited.UserId = questionEditModel.UserId;
            questionToBeEdited.Text = questionEditModel.Text;


            try
            {
                _service.EditQuestion(questionToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(_service.GetQuestionById(id).Id))
                {
                    return RedirectToAction("Not_Found", "Home");
                }

                throw;
            }

            return RedirectToAction(nameof(QuestionsPaginated));
        }

        // GET: Questions/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var question = _service.GetQuestionById(id.Value);

            if (question == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var question = _service.GetQuestionById(id);

            _service.DeleteQuestion(question);

            return RedirectToAction(nameof(QuestionsPaginated));
        }

        private bool QuestionExists(Guid id)
        {
            return _service.CheckIfQuestionExists(id);
        }

    }
}
