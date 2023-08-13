using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TriangleDbRepository;
using TriangleProject.Shared.Models.Answers;
using TriangleProject.Shared.Models.Game;
using TriangleProject.Shared.Models.Questions;

namespace TriangleProject.Server.Controllers
{
    [Route("api/[controller]/{userId}")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly DbRepository _db;

        public GamesController(DbRepository db)
        {
            _db = db;
        }

        //Get all the game for a spesific User.
        [HttpGet("GetGame")]
        public async Task<IActionResult> GetGamesByUser(int userId)
        {
            // session & User check.
            int? sessionId = HttpContext.Session.GetInt32("userId");
            if (sessionId != null)
            {
                if (userId == sessionId)
                {
                    // Get the user name.
                    object param = new
                    {
                        UserId = userId
                    };
                    string userQuery = "SELECT FirstName FROM Users WHERE ID = @UserId";
                    var userRecords = await _db.GetRecordsAsync<UserWithGames>(userQuery, param);
                    UserWithGames user = userRecords.FirstOrDefault();
                    // Get all the games of the user.
                    if (user != null)
                    {
                        string gameQuery = "SELECT ID, GameName, GameCode, TimeToAnswer, UserID, IsPublish, CanPublish FROM Games WHERE UserID = @UserId";
                        var gamesRecords = await _db.GetRecordsAsync<Game>(gameQuery, param);
                        user.Games = gamesRecords.ToList();


                        if (user.Games.Count > 0)
                        {
                            for (int i = 0; i < user.Games.Count; i++)
                            {
                                object Qparam = new
                                {
                                    GameID = user.Games[i].ID
                                };

                                string QuestionQuery = "SELECT ID as id, GameID as game_id, ImgOrText as imag_or_text, Content as content FROM Questions WHERE GameID = @GameID";
                                var QRecords = await _db.GetRecordsAsync<GetQuestionClass>(QuestionQuery, Qparam);
                                user.Games[i].Questions = QRecords.ToList();

                            }
                            foreach (Game game in user.Games)
                            {
                                bool gameValidation = await canPublish(game.ID);
                                game.CanPublish = gameValidation;
                                if (gameValidation == false)
                                {
                                    game.IsPublish = false;
                                }

                            }

                        }
                        return Ok(user);
                        
                    }
                    return BadRequest("User Not Found");
                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");
        }





        // get one game for update.
        [HttpGet("GetOneGame/{GameID}")]
        public async Task<IActionResult> GetOneGame(int userId, int GameID)// שולף משחק אחד
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");//  משיג את הת.ז של המשתמש שהתחבר.
            if (sessionId != null)
            {
                if (userId == sessionId)
                {

                    object param = new
                    {
                        GameID = GameID,
                        UserId = userId
                    };
                    // Query for the game:
                    string query = "SELECT ID, GameName, GameCode, TimeToAnswer, UserID, IsPublish, CanPublish FROM Games WHERE UserID = @UserId AND ID = @GameID";
                    var records = await _db.GetRecordsAsync<Game>(query, param);
                    Game mygame = records.FirstOrDefault();

                    if (mygame != null )// check if we have a game.
                    {
                        object param2 = new
                        {
                            GameID = mygame.ID
                        };
                        // Query for the questions:
                        string GameQuery = "SELECT ID as id, GameID as game_id, ImgOrText as imag_or_text, Content as content FROM Questions WHERE GameID = @GameID";
                        var GameRecords = await _db.GetRecordsAsync<GetQuestionClass>(GameQuery, param2);
                        mygame.Questions = GameRecords.ToList();

                        if (mygame.Questions.Count > 0)// check if we have questions.
                        {
                            for (int i = 0; i < mygame.Questions.Count; i++)
                            {

                                object param3 = new
                                {
                                    questionID = mygame.Questions[i].id,
                                };
                                // Query for the answers:
                                string answerQuery = "SELECT ID as id, QuestionID as question_id, IsCorrect as is_correct, Content as content, ImgOrText as imag_or_text FROM Answers  WHERE questionID = @questionID";
                                var aswersRecords = await _db.GetRecordsAsync<GetAnswerClass>(answerQuery, param3);
                                mygame.Questions[i].answers = aswersRecords.ToList();

                            }
                            return Ok(mygame);
                        }
                        return Ok(mygame);
                    }
                    return BadRequest("No Game");
                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");
        }


        // מפרסם משחק
        [HttpPost("publishGame")]
        public async Task<IActionResult> publishGame(int userId, PublishGame game)
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");
            if (sessionId != null)
            {
                if (userId == sessionId)
                {
                    object param = new
                    {
                        UserId = userId,
                        gameID = game.ID
                    };
                    string checkQuery = "SELECT GameName FROM Games WHERE UserId = @UserId and ID=@gameID";
                    var checkRecords = await _db.GetRecordsAsync<string>(checkQuery, param);
                    string gameName = checkRecords.FirstOrDefault();
                    if (gameName != null)
                    {
                        bool gameValidation = await canPublish(game.ID);
                        if (gameValidation == false)
                        {
                            game.IsPublish = false;
                        }
                        //else
                        //{
                        //    game.IsPublish = true;
                        //}

                        object pubParam = new
                        {
                            ID = game.ID,
                            IsPublish = game.IsPublish,
                            CanPublish = gameValidation
                        };
                        string updateQuery = "UPDATE Games SET IsPublish = @IsPublish, CanPublish=@CanPublish WHERE ID=@ID";
                        bool isUpdate = await _db.SaveDataAsync(updateQuery, pubParam);
                        if (isUpdate == true)
                        {
                            object param2 = new
                            {
                                ID = game.ID
                            };
                            string gameQuery = "SELECT ID, GameName, GameCode, TimeToAnswer, UserID, IsPublish, CanPublish FROM Games WHERE ID = @ID";
                            var gameRecord = await _db.GetRecordsAsync<Game>(gameQuery, param2);
                            Game gameToReturn = gameRecord.FirstOrDefault();
                            if (gameToReturn != null)
                            {
                                return Ok(gameToReturn);
                            }
                        }
                        return BadRequest("Publish Update Failed");
                    }
                    return BadRequest("It's Not Your Game");
                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");
        }





        // פונקציה לבדיקת עמידה בתנאים של פרסום משחק
        private async Task<bool> canPublish(int gameId)
        {
            int validAmountOfQuestions = 9;
            int validAmountOfDistractors = 1;
            object param = new
            {
                ID = gameId,
                ValidAmountOfDistractors = validAmountOfDistractors
            };
            string gameQuery2 = "SELECT Count(*) FROM " +
                            "(SELECT Questions.ID, count(*)" + "FROM Questions, Answers " +
                            "WHERE Questions.ID = Answers.QuestionId AND Questions.GameID = @ID " + "GROUP BY Questions.ID " + "HAVING count(*) > @ValidAmountOfDistractors)";
            var gameRecord2 = await _db.GetRecordsAsync<int>(gameQuery2, param);
            int numOfLegitQuestions = gameRecord2.FirstOrDefault();
            if (numOfLegitQuestions > validAmountOfQuestions && numOfLegitQuestions % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    
        
        [HttpGet("addgame/{GameName}")]
        public async Task<IActionResult> AddNewGame(int userId, string GameName)
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");
            if (sessionId != null)
            {
                if (userId == sessionId)
                {
                    object param = new
                    {
                        UserId = userId
                    };
                    string userQuery = "SELECT FirstName FROM Users WHERE ID = @UserId";
                    var userRecords = await _db.GetRecordsAsync<UserWithGames>(userQuery, param);
                    UserWithGames user = userRecords.FirstOrDefault();
                    if (user != null)
                    {
                        object newGameParam = new
                        {
                            GameName = GameName,
                            GameCode = 0,
                            IsPublish = false,
                            TimeToAnswer = 30,
                            UserID = userId,
                            CanPublish = false
                        };
                        string insertGameQuery = "INSERT INTO Games (GameName, GameCode, IsPublish, TimeToAnswer, UserID, CanPublish) " +
                                "VALUES (@GameName, @GameCode, @IsPublish, @TimeToAnswer, @UserID, @CanPublish)";
                        int newGameId = await _db.InsertReturnId(insertGameQuery, newGameParam);
                        if (newGameId != 0)
                        {
                            int gameCode = newGameId + 100;
                            object updateParam = new
                            {
                                ID = newGameId,
                                GameCode = gameCode

                            };
                            string updateCodeQuery = "UPDATE Games SET GameCode = @GameCode	WHERE ID=@ID";
                            bool isUpdate = await _db.SaveDataAsync(updateCodeQuery, updateParam);
                            if (isUpdate == true)
                            {
                                object param2 = new
                                {
                                    ID = newGameId
                                };
                                string gameQuery = "SELECT ID,GameName,GameCode,IsPublish,CanPublish FROM Games   WHERE ID = @ID";
                                var gameRecord = await _db.GetRecordsAsync<Game>(gameQuery, param2);
                                Game newGame = gameRecord.FirstOrDefault();
                                return Ok(newGame);
                            }
                            return BadRequest("Game Code Not Created");
                        }
                        return BadRequest("Game Not Created");
                    }
                    return BadRequest("User Not Found");
                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");
        }


        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion(int userId, GetQuestionClass questionToAdd)
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");
            if (sessionId != null)// session check.
            {
                if (userId == sessionId)// Checl User Logged in.
                {
                    object param1 = new
                    {

                        GameID = questionToAdd.game_id,
                        ImgOrText = questionToAdd.imag_or_text,
                        Content = questionToAdd.content
                    };
                    // insert answer.
                    string query = "INSERT INTO Questions (GameID, ImgOrText, Content) VALUES (@GameID, @ImgOrText, @Content)";
                    int newQuestionId = await _db.InsertReturnId(query, param1);
                    if (newQuestionId > 0)
                    {
                        for (int i = 0; i < questionToAdd.answers.Count; i++)
                        {
                            object param = new
                            {
                                question_id = newQuestionId,
                                is_correct = questionToAdd.answers[i].is_correct,
                                content = questionToAdd.answers[i].content,
                                imag_or_text = questionToAdd.answers[i].imag_or_text
                            };
                            // insert answer.
                            string ansQuery = "INSERT INTO Answers (QuestionID, IsCorrect, Content, ImgOrText) VALUES (@question_id, @is_correct, @content, @imag_or_text)";
                            int newAnswerId = await _db.InsertReturnId(ansQuery, param);
                        }

                        object param2 = new
                        {
                            ID = newQuestionId
                        };
                        // Get the Question:
                        string questionQuery = "SELECT ID as id, GameID as game_id, ImgOrText as imag_or_text, Content as content FROM Questions WHERE ID = @ID";
                        var questionRecords = await _db.GetRecordsAsync<GetQuestionClass>(questionQuery, param2);
                        GetQuestionClass questionToReturn = questionRecords.FirstOrDefault();
                        if (questionToReturn != null)
                        {
                            object param3 = new
                            {
                                questionID = questionToReturn.id
                            };
                            // Query for the answers:
                            string answerQuery = "SELECT ID as id, QuestionID as question_id, IsCorrect as is_correct, Content as content, ImgOrText as imag_or_text FROM Answers  WHERE questionID = @questionID";
                            var aswersRecords = await _db.GetRecordsAsync<GetAnswerClass>(answerQuery, param3);
                            questionToReturn.answers = aswersRecords.ToList();
                            return Ok(questionToReturn);

                        }
                        return BadRequest("Cant Get Question To return");
                    }
                    return BadRequest("Question Not Insert");

                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");


        }





        [HttpPost("updateGame")]
        public async Task<IActionResult> UpdateGames(int userId, Game gameToUpdate)
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");
            if (sessionId != null)// בדיקת סשן
            {
                if (userId == sessionId)// בדיקה שהמשתמש הנכון מחובר
                {
                    // עדכון משחק
                    string updateQuery = "UPDATE Games SET GameName = @GameName, TimeToAnswer = @TimeToAnswer, IsPublish = @IsPublish,  CanPublish = @CanPublish WHERE UserID = @UserID AND ID = @ID";
                    bool updateGame = await _db.SaveDataAsync(updateQuery, gameToUpdate);// שלחתי את המשחק כפרמטר ולכן אין לי פרמטר

                    if (updateGame == true)//במידה והצליח, אשלוף את המשחק ואחזיר אותו.
                    {
                        object param = new
                        {
                            ID = gameToUpdate.ID
                        };
                        string gameQuery = "SELECT ID, GameName, GameCode, TimeToAnswer, UserID, IsPublish, CanPublish FROM Games WHERE ID = @ID";
                        var gameRecord = await _db.GetRecordsAsync<Game>(gameQuery, param);
                        Game gameToReturn = gameRecord.FirstOrDefault();

                        if (gameToReturn != null)//בדיקה שהמשחק לא ריק מתוכן
                        {
                            return Ok(gameToReturn);
                        }

                    }
                    return BadRequest("Update Failed");
                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");


        }


        // מחיקת משחק!



        //[HttpPost("UpdateQuestion")]
        //public async Task<IActionResult> UpdateQuestion(int userId, GetQuestionClass questionToUpdate)
        //{
        //    int? sessionId = HttpContext.Session.GetInt32("userId");
        //    if (sessionId != null)// בדיקת סשן
        //    {
        //        if (userId == sessionId)// בדיקה שהמשתמש הנכון מחובר
        //        {
        //            //update Question
        //            string questionQuery = "UPDATE Questions SET ImgOrText = @imag_or_text, Content = @content WHERE ID = @id";
        //            bool updateQuestion = await _db.SaveDataAsync(questionQuery, questionToUpdate);// שלחתי את המשחק כפרמטר ולכן אין לי פרמטר
        //            if (updateQuestion == true)
        //            {
        //                foreach (GetAnswerClass answer in questionToUpdate.answers)
        //                {
        //                    object param = new
        //                    {
        //                        ID = answer.id,
        //                        IsCorrect = answer.is_correct,
        //                        Content = answer.content,
        //                        ImgOrText = answer.imag_or_text
        //                    };
        //                    string answerQuery = "UPDATE Answers SET IsCorrect = @IsCorrect, Content = @Content, ImgOrText = @ImgOrText WHERE ID = @ID";
        //                    bool updateAnswer = await _db.SaveDataAsync(answerQuery, param);// שלחתי את המשחק כפרמטר ולכן אין לי פרמטר

        //                }
        //                return Ok();
        //            }
        //            return BadRequest("Question Not Update");
        //        }
        //        return BadRequest("User Not Logged In");
        //    }
        //    return BadRequest("No Session");
        //}





        // delete the question and their answers.



        [HttpDelete("deleteQ/{QuestionID}")]
        public async Task<IActionResult> DeleteQuestion(int userId, int QuestionID)
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");
            if (sessionId != null)// בדיקה שהמשתמש הנכון מחובר
            {
                if (userId == sessionId)// בדיקה שהמשתמש הנכון מחובר
                {
                    // Delete question!
                    object param = new
                    {
                        ID = QuestionID
                    };
                    string deleteQuery = "DELETE FROM Questions WHERE ID = @ID";
                    bool isDeleted = await _db.SaveDataAsync(deleteQuery, param);

                    if (isDeleted == true)// האם המחיקה הצליחה
                    {
                        return Ok("Game Deleted!");
                    }
                    return BadRequest("Delete failed");
                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");
        }


        [HttpDelete("delete/{GameID}")]
        public async Task<IActionResult> DeleteGame(int userId, int GameID)
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");
            if (sessionId != null)// בדיקה שהמשתמש הנכון מחובר
            {
                if (userId == sessionId)// בדיקה שהמשתמש הנכון מחובר
                {
                    // מחיקת משחק!

                    object param = new
                    {
                        userId = userId,
                        GameID = GameID
                    };
                    string deleteQuery = "DELETE FROM Games WHERE UserID = @userId AND ID = @GameID";
                    bool isDeleted = await _db.SaveDataAsync(deleteQuery, param);

                    if (isDeleted == true)// האם המחיקה הצליחה
                    {
                        return Ok("Game Deleted!");
                    }
                    return BadRequest("Delete failed");
                }
                return BadRequest("User Not Logged In");
            }
            return BadRequest("No Session");
        }






    }
}




