using System;
using System.Collections;
using System.Text.RegularExpressions;
using CrazySummerLab.Scripts.Extentions;
using UnityEngine;
using UnityEngine.PlayerIdentity;

namespace CrazySummerLab.Scripts
{
    public class AntiaddictionManager : MonoBehaviour
    {
        private AntiAddictionClientApi _antiAddictionClientApi;
        private bool flag = false;
        private LoginStatus _loginStatusDefault;
        private Action<AntiaddictionType> _antiaddictionUserAge;

        public static Action<Int32> OnJudgeTimes;
        public static Action<LoginStatus> OnLoginStatus;
        public static Action<AntiaddictionType> OnAntiAddictionUserAge;
        public static Action<String, String> OnMandatoryOffline;
        public static Action<Boolean> OnRealNames;

        private Regex regEnglish;

        public static AntiaddictionManager Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            regEnglish = new Regex("^[a-zA-Z]");
            _antiAddictionClientApi = GetComponent<AntiAddictionClientApi>();
            _antiAddictionClientApi.OnKickOff += OnKickOff;
            _antiAddictionClientApi.OnMessage += OnMessage;
            _antiAddictionClientApi.OnJudgePay += OnJudgePay;
            _antiAddictionClientApi.OnJudgeTime += OnJudgeTime;
            _antiAddictionClientApi.OnRealName += OnRealName;
            StartCoroutine(Beat());
        }

        private void OnDestroy()
        {
            if (_antiAddictionClientApi != null)
            {
                _antiAddictionClientApi.OnKickOff -= OnKickOff;
                _antiAddictionClientApi.OnMessage -= OnMessage;
                _antiAddictionClientApi.OnJudgePay -= OnJudgePay;
                _antiAddictionClientApi.OnJudgeTime -= OnJudgeTime;
                _antiAddictionClientApi.OnRealName -= OnRealName;
            }
        }

        private void Update()
        {
            StartCoroutine(CheckLogin(1f));
        }

        private IEnumerator CheckLogin(float delaySeconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(delaySeconds);
                if(_loginStatusDefault != PlayerIdentityManager.Current.loginStatus)
                {
                    _loginStatusDefault = PlayerIdentityManager.Current.loginStatus;
                    flag = false;
                    if (_loginStatusDefault == LoginStatus.AnonymouslyLoggedIn || _loginStatusDefault == LoginStatus.LoggedIn)
                        flag = true;

                    if (_loginStatusDefault == LoginStatus.LoggedIn)
                        JudgePay();

                    if (_loginStatusDefault != LoginStatus.LoginInProgress)
                        OnLoginStatus.SafeInvoke(_loginStatusDefault);
                }
            }
        }

        private IEnumerator Beat()
        {
            while (true)
            {
                if (flag)
                {
                    _antiAddictionClientApi.ContinueHeartbeat("continue beat");
                    yield return new WaitForSeconds(250f);
                }
                yield return null;
            }
        }

        private void OnKickOff(string traceId, string ruleName)
        {
            Debug.Log("Antiaddiction OnKickOff :" + traceId+", ruleName :"+ruleName);
            ReportExectuion(traceId, ruleName);
            PlayerIdentityManager.Current.Logout();
        }

        private void OnMessage(string title, string msg, string context)
        {
            Debug.Log("Antiaddiction OnMessage: " + title + ", msg: " + msg + ", context: " + context);
            if (regEnglish.IsMatch(title)) return;
            if (String.IsNullOrEmpty(msg)) return;
            if (regEnglish.IsMatch(msg)) return;
            OnMandatoryOffline.SafeInvoke(title, msg);
        }

        private void OnJudgePay(int age)
        {
            Debug.Log("Antiaddiction OnJudgePay: " + age);
            AntiaddictionType _type;
            switch (age)
            {
                case (-1):
                    Debug.Log("Antiaddiction user's age is unknown.");
                    _type = AntiaddictionType.USER_AGE_UNKNOWN;
                    break;
                case (1):
                    Debug.Log("Antiaddiction user's age is in [0,8)");
                    _type = AntiaddictionType.USER_AGE_ZERO_IN_EiGHT;
        
                    break;
                case (8):
                    Debug.Log("Antiaddiction user's age is in [8,16)");
                    _type = AntiaddictionType.USER_AGE_EIGHT_IN_SIXTEEN;
                    break;
                case (16):
                    Debug.Log("Antiaddiction user's age is in [16,18)");
                    _type = AntiaddictionType.USER_AGE_SIXTEEN_IN_EIGHTEEN;
                    break;
                default:
                    Debug.Log("Antiaddiction user's age is in more than 18");
                    _type = AntiaddictionType.USER_AGE_ADULT;
                    break;
            }
            OnAntiAddictionUserAge.SafeInvoke(_type);
            _antiaddictionUserAge.SafeInvoke(_type);
            _antiaddictionUserAge = null;
        }

        private void OnJudgeTime(int duration)
        {
            Debug.Log("Antiaddiction OnJudgeTime: " + duration);
            int hours = duration / 3600;
            int minutes = (duration - hours * 3600) / 60;
            int seconds = duration % 60;
            OnJudgeTimes.SafeInvoke(duration);
            Debug.Log("Antiaddiction OnJudgeTime current play time: " + hours + "h" + minutes + "min" + seconds + "s");
        }

        private void ReportExectuion(string traceId, string ruleName)
        {
            var timedelta = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long time = timedelta.Ticks;
            long timeStamp = long.Parse(time.ToString().Substring(0, time.ToString().Length - 4));
            Debug.Log("Antiaddiction ReportExectuion timeStamp: " + timeStamp);
            _antiAddictionClientApi.ReportExecution(traceId, ruleName, timeStamp, "ReportExecution");
        }

        private void JudgePay()
        {
            _antiAddictionClientApi.JudgePay();
        }

        public void OnRealName(Boolean isrealName)
        {
            //isrealname-false not real name，true-yes real name
            Debug.Log("Antiaddiction is Real Name: " + isrealName);
            OnRealNames.SafeInvoke(isrealName);
        }

        public void JudgePay(Action<AntiaddictionType> userIsAdultCallback)
        {
            _antiaddictionUserAge = userIsAdultCallback;
            JudgePay();
        }


    }
}
