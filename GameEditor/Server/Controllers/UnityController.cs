using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TriangleDbRepository;
using TriangleProject.Shared.Models.Answers;
using TriangleProject.Shared.Models.Game;
using TriangleProject.Shared.Models.Questions;

namespace TriangleProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnityController : ControllerBase
    {

        private readonly DbRepository _db;
        public UnityController(DbRepository db)
        {
            _db = db;
        }


        [HttpGet("{GameCode}")]
        public async Task<IActionResult> GetGamesByUser(int GameCode)// מביא לעורך את כל המשחקים שלו
        {
            //int? sessionId = HttpContext.Session.GetInt32("userId");//  משיג את הת.ז של המשתמש שהתחבר.

            object param = new
            {
                GameCode = GameCode
            };
            // שליפה של משחק
            string query = "SELECT ID as id, GameName as game_name, GameCode as game_code, TimeToAnswer as time_to_answer, IsPublish as is_publish FROM Games WHERE GameCode = @GameCode";
            var records = await _db.GetRecordsAsync<UnityGame>(query, param);
            UnityGame mygame = records.FirstOrDefault();

            if (mygame != null && mygame.is_publish == true)// אם יש משחקץ
            {
                object param2 = new
                {
                    GameID = mygame.id
                };
                string GameQuery = "SELECT ID as id, GameID as game_id, ImgOrText as imag_or_text, Content as content FROM Questions WHERE GameID = @GameID";
                var GameRecords = await _db.GetRecordsAsync<GetQuestionClass>(GameQuery, param2);
                mygame.game_questions = GameRecords.ToList();

                if (mygame.game_questions.Count > 0)
                {
                    for (int i = 0; i < mygame.game_questions.Count; i++)
                    {

                        object param3 = new
                        {
                            questionID = mygame.game_questions[i].id,
                        };
                        string answerQuery = "SELECT ID as id, QuestionID as question_id, IsCorrect as is_correct, Content as content, ImgOrText as imag_or_text FROM Answers  WHERE questionID = @questionID";
                        var aswersRecords = await _db.GetRecordsAsync<GetAnswerClass>(answerQuery, param3);
                        mygame.game_questions[i].answers = aswersRecords.ToList();

                    }
                    return Ok(mygame);
                }
                return BadRequest("There Is No Questions");

            }
            return BadRequest("User Not Found / Game Is Not Published");

        }




    }
}
