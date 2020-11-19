namespace UnityEngine.PlayerIdentity.UI
{
    public class ForgetPasswordPanel : AbstractEmailFieldPanel
    {
        public override void OpenPanel()
        {
            base.OpenPanel();
            btn.interactable = true;
        }

        public void OnSendClicked()
        {
            if (FormUtils.IsValidEmail(email.text))
            {
                MainController.Instance.ShowLoading(true);
                PlayerIdentityManager.Current.ResetPassword(email.text, sender =>
                {
                    if (sender.error == null)
                    {
                        OnOkResponseReceived();
                    }
                });
                btn.interactable = false;
            }
            else
            {
                MainController.Instance.PopupController.ShowError(new Error() { message = "邮箱地址格式错误" });
            }
        }

        public void OnOkResponseReceived()
        {
            MainController.Instance.ShowLoading(false);
            MainController.Instance.PopupController.ShowInfo("如果该邮箱已被注册为账号，重置密码的邮件已经发送，请查收。");
            MainController.Instance.PanelController.OnBack();
        }
    }
}