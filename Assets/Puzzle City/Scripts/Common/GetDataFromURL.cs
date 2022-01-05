using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDataFromURL : MonoBehaviour
{
    private void OnEnable()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetParmsAndSave();
#endif
    }
    private void GetParmsAndSave()
    {
        string url = (Debug.isDebugBuild) ? "http://www.example.com?customer_id=123&card_no=visa&account_id=123&site_id=123&win_status=0&game_id=3&win_score=20&win_time=30" : Application.absoluteURL;

        url = Application.absoluteURL;

        Dictionary<string, string> pairs = Utility.GetParametersFromURL(url);
        if (pairs != null)
        {
            UserStatics.customer_id = pairs[Utility.customer_id];
            UserStatics.account_id = pairs[Utility.account_id];
            UserStatics.card_no = pairs[Utility.card_no];
            UserStatics.site_id = pairs[Utility.site_id];
            UserStatics.requiredScore = double.Parse(pairs[Utility.req_score]);
            UserStatics.timeToPlay = double.Parse(pairs[Utility.play_time]);
            print("userName :" + UserStatics.customer_id);
        }
    }
}
