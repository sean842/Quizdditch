using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScript : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ������ ���'�
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
            //����� ������ ������ ����� ���� �� ����� ������ ����.
            gameManager.AnsweredQuestions.Add(gameManager.myQuestions[gameManager.currentQuestion]);
            //���� �� ����� ������ ���� ���� ������
            gameManager.myQuestions.Remove(gameManager.myQuestions[gameManager.currentQuestion]);

        }
        //gameManager.SwitchTurn();
        gameManager.ClearThenGet();// ���� �� �������� ����� �� ���� ������ �� ����� ��� �����
        
        gameManager.updateTimer(gameManager.timer);
        
    }
}
