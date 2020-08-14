using System.Collections;
using System.Collections.Generic;
using CrazySummerLab.Scripts;
using UnityEngine;
using UnityEngine.PlayerIdentity;

public class SampleScene : MonoBehaviour
{
    private void Awake()
    {
        AntiaddictionManager.OnLoginStatus += OnLoginStatus;
        AntiaddictionManager.OnJudgeTimes += OnJudgeTimes;
        AntiaddictionManager.OnAntiAddictionUserAge += OnAntiAddictionUserAge;
        AntiaddictionManager.OnMandatoryOffline += OnMandatoryOffline;
    }

    private void OnMandatoryOffline(string title,string msg)
    {
        //The user needs to add the panel by himself to add the returned title and msg information to cover the panel
        Debug.Log("SampleSceen OnMandatoryOffline: " + title + ", msg: " + msg);
    }

    private void OnAntiAddictionUserAge(AntiaddictionType antiaddictionType)
    {
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
}
