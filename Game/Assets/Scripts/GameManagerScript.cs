using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UIElements;

//using System;
//using System;

public class GameManagerScript : MonoBehaviour
{
    /// <summary>
    /// ����� �� ������ ���� ���� �����
    /// </summary>
    public List<QuestionData> myQuestions;

    public List<QuestionData> myQuestionsBeckUp;

    //public GameInfo Game = jsonPull.myGame;

    int amountOfQuestions = 0;// ����� ��� �� ���� ������ ��� ������

    /// <summary>
    /// ����� ������ �����
    /// </summary>
    public List<QuestionData> AnsweredQuestions;
    
    /// <summary>
    /// ���� ����� �� ����
    /// </summary>
    [SerializeField] private TextMeshPro questionTextBox;
        
    /// <summary>
    /// ���� ������ ����
    /// </summary>
    [SerializeField] private GameObject nextButton;
    
    /// <summary>
    /// ���� ������� ������ ����� �����
    /// </summary>
    [SerializeField] private SubmitScript submitButton;
    
    /// <summary>
    /// ���� ������� ���� ������� ���� ������
    /// </summary>
    [SerializeField] private AnswerScript ansObj;

    /// <summary>
    /// ������ �� ������� �� ����, ����� ����� ������� ��� ����
    /// </summary>
    public List<AnswerScript> answerOnScreen = new List<AnswerScript>();
    
    /// <summary>
    /// ���� ��� ���� ����� ������
    /// </summary>
    public int currentQuestion;
    
    public GameObject PauseMenuScreen;
    public GameObject odotMenuScreen;
    public GameObject HTPScreen;
    public GameObject StopAudio;
    public GameObject PlayAudio;
    
    public GameObject PlayAgain;
    public GameObject ChoosePlayere2;
    public ChosePlayerScript ChosePlayerScript;

    //first player:
    private string firstPlayerName;
    private SpriteRenderer firstCharacter;// the sprite
    private SpriteRenderer FirstPlayerHat;// the hat above the score board
    public SpriteRenderer firstPleyerScoreBG;
    private TextMeshPro FirstPlayerScoreBoardText;// score board
    private int FirstPlayerScore = 0;// score
    private float FirstPlayerNumOfQuestions = 0;// number of questions
    private float FirstPlayerWrongAnswers = 0;// numbers of wrong questions
    private float firstPlayerTotalTime = 0;// total time of the player.

    //secound player:
    private string secoundPlayerName;
    private SpriteRenderer secoundCharacter;// the sprite
    private SpriteRenderer SecoundPlayerHat;// the hat above the score board
    public SpriteRenderer secoundPleyerScoreBG;
    private TextMeshPro SecoundPlayerScoreBoardText;// score board
    private int SecoundPlayerScore = 0;// score
    private float SecoundPlayerNumOfQuestions = 0;// number of questions
    private float SecoundPlayerWrongAnswers = 0;// numbers of wrong questions
    private float secoundPlayerTotalTime = 0;// total time of the player.


    // 
    public SpriteRenderer Blue;//the blue sprite
    public SpriteRenderer Yellow;// the yellow sprite
    public SpriteRenderer BlueHat;
    public SpriteRenderer YellowHat;
    public TextMeshPro yellowScoreBoardText;
    public TextMeshPro blueScoreBoardText;
    public SpriteRenderer YellowScoreBG;
    public SpriteRenderer BlueScoreBG;
    public TextMeshPro blueName;
    public TextMeshPro yellowName;
    
    //public ChosePlayerScript ChosePlayerScript;
    /// <summary>
    /// IF it's Blue, the first character will be blue and start, Else the ooposite for the yellow.
    /// </summary>
    public bool whoPlay;
    //������ ����� ������ ������ �������
    //������ ������� �� �������

    public bool timeIsRunning = false;
    public float timer;
    public TextMeshPro timerText;

    public AudioSource bgsound;

    // list for the answers positions(contains the game objects) 
    public List<GameObject> position2;
    public List<GameObject> position3;
    public List<GameObject> position4;
    public List<GameObject> position5;
    public List<GameObject> position6;

    public TextMeshPro Qprogress;

    public GameObject endGameCanvas;// ��� ���� �����

    [SerializeField] private TextMeshPro P1Name;
    [SerializeField] private TextMeshPro InputGrade1;
    [SerializeField] private TextMeshPro InputWrongs1;
    [SerializeField] private TextMeshPro InputTime1;

