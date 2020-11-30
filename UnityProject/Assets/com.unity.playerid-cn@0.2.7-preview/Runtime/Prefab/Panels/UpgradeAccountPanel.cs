namespace UnityEngine.PlayerIdentity.UI
{
    public class UpgradeAccountPanel : AbstractPasswordConfirmationPanel
    {
        public void OnUpgradeAccountClicked()
        {
            if (FormUtils.IsValidEmail(email.text))
            {
                if (!m_MainController.SensitiveWordManager.CheckTextSart(displayName.text))
                {
                    if (FormUtils.IsValidConfirmationPassword(password.text, passwordConfirmation.text))
                    {
                        m_MainController.ShowLoading(true);
                        PlayerIdentityManager.Current.CreateCredential(email.text, password.text, result =>
                        {
                            if (result.error == null)
                            {
                                if (!string.IsNullOrWhiteSpace(displayName.text))
                                {
                                    UpdateAccountPanel.UpdateDisplayName(displayName.text,
                                        (_) =>
                                        {
                                            m_MainController.OnUpgradeAccountComplete();
                                        });
                                }
                                else
                                {
                                    m_MainController.OnUpgradeAccountComplete();
                                }
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
                    m_MainController.PopupController.ShowError(new Error() { message = "用户名格式错误" });
                }
            }
            else
            {
                m_MainController.PopupController.ShowError(new Error() { message = "邮箱地址格式错误" });
            }
        }
    }
}