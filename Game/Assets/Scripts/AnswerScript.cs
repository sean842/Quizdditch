using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    /// <summary>
    /// הטקסט עצמו בתוך האנסרסקריפט
    /// </summary>
    public TextMeshPro answerConText;
    /// <summary>
    /// גישה לתמונה בתוך אנסרסקריפט
    /// </summary>
    public SpriteRenderer answerConPic;
    /// <summary>
    /// גישה לכפתור בתוך אנסרסקריפט
    /// </summary>
    public SubmitScript submitBTN;
    /// <summary>
    /// האם התשובה נכונה או לא בתוך אנסרסקריפט
    /// </summary>
    public bool isCorrect;
    public GameObject Xindicator; 
    public GameObject Vindicator;
    public GameObject shadow;
    public GameManagerScript gameManager;

    public bool img = false;
    public float maxScaleX = 10.0f; // The maximum scale along the X-axis.
    public float maxScaleY = 10.0f; // The maximum scale along the Y-axis.
    private Vector3 originalScale;


    private void Start()
    {
        Vindicator.SetActive(false);
        Xindicator.SetActive(false) ;
        // Store the original scale for later use.
        originalScale = answerConPic.transform.localScale;
    }


    public void ChangeSize()
    {
        // Scale the sprite renderer along the X and Y axes.
        Vector3 newScale = new Vector3(answerConPic.transform.localScale.x * maxScaleX, answerConPic.transform.localScale.y * maxScaleY, 1f);

        // Check if the new scale exceeds the maximum allowed values.
        newScale.x = Mathf.Clamp(newScale.x, originalScale.x, originalScale.x * maxScaleX);
        newScale.y = Mathf.Clamp(newScale.y, originalScale.y, originalScale.y * maxScaleY);

        answerConPic.transform.localScale = newScale;
    }

    private void OnMouseOver()
    {
        if (img)
        {
            ChangeSize();
        }
    }

    private void OnMouseExit()
    {
        ResetSize();
    }
    public void ResetSize()
    {
        // Reset the sprite renderer scale to its original size.
        answerConPic.transform.localScale = originalScale;
    }

    //private void Update()
    //{
    //    // Get the mouse scroll wheel input
    //    float scroll = Input.GetAxis("Mouse ScrollWheel");

    //    // Calculate the new orthographic size (zoom level)
    //    float newZoom = mainCamera.orthographicSize - scroll * zoomSpeed;

    //    // Clamp the new zoom level to the minZoom and maxZoom values
    //    newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

    //    // Update the camera's orthographic size
    //    mainCamera.orthographicSize = newZoom;
    //}


    private void OnMouseDown()
    {
        //בלחיצת כפתור אני אבדוק האם הכפתור לא נלחץ
        if (!submitBTN.wasItClicked)
        {
            submitBTN.gameObject.SetActive(true);// מדליקים את כפתור הבדיקה שלנו
            submitBTN.userIsCorrect = isCorrect; //מעבירים לכפתור בדיקה האם המשתמש צדק או לא
            submitBTN.Vindicator = Vindicator;
            submitBTN.Xindicator = Xindicator;
        }
        //gameManager.timeIsRunning = false;// לעצור את הטיימר
        gameManager.AddSelected();
        shadow.SetActive(true);

    }





}
