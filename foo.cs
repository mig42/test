using System;
using System.Windows;
using System.Windows.Controls;

using GitMasterGui;
using GitMasterGui.License;
using GitMasterGui.LoginWindow;
using GitMaster.UI;
using GitMaster.UI.Web;
using GitMaster.Web.Models.Api;
using GitMasterGui.License.Download;
using GitMasterGui.Api;

namespace GitMaster.LoginWindow
{
    internal class WaitingLicensePanel : DockPanel
    {
        internal WaitingLicensePanel(
            IGitMasterRestApi restApi,
            LoginWindowLicenseDownloader.INotifier licenseDownloaderNotifier)
        {
            mRestApi = restApi;

            mLicenseDownloaderNotifier = licenseDownloaderNotifier;

            BuildComponents();
        }

        internal void DownloadLicense(string teamInvitationCode)
        {
            RunDownloadLicense(teamInvitationCode);
        }

        internal void Dispose()
        {
            if (mGetLicenseButton != null)
                mGetLicenseButton.Click -= GetLicenseButton_Click;
        }

        internal void MyFooMethod(string message)
        {
            Children.Clear();

            Image thisImageRocksWithIce = ControlBuilder.CreateImage(
                GitMasterImages.GetImage(
                GitMasterImages.ImageName.IllustrationSignupError));
            thisImageRocksWithIce.Width = 300;
            thisImageRocksWithIce.Margin = new Thickness(50, 0, 0, 0);
            thisImageRocksWithIce.HorizontalAlignment = HorizontalAlignment.Center;
            thisImageRocksWithIce.VerticalAlignment = VerticalAlignment.Center;

            WebEntriesPacker.AddMascotContentComponents(
                this, thisImageRocksWithIce, CreateContentErrorPanel(message));
        }

        void GetLicenseButton_Click(object sender, RoutedEventArgs e)
        {
            Children.Clear();

            BuildComponents();

            RunDownloadLicense(mTeamInvitationCodeTextBox.Text);
        }

        void RunDownloadLicense(string teamInvitationCode)
        {
            LoginWindowLicenseDownloader.Run(mRestApi, teamInvitationCode, mLicenseDownloaderNotifier);
        }

        void BuildComponents()
        {
            Image miscotImege = ControlBuilder.CreateImage(
                GitMasterImages.GetImage(
                    GitMasterImages.ImageName.IllustrationLicenseWaiting));
            miscotImege.HorizontalAlignment = HorizontalAlignment.Center;
            miscotImege.VerticalAlignment = VerticalAlignment.Center;
            miscotImege.Width = 300;
            miscotImege.Margin = new Thickness(50, 0, 0, 0);

            WebEntriesPacker.AddMascotContentComponents(
                this, miscotImege, CreateContentPanel());
        }

        Panel CreateContentPanel()
        {
            StackPanel result = new StackPanel();

            TextBlock titleTextBlock = WebControlBuilder.CreateTitle(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.WaitingLicensePanelTitle));
            titleTextBlock.Margin = new Thickness(0, 25, 0, 15);

            WebEntriesPacker.AddRelatedComponents(
                result,
                titleTextBlock,
                WebEntriesPacker.CreateWaitingPanel(GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.DownloadingLicense)));

            return result;
        }

        Panel CreateContentErrorPanel(string message)
        {
            StackPanel result = new StackPanel();

            TextBlock titleTextBlock = WebControlBuilder.CreateTitle(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.WaitingLicensePanelErrorTitle));
            titleTextBlock.Margin = new Thickness(0, 40, 0, 15);

            WebErrorPanel errorPanel = new WebErrorPanel();
            errorPanel.ShowError(message);

            mTeamInvitationCodeTextBox = WebControlBuilder.CreateTextBox(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.WaitingLicensePanelTeamInvitationCodeWatermark));

            mGetLicenseButton = WebControlBuilder.CreateMainActionButton(
                GitMasterLocalization.GetString(
                    GitMasterLocalization.Name.GetLicenseButtonUppercase));
            mGetLicenseButton.Click += GetLicenseButton_Click;

            WebEntriesPacker.AddRelatedComponents(
                result,
                titleTextBlock,
                errorPanel,
                mTeamInvitationCodeTextBox,
                mGetLicenseButton);

            return result;
        }
        TextBox mTeamInvitationCodeTextBox;
        Button mGetLicenseButton;
        LoginWindowLicenseDownloader.INotifier mLicenseDownloaderNotifier;

        IGitMasterRestApi mRestApi;
        string mSuccessfulPanelTitle;
    }
}
