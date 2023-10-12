using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.CourseDtos.QuizDto;
using ModelLayer.Models.Course;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IUnit _unit;

        public QuizController(IUnit unit)
        {
            _unit = unit;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizById(int? id)
        {
            if (id is  null || id == 0)
                return BadRequest(error: "Id is Not Correct");
            var quiz = await _unit.Quiz.GetByIdAsync(q => q.Id == id, new[] { "Questions" });
            if (quiz is null)
                return NotFound(new {message=$"Quiz with id:{id} not found"});
            var newView = new QuizViewDto()
            {
                QuizId=quiz.Id,
                QuizTitle = quiz.Name,
                NoOfQuestions = quiz.Questions.Count(),
            };
            return Ok(newView);
        }
        [HttpGet("Questions/{id}")]
        public async Task<IActionResult> GetQuestionsByQuizId(int? id)
        {
            if (id is null || id == 0)
                return BadRequest(error: "Invalid Id");
            var questions=await _unit.Question.GetAllAsync(qes=>qes.QuizId == id);
            if (questions is null)
                return NotFound(new {message=$"No questions was Found for id:{id}"});
            var questionsView = new List<QuizQuestionsViewDto>();
            foreach (var question in questions)
            {
                var questionV = new QuizQuestionsViewDto()
                {
                    QuestionHead = question.Name,
                    Options = question.Options.Select(o => new OptionRequest { OptionName = o.Name, IsCorrect = o.IsCorrect }).ToList()
                };
                questionsView.Add(questionV);
            }
                        return Ok(questionsView);
        }
        [HttpPost]
        public async Task<IActionResult> AddQuiz([FromBody] QuizRequest request)
        {
            if (request is null || request.QuestionRequests is null || request.QuestionRequests.Any(q => q.QuestionOptions.Count == 0)) //|| request.Sections.Count == 0)
                return BadRequest(error: "All Fields Required!");
            if (!ModelState.IsValid)
                return BadRequest(error: ModelState);         
            var quiz = new Quiz() 
            {
                Name=request.Name     
            };
            await  _unit.Quiz.AddAsync(quiz);
            await _unit.SaveDataAsync();
            //await  _unit.SaveDataAsync();
            foreach (var question in request.QuestionRequests)
            {
                var question1 = new Question()
                {
                    Name = question.Head,
                    QuizId = quiz.Id
                };
                await _unit.Question.AddAsync(question1);
                await _unit.SaveDataAsync();
                var options = new List<Option>();
                foreach(var option in question.QuestionOptions)
                {
                    var option1 = new Option()
                    {
                        IsCorrect = option.IsCorrect,
                        Name = option.OptionName,
                        QuestionId = question1.Id
                    };
                    options.Add(option1);
                }
                await _unit.Option.AddManyAsync(options);
                await _unit.SaveDataAsync();
            }
            return Ok(quiz);
        }
    }
}
