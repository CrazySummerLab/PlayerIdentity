namespace UnityEngine.PlayerIdentity.UI
{
    public class SignUpPanel : AbstractPasswordConfirmationPanel
    {
        public void OnSignUpClicked()
        {
            if (FormUtils.IsValidEmail(email.text))
            {
                if (!m_MainController.SensitiveWordManager.CheckTextSart(displayName.text))
                {
                    if (FormUtils.IsValidConfirmationPassword(password.text, passwordConfirmation.text))
                    {
                        m_MainController.ShowLoading(true);
                        PlayerIdentityManager.Current.Register(email.text, password.text, result =>
                        {
                            if (result.error == null && !string.IsNullOrWhiteSpace(displayName.text))
                            {
                                UpdateAccountPanel.UpdateDisplayName(displayName.text);
                            }
                        });
                    }
                    else
                    {
                        m_MainController.PopupController.ShowError(new Error() { message = "密码不匹配" });
                    }
                }
                else
                {
                    m_MainController.PopupController.ShowError(new Error() { message = "昵称格式错误" });
                }
            }
            else
            {
                m_MainController.PopupController.ShowError(new Error() { message = "邮箱地址格式错误" });
            }
        }
    }
}