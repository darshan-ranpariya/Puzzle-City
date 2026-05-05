using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiCall : MonoBehaviour
{
    public static ApiCall inst;

    private void Awake()
    {
        inst = this;
    }

    IEnumerator checkInternetConnection(Action<bool> action)
    {
            #if isDebug
                    WWW www = new WWW("http://www.triangulargamestudio.com");
            #else
                   WWW www = new WWW("http://52.172.51.202");
            #endif



        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void CallApi(bool isWin)
    {
        Time.timeScale = 0;
        StartCoroutine(checkInternetConnection((isConnected) => {
            // handle connection status here
            if (isConnected)
            {
                print("connected");
                StartCoroutine(Upload(isWin));
            }
            else
            {
                Time.timeScale = 1;
                if (isWin)
                {
                    UIManager.inst.winPanel.Activate();
                }
                else
                {
                    UIManager.inst.gameOverPanel.Activate();
                }
            }
        }));
    }

    IEnumerator Upload(bool isWin)
    {

        WWWForm form = new WWWForm();
        form.AddField("customer_id", UserStatics.customer_id);
        form.AddField("account_id", UserStatics.account_id);
        form.AddField("card_no", UserStatics.card_no);
        form.AddField("site_id", UserStatics.site_id);
        form.AddField("win_status", isWin ? 1 : 0);
        form.AddField("game_id", 4);
        print(string.Format("{0}, {1}, {2}, {3}, {4}", UserStatics.customer_id, UserStatics.account_id, UserStatics.card_no, UserStatics.site_id, isWin));
        using (UnityWebRequest www = UnityWebRequest.Post(Utility.API_URL, form))
        {
            // www.uploadHandler = uH;
            UIManager.inst.loader.SetActive(true);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIManager.inst.loader.SetActive(false);

            }
            else
            {
                print("milan");
                print(www.downloadHandler.text);
                Time.timeScale = 1;
                if (www.downloadHandler.isDone && www.downloadHandler.text != null)
                {
                    UIManager.inst.loader.SetActive(false);

                    string json = www.downloadHandler.text;
                    if (json != null)
                    {
                        WebResponseData data = JsonUtility.FromJson<WebResponseData>(json);
                        if (data != null)
                        {
                            print(data.message);

                            if (data.code == 1)
                            {
                                if (data.message == "1")
                                {
                                    //win
                                    UIManager.inst.winPanel.Activate();
                                    UIManager.inst.gameWinMsg.Value = "CONGRATULATIONS \n YOU HAVE WON 10 PLAY CREDITS!";

                                }
                                else if (data.message == "2")
                                {
                                    //exceed
                                    UIManager.inst.limitExceedPanel.Activate();

                                }
                                else if (data.message == "3")
                                {
                                    UIManager.inst.gameOverPanel.Activate();
                                    UIManager.inst.gameOverMsg.Value = "YOU HAVE EARNED 5 PLAY CREDITS. BETTER LUCK NEXT TIME!";
                                }
                            }
                            else
                            {
                                UIManager.inst.gameOverPanel.Activate();
                                UIManager.inst.gameOverMsg.Value = data.message;
                            }
                        }
                    }
                }
            }
        }
    }
    //resopnse
    /* {
    "code": 1,
    "message": "Success",
    "data": "Transaction is Successfull and your Tx Id is: 1044326"
    }*/
}
