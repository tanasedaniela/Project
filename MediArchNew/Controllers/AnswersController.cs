using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Data.Domain.Interfaces;
using Data.Domain.ServiceInterfaces.Models.AnswerViewModels;

namespace MediArch.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        private readonly IAnswerService _service;

        public AnswersController(IAnswerService service)
        {
            _service = service;
        }

        // GET: Answers
        public IActionResult Index(Guid? questionId)
        {
            TempData["Id"] = questionId;

            return View(_service.GetAllAnswersForGivenQuestion(questionId.Value));
        }

        // GET: Answers/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var answer = _service.GetAnswerById(id.Value);

            if (answer == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create(Guid? id)
        {
            TempData["Id"] = id;
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid? id, [Bind("UserId,QuestionId,Text")] AnswerCreateModel answerCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(answerCreateModel);
            }

            _service.CreateAnswer(id, answerCreateModel);

            return RedirectToAction("QuestionsPaginated", "Questions");
        }

        // GET: Answers/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var answer = _service.GetAnswerById(id.Value);
            if (answer == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var answerEditModel = new AnswerEditModel(
                answer.Id,
                answer.UserId,
                answer.QuestionId,
                answer.Text
            );

            return View(answerEditModel);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid? id, [Bind("Id,UserId,QuestionId,AnswerDate,Text")] AnswerEditModel answerEditModel)
        {
            var answerToBeEdited = _service.GetAnswerById(id.Value);

            if (answerToBeEdited == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(answerEditModel);
            }

            answerToBeEdited.UserId = answerEditModel.UserId;
            answerToBeEdited.QuestionId = answerEditModel.QuestionId;
            answerToBeEdited.Text = answerEditModel.Text;

            try
            {
                _service.EditAnswer(answerToBeEdited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(_service.GetAnswerById(id.Value).Id))
                {
                    return RedirectToAction("Not_Found", "Home");
                }

                throw;
            }

            return RedirectToAction("QuestionsPaginated", "Questions");

        }

        // GET: Answers/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var answer = _service.GetAnswerById(id.Value);
            if (answer == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var answer = _service.GetAnswerById(id);

            _service.DeleteAnswer(answer);

            return RedirectToAction("QuestionsPaginated", "Questions");
        }

        private bool AnswerExists(Guid id)
        {
            return _service.CheckIfAnswerExists(id);
        }

        // GET: Answers/Create
        public IActionResult CreateNew(Guid? qid, Guid? uid, String text)
        {
            TempData["QId"] = qid;
            TempData["UId"] = uid;
            TempData["QText"] = text;
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(Guid? qid, Guid? uid, String text, [Bind("UserId,QuestionId,Text")] AnswerCreateModel answerCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(answerCreateModel);
            }

            _service.CreateNew(qid, uid, text);

            return RedirectToAction("QuestionsPaginated", "Questions",new { NoPage = "1" });
        }

        [HttpPost]
        public ActionResult CreateNewAnswer(Guid uid, Guid qid, string qtext, string noPage)
        {
            _service.CreateNewAnswer(uid, qid, qtext);
            return RedirectToAction("QuestionsPaginated", "Questions", new { NoPage = noPage });
        }
    }
}