    [SerializeField] private TextMeshPro P2Name;
    [SerializeField] private TextMeshPro InputGrade2;
    [SerializeField] private TextMeshPro InputWrongs2;
    [SerializeField] private TextMeshPro InputTime2;

    [SerializeField] private TextMeshPro endText;

    public GameInfo myActiveGame;
    public GameInfo BeckUpGame;
    public float timeForQuestion;
    public string turnToPlay = "first";// ����� �� ����, ����� ������ �� ����.

    bool flag = false;

    public TextMeshPro gameName;
    public TextMeshPro time01; 
    public TextMeshPro time02;
    public bool positions { get; private set; }

    // Start is called before the first frame update

    public SpriteRenderer answerPic;// the pic we scale

    private void Update()
    {
        Qprogress.text = submitButton.progressCount.ToString() + "/" + amountOfQuestions.ToString();
        Timer();

    }



    public void GetGame(GameInfo game)
    {
        myActiveGame = game;
        BeckUpGame = game;
        myQuestions = game.game_questions;
        //Debug.Log("myQ" + " " + myQuestions.Count);
        //Debug.Log("MYACTIV GSME "+ myActiveGame.game_questions.Count);
        timer = game.time_to_answer;
        timeForQuestion = game.time_to_answer;
        if(game.time_to_answer == 0)
        {
            timeIsRunning = false;
        }
    }

    public void StartGame()
    {
        gameName.text=myActiveGame.game_name;
        //myActiveGame=game;
        Debug.Log(myActiveGame.game_questions.Count);
        //Debug.Log(myActiveGame);
        //myQuestions = game.game_questions;
        Debug.Log(myQuestions);
        GetQuestion(); //����� �������� ����� �� ����� �� ����
        WhoStart();
        amountOfQuestions = myQuestions.Count;// ����� ������ �� ���� ������ ��� ��� ������ �����.
        Turn();
        stopbg();
        //playbg();
        endGameCanvas.SetActive(false);
        Debug.Log(timeForQuestion.ToString());

    }




