using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace UTranslator
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Scopes = { SheetsService.Scope.Spreadsheets };
                var service = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = "209879134970-9ieru37pic1856rtkcuobuhjja0nlpg6.apps.googleusercontent.com", ClientSecret = "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" }, Scopes, "user", CancellationToken.None, new FileDataStore("MyAppsToken")).Result, ApplicationName = "Google Sheets API .NET Quickstart", });

                SpreadsheetsResource.ValuesResource.GetRequest getRequest = service.Spreadsheets.Values.Get("1fBCoRUD68MAnVk2Gpdi1zyMt9ey_abaspdFp3Lmmrps", "data-received!A:C");
                ValueRange getResponse = getRequest.Execute();
                IList<IList<Object>> values = getResponse.Values;
                var Range = $"{"data-received"}!A" + (values.Count + 1) + ":C" + (values.Count + 1);

                var ValueRange = new ValueRange();
                ValueRange.Values = new List<IList<object>> { new List<object>() { "test 1", "test 2", "test 3" } };
                var updateRequest = service.Spreadsheets.Values.Update(ValueRange, "1fBCoRUD68MAnVk2Gpdi1zyMt9ey_abaspdFp3Lmmrps", Range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var UpdateResponse = updateRequest.Execute();
            }
            catch
            {
                MessageBox.Show(
                         "Ошибка",
                         "Новая 1",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Scopes = { SheetsService.Scope.Spreadsheets };
                var service = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = "209879134970-9ieru37pic1856rtkcuobuhjja0nlpg6.apps.googleusercontent.com", ClientSecret = "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" }, Scopes, "user", CancellationToken.None, new FileDataStore("MyAppsToken")).Result, ApplicationName = "Google Sheets API .NET Quickstart", });

                var Range = $"{"data-received"}!A" + "3" + ":C" + "3";
                var ValueRange = new ValueRange();
                ValueRange.Values = new List<IList<object>> { new List<object>() { "test 3", "test 5", "test 1" } };
                var updateRequest = service.Spreadsheets.Values.Update(ValueRange, "1fBCoRUD68MAnVk2Gpdi1zyMt9ey_abaspdFp3Lmmrps", Range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var UpdateResponse = updateRequest.Execute();
            }
            catch
            {
                MessageBox.Show(
                         "Ошибка",
                         "Новая 1",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
