using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class SubmitScript : MonoBehaviour
{

    /// <summary>
    /// כפתור הנקסט שלי בכפתור
    /// </summary>
    public GameObject nextButton;
    /// <summary>
    /// האם המשתמש צודק בכפתור
    /// </summary>
    public bool userIsCorrect;
    /// <summary>
    /// האם זה נלחץ או לא בכפתור
    /// </summary>
    public bool wasItClicked = false;

    //public GameObject yes;
    //public GameObject no;

    public GameObject BG;
    public int progressCount = 0;// לבדוק התקדמות במשחק.
    public GameObject Xindicator;
    public GameObject Vindicator;
    public GameManagerScript gameManagerScript;




    private void OnMouseDown()// בלחציה...
    {
        gameManagerScript.timeIsRunning = false;// לעצור את הטיימר
        if (userIsCorrect)// אם המשתמש צדק או לא
        {

            Vindicator.SetActive(true);
            wasItClicked = true;// אומר שזה נלחץ
            nextButton.SetActive(true);// כפתור נקסט פועל
            progressCount++;
            gameManagerScript.Score();// נעלה את הניקוד או הטעויות של השחקנים.
            
        }
        else
        {
            Xindicator.SetActive(true);
            wasItClicked = true;// אומר שזה נלחץ
            nextButton.SetActive(true);// כפתור נקסט פועל
            gameManagerScript.Score();// נעלה את הניקוד או הטעויות של השחקים.


        }
        gameObject.SetActive(false);// תעלים את הכפתור

    }

    



}