    void Timer()
    {

        if (timeIsRunning == true)
        {


            // if the timer true, check if it more then 0 and run the timer.
            if (timeIsRunning == true)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    updateTimer(timer);
                }
                else
                {
                    Debug.Log("Time is up!");
                    timer = timeForQuestion;
                    timeIsRunning = false;
                    questionTextBox.text = "���� ����! ���� ���� ����� ����.";
                    submitButton.userIsCorrect = false;
                    Score();
                    ClearThenGet();

                }
            }

        }
    }


    void WhoStart()// ����� �� ����� ������� ����� ������ �� ����� ��������� ��� ��� ������, ���� ��� ���
    {
        if (ChosePlayerScript.whoStart == true)
        {           
            Debug.Log("First Character Is Blue");
            whoPlay = true;// Blue start

            // Characters:
            firstCharacter = Blue;
            secoundCharacter = Yellow;
            // Score Board:
            FirstPlayerScoreBoardText = blueScoreBoardText;
            SecoundPlayerScoreBoardText = yellowScoreBoardText;
            firstPleyerScoreBG = BlueScoreBG;
            secoundPleyerScoreBG = YellowScoreBG;
            // Hat:
            FirstPlayerHat = BlueHat;
            SecoundPlayerHat = YellowHat;
            //name:
            firstPlayerName = ChosePlayerScript.blueName;
            //firstPlayerEndText.text = ChosePlayerScript.blueName;
            blueName.text = ChosePlayerScript.blueName;
            secoundPlayerName = ChosePlayerScript.yellowName;
            //secoundPlayerEndText.text = ChosePlayerScript.yellowName;
            yellowName.text = ChosePlayerScript.yellowName;
            Debug.Log(firstPlayerName);
            //myQuestions = jsonPull.myGame.game_questions;

        }
        else if(ChosePlayerScript.whoStart == false)//ChosePlayerScript.whoStart == False
        {          
            Debug.Log("First Character Is Yellow");
            whoPlay = false;// Yellow start

            // Characters:
            firstCharacter = Yellow;
            secoundCharacter = Blue;
            // Score Board:
            FirstPlayerScoreBoardText = yellowScoreBoardText;
            SecoundPlayerScoreBoardText = blueScoreBoardText;
            firstPleyerScoreBG = YellowScoreBG;
            secoundPleyerScoreBG = BlueScoreBG;
            // Hat:
            FirstPlayerHat = YellowHat;
            SecoundPlayerHat = BlueHat;
            //name:
            firstPlayerName = ChosePlayerScript.yellowName;
            yellowName.text = ChosePlayerScript.yellowName;
            secoundPlayerName = ChosePlayerScript.blueName;
            blueName.text = ChosePlayerScript.blueName;
            Debug.Log(firstPlayerName);

        }

    }


    //public void Turn()// ����� �� ������ ����� ��� �� ����� �����. ����� ���� �� ���� ������ �� �����
    //{
    //    //Debug.Log("FirstPlayer stat: amount of questions: " + FirstPlayerNumOfQuestions + " wrong answers: " + FirstPlayerWrongAnswers + "correct: " + FirstPlayerScore);
    //    //Debug.Log("Secound Player stat: amount of questions: " + SecoundPlayerNumOfQuestions + " wrong answers: " + SecoundPlayerWrongAnswers + "correct: " + SecoundPlayerScore);

    //    timer = 30;
    //    timeIsRunning = true;

    //    if (whoPlay == true)
    //    {

    //        Debug.Log("first player turn(yellow)");
    //        firstCharacter.color = Color.white;
    //        secoundCharacter.color = Color.black;
    //        //FirstPlayerHat = Color.white;
    //        FirstPlayerNumOfQuestions++;// ���� �� ���� ������ ����� ����� �1

    //    }
    //    else if (whoPlay == false)
    //    {

    //        Debug.Log("secound player turn(blue)");
    //        secoundCharacter.color = Color.white;
    //        firstCharacter.color = Color.black;
    //        SecoundPlayerNumOfQuestions++;// ���� �� ���� ������ ����� ����� �1

    //    }

    //}

    public void Turn()// ����� �� ������ ����� ��� �� ����� �����. ����� ���� �� ���� ������ �� �����
    {
        if (timer > 0)
        {
            timer = timeForQuestion;
            timeIsRunning = true;
        }
        if (turnToPlay == "first")
        {

            Debug.Log("first player turn");
            // Characters:
            firstCharacter.color = Color.white;
            secoundCharacter.color = Color.grey;
            // Hat:
            FirstPlayerHat.enabled = true;
            SecoundPlayerHat.enabled = false;
            // Score Board:
            firstPleyerScoreBG.color = Color.white;
            secoundPleyerScoreBG.color = Color.grey;

            FirstPlayerNumOfQuestions++;// ���� �� ���� ������ ����� ����� �1

        }
        else if (turnToPlay == "secound")
        {

            Debug.Log("secound player turn");
            // Characters:
            secoundCharacter.color = Color.white;
            firstCharacter.color = Color.grey;
            // Hat:
            SecoundPlayerHat.enabled = true;
            FirstPlayerHat.enabled = false;
            // Score Board:
            secoundPleyerScoreBG.color = Color.white;
            firstPleyerScoreBG.color = Color.grey;

            SecoundPlayerNumOfQuestions++;// ���� �� ���� ������ ����� ����� �1

        }
        //Debug.Log( "1: " + firstPlayerGrade);
        //Debug.Log("2: " + secoundPlayerGrade);

        double firstPlayerGrade = ((FirstPlayerNumOfQuestions - FirstPlayerWrongAnswers) / FirstPlayerNumOfQuestions) * 100;
        double secoundPlayerGrade = ((SecoundPlayerNumOfQuestions - SecoundPlayerWrongAnswers) / SecoundPlayerNumOfQuestions) * 100;


        Debug.Log("FirstPlayer stat: amount of questions: " + FirstPlayerNumOfQuestions + " wrong answers: " + FirstPlayerWrongAnswers + "correct: " + FirstPlayerScore );

        Debug.Log("Secound Player stat: amount of questions: " + SecoundPlayerNumOfQuestions + " wrong answers: " + SecoundPlayerWrongAnswers + "correct: " + SecoundPlayerScore );

        Debug.Log(FirstPlayerNumOfQuestions.ToString() + " - " + FirstPlayerWrongAnswers.ToString() + " / " + FirstPlayerNumOfQuestions.ToString() + " * " + "100" + " = "  + firstPlayerGrade.ToString());


    }

    public void Score()
    {
        switch (turnToPlay)
        {
            case "first":

                if (submitButton.userIsCorrect == true)
                {
                    FirstPlayerScore++;// ���� �� ������ �1
                    FirstPlayerScoreBoardText.text = FirstPlayerScore.ToString();
                }
                else
                {
                    FirstPlayerWrongAnswers++;//���� �� ������� �� ����� �1
                }
                firstPlayerTotalTime += timeForQuestion - timer;// ����� �� ���� �� ������ ���� ����� �� �����
                Debug.Log(firstPlayerTotalTime.ToString());
                break;

            case "secound":

                if (submitButton.userIsCorrect == true)
                {
                    SecoundPlayerScore++;// ���� �� ������ �1
                    SecoundPlayerScoreBoardText.text = SecoundPlayerScore.ToString();
                }
                else
                {
                    SecoundPlayerWrongAnswers++;//���� �� ������� �� ����� �1
                }
                secoundPlayerTotalTime += timeForQuestion - timer;// ����� �� ���� �� ������ ���� ����� �� �����
                Debug.Log(secoundPlayerTotalTime.ToString());

                break;
        }
    }

    public void updateTimer (float CurrentTime)
    {
        CurrentTime += 1;

        float minotes = Mathf.FloorToInt(CurrentTime / 60);
        float seconds = Mathf.FloorToInt(CurrentTime % 60);

        if (timeIsRunning == true)
        {
            timerText.text = string.Format("{0:0} : {1:00}", minotes, seconds);
        }
    }
 

    private void GetQuestion() // ���� ���� ������ �������� ������ ������
    {
        currentQuestion = Random.Range(0, myQuestions.Count);//�� ���� ���� ��������
        // ����� ������� ���� �������� ���.
        Debug.Log("Amount Of Questions In CurrentGame: " + amountOfQuestions + " ProgressCount: " + submitButton.progressCount);
        //myQuestions[currentQuestion].showCount++; // ���� �� ������ � ��� ���� ��� ����� ����� ����� ����.

        // ���� ����� �� ���� �� ���� ����� �������
        Debug.Log(myQuestions);
        Debug.Log(myQuestions[currentQuestion].content);
        questionTextBox.text = myQuestions[currentQuestion].content;

        switch (myQuestions[currentQuestion].answers.Count)// ����� ��� ������ �� ����� ������ �������� ������ ������.
        {
            case 2:
                createAnswers(position2, 2);
                break;
            case 3:
                createAnswers(position3, 3);
                break;
            case 4:
                createAnswers(position4, 4);
                break;
            case 5:
                createAnswers(position5, 5);
                break;
            case 6:
                createAnswers(position6, 6);
                break;
            default:
                break;
        }


    }


  

    /// <summary>
    /// creating answers in random positions.
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="numOfAnswers"></param>
    private void createAnswers(List<GameObject> positions, int numOfAnswers)
    {
        // new list of positions.
        List<GameObject> newPositinList = positions.ToList();
        // loop all the answers we have.
        for (int i = 0; i < numOfAnswers; i++)
        {
            // ���� ���� ������� �0 �� ���� ������
            int randomIndex = Random.Range(0, newPositinList.Count);


            // creating an answer.
            AnswerScript newAns = Instantiate(ansObj, new Vector2(newPositinList[randomIndex].transform.position.x, newPositinList[randomIndex].transform.position.y), Quaternion.identity);
            //����� ����� ��� ������ �� ���� �� �����.
            if (myQuestions[currentQuestion].answers[i].imag_or_text != true)// ����� ����
            {
                //�� �� ���� ����� �� �� �����
                newAns.answerConText.text = myQuestions[currentQuestion].answers[i].content;
                newAns.answerConPic.enabled = false;
                newAns.img = false;
            }
            else //  �����
            {
                //�� �� ����� ����� �� �� ������
                newAns.answerConPic.sprite = myQuestions[currentQuestion].answers[i].img_cont;
                newAns.answerConPic.size = new Vector2(1, 1);// ���� �� ���� ������ ��� ���� ����, ��� ���� ��� ���� ��� ���� ������.
                newAns.answerConPic.enabled = true;
                newAns.img = true;
            }

            // ����� ������ ��� ��� ����� �� ��
            newAns.isCorrect = myQuestions[currentQuestion].answers[i].is_correct;
            //����� ������ �� ����� �������
            newAns.submitBTN = submitButton;
            // ����� ������ ������ �� ���� �� ������ ������ �����
            newAns.gameManager = this;// ����� �������� �� ����� �� �����.
            answerOnScreen.Add(newAns);

            //remove the ans from the temporery list.
            newPositinList.Remove(newPositinList[randomIndex]);

        }


    }


    public void ClearThenGet()
    {// ���� �� ������� �����

        submitButton.wasItClicked = false;// ����� ���� ���� ���� ��� ���� ������ ������ ����� ���

        foreach(AnswerScript ans in answerOnScreen)
        {
            Destroy(ans.gameObject);// ���� �� �������� ��� ����
        }
        answerOnScreen.Clear();// ���� �� ����� �������
        nextButton.SetActive(false);// ���� �� ������ ����

        // ����� ��� ������ ���� ���� ����� �����
        if (submitButton.progressCount < amountOfQuestions)
        {
            SwitchTurn();
            Turn();
            GetQuestion();// ���� �������� ����� ����� ����� ���� ����.
        }

        else if (!flag)
                {
            // �� ����� ������� ����� �� �����
            questionTextBox.text = "����� �� �����!"; // ������ �� ������
            FinishGame();
            flag = true;
        }
        else
        {

        }


    }

    public void FinishGame()
    {
        // stop the Timer.
        timeIsRunning = false;
        // show end game pop up.
        endGameCanvas.SetActive(true);

        // players score:
        float firstPlayerGrade = Mathf.Floor(((FirstPlayerNumOfQuestions - FirstPlayerWrongAnswers) / FirstPlayerNumOfQuestions) * 100);
        float secoundPlayerGrade = Mathf.Floor(((SecoundPlayerNumOfQuestions - SecoundPlayerWrongAnswers) / SecoundPlayerNumOfQuestions) * 100);
        
            // players thime:
            float minotes1 = Mathf.FloorToInt(firstPlayerTotalTime / 60);
            float seconds1 = Mathf.FloorToInt(firstPlayerTotalTime % 60);
            string firstPTime = string.Format("{0:0} : {1:00}", minotes1, seconds1);

            float minotes2 = Mathf.FloorToInt(secoundPlayerTotalTime / 60);
            float seconds2 = Mathf.FloorToInt(secoundPlayerTotalTime % 60);
            string secoundPTime = string.Format("{0:0} : {1:00}", minotes2, seconds2);
        

        
        // checking who won the game by grade:
        if (firstPlayerGrade > secoundPlayerGrade)
        {
            endText.text = firstPlayerName + ", �����!";
        }
        else if (firstPlayerGrade < secoundPlayerGrade)
        {
            endText.text = secoundPlayerName + ", �����!";

        }

        //else // if grades are equals, we check the time:
        //{
        //    if (float.Parse(firstPTime) < float.Parse(secoundPTime))
        //    {
        //        endText.text = firstPlayerName + ", �����!";

        //    }
        //    else
        //    {
        //        endText.text = secoundPlayerName + ", �����!";

        //    }
        //}
        // to show the results in the right position we check the player character.

        if (ChosePlayerScript.whoStart == true)
        {
            InputGrade1.text = firstPlayerGrade.ToString();
            InputGrade2.text = secoundPlayerGrade.ToString();
            InputWrongs1.text = FirstPlayerWrongAnswers.ToString();
            InputWrongs2.text = SecoundPlayerWrongAnswers.ToString();
            InputTime1.text = firstPTime.ToString();
            InputTime2.text = secoundPTime.ToString();
            P1Name.text = firstPlayerName;
            P2Name.text = secoundPlayerName;
        }
        else
        {
            InputGrade1.text = secoundPlayerGrade.ToString();
            InputGrade2.text = firstPlayerGrade.ToString();
            InputWrongs1.text = SecoundPlayerWrongAnswers.ToString();
            InputWrongs2.text = FirstPlayerWrongAnswers.ToString();
            InputTime1.text = secoundPTime.ToString();
            InputTime2.text = firstPTime.ToString();
            P1Name.text = secoundPlayerName;
            P2Name.text = firstPlayerName;
            
        }

        // show it in the�game:
        if (timer == 0)
        {
            InputTime1.text = "";
            InputTime2.text = "";
            time01.text = "";
            time02.text = "";

        }
����}


    public void SwitchTurn()
    {
        if (turnToPlay == "first")// if its blue turn we switch to yellow.
        {
            turnToPlay = "secound";
        }
        else // if its yellow turn we switch to blue.
        {
            turnToPlay= "first";
        }
    }

    public void AddSelected()
    {
        foreach (AnswerScript ans in answerOnScreen)
        {
            ans.shadow.SetActive(false);
        }
    }

    public void playbg()
    {
        bgsound.Play();
        StopAudio.SetActive(true);
        PlayAudio.SetActive(false);
    }
    public void stopbg()
    {
        bgsound.Pause();
        StopAudio.SetActive(false);
        PlayAudio.SetActive(true);
    }

    public void PauseGame()// ���� �� �����
    {
        Time.timeScale = 0;
        PauseMenuScreen.SetActive(true);//������ ���� ���� ������ ������ ������
        stopbg();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenuScreen.SetActive(false);//������� ���� ���� ���� ���� �� ���� ������ ����
        playbg();
    }


    public void startOver()//���� ���� ����� ���� ������ ������
    {
        myActiveGame = BeckUpGame;
        //myQuestions = AnsweredQuestions;
        //if(myQuestions.Count == AnsweredQuestions.Count)
        //{
        //    AnsweredQuestions.Clear();
        //}
        Debug.Log(myActiveGame);
        questionTextBox.text = AnsweredQuestions[currentQuestion].content;
        timer = myActiveGame.time_to_answer;
        timeForQuestion = myActiveGame.time_to_answer;
        Debug.Log("myQs"+myQuestions.Count);
        Debug.Log("my active game"+myActiveGame.game_questions.Count);
        //this.gameObject.SetActive(false);
        endGameCanvas.SetActive(false);
        FirstPlayerScore = 0;
        firstPlayerTotalTime = 0;
        FirstPlayerWrongAnswers = 0;
        FirstPlayerNumOfQuestions = 0;
        //amountOfQuestions = 0;
        SecoundPlayerScore = 0;
        secoundPlayerTotalTime = 0;
        SecoundPlayerWrongAnswers = 0;
        SecoundPlayerNumOfQuestions = 0;//��� �����
        GetGame(BeckUpGame);
        flag = false;
       
        StartGame();


        //ChoosePlayere2.SetActive(true);
        //ChosePlayerScript.RandomBTN.gameObject.SetActive(true);

        //ClearThenGet();
        //myQuestions = myActiveGame.game_questions;

        //ChosePlayerScript.TaskOnClick1();//���� �� �����
    }



    //}
    //public void playAgain()//���� �� ����� ����� �������� ����� ����
    //{
    //    FirstPlayerScore = 0;
    //    firstPlayerTotalTime = 0;
    //    FirstPlayerWrongAnswers = 0;
    //    FirstPlayerNumOfQuestions = 0;
    //    amountOfQuestions = 0;
    //    SecoundPlayerScore = 0;
    //    secoundPlayerTotalTime = 0;
    //    SecoundPlayerWrongAnswers = 0;
    //    SecoundPlayerNumOfQuestions = 0;


    //    StartGame();
    //}

    public void NewGame()//���� ���� ������ ���� ���
    {
        SceneManager.LoadScene("Level102");
    }



}




public class GameInfo
{

    public int id;
    public string game_name;
    public int time_to_answer;
    public bool is_publish;
    public int game_code;
    public List<QuestionData> game_questions;

    public static GameInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameInfo>(jsonString);
    }
}

    [System.Serializable]
public class QuestionData
{
    public int id; // �.� �� �����.
    public bool imag_or_text;// �� �� ��� - �� �����, ���� �� ����.
    public string content;    // ���� ���� ������: ���� �� �����
    public int game_id; // �.� �� ����� ������ �����
    public List<Answer> answers;    // ���� ���� ������: ����� �� ������ ���� ���� ����
    

}

[System.Serializable]
public class Answer
{
    public int id; // �.� �� �����
    public int question_id; // �.� �� ����� �� ��� �����.
    public string content; // ���� ���� ����: ����� ����� ������
    public bool imag_or_text; // �� ��� - �� �����, ���� �� ����
    public Sprite img_cont; // ���� ���� ����: ���� ������ ����� ������ 
    public bool is_correct; // ���� ���� ����: ��� ������ ����� �� ��

}


