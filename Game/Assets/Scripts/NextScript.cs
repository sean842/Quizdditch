using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScript : MonoBehaviour
{
    /// <summary>
    /// גישה לקוד שבגיים מנג'ר
    /// </summary>
    public GameManagerScript gameManager;
    public SubmitScript sub;
    
    private void Start()
    {
        
    }
    private void OnMouseDown()
    {
        if (sub.userIsCorrect == true)
        {
            //נוסיף לרשימת השאלות שנענו נכון את השאלה שענינו כרגע.
            gameManager.AnsweredQuestions.Add(gameManager.myQuestions[gameManager.currentQuestion]);
            //נסיר את השאלה שענינו כרגע מתוך הרשימה
            gameManager.myQuestions.Remove(gameManager.myQuestions[gameManager.currentQuestion]);

        }
        //gameManager.SwitchTurn();
        gameManager.ClearThenGet();// תריץ את הפונקציה שמנקה את המסך ובודקת אם נשארו עוד שאלות
        
        gameManager.updateTimer(gameManager.timer);
        
    }
}
