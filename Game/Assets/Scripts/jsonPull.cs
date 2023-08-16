using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;
using static System.Net.WebRequestMethods;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class jsonPull : MonoBehaviour
{
    public string gameCodeinput;
    public TMP_InputField gameCode;
    public Image imageTry;
    public GameInfo myGame;

    public GameManagerScript myGameManager;
    public GameObject activateGame;
    public GameObject ChoosePlayere;
    public GameObject Canvas;

    private string urlPull;


    //public void CheckGameNum()
    //{
    //    gameCodeinput = gameCode.text;

    //    urlPull = Application.dataPath + "/../Editor";

    //    StartCoroutine(GetRequest(urlPull + "/api/Unity/" + gameCodeinput));

    //}


    public void CheckGameNum()
    {
        gameCodeinput = gameCode.text;
        string x = "https://localhost:7265/api/Unity/";
        Debug.Log(x + gameCodeinput);

        StartCoroutine(GetRequest(x + gameCodeinput));
    }

    IEnumerator GetRequest(string uri)
    {
        //Using creates an instance that doesnt stay after it runs;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    myGame = GameInfo.CreateFromJSON(webRequest.downloadHandler.text);

                    StartCoroutine(GetRequestPic(StartAfterGameLoaded));


                    break;

            }

        }
    }



    private IEnumerator GetRequestPic(Action onComplete)
    {

        for (int j = 0; j < myGame.game_questions.Count; j++)
        {
            for (int i = 0; i < myGame.game_questions[j].answers.Count; i++)
            {
                if (myGame.game_questions[j].answers[i].imag_or_text == true) //ווידוא שאכן יש תמונה לתשובה הזאת לאחר שווידאנו בתנאי הקודם שאין בה טקסט
                {
                    string url = "https://localhost:7265/" + myGame.game_questions[j].answers[i].content;
                    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                    yield return www.SendWebRequest();

                    string[] pages = url.Split('/');
                    int page = pages.Length - 1;

                    switch (www.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogError(pages[page] + ": Error: " + www.error);
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogError(pages[page] + ": HTTP Error: " + www.error);
                            break;
                        case UnityWebRequest.Result.Success:
                            Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
                            Sprite webSprite = SpriteFromTexture2D(webTexture);
                            myGame.game_questions[j].answers[i].img_cont = webSprite;

                            break;

                    }
                }
            }
        }
        onComplete?.Invoke();
    }




    //private IEnumerator GetRequestPic(Action onComplete)
    //{

    //    for (int j = 0; j < myGame.game_questions.Count; j++)
    //    {
    //        for (int i = 0; i < myGame.game_questions[j].answers.Count; i++)
    //        {
    //            if (myGame.game_questions[j].answers[i].imag_or_text == true) //ווידוא שאכן יש תמונה לתשובה הזאת לאחר שווידאנו בתנאי הקודם שאין בה טקסט
    //            {
    //                string url = urlPull + "/uploadedFiles/" + myGame.game_questions[j].answers[i].content;
    //                UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    //                yield return www.SendWebRequest();

    //                string[] pages = url.Split('/');
    //                int page = pages.Length - 1;

    //                switch (www.result)
    //                {
    //                    case UnityWebRequest.Result.ConnectionError:
    //                    case UnityWebRequest.Result.DataProcessingError:
    //                        Debug.LogError(pages[page] + ": Error: " + www.error);
    //                        break;
    //                    case UnityWebRequest.Result.ProtocolError:
    //                        Debug.LogError(pages[page] + ": HTTP Error: " + www.error);
    //                        break;
    //                    case UnityWebRequest.Result.Success:
    //                        Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
    //                        Sprite webSprite = SpriteFromTexture2D(webTexture);
    //                        myGame.game_questions[j].answers[i].img_cont = webSprite;

    //                        break;

    //                }
    //            }
    //        }
    //    }
    //    onComplete?.Invoke();
    //}


    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 450.0f);
    }


    void StartAfterGameLoaded()
    {
        ChoosePlayere.SetActive(true);
        //activateGame.SetActive(true);
        myGameManager.GetGame(myGame);
        this.gameObject.SetActive(false);
        Canvas.SetActive(false);
    }
}