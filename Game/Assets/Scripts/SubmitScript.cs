using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class SubmitScript : MonoBehaviour
{

    /// <summary>
    /// ����� ����� ��� ������
    /// </summary>
    public GameObject nextButton;
    /// <summary>
    /// ��� ������ ���� ������
    /// </summary>
    public bool userIsCorrect;
    /// <summary>
    /// ��� �� ���� �� �� ������
    /// </summary>
    public bool wasItClicked = false;

    //public GameObject yes;
    //public GameObject no;

    public GameObject BG;
    public int progressCount = 0;// ����� ������� �����.
    public GameObject Xindicator;
    public GameObject Vindicator;
    public GameManagerScript gameManagerScript;




    private void OnMouseDown()// ������...
    {
        gameManagerScript.timeIsRunning = false;// ����� �� ������
        if (userIsCorrect)// �� ������ ��� �� ��
        {

            Vindicator.SetActive(true);
            wasItClicked = true;// ���� ��� ����
            nextButton.SetActive(true);// ����� ���� ����
            progressCount++;
            gameManagerScript.Score();// ���� �� ������ �� ������� �� �������.
            
        }
        else
        {
            Xindicator.SetActive(true);
            wasItClicked = true;// ���� ��� ����
            nextButton.SetActive(true);// ����� ���� ����
            gameManagerScript.Score();// ���� �� ������ �� ������� �� ������.


        }
        gameObject.SetActive(false);// ����� �� ������

    }

    



}
