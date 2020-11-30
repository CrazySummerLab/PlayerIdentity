using System;
using System.Collections;
using System.Collections.Generic;
using CrazySummerLab.Scripts;
using UnityEngine;
using UnityEngine.PlayerIdentity;
using UnityEngine.PlayerIdentity.UI;

public class SampleScene : MonoBehaviour
{
    private const String _title = "游戏温馨提示";

    [SerializeField] private PlayerIdentityCore _playerIdentityCore;
    [SerializeField] private LoginStatusController _loginStatus;
    [SerializeField] private GameObject _antiAddictionPanel;

    private Boolean isLoginStatus;

    public static Action OnNextScreens;
    public Boolean IsCurfew { get; private set; }

    public static SampleScene Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        AntiaddictionManager.OnLoginStatus += OnLoginStatus;
        AntiaddictionManager.OnJudgeTimes += OnJudgeTimes;
        AntiaddictionManager.OnMandatoryOffline += OnMandatoryOffline;
        AntiaddictionManager.OnRealNames += OnRealNames;
        _antiAddictionPanel.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        AntiaddictionManager.OnLoginStatus -= OnLoginStatus;
        AntiaddictionManager.OnJudgeTimes -= OnJudgeTimes;
        AntiaddictionManager.OnMandatoryOffline -= OnMandatoryOffline;
        AntiaddictionManager.OnRealNames -= OnRealNames;
    }

    private void CheckLoginStatus()
    {
        _playerIdentityCore.gameObject.SetActive(true);
        ShowPlayerIdentity(false);
        StartCoroutine(DelayCheckNetwork());
    }

    private void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //not network
            AntiaddictionManager.Instance.ShowPopUp(_title, "您当前未连接网络,请您连接网络后重试!", () =>
            {
                StartCoroutine(DelayCheckNetwork());
            });
        }
        else
        {
            if (!isLoginStatus)
                ShowPlayerIdentity(true);
        }
    }

    private IEnumerator DelayCheckNetwork()
    {
        yield return new WaitForSeconds(1f);
        CheckNetwork();
    }

    private void ShowPlayerIdentity(Boolean isShow)
    {
        _loginStatus.gameObject.SetActive(isShow);
    }

    private void OnLoginStatus(Boolean isLogin)
    {
        isLoginStatus = isLogin;
        if (isLogin)
        {
            ShowPlayerIdentity(false);
            JudgePay();
        }
    }

    private void OnJudgeTimes(int times)
    {
        Debug.Log("AntiAddictionControl OnJudgeTimes: " + times);
    }

    private void OnRealNames(bool isRealName)
    {
        //false-no realname,true-yes realname
        Debug.Log("AntiAddictionControl OnRealNames: " + isRealName);
        if (!isRealName)
        {
            //Cannot game without real-name authentication
            AntiaddictionManager.Instance.ShowPopUp(_title, "您当前未实名认证,请您实名认证后再次进入游戏。");
        }
    }

    private void OnMandatoryOffline(string title, string msg)
    {
        //The user needs to add the panel by himself to add the returned title and msg information to cover the panel
        Debug.Log("AntiAddictionControl OnMandatoryOffline: " + title + ", msg: " + msg);
        IsCurfew = true;
        AntiaddictionManager.Instance.ShowPopUp(title, msg);
    }

    private void JudgePay()
    {
        AntiaddictionManager.Instance.JudgePay(result =>
        {
            if (result == AntiaddictionType.USER_AGE_ZERO_IN_EiGHT
            || result == AntiaddictionType.USER_AGE_UNKNOWN)
            {
                //Antiaddiction user's age is in [0,8) an age unkown,not game
                AntiaddictionManager.Instance.ShowPopUp(_title, "您未满8岁,不能进行游戏。");
            }
            else
            {
                //enter the game
                OnNextScreens();
            }
        });
    }

    public void CheckAntiaddction()
    {
        _antiAddictionPanel.gameObject.SetActive(false);
        CheckLoginStatus();
    }
}
