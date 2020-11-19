using System.Collections;
using System.Collections.Generic;
using CrazySummerLab.Scripts;
using UnityEngine;
using UnityEngine.PlayerIdentity;
using UnityEngine.PlayerIdentity.UI;

public class SampleScene : MonoBehaviour
{
    private void Awake()
    {
        AntiaddictionManager.OnLoginStatus += OnLoginStatus;
        AntiaddictionManager.OnJudgeTimes += OnJudgeTimes;
        AntiaddictionManager.OnAntiAddictionUserAge += OnAntiAddictionUserAge;
        AntiaddictionManager.OnMandatoryOffline += OnMandatoryOffline;
        AntiaddictionManager.OnRealNames += OnRealNames;
    }

    private void OnRealNames(bool isRealName)
    {
        //false-no realname,true-yes realname
        Debug.Log("SampleSceen OnRealNames: " + isRealName);
    }

    private void OnMandatoryOffline(string title,string msg)
    {
        //The user needs to add the panel by himself to add the returned title and msg information to cover the panel
        Debug.Log("SampleSceen OnMandatoryOffline: " + title + ", msg: " + msg);
        MainController.Instance.PopupController.ShowInfo(title + "&" + msg);
    }

    private void OnAntiAddictionUserAge(AntiaddictionType antiaddictionType)
    {
        switch (antiaddictionType)
        {
            case AntiaddictionType.USER_AGE_ZERO_IN_EiGHT:
                MainController.Instance.PopupController.ShowInfo("年龄0,8岁,不能进行游戏");
                QuitGame();
                break;
            case AntiaddictionType.USER_AGE_EIGHT_IN_SIXTEEN:
                MainController.Instance.PopupController.ShowInfo("年龄8,16岁,未成年人");
                break;
            case AntiaddictionType.USER_AGE_SIXTEEN_IN_EIGHTEEN:
                MainController.Instance.PopupController.ShowInfo("年龄16,18岁,未成年人");
                break;
            case AntiaddictionType.USER_AGE_ADULT:
                MainController.Instance.PopupController.ShowInfo("年龄超过18岁,已成年");
                break;
            default:
                MainController.Instance.PopupController.ShowInfo("用户年龄未知");
                QuitGame();
                break;
        }
        Debug.Log("SampleSceen OnAntiAddictionUserAge: " + antiaddictionType.ToString());
    }

    private void OnLoginStatus(LoginStatus loginStatus)
    {
        Debug.Log("SampleSceen OnLoginStatus: "+loginStatus.ToString());
    }

    private void OnJudgeTimes(int times)
    {
        Debug.Log("SampleSceen OnJudgeTimes: " + times);
    }

    public void JudgePay()
    {
        AntiaddictionManager.Instance.JudgePay(result =>
        {
            Debug.Log("SampleSceen JudgePay: " + result.ToString());
        });
    }

    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
