using System;
using UnityEngine;

namespace UnityEngine.PlayerIdentity
{
    public class SensitiveWordManager : MonoBehaviour
    {
        private WordsLibrary library;
        private void Awake()
        {
            TextAsset text = Resources.Load<TextAsset>("Worlds");
            var words = text.text.Split('、');
            library = new WordsLibrary(words); //实例化 敏感词库
        }

        public String CheckSensitiveWord(String text)
        {
            ContentCheck check = new ContentCheck(library, text);  //实例化 内容检测类
            var str = check.SensitiveWordsReplace();  //调用 敏感词替换方法 返回处理过的字符串
            return str;
        }

        public Boolean CheckTextSart(String text,String star = "*")
        {
            return text.Contains(star);
        }
    }
}
