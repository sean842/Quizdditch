using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class ChosePlayerScript : MonoBehaviour
{
    // בלחיצה על השחן הכחול, השחקן שיתחיל יהיה הכחול, זאת אומרת שבמשחק עצמו נשים משתנה בוליאני שאם הוא חיובי הכחול יתחיל ואם הוא שלילי אצהוב יתחיל. צריך להעביר ערך לשחקן הראשון והשני במשחק

    //public bool names = false;// to know if we have names.
    //public Button BlueCharacter;
    //public Button YellowCharacter;
    public Image BlueP;
    public Image YellowP;
    public TMP_InputField inputblueName;
    public TMP_InputField inputyellowName;
    public static string blueName;
    public static string yellowName;
    //public TextMeshPro Title;// get the name of the game
    public Button Continue;
    public Button RandomBTN;
    public Button StartBTN;

    //public Button RandomBTN2;
    //public Button StartBTN2;

    public TMP_Text Title;
    public TMP_Text Text;
    public int roll;
    /// <summary>
    /// IF it's True, the Blue will start. ELSE the Yellow start.
    /// </summary>
    public static bool whoStart;
    public GameManagerScript myGameManager;
    public jsonPull myJsonPull;
    public GameObject Stage;


    // Start is called before the first frame update
    void Start()
    {

        //Title.text = myJsonPull.myGame.game_name;// get the name of the game.
        Title.text = myJsonPull.myGame.game_name;// get the name of the game.


        // מאפס את הטקסט של השמות
        //inputblueName.text = " ";
        //inputyellowName.text = " ";

    }

    private void Update()
    {
        if (inputblueName.text != "" && inputyellowName.text != "")
        {
            Continue.interactable = true;
        }
    }



    public void TaskOnClick1()
    {
        // השמות של השחקנים ישמרו במשתנה מחרוזתי
        blueName = inputblueName.text;
        yellowName = inputyellowName.text;

        Debug.Log(blueName);
        Debug.Log(yellowName);
        Title.text = "מוכנים לזה?!";
        Text.text = "הסניץ' בורח ועליכם לתפוס אותו. השחקן שיענה נכונה על יותר שאלות יתפוס את הסניץ וינצח במשחק";
        Continue.gameObject.SetActive(false);// hide button.
        RandomBTN.gameObject.SetActive(true);
    }

    public void GetRandP()
    {
        roll = Random.Range(1, 3);
        switch (roll)
        {
            case 1:
                Text.text = blueName + " נבחרת להתחיל!";
                whoStart = true;
                break;
            case 2:
                Text.text = yellowName + " נבחרת להתחיל!";
                whoStart = false;
                break;

        }
        RandomBTN.gameObject.SetActive(false);
        StartBTN.gameObject.SetActive(true);

    }

    public void StartGame()
    {
        this.gameObject.SetActive(false);
        Stage.SetActive(true);
        myGameManager.StartGame();
    }











}