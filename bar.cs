using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
//change through mergedroid
using GitMasterGui;
using GitMasterGui.Configuration;
using GitMasterGui.License;
using GitMasterGui.LoginWindow;
using GitMaster.UI;
using GitMaster.UI.Web;
using WpfUI;
using GitMasterGui.Api;

namespace GitMaster.LoginWindow
{
    internal class LoginPanel : DockPanel
    {
        internal LoginPanel(
            IGitMasterRestApi restApi,
            ILoginWindow loginWindow,
            Login.ILoginNotifier loginSuccessNotifier)
        {
            mRestApi = restApi;

            mLoginWindow = loginWindow;

            mLoginSuccessNotifier = loginSuccessNotifier;

            BuildComponents();

            mUserTextBox.Focus();
        }

        internal void Dispose()
        {
            mSignUpLinkLabel.HyperLink.Click -= SignUpLinkLabel_Click;
            mLoginButton.Click -= LoginButton_Click;
            mPasswordTextBox.Dispose();
        }

        internal string GetTeamInvitationCode()
        {
            return mTeamInvitationCodeTextBox.Text;
        }

        internal void NotifyError(string message)
        {
            mMascotImage.Source = GitMasterImages.GetImage(
                GitMasterImages.ImageName.IllustrationSignupError);

            mWebErrorPanel.ShowError(message);
        }

        void SignUpLinkLabel_Click(object sender, RoutedEventArgs e)
        {
            mLoginWindow.ShowSignUpPanel();
        }

        void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            LoginConfiguration.Data data = LoginConfiguration.Data.Build(
                mUserTextBox.Text,
                mPasswordTextBox.Text);

            Login.ValidationResult result = Login.Validate(data);

            if (!result.IsOk())
            {
                ShowErrors(result);
                return;
            }

            Login.Run(mRestApi, data, mProgressControls, mLoginSuccessNotifier);
        }

        void ShowErrors(Login.ValidationResult result)
        {
            if (!string.IsNullOrEmpty(result.EncryptedPasswordError))
            {
                mPasswordTextBox.HasValidationError = true;
                mPasswordTextBox.Focus();
                mPasswordErrorLabel.Text = result.EncryptedPasswordError;
                mPasswordErrorLabel.Visibility = Visibility.Visible;
            }

            if (!string.IsNullOrEmpty(result.UserError))
            {
                mUserTextBox.HasValidationError = true;
                mUserTextBox.Focus();
                mUserErrorLabel.Text = result.UserError;
                mUserErrorLabel.Visibility = Visibility.Visible;
            }
        }

        void ClearErrors()
        {
            mUserTextBox.HasValidationError = false;
            mUserErrorLabel.Text = string.Empty;
            mUserErrorLabel.Visibility = Visibility.Collapsed;

            mPasswordTextBox.HasValidationError = false;
            mPasswordErrorLabel.Text = string.Empty;
            mPasswordErrorLabel.Visibility = Visibility.Collapsed;
        }

        void BuildComponents()
        {
            mMascotImage = ControlBuilder.CreateImage(
                GitMasterImages.GetImage(
                    GitMasterImages.ImageName.IllustrationLoginSkater));
            mMascotImage.Width = 330;
            mMascotImage.Margin = new Thickness(10, 55, 30, 50);

            WebEntriesPacker.AddMascotContentComponents(
                this, mMascotImage, CreateContentPanel());
        }

        Panel CreateContentPanel()
        {
            StackPanel result = new StackPanel();

            ValidationProgressPanel validationPanel = new ValidationProgressPanel();

            Panel headerPanel = CreateHeaderPanel();

            mWebErrorPanel = new WebErrorPanel();

            mUserTextBox = WebControlBuilder.CreateTextBox(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.LoginPanelUserWatermark));

            mUserErrorLabel = WebControlBuilder.CreateErrorLabel();

            mPasswordTextBox = WebControlBuilder.CreatePasswordTextBox(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.LoginPanelPasswordWatermark));

            mPasswordErrorLabel = WebControlBuilder.CreateErrorLabel();

            mTeamInvitationCodeTextBox = WebControlBuilder.CreateTextBox(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.LoginPanelTeamInvitationCodeWatermark));

            mLoginButton = WebControlBuilder.CreateMainActionButton(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.LoginButtonUppercase));
            mLoginButton.Click += LoginButton_Click;

            WebEntriesPacker.AddRelatedComponents(
                result,
                headerPanel,
                mWebErrorPanel,
                mUserTextBox,
                mUserErrorLabel,
                mPasswordTextBox,
                mPasswordErrorLabel,
                mTeamInvitationCodeTextBox,
                validationPanel,
                mLoginButton);

            mProgressControls = new ProgressControlsForDialogs(
                validationPanel, new UIElement[] { mLoginButton });

            mLoginButton.IsDefault = true;

            return result;
        }

        Panel CreateHeaderPanel()
        {
            TextBlock titleTextBlock = WebControlBuilder.CreateTitle(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.LoginPanelTitle));

            mSignUpLinkLabel = WebControlBuilder.CreateLinkLabel(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.SignUpLinkLabel));
            mSignUpLinkLabel.HyperLink.Click += SignUpLinkLabel_Click;

            return WebEntriesPacker.CreateHeaderPanel(
                titleTextBlock, mSignUpLinkLabel);
        }

        IProgressControlsForDialogs mProgressControls;
        LinkLabel mSignUpLinkLabel;
        Image mMascotImage;
        WebErrorPanel mWebErrorPanel;
        WebTextBox mUserTextBox;
        SelectableLabel mUserErrorLabel;
        PasswordWebTextBox mPasswordTextBox;
        SelectableLabel mPasswordErrorLabel;
        WebTextBox mTeamInvitationCodeTextBox;
        Button mLoginButton;
        Login.ILoginNotifier mLoginSuccessNotifier;

        ILoginWindow mLoginWindow;

        IGitMasterRestApi mRestApi;
    }
}
