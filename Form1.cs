using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
//using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UTranslator.Properties;
using Windows.Media.Ocr;
using Windows.Media.Protection.PlayReady;
using Application = System.Windows.Forms.Application;

namespace UTranslator
{
    public partial class Form1 : Form
    {
        string machineName = Environment.MachineName;
        string myMachineName = "DESKTOP-QUN97QT";
        string[] alreadyTranslated = new string[] { };
        string[] arraysBackup = new string[] { };
        string[] filesToRead = new string[] { };
        string textFromOriginalNew = "";
        string[] textArrayFromOriginalNew = new string[] { };
        bool installEnd = false;
        IWebDriver driver = null;
        string localizationPath = "";
        string dataPath = "";
        string backupPath = "";
        string path = "";
        string gameName = "";
        bool newRowsCheck = false;
        bool updateOrReinstall = false;
        bool translateUpdate = false;
        bool translateReinstall = false;
        bool installBreak = false;
        bool updateBreak = false;
        bool translatedFirstTime = false;
        bool boolDeleteTranslate = false;
        public bool isRun = false;
        int portEnabled = 0;
        int newLinesNumber = 0;
        int deeplInformal = 0;
        bool pressUpdateButton = false;
        List<int> newLinesNumbers = new();

        //[DllImport(@"S:\Visual Projects\UTranslator\bin\Debug\net6.0-windows10.0.17763.0\mem_access_provider.dll")]
        //static extern int write_memory(UInt32 process_id, UInt64 address, string buffer, int size);

        public Form1()
        {
            InitializeComponent();
        }

        void Config_Work()//Работаем с конфигурационным файлом (проверка выставление отметок в интерфейсе)
        {
            string[] configLines = new string[] { };

            if (!File.Exists(Application.StartupPath + @"UTranslator.config"))//Проверяем существует ли файл конфигурации,если нет создаём новый файл конфигурации
            {
                CreateConfig();
            }

            configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");

            if (configLines.Length == 23)
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + @"UTranslator.config", true, encoding: Encoding.UTF8))
                {
                    sw.WriteLine("nwr_ver = 0.1");
                }

                configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
            }

            //if (configLines.Contains("closingshift_ver"))
            //{
            //    ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 22, "");
            //
            //    configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
            //}

            versionLabel.Text = "Версия программы: " + configLines[1].Replace("version = ", "");

            if (configLines[2].Contains("skill = 1"))
                skillsNameCheckTranslate.Checked = true;
            else
                skillsNameCheckTranslate.Checked = false;

            if (configLines[3].Contains("item = 1"))
                itemsCheckTranslate.Checked = true;
            else
                itemsCheckTranslate.Checked = false;

            if (configLines[4].Contains("npc = 1"))
                npcCheckTranslate.Checked = true;
            else
                npcCheckTranslate.Checked = false;

            if (configLines[13].Contains("skill_description = 1"))
                skillsDescriptionCheckTranslate.Checked = true;
            else
                skillsDescriptionCheckTranslate.Checked = false;

            if (configLines[14].Contains("temtem = 1"))
                temtemCheckTranslate.Checked = true;
            else
                temtemCheckTranslate.Checked = false;

            if (configLines[5].Contains("google = 1"))
                googleButton.Checked = true;
            else
                googleButton.Checked = false;

            if (configLines[15].Contains("deepl = 1"))
                deeplButton.Checked = true;
            else
                deeplButton.Checked = false;

            if (configLines[16].Contains("yandex = 1"))
                yandexButton.Checked = true;
            else
                yandexButton.Checked = false;

            if (configLines[21].Contains("item_desc = 1"))
                itemsDesc.Checked = true;
            else
                itemsDesc.Checked = false;
        }

        public void CreateConfig()//Создаём конфигурационный файл
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("firstrun = 1");
            stringBuilder.AppendLine("version = 0.3.8.3");
            stringBuilder.AppendLine("skill = 0");
            stringBuilder.AppendLine("items = 0");
            stringBuilder.AppendLine("npc = 0");
            stringBuilder.AppendLine("google = 0");
            stringBuilder.AppendLine("a_translate = 0");
            stringBuilder.AppendLine("changes = 0");
            stringBuilder.AppendLine("author = ");
            stringBuilder.AppendLine("password = ");
            stringBuilder.AppendLine("email = ");
            stringBuilder.AppendLine("auth_translate = 0");
            stringBuilder.AppendLine("core_ver = 0.5.0");
            stringBuilder.AppendLine("skill_description = 0");
            stringBuilder.AppendLine("temtem = 0");
            stringBuilder.AppendLine("deepl = 0");
            stringBuilder.AppendLine("yandex = 1");
            stringBuilder.AppendLine("escapesim_ver = 0.2");
            stringBuilder.AppendLine("temtem_ver = 1.0.1");
            stringBuilder.AppendLine("proxy = 0");
            stringBuilder.AppendLine("mortuary_ver = 0.5");
            stringBuilder.AppendLine("item_desc = 0");
            stringBuilder.AppendLine("DungeonCrawler_ver = 0.6.1.1684");
            stringBuilder.AppendLine("nwr_ver = 0.1");
            File.WriteAllText(Application.StartupPath + @"UTranslator.config", stringBuilder.ToString());
        }

        void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(500);

            if (Process.GetProcessesByName("UTranslator_x86").Length > 0)
            {
                deleteFile(Application.StartupPath + "UTranslator.exe");
            }
            else
            {
                deleteFile(Application.StartupPath + "UTranslator_x86.exe");
            }

            #region updater
            if (Process.GetProcessesByName("updater").Length <= 0)
            {
                if (File.Exists(Application.StartupPath + "updater.exe"))
                {
                    deleteFile(Application.StartupPath + "updater.exe");
                    System.Threading.Thread.Sleep(100);
                }
            }

            if (File.Exists(Application.StartupPath + "updater.exe"))
            {
                System.Threading.Thread.Sleep(500);
                string fileToDelete = "";
                if (File.Exists(Application.StartupPath + "UTranslator.exe"))
                {
                    fileToDelete = Application.StartupPath + "UTranslator.exe";
                }
                else
                {
                    fileToDelete = Application.StartupPath + "UTranslator_x86.exe";
                }

                deleteFile(fileToDelete);
                System.Threading.Thread.Sleep(500);
                File.Copy(Application.StartupPath + "updater.exe", fileToDelete);
                System.Threading.Thread.Sleep(500);
                Process.Start(new ProcessStartInfo
                {
                    FileName = fileToDelete,
                    WorkingDirectory = Path.GetDirectoryName(fileToDelete),
                    Verb = "runas"
                });

                Close();
            }
            #endregion

            Config_Work();

            if (machineName != myMachineName)
                UpdateButton_Click(sender, e);

            /*if (installEnd)
                updateReinstall.Enabled = true;
            else
                updateReinstall.Enabled = false;*/

            if (!Directory.Exists(Application.StartupPath + @"backup"))
                Directory.CreateDirectory(Application.StartupPath + @"backup");
            backupPath = Application.StartupPath + @"backup\";

            if (File.Exists(Application.StartupPath + "tmp\\info.txt"))//Удаляем файл с информацией о последнем патче
                deleteFile(Application.StartupPath + "tmp\\info.txt");

            if (Directory.Exists(Application.StartupPath + "tmp"))
                Directory.Delete(Application.StartupPath + "tmp");

            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
            if (configLines[0].Contains("firstrun = 0"))
            {
                updateReinstall.Enabled = true;
                //recoverBD.Enabled = true;
                deleteTranslate.Enabled = true;
            }
            else if (configLines[0].Contains("firstrun = 1"))
            {
                if (File.Exists(backupPath + "The Mortuary Assistant_translated_backup") || File.Exists(backupPath + "NewWorld_translated_backup") || File.Exists(backupPath + "Temtem_translated_backup") || File.Exists(backupPath + "CoreKeeper_translated_backup") || File.Exists(backupPath + "Escape Simulator_translated_backup") || File.Exists(backupPath + "DungeonCrawler_translated_backup") || File.Exists(backupPath + "nwr_translated_backup"))
                {
                    System.Threading.Thread.Sleep(500);
                    ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 0, "firstrun = 0");

                    updateReinstall.Enabled = true;
                    //recoverBD.Enabled = true;
                    deleteTranslate.Enabled = true;
                }
            }

            if (machineName == myMachineName)
            {
                machineNameText.Enabled = true;
                machineNameText.Visible = true;
                button1.Enabled = true;
                button1.Visible = true;
                TranslateEditorBtn.Enabled = true;
                TranslateEditorBtn.Visible = true;
                machineNameText.Text = myMachineName;
                deeplButton.Enabled = true;
            }
            else
            {
                machineNameText.Enabled = false;
                machineNameText.Visible = false;
                button1.Enabled = false;
                button1.Visible = false;
                TranslateEditorBtn.Enabled = false;
                TranslateEditorBtn.Visible = false;
                deeplButton.Checked = false;
                deeplButton.Enabled = false;
                yandexButton.Checked = true;
            }

            AppendLine(txtStatus, "Русификатор запущен, приятного использования!", Color.Black);
            isRun = true;
        }

        void choosePathBtn_Click(object sender, EventArgs e)
        {
            ChoosePath();
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            path = txtPath.Text;
            //if (launch_status == 1)
            //    TryFix();

            findGameName();
        }

        void ChoosePath()//Обработка изменения пути к игре
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Укажите путь к игре"
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog.SelectedPath;
                //if (launch_status == 1)
                //    TryFix();
                txtPath.Text = folderBrowserDialog.SelectedPath;

                findGameName();
            }

            //if (gameName == "DungeonCrawler")
            //{
            //    deeplButton.Enabled = false;
            //    deeplButton.Checked = false;
            //    yandexButton.Checked = true;
            //
            //    deeplButton.Text = "Deepl переводчик";
            //}
        }

        void findGameName()
        {
            if (!Directory.Exists(path))
            {
                path = "";
                //txtPath.Text = "";
                gameName = "";

                MessageBox.Show(
                     "Указанная папка не найдена.",
                     "Ошибка",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                return;
            }

            string[] exeFind = Directory.GetFiles(path, "*.exe");

            if (exeFind.Length == 0)
            {
                path = "";
                //txtPath.Text = "";
                gameName = "";

                MessageBox.Show(
                     "Указан неверный путь...",
                     "Ошибка",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                return;
            }

            string gameNameExe;

            for (int i = 0; i < exeFind.Length; i++)
            {
                if (exeFind[i] == path + @"\Temtem.exe" || exeFind[i] == path + @"\The Mortuary Assistant.exe" || exeFind[i] == path + @"\CoreKeeper.exe" || exeFind[i] == path + @"\Escape Simulator.exe" || exeFind[i] == path + @"\NewWorldLauncher.exe" || exeFind[i] == path + @"\DungeonCrawler.exe" || exeFind[i] == path + @"\nwr.exe")
                {
                    gameNameExe = exeFind[i].Replace(path + @"\", "");
                    gameName = gameNameExe.Replace(".exe", "");

                    /*if (gameName == "BookOfTravels")
                        fontSetup.Enabled = false;
                    else if (gameName == "Temtem")
                        fontSetup.Enabled = true;*/

                    if (gameName == "NewWorldLauncher")
                        gameName = "NewWorld";
                    if (gameName == "DungeonCrawler")
                        gameName = "DungeonCrawler";

                    if (gameName == "CoreKeeper")
                        gameNameTxt.Text = "Выбранная игра: Core Keeper";
                    if (gameName == "NewWorld")
                        gameNameTxt.Text = "Выбранная игра: New World";
                    if (gameName == "DungeonCrawler")
                        gameNameTxt.Text = "Выбранная игра: Dark and Darker";

                    if (gameName != "CoreKeeper" && gameName != "NewWorld" && gameName != "DungeonCrawler")
                        gameNameTxt.Text = "Выбранная игра: " + gameName;
                }
            }

            if (gameName != "Temtem" && gameName != "The Mortuary Assistant" && gameName != "CoreKeeper" && gameName != "Escape Simulator" && gameName != "NewWorld" && gameName != "DungeonCrawler" && gameName != "nwr")
            {
                gameNameExe = exeFind[0].Replace(path + @"\", "");
                gameName = gameNameExe.Replace(".exe", "");

                path = "";
                txtPath.Text = "";

                MessageBox.Show(
                     $"Игра {gameName} в данный момент не поддерживается.\nНапишите мне об этой игре и я постараюсь добавить возможность ее перевода.",
                     "Ошибка",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                gameName = "";

                return;
            }

            if (gameName == "Temtem")
            {
                temtemCheckTranslate.Visible = true;
                temtemCheckTranslate.Enabled = true;
                temtemCheckTranslate.Checked = true;
                skillsNameCheckTranslate.Enabled = true;
                skillsNameCheckTranslate.Checked = true;
                itemsCheckTranslate.Enabled = true;
                skillsDescriptionCheckTranslate.Enabled = true;
                npcCheckTranslate.Enabled = true;
                npcCheckTranslate.Checked = true;
                npcCheckTranslate.Text = "Имена NPC";
            }
            else if (gameName == "The Mortuary Assistant")
            {
                if (machineName != myMachineName)
                {
                    temtemCheckTranslate.Checked = false;
                    temtemCheckTranslate.Visible = false;
                    temtemCheckTranslate.Enabled = false;
                    skillsNameCheckTranslate.Checked = false;
                    skillsNameCheckTranslate.Enabled = false;
                    itemsCheckTranslate.Enabled = false;
                    itemsCheckTranslate.Checked = false;
                    skillsDescriptionCheckTranslate.Enabled = false;
                    skillsDescriptionCheckTranslate.Checked = false;
                    npcCheckTranslate.Checked = true;
                    npcCheckTranslate.Enabled = false;
                    npcCheckTranslate.Text = "Имена NPC и трупов";
                }
            }
            else if (gameName == "CoreKeeper" || gameName == "Escape Simulator") //
            {
                if (machineName != myMachineName)
                {
                    temtemCheckTranslate.Checked = false;
                    temtemCheckTranslate.Visible = false;
                    temtemCheckTranslate.Enabled = false;
                    skillsNameCheckTranslate.Checked = false;
                    skillsNameCheckTranslate.Enabled = false;
                    itemsCheckTranslate.Enabled = false;
                    skillsDescriptionCheckTranslate.Enabled = false;
                    npcCheckTranslate.Checked = false;
                    npcCheckTranslate.Enabled = false;
                    npcCheckTranslate.Text = "Имена NPC";
                }
            }
            else if (gameName == "DungeonCrawler")
            {
                temtemCheckTranslate.Checked = false;
                temtemCheckTranslate.Visible = false;
                temtemCheckTranslate.Enabled = false;
                skillsNameCheckTranslate.Checked = true;
                skillsNameCheckTranslate.Enabled = true;
                itemsCheckTranslate.Enabled = true;
                itemsCheckTranslate.Checked = true;
                skillsDescriptionCheckTranslate.Enabled = false;
                npcCheckTranslate.Checked = false;
                npcCheckTranslate.Enabled = true;
                npcCheckTranslate.Text = "Имена NPC";
                itemsDesc.Enabled = false;
                itemsDesc.Visible = true;
                itemsDesc.Checked = false;

                itemsCheckTranslate.Text = "Предметы (Название)";
            }
            else if (gameName == "NewWorld")
            {
                skillsNameCheckTranslate.Enabled = true;
                skillsNameCheckTranslate.Checked = true;
                itemsCheckTranslate.Enabled = true;
                skillsDescriptionCheckTranslate.Enabled = false;
                npcCheckTranslate.Enabled = false;
                npcCheckTranslate.Checked = false;
                itemsCheckTranslate.Text = "Предметы (Название)";
                itemsDesc.Enabled = true;
                itemsDesc.Visible = true;
                itemsDesc.Checked = false;
            }

            if (gameName != "NewWorld" && gameName != "DungeonCrawler")
            {
                itemsCheckTranslate.Text = "Предметы (Название и описание)";
                itemsDesc.Enabled = false;
                itemsDesc.Visible = false;
            }

            if (gameName == "CoreKeeper")
            {
                if (File.Exists(backupPath + gameName + "_backup"))
                    deleteFile(backupPath + gameName + "_backup");
            }

            /*if (gameName == "CoreKeeper")
                npcCheckTranslate.Enabled = false;
            else
                npcCheckTranslate.Enabled = true;*/
        }

        void fontSetup_Click(object sender, EventArgs e)
        {
            if (path == "")
            {
                MessageBox.Show(
                     "Укажите путь к игре.",
                     "Путь не указан",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                return;
            }

            updateReinstall.Enabled = false;
            recoverBD.Enabled = false;
            deleteTranslate.Enabled = false;
            btnStart.Enabled = false;
            choosePathBtn.Enabled = false;
            fontSetup.Enabled = false;
            txtPath.Enabled = false;
            UpdateButton.Enabled = false;

            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");

            DialogResult result = MessageBox.Show(
                         "Установить кастомный шрифт для выбранной игры?",
                         "Установика шрифта",
                         MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Cancel)
            {
                if (configLines.Contains("firstrun = 0"))
                {
                    updateReinstall.Enabled = true;
                    //recoverBD.Enabled = true;
                    deleteTranslate.Enabled = true;
                    btnStart.Enabled = true;
                    UpdateButton.Enabled = true;
                    choosePathBtn.Enabled = true;
                    txtPath.Enabled = true;
                }
                else
                {
                    btnStart.Enabled = true;
                    choosePathBtn.Enabled = true;
                    txtPath.Enabled = true;
                    UpdateButton.Enabled = true;
                }

                fontSetup.Enabled = true;

                return;
            }
            else if (result == DialogResult.OK)
            {
                dataPath = path + @"\" + gameName + "_Data";
                string fontsPath = dataPath + @"\Unity_Assets_Files\sharedassets0\Fonts";

                if (!File.Exists(dataPath + @"\UPacker.exe"))
                    File.WriteAllBytesAsync(dataPath + @"\UPacker.exe", Resources.UPacker_exe);
                File.WriteAllTextAsync(dataPath + @"\Export_Fonts.bat", Resources.Export_ttf_bat);
                File.WriteAllTextAsync(dataPath + @"\Import_Fonts.bat", Resources.Import_Fonts_bat);
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = dataPath + @"\Export_Fonts.bat",
                    WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Export_Fonts.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                deleteFile(dataPath + @"\Export_Fonts.bat");

                deleteFile(fontsPath + @"\NotoSansCJKsc-Medium.otf");
                System.Threading.Thread.Sleep(500);
                //File.WriteAllBytesAsync(fontsPath + @"\NotoSansCJKsc-Medium.otf", Resources.Temtem_custom_font_otf);
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = dataPath + @"\Import_Fonts.bat",
                    WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Import_Fonts.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                deleteFile(dataPath + @"\Import_Fonts.bat");
                deleteFile(dataPath + @"\UPacker.exe");
                deleteFile(fontsPath + @"\NotoSansCJKsc-Medium.otf");
                deleteFile(fontsPath + @"\NanumGothic-Bold.ttf");
                deleteFile(fontsPath + @"\Boku2-Bold.otf");
                Directory.Delete(fontsPath);
                deleteFile(dataPath + @"\Unity_Assets_Files\sharedassets0\Boku2-Bold.font_raw");
                deleteFile(dataPath + @"\Unity_Assets_Files\sharedassets0\NanumGothic-Bold.font_raw");
                deleteFile(dataPath + @"\Unity_Assets_Files\sharedassets0\NotoSansCJKsc-Medium.font_raw");
                Directory.Delete(dataPath + @"\Unity_Assets_Files\sharedassets0");
                Directory.Delete(dataPath + @"\Unity_Assets_Files");

                MessageBox.Show(
                         "Установика шрифта завершена.",
                         "Установика шрифта",
                         MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                if (configLines.Contains("firstrun = 0"))
                {
                    updateReinstall.Enabled = true;
                    //recoverBD.Enabled = true;
                    deleteTranslate.Enabled = true;
                    btnStart.Enabled = true;
                    UpdateButton.Enabled = true;
                    choosePathBtn.Enabled = true;
                    txtPath.Enabled = true;
                }
                else
                {
                    btnStart.Enabled = true;
                    choosePathBtn.Enabled = true;
                    UpdateButton.Enabled = true;
                    txtPath.Enabled = true;
                }

                fontSetup.Enabled = true;
            }
        }

        void StartExport()
        {
            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Разархивирование локализации...", Color.Black)));

            dataPath = path + @"\" + gameName + "_Data";

            if (gameName == "Temtem")
            {
                localizationPath = dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp\I2.Loc\";

                if (!File.Exists(localizationPath + @"I2Languages.LanguageSourceAsset"))
                {
                    if (gameName == "Temtem")
                    {
                        if (!File.Exists(dataPath + @"\UPacker.exe"))
                            File.WriteAllBytesAsync(dataPath + @"\UPacker.exe", Resources.UPacker_exe);
                    }
                    else
                    {
                        if (!File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                            File.WriteAllBytesAsync(dataPath + @"\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                    }
                    File.WriteAllTextAsync(dataPath + @"\Export_l2.bat", Resources.Export_l2_bat);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = dataPath + @"\Export_l2.bat",
                        WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Export_l2.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(dataPath + @"\Export_l2.bat");
                }

                System.Threading.Thread.Sleep(500);
                if (!File.Exists(localizationPath + @"I2Languages.LanguageSourceAsset"))
                    return;

                if (!File.Exists(localizationPath + @"I2Languages.csv"))
                {
                    File.WriteAllBytesAsync(localizationPath + @"Parser_ULS.exe", Resources.Parser_ULS_exe);
                    File.WriteAllTextAsync(localizationPath + @"export_uls.bat", Resources.export_uls_bat);
                    System.Threading.Thread.Sleep(500);
                    ReplaceStringInFile(localizationPath + @"export_uls.bat", 1, @"for %%a in (*.LanguageSource;*.LanguageSourceAsset;) do Parser_ULS.exe -e ""%%a"" -o $0c -dauto");
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"export_uls.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"export_uls.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(localizationPath + @"export_uls.bat");
                }
            }
            else if (gameName == "DungeonCrawler")
            {
                dataPath = path + @"\" + gameName + @"\Content\Paks";

                localizationPath = dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en\";

                if (!File.Exists(localizationPath + @"Game.locres"))
                {
                    if (!File.Exists(dataPath + @"\quickbms_4gb_files.exe"))
                        File.WriteAllBytesAsync(dataPath + @"\quickbms_4gb_files.exe", Resources.quickbms_4gb_files_exe);

                    File.WriteAllTextAsync(dataPath + @"\export_locres.bat", Resources.export_locres_bat);
                    System.Threading.Thread.Sleep(500);

                    //if (newRowsCheck)
                    //{
                    ReplaceStringInFile(dataPath + @"\export_locres.bat", 0, @"quickbms_4gb_files.exe -f Game.locres 4_0.4.27d.bms pakchunk0-Windows_0_P.pak pakchunk0-Windows_0_P");
                    System.Threading.Thread.Sleep(500);
                    //}

                    Directory.CreateDirectory(dataPath + @"\pakchunk0-Windows_0_P");
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = dataPath + @"\export_locres.bat",
                        WorkingDirectory = Path.GetDirectoryName(dataPath + @"\export_locres.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                }

                if (!File.Exists(localizationPath + @"Game.locres"))
                    return;

                if (!File.Exists(localizationPath + @"Game.locres.txt"))
                {
                    if (!File.Exists(localizationPath + @"UE4localizationsTool.exe"))
                        File.WriteAllBytesAsync(localizationPath + @"UE4localizationsTool.exe", Resources.UE4localizationsTool_exe);
                    File.WriteAllTextAsync(localizationPath + @"export_txt_from_locres.bat", Resources.export_txt_from_locres_bat);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"export_txt_from_locres.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"export_txt_from_locres.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(localizationPath + @"export_txt_from_locres.bat");
                }
            }
            else if (gameName == "CoreKeeper")
            {
                if (!Directory.Exists(path + @"\localization"))
                    Directory.CreateDirectory(path + @"\localization");

                localizationPath = path + @"\localization\";
                if (machineName == myMachineName && !File.Exists(localizationPath + @"Localization.csv"))
                {
                    File.Copy(localizationPath + @"Localization Template.csv", localizationPath + @"Localization.csv");
                }
            }
            else if (gameName == "Escape Simulator")
            {
                localizationPath = dataPath + @"\Unity_Assets_Files\resources\";

                if (machineName == myMachineName && !File.Exists(localizationPath + @"English.txt"))
                {
                    File.WriteAllBytesAsync(dataPath + @"\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                    File.WriteAllTextAsync(dataPath + @"\Export_txt.bat", Resources.Export_txt_bat);
                    System.Threading.Thread.Sleep(500);
                    ReplaceStringInFile(dataPath + @"\Export_txt.bat", 2, @$"for %%a in (resources.assets;) do UPacker_Advanced.exe export ""%%a"" -t *English*.txt");
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = dataPath + @"\Export_txt.bat",
                        WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Export_txt.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(dataPath + @"\Export_txt.bat");
                    if (File.Exists(localizationPath + @"EnglishEditor.txt"))
                        deleteFile(localizationPath + @"EnglishEditor.txt");
                }
            }
            else if (gameName == "The Mortuary Assistant")
            {
                localizationPath = dataPath + @"\Unity_Assets_Files\resources\";

                if (machineName == myMachineName)
                {
                    if (!File.Exists(localizationPath + @"PatreonNamesFemale.txt"))
                    {
                        File.WriteAllBytesAsync(dataPath + @"\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                        File.WriteAllTextAsync(dataPath + @"\Export_txt.bat", Resources.Export_txt_bat);
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = dataPath + @"\Export_txt.bat",
                            WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Export_txt.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();

                        deleteFile(dataPath + @"\Export_txt.bat");
                    }

                    deleteFile(localizationPath + @"PatreonNamesFemale.txt");
                    deleteFile(localizationPath + @"PatreonNamesMale.txt");
                    deleteFile(localizationPath + @"femaleFirstNames.txt");
                    deleteFile(localizationPath + @"lastNames.txt");
                    deleteFile(localizationPath + @"maleFirstNames.txt");
                    deleteFile(localizationPath + @"LineBreaking Following Characters.txt");
                    deleteFile(localizationPath + @"LineBreaking Leading Characters.txt");
                    deleteFile(localizationPath + @"BasementTapeFragment.txt");
                    deleteFile(localizationPath + @"LanguageCharacters.txt");
                    deleteFile(localizationPath + @"BasementTapeWhole.txt");
                    deleteFile(localizationPath + @"BibleScrap_00001.txt");
                    deleteFile(localizationPath + @"callingThePolice.txt");
                    deleteFile(localizationPath + @"CarStart.txt");
                    deleteFile(localizationPath + @"caveDificult.txt");
                    deleteFile(localizationPath + @"ccApartmentCall.txt");
                    deleteFile(localizationPath + @"ccDemonGenLines.txt");
                    deleteFile(localizationPath + @"ccGeneric_00001.txt");
                    deleteFile(localizationPath + @"ccIntroScene.txt");
                    deleteFile(localizationPath + @"ccRaymonGenLines.txt");
                    deleteFile(localizationPath + @"ccRebeccaGenLines_00001.txt");
                    deleteFile(localizationPath + @"clipboardData.txt");
                    deleteFile(localizationPath + @"doYouKnowMe.txt");
                    deleteFile(localizationPath + @"EndingFather_00001.txt");
                    deleteFile(localizationPath + @"EndingFirst_00001.txt");
                    deleteFile(localizationPath + @"EndingSecret_00001.txt");
                    deleteFile(localizationPath + @"EndingStandard.txt");
                    deleteFile(localizationPath + @"EndingWrongbody_00001.txt");
                    deleteFile(localizationPath + @"evilTape.txt");
                    deleteFile(localizationPath + @"familiarVoice_00001.txt");
                    deleteFile(localizationPath + @"father911Call_00001.txt");
                    deleteFile(localizationPath + @"FatherFallOnRocks_00001.txt");
                    deleteFile(localizationPath + @"FatherFoundRebecca.txt");
                    deleteFile(localizationPath + @"GenNotes_00001.txt");
                    deleteFile(localizationPath + @"genText.txt");
                    deleteFile(localizationPath + @"getStarted.txt");
                    deleteFile(localizationPath + @"grandmaEvent2BroughtYouSomethign_00001.txt");
                    deleteFile(localizationPath + @"grandmaEvent2Run.txt");
                    deleteFile(localizationPath + @"grandmaEvent2SlitWrists_00001.txt");
                    deleteFile(localizationPath + @"HateMeAllYouWant_00001.txt");
                    deleteFile(localizationPath + @"HauntGhostbodies.txt");
                    deleteFile(localizationPath + @"IllGetIt.txt");
                    deleteFile(localizationPath + @"introGetBackToWork_00001.txt");
                    deleteFile(localizationPath + @"itemInfo_00001.txt");
                    deleteFile(localizationPath + @"LeadInCallOne.txt");
                    deleteFile(localizationPath + @"LeadInCallTwo_00001.txt");
                    deleteFile(localizationPath + @"letMeInLaugh.txt");
                    deleteFile(localizationPath + @"Letters_00001.txt");
                    deleteFile(localizationPath + @"littleRaymond_00001.txt");
                    deleteFile(localizationPath + @"NightShiftText.txt");
                    deleteFile(localizationPath + @"NsHistory.txt");
                    deleteFile(localizationPath + @"packetToZoe.txt");
                    deleteFile(localizationPath + @"PhoneCallFriendBeach.txt");
                    deleteFile(localizationPath + @"PhoneCallKillYou_00001.txt");
                    deleteFile(localizationPath + @"RaymondJournal.txt");
                    deleteFile(localizationPath + @"RecordMarks.txt");
                    deleteFile(localizationPath + @"shouldBeoutHere_00001.txt");
                    deleteFile(localizationPath + @"TapeOne_00001.txt");
                    deleteFile(localizationPath + @"TapeThree.txt");
                    deleteFile(localizationPath + @"TapeTwo.txt");
                    deleteFile(localizationPath + @"theKnock.txt");
                    deleteFile(localizationPath + @"toolateDoorOpen_00001.txt");
                    deleteFile(localizationPath + @"TutEnter.txt");
                    deleteFile(localizationPath + @"TutorialText_00001.txt");
                    deleteFile(localizationPath + @"uiText_00001.txt");
                    deleteFile(localizationPath + @"ValEvent1_00001.txt");
                    deleteFile(localizationPath + @"ValEvent2_00001.txt");
                    deleteFile(localizationPath + @"ValEvent3_00001.txt");
                    deleteFile(localizationPath + @"ValEvent4.txt");
                    deleteFile(localizationPath + @"ValEvent5_00001.txt");
                    deleteFile(localizationPath + @"whisperPenatent_00001.txt");
                    deleteFile(localizationPath + @"YouShouldBe.txt");
                    deleteFile(localizationPath + @"itemInfoPatch.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00001.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00002.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00003.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00004.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00006.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00007.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00008.txt");
                    deleteFile(localizationPath + @"itemInfoPatch_00009.txt");
                    deleteFile(localizationPath + @"Patch_ccLines.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00001.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00002.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00003.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00005.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00006.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00007.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00008.txt");
                    deleteFile(localizationPath + @"Patch_ccLines_00009.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00004.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00002.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00003.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00005.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00006.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00007.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00008.txt");
                    deleteFile(localizationPath + @"Patch_ExorcismWIP_00009.txt");
                    deleteFile(localizationPath + @"Patch_Notes.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00004.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00002.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00003.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00005.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00006.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00007.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00008.txt");
                    deleteFile(localizationPath + @"Patch_Notes_00001.txt");
                    deleteFile(localizationPath + @"Patch_UiText.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00004.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00002.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00009.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00005.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00006.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00007.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00008.txt");
                    deleteFile(localizationPath + @"Patch_UiText_00001.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00004.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00002.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00009.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00005.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00003.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00007.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00008.txt");
                    deleteFile(localizationPath + @"Patch_VHS_slide_00001.txt");

                    string[] filesToRead = Directory.GetFiles(localizationPath);

                    for (int i = 0; i < filesToRead.Length; i++)
                    {
                        string checkedFile = filesToRead[i].Replace(localizationPath, "");

                        if (checkedFile.Contains("_fr") || checkedFile.Contains("_de") || checkedFile.Contains("_es") || checkedFile.Contains("_it") || checkedFile.Contains("_ja")
                            || checkedFile.Contains("_ko") || checkedFile.Contains("_tr") || checkedFile.Contains("_zh_cn"))
                        {
                            deleteFile(filesToRead[i]);
                        }
                    }
                }
            }
            else if (gameName == "NewWorld")
            {
                localizationPath = dataPath + @"\levels\newlevels\localization\en-us\";

                if (File.Exists(localizationPath + @"javelindata_itemdefinitions_master.loc.xml"))
                {
                    string[] alreadyTranslatedFiles = Directory.GetFiles(localizationPath);

                    for (int i = 0; i < alreadyTranslatedFiles.Length; i++)
                    {
                        File.Move(alreadyTranslatedFiles[i], dataPath + @"\levels\newlevels\localization" + alreadyTranslatedFiles[i].Replace(localizationPath + @"", ""));
                    }
                }

                File.WriteAllBytesAsync(dataPath + @"\quickbms.exe", Resources.quickbms_exe);
                File.WriteAllBytesAsync(dataPath + @"\nw_oodle.bms", Resources.nw_oodle_bms);
                File.WriteAllTextAsync(dataPath + @"\xml_q_export.bat", Resources.xml_q_export_bat);
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = dataPath + @"\xml_q_export.bat",
                    WorkingDirectory = Path.GetDirectoryName(dataPath + @"\xml_q_export.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                deleteFile(dataPath + @"\xml_q_export.bat");
                deleteFile(dataPath + @"\nw_oodle.bms");
                deleteFile(dataPath + @"\quickbms.exe");
                deleteFile(localizationPath + @"01_npcmisc.loc.xml");
                deleteFile(localizationPath + @"01_npctopics.loc.xml");
                deleteFile(localizationPath + @"02_npcmisc.loc.xml");
                deleteFile(localizationPath + @"03_npcmisc.loc.xml");
                deleteFile(localizationPath + @"04_npcmisc.loc.xml");
                deleteFile(localizationPath + @"04a_npcmisc.loc.xml");
                deleteFile(localizationPath + @"05_npcmisc.loc.xml");
                deleteFile(localizationPath + @"06_npcmisc.loc.xml");
                deleteFile(localizationPath + @"06a_npcmisc.loc.xml");
                deleteFile(localizationPath + @"06a_npctopics.loc.xml");
                deleteFile(localizationPath + @"07_npcmisc.loc.xml");
                deleteFile(localizationPath + @"08_npcmisc.loc.xml");
                deleteFile(localizationPath + @"08_npctopics.loc.xml");
                deleteFile(localizationPath + @"09_npcmisc.loc.xml");
                deleteFile(localizationPath + @"10_npcmisc.loc.xml");
                deleteFile(localizationPath + @"11_npcmisc.loc.xml");
                deleteFile(localizationPath + @"11_npctopics.loc.xml");
                deleteFile(localizationPath + @"12_npcmisc.loc.xml");
                deleteFile(localizationPath + @"12a_npcmisc.loc.xml");
                deleteFile(localizationPath + @"12a_npctopics.loc.xml");
                deleteFile(localizationPath + @"13_npcmisc.loc.xml");
                deleteFile(localizationPath + @"14_npcmisc.loc.xml");
                deleteFile(localizationPath + @"15_npcmisc.loc.xml");
                deleteFile(localizationPath + @"16_npcmisc.loc.xml");
                deleteFile(localizationPath + @"16_npctopics.loc.xml");
                deleteFile(localizationPath + @"96_cinematics.loc.xml");
                deleteFile(localizationPath + @"98_npcmisc.loc.xml");
                deleteFile(localizationPath + @"98_npctopics.loc.xml");
                deleteFile(localizationPath + @"99_npcmisc.loc.xml");
                deleteFile(localizationPath + @"99a_npcmisc.loc.xml");
                deleteFile(localizationPath + @"dungeonregions.loc.xml");
                deleteFile(localizationPath + @"easyanticheat.loc.xml");
                deleteFile(localizationPath + @"fishbehaviors.loc.xml");
                deleteFile(localizationPath + @"guildnames.loc.xml");
                deleteFile(localizationPath + @"javelindata_affixdefinitions.loc.xml");
                deleteFile(localizationPath + @"javelindata_craftingnames.loc.xml");
                deleteFile(localizationPath + @"javelindata_craftingrecipes.loc.xml");
                deleteFile(localizationPath + @"javelindata_damagetypes.loc.xml");
                deleteFile(localizationPath + @"javelindata_gatherables.loc.xml");
                deleteFile(localizationPath + @"javelindata_itemdefinitions_blueprints.loc.xml");
                deleteFile(localizationPath + @"javelindata_vitals.loc.xml");
                deleteFile(localizationPath + @"javelindata_metaachievements.loc.xml");
                deleteFile(localizationPath + @"javelindata_tooltiplayout.loc.xml");
                deleteFile(localizationPath + @"legaltext.loc.xml");
                deleteFile(localizationPath + @"npcmisc.loc.xml");
                deleteFile(localizationPath + @"periodicrewards.loc.xml");
                deleteFile(localizationPath + @"territories.loc.xml");
                deleteFile(localizationPath + @"tracts.loc.xml");
                deleteFile(localizationPath + @"territorystandingstitles.loc.xml");
                deleteFile(localizationPath + @"areadefinitions.loc.xml");
            }
            #region Book of travels
            /*else if (gameName == "BookOfTravels")
            {
                localizationPath = dataPath + @"\Unity_Assets_Files\resources";

                if (!File.Exists(localizationPath + @"LICENSE.txt"))
                {
                    File.WriteAllBytesAsync(dataPath + @"\Unity.exe", Resources.Unity_exe);
                    File.WriteAllTextAsync(dataPath + @"\Export_Binary.bat", Resources.Export_Binary_bat);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = dataPath + @"\Export_Binary.bat",
                        WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Export_Binary.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                }

                deleteFile(dataPath + @"\Export_Binary.bat");

                if (!File.Exists(localizationPath + @"LICENSE.txt"))
                    return;

                if (!updateOrReinstall)
                {
                    CreateBackupTestAsync();
                    int rowsInFile = File.ReadAllLines(localizationPath + @"I2Languages.csv").Length;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + (rowsInFile * 3) + 5));

                    SqlWorkAsync();
                }
            }*/
            #endregion
        }

        void deleteTranslate_Click(object sender, EventArgs e)
        {
            if (path == "")
            {
                MessageBox.Show(
                     "Укажите путь к игре.",
                     "Путь не указан",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                return;
            }

            updateReinstall.Enabled = false;
            recoverBD.Enabled = false;
            deleteTranslate.Enabled = false;
            btnStart.Enabled = false;
            UpdateButton.Enabled = false;
            choosePathBtn.Enabled = false;
            txtPath.Enabled = false;
            //fontSetup.Enabled = false;

            DialogResult result = MessageBox.Show(
                         "Вы точно хотите полностью удалить перевод для выбранной игры?",
                         "Удаление перевода",
                         MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Cancel)
            {
                updateReinstall.Enabled = true;
                //recoverBD.Enabled = true;
                deleteTranslate.Enabled = true;
                btnStart.Enabled = true;
                UpdateButton.Enabled = true;
                choosePathBtn.Enabled = true;
                txtPath.Enabled = true;
                //fontSetup.Enabled = true;

                return;
            }
            else if (result == DialogResult.OK)
            {
                if (gameName == "Temtem")
                {
                    updateOrReinstall = true;
                    StartExport();

                    string backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono\Assembly-CSharp";

                    unArchivTranslatedBackup();

                    int rowsInBackupFile = File.ReadAllLines(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv").Length;
                    int rowsInNewFile = File.ReadAllLines(localizationPath + @"I2Languages.csv").Length;

                    if (rowsInBackupFile == rowsInNewFile)
                    {
                        if (File.Exists(backupPath + gameName + "_translated_backup"))
                            deleteFile(backupPath + gameName + "_translated_backup");

                        //unArchivBackup();

                        deleteFile(localizationPath + @"I2Languages.csv");
                        File.Copy(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv", localizationPath + @"I2Languages.csv");
                        System.Threading.Thread.Sleep(500);

                        if (File.Exists(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv"))
                        {
                            deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv");
                            delFolder(backupTranslatedLocalizationPath + @"\I2.Loc");
                            delFolder(backupTranslatedLocalizationPath);
                            delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono");
                            delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup");
                            delFolder(backupPath + @"Unity_Assets_Files");
                        }

                        boolDeleteTranslate = true;
                        ImportAsync();
                    }
                    else if (rowsInNewFile > rowsInBackupFile)
                    {
                        MessageBox.Show(
                         "В файле локализации есть новые строки.\n" +
                         "Нажмите \"Обновить/Переустановить\", чтобы обновить перевод.\n" +
                         "Если Вы хотите удалить перевод, то просто проверьте целостность файлов игры в Steam.",
                         "Удаление перевода",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                        deleteFile(localizationPath + @"I2Languages.csv");
                        delFolder(localizationPath);
                        delFolder(localizationPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono\Assembly-CSharp");
                        delFolder(localizationPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono");
                        delFolder(localizationPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup");
                        delFolder(localizationPath + @"Unity_Assets_Files");

                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv");
                        delFolder(backupTranslatedLocalizationPath + @"\I2.Loc");
                        delFolder(backupTranslatedLocalizationPath);
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono");
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup");
                        delFolder(backupPath + @"Unity_Assets_Files");

                        return;
                    }
                }
                else if (gameName == "Escape Simulator")
                {
                    string localDataPath = path + @"\" + gameName + "_Data";
                    //string localLocalizationPath = localDataPath + @"\Unity_Assets_Files\resources\Mono\I2\I2.Loc";

                    deleteFile(localDataPath + @"\resources.assets");
                    File.Copy(Application.StartupPath + @$"backup\{gameName}_backup", localDataPath + @"\resources.assets");
                }
                else if (gameName == "The Mortuary Assistant")
                {
                    string localDataPath = path + @"\" + gameName + "_Data";

                    deleteFile(localDataPath + @"\resources.assets");
                    System.Threading.Thread.Sleep(500);
                    File.Copy(backupPath + $"{gameName}_backup", localDataPath + @"\resources.assets");
                    System.Threading.Thread.Sleep(500);
                }
                else if (gameName == "CoreKeeper")
                {
                    deleteFile(path + @"\localization\Localization.csv");
                }
                else if (gameName == "NewWorld")
                {
                    string localDataPath = path + @"\assets";

                    deleteFile(localDataPath + @"\DataStrm-part1.pak");
                    System.Threading.Thread.Sleep(500);
                    File.Copy(backupPath + $"{gameName}_backup", localDataPath + @"\DataStrm-part1.pak");
                }
                else if (gameName == "DungeonCrawler")
                {
                    string localDataPath = path + @"\" + gameName + @"\Content\Paks\~mods";
                    //string localLocalizationPath = localDataPath + @"\Unity_Assets_Files\resources\Mono\I2\I2.Loc";

                    deleteFile(localDataPath + @"\pakchunk0-Windows_0_P.pak");
                    System.Threading.Thread.Sleep(500);
                    Directory.Delete(localDataPath);

                    //File.Copy(Application.StartupPath + @$"backup\{gameName}_backup", localDataPath + @"\pakchunk0-Windows_0_P.pak");
                }

                deleteFile(backupPath + $"{gameName}_backup");
                deleteFile(backupPath + $"{gameName}_translated_backup");

                MessageBox.Show(
                         "Перевод выбранной игры полность удален.\nПроверьте целостность игровых файлов через Steam.",
                         "Удаление перевода",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
            }
        }

        void CreateBackupTest()
        {
            if (!Directory.Exists(Application.StartupPath + @"backup"))
                Directory.CreateDirectory(Application.StartupPath + @"backup");
            System.Threading.Thread.Sleep(500);
            if (File.Exists(backupPath + $"{gameName}_backup"))
            {
                if (translateUpdate && machineName == myMachineName)
                    File.Copy(backupPath + $"{gameName}_backup", backupPath + $"{gameName}_backup_old");

                System.Threading.Thread.Sleep(500);
                deleteFile(backupPath + $"{gameName}_backup");
            }
            System.Threading.Thread.Sleep(500);
            if (File.Exists(backupPath + $"{gameName}_global_backup"))
            {
                if (translateUpdate && machineName == myMachineName)
                    File.Copy(backupPath + $"{gameName}_global_backup", backupPath + $"{gameName}_global_backup_old");

                System.Threading.Thread.Sleep(500);
                deleteFile(backupPath + $"{gameName}_global_backup");
            }

            System.Threading.Thread.Sleep(500);
            if (gameName != "CoreKeeper" && gameName != "NewWorld" && gameName != "DungeonCrawler" && gameName != "nwr")
            {
                File.Copy(dataPath + @"\resources.assets", backupPath + $"{gameName}_backup");
                if (gameName == "Temtem")
                    File.Copy(dataPath + @"\globalgamemanagers.assets", backupPath + gameName + "_global_backup");
            }

            if (gameName == "NewWorld")
            {
                File.Copy(dataPath + @"\DataStrm-part1.pak", backupPath + $"{gameName}_backup");
            }
            else if (gameName == "DungeonCrawler")
            {
                File.Copy(dataPath + @"\pakchunk0-Windows_0_P.pak", backupPath + $"{gameName}_backup");
            }
            else if (gameName == "nwr")
            {
                File.Copy(path + @"\addon_english.txt", backupPath + $"{gameName}_backup");
            }

            /*if (gameName == "Escape Simulator")
            {
                Directory.CreateDirectory(Application.StartupPath + @"backup\EscapeSimulator_backup");
            }*/

            /*if (File.Exists(backupPath + "Unity.exe"))
            {
                deleteFile(backupPath + @"Unity_Assets_Files\resources\Mono\Assembly-CSharp\I2.Loc\I2Languages.csv"); 
                deleteFile(backupPath + @"Unity_Assets_Files\resources\Mono\Assembly-CSharp\I2.Loc\Parser_ULS.exe");
                deleteFile(backupPath + @"Unity_Assets_Files\resources\Mono\Assembly-CSharp\I2.Loc\export_uls.bat");
                deleteFile(backupPath + "Unity.exe");
                deleteFile(backupPath + "Export_l2.bat");
                deleteFile(backupPath + "Export_Binary.bat");
            }*/
        }

        async void StartInstallReadyTranslateAsync()
        {
            await Task.Run(() => StartInstallReadyTranslate());
        }

        void StartInstallReadyTranslate()
        {
            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 4));

            bool idCheck = false;

            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Загрузка готового перевода...", Color.Black)));

            if (gameName != "CoreKeeper" && gameName != "DungeonCrawler")
            {
                StartExport();
                System.Threading.Thread.Sleep(500);
                CreateBackupTest();

                System.Threading.Thread.Sleep(500);
                if (File.Exists(dataPath + @"\resources.assets"))
                    deleteFile(dataPath + @"\resources.assets");
                if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                    deleteFile(dataPath + @"\UPacker_Advanced.exe");
                if (File.Exists(dataPath + @"\UPacker.exe"))
                    deleteFile(dataPath + @"\UPacker.exe");
            }

            if (gameName == "DungeonCrawler")
            {
                dataPath = path + @"\" + gameName + @"\Content\Paks";

                if (!Directory.Exists(dataPath))
                    Directory.CreateDirectory(dataPath);

                if (!File.Exists(backupPath + @"\DungeonCrawler_backup"))
                {
                    //File.Copy(dataPath + @"\pakchunk0-Windows_0_P.pak", backupPath + @"\DungeonCrawler_backup");
                    File.Copy(path + @"\" + gameName + @"\Content\Paks" + @"\pakchunk0-Windows_0_P.pak", backupPath + @"\DungeonCrawler_backup");
                }
                System.Threading.Thread.Sleep(500);
                deleteFile(path + @"\" + gameName + @"\Content\Paks" + @"\pakchunk0-Windows_0_P.pak");
            }
            System.Threading.Thread.Sleep(500);

            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
            int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

            if (gameName == "CoreKeeper")
            {
                if (yandexButton.Checked)
                    //Downloading_Files("https://downloader.disk.yandex.ru/disk/efbb7bef0e5fbdd91254b55a854ad4b32de68a637ed77303cb9d8674bb48dfed/6471de96/KDCJoYsET2b29yC03CAOk2V2B7vrSvZHf4wmBpZNE88TNLNbrIwfh59JsvkntJojqKk-F9s9K5YHOX66Cwu0bQ%3D%3D?uid=0&filename=CoreKeeper_translated_backup&disposition=attachment&hash=9i94YlN70SrJAbo%2B9sr15IuX5E%2Btr64sxjoZ18jCYs7bu5C4ksWT3Pxl6c2bZ3%2Blq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=text%2Fplain&owner_uid=132802752&fsize=1528640&hid=db95a36140f262f97155d5738f9659fc&media_type=text&tknv=v2", path + @"\localization\Localization.csv"); //yandex
                    Downloading_Files("https://drive.google.com/uc?export=download&id=1zHB5qio3eKNezs5gEMnMYgM1RBrDDaNE&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", path + @"\localization\Localization.csv"); //yandex

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
            }
            else if (gameName == "Escape Simulator")
            {
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 3));

                System.Threading.Thread.Sleep(500);
                if (yandexButton.Checked)
                    //Downloading_Files("https://downloader.disk.yandex.ru/disk/08126865778282f6abff61760af6e47ab6bfca1ad679c7395acbd2bc248cde0d/6471e017/5vh2sRUvGYMCC5LYmj5UZ3JzbD-wFZgBRlPY8KZmelpgOPJss7d_Ovu9vY5pYe_Lp118SHKicBBon-_l89pOFw%3D%3D?uid=0&filename=Escape%20Simulator_translated_backup&disposition=attachment&hash=BLePB%2BSBe7fNWRrDnAmWvVq8kyz%2BqE9fW/RRaJQW/NJTXjhNIL7ZMm5wiKtecHLeq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=8884220&hid=0ce32835aa86fc73ca9189e99c0f3ff9&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //yandex
                    Downloading_Files("https://drive.google.com/uc?export=download&id=1fXpKDiKrQpj1ohPa5w6LVLr5X61f77IW&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //yandex

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5");

                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4.assets");
                deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5.assets");

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                Downloading_Files("https://drive.google.com/uc?export=download&id=1wCASlNXEYLrhI4Vm6bSPMnMqzUU-cpKV&confirm=t&uuid=2f10594c-ea34-4ffd-bea5-bf9868021425&at=ACjLJWnO6PTXAIizOXJbufNqjrdU:1672137241899", dataPath + @"\es_texs.zip");
                //Downloading_Files("https://downloader.disk.yandex.ru/disk/31dff3060e6c9c945a88ffb0dbbde81d103406b7fac11ee5d1028768ff8ffdbf/6471e2b9/KDCJoYsET2b29yC03CAOk-iuM9VWB0J4NiPrgqq4Homv10IkS9R_IjQVZrS5QIeu_Kd9Uavm-mBs8e-Jc0ivZA%3D%3D?uid=0&filename=es_tex.zip&disposition=attachment&hash=gVEfqrkJ7ONwwCoyuRoCCqEQiUA7u130eACHi1bAIf7fNdiuQ/4VfGw9XZN%2BDTxQq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=132802752&fsize=478248813&hid=2c691d199f88a961a20b47045bd51dab&media_type=compressed&tknv=v2", dataPath + @"\es_texs.zip");
                System.Threading.Thread.Sleep(500);
                //Downloading_Files("https://downloader.disk.yandex.ru/disk/ee303f2fe8500c4dec1f44a6520c5225d886f7c4c32c1ef5afcac79f40b8ed9e/6471e2d6/KDCJoYsET2b29yC03CAOk15a97JSv2eyDjByuEZ3VAsFR7qL7VzZ8C5Z2RHJZ3-eaR0KQQiuATfQrhYONncr-Q%3D%3D?uid=0&filename=es_tex2.zip&disposition=attachment&hash=w80%2BJR2Xupnpz2s%2BpW0C0KSDQGl6hd9ris9MlW721bhIrXQN11jrMnVLp6YxDigLq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=132802752&fsize=709306497&hid=edca4f7175391b8e401749ad7fb37873&media_type=compressed&tknv=v2", dataPath + @"\es_texs2.zip");
                System.Threading.Thread.Sleep(500);

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                ZipFile.ExtractToDirectory(dataPath + @"\es_texs.zip", dataPath + @"\StreamingAssets\AssetBundles\Levels");
                System.Threading.Thread.Sleep(500);

                deleteFile(dataPath + @"\es_texs.zip");
                System.Threading.Thread.Sleep(500);

                //ZipFile.ExtractToDirectory(dataPath + @"\es_texs2.zip", dataPath + @"\StreamingAssets\AssetBundles\Levels");
                //System.Threading.Thread.Sleep(500);
                //
                //deleteFile(dataPath + @"\es_texs2.zip");

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
            }
            else if (gameName == "Temtem")
            {
                if (yandexButton.Checked)
                    //Downloading_Files("https://downloader.disk.yandex.ru/disk/ae4c5a03ad18ef2767bcd5e32fd116dcf8fd42f5e21bd3be4e4bce16f4764193/6471e348/KDCJoYsET2b29yC03CAOk-7TPv0tfwIiBQxRXtGvj9ip9JDo1OrqQ01YdhzypNPdfDdTwn_kKVN9HtES0_eN1Q%3D%3D?uid=0&filename=Temtem_translated_backup&disposition=attachment&hash=zPVTGGvnoHDIm09Fs7Cb1007LVsUu5hEY4l0yE6eLsodjfP9bJquM5uv1XuHzDGqq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=33688584&hid=204ccdfbcfad5d128e50ef1a139e5d73&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //yandex
                    Downloading_Files("https://drive.google.com/uc?export=download&id=1PnzSckZWc44GWSgOGkgCKig7DKW6bxSD&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //yandex
                else if (deeplButton.Checked)
                    //Downloading_Files("https://downloader.disk.yandex.ru/disk/ae4c5a03ad18ef2767bcd5e32fd116dcf8fd42f5e21bd3be4e4bce16f4764193/6471e348/KDCJoYsET2b29yC03CAOk-7TPv0tfwIiBQxRXtGvj9ip9JDo1OrqQ01YdhzypNPdfDdTwn_kKVN9HtES0_eN1Q%3D%3D?uid=0&filename=Temtem_translated_backup&disposition=attachment&hash=zPVTGGvnoHDIm09Fs7Cb1007LVsUu5hEY4l0yE6eLsodjfP9bJquM5uv1XuHzDGqq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=33688584&hid=204ccdfbcfad5d128e50ef1a139e5d73&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //deepl
                    Downloading_Files("https://drive.google.com/uc?export=download&id=1WYi13nZ9r4nYmvSlPjEDI1BaxIsS_Dbv&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //deepl

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                if (File.Exists(dataPath + @"\UPacker.exe"))
                    deleteFile(dataPath + @"\UPacker.exe");
                if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                    deleteFile(dataPath + @"\UPacker_Advanced.exe");
                if (File.Exists(localizationPath + @"I2Languages.csv"))
                    deleteFile(localizationPath + @"I2Languages.csv");
                if (File.Exists(localizationPath + @"I2Languages.LanguageSourceAsset"))
                    deleteFile(localizationPath + @"I2Languages.LanguageSourceAsset");
                if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                    deleteFile(localizationPath + @"Parser_ULS.exe");
                if (File.Exists(localizationPath + @"export_uls.bat"))
                    deleteFile(localizationPath + @"export_uls.bat");
                if (File.Exists(localizationPath + @"import_uls.bat"))
                    deleteFile(localizationPath + @"import_uls.bat");
                delFolder(localizationPath);
                delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                delFolder(dataPath + @"\Unity_Assets_Files\resources");
                delFolder(dataPath + @"\Unity_Assets_Files");

                System.Threading.Thread.Sleep(15000);

                StartExport();
                unArchivBackup();

                System.Threading.Thread.Sleep(500);
                while (!File.Exists(localizationPath + @"I2Languages.csv"))
                    System.Threading.Thread.Sleep(500);

                int translatedLines = File.ReadAllLines(localizationPath + @"I2Languages.csv").Length;
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + translatedLines + 5));

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                IdCheckForCompletedTranslate();

                Import();
            }
            else if (gameName == "The Mortuary Assistant")
            {
                if (yandexButton.Checked)
                {
                    //Downloading_Files("https://downloader.disk.yandex.ru/disk/64bbee4a3c937d287e97626c844c27e97355685c09d119f51fd7e057b85de8c0/6471e38c/KDCJoYsET2b29yC03CAOk2q3Y8qRT1gQT6IMP-p2cDwBNycLa9MW7x_ixFLVIGEIRdOyxv966bYjC0j2X968mA%3D%3D?uid=0&filename=The%20Mortuary%20Assistant_translated_backup&disposition=attachment&hash=F3DT0yVFzTxloDU%2BZlOGXP8U9WVPC%2B5I4jkxKPF9dpoXAQ0shy3RvfAw160gIT6xq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=62436588&hid=0b4851c237d9881f8909649b66b89c0b&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //yandex
                    Downloading_Files("https://drive.google.com/uc?export=download&id=1bHyCzZyIiMSXqe4NZaBPCLO2WLY9nW7L&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //yandex
                }

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
            }
            else if (gameName == "DungeonCrawler")
            {
                if (!skillsNameCheckTranslate.Checked && !skillsDescriptionCheckTranslate.Checked && !itemsCheckTranslate.Checked && !npcCheckTranslate.Checked && !itemsDesc.Checked)
                {
                    dataPath = path + @"\" + gameName + @"\Content\Paks\~mods";

                    if (!Directory.Exists(dataPath))
                        Directory.CreateDirectory(dataPath);

                    if (yandexButton.Checked)
                        //Downloading_Files("https://downloader.disk.yandex.ru/disk/529d0741b43a1bc54acf1b8bf9e3528ba26b0bc80e1eb0b183168402fb854d49/6471e3f5/KDCJoYsET2b29yC03CAOkyOBwRip1O6oXFwDQLB20GI_hNcHC2cL0Cl3a3c5GdU2QlP81drjAPwMYgQgHSCK5A%3D%3D?uid=0&filename=DungeonCrawler_translated_backup&disposition=attachment&hash=lvxiFKJH47VErPpS3%2BkS8JaKlBnb58ZgRxVyCO/pbaY7lDevDVVSgx8g9j8QNdcTq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=22790573&hid=ca5d746506debd7e81a0c70e16b5fb54&media_type=data&tknv=v2", dataPath + @"\pakchunk0-Windows_0_P.pak"); //yandex
                        Downloading_Files("https://drive.google.com/uc?export=download&id=1WjUKNVbVfW55QAjol2bBSX3Azn4f7IjN&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\pakchunk0-Windows_0_P.pak"); //yandex
                    else if (deeplButton.Checked)
                        //Downloading_Files("https://downloader.disk.yandex.ru/disk/529d0741b43a1bc54acf1b8bf9e3528ba26b0bc80e1eb0b183168402fb854d49/6471e3f5/KDCJoYsET2b29yC03CAOkyOBwRip1O6oXFwDQLB20GI_hNcHC2cL0Cl3a3c5GdU2QlP81drjAPwMYgQgHSCK5A%3D%3D?uid=0&filename=DungeonCrawler_translated_backup&disposition=attachment&hash=lvxiFKJH47VErPpS3%2BkS8JaKlBnb58ZgRxVyCO/pbaY7lDevDVVSgx8g9j8QNdcTq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=22790573&hid=ca5d746506debd7e81a0c70e16b5fb54&media_type=data&tknv=v2", dataPath + @"\pakchunk0-Windows_0_P.pak"); //deepl
                        Downloading_Files("https://drive.google.com/uc?export=download&id=1Pp3rag6dNFQkTRqLmbSAVsHJUKJF0gnv&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\pakchunk0-Windows_0_P.pak"); //deepl
                }
                else
                {
                    idCheck = true;

                    //Downloading_Files("https://downloader.disk.yandex.ru/disk/1647ffc297075f6765f140e9f718f952451e97558ede4ec533f25e3d5dc2dd08/6471e43d/KDCJoYsET2b29yC03CAOk8WYzHjxZD1UBqMgM9GDoQHew4dvScwqonDzGhz6rTs6rHrW2i4C0zNWT3nXI8PJLw%3D%3D?uid=0&filename=DungeonCrawler.zip&disposition=attachment&hash=fo90aF8tvczFPkRkU7RqlvijeKXq6vgfzss%2B4SnnFcF3GIPFZTwLy7U7EfHPvb%2Baq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=132802752&fsize=23075190&hid=423ddb166164a4dc3f2600db429bccff&media_type=compressed&tknv=v2", dataPath + @"\DungeonCrawler.zip"); //yandex
                    Downloading_Files("https://drive.google.com/uc?export=download&id=1LanJhy6kKlRYnaZ-CiOJr3-hlx0-sTje&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\DungeonCrawler.zip"); //yandex

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    if (Directory.Exists(dataPath + @"\pakchunk0-Windows_0_P"))
                        delFolder(dataPath + @"\pakchunk0-Windows_0_P");
                    System.Threading.Thread.Sleep(500);

                    ZipFile.ExtractToDirectory(dataPath + @"\DungeonCrawler.zip", dataPath);

                    localizationPath = dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en\";

                    deleteFile(dataPath + @"\DungeonCrawler.zip");

                    if (!File.Exists(localizationPath + @"UE4localizationsTool.exe"))
                        File.WriteAllBytesAsync(localizationPath + @"UE4localizationsTool.exe", Resources.UE4localizationsTool_exe);
                    File.WriteAllTextAsync(localizationPath + @"export_txt_from_locres.bat", Resources.export_txt_from_locres_bat);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"export_txt_from_locres.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"export_txt_from_locres.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                    System.Threading.Thread.Sleep(500);

                    deleteFile(localizationPath + @"UE4localizationsTool.exe");
                    deleteFile(localizationPath + @"export_txt_from_locres.bat");

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    //Downloading_Files("https://downloader.disk.yandex.ru/disk/8bee20fe5e3370a4597af5a6f8ccd2bf6351c668a3559eb1c80307a5d17b29bb/6471e493/KDCJoYsET2b29yC03CAOk2SWYSLJrZ6HvPC5YMn6QPHmDrOE0GHKwIrGpFgBYDbes18_-ud5_RiMLX4ewnnAtA%3D%3D?uid=0&filename=Game_translated.locres.txt&disposition=attachment&hash=a7liznpMuBNzF3PWq3lnj2ckyqaaL/HubPgFXDfcXWDxijE5ckTnoXrJS%2B6fzsC9q/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=text%2Fplain&owner_uid=132802752&fsize=269957&hid=38a96d4a4816ea144da8386a48104651&media_type=document&tknv=v2", localizationPath + @"Game_translated.locres.txt"); //yandex
                    Downloading_Files("https://drive.google.com/uc?export=download&id=1zzO9GwqE0YyaZ8PlWbupwkzuKkR7tXCb&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", localizationPath + @"Game_translated.locres.txt"); //yandex

                    System.Threading.Thread.Sleep(500);
                    int translatedLines = File.ReadAllLines(localizationPath + @"Game.locres.txt").Length;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + translatedLines + 5));

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    IdCheckForCompletedTranslate();

                    deleteFile(localizationPath + "Game.locres.txt");
                    File.Copy(localizationPath + "Game_translated.locres.txt", localizationPath + "Game.locres.txt");
                    System.Threading.Thread.Sleep(500);
                    deleteFile(localizationPath + "Game_translated.locres.txt");

                    Import();
                }

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
            }

            if (gameName == "NewWorld")
            {
                string[] nwFilesToRead = Directory.GetFiles(localizationPath);
                string globalTextFile = localizationPath + @"Global_English.txt";

                if (yandexButton.Checked)
                    Downloading_Files("", globalTextFile); //yandex
                System.Threading.Thread.Sleep(500);

                File.Copy(globalTextFile, backupPath + gameName + "_translated_backup");

                string[] globalTranslatedLines = File.ReadAllLines(globalTextFile);

                for (int i = 0; i < nwFilesToRead.Length; i++)
                {
                    string[] originalLines = File.ReadAllLines(nwFilesToRead[i]);

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(nwFilesToRead[i]);

                    XmlElement xRoot = xDoc.DocumentElement;

                    if (xRoot != null)
                    {
                        foreach (XmlElement xnode in xRoot)
                        {
                            string? id = xnode.Attributes.GetNamedItem("key").Value;

                            for (int ii = 0; ii < globalTranslatedLines.Length; ii++)
                            {
                                string[] translatedValues = globalTranslatedLines[ii].Split(SeparatorDifferentGames());

                                if (translatedValues[0] == id)
                                {
                                    if (CheckForCompletedTranslate(nwFilesToRead[i], id))
                                        xnode.InnerText = translatedValues[1];

                                    break;
                                }
                            }
                        }
                    }

                    xDoc.Save(nwFilesToRead[i]);
                }

                deleteFile(globalTextFile);

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 0, "firstrun = 0");

                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Загрузка перевода завершена.", Color.Black)));

                string translateFinish = "Игра русифицирована!";

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum));
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

                MessageBox.Show(
                 translateFinish,
                 "Русификация",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information,
                 MessageBoxDefaultButton.Button1,
                 MessageBoxOptions.DefaultDesktopOnly);

                updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
                if (machineName == myMachineName)
                    deeplButton.BeginInvoke((MethodInvoker)(() => deeplButton.Enabled = true));
            }

            if (gameName != "Temtem" && gameName != "NewWorld" && gameName != "DungeonCrawler")
            {
                System.Threading.Thread.Sleep(500);
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                if (gameName != "CoreKeeper")
                    File.Copy(dataPath + @"\resources.assets", backupPath + gameName + "_translated_backup");
                else
                    File.Copy(path + @"\localization\Localization.csv", backupPath + gameName + "_translated_backup");
                System.Threading.Thread.Sleep(500);

                if (File.Exists(dataPath + @"\UPacker.exe"))
                    deleteFile(dataPath + @"\UPacker.exe");
                if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                    deleteFile(dataPath + @"\UPacker_Advanced.exe");
                if (File.Exists(localizationPath + @"I2Languages.csv"))
                    deleteFile(localizationPath + @"I2Languages.csv");
                if (File.Exists(localizationPath + @"I2Languages.LanguageSourceAsset"))
                    deleteFile(localizationPath + @"I2Languages.LanguageSourceAsset");
                if (File.Exists(localizationPath + @"English.txt"))
                    deleteFile(localizationPath + @"English.txt");
                if (File.Exists(localizationPath + @"EnglishEditor.txt"))
                    deleteFile(localizationPath + @"EnglishEditor.txt");
                if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                    deleteFile(localizationPath + @"Parser_ULS.exe");
                if (File.Exists(localizationPath + @"export_uls.bat"))
                    deleteFile(localizationPath + @"export_uls.bat");
                if (File.Exists(localizationPath + @"import_uls.bat"))
                    deleteFile(localizationPath + @"import_uls.bat");
                if (File.Exists(dataPath + @"\Export_txt.bat"))
                    deleteFile(dataPath + @"\Export_txt.bat");

                if (Directory.Exists(localizationPath))
                {
                    delFolder(localizationPath);
                    if (gameName == "Temtem")
                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                    else if (gameName == "CoreKeeper")
                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\I2");
                    if (gameName == "Temtem" || gameName == "CoreKeeper")
                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                    delFolder(dataPath + @"\Unity_Assets_Files\resources");
                    delFolder(dataPath + @"\Unity_Assets_Files");
                }

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 0, "firstrun = 0");

                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Загрузка перевода завершена.", Color.Black)));

                string translateFinish = "Игра русифицирована!";
                if (gameName == "The Mortuary Assistant")
                    translateFinish = "Игра русифицирована!\nНе забудьте включить субтитры в настройках игры!";

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum));
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

                MessageBox.Show(
                 translateFinish,
                 "Русификация",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information,
                 MessageBoxDefaultButton.Button1,
                 MessageBoxOptions.DefaultDesktopOnly);

                updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
                if (machineName == myMachineName)
                    deeplButton.BeginInvoke((MethodInvoker)(() => deeplButton.Enabled = true));
            }

            if (gameName == "DungeonCrawler" && !idCheck)
            {
                if (!File.Exists(backupPath + @"\DungeonCrawler_translated_backup"))
                {
                    File.Copy(dataPath + @"\pakchunk0-Windows_0_P.pak", backupPath + @"\DungeonCrawler_translated_backup");
                }

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 0, "firstrun = 0");

                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Загрузка перевода завершена.", Color.Black)));

                string translateFinish = "Игра русифицирована!";

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum));
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

                MessageBox.Show(
                 translateFinish,
                 "Русификация",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information,
                 MessageBoxDefaultButton.Button1,
                 MessageBoxOptions.DefaultDesktopOnly);

                updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
                if (machineName == myMachineName)
                    deeplButton.BeginInvoke((MethodInvoker)(() => deeplButton.Enabled = true));
            }
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            if (path == "")
            {
                MessageBox.Show(
                     "Укажите путь к игре.",
                     "Путь не указан",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                return;
            }

            updateOrReinstall = false;
            translateUpdate = false;
            updateReinstall.Enabled = false;
            recoverBD.Enabled = false;
            deleteTranslate.Enabled = false;
            btnStart.Enabled = false;
            UpdateButton.Enabled = false;
            choosePathBtn.Enabled = false;
            txtPath.Enabled = false;
            //fontSetup.Enabled = false;
            yandexButton.Enabled = false;
            deeplButton.Enabled = false;
            newRowsCheck = false;

            temtemCheckTranslate.Enabled = false;
            skillsNameCheckTranslate.Enabled = false;
            itemsCheckTranslate.Enabled = false;
            skillsDescriptionCheckTranslate.Enabled = false;
            npcCheckTranslate.Enabled = false;

            if (machineName != myMachineName)
            {
                if (File.Exists(Application.StartupPath + @$"backup\{gameName}_translated_backup"))
                {
                    MessageBox.Show(
                             "Игра " + gameName + " уже была русифицирована, нажмите кнопку \"Обновить/Переустановить\", чтобы переустановить перевод.",
                             "Игра уже была переведена",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 0));

                    string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                    if (configLines[0].Contains("firstrun = 0"))
                    {
                        updateReinstall.Enabled = true;
                        //recoverBD.Enabled = true;
                        deleteTranslate.Enabled = true;
                    }

                    btnStart.Enabled = true;
                    UpdateButton.Enabled = true;
                    choosePathBtn.Enabled = true;
                    txtPath.Enabled = true;
                    yandexButton.Enabled = true;

                    return;
                }

                string startInstallReadeTrasnlateMessage = "Установить машинный перевод?";
                if (gameName == "The Mortuary Assistant")                                                                                                                                                                                                                                          //в идеале объединить
                {                                                                                                                                                                                                                                                                                  //в идеале объединить                                                                                                                     //в идеале объединить
                    if (!path.Contains("Steam"))                                                                                                                                                                                                                                                                          //в идеале объединить
                    {                                                                                                                                                                                                                                                                              //в идеале объединить
                        if (!Directory.Exists(Application.StartupPath + "tmp"))//Создаём временную директорию                                                                                                                                                                                      //в идеале объединить
                            Directory.CreateDirectory(Application.StartupPath + "tmp");                                                                                                                                                                                                            //в идеале объединить
                                                                                                                                                                                                                                                                                                   //в идеале объединить
                        Downloading_Files("https://drive.google.com/u/0/uc?id=133H0Fa8zwFqjVJViY20GeLpABFxpj-Dj&export=download&confirm=t&uuid=539fa2d8-5ac2-48dd-b8d3-164bff2905da&at=AB6BwCCFIP68el8g5-AV873P7cJy:1698171282437", "tmp\\info.txt");//Загружаем файл с информацией о версии приложения
                                                                                                                                                                                                                                                     //Downloading_Files("https://cloclo51.cloud.mail.ru/weblink/view/PDGf/x9oxppGd5", Application.StartupPath + "tmp\\info.txt");//Загружаем файл с информацией о версии приложения                                                                        //в идеале объединить
                                                                                                                                                                                                                                                     //в идеале объединить
                        System.Threading.Thread.Sleep(5000);

                        string[] tmpLines = File.ReadAllLines(Application.StartupPath + "tmp\\info.txt");//Считываем первую строку, в ней указана версия                                                                                                                                           //в идеале объединить
                        string ver = tmpLines[4].Replace("mortuary_ver = ", "");                                                                                                                                                                                                                   //в идеале объединить
                                                                                                                                                                                                                                                                                                   //в идеале объединить
                        startInstallReadeTrasnlateMessage = $"Учтите, что пиратка обязательно должна быть версии {ver}!\nУстановить машинный перевод?";     //в идеале объединить
                                                                                                                                                            //в идеале объединить
                        if (File.Exists(Application.StartupPath + "tmp\\info.txt"))//Удаляем старый файл с информацией о последнем патче                                                                                                                                                           //в идеале объединить
                            Directory.Delete(Application.StartupPath + "tmp", true);                                                                                                                                                                                                               //в идеале объединить
                    }                                                                                                                                                                                                                                                                              //в идеале объединить
                }
                else                                                                                                                                                                                                                                                                               //в идеале объединить
                {                                                                                                                                                                                                                                                                                  //в идеале объединить
                    if (!path.Contains("Steam"))                                                                                                                                                                                                                                                                          //в идеале объединить
                    {                                                                                                                                                                                                                                                                              //в идеале объединить
                        if (!Directory.Exists(Application.StartupPath + "tmp"))//Создаём временную директорию                                                                                                                                                                                      //в идеале объединить
                            Directory.CreateDirectory(Application.StartupPath + "tmp");                                                                                                                                                                                                            //в идеале объединить
                                                                                                                                                                                                                                                                                                   //в идеале объединить
                        Downloading_Files("https://drive.google.com/u/0/uc?id=133H0Fa8zwFqjVJViY20GeLpABFxpj-Dj&export=download&confirm=t&uuid=539fa2d8-5ac2-48dd-b8d3-164bff2905da&at=AB6BwCCFIP68el8g5-AV873P7cJy:1698171282437", "tmp\\info.txt");//Загружаем файл с информацией о версии приложения
                                                                                                                                                                                                                                                     //Downloading_Files("https://cloclo51.cloud.mail.ru/weblink/view/PDGf/x9oxppGd5", Application.StartupPath + "tmp\\info.txt");//Загружаем файл с информацией о версии приложения                                                                        //в идеале объединить
                                                                                                                                                                                                                                                     //в идеале объединить

                        System.Threading.Thread.Sleep(5000);

                        string[] tmpLines = File.ReadAllLines(Application.StartupPath + "tmp\\info.txt");//Считываем первую строку, в ней указана версия                                                                                                                                           //в идеале объединить
                        string ver = "";                                                                                                                                                                                                                                                           //в идеале объединить
                        if (gameName == "CoreKeeper")                                                                                                                                                                                                                                              //в идеале объединить
                            ver = tmpLines[1].Replace("core_ver = ", "");                                                                                                                                                                                                                          //в идеале объединить
                        else if (gameName == "Escape Simulator")                                                                                                                                                                                                                                   //в идеале объединить
                            ver = tmpLines[2].Replace("escapesim_ver = ", "");                                                                                                                                                                                                                     //в идеале объединить
                        else if (gameName == "Temtem")                                                                                                                                                                                                                                             //в идеале объединить
                            ver = tmpLines[3].Replace("temtem_ver = ", ""); //в идеале объединить
                        else if (gameName == "DungeonCrawler")
                            ver = tmpLines[5].Replace("DungeonCrawler_ver = ", "");

                        if (gameName != "DungeonCrawler")
                            startInstallReadeTrasnlateMessage = $"Учтите, что пиратка обязательно должна быть версии {ver}!\nУстановить машинный перевод?";
                        else
                            startInstallReadeTrasnlateMessage = $"Учтите, что это первый тестовый вариант перевода, в будущем он будет доработан!\nПеревод для версии игры {ver}";
                        //в идеале объединить
                        //в идеале объединить
                        if (File.Exists(Application.StartupPath + "tmp\\info.txt"))//Удаляем старый файл с информацией о последнем патче
                            Directory.Delete(Application.StartupPath + "tmp", true);
                    }
                }

                if (gameName == "The Mortuary Assistant")
                {
                    DialogResult result = MessageBox.Show(
                                     startInstallReadeTrasnlateMessage,
                                     "Установка",
                                     MessageBoxButtons.OKCancel,
                                     MessageBoxIcon.Question,
                                     MessageBoxDefaultButton.Button1,
                                     MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Cancel)
                    {
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 0));

                        string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                        if (configLines[0].Contains("firstrun = 0"))
                        {
                            updateReinstall.Enabled = true;
                            //recoverBD.Enabled = true;
                            deleteTranslate.Enabled = true;
                        }

                        btnStart.Enabled = true;
                        UpdateButton.Enabled = true;
                        choosePathBtn.Enabled = true;
                        txtPath.Enabled = true;
                        yandexButton.Enabled = true;

                        return;
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show(
                                     startInstallReadeTrasnlateMessage,
                                     "Установка",
                                     MessageBoxButtons.OKCancel,
                                     MessageBoxIcon.Question,
                                     MessageBoxDefaultButton.Button1,
                                     MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Cancel)
                    {
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 0));

                        string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                        if (configLines[0].Contains("firstrun = 0"))
                        {
                            updateReinstall.Enabled = true;
                            //recoverBD.Enabled = true;
                            deleteTranslate.Enabled = true;
                        }

                        btnStart.Enabled = true;
                        UpdateButton.Enabled = true;
                        choosePathBtn.Enabled = true;
                        txtPath.Enabled = true;
                        yandexButton.Enabled = true;

                        return;
                    }
                }

                StartInstallReadyTranslateAsync();
            }
            else
            {
                if (gameName == "Temtem" || gameName == "Escape Simulator" || gameName == "The Mortuary Assistant" || gameName == "DungeonCrawler")
                {
                    StartExport();

                    if (File.Exists(backupPath + gameName + @"_translated_backup"))
                    {
                        MessageBox.Show(
                             "Игра " + gameName + " уже была русифицирована, нажмите кнопку \"Обновить/Переустановить\", чтобы обновить перевод.",
                             "Игра уже была переведена",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                        if (gameName == "DungeonCrawler")
                        {
                            deleteFile(dataPath + @"\export_locres.bat");
                            deleteFile(dataPath + @"\quickbms_4gb_files.exe");
                            deleteFile(localizationPath + @"export_txt_from_locres.bat");
                            deleteFile(localizationPath + @"Game.locres");
                            deleteFile(localizationPath + @"Game.locres.txt");
                            deleteFile(localizationPath + @"UE4localizationsTool.exe");

                            localizationPath = localizationPath.Replace(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en\", dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en");
                            delFolder(localizationPath);
                            delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game");
                            delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization");
                            delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content");
                            delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName);
                            delFolder(dataPath + @"\pakchunk0-Windows_0_P");
                        }
                        else
                        {
                            if (File.Exists(localizationPath + @"English.txt"))
                                deleteFile(localizationPath + @"English.txt");
                            if (File.Exists(localizationPath + @"EnglishEditor.txt"))
                                deleteFile(localizationPath + @"EnglishEditor.txt");
                            if (File.Exists(localizationPath + @"I2Languages.csv"))
                                deleteFile(localizationPath + @"I2Languages.csv");
                            if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                                deleteFile(localizationPath + @"Parser_ULS.exe");
                            if (File.Exists(localizationPath + @"export_uls.bat"))
                                deleteFile(localizationPath + @"export_uls.bat");
                            if (File.Exists(localizationPath + @"import_uls.bat"))
                                deleteFile(localizationPath + @"import_uls.bat");
                            delFolder(localizationPath);
                            if (File.Exists(dataPath + @"\UPacker.exe"))
                                deleteFile(dataPath + @"\UPacker.exe");
                            if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                                deleteFile(dataPath + @"\UPacker_Advanced.exe");
                        }

                        if (gameName == "Temtem")
                            delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");

                        if (gameName == "Temtem")
                        {
                            delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                            delFolder(dataPath + @"\Unity_Assets_Files\resources");
                            delFolder(dataPath + @"\Unity_Assets_Files");
                        }
                        else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                        {
                            delFolder(dataPath + @"\Unity_Assets_Files\resources");
                            delFolder(dataPath + @"\Unity_Assets_Files");
                        }

                        updateReinstall.Enabled = true;
                        //recoverBD.Enabled = true;
                        deleteTranslate.Enabled = true;
                        btnStart.Enabled = true;
                        UpdateButton.Enabled = true;
                        choosePathBtn.Enabled = true;
                        txtPath.Enabled = true;

                        return;
                    }
                    else if (!File.Exists(backupPath + gameName + @"_translated_backup") && File.Exists(localizationPath + @"backup.txt"))
                    {
                        DialogResult result = MessageBox.Show(
                                 "Похоже установка была прервана... Продолжить установку?\nСчитывание файлов локализации начнется заново, но уже переведенные строки будут пропущены.",
                                 "Процесс может занять длительное время!",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);
                        if (result == DialogResult.Cancel)
                        {
                            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                            if (configLines[0].Contains("firstrun = 0"))
                            {
                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }
                            else
                            {
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }

                            return;
                        }

                        installBreak = true;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(
                                 "Это длительный процесс, который может занять до нескольких часов.\nПродолжить?",
                                 "Процесс может занять длительное время!",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);
                        if (result == DialogResult.Cancel)
                        {
                            if (File.Exists(localizationPath + @"backup.txt"))
                                deleteFile(localizationPath + @"backup.txt");
                            if (File.Exists(localizationPath + @"English.txt"))
                                deleteFile(localizationPath + @"English.txt");
                            if (File.Exists(localizationPath + @"EnglishEditor.txt"))
                                deleteFile(localizationPath + @"EnglishEditor.txt");
                            if (File.Exists(localizationPath + @"I2Languages.csv"))
                                deleteFile(localizationPath + @"I2Languages.csv");
                            if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                                deleteFile(localizationPath + @"Parser_ULS.exe");
                            if (File.Exists(localizationPath + @"export_uls.bat"))
                                deleteFile(localizationPath + @"export_uls.bat");
                            if (File.Exists(localizationPath + @"import_uls.bat"))
                                deleteFile(localizationPath + @"import_uls.bat");
                            delFolder(localizationPath);
                            if (File.Exists(localizationPath + @"UPacker.exe"))
                                deleteFile(dataPath + @"\UPacker.exe");
                            if (File.Exists(localizationPath + @"UPacker_Advanced.exe"))
                                deleteFile(dataPath + @"\UPacker_Advanced.exe");
                            if (gameName == "Temtem")
                                delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                            if (gameName == "Temtem")
                            {
                                delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                                delFolder(dataPath + @"\Unity_Assets_Files\resources");
                                delFolder(dataPath + @"\Unity_Assets_Files");
                            }
                            else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                            {
                                delFolder(dataPath + @"\Unity_Assets_Files\resources");
                                delFolder(dataPath + @"\Unity_Assets_Files");
                            }

                            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                            if (configLines[0].Contains("firstrun = 0"))
                            {
                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }
                            else
                            {
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }

                            return;
                        }

                        installBreak = false;
                    }
                }
                else if (gameName == "CoreKeeper" || gameName == "nwr")
                {
                    StartExport();

                    if (File.Exists(backupPath + gameName + @"_translated_backup"))
                    {
                        MessageBox.Show(
                             "Игра " + gameName + " уже была русифицирована, нажмите кнопку \"Обновить/Переустановить\", чтобы обновить перевод.",
                             "Игра уже была переведена",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else if (!File.Exists(backupPath + gameName + @"_translated_backup") && File.Exists(localizationPath + @"backup.txt"))
                    {
                        DialogResult result = MessageBox.Show(
                                 "Похоже установка была прервана... Продолжить установку?\nСчитывание файлов локализации начнется заново, но уже переведенные строки будут пропущены.",
                                 "Процесс может занять длительное время!",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);
                        if (result == DialogResult.Cancel)
                        {
                            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                            if (configLines[0].Contains("firstrun = 0"))
                            {
                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }
                            else
                            {
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }

                            return;
                        }

                        installBreak = true;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(
                                 "Это длительный процесс, который может занять до нескольких часов.\nПродолжить?",
                                 "Процесс может занять длительное время!",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);
                        if (result == DialogResult.Cancel)
                        {
                            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                            if (configLines[0].Contains("firstrun = 0"))
                            {
                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }
                            else
                            {
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }

                            return;
                        }

                        installBreak = false;
                    }
                }
                else if (gameName == "NewWorld")
                {
                    StartExport();

                    if (File.Exists(backupPath + gameName + @"_translated_backup"))
                    {
                        MessageBox.Show(
                             "Игра " + gameName + " уже была русифицирована, нажмите кнопку \"Обновить/Переустановить\", чтобы обновить перевод.",
                             "Игра уже была переведена",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                        string[] files = Directory.GetFiles(localizationPath);
                        if (files.Length != 0)
                        {
                            for (int i = 0; i < files.Length; i++)
                            {
                                deleteFile(files[i]);
                            }
                        }
                        if (Directory.Exists(localizationPath))
                        {
                            delFolder(localizationPath);
                            delFolder(dataPath + @"\levels\newlevels");
                            delFolder(dataPath + @"\levels");
                        }
                        if (File.Exists(dataPath + @"\nw_oodle.bms"))
                            deleteFile(dataPath + @"\nw_oodle.bms");
                        if (File.Exists(dataPath + @"\quickbms_4gb_files.exe"))
                            deleteFile(dataPath + @"\quickbms_4gb_files.exe");
                        if (File.Exists(dataPath + @"\xml_q_export.bat"))
                            deleteFile(dataPath + @"\xml_q_export.bat");

                        updateReinstall.Enabled = true;
                        //recoverBD.Enabled = true;
                        deleteTranslate.Enabled = true;
                        btnStart.Enabled = true;
                        UpdateButton.Enabled = true;
                        choosePathBtn.Enabled = true;
                        txtPath.Enabled = true;

                        return;
                    }
                    else if (!File.Exists(backupPath + gameName + @"_translated_backup") && File.Exists(localizationPath + @"backup.txt"))
                    {
                        DialogResult result = MessageBox.Show(
                                 "Похоже установка была прервана... Продолжить установку?\nСчитывание файлов локализации начнется заново, но уже переведенные строки будут пропущены.",
                                 "Процесс может занять длительное время!",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);
                        if (result == DialogResult.Cancel)
                        {
                            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                            if (configLines[0].Contains("firstrun = 0"))
                            {
                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }
                            else
                            {
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }

                            return;
                        }

                        installBreak = true;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(
                                 "Это длительный процесс, который может занять до нескольких часов.\nПродолжить?",
                                 "Процесс может занять длительное время!",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);
                        if (result == DialogResult.Cancel)
                        {
                            string[] files = Directory.GetFiles(localizationPath);
                            if (files.Length != 0)
                            {
                                for (int i = 0; i < files.Length; i++)
                                {
                                    deleteFile(files[i]);
                                }
                            }
                            if (Directory.Exists(localizationPath))
                            {
                                delFolder(localizationPath);
                                delFolder(dataPath + @"\levels\newlevels");
                                delFolder(dataPath + @"\levels");
                            }
                            if (File.Exists(dataPath + @"\nw_oodle.bms"))
                                deleteFile(dataPath + @"\nw_oodle.bms");
                            if (File.Exists(dataPath + @"\quickbms_4gb_files.exe"))
                                deleteFile(dataPath + @"\quickbms_4gb_files.exe");
                            if (File.Exists(dataPath + @"\xml_q_export.bat"))
                                deleteFile(dataPath + @"\xml_q_export.bat");

                            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                            if (configLines[0].Contains("firstrun = 0"))
                            {
                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }
                            else
                            {
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                            }

                            return;
                        }

                        installBreak = false;
                    }
                }

                CreateBackupTest(); // need check

                if (gameName == "Temtem")
                {
                    int rowsInFile = File.ReadAllLines(localizationPath + @"I2Languages.csv").Length;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + rowsInFile + 27));
                }
                else if (gameName == "DungeonCrawler")
                {
                    int rowsInFile = File.ReadAllLines(localizationPath + @"Game.locres.txt").Length;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + rowsInFile + 27));
                }
                else if (gameName == "Escape Simulator")
                {
                    int rowsInFile = File.ReadAllLines(localizationPath + @"English.txt").Length;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + rowsInFile + 27));
                }
                else if (gameName == "CoreKeeper")
                {
                    if (!File.Exists(localizationPath + @"Localization_3.txt"))
                    {
                        File.WriteAllTextAsync(localizationPath + @"unpack_CX.bat", Resources.unpack_CX_bat);
                        File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                        System.Threading.Thread.Sleep(500);

                        ReplaceStringInFile(localizationPath + @"unpack_CX.bat", 1, "for %%a in (Localization.csv) do unPacker_CSV.exe -uc 3 \"%%a\"");
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = localizationPath + @"unpack_CX.bat",
                            WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();

                        deleteFile(localizationPath + @"Localization_3.txt");
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = localizationPath + @"unpack_CX.bat",
                            WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();
                    }

                    int rowsInFile = File.ReadAllLines(localizationPath + @"Localization_3.txt").Length;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + rowsInFile + 27));
                }
                else if (gameName == "nwr")
                {
                    int rowsInFile = File.ReadAllLines(path + @"\addon_english.txt").Length;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + rowsInFile + 27));
                }

                WorkAsync();
            }
        }

        bool CheckForCompletedTranslate(string fileName, string id)
        {
            if (!skillsNameCheckTranslate.Checked && !skillsDescriptionCheckTranslate.Checked && !itemsCheckTranslate.Checked && !npcCheckTranslate.Checked && !temtemCheckTranslate.Checked)
                return true;

            bool idCheck = true;

            if (skillsNameCheckTranslate.Checked)
            {
                if (fileName == "weaponabilities.loc.xml" && !id.Contains("description") || fileName == "weaponabilities.loc.xml" && id.Contains("active")
                    || fileName == "weaponabilities.loc.xml" && id.Contains("inactive") || id.Contains("ui_afflicted") && !id.Contains("tooltip")
                    || id.Contains("Perk_Attribute") && !id.Contains("Description") || id.Contains("PerkID") && id.Contains("Name")
                    || id.Contains("Status_Runeglass") && !id.Contains("Description"))
                {
                    idCheck = false;
                }
            }

            if (skillsDescriptionCheckTranslate.Checked)
            {
                if (fileName == "weaponabilities.loc.xml" && id.Contains("description") || fileName == "weaponabilities.loc.xml" && id.Contains("active")
                    || fileName == "weaponabilities.loc.xml" && id.Contains("inactive") || id.Contains("ui_afflicted") && id.Contains("tooltip")
                    || id.Contains("Perk_Attribute") && id.Contains("Description") || id.Contains("PerkID") && !id.Contains("Name")
                    || id.Contains("Status_Runeglass") && id.Contains("Description"))
                {
                    idCheck = false;
                }
            }

            if (itemsCheckTranslate.Checked)
            {
                if (fileName == "javelindata_itemdefinitions_master.loc.xml" && !id.Contains("Description")
                    || fileName == "javelindata_itemdefinitions_master.loc.xml" && !id.Contains("description"))
                {
                    idCheck = false;
                }
            }

            if (itemsDesc.Checked)
            {
                if (fileName == "javelindata_itemdefinitions_master.loc.xml" && id.Contains("Description")
                    || fileName == "javelindata_itemdefinitions_master.loc.xml" && id.Contains("description"))
                {
                    idCheck = false;
                }
            }

            return idCheck;
        }

        async void UpdateReadyTranslateAsync()
        {
            await Task.Run(() => UpdateReadyTranslate());
        }

        void UpdateReadyTranslate()
        {
            if (File.Exists(Application.StartupPath + "tmp\\info.txt"))//Удаляем файл с информацией о последнем патче
                deleteFile(Application.StartupPath + "tmp\\info.txt");

            if (gameName != "DungeonCrawler")
                StartExport();

            FileInfo backupFile = new FileInfo(backupPath + gameName + "_translated_backup");
            FileInfo newFile = new FileInfo(Application.StartupPath + "UTranslator.exe");
            if (gameName == "CoreKeeper")
            {
                if (File.Exists(path + @"\localization\Localization.csv"))
                    newFile = new FileInfo(path + @"\localization\Localization.csv");
            }
            else if (gameName == "DungeonCrawler")
            {
                dataPath = path + @"\" + gameName + @"\Content\Paks";

                //Directory.CreateDirectory(dataPath);

                newFile = new FileInfo(path + @"\" + gameName + @"\Content\Paks" + @"\pakchunk0-Windows_0_P.pak");
            }
            else
                newFile = new FileInfo(dataPath + @"\resources.assets");

            if (backupFile.Length != newFile.Length)
            {
                if (!Directory.Exists(Application.StartupPath + "tmp"))
                    Directory.CreateDirectory(Application.StartupPath + "tmp");

                Downloading_Files("https://drive.google.com/u/0/uc?id=133H0Fa8zwFqjVJViY20GeLpABFxpj-Dj&export=download&confirm=t&uuid=539fa2d8-5ac2-48dd-b8d3-164bff2905da&at=AB6BwCCFIP68el8g5-AV873P7cJy:1698171282437", "tmp\\info.txt");//Загружаем файл с информацией о версии приложения
                //Downloading_Files("https://cloclo51.cloud.mail.ru/weblink/view/PDGf/x9oxppGd5", Application.StartupPath + "tmp\\info.txt");//Загружаем файл с информацией о версии приложения

                System.Threading.Thread.Sleep(5000);

                string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                string cur_ver = "";
                if (gameName == "CoreKeeper")
                    cur_ver = configLines[12].Replace("core_ver = ", "");
                else if (gameName == "Escape Simulator")
                    cur_ver = configLines[17].Replace("escapesim_ver = ", "");
                else if (gameName == "Temtem")
                    cur_ver = configLines[18].Replace("temtem_ver = ", "");
                else if (gameName == "The Mortuary Assistant")
                    cur_ver = configLines[20].Replace("mortuary_ver = ", "");
                else if (gameName == "DungeonCrawler")
                    cur_ver = configLines[22].Replace("DungeonCrawler_ver = ", "");
                string[] tmpLines = File.ReadAllLines(Application.StartupPath + "tmp\\info.txt");//Считываем первую строку, в ней указана версия
                string new_ver = "";
                if (gameName == "CoreKeeper")
                    new_ver = tmpLines[1].Replace("core_ver = ", "");
                else if (gameName == "Escape Simulator")
                    new_ver = tmpLines[2].Replace("escapesim_ver = ", "");
                else if (gameName == "Escape Simulator")
                    new_ver = tmpLines[3].Replace("temtem_ver = ", "");
                else if (gameName == "The Mortuary Assistant")
                    new_ver = tmpLines[4].Replace("mortuary_ver = ", "");
                else if (gameName == "DungeonCrawler")
                    new_ver = tmpLines[5].Replace("DungeonCrawler_ver = ", "");

                if (new_ver != cur_ver)
                {
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 4));

                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Загрузка новой версии готового перевода...", Color.Black)));

                    if (gameName != "CoreKeeper" && gameName != "DungeonCrawler")
                    {
                        deleteFile(Application.StartupPath + @$"backup\{gameName}_backup");
                        System.Threading.Thread.Sleep(500);
                        CreateBackupTest();
                        System.Threading.Thread.Sleep(500);
                        deleteFile(dataPath + @"\resources.assets");
                    }
                    deleteFile(Application.StartupPath + @$"backup\{gameName}_translated_backup");
                    deleteFile(Application.StartupPath + "tmp\\info.txt");

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    if (gameName == "CoreKeeper")
                    {
                        if (yandexButton.Checked)
                            //Downloading_Files("https://downloader.disk.yandex.ru/disk/0f71b2bc84650edd3efe7bba3fb47fca5a2656e9f68fb2474cb3cb6b978e3002/6471e52c/KDCJoYsET2b29yC03CAOk2V2B7vrSvZHf4wmBpZNE88TNLNbrIwfh59JsvkntJojqKk-F9s9K5YHOX66Cwu0bQ%3D%3D?uid=0&filename=CoreKeeper_translated_backup&disposition=attachment&hash=9i94YlN70SrJAbo%2B9sr15IuX5E%2Btr64sxjoZ18jCYs7bu5C4ksWT3Pxl6c2bZ3%2Blq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=text%2Fplain&owner_uid=132802752&fsize=1528640&hid=db95a36140f262f97155d5738f9659fc&media_type=text&tknv=v2", path + @"\localization\Localization.csv"); //yandex
                            Downloading_Files("https://drive.google.com/uc?export=download&id=1zHB5qio3eKNezs5gEMnMYgM1RBrDDaNE&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", path + @"\localization\Localization.csv"); //yandex

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
                    }
                    else if (gameName == "Escape Simulator")
                    {
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 3));

                        System.Threading.Thread.Sleep(500);
                        if (yandexButton.Checked)
                            //Downloading_Files("https://downloader.disk.yandex.ru/disk/8651bf89b578410b2117fc26ac9db734d7b2a4f585cf210dc8b1cd128d624b76/6471e550/5vh2sRUvGYMCC5LYmj5UZ3JzbD-wFZgBRlPY8KZmelpgOPJss7d_Ovu9vY5pYe_Lp118SHKicBBon-_l89pOFw%3D%3D?uid=0&filename=Escape%20Simulator_translated_backup&disposition=attachment&hash=BLePB%2BSBe7fNWRrDnAmWvVq8kyz%2BqE9fW/RRaJQW/NJTXjhNIL7ZMm5wiKtecHLeq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=8884220&hid=0ce32835aa86fc73ca9189e99c0f3ff9&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //yandex
                            Downloading_Files("https://drive.google.com/uc?export=download&id=1fXpKDiKrQpj1ohPa5w6LVLr5X61f77IW&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //yandex

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5");

                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5.assets");

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        Downloading_Files("https://drive.google.com/uc?export=download&id=1wCASlNXEYLrhI4Vm6bSPMnMqzUU-cpKV&confirm=t&uuid=2f10594c-ea34-4ffd-bea5-bf9868021425&at=ACjLJWnO6PTXAIizOXJbufNqjrdU:1672137241899", dataPath + @"\es_texs.zip");
                        //Downloading_Files("https://downloader.disk.yandex.ru/disk/31dff3060e6c9c945a88ffb0dbbde81d103406b7fac11ee5d1028768ff8ffdbf/6471e2b9/KDCJoYsET2b29yC03CAOk-iuM9VWB0J4NiPrgqq4Homv10IkS9R_IjQVZrS5QIeu_Kd9Uavm-mBs8e-Jc0ivZA%3D%3D?uid=0&filename=es_tex.zip&disposition=attachment&hash=gVEfqrkJ7ONwwCoyuRoCCqEQiUA7u130eACHi1bAIf7fNdiuQ/4VfGw9XZN%2BDTxQq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=132802752&fsize=478248813&hid=2c691d199f88a961a20b47045bd51dab&media_type=compressed&tknv=v2", dataPath + @"\es_texs.zip");
                        System.Threading.Thread.Sleep(500);
                        //Downloading_Files("https://downloader.disk.yandex.ru/disk/ee303f2fe8500c4dec1f44a6520c5225d886f7c4c32c1ef5afcac79f40b8ed9e/6471e2d6/KDCJoYsET2b29yC03CAOk15a97JSv2eyDjByuEZ3VAsFR7qL7VzZ8C5Z2RHJZ3-eaR0KQQiuATfQrhYONncr-Q%3D%3D?uid=0&filename=es_tex2.zip&disposition=attachment&hash=w80%2BJR2Xupnpz2s%2BpW0C0KSDQGl6hd9ris9MlW721bhIrXQN11jrMnVLp6YxDigLq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=132802752&fsize=709306497&hid=edca4f7175391b8e401749ad7fb37873&media_type=compressed&tknv=v2", dataPath + @"\es_texs2.zip");
                        System.Threading.Thread.Sleep(500);

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        ZipFile.ExtractToDirectory(dataPath + @"\es_texs.zip", dataPath + @"\StreamingAssets\AssetBundles\Levels");
                        System.Threading.Thread.Sleep(500);

                        deleteFile(dataPath + @"\es_texs.zip");
                        System.Threading.Thread.Sleep(500);

                        //ZipFile.ExtractToDirectory(dataPath + @"\es_texs2.zip", dataPath + @"\StreamingAssets\AssetBundles\Levels");
                        //System.Threading.Thread.Sleep(500);
                        //
                        //deleteFile(dataPath + @"\es_texs2.zip");

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
                    }
                    else if (gameName == "Temtem")
                    {
                        if (yandexButton.Checked)
                            //Downloading_Files("https://downloader.disk.yandex.ru/disk/0a5ee83a1b9b656a54da78616828ede10a0ca37b1a69e99df061486d6645946b/6471e5b6/KDCJoYsET2b29yC03CAOk-7TPv0tfwIiBQxRXtGvj9ip9JDo1OrqQ01YdhzypNPdfDdTwn_kKVN9HtES0_eN1Q%3D%3D?uid=0&filename=Temtem_translated_backup&disposition=attachment&hash=zPVTGGvnoHDIm09Fs7Cb1007LVsUu5hEY4l0yE6eLsodjfP9bJquM5uv1XuHzDGqq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=33688584&hid=204ccdfbcfad5d128e50ef1a139e5d73&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //yandex
                            Downloading_Files("https://drive.google.com/uc?export=download&id=1PnzSckZWc44GWSgOGkgCKig7DKW6bxSD&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //yandex
                        else if (deeplButton.Checked)
                            //Downloading_Files("https://downloader.disk.yandex.ru/disk/0a5ee83a1b9b656a54da78616828ede10a0ca37b1a69e99df061486d6645946b/6471e5b6/KDCJoYsET2b29yC03CAOk-7TPv0tfwIiBQxRXtGvj9ip9JDo1OrqQ01YdhzypNPdfDdTwn_kKVN9HtES0_eN1Q%3D%3D?uid=0&filename=Temtem_translated_backup&disposition=attachment&hash=zPVTGGvnoHDIm09Fs7Cb1007LVsUu5hEY4l0yE6eLsodjfP9bJquM5uv1XuHzDGqq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=33688584&hid=204ccdfbcfad5d128e50ef1a139e5d73&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //deepl
                            Downloading_Files("https://drive.google.com/uc?export=download&id=1WYi13nZ9r4nYmvSlPjEDI1BaxIsS_Dbv&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //deepl

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        if (File.Exists(dataPath + @"\UPacker.exe"))
                            deleteFile(dataPath + @"\UPacker.exe");
                        if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                            deleteFile(dataPath + @"\UPacker_Advanced.exe");
                        if (File.Exists(localizationPath + @"I2Languages.csv"))
                            deleteFile(localizationPath + @"I2Languages.csv");
                        if (File.Exists(localizationPath + @"I2Languages.LanguageSourceAsset"))
                            deleteFile(localizationPath + @"I2Languages.LanguageSourceAsset");
                        if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                            deleteFile(localizationPath + @"Parser_ULS.exe");
                        if (File.Exists(localizationPath + @"export_uls.bat"))
                            deleteFile(localizationPath + @"export_uls.bat");
                        if (File.Exists(localizationPath + @"import_uls.bat"))
                            deleteFile(localizationPath + @"import_uls.bat");
                        delFolder(localizationPath);
                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                        delFolder(dataPath + @"\Unity_Assets_Files\resources");
                        delFolder(dataPath + @"\Unity_Assets_Files");

                        System.Threading.Thread.Sleep(15000);

                        StartExport();
                        unArchivBackup();

                        System.Threading.Thread.Sleep(500);
                        while (!File.Exists(localizationPath + @"I2Languages.csv"))
                            System.Threading.Thread.Sleep(500);

                        int translatedLines = File.ReadAllLines(localizationPath + @"I2Languages.csv").Length;
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + translatedLines + 5));

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        IdCheckForCompletedTranslate();

                        ReplaceStringInFile(Application.StartupPath + "UTranslator.config", 18, "temtem_ver = " + new_ver);

                        Import();
                    }
                    else if (gameName == "The Mortuary Assistant")
                    {
                        if (yandexButton.Checked)
                        {
                            //Downloading_Files("https://downloader.disk.yandex.ru/disk/a404b7d15853ec455a68fabc1c288918d83d9989754fe46cfba231ffb5ead00d/6471e5e3/KDCJoYsET2b29yC03CAOk2q3Y8qRT1gQT6IMP-p2cDwBNycLa9MW7x_ixFLVIGEIRdOyxv966bYjC0j2X968mA%3D%3D?uid=0&filename=The%20Mortuary%20Assistant_translated_backup&disposition=attachment&hash=F3DT0yVFzTxloDU%2BZlOGXP8U9WVPC%2B5I4jkxKPF9dpoXAQ0shy3RvfAw160gIT6xq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=62436588&hid=0b4851c237d9881f8909649b66b89c0b&media_type=data&tknv=v2", dataPath + @"\resources.assets"); //yandex
                            Downloading_Files("https://drive.google.com/uc?export=download&id=1bHyCzZyIiMSXqe4NZaBPCLO2WLY9nW7L&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\resources.assets"); //yandex
                        }

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
                    }
                    else if (gameName == "DungeonCrawler")
                    {
                        if (!skillsNameCheckTranslate.Checked && !skillsDescriptionCheckTranslate.Checked && !itemsCheckTranslate.Checked && !npcCheckTranslate.Checked && !itemsDesc.Checked)
                        {
                            dataPath = path + @"\" + gameName + @"\Content\Paks\~mods";

                            if (!Directory.Exists(dataPath))
                                Directory.CreateDirectory(dataPath);

                            if (yandexButton.Checked)
                                //Downloading_Files("https://downloader.disk.yandex.ru/disk/97edc2170ed427d72a0836abb63031759b7705ee5ba4b15d12e466c21bbe9f53/6471e621/KDCJoYsET2b29yC03CAOkyOBwRip1O6oXFwDQLB20GI_hNcHC2cL0Cl3a3c5GdU2QlP81drjAPwMYgQgHSCK5A%3D%3D?uid=0&filename=DungeonCrawler_translated_backup&disposition=attachment&hash=lvxiFKJH47VErPpS3%2BkS8JaKlBnb58ZgRxVyCO/pbaY7lDevDVVSgx8g9j8QNdcTq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=22790573&hid=ca5d746506debd7e81a0c70e16b5fb54&media_type=data&tknv=v2", dataPath + @"\pakchunk0-Windows_0_P.pak"); //yandex
                                Downloading_Files("https://drive.google.com/uc?export=download&id=1WjUKNVbVfW55QAjol2bBSX3Azn4f7IjN&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\pakchunk0-Windows_0_P.pak"); //yandex
                            else if (deeplButton.Checked)
                                //Downloading_Files("https://downloader.disk.yandex.ru/disk/97edc2170ed427d72a0836abb63031759b7705ee5ba4b15d12e466c21bbe9f53/6471e621/KDCJoYsET2b29yC03CAOkyOBwRip1O6oXFwDQLB20GI_hNcHC2cL0Cl3a3c5GdU2QlP81drjAPwMYgQgHSCK5A%3D%3D?uid=0&filename=DungeonCrawler_translated_backup&disposition=attachment&hash=lvxiFKJH47VErPpS3%2BkS8JaKlBnb58ZgRxVyCO/pbaY7lDevDVVSgx8g9j8QNdcTq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Foctet-stream&owner_uid=132802752&fsize=22790573&hid=ca5d746506debd7e81a0c70e16b5fb54&media_type=data&tknv=v2", dataPath + @"\pakchunk0-Windows_0_P.pak"); //deepl
                                Downloading_Files("https://drive.google.com/uc?export=download&id=1Pp3rag6dNFQkTRqLmbSAVsHJUKJF0gnv&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\pakchunk0-Windows_0_P.pak"); //deepl

                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                            progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                            deleteFile(backupPath + gameName + "_translated_backup");
                            System.Threading.Thread.Sleep(500);
                            File.Copy(dataPath + @"\pakchunk0-Windows_0_P.pak", backupPath + gameName + "_translated_backup");

                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                            progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                            ReplaceStringInFile(Application.StartupPath + "UTranslator.config", 22, "DungeonCrawler_ver = " + new_ver);

                            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Загрузка перевода завершена.", Color.Black)));

                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum));
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

                            MessageBox.Show(
                                 "Перевод игры обновлен!",
                                 "Игра русифицирована",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information,
                                 MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.DefaultDesktopOnly);

                            updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                            deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                            btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                            UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                            choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                            txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                            yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
                        }
                        else
                        {
                            //Downloading_Files("https://downloader.disk.yandex.ru/disk/a126fd15b04bc3d5744d5ba5f4d7aec3df178484348dd71ee177fe5a31dd8208/6471e64d/KDCJoYsET2b29yC03CAOk8WYzHjxZD1UBqMgM9GDoQHew4dvScwqonDzGhz6rTs6rHrW2i4C0zNWT3nXI8PJLw%3D%3D?uid=0&filename=DungeonCrawler.zip&disposition=attachment&hash=fo90aF8tvczFPkRkU7RqlvijeKXq6vgfzss%2B4SnnFcF3GIPFZTwLy7U7EfHPvb%2Baq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=application%2Fzip&owner_uid=132802752&fsize=23075190&hid=423ddb166164a4dc3f2600db429bccff&media_type=compressed&tknv=v2", dataPath + @"\DungeonCrawler.zip"); //yandex
                            Downloading_Files("https://drive.google.com/uc?export=download&id=1LanJhy6kKlRYnaZ-CiOJr3-hlx0-sTje&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", dataPath + @"\DungeonCrawler.zip"); //yandex

                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                            progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                            if (Directory.Exists(dataPath + @"\pakchunk0-Windows_0_P"))
                                delFolder(dataPath + @"\pakchunk0-Windows_0_P");
                            System.Threading.Thread.Sleep(500);

                            ZipFile.ExtractToDirectory(dataPath + @"\DungeonCrawler.zip", dataPath);

                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                            progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                            localizationPath = dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en\";

                            deleteFile(dataPath + @"\DungeonCrawler.zip");

                            if (!File.Exists(localizationPath + @"UE4localizationsTool.exe"))
                                File.WriteAllBytesAsync(localizationPath + @"UE4localizationsTool.exe", Resources.UE4localizationsTool_exe);
                            File.WriteAllTextAsync(localizationPath + @"export_txt_from_locres.bat", Resources.export_txt_from_locres_bat);
                            System.Threading.Thread.Sleep(500);

                            Process.Start(new ProcessStartInfo
                            {
                                FileName = localizationPath + @"export_txt_from_locres.bat",
                                WorkingDirectory = Path.GetDirectoryName(localizationPath + @"export_txt_from_locres.bat"),
                                CreateNoWindow = true
                            }).WaitForExit();
                            System.Threading.Thread.Sleep(500);

                            deleteFile(localizationPath + @"UE4localizationsTool.exe");
                            deleteFile(localizationPath + @"export_txt_from_locres.bat");

                            //Downloading_Files("https://downloader.disk.yandex.ru/disk/950bf941534c0b3a271f01d70be9ca45de22544ff240a57d83b1717ba4f7fed7/6471e675/KDCJoYsET2b29yC03CAOk2SWYSLJrZ6HvPC5YMn6QPHmDrOE0GHKwIrGpFgBYDbes18_-ud5_RiMLX4ewnnAtA%3D%3D?uid=0&filename=Game_translated.locres.txt&disposition=attachment&hash=a7liznpMuBNzF3PWq3lnj2ckyqaaL/HubPgFXDfcXWDxijE5ckTnoXrJS%2B6fzsC9q/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=text%2Fplain&owner_uid=132802752&fsize=269957&hid=38a96d4a4816ea144da8386a48104651&media_type=document&tknv=v2", localizationPath + @"Game_translated.locres.txt"); //yandex
                            Downloading_Files("https://drive.google.com/uc?export=download&id=1zzO9GwqE0YyaZ8PlWbupwkzuKkR7tXCb&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", localizationPath + @"Game_translated.locres.txt"); //yandex

                            System.Threading.Thread.Sleep(500);
                            int translatedLines = File.ReadAllLines(localizationPath + @"Game.locres.txt").Length;
                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + translatedLines + 5));

                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                            progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                            IdCheckForCompletedTranslate();

                            deleteFile(localizationPath + "Game.locres.txt");
                            File.Copy(localizationPath + "Game_translated.locres.txt", localizationPath + "Game.locres.txt");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(localizationPath + "Game_translated.locres.txt");

                            ReplaceStringInFile(Application.StartupPath + "UTranslator.config", 22, "DungeonCrawler_ver = " + new_ver);

                            Import();
                        }
                    }

                    if (gameName != "Temtem" && gameName != "DungeonCrawler")
                    {
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        if (gameName != "CoreKeeper")
                            File.Copy(dataPath + @"\resources.assets", backupPath + gameName + "_translated_backup");
                        else
                            File.Copy(path + @"\localization\Localization.csv", backupPath + gameName + "_translated_backup");

                        if (File.Exists(dataPath + @"\UPacker.exe"))
                            deleteFile(dataPath + @"\UPacker.exe");
                        if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                            deleteFile(dataPath + @"\UPacker_Advanced.exe");
                        if (File.Exists(localizationPath + @"I2Languages.csv"))
                            deleteFile(localizationPath + @"I2Languages.csv");
                        if (File.Exists(localizationPath + @"I2Languages.LanguageSourceAsset"))
                            deleteFile(localizationPath + @"I2Languages.LanguageSourceAsset");
                        if (File.Exists(localizationPath + @"English.txt"))
                            deleteFile(localizationPath + @"English.txt");
                        if (File.Exists(localizationPath + @"EnglishEditor.txt"))
                            deleteFile(localizationPath + @"EnglishEditor.txt");
                        if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                            deleteFile(localizationPath + @"Parser_ULS.exe");
                        if (File.Exists(localizationPath + @"export_uls.bat"))
                            deleteFile(localizationPath + @"export_uls.bat");
                        if (File.Exists(localizationPath + @"import_uls.bat"))
                            deleteFile(localizationPath + @"import_uls.bat");
                        if (File.Exists(dataPath + @"\Export_txt.bat"))
                            deleteFile(dataPath + @"\Export_txt.bat");

                        if (Directory.Exists(localizationPath))
                        {
                            delFolder(localizationPath);
                            if (gameName == "Temtem")
                                delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                            if (gameName == "Temtem")
                                delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                            delFolder(dataPath + @"\Unity_Assets_Files\resources");
                            delFolder(dataPath + @"\Unity_Assets_Files");
                        }

                        if (gameName == "Escape Simulator")
                            ReplaceStringInFile(Application.StartupPath + "UTranslator.config", 17, "escapesim_ver = " + new_ver);
                        else if (gameName == "Temtem")
                            ReplaceStringInFile(Application.StartupPath + "UTranslator.config", 18, "temtem_ver = " + new_ver);
                        else if (gameName == "The Mortuary Assistant")
                            ReplaceStringInFile(Application.StartupPath + "UTranslator.config", 20, "mortuary_ver = " + new_ver);

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Загрузка перевода завершена.", Color.Black)));

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum));
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

                        MessageBox.Show(
                             "Игра русифицирована!",
                             "Русификация",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                        updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                        deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                        btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                        UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                        choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                        txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                        yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
                    }
                }
                else
                {
                    DialogResult newRowsNotFound = MessageBox.Show(
                         "Перевод еще не обновлен, дождитесь оповещания в группе ВК или попробуйте еще раз позже.",
                         "Обновление",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 0));

                    updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                    deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                    btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                    UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                    choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                    txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                    yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));

                    return;
                }
            }
            else if (newFile.Length == backupFile.Length)
            {
                if (gameName != "CoreKeeper")
                {
                    DialogResult newRowsNotFound = MessageBox.Show(
                         "Новые строки не найдены.\nПереустановить машинный перевод на новую версию игры?",
                         "Переустановка",
                         MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
                    if (newRowsNotFound == DialogResult.Cancel)
                    {
                        updateOrReinstall = false;
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                        updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                        deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                        btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                        UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                        choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                        txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                        yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));

                        BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Русификация " + gameName + " отменена.", Color.Black)));

                        return;
                    }

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 1));

                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Переустановка готового перевода...", Color.Black)));

                    if (gameName != "DungeonCrawler")
                    {
                        deleteFile(dataPath + @"\resources.assets");

                        File.Copy(backupPath + gameName + "_translated_backup", dataPath + @"\resources.assets");
                    }
                    else
                    {
                        deleteFile(dataPath + @"\pakchunk0-Windows_0_P.pak");

                        File.Copy(backupPath + gameName + "_translated_backup", dataPath + @"\pakchunk0-Windows_0_P.pak");
                    }

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Переустановка перевода завершена.", Color.Black)));

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

                    MessageBox.Show(
                         "Игра русифицирована!",
                         "Русификация",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                    updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                    deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                    btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                    UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                    choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                    txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                    yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
                }
                else
                {
                    MessageBox.Show(
                         "Новые строки не найдены.",
                         "Переустановка",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                    updateOrReinstall = false;
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                    updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                    deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));
                    btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                    UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                    choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                    txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true));
                    yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));

                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Русификация " + gameName + " отменена.", Color.Black)));

                    return;
                }
            }
        }

        void updateReinstall_Click(object sender, EventArgs e)
        {
            if (path == "")
            {
                MessageBox.Show(
                     "Укажите путь к игре.",
                     "Путь не указан",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                return;
            }

            int rowsInFile = 0;
            int rowsInBackupFile = 0;

            updateOrReinstall = false;
            translateUpdate = false;
            updateReinstall.Enabled = false;
            recoverBD.Enabled = false;
            deleteTranslate.Enabled = false;
            btnStart.Enabled = false;
            UpdateButton.Enabled = false;
            choosePathBtn.Enabled = false;
            txtPath.Enabled = false;
            yandexButton.Enabled = false;
            deeplButton.Enabled = false;

            temtemCheckTranslate.Enabled = false;
            skillsNameCheckTranslate.Enabled = false;
            itemsCheckTranslate.Enabled = false;
            skillsDescriptionCheckTranslate.Enabled = false;
            npcCheckTranslate.Enabled = false;

            if (gameName == "BookOfTravels")
            {
                DialogResult newRowsNotFound = MessageBox.Show(
                         "Для игры " + gameName + " поиск новых строк текста не требуется, нужно лишь переустановить перевод после обновления игры.\nПереустановить перевод?",
                         "Переустановка",
                         MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
                if (newRowsNotFound == DialogResult.Cancel)
                {
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                    updateReinstall.Enabled = true;
                    //recoverBD.Enabled = true;
                    deleteTranslate.Enabled = false;
                    btnStart.Enabled = true;
                    UpdateButton.Enabled = true;
                    choosePathBtn.Enabled = true;
                    txtPath.Enabled = true;
                    //fontSetup.Enabled = true;

                    return;
                }
                else if (newRowsNotFound == DialogResult.OK)
                {
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 10));

                    updateOrReinstall = true;
                    //AutoTranslatorAsync();
                }
            }
            else
            {
                if (File.Exists(backupPath + gameName + "_translated_backup.assets"))
                {
                    File.Copy(backupPath + gameName + "_translated_backup.assets", backupPath + gameName + "_translated_backup");
                    deleteFile(backupPath + gameName + "_translated_backup.assets");
                }
                if (File.Exists(backupPath + gameName + "globalgamemanagers.assets"))
                {
                    File.Copy(backupPath + gameName + "globalgamemanagers.assets", backupPath + gameName + "_global_backup");
                    deleteFile(backupPath + gameName + "globalgamemanagers.assets");
                }

                if (!File.Exists(backupPath + gameName + "_translated_backup"))
                {
                    MessageBox.Show(
                             "Игра не русифицирована, нажмите кнопку \"Установить\"",
                             "Игра не русифицирована",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Error,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                    updateReinstall.Enabled = true;
                    //recoverBD.Enabled = true;
                    deleteTranslate.Enabled = true;
                    btnStart.Enabled = true;
                    UpdateButton.Enabled = true;
                    choosePathBtn.Enabled = true;
                    txtPath.Enabled = true;
                    yandexButton.Enabled = true;
                    if (machineName == myMachineName)
                        deeplButton.Enabled = true;

                    return;
                }

                if (machineName != myMachineName)
                {
                    UpdateReadyTranslateAsync();
                }
                else
                {
                    updateOrReinstall = true;
                    newRowsCheck = true;
                    StartExport();
                    unArchivBackup();

                    newRowsCheck = false;

                    if (gameName == "Escape Simulator")
                    {
                        rowsInFile = File.ReadAllLines(localizationPath + @"English.txt").Length;
                        rowsInBackupFile = File.ReadAllLines(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\English.txt").Length;
                    }
                    else if (gameName == "The Mortuary Assistant")
                    {
                        filesToRead = Directory.GetFiles(localizationPath);
                        string[] backupFilesToRead = Directory.GetFiles(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");

                        for (int i = 0; i < filesToRead.Length; i++)
                        {
                            string[] globalText = new string[] { };
                            if (File.Exists(localizationPath + @"Global_English.txt"))
                                globalText = File.ReadAllLines(localizationPath + @"Global_English.txt");

                            using (StreamReader reader = new StreamReader(filesToRead[i], encoding: Encoding.UTF8))
                            {
                                using (StreamWriter sw = new StreamWriter(localizationPath + @"Global_English.txt", true, encoding: Encoding.UTF8))
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        string line = reader.ReadLine();

                                        if (line != "{" && line != "}" && line != "" && line != " " && line != "  ")
                                        {
                                            string[] idValues = line.Split(SeparatorDifferentGames());
                                            idValues[0] = idValues[0].TrimStart();
                                            bool exist = false;

                                            for (int ii = 0; ii < globalText.Length; ii++)
                                            {
                                                string[] idValuesGlobal = globalText[ii].Split(SeparatorDifferentGames());

                                                if (idValuesGlobal[0] == idValues[0])
                                                {
                                                    exist = true;

                                                    break;
                                                }
                                            }

                                            if (!exist)
                                            {
                                                line = line.TrimStart();
                                                sw.WriteLine(line);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < backupFilesToRead.Length; i++)
                        {
                            string[] globalText = new string[] { };
                            if (File.Exists(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Global_English.txt"))
                                globalText = File.ReadAllLines(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Global_English.txt");

                            using (StreamReader reader = new StreamReader(backupFilesToRead[i], encoding: Encoding.UTF8))
                            {
                                using (StreamWriter sw = new StreamWriter(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Global_English.txt", true, encoding: Encoding.UTF8))
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        string line = reader.ReadLine();

                                        if (line != "{" && line != "}" && line != "" && line != " " && line != "  ")
                                        {
                                            string[] idValues = line.Split(SeparatorDifferentGames());
                                            idValues[0] = idValues[0].TrimStart();
                                            bool exist = false;

                                            for (int ii = 0; ii < globalText.Length; ii++)
                                            {
                                                string[] idValuesGlobal = globalText[ii].Split(SeparatorDifferentGames());

                                                if (idValuesGlobal[0] == idValues[0])
                                                {
                                                    exist = true;

                                                    break;
                                                }
                                            }

                                            if (!exist)
                                            {
                                                line = line.TrimStart();
                                                sw.WriteLine(line);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        rowsInFile = File.ReadAllLines(localizationPath + @"Global_English.txt").Length;
                        rowsInBackupFile = File.ReadAllLines(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Global_English.txt").Length;
                    }
                    else if (gameName == "NewWorld")
                    {
                        filesToRead = Directory.GetFiles(localizationPath);
                        string[] backupFilesToRead = Directory.GetFiles(backupPath + @"levels\newlevels\localization\en-us");

                        for (int i = 0; i < filesToRead.Length; i++)
                        {
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load(filesToRead[i]);

                            XmlElement xRoot = xDoc.DocumentElement;

                            if (xRoot != null)
                            {
                                foreach (XmlElement xnode in xRoot)
                                {
                                    string? id = xnode.Attributes.GetNamedItem("key").Value;
                                    string? text = xnode.InnerText;
                                    XmlNode genderNode = xnode.Attributes.GetNamedItem("gender");
                                    string gender = "";
                                    if (genderNode != null)
                                        gender = genderNode.Value;

                                    if (id != "")
                                    {
                                        using (StreamWriter sw = new StreamWriter(localizationPath + @"Global_English.txt", true, encoding: Encoding.UTF8))
                                        {

                                            if (gender == "")
                                                sw.WriteLine(id + "\t" + text);
                                            else if (gender != "")
                                                sw.WriteLine(id + "\t" + text + "\t" + gender);
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < backupFilesToRead.Length; i++)
                        {
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load(backupFilesToRead[i]);

                            XmlElement xRoot = xDoc.DocumentElement;

                            if (xRoot != null)
                            {
                                foreach (XmlElement xnode in xRoot)
                                {
                                    string? id = xnode.Attributes.GetNamedItem("key").Value;
                                    string? text = xnode.InnerText;
                                    XmlNode genderNode = xnode.Attributes.GetNamedItem("gender");
                                    string gender = "";
                                    if (genderNode != null)
                                        gender = genderNode.Value;

                                    if (id != "")
                                    {
                                        using (StreamWriter sw = new StreamWriter(backupFilesToRead + @"\Global_English.txt", true, encoding: Encoding.UTF8))
                                        {

                                            if (gender == "")
                                                sw.WriteLine(id + "\t" + text);
                                            else if (gender != "")
                                                sw.WriteLine(id + "\t" + text + "\t" + gender);
                                        }
                                    }
                                }
                            }
                        }

                        rowsInFile = File.ReadAllLines(localizationPath + @"Global_English.txt").Length;
                        rowsInBackupFile = File.ReadAllLines(backupFilesToRead + @"\Global_English.txt").Length;
                    }
                    else if (gameName == "Temtem")
                    {
                        rowsInFile = File.ReadAllLines(localizationPath + @"I2Languages.csv").Length;
                        rowsInBackupFile = File.ReadAllLines(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp\I2.Loc\I2Languages.csv").Length;
                    }
                    else if (gameName == "DungeonCrawler")
                    {
                        rowsInFile = File.ReadAllLines(localizationPath + @"Game.locres.txt").Length;
                        rowsInBackupFile = File.ReadAllLines(backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en\Game.locres.txt").Length;
                    }
                    else if (gameName == "CoreKeeper")
                    {
                        string translatedText = File.ReadAllText(localizationPath + @"Localization.csv");
                        rowsInFile = translatedText.Split("[new]").Length;
                        rowsInBackupFile = 0;
                    }
                    if (gameName == "Escape Simulator")
                    {
                        rowsInFile = File.ReadAllLines(path + @"\addon_english.txt").Length;
                        rowsInBackupFile = File.ReadAllLines(backupPath + @"addon_english.txt").Length;
                    }

                    if (rowsInFile == rowsInBackupFile)
                    {
                        if (gameName != "CoreKeeper" && gameName != "NewWorld" && gameName != "nwr")
                        {
                            DialogResult newRowsNotFound = MessageBox.Show(
                             "Новые строки не найдены.\nПереустановить машинный перевод на новую версию игры?",
                             "Переустановка",
                             MessageBoxButtons.OKCancel,
                             MessageBoxIcon.Question,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);
                            if (newRowsNotFound == DialogResult.Cancel)
                            {
                                updateOrReinstall = false;
                                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                                if (gameName == "DungeonCrawler")
                                {
                                    deleteFile(dataPath + @"\export_locres.bat");
                                    deleteFile(dataPath + @"\quickbms_4gb_files.exe");
                                    deleteFile(localizationPath + @"export_txt_from_locres.bat");
                                    deleteFile(localizationPath + @"Game.locres");
                                    deleteFile(localizationPath + @"Game.locres.txt");
                                    deleteFile(localizationPath + @"UE4localizationsTool.exe");

                                    //localizationPath = localizationPath.Replace(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en\", dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game");
                                    //deleteFile(localizationPath + @"\en\Game.locres");
                                    //delFolder(localizationPath + @"\en");
                                    //delFolder(localizationPath);
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game");
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization");
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content");
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName);
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P");
                                    // переделать

                                    string backupLocalizationPath = backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en\";

                                    deleteFile(backupPath + @"\export_locres.bat");
                                    deleteFile(backupPath + @"\quickbms_4gb_files.exe");
                                    deleteFile(backupLocalizationPath + @"export_txt_from_locres.bat");
                                    deleteFile(backupLocalizationPath + @"Game.locres");
                                    deleteFile(backupLocalizationPath + @"Game.locres.txt");
                                    deleteFile(backupLocalizationPath + @"UE4localizationsTool.exe");

                                    //backupLocalizationPath = backupLocalizationPath.Replace(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game\en\", backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game");
                                    //deleteFile(backupLocalizationPath + @"\en\Game.locres");
                                    //delFolder(backupLocalizationPath + @"\en");
                                    //delFolder(backupLocalizationPath);
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game");
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization");
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content");
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName);
                                    //delFolder(backupPath + @"\" + gameName + @"\backup");
                                }
                                else
                                {
                                    if (File.Exists(dataPath + @"\UPacker.exe"))
                                        deleteFile(dataPath + @"\UPacker.exe");
                                    if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                                        deleteFile(dataPath + @"\UPacker_Advanced.exe");
                                    if (File.Exists(localizationPath + @"English.txt"))
                                        deleteFile(localizationPath + @"English.txt");
                                    if (File.Exists(localizationPath + @"I2Languages.csv"))
                                        deleteFile(localizationPath + @"I2Languages.csv");
                                    if (File.Exists(localizationPath + @"I2Languages.LanguageSourceAsset"))
                                        deleteFile(localizationPath + @"I2Languages.LanguageSourceAsset");
                                    if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                                        deleteFile(localizationPath + @"Parser_ULS.exe");
                                    if (File.Exists(localizationPath + @"export_uls.bat"))
                                        deleteFile(localizationPath + @"export_uls.bat");
                                    if (File.Exists(localizationPath + @"import_uls.bat"))
                                        deleteFile(localizationPath + @"import_uls.bat");

                                    if (gameName == "The Mortuary Assistant")
                                    {
                                        string[] backupFilesToRead = Directory.GetFiles(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                                        for (int i = 0; i < filesToRead.Length; i++)
                                        {
                                            deleteFile(filesToRead[i]);
                                        }
                                        for (int i = 0; i < backupFilesToRead.Length; i++)
                                        {
                                            deleteFile(backupFilesToRead[i]);
                                        }

                                        deleteFile(localizationPath + @"Global_English.txt");
                                    }
                                    delFolder(localizationPath);
                                    if (gameName == "Temtem")
                                    {
                                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                                    }
                                    delFolder(dataPath + @"\Unity_Assets_Files\resources");
                                    delFolder(dataPath + @"\Unity_Assets_Files");

                                    string backupLocalizationPath = "";
                                    if (gameName == "Temtem")
                                        backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp";
                                    else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                                        backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";

                                    if (gameName == "Temtem")
                                    {
                                        deleteFile(backupLocalizationPath + @"\I2.Loc\I2Languages.csv");
                                        delFolder(backupLocalizationPath + @"\I2.Loc");
                                        delFolder(backupLocalizationPath);
                                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono");
                                    }
                                    else if (gameName == "Escape Simulator") // для Mortuary удалил выше
                                        deleteFile(backupLocalizationPath + @"\English.txt");
                                    delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                                    delFolder(backupPath + @"\Unity_Assets_Files");
                                }

                                if (File.Exists(backupPath + gameName + "_backup.assets"))
                                {
                                    File.Copy(backupPath + gameName + "_backup.assets", backupPath + gameName + "_backup");
                                    deleteFile(backupPath + gameName + "_backup.assets");
                                }

                                if (File.Exists(backupPath + gameName + "_backup.pak"))
                                {
                                    File.Copy(backupPath + gameName + "_backup.pak", backupPath + gameName + "_backup");
                                    deleteFile(backupPath + gameName + "_backup.pak");
                                }

                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                                yandexButton.Enabled = true;
                                if (machineName == myMachineName)
                                    deeplButton.Enabled = true;

                                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Русификация " + gameName + " отменена.", Color.Black)));

                                return;
                            }
                            else if (newRowsNotFound == DialogResult.OK)
                            {
                                translateUpdate = false;
                                translateReinstall = true;
                                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 5));
                                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                                reinstallTranslate();
                            }
                        }
                        else if (gameName == "CoreKeeper" || gameName == "NewWorld" || gameName == "nwr")
                        {
                            if (gameName == "NewWorld")
                            {
                                string[] alreadyTranslatedFiles = Directory.GetFiles(dataPath + @"\levels\newlevels\localization");
                                string[] backupLocalizationPath = Directory.GetFiles(backupPath + @"levels\newlevels\localization\en-us");

                                for (int i = 0; i < filesToRead.Length; i++)
                                {
                                    deleteFile(filesToRead[i]);
                                }

                                for (int i = 0; i < backupLocalizationPath.Length; i++)
                                {
                                    deleteFile(backupLocalizationPath[i]);
                                }

                                Directory.Delete(backupPath + @"levels\newlevels\localization\en-us");
                                Directory.Delete(backupPath + @"levels\newlevels\localization");
                                Directory.Delete(backupPath + @"levels\newlevels");
                                Directory.Delete(backupPath + @"levels");

                                for (int i = 0; i < alreadyTranslatedFiles.Length; i++)
                                {
                                    File.Move(alreadyTranslatedFiles[i], localizationPath + alreadyTranslatedFiles[i].Replace(alreadyTranslatedFiles + @"\", ""));
                                }
                            }

                            MessageBox.Show(
                             "Новые строки не найдены.",
                             "Проверка новых строк",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                            updateOrReinstall = false;
                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                            updateReinstall.Enabled = true;
                            //recoverBD.Enabled = true;
                            deleteTranslate.Enabled = true;
                            btnStart.Enabled = true;
                            UpdateButton.Enabled = true;
                            choosePathBtn.Enabled = true;
                            txtPath.Enabled = true;
                            yandexButton.Enabled = true;
                            if (machineName == myMachineName)
                                deeplButton.Enabled = true;

                            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Русификация " + gameName + " отменена.", Color.Black)));

                            return;
                        }
                    }
                    else
                    {
                        if (rowsInBackupFile > rowsInFile)
                        {
                            MessageBox.Show(
                             "Произошла непредвиденная ошибка, попробуйте еще раз или свяжитесь со мной!",
                             "Непредвиденная ошибка",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Error,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                            if (gameName == "DungeonCrawler")
                            {
                                deleteFile(dataPath + @"\export_locres.bat");
                                deleteFile(dataPath + @"\quickbms_4gb_files.exe");
                                deleteFile(localizationPath + @"export_txt_from_locres.bat");
                                deleteFile(localizationPath + @"Game.locres");
                                deleteFile(localizationPath + @"Game.locres.txt");
                                deleteFile(localizationPath + @"UE4localizationsTool.exe");

                                //localizationPath = localizationPath.Replace(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en\", dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game");
                                //deleteFile(localizationPath + @"\en\Game.locres");
                                //delFolder(localizationPath + @"\en");
                                //delFolder(localizationPath);
                                //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game");
                                //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization");
                                //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content");
                                //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName);
                                //delFolder(dataPath + @"\pakchunk0-Windows_0_P");
                                // переделать
                                string backupLocalizationPath = backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en\";

                                deleteFile(backupPath + @"\export_locres.bat");
                                deleteFile(backupPath + @"\quickbms_4gb_files.exe");
                                deleteFile(backupLocalizationPath + @"export_txt_from_locres.bat");
                                deleteFile(backupLocalizationPath + @"Game.locres");
                                deleteFile(backupLocalizationPath + @"Game.locres.txt");
                                deleteFile(backupLocalizationPath + @"UE4localizationsTool.exe");

                                //backupLocalizationPath = backupLocalizationPath.Replace(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game\en\", backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game");
                                //deleteFile(backupLocalizationPath + @"\en\Game.locres");
                                //delFolder(backupLocalizationPath + @"\en");
                                //delFolder(backupLocalizationPath);
                                //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game");
                                //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization");
                                //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content");
                                //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName);
                                //delFolder(backupPath + @"\" + gameName + @"\backup");
                            }
                            else
                            {
                                if (File.Exists(dataPath + @"\UPacker.exe"))
                                    deleteFile(dataPath + @"\UPacker.exe");
                                if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                                    deleteFile(dataPath + @"\UPacker_Advanced.exe");
                                if (File.Exists(localizationPath + @"English.txt"))
                                    deleteFile(localizationPath + @"English.txt");
                                if (File.Exists(localizationPath + @"EnglishEditor.txt"))
                                    deleteFile(localizationPath + @"EnglishEditor.txt");
                                if (File.Exists(localizationPath + @"I2Languages.csv"))
                                    deleteFile(localizationPath + @"I2Languages.csv");
                                if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                                    deleteFile(localizationPath + @"Parser_ULS.exe");
                                if (File.Exists(localizationPath + @"export_uls.bat"))
                                    deleteFile(localizationPath + @"export_uls.bat");
                                if (File.Exists(localizationPath + @"import_uls.bat"))
                                    deleteFile(localizationPath + @"import_uls.bat");

                                if (gameName == "The Mortuary Assistant")
                                {
                                    string[] backupFilesToRead = Directory.GetFiles(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                                    for (int i = 0; i < filesToRead.Length; i++)
                                    {
                                        deleteFile(filesToRead[i]);
                                    }
                                    for (int i = 0; i < backupFilesToRead.Length; i++)
                                    {
                                        deleteFile(backupFilesToRead[i]);
                                    }

                                    deleteFile(localizationPath + @"Global_English.txt");
                                }
                                delFolder(localizationPath);
                                if (gameName == "Temtem")
                                    delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                                else if (gameName == "CoreKeeper")
                                    delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\I2");
                                if (gameName == "Temtem" || gameName == "CoreKeeper")
                                    delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                                delFolder(dataPath + @"\Unity_Assets_Files\resources");
                                delFolder(dataPath + @"\Unity_Assets_Files");

                                string backupLocalizationPath = "";
                                if (gameName == "Temtem")
                                    backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp";
                                else if (gameName == "CoreKeeper")
                                    backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\I2";
                                else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                                    backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";

                                if (gameName == "Temtem" || gameName == "CoreKeeper")
                                {
                                    deleteFile(backupLocalizationPath + @"\I2.Loc\I2Languages.csv");
                                    delFolder(backupLocalizationPath + @"\I2.Loc");
                                    delFolder(backupLocalizationPath);
                                    delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono");
                                }
                                else if (gameName == "Escape Simulator") // для Mortuary удалил выше
                                    deleteFile(backupLocalizationPath + @"\English.txt");
                                delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                                delFolder(backupPath + @"\Unity_Assets_Files");
                            }

                            updateReinstall.Enabled = true;
                            //recoverBD.Enabled = true;
                            deleteTranslate.Enabled = true;
                            btnStart.Enabled = true;
                            UpdateButton.Enabled = true;
                            choosePathBtn.Enabled = true;
                            txtPath.Enabled = true;
                            yandexButton.Enabled = true;
                            if (machineName == myMachineName)
                                deeplButton.Enabled = true;

                            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Произошла непредвиденная ошибка!", Color.Black)));

                            return;
                        }
                        else if (rowsInBackupFile < rowsInFile)
                        {
                            int newRowsDifference = rowsInFile - rowsInBackupFile;
                            if (File.Exists(localizationPath + @"backup.txt"))
                                updateBreak = true;

                            string message;
                            if (updateBreak)
                                message = "Похоже обновление было прервано... Продолжить обновление?\nСчитывание файлов локализации начнется заново, но уже переведенные строки будут пропущены.";
                            else
                                message = $"С последнего обновления обнаружено {newRowsDifference} новых строк!\nПеревести их и обновить перевод?";
                            DialogResult newRowsResult = MessageBox.Show(
                                   message,
                                   "Обновление",
                                   MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1,
                                   MessageBoxOptions.DefaultDesktopOnly);
                            if (newRowsResult == DialogResult.Cancel)
                            {
                                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                                if (gameName == "DungeonCrawler")
                                {
                                    deleteFile(dataPath + @"\export_locres.bat");
                                    deleteFile(dataPath + @"\quickbms_4gb_files.exe");
                                    deleteFile(localizationPath + @"export_txt_from_locres.bat");
                                    deleteFile(localizationPath + @"Game.locres");
                                    deleteFile(localizationPath + @"Game.locres.txt");
                                    deleteFile(localizationPath + @"UE4localizationsTool.exe");

                                    //localizationPath = localizationPath.Replace(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game\en\", dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game");
                                    //deleteFile(localizationPath + @"\en\Game.locres");
                                    //delFolder(localizationPath + @"\en");
                                    //delFolder(localizationPath);
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization\Game");
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content\Localization");
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName + @"\Content");
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P\" + gameName);
                                    //delFolder(dataPath + @"\pakchunk0-Windows_0_P");
                                    // переделать
                                    string backupLocalizationPath = backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en\";

                                    deleteFile(backupPath + @"\export_locres.bat");
                                    deleteFile(backupPath + @"\quickbms_4gb_files.exe");
                                    deleteFile(backupLocalizationPath + @"export_txt_from_locres.bat");
                                    deleteFile(backupLocalizationPath + @"Game.locres");
                                    deleteFile(backupLocalizationPath + @"Game.locres.txt");
                                    deleteFile(backupLocalizationPath + @"UE4localizationsTool.exe");

                                    //backupLocalizationPath = backupLocalizationPath.Replace(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game\en\", backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game");
                                    //deleteFile(backupLocalizationPath + @"\en\Game.locres");
                                    //delFolder(backupLocalizationPath + @"\en");
                                    //delFolder(backupLocalizationPath);
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game");
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization");
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content");
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName);
                                    //delFolder(backupPath + @"\" + gameName + @"_backup\");
                                }
                                else
                                {
                                    if (gameName != "CoreKeeper" && gameName != "NewWorld")
                                    {
                                        if (File.Exists(localizationPath + @"backup.txt"))
                                            deleteFile(localizationPath + @"backup.txt");
                                        if (File.Exists(dataPath + @"\UPacker.exe"))
                                            deleteFile(dataPath + @"\UPacker.exe");
                                        if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                                            deleteFile(dataPath + @"\UPacker_Advanced.exe");
                                        if (File.Exists(localizationPath + @"English.txt"))
                                            deleteFile(localizationPath + @"English.txt");
                                        if (File.Exists(localizationPath + @"I2Languages.csv"))
                                            deleteFile(localizationPath + @"I2Languages.csv");
                                        if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                                            deleteFile(localizationPath + @"Parser_ULS.exe");
                                        if (File.Exists(localizationPath + @"export_uls.bat"))
                                            deleteFile(localizationPath + @"export_uls.bat");
                                        if (File.Exists(localizationPath + @"import_uls.bat"))
                                            deleteFile(localizationPath + @"import_uls.bat");
                                        if (gameName == "The Mortuary Assistant")
                                        {
                                            string[] backupFilesToRead = Directory.GetFiles(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                                            for (int i = 0; i < filesToRead.Length; i++)
                                            {
                                                deleteFile(filesToRead[i]);
                                            }
                                            for (int i = 0; i < backupFilesToRead.Length; i++)
                                            {
                                                deleteFile(backupFilesToRead[i]);
                                            }

                                            deleteFile(localizationPath + @"Global_English.txt");
                                        }
                                        delFolder(localizationPath);
                                        if (gameName == "Temtem")
                                            delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                                        else if (gameName == "CoreKeeper")
                                            delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\I2");
                                        if (gameName == "Temtem" || gameName == "CoreKeeper")
                                            delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                                        delFolder(dataPath + @"\Unity_Assets_Files\resources");
                                        delFolder(dataPath + @"\Unity_Assets_Files");

                                        string backupLocalizationPath = "";
                                        if (gameName == "Temtem")
                                            backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp";
                                        else if (gameName == "CoreKeeper")
                                            backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\I2";
                                        else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                                            backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";

                                        if (gameName == "Temtem" || gameName == "CoreKeeper")
                                        {
                                            deleteFile(backupLocalizationPath + @"\I2.Loc\I2Languages.csv");
                                            delFolder(backupLocalizationPath + @"\I2.Loc");
                                            delFolder(backupLocalizationPath);
                                            delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono");
                                        }
                                        else if (gameName == "Escape Simulator") // для Mortuary удалил выше
                                            deleteFile(backupLocalizationPath + @"\English.txt");
                                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                                        delFolder(backupPath + @"\Unity_Assets_Files");
                                    }
                                }

                                if (gameName == "NewWorld")
                                {
                                    if (File.Exists(localizationPath + @"backup.txt"))
                                        deleteFile(localizationPath + @"backup.txt");
                                    if (File.Exists(dataPath + @"\quickmbs.exe"))
                                        deleteFile(dataPath + @"\quickmbs.exe");
                                    if (File.Exists(localizationPath + @"Global_English.txt"))
                                        deleteFile(localizationPath + @"Global_English.txt");
                                    if (File.Exists(dataPath + @"\xml_q_export.bat"))
                                        deleteFile(dataPath + @"\xml_q_export.bat");
                                    if (File.Exists(dataPath + @"\nw_oodle.bms"))
                                        deleteFile(dataPath + @"\nw_oodle.bms");

                                    string backupLocalizationPath = backupPath + @"levels\newlevels\localization\en-us";

                                    string[] backupFilesToRead = Directory.GetFiles(backupLocalizationPath);
                                    for (int i = 0; i < backupFilesToRead.Length; i++)
                                    {
                                        deleteFile(backupFilesToRead[i]);
                                    }

                                    delFolder(backupLocalizationPath);
                                    delFolder(backupPath + @"levels\newlevels\localization");
                                    delFolder(backupPath + @"levels\newlevels");
                                    delFolder(backupPath + @"levels");
                                }

                                updateReinstall.Enabled = true;
                                //recoverBD.Enabled = true;
                                deleteTranslate.Enabled = true;
                                btnStart.Enabled = true;
                                UpdateButton.Enabled = true;
                                choosePathBtn.Enabled = true;
                                txtPath.Enabled = true;
                                updateBreak = false;
                                yandexButton.Enabled = true;
                                if (machineName == myMachineName)
                                    deeplButton.Enabled = true;

                                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Русификация " + gameName + " отменена.", Color.Black)));

                                return;
                            }
                            else if (newRowsResult == DialogResult.OK)
                            {
                                temtemCheckTranslate.Enabled = false;
                                skillsNameCheckTranslate.Enabled = false;
                                itemsCheckTranslate.Enabled = false;
                                skillsDescriptionCheckTranslate.Enabled = false;
                                npcCheckTranslate.Enabled = false;

                                yandexButton.Enabled = false;
                                deeplButton.Enabled = false;

                                translateReinstall = false;
                                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));
                                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + newRowsDifference + 27));

                                translateUpdate = true;

                                newLinesNumber = newLinesNumber + newRowsDifference;

                                if (gameName == "NewWorld")
                                {
                                    string[] alreadyTranslatedFiles = Directory.GetFiles(dataPath + @"\levels\newlevels\localization");
                                    for (int i = 0; i < alreadyTranslatedFiles.Length; i++)
                                    {
                                        deleteFile(alreadyTranslatedFiles[i]);
                                    }
                                }

                                //if (updateBreak)
                                //    WorkAsync();
                                //else
                                //{
                                //    if (gameName != "CoreKeeper")
                                //        replaceNewRows();
                                //    else
                                //        WorkAsync();
                                //}

                                if (updateBreak)
                                    WorkAsync();
                                else
                                    replaceNewRowsAsync();
                            }
                        }
                    }
                }
            }
        }

        public void IdCheckForCompletedTranslate()
        {
            if (!skillsNameCheckTranslate.Checked && !skillsDescriptionCheckTranslate.Checked && !itemsCheckTranslate.Checked && !npcCheckTranslate.Checked && !temtemCheckTranslate.Checked && !itemsDesc.Checked)
                return;

            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Применение настроек, это займет несколько минут...", Color.Black)));

            int linePosition = 0;

            if (gameName == "Temtem")
            {
                string[] translatedLines = File.ReadAllLines(localizationPath + @"I2Languages.csv");
                string backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp\I2.Loc";

                using (StreamReader reader = new StreamReader(backupLocalizationPath + @"\I2Languages.csv", encoding: Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string idLine = reader.ReadLine();

                        if (hasRuLetters(translatedLines[linePosition]))
                        {
                            string[] idValues = idLine.Split(SeparatorDifferentGames());

                            if (idValues[0].Contains("Techniques/Tech_") || idValues[0].Contains("Passives/"))
                            {
                                if (skillsNameCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[4], idValues[4]);

                                    ReplaceStringInFile(localizationPath + @"I2Languages.csv", linePosition, translatedLine);
                                }
                            }

                            if (idValues[0].Contains("TechniquesDesc/Tech_") || idValues[0].Contains("PassivesDesc/"))
                            {
                                if (skillsDescriptionCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[4], idValues[4]);

                                    ReplaceStringInFile(localizationPath + @"I2Languages.csv", linePosition, translatedLine);
                                }
                            }

                            if (idValues[0].Contains("Inventory") && !idValues[0].Contains("UI/Inventory") && !idValues[0].Contains("UI/HousingEditInventoryFull") && !idValues[0].Contains("UI/ClubsActivityLogInventory"))
                            {
                                if (itemsCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[4], idValues[4]);

                                    ReplaceStringInFile(localizationPath + @"I2Languages.csv", linePosition, translatedLine);
                                }
                            }

                            if (idValues[0].Contains("DialoguesNames/"))
                            {
                                if (npcCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[4], idValues[4]);

                                    ReplaceStringInFile(localizationPath + @"I2Languages.csv", linePosition, translatedLine);
                                }
                            }

                            //if (idValues[0].Contains("Monsters/") || idValues[0].Contains("MonstersDesc/"))
                            if (idValues[0].Contains("Monsters/"))
                            {
                                if (temtemCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[4], idValues[4]);

                                    ReplaceStringInFile(localizationPath + @"I2Languages.csv", linePosition, translatedLine);
                                }
                            }
                        }

                        linePosition = linePosition + 1;

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
                    }
                }
            }
            else
            {
                string[] translatedLines = File.ReadAllLines(localizationPath + @"Game_translated.locres.txt");
                //string backupLocalizationPath = backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game\en";

                //using (StreamReader reader = new StreamReader(backupLocalizationPath + @"\Game.locres.txt", encoding: Encoding.UTF8))
                using (StreamReader reader = new StreamReader(localizationPath + @"\Game.locres.txt", encoding: Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string idLine = reader.ReadLine();

                        if (hasRuLetters(translatedLines[linePosition]))
                        {
                            string[] idValues = idLine.Split(SeparatorDifferentGames());

                            if (idValues[0].Contains("Text_DesignData_Perk_Perk") || idValues[0].Contains("_Spell_Fireball") || idValues[0].Contains("Text_DesignData_Spell_Spell") || idValues[0].Contains("_Property_Perk_") || idValues[0].Contains("Text_DesignData_Skill_Skill"))
                            {
                                if (skillsNameCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[1], idValues[1]);

                                    ReplaceStringInFile(localizationPath + @"Game_translated.locres.txt", linePosition, translatedLine);
                                }
                            }

                            //if (idValues[0].Contains("TechniquesDesc/Tech_") || idValues[0].Contains("PassivesDesc/"))
                            //{
                            //    if (skillsDescriptionCheckTranslate.Checked)
                            //    {
                            //        string translatedLine = translatedLines[linePosition];
                            //        string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());
                            //
                            //        translatedLine = translatedLine.Replace(translatedValues[1], idValues[1]);
                            //
                            //        ReplaceStringInFile(localizationPath + @"Game.locres.txt", linePosition, translatedLine);
                            //    }
                            //}

                            if (idValues[0].Contains("_Item_Item_"))
                            {
                                if (itemsCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[1], idValues[1]);

                                    ReplaceStringInFile(localizationPath + @"Game_translated.locres.txt", linePosition, translatedLine);
                                }
                            }

                            //if (idValues[0].Contains("Inventory") && !idValues[0].Contains("UI/Inventory") && !idValues[0].Contains("UI/HousingEditInventoryFull") && !idValues[0].Contains("UI/ClubsActivityLogInventory"))
                            //{
                            //    if (itemsDesc.Checked)
                            //    {
                            //        string translatedLine = translatedLines[linePosition];
                            //        string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());
                            //
                            //        translatedLine = translatedLine.Replace(translatedValues[1], idValues[1]);
                            //
                            //        ReplaceStringInFile(localizationPath + @"Game.locres.txt", linePosition, translatedLine);
                            //    }
                            //}

                            if (idValues[0].Contains("Text_DesignData_Merchant_Merchant") || idValues[0].Contains("_Monster_Monster_"))
                            {
                                if (npcCheckTranslate.Checked)
                                {
                                    string translatedLine = translatedLines[linePosition];
                                    string[] translatedValues = translatedLine.Split(SeparatorDifferentGames());

                                    translatedLine = translatedLine.Replace(translatedValues[1], idValues[1]);

                                    ReplaceStringInFile(localizationPath + @"Game_translated.locres.txt", linePosition, translatedLine);
                                }
                            }
                        }

                        linePosition = linePosition + 1;

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
                    }
                }
            }
        }

        void recoverBD_Click(object sender, EventArgs e)
        {
            updateReinstall.Enabled = false;
            recoverBD.Enabled = false;
            deleteTranslate.Enabled = false;
            btnStart.Enabled = false;
            UpdateButton.Enabled = false;
            choosePathBtn.Enabled = false;
            txtPath.Enabled = false;
            //fontSetup.Enabled = false;

            DialogResult newRowsResult = MessageBox.Show(
                               "Восстановить предыдущую версию Базы Данных?",
                               "Обновление",
                               MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button1,
                               MessageBoxOptions.DefaultDesktopOnly);
            if (newRowsResult == DialogResult.Cancel)
            {
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "0%"));

                updateReinstall.Enabled = true;
                //recoverBD.Enabled = true;
                deleteTranslate.Enabled = true;
                btnStart.Enabled = true;
                UpdateButton.Enabled = true;
                choosePathBtn.Enabled = true;
                //fontSetup.Enabled = true;

                return;
            }
            else if (newRowsResult == DialogResult.OK)
            {
                if (File.Exists(Application.StartupPath + @"db\db_backup.db3"))
                {
                    deleteFile(Application.StartupPath + @"db\db.db3");
                    System.Threading.Thread.Sleep(500);
                    File.Copy(Application.StartupPath + @"db\db_backup.db3", Application.StartupPath + @"db\db.db3");

                    MessageBox.Show(
                         "Восстановление завершено.",
                         "Воостановление БД",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                    updateReinstall.Enabled = true;
                    //recoverBD.Enabled = true;
                    deleteTranslate.Enabled = true;
                    btnStart.Enabled = true;
                    UpdateButton.Enabled = true;
                    choosePathBtn.Enabled = true;
                }
                else
                {
                    MessageBox.Show(
                         "Восстановление невозможно.",
                         "Воостановление БД",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                    updateReinstall.Enabled = true;
                    //recoverBD.Enabled = true;
                    deleteTranslate.Enabled = true;
                    btnStart.Enabled = true;
                    UpdateButton.Enabled = true;
                    choosePathBtn.Enabled = true;

                    return;
                }
            }
        }

        void il2cppFontChange()
        {
            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Во время русификации будет запущена игра. Не выключай ее, она выключится сама!", Color.Red)));

            Downloading_Files("https://drive.google.com/uc?export=download&id=1bJPLDBVdDRjUtGzv357jMI0sqXX44GXn&confirm=t&uuid=d4e21369-10c5-4373-b8b4-944fa67bf364", path + @"\il2cpp_font.zip");

            ZipFile.ExtractToDirectory(path + @"\il2cpp_font.zip", path);

            deleteFile(path + @"\il2cpp_font.zip");

            System.Threading.Thread.Sleep(500);

            if (path.Contains("Steam"))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "steam://rungameid/1295920", //CoreKeeper 1621690
                    UseShellExecute = true
                });
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path + @"\" + gameName + ".exe",
                    WorkingDirectory = Path.GetDirectoryName(path + @"\" + gameName + ".exe"),
                    Verb = "runas",
                    CreateNoWindow = true
                });
            }

            System.Threading.Thread.Sleep(60000);

            //while(Process.GetProcessesByName("CoreKeeper").Length > 0)
            //{
            //    System.Threading.Thread.Sleep(500);
            //}

            cmdKill("\"" + gameName + ".exe\"");
            System.Threading.Thread.Sleep(500);

            string configPath = path + @"\BepInEx\config";

            ReplaceStringInFile(configPath + @"\AutoTranslatorConfig.ini", 1, "Endpoint=");
            System.Threading.Thread.Sleep(500);
            ReplaceStringInFile(configPath + @"\AutoTranslatorConfig.ini", 25, "IgnoreWhitespaceInDialogue=False");
            System.Threading.Thread.Sleep(500);
            ReplaceStringInFile(configPath + @"\AutoTranslatorConfig.ini", 37, "FallbackFontTextMeshPro=utranslator_sdf");
            System.Threading.Thread.Sleep(500);
            ReplaceStringInFile(configPath + @"\AutoTranslatorConfig.ini", 39, "ForceUIResizing=True");
        }

        void AppendLine(RichTextBox source, string value, Color color)
        {
            source.SelectionColor = color;

            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }

        void ReplaceStringInFile(string filename, int position, string str)
        {
            // 1. Проверка наличия файла.
            if (!File.Exists(filename))
                return;

            // 2. Получить данные из файла в виде массива строк.
            string[] arrayS = File.ReadAllLines(filename);

            // 3. Проверка, коректно ли значение позиции строки
            if ((position < 0) || (position > arrayS.Length))
                return;

            // 4. Заменить строку
            arrayS[position] = str;

            // 5. Записать массив строк в файл
            File.WriteAllLines(filename, arrayS);
        }

        void cmdKill(string processName)
        {
            string processNameLine = "taskkill /f /im " + processName;
            Process.Start(new ProcessStartInfo { FileName = "cmd", Arguments = $"/c {processNameLine}", CreateNoWindow = true, Verb = "runas" }).WaitForExit();
        }

        void powershellKill(string processName)
        {
            string processNameLine = "stop-pocess -name " + processName + " -Force";
            Process.Start(new ProcessStartInfo { FileName = "powershell", Arguments = $"/c {processNameLine}", CreateNoWindow = true, Verb = "runas" }).WaitForExit();
        }

        void delFolder(string DelFileFolder)
        {
            //Directory.CreateDirectory(DelFileFolder);
            //string[] files = Directory.GetFileSystemEntries(DelFileFolder);

            if (!Directory.Exists(DelFileFolder))
                return;

            DirectoryInfo di = new DirectoryInfo(DelFileFolder);

            foreach (FileInfo file in di.EnumerateFiles())
            {
                if (file.Exists)
                    file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                if (dir.Exists)
                    dir.Delete(true);
            }

            if (Directory.Exists(DelFileFolder))
                Directory.Delete(DelFileFolder);
        }

        void deleteFile(string delFile)
        {
            if (!File.Exists(delFile))
                return;

            try
            {
                File.Delete(delFile);
            }
            catch
            {
                try
                {
                    FileInfo delFileInfo = new FileInfo(delFile);
                    if (delFileInfo.IsReadOnly)
                    {
                        File.SetAttributes(delFile, ~FileAttributes.ReadOnly);
                        File.Delete(delFile);
                    }
                    else
                    {
                        File.Delete(delFile);
                    }
                }
                catch
                {
                    MessageBox.Show(
                            @"Зайди в свойства папки с игрой, а так же в свойства папки с русификатором, и сними галочку с ""Только чтение"", после чего попробуй еще раз.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }

        void unArchivBackup()
        {
            string backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp\I2.Loc";
            if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";
            else if (gameName == "NewWorld")
                backupLocalizationPath = backupPath + @"levels\newlevels\localization\en-us";
            else if (gameName == "DungeonCrawler")
                backupLocalizationPath = backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en\";

            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Разархивирование резервной копии...", Color.Black)));

            if (gameName == "Temtem")
            {
                if (File.Exists(backupLocalizationPath + @"\I2Languages.csv"))
                    return;

                if (File.Exists(backupPath + gameName + "_backup"))
                {
                    File.Copy(backupPath + gameName + "_backup", backupPath + gameName + "_backup.assets");
                    System.Threading.Thread.Sleep(500);
                    deleteFile(backupPath + gameName + "_backup");
                }

                File.Copy(backupPath + gameName + "_global_backup", backupPath + "globalgamemanagers.assets");
                System.Threading.Thread.Sleep(500);
                deleteFile(backupPath + gameName + "_global_backup");

                if (gameName == "Temtem")
                {
                    if (!File.Exists(backupPath + "UPacker.exe"))
                        File.WriteAllBytesAsync(backupPath + "UPacker.exe", Resources.UPacker_exe);
                }
                else
                {
                    if (!File.Exists(backupPath + "UPacker_Advanced.exe"))
                        File.WriteAllBytesAsync(backupPath + "UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                }

                File.WriteAllTextAsync(backupPath + "Export_l2.bat", Resources.Export_l2_bat);
                System.Threading.Thread.Sleep(500);

                if (gameName == "Temtem")
                    ReplaceStringInFile(backupPath + "Export_l2.bat", 1, @$"for %%a in ({gameName}_backup.assets;) do UPacker.exe export ""%%a"" -mb_new -t LanguageSourceAsset");
                else
                    ReplaceStringInFile(backupPath + "Export_l2.bat", 1, @$"for %%a in (*.assets;) do UPacker_Advanced.exe export ""%%a"" -mb_new -t LanguageSourceAsset");

                Process.Start(new ProcessStartInfo
                {
                    FileName = backupPath + "Export_l2.bat",
                    WorkingDirectory = Path.GetDirectoryName(backupPath + "Export_l2.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
                System.Threading.Thread.Sleep(500);

                File.WriteAllBytesAsync(backupLocalizationPath + @"\Parser_ULS.exe", Resources.Parser_ULS_exe);
                File.WriteAllTextAsync(backupLocalizationPath + @"\export_uls.bat", Resources.export_uls_bat);
                System.Threading.Thread.Sleep(500);
                ReplaceStringInFile(backupLocalizationPath + @"\export_uls.bat", 1, @"for %%a in (*.LanguageSource;*.LanguageSourceAsset;) do Parser_ULS.exe -e ""%%a"" -o $0c -dauto");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = backupLocalizationPath + @"\export_uls.bat",
                    WorkingDirectory = Path.GetDirectoryName(backupLocalizationPath + @"\export_uls.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                deleteFile(backupPath + "Export_l2.bat");
                if (File.Exists(backupPath + "UPacker.exe"))
                    deleteFile(backupPath + "UPacker.exe");
                if (File.Exists(backupPath + "UPacker_Advanced.exe"))
                    deleteFile(backupPath + "UPacker_Advanced.exe");
                deleteFile(backupLocalizationPath + @"\I2Languages.LanguageSourceAsset");
                deleteFile(backupLocalizationPath + @"\export_uls.bat");
                deleteFile(backupLocalizationPath + @"\Parser_ULS.exe");

                File.Copy(backupPath + gameName + "_backup.assets", backupPath + gameName + "_backup");
                System.Threading.Thread.Sleep(500);
                deleteFile(backupPath + gameName + "_backup.assets");
                File.Copy(backupPath + "globalgamemanagers.assets", backupPath + gameName + "_global_backup");
                System.Threading.Thread.Sleep(500);
                deleteFile(backupPath + "globalgamemanagers.assets");
            }
            else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
            {
                if (gameName == "Escape Simulator" && File.Exists(backupLocalizationPath + @"\English.txt"))
                    return;
                else if (gameName == "The Mortuary Assistant" && File.Exists(backupLocalizationPath + @"\Global_English.txt"))
                    return;

                if (!File.Exists(backupLocalizationPath + @"\PatreonNamesFemale.txt"))
                {

                    if (File.Exists(backupPath + gameName + "_backup"))
                    {
                        File.Copy(backupPath + gameName + "_backup", backupPath + gameName + "_backup.assets");
                        deleteFile(backupPath + gameName + "_backup");
                    }

                    File.WriteAllBytesAsync(backupPath + @"\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                    File.WriteAllTextAsync(backupPath + @"\Export_txt.bat", Resources.Export_txt_bat);
                    System.Threading.Thread.Sleep(500);
                    if (gameName == "Escape Simulator")
                        ReplaceStringInFile(backupPath + @"\Export_txt.bat", 2, @$"for %%a in (*.assets;) do UPacker_Advanced.exe export ""%%a"" -t *English*.txt");
                    else
                        ReplaceStringInFile(backupPath + @"\Export_txt.bat", 2, @$"for %%a in (*.assets;) do UPacker_Advanced.exe export ""%%a"" -t txt");
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = backupPath + @"\Export_txt.bat",
                        WorkingDirectory = Path.GetDirectoryName(backupPath + @"\Export_txt.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(backupPath + @"\Export_txt.bat");
                    deleteFile(backupPath + @"\UPacker_Advanced.exe");
                }

                if (gameName == "The Mortuary Assistant")
                {
                    deleteFile(backupLocalizationPath + @"\PatreonNamesFemale.txt");
                    deleteFile(backupLocalizationPath + @"\PatreonNamesMale.txt");
                    deleteFile(backupLocalizationPath + @"\femaleFirstNames.txt");
                    deleteFile(backupLocalizationPath + @"\lastNames.txt");
                    deleteFile(backupLocalizationPath + @"\maleFirstNames.txt");
                    deleteFile(backupLocalizationPath + @"\LineBreaking Following Characters.txt");
                    deleteFile(backupLocalizationPath + @"\LineBreaking Leading Characters.txt");
                    deleteFile(backupLocalizationPath + @"\BasementTapeFragment.txt");
                    deleteFile(backupLocalizationPath + @"\LanguageCharacters.txt");
                    deleteFile(backupLocalizationPath + @"\BasementTapeWhole.txt");
                    deleteFile(backupLocalizationPath + @"\BibleScrap_00001.txt");
                    deleteFile(backupLocalizationPath + @"\callingThePolice.txt");
                    deleteFile(backupLocalizationPath + @"\CarStart.txt");
                    deleteFile(backupLocalizationPath + @"\caveDificult.txt");
                    deleteFile(backupLocalizationPath + @"\ccApartmentCall.txt");
                    deleteFile(backupLocalizationPath + @"\ccDemonGenLines.txt");
                    deleteFile(backupLocalizationPath + @"\ccGeneric_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ccIntroScene.txt");
                    deleteFile(backupLocalizationPath + @"\ccRaymonGenLines.txt");
                    deleteFile(backupLocalizationPath + @"\ccRebeccaGenLines_00001.txt");
                    deleteFile(backupLocalizationPath + @"\clipboardData.txt");
                    deleteFile(backupLocalizationPath + @"\doYouKnowMe.txt");
                    deleteFile(backupLocalizationPath + @"\EndingFather_00001.txt");
                    deleteFile(backupLocalizationPath + @"\EndingFirst_00001.txt");
                    deleteFile(backupLocalizationPath + @"\EndingSecret_00001.txt");
                    deleteFile(backupLocalizationPath + @"\EndingStandard.txt");
                    deleteFile(backupLocalizationPath + @"\EndingWrongbody_00001.txt");
                    deleteFile(backupLocalizationPath + @"\evilTape.txt");
                    deleteFile(backupLocalizationPath + @"\familiarVoice_00001.txt");
                    deleteFile(backupLocalizationPath + @"\father911Call_00001.txt");
                    deleteFile(backupLocalizationPath + @"\FatherFallOnRocks_00001.txt");
                    deleteFile(backupLocalizationPath + @"\FatherFoundRebecca.txt");
                    deleteFile(backupLocalizationPath + @"\GenNotes_00001.txt");
                    deleteFile(backupLocalizationPath + @"\genText.txt");
                    deleteFile(backupLocalizationPath + @"\getStarted.txt");
                    deleteFile(backupLocalizationPath + @"\grandmaEvent2BroughtYouSomethign_00001.txt");
                    deleteFile(backupLocalizationPath + @"\grandmaEvent2Run.txt");
                    deleteFile(backupLocalizationPath + @"\grandmaEvent2SlitWrists_00001.txt");
                    deleteFile(backupLocalizationPath + @"\HateMeAllYouWant_00001.txt");
                    deleteFile(backupLocalizationPath + @"\HauntGhostbodies.txt");
                    deleteFile(backupLocalizationPath + @"\IllGetIt.txt");
                    deleteFile(backupLocalizationPath + @"\introGetBackToWork_00001.txt");
                    deleteFile(backupLocalizationPath + @"\itemInfo_00001.txt");
                    deleteFile(backupLocalizationPath + @"\LeadInCallOne.txt");
                    deleteFile(backupLocalizationPath + @"\LeadInCallTwo_00001.txt");
                    deleteFile(backupLocalizationPath + @"\letMeInLaugh.txt");
                    deleteFile(backupLocalizationPath + @"\Letters_00001.txt");
                    deleteFile(backupLocalizationPath + @"\littleRaymond_00001.txt");
                    deleteFile(backupLocalizationPath + @"\NightShiftText.txt");
                    deleteFile(backupLocalizationPath + @"\NsHistory.txt");
                    deleteFile(backupLocalizationPath + @"\packetToZoe.txt");
                    deleteFile(backupLocalizationPath + @"\PhoneCallFriendBeach.txt");
                    deleteFile(backupLocalizationPath + @"\PhoneCallKillYou_00001.txt");
                    deleteFile(backupLocalizationPath + @"\RaymondJournal.txt");
                    deleteFile(backupLocalizationPath + @"\RecordMarks.txt");
                    deleteFile(backupLocalizationPath + @"\shouldBeoutHere_00001.txt");
                    deleteFile(backupLocalizationPath + @"\TapeOne_00001.txt");
                    deleteFile(backupLocalizationPath + @"\TapeThree.txt");
                    deleteFile(backupLocalizationPath + @"\TapeTwo.txt");
                    deleteFile(backupLocalizationPath + @"\theKnock.txt");
                    deleteFile(backupLocalizationPath + @"\toolateDoorOpen_00001.txt");
                    deleteFile(backupLocalizationPath + @"\TutEnter.txt");
                    deleteFile(backupLocalizationPath + @"\TutorialText_00001.txt");
                    deleteFile(backupLocalizationPath + @"\uiText_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent1_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent2_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent3_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent4.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent5_00001.txt");
                    deleteFile(backupLocalizationPath + @"\whisperPenatent_00001.txt");
                    deleteFile(backupLocalizationPath + @"\YouShouldBe.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00001.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00002.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00003.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00004.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00006.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00007.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00008.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00001.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00001.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00001.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00001.txt");

                    string[] backupFilesToRead = Directory.GetFiles(backupLocalizationPath);

                    for (int i = 0; i < backupFilesToRead.Length; i++)
                    {
                        string checkedFile = backupFilesToRead[i].Replace(backupLocalizationPath, "");

                        if (checkedFile.Contains("_fr") || checkedFile.Contains("_de") || checkedFile.Contains("_es") || checkedFile.Contains("_it") || checkedFile.Contains("_ja")
                            || checkedFile.Contains("_ko") || checkedFile.Contains("_tr") || checkedFile.Contains("_zh_cn"))
                        {
                            deleteFile(backupFilesToRead[i]);
                        }
                    }
                }
                else if (gameName == "Escape Simulator")
                {
                    if (File.Exists(backupLocalizationPath + @"\EnglishEditor.txt"))
                        deleteFile(backupLocalizationPath + @"\EnglishEditor.txt");
                }

                if (File.Exists(backupPath + gameName + "_backup.assets"))
                {
                    File.Copy(backupPath + gameName + "_backup.assets", backupPath + gameName + "_backup");
                    deleteFile(backupPath + gameName + "_backup.assets");
                }
            }
            else if (gameName == "NewWorld")
            {
                if (File.Exists(backupLocalizationPath + @"\javelindata_itemdefinitions_master.loc.xml"))
                    return;

                if (File.Exists(backupPath + gameName + "_backup"))
                {
                    File.Copy(backupPath + gameName + "_backup", backupPath + "DataStrm-part1.pak");
                    deleteFile(backupPath + gameName + "_backup");
                }

                File.WriteAllBytesAsync(backupPath + @"\quickbms.exe", Resources.quickbms_exe);
                File.WriteAllBytesAsync(backupPath + @"\nw_oodle.bms", Resources.nw_oodle_bms);
                File.WriteAllTextAsync(backupPath + @"\xml_q_export.bat", Resources.xml_q_export_bat);
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = backupPath + @"\xml_q_export.bat",
                    WorkingDirectory = Path.GetDirectoryName(backupPath + @"\xml_q_export.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                deleteFile(backupPath + @"\xml_q_export.bat");
                deleteFile(backupPath + @"\nw_oodle.bms");
                deleteFile(backupPath + @"\quickbms.exe");
                deleteFile(backupLocalizationPath + @"\01_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\01_npctopics.loc.xml");
                deleteFile(backupLocalizationPath + @"\02_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\03_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\04_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\04a_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\05_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\06_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\06a_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\06a_npctopics.loc.xml");
                deleteFile(backupLocalizationPath + @"\07_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\08_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\08_npctopics.loc.xml");
                deleteFile(backupLocalizationPath + @"\09_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\10_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\11_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\11_npctopics.loc.xml");
                deleteFile(backupLocalizationPath + @"\12_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\12a_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\12a_npctopics.loc.xml");
                deleteFile(backupLocalizationPath + @"\13_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\14_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\15_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\16_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\16_npctopics.loc.xml");
                deleteFile(backupLocalizationPath + @"\96_cinematics.loc.xml");
                deleteFile(backupLocalizationPath + @"\98_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\98_npctopics.loc.xml");
                deleteFile(backupLocalizationPath + @"\99_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\99a_npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\dungeonregions.loc.xml");
                deleteFile(backupLocalizationPath + @"\easyanticheat.loc.xml");
                deleteFile(backupLocalizationPath + @"\fishbehaviors.loc.xml");
                deleteFile(backupLocalizationPath + @"\guildnames.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_affixdefinitions.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_craftingnames.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_craftingrecipes.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_damagetypes.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_gatherables.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_itemdefinitions_blueprints.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_vitals.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_metaachievements.loc.xml");
                deleteFile(backupLocalizationPath + @"\javelindata_tooltiplayout.loc.xml");
                deleteFile(backupLocalizationPath + @"\legaltext.loc.xml");
                deleteFile(backupLocalizationPath + @"\npcmisc.loc.xml");
                deleteFile(backupLocalizationPath + @"\periodicrewards.loc.xml");
                deleteFile(backupLocalizationPath + @"\territories.loc.xml");
                deleteFile(backupLocalizationPath + @"\tracts.loc.xml");
                deleteFile(backupLocalizationPath + @"\territorystandingstitles.loc.xml");
                deleteFile(backupLocalizationPath + @"\areadefinitions.loc.xml");

                File.Copy(backupPath + "DataStrm-part1.pak", backupPath + gameName + "_backup");
                deleteFile(backupPath + "DataStrm-part1.pak");
            }
            else if (gameName == "DungeonCrawler")
            {
                if (File.Exists(backupLocalizationPath + @"Game.locres.txt"))
                    return;

                if (File.Exists(backupPath + gameName + "_backup"))
                {
                    File.Copy(backupPath + gameName + "_backup", "pakchunk0-Windows_0_P.pak");
                    deleteFile(backupPath + gameName + "_backup");
                }

                if (!File.Exists(backupLocalizationPath + @"Game.locres"))
                {
                    if (!File.Exists(backupPath + @"\quickbms_4gb_files.exe"))
                        File.WriteAllBytesAsync(backupPath + @"\quickbms_4gb_files.exe", Resources.quickbms_4gb_files_exe);

                    if (!File.Exists(backupPath + @"\export_locres.bat"))
                        File.WriteAllTextAsync(backupPath + @"\export_locres.bat", Resources.export_locres_bat);
                    System.Threading.Thread.Sleep(500);

                    Directory.CreateDirectory(backupPath + @"\" + gameName + @"_backup_folder");
                    System.Threading.Thread.Sleep(500);

                    //if (newRowsCheck)
                    //{
                    ReplaceStringInFile(backupPath + @"\export_locres.bat", 0, @"quickbms_4gb_files.exe -f Game.locres 4_0.4.27d.bms pakchunk0-Windows_0_P.pak " + gameName + @"_backup_folder");
                    System.Threading.Thread.Sleep(500);
                    //}

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = backupPath + @"\export_locres.bat",
                        WorkingDirectory = Path.GetDirectoryName(backupPath + @"\export_locres.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                }

                if (!File.Exists(backupLocalizationPath + @"Game.locres"))
                    return;

                if (!File.Exists(backupLocalizationPath + @"Game.locres.txt"))
                {
                    File.WriteAllBytesAsync(backupLocalizationPath + @"UE4localizationsTool.exe", Resources.UE4localizationsTool_exe);
                    File.WriteAllTextAsync(backupLocalizationPath + @"export_txt_from_locres.bat", Resources.export_txt_from_locres_bat);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = backupLocalizationPath + @"export_txt_from_locres.bat",
                        WorkingDirectory = Path.GetDirectoryName(backupLocalizationPath + @"export_txt_from_locres.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(backupLocalizationPath + @"export_txt_from_locres.bat");
                    deleteFile(backupLocalizationPath + @"UE4localizationsTool.exe");

                    File.Copy("pakchunk0-Windows_0_P.pak", backupPath + gameName + "_backup");
                    deleteFile("pakchunk0-Windows_0_P.pak");

                    deleteFile(backupPath + @"\export_locres.bat");
                    deleteFile(backupPath + @"\quickbms_4gb_files.exe");

                    backupLocalizationPath = backupLocalizationPath.Replace(backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en\", backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game");
                    deleteFile(backupLocalizationPath + @"\en\Game.locres");
                    delFolder(backupLocalizationPath + @"\en");
                }

                File.Copy("pakchunk0-Windows_0_P.pak", backupPath + gameName + "_backup");
                deleteFile("pakchunk0-Windows_0_P.pak");
            }
            else if (gameName == "nwr")
            {
                if (File.Exists(backupPath + @"addon_english.txt"))
                    return;

                File.Copy(backupPath + gameName + "_backup", backupPath + @"addon_english.txt");
            }
        }

        void unArchivTranslatedBackup()
        {
            System.Threading.Thread.Sleep(500);

            string backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono\Assembly-CSharp\I2.Loc";
            if (gameName == "CoreKeeper")
                backupLocalizationPath = backupPath + @"Localization.csv";
            else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup";
            else if (gameName == "NewWorld")
                backupLocalizationPath = backupPath + @"levels\newlevels\localization\en-us";
            else if (gameName == "DungeonCrawler")
                backupLocalizationPath = backupPath + @"\" + gameName + @"_translated_backup_folder\" + gameName + @"\Content\Localization\Game\en\";

            if (!File.Exists(backupPath + gameName + "_translated_backup"))
                return;

            if (gameName == "Temtem")
            {
                if (File.Exists(backupLocalizationPath + @"\I2Languages.csv"))
                    return;

                File.Copy(backupPath + gameName + "_translated_backup", backupPath + gameName + "_translated_backup.assets");
                System.Threading.Thread.Sleep(500);
                deleteFile(backupPath + gameName + "_translated_backup");

                File.Copy(backupPath + gameName + "_global_backup", backupPath + "globalgamemanagers.assets");
                deleteFile(backupPath + gameName + "_global_backup");

                if (gameName == "Temtem")
                {
                    if (!File.Exists(backupPath + "UPacker.exe"))
                        File.WriteAllBytesAsync(backupPath + "UPacker.exe", Resources.UPacker_exe);
                }
                else
                {
                    if (!File.Exists(backupPath + "UPacker_Advanced.exe"))
                        File.WriteAllBytesAsync(backupPath + "UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                }

                File.WriteAllTextAsync(backupPath + "Export_l2.bat", Resources.Export_l2_bat);
                System.Threading.Thread.Sleep(500);
                if (gameName == "Temtem")
                    ReplaceStringInFile(backupPath + "Export_l2.bat", 1, @$"for %%a in (*.assets;) do UPacker.exe export ""%%a"" -mb_new -t LanguageSourceAsset");
                else
                    ReplaceStringInFile(backupPath + "Export_l2.bat", 1, @$"for %%a in (*.assets;) do UPacker_Advanced.exe export ""%%a"" -mb_new -t LanguageSourceAsset");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = backupPath + "Export_l2.bat",
                    WorkingDirectory = Path.GetDirectoryName(backupPath + "Export_l2.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
                System.Threading.Thread.Sleep(500);

                File.WriteAllBytesAsync(backupLocalizationPath + @"\Parser_ULS.exe", Resources.Parser_ULS_exe);
                File.WriteAllTextAsync(backupLocalizationPath + @"\export_uls.bat", Resources.export_uls_bat);
                System.Threading.Thread.Sleep(500);
                ReplaceStringInFile(backupLocalizationPath + @"\export_uls.bat", 1, @"for %%a in (*.LanguageSource;*.LanguageSourceAsset;) do Parser_ULS.exe -e ""%%a"" -o $0c -dauto");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = backupLocalizationPath + @"\export_uls.bat",
                    WorkingDirectory = Path.GetDirectoryName(backupLocalizationPath + @"\export_uls.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
                System.Threading.Thread.Sleep(500);

                deleteFile(backupPath + "Export_l2.bat");
                if (File.Exists(backupPath + "UPacker.exe"))
                    deleteFile(backupPath + "UPacker.exe");
                if (File.Exists(backupPath + "UPacker_Advanced.exe"))
                    deleteFile(backupPath + "UPacker_Advanced.exe");
                deleteFile(backupLocalizationPath + @"\I2Languages.LanguageSourceAsset");
                deleteFile(backupLocalizationPath + @"\export_uls.bat");
                deleteFile(backupLocalizationPath + @"\Parser_ULS.exe");

                if (!translateReinstall)
                {
                    File.Copy(backupPath + gameName + "_translated_backup.assets", backupPath + gameName + "_translated_backup");
                    deleteFile(backupPath + gameName + "_translated_backup.assets");
                    File.Copy(backupPath + "globalgamemanagers.assets", backupPath + gameName + "_global_backup");
                    deleteFile(backupPath + "globalgamemanagers.assets");
                }
            }
            else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
            {
                if (gameName == "Escape Simulator" && File.Exists(backupLocalizationPath + @"\English.txt"))
                    return;
                if (gameName == "The Mortuary Assistant" && File.Exists(backupLocalizationPath + @"\Global_English.txt"))
                    return;

                if (!File.Exists(backupLocalizationPath + @"\PatreonNamesFemale.txt"))
                {
                    if (File.Exists(backupPath + gameName + "_translated_backup"))
                    {
                        File.Copy(backupPath + gameName + "_translated_backup", backupPath + gameName + "_translated_backup.assets");
                        System.Threading.Thread.Sleep(500);
                        deleteFile(backupPath + gameName + "_translated_backup");
                    }

                    File.WriteAllBytesAsync(backupPath + @"\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                    File.WriteAllTextAsync(backupPath + @"\Export_txt.bat", Resources.Export_txt_bat);

                    System.Threading.Thread.Sleep(500);
                    if (gameName == "Escape Simulator")
                        ReplaceStringInFile(backupPath + @"\Export_txt.bat", 2, @$"for %%a in (*.assets;) do UPacker_Advanced.exe export ""%%a"" -t *English*.txt");
                    else
                        ReplaceStringInFile(backupPath + @"\Export_txt.bat", 2, @$"for %%a in (*.assets;) do UPacker_Advanced.exe export ""%%a"" -t txt");
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = backupPath + @"\Export_txt.bat",
                        WorkingDirectory = Path.GetDirectoryName(backupPath + @"\Export_txt.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(backupPath + @"\Export_txt.bat");
                    deleteFile(backupPath + @"\UPacker_Advanced.exe");
                }

                if (gameName == "The Mortuary Assistant")
                {
                    deleteFile(backupLocalizationPath + @"\PatreonNamesFemale.txt");
                    deleteFile(backupLocalizationPath + @"\PatreonNamesMale.txt");
                    deleteFile(backupLocalizationPath + @"\femaleFirstNames.txt");
                    deleteFile(backupLocalizationPath + @"\lastNames.txt");
                    deleteFile(backupLocalizationPath + @"\maleFirstNames.txt");
                    deleteFile(backupLocalizationPath + @"\LineBreaking Following Characters.txt");
                    deleteFile(backupLocalizationPath + @"\LineBreaking Leading Characters.txt");
                    deleteFile(backupLocalizationPath + @"\BasementTapeFragment.txt");
                    deleteFile(backupLocalizationPath + @"\LanguageCharacters.txt");
                    deleteFile(backupLocalizationPath + @"\BasementTapeWhole.txt");
                    deleteFile(backupLocalizationPath + @"\BibleScrap_00001.txt");
                    deleteFile(backupLocalizationPath + @"\callingThePolice.txt");
                    deleteFile(backupLocalizationPath + @"\CarStart.txt");
                    deleteFile(backupLocalizationPath + @"\caveDificult.txt");
                    deleteFile(backupLocalizationPath + @"\ccApartmentCall.txt");
                    deleteFile(backupLocalizationPath + @"\ccDemonGenLines.txt");
                    deleteFile(backupLocalizationPath + @"\ccGeneric_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ccIntroScene.txt");
                    deleteFile(backupLocalizationPath + @"\ccRaymonGenLines.txt");
                    deleteFile(backupLocalizationPath + @"\ccRebeccaGenLines_00001.txt");
                    deleteFile(backupLocalizationPath + @"\clipboardData.txt");
                    deleteFile(backupLocalizationPath + @"\doYouKnowMe.txt");
                    deleteFile(backupLocalizationPath + @"\EndingFather_00001.txt");
                    deleteFile(backupLocalizationPath + @"\EndingFirst_00001.txt");
                    deleteFile(backupLocalizationPath + @"\EndingSecret_00001.txt");
                    deleteFile(backupLocalizationPath + @"\EndingStandard.txt");
                    deleteFile(backupLocalizationPath + @"\EndingWrongbody_00001.txt");
                    deleteFile(backupLocalizationPath + @"\evilTape.txt");
                    deleteFile(backupLocalizationPath + @"\familiarVoice_00001.txt");
                    deleteFile(backupLocalizationPath + @"\father911Call_00001.txt");
                    deleteFile(backupLocalizationPath + @"\FatherFallOnRocks_00001.txt");
                    deleteFile(backupLocalizationPath + @"\FatherFoundRebecca.txt");
                    deleteFile(backupLocalizationPath + @"\GenNotes_00001.txt");
                    deleteFile(backupLocalizationPath + @"\genText.txt");
                    deleteFile(backupLocalizationPath + @"\getStarted.txt");
                    deleteFile(backupLocalizationPath + @"\grandmaEvent2BroughtYouSomethign_00001.txt");
                    deleteFile(backupLocalizationPath + @"\grandmaEvent2Run.txt");
                    deleteFile(backupLocalizationPath + @"\grandmaEvent2SlitWrists_00001.txt");
                    deleteFile(backupLocalizationPath + @"\HateMeAllYouWant_00001.txt");
                    deleteFile(backupLocalizationPath + @"\HauntGhostbodies.txt");
                    deleteFile(backupLocalizationPath + @"\IllGetIt.txt");
                    deleteFile(backupLocalizationPath + @"\introGetBackToWork_00001.txt");
                    deleteFile(backupLocalizationPath + @"\itemInfo_00001.txt");
                    deleteFile(backupLocalizationPath + @"\LeadInCallOne.txt");
                    deleteFile(backupLocalizationPath + @"\LeadInCallTwo_00001.txt");
                    deleteFile(backupLocalizationPath + @"\letMeInLaugh.txt");
                    deleteFile(backupLocalizationPath + @"\Letters_00001.txt");
                    deleteFile(backupLocalizationPath + @"\littleRaymond_00001.txt");
                    deleteFile(backupLocalizationPath + @"\NightShiftText.txt");
                    deleteFile(backupLocalizationPath + @"\NsHistory.txt");
                    deleteFile(backupLocalizationPath + @"\packetToZoe.txt");
                    deleteFile(backupLocalizationPath + @"\PhoneCallFriendBeach.txt");
                    deleteFile(backupLocalizationPath + @"\PhoneCallKillYou_00001.txt");
                    deleteFile(backupLocalizationPath + @"\RaymondJournal.txt");
                    deleteFile(backupLocalizationPath + @"\RecordMarks.txt");
                    deleteFile(backupLocalizationPath + @"\shouldBeoutHere_00001.txt");
                    deleteFile(backupLocalizationPath + @"\TapeOne_00001.txt");
                    deleteFile(backupLocalizationPath + @"\TapeThree.txt");
                    deleteFile(backupLocalizationPath + @"\TapeTwo.txt");
                    deleteFile(backupLocalizationPath + @"\theKnock.txt");
                    deleteFile(backupLocalizationPath + @"\toolateDoorOpen_00001.txt");
                    deleteFile(backupLocalizationPath + @"\TutEnter.txt");
                    deleteFile(backupLocalizationPath + @"\TutorialText_00001.txt");
                    deleteFile(backupLocalizationPath + @"\uiText_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent1_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent2_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent3_00001.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent4.txt");
                    deleteFile(backupLocalizationPath + @"\ValEvent5_00001.txt");
                    deleteFile(backupLocalizationPath + @"\whisperPenatent_00001.txt");
                    deleteFile(backupLocalizationPath + @"\YouShouldBe.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00001.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00002.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00003.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00004.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00006.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00007.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00008.txt");
                    deleteFile(backupLocalizationPath + @"itemInfoPatch_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00001.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ccLines_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_ExorcismWIP_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_Notes_00001.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00006.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_UiText_00001.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00004.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00002.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00009.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00005.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00003.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00007.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00008.txt");
                    deleteFile(backupLocalizationPath + @"Patch_VHS_slide_00001.txt");

                    string[] backupFilesToRead = Directory.GetFiles(backupLocalizationPath);

                    for (int i = 0; i < backupFilesToRead.Length; i++)
                    {
                        string checkedFile = backupFilesToRead[i].Replace(backupLocalizationPath, "");

                        if (checkedFile.Contains("_fr") || checkedFile.Contains("_de") || checkedFile.Contains("_es") || checkedFile.Contains("_it") || checkedFile.Contains("_ja")
                            || checkedFile.Contains("_ko") || checkedFile.Contains("_tr") || checkedFile.Contains("_zh_cn"))
                        {
                            deleteFile(backupFilesToRead[i]);
                        }
                    }
                }
                else if (gameName == "Escape Simulator")
                {
                    if (File.Exists(backupLocalizationPath + @"\EnglishEditor.txt"))
                        deleteFile(backupLocalizationPath + @"\EnglishEditor.txt");
                }

                if (!translateReinstall)
                {
                    if (File.Exists(backupPath + gameName + "_translated_backup.assets"))
                    {
                        File.Copy(backupPath + gameName + "_translated_backup.assets", backupPath + gameName + "_translated_backup");
                        deleteFile(backupPath + gameName + "_translated_backup.assets");
                    }
                }
            }
            else if (gameName == "NewWorld")
            {
                if (File.Exists(backupPath + @"\Global_English.txt"))
                    return;

                File.Copy(backupPath + gameName + "_translated_backup", backupPath + @"\Global_English.txt");
                deleteFile(backupPath + gameName + "_translated_backup"); // ?
            }
            else if (gameName == "DungeonCrawler")
            {
                if (File.Exists(backupLocalizationPath + @"Game.locres.txt"))
                    return;

                if (File.Exists(backupPath + gameName + "_translated_backup"))
                {
                    File.Copy(backupPath + gameName + "_translated_backup", "pakchunk0-Windows_0_P.pak");
                    deleteFile(backupPath + gameName + "_translated_backup");
                }

                if (!File.Exists(backupLocalizationPath + @"Game.locres"))
                {
                    if (!File.Exists(backupPath + @"\quickbms_4gb_files.exe"))
                        File.WriteAllBytesAsync(backupPath + @"\quickbms_4gb_files.exe", Resources.quickbms_4gb_files_exe);

                    if (!File.Exists(backupPath + @"\export_locres.bat"))
                        File.WriteAllTextAsync(backupPath + @"\export_locres.bat", Resources.export_locres_bat);
                    System.Threading.Thread.Sleep(500);

                    Directory.CreateDirectory(backupPath + @"\" + gameName + @"_translated_backup_folder");
                    System.Threading.Thread.Sleep(500);

                    //if (newRowsCheck)
                    //{
                    ReplaceStringInFile(backupPath + @"\export_locres.bat", 0, @"quickbms_4gb_files.exe -f Game.locres 4_0.4.27d.bms pakchunk0-Windows_0_P.pak " + gameName + @"_translated_backup_folder");
                    System.Threading.Thread.Sleep(500);
                    //}

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = backupPath + @"\export_locres.bat",
                        WorkingDirectory = Path.GetDirectoryName(backupPath + @"\export_locres.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                }

                if (!File.Exists(backupLocalizationPath + @"Game.locres"))
                    return;

                if (!File.Exists(backupLocalizationPath + @"Game.locres.txt"))
                {
                    if (!File.Exists(backupLocalizationPath + @"UE4localizationsTool.exe"))
                        File.WriteAllBytesAsync(backupLocalizationPath + @"UE4localizationsTool.exe", Resources.UE4localizationsTool_exe);
                    if (!File.Exists(backupLocalizationPath + @"export_txt_from_locres.bat"))
                        File.WriteAllTextAsync(backupLocalizationPath + @"export_txt_from_locres.bat", Resources.export_txt_from_locres_bat);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = backupLocalizationPath + @"export_txt_from_locres.bat",
                        WorkingDirectory = Path.GetDirectoryName(backupLocalizationPath + @"export_txt_from_locres.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(backupLocalizationPath + @"export_txt_from_locres.bat");
                    deleteFile(backupLocalizationPath + @"UE4localizationsTool.exe");

                    File.Copy("pakchunk0-Windows_0_P.pak", backupPath + gameName + "_backup");
                    deleteFile("pakchunk0-Windows_0_P.pak");

                    deleteFile(backupPath + @"\export_locres.bat");
                    deleteFile(backupPath + @"\quickbms_4gb_files.exe");

                    backupLocalizationPath = backupLocalizationPath.Replace(backupPath + @"\" + gameName + @"_translated_backup_folder\" + gameName + @"\Content\Localization\Game\en\", backupPath + @"\" + gameName + @"_translated_backup_folder\" + gameName + @"\Content\Localization\Game");
                    deleteFile(backupLocalizationPath + @"\en\Game.locres");
                    delFolder(backupLocalizationPath + @"\en");
                }

                File.Copy("pakchunk0-Windows_0_P.pak", backupPath + gameName + "_translated_backup");
                deleteFile("pakchunk0-Windows_0_P.pak");
            }
            else if (gameName == "nwr")
            {
                if (File.Exists(backupPath + @"addon_russian.txt"))
                    return;

                File.Copy(backupPath + gameName + "_translated_backup", backupPath + @"addon_russian.txt");
            }
        }

        void reinstallTranslate()
        {
            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Разархивирование резервной копии...", Color.Black)));

            if (File.Exists(backupPath + gameName + "_translated_backup"))
            {
                if (gameName == "DungeonCrawler")
                {
                    MessageBox.Show(
                         "Функция временно недоступна",
                         "Временно недоступно",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                    return;
                }

                unArchivTranslatedBackup();

                string backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono\Assembly-CSharp";
                if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                    backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup";
                else if (gameName == "DungeonCrawler")
                    backupTranslatedLocalizationPath = backupPath + @"\" + gameName + @"_translated_backup_folder\" + gameName + @"\Content\Localization\Game\en"; //

                string backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp";
                if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                    backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";
                else if (gameName == "DungeonCrawler")
                    backupLocalizationPath = backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en";

                if (File.Exists(localizationPath + @"I2Languages.csv"))
                {
                    deleteFile(localizationPath + @"I2Languages.csv");
                    System.Threading.Thread.Sleep(500);
                    File.Copy(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv", localizationPath + @"I2Languages.csv");
                    System.Threading.Thread.Sleep(500);
                    deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv");
                    delFolder(backupTranslatedLocalizationPath + @"\I2.Loc");
                    deleteFile(backupPath + @"Export_l2.bat");
                    deleteFile(backupTranslatedLocalizationPath + @"\Parser_ULS.exe");
                    deleteFile(backupTranslatedLocalizationPath + @"\export_uls.bat");
                    delFolder(backupTranslatedLocalizationPath);
                    delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono");
                    delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup");
                }
                if (File.Exists(localizationPath + @"English.txt"))
                {
                    deleteFile(localizationPath + @"English.txt");
                    System.Threading.Thread.Sleep(500);
                    File.Copy(backupTranslatedLocalizationPath + @"\English.txt", localizationPath + @"English.txt");
                    System.Threading.Thread.Sleep(500);
                    deleteFile(backupTranslatedLocalizationPath + @"\English.txt");
                }
                if (gameName == "The Mortuary Assistant")
                {
                    deleteFile(backupLocalizationPath + @"\Global_English.txt");
                    deleteFile(localizationPath + @"Global_English.txt");

                    string[] backupTranslatedFilesToRead = Directory.GetFiles(backupTranslatedLocalizationPath);

                    for (int i = 0; i < filesToRead.Length; i++)
                    {
                        deleteFile(filesToRead[i]);
                    }

                    for (int i = 0; i < backupTranslatedFilesToRead.Length; i++)
                    {
                        File.Copy(backupTranslatedFilesToRead[i], filesToRead[i]);
                    }

                    for (int i = 0; i < backupTranslatedFilesToRead.Length; i++)
                    {
                        deleteFile(backupTranslatedFilesToRead[i]);
                    }
                }
                else if (gameName == "DungeonCrawler")
                {

                }

                if (File.Exists(backupPath + @"UPacker.exe"))
                    deleteFile(backupPath + @"UPacker.exe");
                if (File.Exists(backupPath + @"UPacker_Advanced.exe"))
                    deleteFile(backupPath + @"UPacker_Advanced.exe");

                if (gameName == "Escape Simulator")
                {
                    deleteFile(backupLocalizationPath + @"\English.txt");
                    delFolder(backupLocalizationPath);
                    delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");

                    deleteFile(backupTranslatedLocalizationPath + @"\English.txt");
                    delFolder(backupTranslatedLocalizationPath);
                    delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup");
                }
                else if (gameName == "The Mortuary Assistant")
                {
                    string[] backupFilesToRead = Directory.GetFiles(backupLocalizationPath);

                    for (int i = 0; i < backupFilesToRead.Length; i++)
                    {
                        deleteFile(backupFilesToRead[i]);
                    }

                    delFolder(backupLocalizationPath);

                    delFolder(backupTranslatedLocalizationPath);
                }

                if (gameName != "DungeonCrawler")
                    delFolder(backupPath + "Unity_Assets_Files");

                deleteFile(backupPath + gameName + "_translated_backup.assets");
                if (gameName == "Temtem")
                {
                    deleteFile(backupPath + "globalgamemanagers.assets");
                }

                CreateBackupTest();

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                ImportAsync();
            }
            /*else
            {
                // установка прервалась
                int rowsInFile = File.ReadAllLines(localizationPath + @"I2Languages.csv").Length;
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + rowsInFile + 6));

                reinstallWithoutBackup = true;
                translateUpdate = false;
                WorkAsync();
            }*/
        }

        async void replaceNewRowsAsync()
        {
            await Task.Run(() => replaceNewRows());
        }

        void replaceNewRows()
        {
            string[] textFromOld = new string[] { };
            string textFromNew = "";
            int lineNumber = 0;
            int numberExist = 0;

            string backupLocalizationPath = "";
            if (gameName == "Temtem")
                backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp";
            else if (gameName == "DungeonCrawler")
                backupLocalizationPath = backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en";
            else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";
            else if (gameName == "CoreKeeper")
                backupLocalizationPath = backupPath + @"Localization.csv";
            else if (gameName == "NewWorld")
                backupLocalizationPath = backupPath + @"levels\newlevels\localization\en-us";

            if (gameName == "Temtem")
            {
                if (File.Exists(backupLocalizationPath + @"\I2.Loc\I2Languages.csv"))
                    textFromOld = File.ReadAllLines(backupLocalizationPath + @"\I2.Loc\I2Languages.csv");
            }
            else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant" || gameName == "NewWorld")
            {
                string[] text = File.ReadAllLines(localizationPath + FilesNameDifferentGames());
                deleteFile(localizationPath + FilesNameDifferentGames());

                using (StreamWriter sw = new StreamWriter(localizationPath + FilesNameDifferentGames(), false, encoding: Encoding.UTF8))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] != "" && text[i] != " ")
                        {
                            if (!text[i].Contains("#") || text[i].Contains("#") && text[i].Contains(":"))
                            {
                                sw.WriteLine(text[i]);
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(500);

                text = File.ReadAllLines(backupLocalizationPath + FilesNameDifferentGames());
                deleteFile(backupLocalizationPath + FilesNameDifferentGames());

                using (StreamWriter sw = new StreamWriter(backupLocalizationPath + FilesNameDifferentGames(), false, encoding: Encoding.UTF8))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] != "" && text[i] != " ")
                        {
                            if (!text[i].Contains("#") || text[i].Contains("#") && text[i].Contains(":"))
                            {
                                sw.WriteLine(text[i]);
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(500);

                if (File.Exists(backupLocalizationPath + FilesNameDifferentGames()))
                    textFromOld = File.ReadAllLines(backupLocalizationPath + FilesNameDifferentGames());
            }
            else if (gameName == "CoreKeeper")
            {
                if (!File.Exists(backupLocalizationPath))
                    File.Copy(backupPath + gameName + "_translated_backup", backupLocalizationPath); // ?

                File.WriteAllTextAsync(backupPath + @"\unpack_CX.bat", Resources.unpack_CX_bat);
                File.WriteAllBytes(backupPath + @"\unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                System.Threading.Thread.Sleep(500);

                ReplaceStringInFile(backupPath + @"\unpack_CX.bat", 1, "for %%a in (Localization.csv) do unPacker_CSV.exe -uc 3 \"%%a\"");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = backupPath + @"\unpack_CX.bat",
                    WorkingDirectory = Path.GetDirectoryName(backupPath + @"\unpack_CX.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                deleteFile(backupPath + @"\Localization_3.txt");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = backupPath + @"\unpack_CX.bat",
                    WorkingDirectory = Path.GetDirectoryName(backupPath + @"\unpack_CX.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                if (File.Exists(backupPath + @"\Localization_3.txt"))
                    textFromOld = File.ReadAllLines(backupPath + @"\Localization_3.txt");
            }
            else if (gameName == "DungeonCrawler")
            {
                if (File.Exists(backupLocalizationPath + @"\Game.locres.txt"))
                    textFromOld = File.ReadAllLines(backupLocalizationPath + @"\Game.locres.txt");
            }

            if (gameName == "CoreKeeper")
            {
                File.WriteAllTextAsync(localizationPath + @"unpack_CX.bat", Resources.unpack_CX_bat);
                File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                System.Threading.Thread.Sleep(500);

                ReplaceStringInFile(localizationPath + @"unpack_CX.bat", 1, "for %%a in (Localization.csv) do unPacker_CSV.exe -uc 3 \"%%a\"");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = localizationPath + @"unpack_CX.bat",
                    WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                    CreateNoWindow = true
                }).WaitForExit();

                deleteFile(localizationPath + @"Localization_3.txt");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = localizationPath + @"unpack_CX.bat",
                    WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
            }

            if (File.Exists(localizationPath + FilesNameDifferentGames()))
            {
                //textFromNew = File.ReadAllText(localizationPath + FilesNameDifferentGames());
                //textFromOriginalNew = textFromNew;

                if (gameName != "CoreKeeper" && gameName != "Temtem")
                    textArrayFromOriginalNew = File.ReadAllLines(localizationPath + FilesNameDifferentGames());

                if (gameName == "Temtem")
                {
                    string fileToRead = localizationPath + @"I2Languages_4.txt";

                    File.WriteAllTextAsync(localizationPath + @"unpack_CX.bat", Resources.unpack_CX_bat);
                    File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(fileToRead);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                    System.Threading.Thread.Sleep(500);

                    textArrayFromOriginalNew = File.ReadAllLines(fileToRead);
                    File.Copy(localizationPath + FilesNameDifferentGames(), localizationPath + @"OriginalLocalization.txt");
                    System.Threading.Thread.Sleep(500);

                    deleteFile(localizationPath + @"unpack_CX.bat");
                    deleteFile(localizationPath + @"unPacker_CSV.exe");
                    deleteFile(fileToRead);
                }
            }

            using (StreamReader reader = new StreamReader(localizationPath + FilesNameDifferentGames(), encoding: Encoding.UTF8))
            {
                if (gameName == "Temtem")
                {
                    using (StreamWriter sw = new StreamWriter(localizationPath + @"I2Languages2.csv", false, encoding: Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            if (line != "" && line != " ")
                            {
                                string[] idValues = line.Split(SeparatorDifferentGames());
                                string newValues = "";

                                if (gameName == "Temtem")
                                {
                                    idValues[4] = idValues[4].Trim();
                                    newValues = idValues[4];
                                }
                                else
                                {
                                    idValues[3] = idValues[3].Trim();
                                    newValues = idValues[3];
                                }

                                bool exist = false;
                                bool textChanged = false;

                                for (int ii = 0; ii < textFromOld.Length; ii++)
                                {
                                    if (textFromOld[ii] != "" && textFromOld[ii] != " ")
                                    {
                                        string[] idValuesGlobal = textFromOld[ii].Split(SeparatorDifferentGames());
                                        string oldValues = "";

                                        if (gameName == "Temtem")
                                        {
                                            idValuesGlobal[4] = idValuesGlobal[4].Trim();
                                            oldValues = idValuesGlobal[4];
                                        }
                                        else
                                        {
                                            idValuesGlobal[3] = idValuesGlobal[3].Trim();
                                            oldValues = idValuesGlobal[3];
                                        }

                                        idValuesGlobal[0] = idValuesGlobal[0].Trim();
                                        idValues[0] = idValues[0].Trim();

                                        if (idValuesGlobal[0] == idValues[0] && oldValues == newValues)
                                        {
                                            exist = true;

                                            break;
                                        }
                                        //else if (idValuesGlobal[0] == idValues[0] && oldValues != newValues)
                                        //{
                                        //    textChanged = true;
                                        //
                                        //    break;
                                        //
                                        //    // возможно нужно сделать проверку на строки, как выше...
                                        //    //int count = textFromNew.Split(idValues[0]).Length - 1;
                                        //    //if (count > 1)
                                        //    //{
                                        //    //
                                        //    //}
                                        //    //else
                                        //    //{
                                        //    //    textChanged = true;
                                        //    //
                                        //    //    break;
                                        //    //}
                                        //}
                                    }
                                }

                                if (!exist)
                                {
                                    line = line.Trim();
                                    sw.WriteLine(line);
                                    newLinesNumbers.Add(lineNumber);

                                    //if (!textChanged)
                                    //    numberExist = numberExist + 1;
                                }
                            }

                            lineNumber = lineNumber + 1;
                        }
                    }
                }
                else if (gameName == "Escape Simulator")
                {
                    using (StreamWriter sw = new StreamWriter(localizationPath + @"English2.txt", false, encoding: Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            if (line != "" && line != " ")
                            {
                                string[] idValues = line.Split(SeparatorDifferentGames());
                                string newValues = "";

                                if (idValues.Length > 1)
                                {
                                    idValues[1] = idValues[1].Trim();
                                    newValues = idValues[1];
                                }
                                else
                                    newValues = idValues[0];

                                bool exist = false;
                                bool textChanged = false;

                                for (int ii = 0; ii < textFromOld.Length; ii++)
                                {
                                    if (textFromOld[ii] != "" && textFromOld[ii] != " ")
                                    {
                                        string[] idValuesGlobal = textFromOld[ii].Split(SeparatorDifferentGames());
                                        string oldValues = "";

                                        if (idValuesGlobal.Length > 1)
                                        {
                                            idValuesGlobal[1] = idValuesGlobal[1].Trim();
                                            oldValues = idValuesGlobal[1];
                                        }
                                        else
                                            oldValues = idValuesGlobal[0];

                                        idValues[0] = idValues[0].Trim();
                                        idValuesGlobal[0] = idValuesGlobal[0].Trim();

                                        //if (idValuesGlobal[0] == idValues[0] && oldValues == newValues)
                                        //{
                                        //    if (lineNumber == ii + numberExist)
                                        //    {
                                        //        exist = true;
                                        //
                                        //        break;
                                        //    }
                                        //    else
                                        //    {
                                        //        int count = textFromNew.Split(line).Length - 1;
                                        //        if (count > 1)
                                        //        {
                                        //
                                        //        }
                                        //        else
                                        //        {
                                        //            exist = true; //из-за того, что это закомменчено слетит кастомный перевод
                                        //            textChanged = true;
                                        //
                                        //            break;
                                        //        }
                                        //    }
                                        //}
                                        //else if (idValuesGlobal[0] == idValues[0] && oldValues != newValues)
                                        //{
                                        //    //textChanged = true;
                                        //    //
                                        //    //break;
                                        //
                                        //    //if (lineNumber == ii + numberExist)
                                        //    //{
                                        //    //    textChanged = true;
                                        //    //
                                        //    //    break;
                                        //    //}
                                        //    //else
                                        //    //{
                                        //    int count = textFromNew.Split(idValues[0]).Length - 1;
                                        //    if (count > 1)
                                        //    {
                                        //
                                        //    }
                                        //    else
                                        //    {
                                        //        textChanged = true;
                                        //
                                        //        break;
                                        //    }
                                        //    //}
                                        //}

                                        if (idValuesGlobal[0] == idValues[0] && oldValues == newValues)
                                        {
                                            exist = true;

                                            break;
                                        }
                                        //else if (idValuesGlobal[0] == idValues[0] && oldValues != newValues)
                                        //{
                                        //    textChanged = true;
                                        //    
                                        //    break;
                                        //}
                                    }
                                }

                                if (!exist)
                                {
                                    line = line.Trim();
                                    sw.WriteLine(line);

                                    newLinesNumbers.Add(lineNumber);

                                    //if (!textChanged)
                                    //    numberExist = numberExist + 1;
                                }
                            }

                            lineNumber = lineNumber + 1;
                        }
                    }
                }
                else if (gameName == "The Mortuary Assistant")
                {
                    using (StreamWriter sw = new StreamWriter(localizationPath + @"Global_English2.txt", false, encoding: Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            if (line != "" && line != " " && line != "  ")
                            {
                                string[] idValues = line.Split(SeparatorDifferentGames());
                                string newValues = "";

                                if (idValues.Length > 1)
                                {
                                    idValues[1] = idValues[1].Trim();
                                    newValues = idValues[1];
                                }
                                else
                                    newValues = idValues[0];

                                bool exist = false;
                                //bool textChanged = false;

                                for (int ii = 0; ii < textFromOld.Length; ii++)
                                {
                                    if (textFromOld[ii] != "" && textFromOld[ii] != " " && textFromOld[ii] != "  ")
                                    {
                                        string[] idValuesGlobal = textFromOld[ii].Split(SeparatorDifferentGames());
                                        string oldValues = "";

                                        if (idValuesGlobal.Length > 1)
                                        {
                                            idValuesGlobal[1] = idValuesGlobal[1].Trim();
                                            oldValues = idValuesGlobal[1];
                                        }
                                        else
                                            oldValues = idValuesGlobal[0];

                                        idValues[0] = idValues[0].Trim();
                                        idValuesGlobal[0] = idValuesGlobal[0].Trim();

                                        if (idValuesGlobal[0] == idValues[0] && oldValues == newValues)
                                        {
                                            //if (lineNumber == ii + numberExist) // понять как можно организовать проверку на строки, если это возможно,
                                            // т.к. я собераю строки из разных файлов в один файл...
                                            //{
                                            exist = true;

                                            break;
                                            //}
                                        }
                                        //else if (idValuesGlobal[0] == idValues[0] && oldValues != newValues)
                                        //{
                                        //textChanged = true;

                                        //break;
                                        //}
                                    }
                                }

                                if (!exist)
                                {
                                    line = line.TrimStart();
                                    sw.WriteLine(line);
                                    newLinesNumbers.Add(lineNumber);

                                    //if (!textChanged)
                                    //    numberExist = numberExist + 1;
                                }
                            }

                            lineNumber = lineNumber + 1;
                        }
                    }
                }
                else if (gameName == "CoreKeeper")
                {
                    using (StreamWriter sw = new StreamWriter(localizationPath + @"Localization_3_2.txt", false, encoding: Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            if (line != "" && line != " ")
                            {
                                string newValues = line;

                                bool exist = false;
                                bool textChanged = false;

                                for (int ii = 0; ii < textFromOld.Length; ii++)
                                {
                                    if (textFromOld[ii] != "" && textFromOld[ii] != " ")
                                    {
                                        string oldValues = textFromOld[ii];

                                        if (oldValues == newValues)
                                        {
                                            exist = true;

                                            break;
                                        }
                                    }
                                }

                                if (!exist)
                                {
                                    line = line.Trim();
                                    sw.WriteLine(line);
                                    newLinesNumbers.Add(lineNumber);

                                    //if (!textChanged)
                                    //    numberExist = numberExist + 1;
                                }
                            }

                            lineNumber = lineNumber + 1;
                        }
                    }
                }
                else if (gameName == "NewWorld")
                {
                    using (StreamWriter sw = new StreamWriter(localizationPath + @"Global_English2.txt", false, encoding: Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            if (line != "")
                            {
                                string[] idValues = line.Split(SeparatorDifferentGames());


                                string newValues = idValues[1];

                                //idValues[1] = idValues[1].Trim();

                                bool exist = false;
                                //bool textChanged = false;

                                for (int ii = 0; ii < textFromOld.Length; ii++)
                                {
                                    string[] idValuesGlobal = textFromOld[ii].Split(SeparatorDifferentGames());
                                    string oldValues = idValuesGlobal[1];

                                    //idValuesGlobal[1] = idValuesGlobal[1].Trim();

                                    //idValues[0] = idValues[0].Trim();
                                    //idValuesGlobal[0] = idValuesGlobal[0].Trim();

                                    if (idValuesGlobal[0] == idValues[0] && oldValues == newValues)
                                    {
                                        //if (lineNumber == ii + numberExist) // понять как можно организовать проверку на строки, если это возможно,
                                        // т.к. я собираю строки из разных файлов в один файл...
                                        //{
                                        exist = true;

                                        break;
                                        //}
                                    }
                                    //else if (idValuesGlobal[0] == idValues[0] && oldValues != newValues)
                                    //{
                                    //textChanged = true;

                                    //break;
                                    //}
                                }

                                if (!exist)
                                {
                                    //line = line.TrimStart();
                                    sw.WriteLine(line);
                                    newLinesNumbers.Add(lineNumber);

                                    //if (!textChanged)
                                    //    numberExist = numberExist + 1;
                                }
                            }

                            lineNumber = lineNumber + 1;
                        }
                    }
                }
                else if (gameName == "DungeonCrawler")
                {
                    using (StreamWriter sw = new StreamWriter(localizationPath + @"Game2.locres.txt", false, encoding: Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            if (line != "" && line != " ")
                            {
                                string[] idValues = line.Split(SeparatorDifferentGames());
                                string newValues = "";

                                idValues[1] = idValues[1].Trim();
                                newValues = idValues[1];

                                bool exist = false;
                                bool textChanged = false;

                                for (int ii = 0; ii < textFromOld.Length; ii++)
                                {
                                    if (textFromOld[ii] != "" && textFromOld[ii] != " ")
                                    {
                                        string[] idValuesGlobal = textFromOld[ii].Split(SeparatorDifferentGames());
                                        string oldValues = "";

                                        idValuesGlobal[1] = idValuesGlobal[1].Trim();
                                        oldValues = idValuesGlobal[1];

                                        idValuesGlobal[0] = idValuesGlobal[0].Trim();
                                        idValues[0] = idValues[0].Trim();

                                        if (idValuesGlobal[0] == idValues[0] && oldValues == newValues)
                                        {
                                            exist = true;

                                            break;
                                        }
                                        //else if (idValuesGlobal[0] == idValues[0] && oldValues != newValues)
                                        //{
                                        //    textChanged = true;
                                        //
                                        //    break;
                                        //
                                        //    // возможно нужно сделать проверку на строки, как выше...
                                        //    //int count = textFromNew.Split(idValues[0]).Length - 1;
                                        //    //if (count > 1)
                                        //    //{
                                        //    //
                                        //    //}
                                        //    //else
                                        //    //{
                                        //    //    textChanged = true;
                                        //    //
                                        //    //    break;
                                        //    //}
                                        //}
                                    }
                                }

                                if (!exist)
                                {
                                    line = line.Trim();
                                    sw.WriteLine(line);
                                    newLinesNumbers.Add(lineNumber);

                                    //if (!textChanged)
                                    //    numberExist = numberExist + 1;
                                }
                            }

                            lineNumber = lineNumber + 1;
                        }
                    }
                }
            }

            if (gameName == "Temtem")
            {
                deleteFile(localizationPath + FilesNameDifferentGames());
                File.Copy(localizationPath + @"I2Languages2.csv", localizationPath + FilesNameDifferentGames());
                deleteFile(localizationPath + @"I2Languages2.csv");

                deleteFile(backupLocalizationPath + @"\I2.Loc\I2Languages.csv");
                delFolder(backupLocalizationPath + @"\I2.Loc");
                delFolder(backupLocalizationPath);
                delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono");
                delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                delFolder(backupPath + "Unity_Assets_Files");
            }
            else if (gameName == "Escape Simulator")
            {
                deleteFile(localizationPath + FilesNameDifferentGames());
                File.Copy(localizationPath + @"English2.txt", localizationPath + FilesNameDifferentGames());
                deleteFile(localizationPath + @"English2.txt");

                deleteFile(backupLocalizationPath + FilesNameDifferentGames());
                delFolder(backupLocalizationPath);
                delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                delFolder(backupPath + "Unity_Assets_Files");
            }
            else if (gameName == "The Mortuary Assistant")
            {
                deleteFile(localizationPath + FilesNameDifferentGames());
                File.Copy(localizationPath + @"Global_English2.txt", localizationPath + FilesNameDifferentGames());
                deleteFile(localizationPath + @"Global_English2.txt");

                deleteFile(backupLocalizationPath + FilesNameDifferentGames());
                delFolder(backupLocalizationPath);
                delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                delFolder(backupPath + "Unity_Assets_Files");
            }
            else if (gameName == "CoreKeeper")
            {
                deleteFile(localizationPath + FilesNameDifferentGames());
                File.Copy(localizationPath + @"Localization_3_2.txt", localizationPath + FilesNameDifferentGames());
                deleteFile(localizationPath + @"Localization_3_2.txt");

                deleteFile(backupLocalizationPath);
            }
            else if (gameName == "NewWorld")
            {
                deleteFile(localizationPath + @"Global_English.txt");
                File.Copy(localizationPath + @"Global_English2.txt", localizationPath + @"Global_English.txt");
                deleteFile(localizationPath + @"Global_English2.txt");

                string[] backupFilesToRead = Directory.GetFiles(backupLocalizationPath);

                for (int i = 0; i < backupFilesToRead.Length; i++)
                {
                    deleteFile(backupFilesToRead[i]);
                }
                delFolder(backupLocalizationPath);
                delFolder(backupPath + @"levels\newlevels\localization");
                delFolder(backupPath + @"levels\newlevels");
                delFolder(backupPath + @"levels");
            }
            else if (gameName == "DungeonCrawler")
            {
                deleteFile(localizationPath + FilesNameDifferentGames());
                File.Copy(localizationPath + @"Game2.locres.txt", localizationPath + FilesNameDifferentGames());
                deleteFile(localizationPath + @"Game2.locres.txt");

                deleteFile(backupPath + @"\export_locres.bat");
                deleteFile(backupPath + @"\quickbms_4gb_files.exe");
                deleteFile(backupLocalizationPath + @"export_txt_from_locres.bat");
                deleteFile(backupLocalizationPath + @"Game.locres");
                deleteFile(backupLocalizationPath + @"Game.locres.txt");
                deleteFile(backupLocalizationPath + @"UE4localizationsTool.exe");

                backupLocalizationPath = backupLocalizationPath.Replace(backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en\", backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game");
                deleteFile(backupLocalizationPath + @"\en\Game.locres");
                if (Directory.Exists(backupLocalizationPath + @"\en"))
                {
                    delFolder(backupLocalizationPath + @"\en");
                    delFolder(backupLocalizationPath);
                    delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization\Game");
                    delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content\Localization");
                    delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName + @"\Content");
                    delFolder(backupPath + @"\" + gameName + @"_backup\" + gameName);
                    delFolder(backupPath + @"\" + gameName + @"_backup\");
                }
            }

            System.Threading.Thread.Sleep(500);

            WorkAsync();
        }

        /*async void SqlUpdateAsync()
        {
            await Task.Run(() => SqlUpdate());
        }

        void SqlUpdate()
        {
            CreateBackupTestAsync();

            System.Threading.Thread.Sleep(1500);

            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Запись новых строк в базу данных...", Color.Black)));

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=db\\db.db3; Version = 3; Compress = True;"))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(conn);
                SQLiteCommand command2 = new SQLiteCommand(conn);
                command.Connection = conn;
                command2.Connection = conn;

                if (!installBreak)
                {
                    string str = "CREATE TABLE IF NOT EXISTS " + gameName + "_newTranslate (id VARCHAR, text VARCHAR, nil VARCHAR, nil1 VARCHAR, eng VARCHAR, es VARCHAR, fr VARCHAR, de VARCHAR, jp VARCHAR, chn VARCHAR, kr VARCHAR, prt VARCHAR, nil2 VARCHAR, nil3 VARCHAR, id_sort INTEGER)";
                    command.CommandText = str;
                    command.ExecuteNonQuery();
                }

                using (StreamReader reader = new StreamReader(localizationPath + @"I2Languages.csv", encoding: Encoding.UTF8))
                {
                    SQLiteDataReader dr = null;
                    bool readingSql;
                    if (installBreak)
                    {
                        string sqlQuery = "SELECT id FROM " + gameName;
                        command.CommandText = sqlQuery;
                        dr = command.ExecuteReader();

                        readingSql = !reader.EndOfStream && dr.Read();
                    }
                    else
                    {
                        readingSql = !reader.EndOfStream;
                    }

                    int id_sort = 0;
                    while (readingSql)
                    {
                        string line = reader.ReadLine();
                        line = line.Replace("'", "''");
                        string[] values = line.Split(';');

                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                        int progressPercent = progressBar.Value * 100 / progressBar.Maximum;
                        lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                        if (!installBreak)
                        {
                            exitNotRecommended = true;

                            if (values[0] != "")
                            {
                                id_sort += 1;
                                string sql = "";
                                sql = "BEGIN TRANSACTION;INSERT INTO " + gameName + $"_newTranslate VALUES ('{values[0]}', '{values[1]}', '{values[2]}', '{values[3]}', '{values[4]}', '{values[5]}', '{values[6]}', '{values[7]}', '{values[8]}', '{values[9]}', '{values[10]}', '{values[11]}', '{values[12]}', '{values[13]}', '{id_sort}');COMMIT;";
                                command2.CommandText = sql;
                                command2.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            if (values[0] != "" && values[0].Replace("''", "'") != dr.GetString(0))
                            {
                                id_sort += 1;
                                string sql = "";
                                sql = "BEGIN TRANSACTION;INSERT INTO " + gameName + $" VALUES ('{values[0]}', '{values[1]}', '{values[2]}', '{values[3]}', '{values[4]}', '{values[5]}', '{values[6]}', '{values[7]}', '{values[8]}', '{values[9]}', '{values[10]}', '{values[11]}', '{values[12]}', '{values[13]}', '{id_sort}');COMMIT;";
                                command2.CommandText = sql;
                                command2.ExecuteNonQuery();
                            }
                        }
                    }

                    if (installBreak)
                        dr.Close();
                    reader.Close();
                    conn.Close();

                    exitNotRecommended = false;
                }
            }

            WorkAsync();
        }*/

        string FilesNameDifferentGames()
        {
            string fileName = "";

            if (gameName == "Temtem")
                fileName = @"\I2Languages.csv";
            else if (gameName == "Escape Simulator")
                fileName = @"\English.txt";
            else if (gameName == "The Mortuary Assistant" || gameName == "NewWorld")
                fileName = @"\Global_English.txt";
            else if (gameName == "CoreKeeper")
                fileName = @"\Localization_3.txt";
            else if (gameName == "DungeonCrawler")
                fileName = @"\Game.locres.txt";

            return fileName;
        }

        char SeparatorDifferentGames()
        {
            char separator = ';';

            if (gameName == "Temtem")
                separator = ';';
            else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                separator = ':';
            else if (gameName == "CoreKeeper" || gameName == "NewWorld")
                separator = '\t';
            else if (gameName == "DungeonCrawler")
                separator = '=';
            else if (gameName == "nwr")
                separator = '\"';

            return separator;
        }

        /*string SqlSelectDifferentGames()
        {
            string selectReturn = "";

            if (gameName == "Temtem")
            {
                selectReturn = "id, text, nil, nil1, eng, es, fr, de, jp, chn, kr, prt, nil2, nil3";
            }

            return selectReturn;
        }*/

        /*void SqlUpdateTranslatedText(string sourceText, SQLiteCommand command, string result)
        {
            if (sourceText == "")
                return;

            string sqlTranslatedInsert = "";
            if (!translateUpdate || reinstallWithoutBackup || installBreak)
                sqlTranslatedInsert = "UPDATE " + gameName + $" SET eng = REPLACE (eng, '{sourceText}', '{result}')";
            else if (translateUpdate && !reinstallWithoutBackup && !installBreak)
                sqlTranslatedInsert = "UPDATE " + gameName + $"_newTranslate SET eng = REPLACE (eng, '{sourceText}', '{result}')";
            command.CommandText = sqlTranslatedInsert;
            command.ExecuteNonQuery();
        }*/

        void TranslatedTextConstructor(string sourceText, List<int> lineNumbers)
        {
            if (sourceText != "")
            {
                string[] originalText = sourceText.Split("\n____\n");
                string[] originalText3 = sourceText.Split("\n____\n");
                string[] translatedText = new string[] { };
                List<string> curlyBracketValues = new();
                List<string> squareQuoteValues = new();
                List<string> triangQuoteValues = new();
                List<string> percentValues = new();

                if (gameName == "Temtem" || gameName == "Escape Simulator")
                {
                    for (int i = 0; i < originalText.Length; i++)
                    {
                        if (originalText[i].Contains("[") && originalText[i].Contains("]"))
                        {
                            int t = 0;
                            int count = originalText[i].Split("]").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf("]", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("[", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("[", t);
                                int end = originalText[i].IndexOf("]", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    squareQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }

                        if (originalText[i].Contains("<") && originalText[i].Contains(">"))
                        {
                            int t = 0;
                            int count = originalText[i].Split(">").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf(">", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("<", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("<", t);
                                int end = originalText[i].IndexOf(">", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    triangQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }
                    }

                    sourceText = sourceText.Replace("[]", @"<4>_ ");
                    if (yandexButton.Checked)
                        sourceText = sourceText.Replace("<>", @"<3>_ ");
                    else
                        sourceText = sourceText.Replace("<>", @"<3>");
                }
                else if (gameName == "The Mortuary Assistant")
                {
                    for (int i = 0; i < originalText.Length; i++)
                    {
                        if (originalText[i].Contains("<") && originalText[i].Contains(">"))
                        {
                            int t = 0;
                            int count = originalText[i].Split(">").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf(">", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("<", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("<", t);
                                int end = originalText[i].IndexOf(">", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    triangQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }
                    }

                    if (yandexButton.Checked)
                        sourceText = sourceText.Replace("<>", @"<3>_ ");
                    else
                        sourceText = sourceText.Replace("<>", @"<3>");
                }
                else if (gameName == "NewWorld")
                {
                    for (int i = 0; i < originalText.Length; i++)
                    {
                        originalText[i] = originalText[i].Replace("&lt;", "<");
                        originalText[i] = originalText[i].Replace("&gt;", ">");
                        originalText[i] = originalText[i].Replace("&amp;", "™");
                        originalText[i] = originalText[i].Replace("&apos;", "'");

                        if (originalText[i].Contains("{") && originalText[i].Contains("}"))
                        {
                            int t = 0;
                            int count = originalText[i].Split("}").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf("}", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("{", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("{", t);
                                int end = originalText[i].IndexOf("}", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    curlyBracketValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }

                        if (originalText[i].Contains("[") && originalText[i].Contains("]"))
                        {
                            int t = 0;
                            int count = originalText[i].Split("]").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf("]", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("[", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("[", t);
                                int end = originalText[i].IndexOf("]", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    squareQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }

                        if (originalText[i].Contains("<") && originalText[i].Contains(">"))
                        {
                            int t = 0;
                            int count = originalText[i].Split(">").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf(">", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("<", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("<", t);
                                int end = originalText[i].IndexOf(">", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    triangQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }
                    }

                    sourceText = sourceText.Replace("{}", @"<6>_ ");
                    sourceText = sourceText.Replace("[]", @"<4>_ ");
                    if (yandexButton.Checked)
                        sourceText = sourceText.Replace("<>", @"<3>_ ");
                    else
                        sourceText = sourceText.Replace("<>", @"<3>");
                }
                else if (gameName == "DungeonCrawler")
                {
                    for (int i = 0; i < originalText.Length; i++)
                    {
                        originalText[i] = originalText[i].Replace("&lt;", "<");
                        originalText[i] = originalText[i].Replace("&gt;", ">");
                        originalText[i] = originalText[i].Replace("&amp;", "™");
                        originalText[i] = originalText[i].Replace("&apos;", "'");

                        if (originalText[i].Contains("[") && originalText[i].Contains("]"))
                        {
                            int t = 0;
                            int count = originalText[i].Split("]").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf("]", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("[", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("[", t);
                                int end = originalText[i].IndexOf("]", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    squareQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }

                        if (originalText[i].Contains("{") && originalText[i].Contains("}"))
                        {
                            int t = 0;
                            int count = originalText[i].Split("}").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf("}", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("{", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("{", t);
                                int end = originalText[i].IndexOf("}", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    curlyBracketValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }

                        if (originalText[i].Contains("<") && originalText[i].Contains(">"))
                        {
                            int t = 0;
                            int count = originalText[i].Split(">").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf(">", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("<", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("<", t);
                                int end = originalText[i].IndexOf(">", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    triangQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }
                    }

                    sourceText = sourceText.Replace("{}", @"<6>_ ");
                    sourceText = sourceText.Replace("[]", @"<4>_ ");
                    if (yandexButton.Checked)
                        sourceText = sourceText.Replace("<>", @"<3>_ ");
                    else
                        sourceText = sourceText.Replace("<>", @"<3>");
                }
                else if (gameName == "nwr")
                {
                    for (int i = 0; i < originalText.Length; i++)
                    {
                        originalText[i] = originalText[i].Replace("&lt;", "<");
                        originalText[i] = originalText[i].Replace("&gt;", ">");
                        originalText[i] = originalText[i].Replace("&amp;", "™");
                        originalText[i] = originalText[i].Replace("&apos;", "'");

                        if (originalText[i].Contains("{") && originalText[i].Contains("}"))
                        {
                            int t = 0;
                            int count = originalText[i].Split("}").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf("}", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("{", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("{", t);
                                int end = originalText[i].IndexOf("}", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    curlyBracketValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }

                        if (originalText[i].Contains("<") && originalText[i].Contains(">"))
                        {
                            int t = 0;
                            int count = originalText[i].Split(">").Length - 1;

                            for (int j = 0; j < count; j++)
                            {
                                if (j == count)
                                    break;

                                if (j > 0)
                                {
                                    t = originalText[i].IndexOf(">", t + 1);
                                    //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                    if (originalText[i].IndexOf("<", t) == -1)
                                        break;
                                }

                                int start = originalText[i].IndexOf("<", t);
                                int end = originalText[i].IndexOf(">", start);
                                string result = originalText[i].Substring(start + 1, end - start - 1);

                                if (result.Any(char.IsLetter))
                                {
                                    triangQuoteValues.Add(result);

                                    string rep = originalText[i].Remove(start + 1, end - start - 1);
                                    sourceText = sourceText.Replace(originalText[i], rep);
                                    originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                }
                            }
                        }

                        if (originalText[i].Contains("%"))
                        {
                            int t = 0;
                            int count = originalText[i].Split("%").Length - 1;

                            if (count > 1)
                            {
                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = originalText[i].IndexOf("%", t + 1);
                                        //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                                        if (originalText[i].IndexOf("%", t) == -1)
                                            break;
                                    }

                                    int start = originalText[i].IndexOf("%", t);
                                    int end = originalText[i].IndexOf("%", start);
                                    string result = originalText[i].Substring(start + 1, end - start - 1);

                                    if (result.Any(char.IsLetter))
                                    {
                                        percentValues.Add(result);

                                        string rep = originalText[i].Remove(start + 1, end - start - 1);
                                        sourceText = sourceText.Replace(originalText[i], rep);
                                        originalText[i] = originalText[i].Remove(start + 1, end - start - 1);
                                    }
                                }
                            }
                        }
                    }

                    sourceText = sourceText.Replace("{}", @"<6>_ ");
                    sourceText = sourceText.Replace("%%", @"<5>_ ");
                    if (yandexButton.Checked)
                        sourceText = sourceText.Replace("<>", @"<3>_ ");
                    else
                        sourceText = sourceText.Replace("<>", @"<3>");
                }

                if (gameName == "Temtem")
                    sourceText = sourceText.Replace("pansuns", "<213>_ ");
                sourceText = sourceText.Replace(@"\n\n", "<236>_ ");
                sourceText = sourceText.Replace(@"\n", "<216>_ ");

                //int saying3 = sourceText.Split(" - - ").Length - 1;

                if (deeplButton.Checked)
                {
                    sourceText = sourceText.Replace(@"\x20", " ");

                    try
                    {
                        var clearButton = driver.FindElement(By.XPath("//*[@id=\"translator-source-clear-button\"]"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                    }
                    catch
                    {
                        try
                        {
                            var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                        }
                        catch
                        {

                        }
                    }

                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]"));

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            #region proxies
                            if (portEnabled == 0)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "8110";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 1;
                            }
                            else if (portEnabled == 1)
                            {
                                string port = "8133";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 2;
                            }
                            else if (portEnabled == 2)
                            {
                                string port = "7300";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 3;
                            }
                            else if (portEnabled == 3)
                            {
                                string port = "8382";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 4;
                            }
                            else if (portEnabled == 4)
                            {
                                string port = "6286";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 5;
                            }
                            else if (portEnabled == 5)
                            {
                                string port = "6100";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 6;
                            }
                            else if (portEnabled == 6)
                            {
                                string port = "8279";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 7;
                            }
                            else if (portEnabled == 7)
                            {
                                string port = "6893";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 8;
                            }
                            else if (portEnabled == 8)
                            {
                                string port = "7492";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 9;
                            }
                            else if (portEnabled == 9)
                            {
                                string port = "8780";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 10;
                            }
                            else if (portEnabled == 10)
                            {
                                portEnabled = 0;

                                MessageBox.Show(
                                    "Proxy is out",
                                    "Proxy 10",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                            }
                            #endregion

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://www.deepl.com/translator"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {

                            }

                            try
                            {
                                var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(clearButton).Click().Perform();
                            }
                            catch
                            {
                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {

                                }
                            }

                            System.Threading.Thread.Sleep(500);
                        }
                        catch
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//div[@id=\"headlessui-dialog-title-6\"]"));

                                driver.Close();
                                driver.Quit();
                                driver = null;

                                //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                cmdKill("geckodriver.exe");
                                //if (Process.GetProcessesByName("firefox").Length > 0)
                                cmdKill("firefox.exe");
                                System.Threading.Thread.Sleep(500);
                                deleteFile(Application.StartupPath + @"geckodriver.exe");
                                System.Threading.Thread.Sleep(500);

                                if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                    File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                System.Threading.Thread.Sleep(500);

                                FirefoxOptions firefoxOptions = new FirefoxOptions();
                                FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                #region proxies
                                if (portEnabled == 0)
                                {
                                    //var myProxy = "login:pass@proxy:port";
                                    string port = "8110";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 1;
                                }
                                else if (portEnabled == 1)
                                {
                                    string port = "8133";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 2;
                                }
                                else if (portEnabled == 2)
                                {
                                    string port = "7300";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 3;
                                }
                                else if (portEnabled == 3)
                                {
                                    string port = "8382";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 4;
                                }
                                else if (portEnabled == 4)
                                {
                                    string port = "6286";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 5;
                                }
                                else if (portEnabled == 5)
                                {
                                    string port = "6100";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 6;
                                }
                                else if (portEnabled == 6)
                                {
                                    string port = "8279";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 7;
                                }
                                else if (portEnabled == 7)
                                {
                                    string port = "6893";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 8;
                                }
                                else if (portEnabled == 8)
                                {
                                    string port = "7492";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 9;
                                }
                                else if (portEnabled == 9)
                                {
                                    string port = "8780";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 10;
                                }
                                else if (portEnabled == 10)
                                {
                                    portEnabled = 0;

                                    MessageBox.Show(
                                        "Proxy is out",
                                        "Proxy 10",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                                }
                                #endregion

                                firefoxService.HideCommandPromptWindow = true;
                                firefoxOptions.AddArguments("--headless");

                                driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                {
                                    Url = "https://www.deepl.com/translator"
                                };

                                System.Threading.Thread.Sleep(5000);

                                try
                                {
                                    driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                }
                                catch
                                {

                                }

                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {
                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {

                                    }
                                }

                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                                try
                                {
                                    driver.FindElement(By.XPath("//div[@class=\"lmt__notification__blocked\"]"));

                                    driver.Close();
                                    driver.Quit();
                                    driver = null;

                                    //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                    cmdKill("geckodriver.exe");
                                    //if (Process.GetProcessesByName("firefox").Length > 0)
                                    cmdKill("firefox.exe");
                                    System.Threading.Thread.Sleep(500);
                                    deleteFile(Application.StartupPath + @"geckodriver.exe");
                                    System.Threading.Thread.Sleep(500);

                                    if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                        File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                    System.Threading.Thread.Sleep(500);

                                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                                    FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                    #region proxies
                                    if (portEnabled == 0)
                                    {
                                        //var myProxy = "login:pass@proxy:port";
                                        string port = "8110";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 1;
                                    }
                                    else if (portEnabled == 1)
                                    {
                                        string port = "8133";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 2;
                                    }
                                    else if (portEnabled == 2)
                                    {
                                        string port = "7300";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 3;
                                    }
                                    else if (portEnabled == 3)
                                    {
                                        string port = "8382";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 4;
                                    }
                                    else if (portEnabled == 4)
                                    {
                                        string port = "6286";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 5;
                                    }
                                    else if (portEnabled == 5)
                                    {
                                        string port = "6100";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 6;
                                    }
                                    else if (portEnabled == 6)
                                    {
                                        string port = "8279";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 7;
                                    }
                                    else if (portEnabled == 7)
                                    {
                                        string port = "6893";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 8;
                                    }
                                    else if (portEnabled == 8)
                                    {
                                        string port = "7492";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 9;
                                    }
                                    else if (portEnabled == 9)
                                    {
                                        string port = "8780";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 10;
                                    }
                                    else if (portEnabled == 10)
                                    {
                                        portEnabled = 0;

                                        MessageBox.Show(
                                            "Proxy is out",
                                            "Proxy 10",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1,
                                            MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                    #endregion

                                    firefoxService.HideCommandPromptWindow = true;
                                    firefoxOptions.AddArguments("--headless");

                                    driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                    {
                                        Url = "https://www.deepl.com/translator"
                                    };

                                    System.Threading.Thread.Sleep(5000);

                                    try
                                    {
                                        driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                        }
                                        catch
                                        {

                                        }
                                    }

                                    System.Threading.Thread.Sleep(500);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }

                    var textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                    //new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();
                    textInputElement.Click();
                    textInputElement.SendKeys(sourceText);

                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                    System.Threading.Thread.Sleep(2500);

                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//div[@id=\"headlessui-dialog-title-6\"]")); // "//*[@dl-test=\"notification-too-many-requests\"]"

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            #region proxies
                            if (portEnabled == 0)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "8110";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 1;
                            }
                            else if (portEnabled == 1)
                            {
                                string port = "8133";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 2;
                            }
                            else if (portEnabled == 2)
                            {
                                string port = "7300";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 3;
                            }
                            else if (portEnabled == 3)
                            {
                                string port = "8382";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 4;
                            }
                            else if (portEnabled == 4)
                            {
                                string port = "6286";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 5;
                            }
                            else if (portEnabled == 5)
                            {
                                string port = "6100";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 6;
                            }
                            else if (portEnabled == 6)
                            {
                                string port = "8279";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 7;
                            }
                            else if (portEnabled == 7)
                            {
                                string port = "6893";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 8;
                            }
                            else if (portEnabled == 8)
                            {
                                string port = "7492";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 9;
                            }
                            else if (portEnabled == 9)
                            {
                                string port = "8780";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 10;
                            }
                            else if (portEnabled == 10)
                            {
                                portEnabled = 0;

                                MessageBox.Show(
                                    "Proxy is out",
                                    "Proxy 10",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                            }
                            #endregion

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://www.deepl.com/translator"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                            }

                            driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                            System.Threading.Thread.Sleep(500);

                            try
                            {
                                var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(clearButton).Click().Perform();
                            }
                            catch
                            {
                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {

                                }
                            }

                            System.Threading.Thread.Sleep(500);

                            textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                            new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();
                        }
                        catch
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]"));

                                driver.Close();
                                driver.Quit();
                                driver = null;

                                //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                cmdKill("geckodriver.exe");
                                //if (Process.GetProcessesByName("firefox").Length > 0)
                                cmdKill("firefox.exe");
                                System.Threading.Thread.Sleep(500);
                                deleteFile(Application.StartupPath + @"geckodriver.exe");
                                System.Threading.Thread.Sleep(500);

                                if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                    File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                System.Threading.Thread.Sleep(500);

                                FirefoxOptions firefoxOptions = new FirefoxOptions();
                                FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                #region proxies
                                if (portEnabled == 0)
                                {
                                    //var myProxy = "login:pass@proxy:port";
                                    string port = "8110";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 1;
                                }
                                else if (portEnabled == 1)
                                {
                                    string port = "8133";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 2;
                                }
                                else if (portEnabled == 2)
                                {
                                    string port = "7300";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 3;
                                }
                                else if (portEnabled == 3)
                                {
                                    string port = "8382";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 4;
                                }
                                else if (portEnabled == 4)
                                {
                                    string port = "6286";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 5;
                                }
                                else if (portEnabled == 5)
                                {
                                    string port = "6100";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 6;
                                }
                                else if (portEnabled == 6)
                                {
                                    string port = "8279";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 7;
                                }
                                else if (portEnabled == 7)
                                {
                                    string port = "6893";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 8;
                                }
                                else if (portEnabled == 8)
                                {
                                    string port = "7492";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 9;
                                }
                                else if (portEnabled == 9)
                                {
                                    string port = "8780";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 10;
                                }
                                else if (portEnabled == 10)
                                {
                                    portEnabled = 0;

                                    MessageBox.Show(
                                        "Proxy is out",
                                        "Proxy 10",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                                }
                                #endregion

                                firefoxService.HideCommandPromptWindow = true;
                                firefoxOptions.AddArguments("--headless");

                                driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                {
                                    Url = "https://www.deepl.com/translator"
                                };

                                System.Threading.Thread.Sleep(5000);

                                try
                                {
                                    driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                }
                                catch
                                {

                                }

                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {
                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {

                                    }
                                }

                                System.Threading.Thread.Sleep(500);

                                textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                                new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();
                            }
                            catch
                            {
                                try
                                {
                                    driver.FindElement(By.XPath("//div[@class=\"lmt__notification__blocked\"]"));

                                    driver.Close();
                                    driver.Quit();
                                    driver = null;

                                    //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                    cmdKill("geckodriver.exe");
                                    //if (Process.GetProcessesByName("firefox").Length > 0)
                                    cmdKill("firefox.exe");
                                    System.Threading.Thread.Sleep(500);
                                    deleteFile(Application.StartupPath + @"geckodriver.exe");
                                    System.Threading.Thread.Sleep(500);

                                    if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                        File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                    System.Threading.Thread.Sleep(500);

                                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                                    FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                    #region proxies
                                    if (portEnabled == 0)
                                    {
                                        //var myProxy = "login:pass@proxy:port";
                                        string port = "8110";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 1;
                                    }
                                    else if (portEnabled == 1)
                                    {
                                        string port = "8133";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 2;
                                    }
                                    else if (portEnabled == 2)
                                    {
                                        string port = "7300";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 3;
                                    }
                                    else if (portEnabled == 3)
                                    {
                                        string port = "8382";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 4;
                                    }
                                    else if (portEnabled == 4)
                                    {
                                        string port = "6286";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 5;
                                    }
                                    else if (portEnabled == 5)
                                    {
                                        string port = "6100";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 6;
                                    }
                                    else if (portEnabled == 6)
                                    {
                                        string port = "8279";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 7;
                                    }
                                    else if (portEnabled == 7)
                                    {
                                        string port = "6893";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 8;
                                    }
                                    else if (portEnabled == 8)
                                    {
                                        string port = "7492";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 9;
                                    }
                                    else if (portEnabled == 9)
                                    {
                                        string port = "8780";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 10;
                                    }
                                    else if (portEnabled == 10)
                                    {
                                        portEnabled = 0;

                                        MessageBox.Show(
                                            "Proxy is out",
                                            "Proxy 10",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1,
                                            MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                    #endregion

                                    firefoxService.HideCommandPromptWindow = true;
                                    firefoxOptions.AddArguments("--headless");

                                    driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                    {
                                        Url = "https://www.deepl.com/translator"
                                    };

                                    System.Threading.Thread.Sleep(5000);

                                    try
                                    {
                                        driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                        }
                                        catch
                                        {

                                        }
                                    }

                                    System.Threading.Thread.Sleep(500);

                                    textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                                    new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();
                                }
                                catch
                                {

                                }
                            }
                        }
                    }

                    if (driver.FindElement(By.XPath("//*[@id=\"deepl-ui-tooltip-id-1\"]/span[1]")).GetAttribute("innerHTML") != "русский")
                    {
                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();//*[@id="panelTranslateText"]/div/div[2]/section[2]/div[8]/div[2]/div[2]/button[9]
                        System.Threading.Thread.Sleep(1000);
                    }

                    try
                    {
                        int t = 0;

                        while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML").Length <= 4/*2*/ ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "" ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == " " ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "..." ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "\"" ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == @"""" ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML").Contains("[...]") ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed ||
                            driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed ||
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "lt" ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "&lt;")
                        {
                            System.Threading.Thread.Sleep(500);
                            t = t + 1;

                            if (t > 30 && installBreak)
                            {
                                break;
                            }
                            else if (t > 30 && !installBreak)
                            {
                                driver.Close();
                                driver.Quit();
                                driver = null;

                                //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                cmdKill("geckodriver.exe");
                                //if (Process.GetProcessesByName("firefox").Length > 0)
                                cmdKill("firefox.exe");
                                System.Threading.Thread.Sleep(500);
                                deleteFile(Application.StartupPath + @"geckodriver.exe");
                                System.Threading.Thread.Sleep(500);

                                if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                    File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                System.Threading.Thread.Sleep(500);

                                FirefoxOptions firefoxOptions = new FirefoxOptions();
                                FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                #region proxies
                                if (portEnabled == 0)
                                {
                                    //var myProxy = "login:pass@proxy:port";
                                    string port = "8110";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 1;
                                }
                                else if (portEnabled == 1)
                                {
                                    string port = "8133";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 2;
                                }
                                else if (portEnabled == 2)
                                {
                                    string port = "7300";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 3;
                                }
                                else if (portEnabled == 3)
                                {
                                    string port = "8382";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 4;
                                }
                                else if (portEnabled == 4)
                                {
                                    string port = "6286";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 5;
                                }
                                else if (portEnabled == 5)
                                {
                                    string port = "6100";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 6;
                                }
                                else if (portEnabled == 6)
                                {
                                    string port = "8279";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 7;
                                }
                                else if (portEnabled == 7)
                                {
                                    string port = "6893";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 8;
                                }
                                else if (portEnabled == 8)
                                {
                                    string port = "7492";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 9;
                                }
                                else if (portEnabled == 9)
                                {
                                    string port = "8780";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 10;
                                }
                                else if (portEnabled == 10)
                                {
                                    portEnabled = 0;

                                    MessageBox.Show(
                                        "Proxy is out",
                                        "Proxy 10",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                                }
                                #endregion

                                firefoxService.HideCommandPromptWindow = true;
                                firefoxOptions.AddArguments("--headless");

                                driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                {
                                    Url = "https://www.deepl.com/translator"
                                };

                                System.Threading.Thread.Sleep(5000);

                                try
                                {
                                    driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                }
                                catch
                                {
                                }

                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                System.Threading.Thread.Sleep(500);

                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {
                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {

                                    }
                                }

                                System.Threading.Thread.Sleep(500);

                                textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                                new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();

                                try
                                {
                                    while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed)
                                    {
                                        System.Threading.Thread.Sleep(500);
                                    }
                                }
                                catch
                                {

                                }

                                try
                                {
                                    while (driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "..." || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "\"" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == @"""" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains(" [...]  [...] "))
                                    {
                                        System.Threading.Thread.Sleep(500);
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    catch
                    {

                    }

                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]"));

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            #region proxies
                            if (portEnabled == 0)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "8110";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 1;
                            }
                            else if (portEnabled == 1)
                            {
                                string port = "8133";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 2;
                            }
                            else if (portEnabled == 2)
                            {
                                string port = "7300";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 3;
                            }
                            else if (portEnabled == 3)
                            {
                                string port = "8382";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 4;
                            }
                            else if (portEnabled == 4)
                            {
                                string port = "6286";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 5;
                            }
                            else if (portEnabled == 5)
                            {
                                string port = "6100";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 6;
                            }
                            else if (portEnabled == 6)
                            {
                                string port = "8279";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 7;
                            }
                            else if (portEnabled == 7)
                            {
                                string port = "6893";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 8;
                            }
                            else if (portEnabled == 8)
                            {
                                string port = "7492";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 9;
                            }
                            else if (portEnabled == 9)
                            {
                                string port = "8780";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 10;
                            }
                            else if (portEnabled == 10)
                            {
                                portEnabled = 0;

                                MessageBox.Show(
                                    "Proxy is out",
                                    "Proxy 10",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                            }
                            #endregion

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://www.deepl.com/translator"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                            }

                            driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                            System.Threading.Thread.Sleep(500);

                            try
                            {
                                var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(clearButton).Click().Perform();
                            }
                            catch
                            {
                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {

                                }
                            }

                            System.Threading.Thread.Sleep(500);

                            textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                            new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();

                            try
                            {
                                while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed)
                                {
                                    System.Threading.Thread.Sleep(500);
                                }
                            }
                            catch
                            {

                            }

                            try
                            {
                                while (driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "..." || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "\"" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == @"""" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains(" [...]  [...] "))
                                {
                                    System.Threading.Thread.Sleep(500);
                                }
                            }
                            catch
                            {

                            }
                        }
                        catch
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//div[@id=\"headlessui-dialog-title-6\"]"));

                                driver.Close();
                                driver.Quit();
                                driver = null;

                                //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                cmdKill("geckodriver.exe");
                                //if (Process.GetProcessesByName("firefox").Length > 0)
                                cmdKill("firefox.exe");
                                System.Threading.Thread.Sleep(500);
                                deleteFile(Application.StartupPath + @"geckodriver.exe");
                                System.Threading.Thread.Sleep(500);

                                if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                    File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                System.Threading.Thread.Sleep(500);

                                FirefoxOptions firefoxOptions = new FirefoxOptions();
                                FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                #region proxies
                                if (portEnabled == 0)
                                {
                                    //var myProxy = "login:pass@proxy:port";
                                    string port = "8110";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 1;
                                }
                                else if (portEnabled == 1)
                                {
                                    string port = "8133";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 2;
                                }
                                else if (portEnabled == 2)
                                {
                                    string port = "7300";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 3;
                                }
                                else if (portEnabled == 3)
                                {
                                    string port = "8382";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 4;
                                }
                                else if (portEnabled == 4)
                                {
                                    string port = "6286";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 5;
                                }
                                else if (portEnabled == 5)
                                {
                                    string port = "6100";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 6;
                                }
                                else if (portEnabled == 6)
                                {
                                    string port = "8279";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 7;
                                }
                                else if (portEnabled == 7)
                                {
                                    string port = "6893";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 8;
                                }
                                else if (portEnabled == 8)
                                {
                                    string port = "7492";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 9;
                                }
                                else if (portEnabled == 9)
                                {
                                    string port = "8780";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 10;
                                }
                                else if (portEnabled == 10)
                                {
                                    portEnabled = 0;

                                    MessageBox.Show(
                                        "Proxy is out",
                                        "Proxy 10",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                                }
                                #endregion

                                firefoxService.HideCommandPromptWindow = true;
                                firefoxOptions.AddArguments("--headless");

                                driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                {
                                    Url = "https://www.deepl.com/translator"
                                };

                                System.Threading.Thread.Sleep(5000);

                                try
                                {
                                    driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                }
                                catch
                                {
                                }

                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                System.Threading.Thread.Sleep(500);

                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {
                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {

                                    }
                                }

                                System.Threading.Thread.Sleep(500);

                                textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                                new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();

                                try
                                {
                                    while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed)
                                    {
                                        System.Threading.Thread.Sleep(500);
                                    }
                                }
                                catch
                                {

                                }

                                try
                                {
                                    while (driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "..." || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "\"" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == @"""" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains(" [...]  [...] "))
                                    {
                                        System.Threading.Thread.Sleep(500);
                                    }
                                }
                                catch
                                {

                                }
                            }
                            catch
                            {
                                try
                                {
                                    driver.FindElement(By.XPath("//div[@class=\"lmt__notification__blocked\"]"));

                                    driver.Close();
                                    driver.Quit();
                                    driver = null;

                                    //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                    cmdKill("geckodriver.exe");
                                    //if (Process.GetProcessesByName("firefox").Length > 0)
                                    cmdKill("firefox.exe");
                                    System.Threading.Thread.Sleep(500);
                                    deleteFile(Application.StartupPath + @"geckodriver.exe");
                                    System.Threading.Thread.Sleep(500);

                                    if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                        File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                    System.Threading.Thread.Sleep(500);

                                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                                    FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                    #region proxies
                                    if (portEnabled == 0)
                                    {
                                        //var myProxy = "login:pass@proxy:port";
                                        string port = "8110";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 1;
                                    }
                                    else if (portEnabled == 1)
                                    {
                                        string port = "8133";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 2;
                                    }
                                    else if (portEnabled == 2)
                                    {
                                        string port = "7300";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 3;
                                    }
                                    else if (portEnabled == 3)
                                    {
                                        string port = "8382";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 4;
                                    }
                                    else if (portEnabled == 4)
                                    {
                                        string port = "6286";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 5;
                                    }
                                    else if (portEnabled == 5)
                                    {
                                        string port = "6100";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 6;
                                    }
                                    else if (portEnabled == 6)
                                    {
                                        string port = "8279";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 7;
                                    }
                                    else if (portEnabled == 7)
                                    {
                                        string port = "6893";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 8;
                                    }
                                    else if (portEnabled == 8)
                                    {
                                        string port = "7492";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 9;
                                    }
                                    else if (portEnabled == 9)
                                    {
                                        string port = "8780";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 10;
                                    }
                                    else if (portEnabled == 10)
                                    {
                                        portEnabled = 0;

                                        MessageBox.Show(
                                            "Proxy is out",
                                            "Proxy 10",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1,
                                            MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                    #endregion

                                    firefoxService.HideCommandPromptWindow = true;
                                    firefoxOptions.AddArguments("--headless");

                                    driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                    {
                                        Url = "https://www.deepl.com/translator"
                                    };

                                    System.Threading.Thread.Sleep(5000);

                                    try
                                    {
                                        driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    catch
                                    {
                                    }

                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                    System.Threading.Thread.Sleep(500);

                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                        }
                                        catch
                                        {

                                        }
                                    }

                                    System.Threading.Thread.Sleep(500);

                                    //textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div")); //*[@id="panelTranslateText"]/div[3]/section[1]/div[3]/div[2]/textarea                                                                                             //textInputElement.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                                    //textInputElement.SendKeys(sourceText);
                                    textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                                    new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();

                                    try
                                    {
                                        while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed)
                                        {
                                            System.Threading.Thread.Sleep(500);
                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        while (driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "..." || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "\"" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == @"""" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains(" [...]  [...] "))
                                        {
                                            System.Threading.Thread.Sleep(500);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }

                    string result = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML");

                    int count = 0;
                    if (gameName == "Temtem")
                        count = squareQuoteValues.Count + triangQuoteValues.Count;
                    else if (gameName == "The Mortuary Assistant")
                        count = triangQuoteValues.Count;
                    else if (gameName == "NewWorld" || gameName == "DungeonCrawler")
                        count = triangQuoteValues.Count + curlyBracketValues.Count + squareQuoteValues.Count;
                    else if (gameName == "nwr")
                        count = curlyBracketValues.Count + triangQuoteValues.Count;

                    result = result.Replace("&lt;236 &gt;", "<236>");
                    result = result.Replace("&lt;216 &gt;", "<216>");
                    result = result.Replace("&lt; 236&gt;", "<236>");
                    result = result.Replace("&lt; 216&gt;", "<216>");
                    result = result.Replace("&lt; 236 &gt;", "<236>");
                    result = result.Replace("&lt; 216 &gt;", "<216>");
                    result = result.Replace("&lt;213 &gt;", "<213>");
                    result = result.Replace("&lt; 213&gt;", "<213>");
                    result = result.Replace("&lt; 213 &gt;", "<213>");
                    result = result.Replace("&lt;216&gt;", "<216>");
                    result = result.Replace("&lt;213&gt;", "<213>");
                    result = result.Replace("&lt;230 &gt;", "<230>");
                    result = result.Replace("&lt;231 &gt;", "<231>");
                    result = result.Replace("&lt;235 &gt;", "<235>");
                    result = result.Replace("&lt;234 &gt;", "<234>");
                    result = result.Replace("&lt; 230&gt;", "<230>");
                    result = result.Replace("&lt; 231&gt;", "<231>");
                    result = result.Replace("&lt; 235&gt;", "<235>");
                    result = result.Replace("&lt; 234&gt;", "<234>");
                    result = result.Replace("&lt; 230 &gt;", "<230>");
                    result = result.Replace("&lt; 231 &gt;", "<231>");
                    result = result.Replace("&lt; 235 &gt;", "<235>");
                    result = result.Replace("&lt; 234 &gt;", "<234>");
                    result = result.Replace("&lt;230&gt;", "<230>");
                    result = result.Replace("&lt;231&gt;", "<231>");
                    result = result.Replace("&lt;235&gt;", "<235>");
                    result = result.Replace("&lt;234&gt;", "<234>");

                    int translatedCount = result.Split("&gt;").Length - 1;
                    //int translatedSayingCount = result.Split(" - - ").Length - 1;

                    if (count != translatedCount) //if (count != translatedCount || saying3 != translatedSayingCount)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[1]/div[2]/div/div/div[1]/div/div/span/span/span/button")).Click();
                        }
                        catch
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"headlessui-popover-button-3\"]")).Click();
                            }
                            catch
                            {

                            }
                        }
                        System.Threading.Thread.Sleep(500);
                        for (int b = deeplInformal; b < 9999; b++)
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"headlessui-popover-panel-" + b.ToString() + "\"]/div/button[2]")).Click();
                                deeplInformal = b;
                                break;
                            }
                            catch
                            { }
                        }
                        System.Threading.Thread.Sleep(500);

                        try
                        {
                            int t = 0;

                            while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML").Length <= 4/*2*/ ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "" ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == " " ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "..." ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "\"" ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == @"""" ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML").Contains("[...]") ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed ||
                                driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "lt" ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "&lt;" ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "&lt" ||
                                driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML") == "<")
                            {
                                System.Threading.Thread.Sleep(500);
                                t = t + 1;

                                if (t > 30 && installBreak)
                                {
                                    break;
                                }
                                else if (t > 30 && !installBreak)
                                {
                                    driver.Close();
                                    driver.Quit();
                                    driver = null;

                                    //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                    cmdKill("geckodriver.exe");
                                    //if (Process.GetProcessesByName("firefox").Length > 0)
                                    cmdKill("firefox.exe");
                                    System.Threading.Thread.Sleep(500);
                                    deleteFile(Application.StartupPath + @"geckodriver.exe");
                                    System.Threading.Thread.Sleep(500);

                                    if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                        File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                    System.Threading.Thread.Sleep(500);

                                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                                    FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                                    #region proxies
                                    if (portEnabled == 0)
                                    {
                                        //var myProxy = "login:pass@proxy:port";
                                        string port = "8110";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 1;
                                    }
                                    else if (portEnabled == 1)
                                    {
                                        string port = "8133";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 2;
                                    }
                                    else if (portEnabled == 2)
                                    {
                                        string port = "7300";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 3;
                                    }
                                    else if (portEnabled == 3)
                                    {
                                        string port = "8382";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 4;
                                    }
                                    else if (portEnabled == 4)
                                    {
                                        string port = "6286";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 5;
                                    }
                                    else if (portEnabled == 5)
                                    {
                                        string port = "6100";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 6;
                                    }
                                    else if (portEnabled == 6)
                                    {
                                        string port = "8279";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 7;
                                    }
                                    else if (portEnabled == 7)
                                    {
                                        string port = "6893";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 8;
                                    }
                                    else if (portEnabled == 8)
                                    {
                                        string port = "7492";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 9;
                                    }
                                    else if (portEnabled == 9)
                                    {
                                        string port = "8780";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 10;
                                    }
                                    else if (portEnabled == 10)
                                    {
                                        portEnabled = 0;

                                        MessageBox.Show(
                                            "Proxy is out",
                                            "Proxy 10",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1,
                                            MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                    #endregion

                                    firefoxService.HideCommandPromptWindow = true;
                                    firefoxOptions.AddArguments("--headless");

                                    driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                    {
                                        Url = "https://www.deepl.com/translator"
                                    };

                                    System.Threading.Thread.Sleep(5000);

                                    try
                                    {
                                        driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    catch
                                    {
                                    }

                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                    System.Threading.Thread.Sleep(500);

                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                        }
                                        catch
                                        {

                                        }
                                    }

                                    System.Threading.Thread.Sleep(500);

                                    textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                                    new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();

                                    try
                                    {
                                        while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed)
                                        {
                                            System.Threading.Thread.Sleep(500);
                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        while (driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "..." || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "\"" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == @"""" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains(" [...]  [...] "))
                                        {
                                            System.Threading.Thread.Sleep(500);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                        catch
                        {

                        }

                        result = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[2]/div[3]/div[4]/ul/li/button")).GetAttribute("innerHTML");
                    }

                    translatedText = result.Split("____");

                    if (translatedText.Length != originalText.Length && driver == null)
                    {
                        MessageBox.Show(
                           "notification-too-many-requests\n" +
                           "Сообщите разработчику об этой ошибке.",
                           "Error",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1,
                           MessageBoxOptions.DefaultDesktopOnly);
                    }
                    if (translatedText.Length != originalText.Length && driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                        driver = null;

                        //if (Process.GetProcessesByName("geckodriver").Length > 0)
                        cmdKill("geckodriver.exe");
                        //if (Process.GetProcessesByName("firefox").Length > 0)
                        cmdKill("firefox.exe");
                        System.Threading.Thread.Sleep(500);
                        deleteFile(Application.StartupPath + @"geckodriver.exe");
                        System.Threading.Thread.Sleep(500);

                        BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, result.Length.ToString() + "result", Color.Black)));

                        if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                            File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                        System.Threading.Thread.Sleep(500);

                        FirefoxOptions firefoxOptions = new FirefoxOptions();
                        FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                        #region proxies
                        if (portEnabled == 0)
                        {
                            //var myProxy = "login:pass@proxy:port";
                            string port = "8110";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 1;
                        }
                        else if (portEnabled == 1)
                        {
                            string port = "8133";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 2;
                        }
                        else if (portEnabled == 2)
                        {
                            string port = "7300";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 3;
                        }
                        else if (portEnabled == 3)
                        {
                            string port = "8382";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 4;
                        }
                        else if (portEnabled == 4)
                        {
                            string port = "6286";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 5;
                        }
                        else if (portEnabled == 5)
                        {
                            string port = "6100";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 6;
                        }
                        else if (portEnabled == 6)
                        {
                            string port = "8279";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 7;
                        }
                        else if (portEnabled == 7)
                        {
                            string port = "6893";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 8;
                        }
                        else if (portEnabled == 8)
                        {
                            string port = "7492";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 9;
                        }
                        else if (portEnabled == 9)
                        {
                            string port = "8780";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 10;
                        }
                        else if (portEnabled == 10)
                        {
                            portEnabled = 0;

                            MessageBox.Show(
                                "Proxy is out",
                                "Proxy 10",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                        }
                        #endregion

                        firefoxService.HideCommandPromptWindow = true;
                        firefoxOptions.AddArguments("--headless");

                        driver = new FirefoxDriver(firefoxService, firefoxOptions)
                        {
                            Url = "https://www.deepl.com/translator"
                        };

                        System.Threading.Thread.Sleep(5000);

                        try
                        {
                            driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                            System.Threading.Thread.Sleep(500);
                        }
                        catch
                        {
                        }

                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                        System.Threading.Thread.Sleep(500);

                        try
                        {
                            var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                        }
                        catch
                        {
                            try
                            {
                                var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(clearButton).Click().Perform();
                            }
                            catch
                            {

                            }
                        }

                        System.Threading.Thread.Sleep(500);

                        if (driver != null)
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]"));

                                driver.Close();
                                driver.Quit();
                                driver = null;

                                //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                cmdKill("geckodriver.exe");
                                //if (Process.GetProcessesByName("firefox").Length > 0)
                                cmdKill("firefox.exe");
                                System.Threading.Thread.Sleep(500);
                                deleteFile(Application.StartupPath + @"geckodriver.exe");
                                System.Threading.Thread.Sleep(500);

                                if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                    File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                System.Threading.Thread.Sleep(500);

                                firefoxOptions = new FirefoxOptions();
                                firefoxService = FirefoxDriverService.CreateDefaultService();

                                #region proxies
                                if (portEnabled == 0)
                                {
                                    //var myProxy = "login:pass@proxy:port";
                                    string port = "8110";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 1;
                                }
                                else if (portEnabled == 1)
                                {
                                    string port = "8133";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 2;
                                }
                                else if (portEnabled == 2)
                                {
                                    string port = "7300";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 3;
                                }
                                else if (portEnabled == 3)
                                {
                                    string port = "8382";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 4;
                                }
                                else if (portEnabled == 4)
                                {
                                    string port = "6286";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 5;
                                }
                                else if (portEnabled == 5)
                                {
                                    string port = "6100";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 6;
                                }
                                else if (portEnabled == 6)
                                {
                                    string port = "8279";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 7;
                                }
                                else if (portEnabled == 7)
                                {
                                    string port = "6893";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 8;
                                }
                                else if (portEnabled == 8)
                                {
                                    string port = "7492";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 9;
                                }
                                else if (portEnabled == 9)
                                {
                                    string port = "8780";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = 10;
                                }
                                else if (portEnabled == 10)
                                {
                                    portEnabled = 0;

                                    MessageBox.Show(
                                        "Proxy is out",
                                        "Proxy 10",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                                }
                                #endregion

                                firefoxService.HideCommandPromptWindow = true;
                                firefoxOptions.AddArguments("--headless");

                                driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                {
                                    Url = "https://www.deepl.com/translator"
                                };

                                System.Threading.Thread.Sleep(5000);

                                try
                                {
                                    driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                }
                                catch
                                {
                                }

                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                System.Threading.Thread.Sleep(500);

                                try
                                {
                                    var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                }
                                catch
                                {
                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            catch
                            {
                                try
                                {
                                    driver.FindElement(By.XPath("//div[@id=\"headlessui-dialog-title-6\"]"));

                                    driver.Close();
                                    driver.Quit();
                                    driver = null;

                                    //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                    cmdKill("geckodriver.exe");
                                    //if (Process.GetProcessesByName("firefox").Length > 0)
                                    cmdKill("firefox.exe");
                                    System.Threading.Thread.Sleep(500);
                                    deleteFile(Application.StartupPath + @"geckodriver.exe");
                                    System.Threading.Thread.Sleep(500);

                                    if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                        File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                    System.Threading.Thread.Sleep(500);

                                    firefoxOptions = new FirefoxOptions();
                                    firefoxService = FirefoxDriverService.CreateDefaultService();

                                    #region proxies
                                    if (portEnabled == 0)
                                    {
                                        //var myProxy = "login:pass@proxy:port";
                                        string port = "8110";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 1;
                                    }
                                    else if (portEnabled == 1)
                                    {
                                        string port = "8133";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 2;
                                    }
                                    else if (portEnabled == 2)
                                    {
                                        string port = "7300";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 3;
                                    }
                                    else if (portEnabled == 3)
                                    {
                                        string port = "8382";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 4;
                                    }
                                    else if (portEnabled == 4)
                                    {
                                        string port = "6286";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 5;
                                    }
                                    else if (portEnabled == 5)
                                    {
                                        string port = "6100";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 6;
                                    }
                                    else if (portEnabled == 6)
                                    {
                                        string port = "8279";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 7;
                                    }
                                    else if (portEnabled == 7)
                                    {
                                        string port = "6893";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 8;
                                    }
                                    else if (portEnabled == 8)
                                    {
                                        string port = "7492";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 9;
                                    }
                                    else if (portEnabled == 9)
                                    {
                                        string port = "8780";
                                        string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                        firefoxOptions.SetPreference("network.proxy.type", 1);
                                        firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                        firefoxOptions.SetPreference("network.proxy.http_port", port);

                                        portEnabled = 10;
                                    }
                                    else if (portEnabled == 10)
                                    {
                                        portEnabled = 0;

                                        MessageBox.Show(
                                            "Proxy is out",
                                            "Proxy 10",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1,
                                            MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                    #endregion

                                    firefoxService.HideCommandPromptWindow = true;
                                    firefoxOptions.AddArguments("--headless");

                                    driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                    {
                                        Url = "https://www.deepl.com/translator"
                                    };

                                    System.Threading.Thread.Sleep(5000);

                                    try
                                    {
                                        driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    catch
                                    {
                                    }

                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                    System.Threading.Thread.Sleep(500);

                                    try
                                    {
                                        var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                        new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                                catch
                                {
                                    try
                                    {
                                        driver.FindElement(By.XPath("//div[@class=\"lmt__notification__blocked\"]"));

                                        driver.Close();
                                        driver.Quit();
                                        driver = null;

                                        //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                        cmdKill("geckodriver.exe");
                                        //if (Process.GetProcessesByName("firefox").Length > 0)
                                        cmdKill("firefox.exe");
                                        System.Threading.Thread.Sleep(500);
                                        deleteFile(Application.StartupPath + @"geckodriver.exe");
                                        System.Threading.Thread.Sleep(500);

                                        if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                            File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                        System.Threading.Thread.Sleep(500);

                                        firefoxOptions = new FirefoxOptions();
                                        firefoxService = FirefoxDriverService.CreateDefaultService();

                                        #region proxies
                                        if (portEnabled == 0)
                                        {
                                            //var myProxy = "login:pass@proxy:port";
                                            string port = "8110";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 1;
                                        }
                                        else if (portEnabled == 1)
                                        {
                                            string port = "8133";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 2;
                                        }
                                        else if (portEnabled == 2)
                                        {
                                            string port = "7300";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 3;
                                        }
                                        else if (portEnabled == 3)
                                        {
                                            string port = "8382";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 4;
                                        }
                                        else if (portEnabled == 4)
                                        {
                                            string port = "6286";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 5;
                                        }
                                        else if (portEnabled == 5)
                                        {
                                            string port = "6100";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 6;
                                        }
                                        else if (portEnabled == 6)
                                        {
                                            string port = "8279";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 7;
                                        }
                                        else if (portEnabled == 7)
                                        {
                                            string port = "6893";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 8;
                                        }
                                        else if (portEnabled == 8)
                                        {
                                            string port = "7492";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 9;
                                        }
                                        else if (portEnabled == 9)
                                        {
                                            string port = "8780";
                                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                            firefoxOptions.SetPreference("network.proxy.type", 1);
                                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                                            portEnabled = 10;
                                        }
                                        else if (portEnabled == 10)
                                        {
                                            portEnabled = 0;

                                            MessageBox.Show(
                                                "Proxy is out",
                                                "Proxy 10",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error,
                                                MessageBoxDefaultButton.Button1,
                                                MessageBoxOptions.DefaultDesktopOnly);
                                        }
                                        #endregion

                                        firefoxService.HideCommandPromptWindow = true;
                                        firefoxOptions.AddArguments("--headless");

                                        driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                        {
                                            Url = "https://www.deepl.com/translator"
                                        };

                                        System.Threading.Thread.Sleep(5000);

                                        try
                                        {
                                            driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                                            System.Threading.Thread.Sleep(500);
                                        }
                                        catch
                                        {
                                        }

                                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                                        System.Threading.Thread.Sleep(500);
                                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                                        System.Threading.Thread.Sleep(500);

                                        try
                                        {
                                            var clearButton = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[1]/div[2]/div/span/span/span/button"));
                                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                            new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                var clearButton = driver.FindElement(By.XPath("//*[@dl-test=\"translator-source-clear-button\"]"));
                                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                                new Actions(driver).MoveToElement(clearButton).Click().Perform();
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }

                        textInputElement = driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[1]/div[2]/section[1]/div[3]/div[2]/d-textarea/div"));
                        new Actions(driver).MoveToElement(textInputElement).Click().SendKeys(sourceText).Perform();

                        try
                        {
                            while (driver.FindElement(By.XPath("//*[@id=\"panelTranslateText\"]/div[3]/section[2]/div[3]/div[3]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translator-progress-close\"]")).Displayed)
                            {
                                System.Threading.Thread.Sleep(500);
                            }
                        }
                        catch
                        {

                        }

                        try
                        {
                            while (driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "..." || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == "\"" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML") == @"""" || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains("[...]  [...] ") || driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML").Contains(" [...]  [...] "))
                            {
                                System.Threading.Thread.Sleep(500);
                            }
                        }
                        catch
                        {

                        }

                        System.Threading.Thread.Sleep(500);

                        result = driver.FindElement(By.XPath("//div[@id=\"target-dummydiv\"]")).GetAttribute("innerHTML");

                        translatedText = result.Split("____");
                    }
                }
                #region googleButton.Checked
                /*else if (googleButton.Checked)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                    System.Threading.Thread.Sleep(500);
                    var clearButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                    clearButton.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                    clearButton.SendKeys(OpenQA.Selenium.Keys.Delete);
                    System.Threading.Thread.Sleep(500);

                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]")); //need change !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            if (!portEnabled)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "9223";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@193.8.56.159:9223";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = true;
                            }
                            else
                            {
                                portEnabled = false;
                            }

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.google.com"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[2]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[1]/div/div[3]/div/div[3]/div[6]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[5]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                var ruButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[2]/div/div[3]/div/div[2]/div[89]"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(ruButton).Click().Perform();
                            }
                            catch
                            {

                            }

                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                            System.Threading.Thread.Sleep(500);
                            clearButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Delete);
                            System.Threading.Thread.Sleep(500);
                        }
                        catch
                        {
                            
                        }
                    }

                    //BeginInvoke((MethodInvoker)(() => Clipboard.SetText(sourceText)));
                    var textInputElement = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                    //new Actions(driver).MoveToElement(textInputElement).Click().Perform();
                    textInputElement.SendKeys(sourceText);

                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//div[@id=\"headlessui-dialog-title-6\"]")); // "//*[@dl-test=\"notification-too-many-requests\"]" //need change !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            if (!portEnabled)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "9223";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@193.8.56.159:9223";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = true;
                            }
                            else
                            {
                                portEnabled = false;
                            }

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.google.com"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[2]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[1]/div/div[3]/div/div[3]/div[6]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[5]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                var ruButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[2]/div/div[3]/div/div[2]/div[89]"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(ruButton).Click().Perform();
                            }
                            catch
                            {

                            }

                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                            System.Threading.Thread.Sleep(500);
                            clearButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Delete);
                            System.Threading.Thread.Sleep(500);

                            textInputElement = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                            //new Actions(driver).MoveToElement(textInputElement).Click().Perform();
                            textInputElement.SendKeys(sourceText);
                        }
                        catch
                        {
                            
                        }
                    }

                    while (driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[1]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[2]")).Displayed)
                    {
                        System.Threading.Thread.Sleep(500);
                    }

                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]")); //need change !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            if (!portEnabled)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "9223";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@193.8.56.159:9223";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = true;
                            }
                            else
                            {
                                portEnabled = false;
                            }

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.google.com"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[2]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[1]/div/div[3]/div/div[3]/div[6]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[5]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                var ruButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[2]/div/div[3]/div/div[2]/div[89]"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(ruButton).Click().Perform();
                            }
                            catch
                            {

                            }

                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                            System.Threading.Thread.Sleep(500);
                            clearButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Delete);
                            System.Threading.Thread.Sleep(500);

                            textInputElement = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                            //new Actions(driver).MoveToElement(textInputElement).Click().Perform();
                            textInputElement.SendKeys(sourceText);

                            while (driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[1]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[2]")).Displayed)
                            {
                                System.Threading.Thread.Sleep(500);
                            }
                        }
                        catch
                        {
                            
                        }
                    }

                    System.Threading.Thread.Sleep(500);

                    try
                    {
                        driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[8]/div/div[1]/span[1]"));
                    }
                    catch
                    {
                        try
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                            System.Threading.Thread.Sleep(500);
                            var tryAgainButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[6]/div[2]/button"));
                            new Actions(driver).MoveToElement(tryAgainButton).Click().Perform();
                            System.Threading.Thread.Sleep(500);
                            while (driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[1]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[2]")).Displayed)
                            {
                                System.Threading.Thread.Sleep(500);
                            }
                        }
                        catch
                        {
                        }
                    }

                    string result = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[8]/div/div[1]/span[1]")).Text;
                    result = result.Replace("notranslate", "");
                    result = result.Replace("&lt;", "<");
                    result = result.Replace("&gt;", ">");
                    result = result.Replace("<218>", "<br>");

                    translatedText = result.Split("____");

                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]")); //need change !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            if (!portEnabled)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "9223";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@193.8.56.159:9223";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = true;
                            }
                            else
                            {
                                portEnabled = false;
                            }

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.google.com"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[2]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[1]/div/div[3]/div/div[3]/div[6]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[5]/button/div[3]")).Click();
                                System.Threading.Thread.Sleep(500);
                                var ruButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[2]/div/div[3]/div/div[2]/div[89]"));
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                new Actions(driver).MoveToElement(ruButton).Click().Perform();
                            }
                            catch
                            {

                            }

                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                            System.Threading.Thread.Sleep(500);
                            clearButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                            clearButton.SendKeys(OpenQA.Selenium.Keys.Delete);
                            System.Threading.Thread.Sleep(500);

                            textInputElement = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                            //new Actions(driver).MoveToElement(textInputElement).Click().Perform();
                            textInputElement.SendKeys(sourceText);

                            while (driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[1]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[2]")).Displayed)
                            {
                                System.Threading.Thread.Sleep(500);
                            }

                            result = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[8]/div/div[1]/span[1]")).Text;
                            result = result.Replace("notranslate", "");
                            result = result.Replace("&lt;", "<");
                            result = result.Replace("&gt;", ">");

                            translatedText = result.Split("____");
                        }
                        catch
                        {
                            
                        }
                    }

                    if (translatedText.Length != originalText.Length && driver == null)
                    {
                        MessageBox.Show(
                           "notification-too-many-requests\n" +
                           "Сообщите разработчику об этой ошибке.",
                           "Error",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1,
                           MessageBoxOptions.DefaultDesktopOnly);
                    }
                    if (translatedText.Length != originalText.Length && driver != null)
                    {
                        MessageBox.Show(
                           "translatedText.Length != originalText.Length && driver != null",
                           "Error",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1,
                           MessageBoxOptions.DefaultDesktopOnly); // не забыть убрать !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        driver.Close();
                        driver.Quit();
                        driver = null;

                        //if (Process.GetProcessesByName("geckodriver").Length > 0)
                        cmdKill("geckodriver.exe");
                        //if (Process.GetProcessesByName("firefox").Length > 0)
                        cmdKill("firefox.exe");
                        System.Threading.Thread.Sleep(500);
                        deleteFile(Application.StartupPath + @"geckodriver.exe");
                        System.Threading.Thread.Sleep(500);

                        if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                            File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                        System.Threading.Thread.Sleep(500);

                        FirefoxOptions firefoxOptions = new FirefoxOptions();
                        FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                        if (!portEnabled)
                        {
                            //var myProxy = "login:pass@proxy:port";
                            string port = "9223";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@193.8.56.159:9223";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = true;
                        }
                        else
                        {
                            portEnabled = false;
                        }

                        firefoxService.HideCommandPromptWindow = true;
                        firefoxOptions.AddArguments("--headless");

                        driver = new FirefoxDriver(firefoxService, firefoxOptions)
                        {
                            Url = "https://translate.google.com"
                        };

                        System.Threading.Thread.Sleep(5000);

                        try
                        {
                            driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[2]/button/div[3]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[1]/div/div[3]/div/div[3]/div[6]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[5]/button/div[3]")).Click();
                            System.Threading.Thread.Sleep(500);
                            var ruButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[2]/div/div[3]/div/div[2]/div[89]"));
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                            new Actions(driver).MoveToElement(ruButton).Click().Perform();
                        }
                        catch
                        {

                        }

                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                        System.Threading.Thread.Sleep(500);
                        clearButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/п"));
                        clearButton.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                        clearButton.SendKeys(OpenQA.Selenium.Keys.Delete);
                        System.Threading.Thread.Sleep(500);

                        if (driver != null)
                        {
                            try
                            {
                                driver.FindElement(By.XPath("//*[@dl-test=\"notification-too-many-requests\"]")); //need change !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                                driver.Close();
                                driver.Quit();
                                driver = null;

                                //if (Process.GetProcessesByName("geckodriver").Length > 0)
                                cmdKill("geckodriver.exe");
                                //if (Process.GetProcessesByName("firefox").Length > 0)
                                cmdKill("firefox.exe");
                                System.Threading.Thread.Sleep(500);
                                deleteFile(Application.StartupPath + @"geckodriver.exe");
                                System.Threading.Thread.Sleep(500);

                                if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                    File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                                System.Threading.Thread.Sleep(500);

                                firefoxOptions = new FirefoxOptions();
                                firefoxService = FirefoxDriverService.CreateDefaultService();

                                if (!portEnabled)
                                {
                                    //var myProxy = "login:pass@proxy:port";
                                    string port = "9223";
                                    string myProxy = "fcetxjzg:lrdyxa7zqqcf@193.8.56.159:9223";
                                    firefoxOptions.SetPreference("network.proxy.type", 1);
                                    firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                    firefoxOptions.SetPreference("network.proxy.http_port", port);

                                    portEnabled = true;
                                }
                                else
                                {
                                    portEnabled = false;
                                }

                                firefoxService.HideCommandPromptWindow = true;
                                firefoxOptions.AddArguments("--headless");

                                driver = new FirefoxDriver(firefoxService, firefoxOptions)
                                {
                                    Url = "https://translate.google.com"
                                };

                                System.Threading.Thread.Sleep(5000);

                                try
                                {
                                    driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[2]/button/div[3]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[1]/div/div[3]/div/div[3]/div[6]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[5]/button/div[3]")).Click();
                                    System.Threading.Thread.Sleep(500);
                                    var ruButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[2]/div/div[3]/div/div[2]/div[89]"));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                                    new Actions(driver).MoveToElement(ruButton).Click().Perform();
                                }
                                catch
                                {

                                }

                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");
                                System.Threading.Thread.Sleep(500);
                                clearButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                                clearButton.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                                clearButton.SendKeys(OpenQA.Selenium.Keys.Delete);
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                                
                            }
                        }

                        textInputElement = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[1]/span/span/div/textarea"));
                        //new Actions(driver).MoveToElement(textInputElement).Click().Perform();
                        textInputElement.SendKeys(sourceText);

                        while (driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[1]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[2]")).Displayed)
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        result = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[8]/div/div[1]/span[1]")).Text;
                        result = result.Replace("notranslate", "");
                        result = result.Replace("&lt;", "<");
                        result = result.Replace("&gt;", ">");

                        translatedText = result.Split("____");
                    }
                }*/
                #endregion
                #region yandexButton.Checked
                else if (yandexButton.Checked)
                {
                    //if (gameName == "Temtem" || gameName == "NewWorld") // потому что яндекс почему-то одну [] в тексте вставляет в переведенный текст как две...
                    //{
                    //    sourceText = sourceText.Replace("[]", "/|");
                    //}

                    try
                    {
                        driver.FindElement(By.XPath("//*[@id=\"clearButton\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                    }
                    catch
                    {
                    }

                    if (driver != null)
                    {
                        try
                        {
                            #region yandex captcha
                            driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/form"));

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            firefoxService.HideCommandPromptWindow = true;

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.yandex.ru"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div[3]/div/div")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"settingOptions\"]/div[1]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"srcLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[5]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"dstLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[68]")).Click();
                            }
                            catch
                            {
                            }

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"clearButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                            }

                            MessageBox.Show(
                                "Введите каптчу в браузере Firefox, чтобы продолжить русификацию.\n" +
                                @"Нажмите кнопку ""Ок"" только ПОСЛЕ того, как введете каптчу!",
                                "Внимание!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);

                            MessageBox.Show(
                                "Вторая проверка на всякий случай, Вы точно ввели каптчу?",
                                "Внимание! #2",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            #endregion
                            System.Threading.Thread.Sleep(500);

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            firefoxOptions = new FirefoxOptions();
                            firefoxService = FirefoxDriverService.CreateDefaultService();

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");//должно быть всегда активно, не закомменчено

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.yandex.ru"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div[3]/div/div")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"settingOptions\"]/div[1]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"srcLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[5]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"dstLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[68]")).Click();
                            }
                            catch
                            {
                            }

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"clearButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (driver != null)
                    {
                        try
                        {
                            driver.FindElement(By.XPath("//*[@id=\"textarea\"]"));
                        }
                        catch
                        {
                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            #region proxies
                            if (portEnabled == 0)
                            {
                                //var myProxy = "login:pass@proxy:port";
                                string port = "8110";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 1;
                            }
                            else if (portEnabled == 1)
                            {
                                string port = "8133";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 2;
                            }
                            else if (portEnabled == 2)
                            {
                                string port = "7300";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 3;
                            }
                            else if (portEnabled == 3)
                            {
                                string port = "8382";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 4;
                            }
                            else if (portEnabled == 4)
                            {
                                string port = "6286";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 5;
                            }
                            else if (portEnabled == 5)
                            {
                                string port = "6100";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 6;
                            }
                            else if (portEnabled == 6)
                            {
                                string port = "8279";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 7;
                            }
                            else if (portEnabled == 7)
                            {
                                string port = "6893";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 8;
                            }
                            else if (portEnabled == 8)
                            {
                                string port = "7492";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 9;
                            }
                            else if (portEnabled == 9)
                            {
                                string port = "8780";
                                string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                                firefoxOptions.SetPreference("network.proxy.type", 1);
                                firefoxOptions.SetPreference("network.proxy.http", myProxy);
                                firefoxOptions.SetPreference("network.proxy.http_port", port);

                                portEnabled = 10;
                            }
                            else if (portEnabled == 10)
                            {
                                portEnabled = 0;

                                MessageBox.Show(
                                    "Proxy is out",
                                    "Proxy 10",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                            }
                            #endregion

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.yandex.ru"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div[3]/div/div")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"settingOptions\"]/div[1]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"srcLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[5]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"dstLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[68]")).Click();
                            }
                            catch
                            {
                            }

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"clearButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                            }
                        }
                    }
                    var textInputElement = driver.FindElement(By.XPath("//*[@id=\"textarea\"]"));
                    textInputElement.SendKeys(sourceText);

                    if (driver != null)
                    {
                        try
                        {
                            #region yandex captcha
                            driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/form"));

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                            firefoxService.HideCommandPromptWindow = true;

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.yandex.ru"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div[3]/div/div")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"settingOptions\"]/div[1]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"srcLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[5]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"dstLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[68]")).Click();
                            }
                            catch
                            {
                            }

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"clearButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                            }

                            MessageBox.Show(
                                "Введите каптчу в браузере Firefox, чтобы продолжить русификацию.\n" +
                                @"Нажмите кнопку ""Ок"" только ПОСЛЕ того, как введете каптчу!",
                                "Внимание!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);

                            MessageBox.Show(
                                "Вторая проверка на всякий случай, Вы точно ввели каптчу?",
                                "Внимание! #2",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            #endregion
                            System.Threading.Thread.Sleep(500);

                            driver.Close();
                            driver.Quit();
                            driver = null;

                            //if (Process.GetProcessesByName("geckodriver").Length > 0)
                            cmdKill("geckodriver.exe");
                            //if (Process.GetProcessesByName("firefox").Length > 0)
                            cmdKill("firefox.exe");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(Application.StartupPath + @"geckodriver.exe");
                            System.Threading.Thread.Sleep(500);

                            if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                                File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                            System.Threading.Thread.Sleep(500);

                            firefoxOptions = new FirefoxOptions();
                            firefoxService = FirefoxDriverService.CreateDefaultService();

                            firefoxService.HideCommandPromptWindow = true;
                            firefoxOptions.AddArguments("--headless");//должно быть всегда активно, не закомменчено

                            driver = new FirefoxDriver(firefoxService, firefoxOptions)
                            {
                                Url = "https://translate.yandex.ru"
                            };

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div[3]/div/div")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"settingOptions\"]/div[1]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"srcLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[5]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"dstLangButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                                driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[68]")).Click();
                            }
                            catch
                            {
                            }

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"clearButton\"]")).Click();
                                System.Threading.Thread.Sleep(500);
                            }
                            catch
                            {
                            }
                        }
                        catch
                        {
                        }
                    }

                    System.Threading.Thread.Sleep(2000);

                    try
                    {
                        while (driver.FindElement(By.XPath("//*[@id=\"textbox2\"]/div[1]/div[2]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"textbox2\"]/div[1]/div[2]/span")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translation\"]")).Text == "")
                        {
                            System.Threading.Thread.Sleep(500);

                            try
                            {
                                driver.FindElement(By.XPath("//*[@id=\"textbox2\"]/div[3]/div[1]/button")).Click(); //*[@id="textbox2"]/div[3]/div[1]/button (повторить попытку)
                                System.Threading.Thread.Sleep(500);

                            }
                            catch
                            { }
                        }
                    }
                    catch
                    {

                    }

                    string result = driver.FindElement(By.XPath("//*[@id=\"translation\"]")).Text;
                    result = result.Replace("nodonttranslate", "");
                    result = result.Replace("&lt;", "<");
                    result = result.Replace("&gt;", ">");

                    //if (gameName == "Temtem" || gameName == "NewWorld") // потому что яндекс почему-то одну [] в тексте вставляет в переведенный текст как две...
                    //{
                    //    result = result.Replace("/|", "[]");
                    //}

                    translatedText = result.Split("____");

                    if (translatedText.Length != originalText.Length && driver == null)
                    {
                        MessageBox.Show(
                           "notification-too-many-requests\n" +
                           "Сообщите разработчику об этой ошибке.",
                           "Error",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1,
                           MessageBoxOptions.DefaultDesktopOnly);
                    }
                    if (translatedText.Length != originalText.Length && driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                        driver = null;

                        //if (Process.GetProcessesByName("geckodriver").Length > 0)
                        cmdKill("geckodriver.exe");
                        //if (Process.GetProcessesByName("firefox").Length > 0)
                        cmdKill("firefox.exe");
                        System.Threading.Thread.Sleep(500);
                        deleteFile(Application.StartupPath + @"geckodriver.exe");
                        System.Threading.Thread.Sleep(500);

                        if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                            File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                        System.Threading.Thread.Sleep(500);

                        FirefoxOptions firefoxOptions = new FirefoxOptions();
                        FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                        #region proxies
                        if (portEnabled == 0)
                        {
                            //var myProxy = "login:pass@proxy:port";
                            string port = "8110";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.94.47.66:8110";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 1;
                        }
                        else if (portEnabled == 1)
                        {
                            string port = "8133";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@45.155.68.129:8133";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 2;
                        }
                        else if (portEnabled == 2)
                        {
                            string port = "7300";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.228.220:7300";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 3;
                        }
                        else if (portEnabled == 3)
                        {
                            string port = "8382";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.231.45:8382";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 4;
                        }
                        else if (portEnabled == 4)
                        {
                            string port = "6286";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.207:6286";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 5;
                        }
                        else if (portEnabled == 5)
                        {
                            string port = "6100";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.210.21:6100";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 6;
                        }
                        else if (portEnabled == 6)
                        {
                            string port = "8279";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@188.74.183.10:8279";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 7;
                        }
                        else if (portEnabled == 7)
                        {
                            string port = "6893";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@154.95.36.199:6893";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 8;
                        }
                        else if (portEnabled == 8)
                        {
                            string port = "7492";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@185.199.229.156:7492";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 9;
                        }
                        else if (portEnabled == 9)
                        {
                            string port = "8780";
                            string myProxy = "fcetxjzg:lrdyxa7zqqcf@144.168.217.88:8780";
                            firefoxOptions.SetPreference("network.proxy.type", 1);
                            firefoxOptions.SetPreference("network.proxy.http", myProxy);
                            firefoxOptions.SetPreference("network.proxy.http_port", port);

                            portEnabled = 10;
                        }
                        else if (portEnabled == 10)
                        {
                            portEnabled = 0;

                            MessageBox.Show(
                                "Proxy is out",
                                "Proxy 10",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                        }
                        #endregion

                        firefoxService.HideCommandPromptWindow = true;
                        firefoxOptions.AddArguments("--headless");

                        driver = new FirefoxDriver(firefoxService, firefoxOptions)
                        {
                            Url = "https://translate.yandex.ru"
                        };

                        System.Threading.Thread.Sleep(5000);

                        try
                        {
                            driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div[3]/div/div")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@id=\"settingOptions\"]/div[1]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@id=\"srcLangButton\"]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[5]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@id=\"dstLangButton\"]")).Click();
                            System.Threading.Thread.Sleep(500);
                            driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[68]")).Click();
                        }
                        catch
                        {
                        }

                        try
                        {
                            driver.FindElement(By.XPath("//*[@id=\"clearButton\"]")).Click();
                            System.Threading.Thread.Sleep(500);
                        }
                        catch
                        {
                        }

                        textInputElement = driver.FindElement(By.XPath("//*[@id=\"textarea\"]"));
                        textInputElement.SendKeys(sourceText);

                        while (driver.FindElement(By.XPath("//*[@id=\"textbox2\"]/div[1]/div[2]")).Displayed || driver.FindElement(By.XPath("//*[@id=\"textbox2\"]/div[1]/div[2]/span")).Displayed || driver.FindElement(By.XPath("//*[@id=\"translation\"]")).Text == "")
                        {
                            System.Threading.Thread.Sleep(500);
                        }

                        result = driver.FindElement(By.XPath("//*[@id=\"translation\"]")).Text;
                        result = result.Replace("nodonttranslate", "");
                        result = result.Replace("&lt;", "<");
                        result = result.Replace("&gt;", ">");

                        translatedText = result.Split("____");
                    }
                }
                #endregion

                for (int i = 0; i < translatedText.Length; i++)
                {
                    originalText[i] = originalText[i].Trim();
                    translatedText[i] = translatedText[i].Trim();
                    translatedText[i] = translatedText[i].Replace("&lt;", "<");
                    translatedText[i] = translatedText[i].Replace("&gt;", ">");
                    translatedText[i] = translatedText[i].Replace(@".\n\b", @".\""");
                    translatedText[i] = translatedText[i].Replace(@"\\\n", @"\n");

                    translatedText[i] = translatedText[i].Replace("<236 >", "<236>");
                    translatedText[i] = translatedText[i].Replace("<216 >", "<216>");
                    translatedText[i] = translatedText[i].Replace("< 236>", "<236>");
                    translatedText[i] = translatedText[i].Replace("< 216>", "<216>");
                    translatedText[i] = translatedText[i].Replace("< 236 >", "<236>");
                    translatedText[i] = translatedText[i].Replace("< 216 >", "<216>");
                    translatedText[i] = translatedText[i].Replace("<236>_", @"\n\n");
                    translatedText[i] = translatedText[i].Replace("<216>_", @"\n");

                    translatedText[i] = translatedText[i].Replace("<213 >", "<213>");
                    translatedText[i] = translatedText[i].Replace("< 213>", "<213>");
                    translatedText[i] = translatedText[i].Replace("< 213 >", "<213>");
                    translatedText[i] = translatedText[i].Replace("<213>_", "pansuns");

                    translatedText[i] = translatedText[i].Replace("<230 >", "<230>");
                    translatedText[i] = translatedText[i].Replace("<231 >", "<231>");
                    translatedText[i] = translatedText[i].Replace("<235 >", "<235>");
                    translatedText[i] = translatedText[i].Replace("<234 >", "<234>");
                    translatedText[i] = translatedText[i].Replace("< 230>", "<230>");
                    translatedText[i] = translatedText[i].Replace("< 231>", "<231>");
                    translatedText[i] = translatedText[i].Replace("< 235>", "<235>");
                    translatedText[i] = translatedText[i].Replace("< 234>", "<234>");
                    translatedText[i] = translatedText[i].Replace("< 230 >", "<230>");
                    translatedText[i] = translatedText[i].Replace("< 231 >", "<231>");
                    translatedText[i] = translatedText[i].Replace("< 235 >", "<235>");
                    translatedText[i] = translatedText[i].Replace("< 234 >", "<234>");

                    if (gameName == "Temtem" || gameName == "Escape Simulator")
                    {
                        translatedText[i] = translatedText[i].Replace(@"<4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"<3 >", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 4>", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3>", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3 >", "<3>");

                        translatedText[i] = translatedText[i].Replace(@"<4>_", "[]");
                        if (yandexButton.Checked)
                            translatedText[i] = translatedText[i].Replace(@"<3>_", "<>");
                        else
                            translatedText[i] = translatedText[i].Replace(@"<3>", "<>");

                        if (translatedText[i].Contains("[") && translatedText[i].Contains("]"))
                        {
                            if (squareQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("[").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf("]", t + 1);
                                        //if (translatedText[i][t] == ']' && translatedText[i].IndexOf("[", t) == -1)
                                        if (translatedText[i].IndexOf("[", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("[", t);
                                    int end = translatedText[i].IndexOf("]", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "" && originalText[i] != "\"!?'.,;:()<>/\\-+=#_&~*¡¿%ßÑñÇçčÆæŒœÀÁÂÃÄÅÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜŸàáâãäåąèéêëęìíîïñòóôõöśùúûüÿ[]▲…`´¨ºª^@¬|¹²•")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, squareQuoteValues[0]);
                                        squareQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (translatedText[i].Contains("<") && translatedText[i].Contains(">"))
                        {
                            if (triangQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("<").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf(">", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("<", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("<", t);
                                    int end = translatedText[i].IndexOf(">", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "" && originalText[i] != "\"!?'.,;:()<>/\\-+=#_&~*¡¿%ßÑñÇçčÆæŒœÀÁÂÃÄÅÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜŸàáâãäåąèéêëęìíîïñòóôõöśùúûüÿ[]▲…`´¨ºª^@¬|¹²•")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, triangQuoteValues[0]);
                                        triangQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }
                    }
                    else if (gameName == "The Mortuary Assistant")
                    {
                        translatedText[i] = translatedText[i].Replace(@"<3 >", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 3>", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 3 >", "<3>");

                        if (yandexButton.Checked)
                            translatedText[i] = translatedText[i].Replace(@"<3>_", "<>");
                        else
                            translatedText[i] = translatedText[i].Replace(@"<3>", "<>");

                        if (translatedText[i].Contains("<") && translatedText[i].Contains(">"))
                        {
                            if (triangQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("<").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf(">", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("<", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("<", t);
                                    int end = translatedText[i].IndexOf(">", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "" && originalText[i] != "\"!?'.,;:()<>/\\-+=#_&~*¡¿%ßÑñÇçčÆæŒœÀÁÂÃÄÅÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜŸàáâãäåąèéêëęìíîïñòóôõöśùúûüÿ[]▲…`´¨ºª^@¬|¹²•")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, triangQuoteValues[0]);
                                        triangQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }
                    }
                    else if (gameName == "NewWorld")
                    {
                        translatedText[i] = translatedText[i].Replace(@"<6 >", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"<4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"<3 >", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 6>", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"< 4>", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3>", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 6 >", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"< 4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3 >", "<3>");

                        translatedText[i] = translatedText[i].Replace(@"<6>_", "{}");
                        translatedText[i] = translatedText[i].Replace(@"<4>_", "[]");
                        if (yandexButton.Checked)
                            translatedText[i] = translatedText[i].Replace(@"<3>_", "<>");
                        else
                            translatedText[i] = translatedText[i].Replace(@"<3>", "<>");

                        if (translatedText[i].Contains("[") && translatedText[i].Contains("]"))
                        {
                            if (squareQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("[").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf("]", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("[", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("[", t);
                                    int end = translatedText[i].IndexOf("]", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, squareQuoteValues[0]);
                                        squareQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (translatedText[i].Contains("<") && translatedText[i].Contains(">"))
                        {
                            if (triangQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("<").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf(">", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("<", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("<", t);
                                    int end = translatedText[i].IndexOf(">", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, triangQuoteValues[0]);
                                        triangQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (translatedText[i].Contains("{") && translatedText[i].Contains("}"))
                        {
                            if (curlyBracketValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("{").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf("}", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("{", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("{", t);
                                    int end = translatedText[i].IndexOf("}", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, curlyBracketValues[0]);
                                        curlyBracketValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (originalText3[i].Contains("&lt;") && originalText3[i].Contains("&gt;"))
                        {
                            translatedText[i] = translatedText[i].Replace("<", "&lt;");
                            translatedText[i] = translatedText[i].Replace(">", "&gt;");
                        }

                        if (originalText3[i].Contains("&amp;"))
                        {
                            translatedText[i] = translatedText[i].Replace("™", "&amp;");
                        }

                        if (originalText3[i].Contains("&apos;"))
                        {
                            translatedText[i] = translatedText[i].Replace("'", "&apos;");
                        }
                    }
                    else if (gameName == "DungeonCrawler")
                    {
                        translatedText[i] = translatedText[i].Replace(@"<6 >", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"<4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"<3 >", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 6>", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"< 4>", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3>", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 6 >", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"< 4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3 >", "<3>");

                        translatedText[i] = translatedText[i].Replace(@"<6>_", "{}");
                        translatedText[i] = translatedText[i].Replace(@"<4>_", "[]");
                        if (yandexButton.Checked)
                            translatedText[i] = translatedText[i].Replace(@"<3>_", "<>");
                        else
                            translatedText[i] = translatedText[i].Replace(@"<3>", "<>");

                        if (translatedText[i].Contains("[") && translatedText[i].Contains("]"))
                        {
                            if (squareQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("[").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf("]", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("[", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("[", t);
                                    int end = translatedText[i].IndexOf("]", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, squareQuoteValues[0]);
                                        squareQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (translatedText[i].Contains("<") && translatedText[i].Contains(">"))
                        {
                            if (triangQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("<").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf(">", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("<", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("<", t);
                                    int end = translatedText[i].IndexOf(">", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, triangQuoteValues[0]);
                                        triangQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (translatedText[i].Contains("{") && translatedText[i].Contains("}"))
                        {
                            if (curlyBracketValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("{").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf("}", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("{", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("{", t);
                                    int end = translatedText[i].IndexOf("}", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, curlyBracketValues[0]);
                                        curlyBracketValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (originalText3[i].Contains("&lt;") && originalText3[i].Contains("&gt;"))
                        {
                            translatedText[i] = translatedText[i].Replace("<", "&lt;");
                            translatedText[i] = translatedText[i].Replace(">", "&gt;");
                        }

                        if (originalText3[i].Contains("&amp;"))
                        {
                            translatedText[i] = translatedText[i].Replace("™", "&amp;");
                        }

                        if (originalText3[i].Contains("&apos;"))
                        {
                            translatedText[i] = translatedText[i].Replace("'", "&apos;");
                        }
                    }
                    else if (gameName == "nwr")
                    {
                        translatedText[i] = translatedText[i].Replace(@"<6 >", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"<4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"<3 >", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 6>", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"< 4>", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3>", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 6 >", "<6>");
                        translatedText[i] = translatedText[i].Replace(@"< 4 >", "<4>");
                        translatedText[i] = translatedText[i].Replace(@"< 3 >", "<3>");
                        translatedText[i] = translatedText[i].Replace(@"< 5 >", "<5>");
                        translatedText[i] = translatedText[i].Replace(@"<5 >", "<5>");
                        translatedText[i] = translatedText[i].Replace(@"< 5>", "<5>");

                        translatedText[i] = translatedText[i].Replace(@"<6>_", "{}");
                        translatedText[i] = translatedText[i].Replace(@"<5>_", "%%");
                        if (yandexButton.Checked)
                            translatedText[i] = translatedText[i].Replace(@"<3>_", "<>");
                        else
                            translatedText[i] = translatedText[i].Replace(@"<3>", "<>");

                        if (translatedText[i].Contains("<") && translatedText[i].Contains(">"))
                        {
                            if (triangQuoteValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("<").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf(">", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("<", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("<", t);
                                    int end = translatedText[i].IndexOf(">", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, triangQuoteValues[0]);
                                        triangQuoteValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (translatedText[i].Contains("{") && translatedText[i].Contains("}"))
                        {
                            if (curlyBracketValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("{").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf("}", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("{", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("{", t);
                                    int end = translatedText[i].IndexOf("}", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, curlyBracketValues[0]);
                                        curlyBracketValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }

                        if (translatedText[i].Contains("%"))
                        {
                            if (percentValues.Any())
                            {
                                int t = 0;
                                int count = translatedText[i].Split("%").Length - 1;

                                for (int j = 0; j < count; j++)
                                {
                                    if (j == count)
                                        break;

                                    if (j > 0)
                                    {
                                        t = translatedText[i].IndexOf("%", t + 1);
                                        //if (translatedText[i][t] == '>' && translatedText[i].IndexOf("<", t) == -1)
                                        if (translatedText[i].IndexOf("%", t) == -1)
                                            break;
                                    }

                                    int start = translatedText[i].IndexOf("%", t);
                                    int end = translatedText[i].IndexOf("%", start);
                                    string result = translatedText[i].Substring(start + 1, end - start - 1);
                                    if (result == "")
                                    {
                                        translatedText[i] = translatedText[i].Insert(start + 1, percentValues[0]);
                                        percentValues.RemoveRange(0, 1);
                                    }
                                }
                            }
                        }
                    }


                    #region translation fixes
                    #region Core Keeper
                    if (gameName == "CoreKeeper" && translatedText[i] == "Еда")
                        translatedText[i] = "Сытость";
                    if (gameName == "CoreKeeper" && originalText[i] == "Reflections")
                        translatedText[i] = "Отражения";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Показать внутриигровой пользовательский интерфейс")
                        translatedText[i] = "Показывать пользовательский интерфейс";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Показать номера повреждений")
                        translatedText[i] = "Показывать урон";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Направление лицевой мыши")
                        translatedText[i] = "Направление движения мышью";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Экран дрожит")
                        translatedText[i] = "Дрожание экрана";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Удалить параметры выполнения")
                        translatedText[i] = "Удалить прогресс";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Меню Удаления символов")
                        translatedText[i] = "Меню Удаления персонажей";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Игровой автомат World slot")
                        translatedText[i] = "World slot";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Фон")
                        translatedText[i] = "Класс";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Выберите тип символа")
                        translatedText[i] = "Выберите тип персонажа";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Выберите символ")
                        translatedText[i] = "Выберите персонажа";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Удалить символ")
                        translatedText[i] = "Удалить персонажа";
                    if (gameName == "CoreKeeper" && translatedText[i] == "Погрузка...")
                        translatedText[i] = "Загрузка...";
                    #endregion
                    #region Escape Simulator
                    if (gameName == "Escape Simulator" && translatedText[i] == "Кредиты")
                        translatedText[i] = "Титры";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Уволиться")
                        translatedText[i] = "Выход";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Семинар")
                        translatedText[i] = "Мастерская";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Кассета с кассетами")
                        translatedText[i] = "Кассетная лента";
                    if (gameName == "Escape Simulator" && translatedText[i] == "спина")
                        translatedText[i] = "Назад";
                    if (gameName == "Escape Simulator" && translatedText[i] == "ОТРЕМОНТИРОВАТЬ ВСЕ\"")
                        translatedText[i] = "ОТРЕМОНТИРОВАТЬ ВСЕ"; // в идеале исправить, что почему-то ковычка или не тримится(!), или добавляется после перевода
                    if (gameName == "Escape Simulator" && translatedText[i] == "Выходная комната")
                        translatedText[i] = "Выход из комнаты";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Редактор номеров")
                        translatedText[i] = "Редактор комнат";
                    if (gameName == "Escape Simulator" && translatedText[i] == "НОМЕРА")
                        translatedText[i] = "Комнаты";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Учебник")
                        translatedText[i] = "Обучение";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Издательская комната...")
                        translatedText[i] = "Публикация комнаты...";
                    if (gameName == "Escape Simulator" && translatedText[i] == "При публикации номера произошла ошибка.")
                        translatedText[i] = "При публикации комнаты произошла ошибка.";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Тарифный номер")
                        translatedText[i] = "Тарифная комната";
                    if (gameName == "Escape Simulator" && translatedText[i] == "На сайте")
                        translatedText[i] = "Вкл";
                    if (gameName == "Escape Simulator" && translatedText[i] == "В сети")
                        translatedText[i] = "Выкл";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Джек-О-Фонарь")
                        translatedText[i] = "Светильник Джека";
                    if (gameName == "Escape Simulator" && translatedText[i] == "Джек-О'-Фонарь")
                        translatedText[i] = "Светильник Джека";
                    if (gameName == "Escape Simulator" && translatedText[i] == "УСТРОЙСТВО ДЛЯ ЧТЕНИЯ ОТКРЫТОК")
                        translatedText[i] = "УСТРОЙСТВО ДЛЯ ЧТЕНИЯ КАРТ";
                    #endregion
                    #region Temtem
                    if (gameName == "Temtem" && translatedText[i] == "Два Темкара столкнулись в воздухе!")
                        translatedText[i] = "Две Тэмкарты столкнулись в воздухе!";
                    if (gameName == "Temtem" && translatedText[i] == "Холод не имеет никаких сопутствующих эффектов, но Предмет, который дважды остынет, станет замороженным. Будучи замороженным, Темтем не сможет использовать какие-либо техники.")
                        translatedText[i] = "Холод не имеет никаких сопутствующих эффектов, но Тэм, который дважды замерзнет, станет замороженным. Будучи замороженным, Тэм не сможет использовать какие-либо техники.";
                    if (gameName == "Temtem" && originalText[i] == "HP")
                        translatedText[i] = "HP";
                    if (gameName == "Temtem" && originalText[i] == "STA")
                        translatedText[i] = "STA";
                    if (gameName == "Temtem" && originalText[i] == "SPD")
                        translatedText[i] = "SPD";
                    if (gameName == "Temtem" && originalText[i] == "ATK")
                        translatedText[i] = "ATK";
                    if (gameName == "Temtem" && originalText[i] == "DEF")
                        translatedText[i] = "DEF";
                    if (gameName == "Temtem" && originalText[i] == "SPATK")
                        translatedText[i] = "SPATK";
                    if (gameName == "Temtem" && originalText[i] == "SPDEF")
                        translatedText[i] = "SPDEF";
                    if (gameName == "Temtem" && originalText[i] == "HP SVs")
                        translatedText[i] = "HP SVs";
                    if (gameName == "Temtem" && originalText[i] == "STA SVs")
                        translatedText[i] = "STA SVs";
                    if (gameName == "Temtem" && originalText[i] == "SPD SVs")
                        translatedText[i] = "SPD SVs";
                    if (gameName == "Temtem" && originalText[i] == "ATK SVs")
                        translatedText[i] = "ATK SVs";
                    if (gameName == "Temtem" && originalText[i] == "DEF SVs")
                        translatedText[i] = "DEF SVs";
                    if (gameName == "Temtem" && originalText[i] == "SPATK SVs")
                        translatedText[i] = "SPATK SVs";
                    if (gameName == "Temtem" && originalText[i] == "SPDEF SVs")
                        translatedText[i] = "SPDEF SVs";
                    if (gameName == "Temtem" && originalText[i] == "HP TVs")
                        translatedText[i] = "HP TVs";
                    if (gameName == "Temtem" && originalText[i] == "STA TVs")
                        translatedText[i] = "STA TVs";
                    if (gameName == "Temtem" && originalText[i] == "SPD TVs")
                        translatedText[i] = "SPD TVs";
                    if (gameName == "Temtem" && originalText[i] == "ATK TVs")
                        translatedText[i] = "ATK TVs";
                    if (gameName == "Temtem" && originalText[i] == "DEF TVs")
                        translatedText[i] = "DEF TVs";
                    if (gameName == "Temtem" && originalText[i] == "SPATK TVs")
                        translatedText[i] = "SPATK TVs";
                    if (gameName == "Temtem" && originalText[i] == "SPDEF TVs")
                        translatedText[i] = "SPDEF TVs";
                    if (gameName == "Temtem" && originalText[i] == "TMR")
                        translatedText[i] = "TMR";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Bug - HP")
                        translatedText[i] = "Telomere Bug - HP";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Bug - STA")
                        translatedText[i] = "Telomere Bug - STA";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Bug - SPD")
                        translatedText[i] = "Telomere Bug - SPD";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Bug - ATK")
                        translatedText[i] = "Telomere Bug - ATK";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Bug - DEF")
                        translatedText[i] = "Telomere Bug - DEF";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Bug - SPATK")
                        translatedText[i] = "Telomere Bug - SPATK";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Bug - SPDEF")
                        translatedText[i] = "Telomere Bug - SPDEF";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Hack - HP")
                        translatedText[i] = "Telomere Hack - HP";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Hack - STA")
                        translatedText[i] = "Telomere Hack - STA";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Hack - SPD")
                        translatedText[i] = "Telomere Hack - SPD";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Hack - ATK")
                        translatedText[i] = "Telomere Hack - ATK";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Hack - DEF")
                        translatedText[i] = "Telomere Hack - DEF";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Hack - SPATK")
                        translatedText[i] = "Telomere Hack - SPATK";
                    if (gameName == "Temtem" && originalText[i] == "Telomere Hack - SPDEF")
                        translatedText[i] = "Telomere Hack - SPDEF";
                    if (gameName == "Temtem" && originalText[i] == "TVs")
                        translatedText[i] = "TVs";
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("телевизоры", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("телевизионный", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("передача", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("телевизор", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("Телевизоры", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("Телевизионный", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("Передача", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("TVs"))
                        translatedText[i] = translatedText[i].Replace("Телевизор", "TVs");
                    if (gameName == "Temtem" && originalText[i].Contains("SPATK"))
                        translatedText[i] = translatedText[i].Replace("СПАТК", "SPATK");
                    if (gameName == "Temtem" && originalText[i].Contains("SPATK"))
                        translatedText[i] = translatedText[i].Replace("СПАТКУ", "SPATK");
                    if (gameName == "Temtem" && originalText[i].Contains("SPATK"))
                        translatedText[i] = translatedText[i].Replace("СПАТКОВ", "SPATK");
                    if (gameName == "Temtem" && originalText[i].Contains("SPDEF"))
                        translatedText[i] = translatedText[i].Replace("СПДЕФ", "SPDEF");
                    if (gameName == "Temtem" && originalText[i].Contains("DEF"))
                        translatedText[i] = translatedText[i].Replace("ЗАЩИТУ", "DEF");
                    if (gameName == "Temtem" && originalText[i].Contains("DEF"))
                        translatedText[i] = translatedText[i].Replace("ДЕФ", "DEF");
                    if (gameName == "Temtem" && originalText[i].Contains("сотрудников"))
                        translatedText[i] = translatedText[i].Replace("сотрудников", "STA");
                    if (gameName == "Temtem" && translatedText[i] == "Вы уверены, что хотите показать полную версию tempedia? Вы будете избалованы каждой отдельной темой в игре.")
                        translatedText[i] = translatedText[i].Replace("Вы уверены, что хотите показать полную версию tempedia? Вы будете избалованы каждой отдельной темой в игре.", "Вы уверены, что хотите посмотреть полную версию Tempedia? Там содержится информация о каждой отдельном Тэмтэме в игре.");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темемологию", "тэмологию");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темемология", "тэмология");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темемологии", "тэмологии");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Темемологию", "Тэмологию");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Темемология", "Тэмология");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Темемологии", "Тэмологии");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("курятник", "команду");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("партия", "группа");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("партию", "группу");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("партии", "группы");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("вечеринка", "группа");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("вечеринку", "группу");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("вечеринки", "группы");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("вечеринке", "группе");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Курятник", "Команду");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Партия", "Группа");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Партию", "Группу");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Партии", "Группы");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Вечеринка", "Группа");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Вечеринку", "Группу");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Вечеринки", "Группы");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Вечеринке", "Группе");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Вечеринкой", "Группой");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Teming", "Ловля");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Темы", "Тэмы");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Темой", "Тэмой");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Тему", "Тэму");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Тема", "Тэма");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темы", "Тэмы");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темой", "Тэмой");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("тему", "Тэму");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темтом", "Тэмом");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темтам", "Тэмам");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("темах", "Тэмах");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("'Tems", "Тэмы");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Темтем", "Тэмтэм");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Темем", "Тэмтэм");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Tamer Pass", "Пропуск Укротителя");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("RMT / Buying or Selling Accounts", "RMT / Покупка или продажа аккаунтов");
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Change Gear", "Сменить снаряжение");
                    if (gameName == "Temtem" && originalText[i].Contains("MATCHMAKING"))
                        translatedText[i] = translatedText[i].Replace("СВАТОВСТВО", "MATCHMAKING");
                    if (gameName == "Temtem" && originalText[i].Contains("MATCHMAKING"))
                        translatedText[i] = translatedText[i].Replace("СВАТАНИЕ", "MATCHMAKING");
                    if (gameName == "Temtem" && originalText[i] == "Mount")
                        translatedText[i] = translatedText[i].Replace("Гора", "Маунт");
                    if (gameName == "Temtem" && originalText[i].Contains("Redeem"))
                        translatedText[i] = "Redeem";
                    if (gameName == "Temtem" && translatedText[i] == "Продолжать")
                        translatedText[i] = "Продолжить";
                    if (gameName == "Temtem" && translatedText[i] == "У вас есть некоторые несохраненные изменения. Вы хотите их спасти?")
                        translatedText[i] = "У вас есть некоторые несохраненные изменения. Вы хотите их сохранить?";
                    if (gameName == "Temtem" && translatedText[i] == "Телевизионный выход")
                        translatedText[i] = "Даст TV";
                    if (gameName == "Temtem" && translatedText[i] == "Последний раз видел")
                        translatedText[i] = "Последний раз онлайн";
                    if (gameName == "Temtem" && originalText[i] == "Recent")
                        translatedText[i] = "Недавние";
                    if (gameName == "Temtem" && originalText[i] == "Capture")
                        translatedText[i] = "Поимка";
                    if (gameName == "Temtem" && originalText[i] == "Medicine")
                        translatedText[i] = "Медицина";
                    if (gameName == "Temtem" && originalText[i] == "Performance")
                        translatedText[i] = "Performance";
                    if (gameName == "Temtem" && originalText[i] == "Gear")
                        translatedText[i] = "Снаряжение";
                    if (gameName == "Temtem" && originalText[i] == "Titles")
                        translatedText[i] = "Титулы";
                    if (gameName == "Temtem" && originalText[i] == "Pan")
                        translatedText[i] = "Перемещать";
                    if (gameName == "Temtem" && originalText[i] == "Competitive")
                        translatedText[i] = "Competitive";
                    if (gameName == "Temtem" && translatedText[i] == "ПОДПИСКИ ОТКРЫТЫ")
                        translatedText[i] = "ВСТУПЛЕНИЕ ДОСТУПНО";
                    if (gameName == "Temtem" && translatedText[i] == "подписываться")
                        translatedText[i] = "вступить";
                    if (gameName == "Temtem" && translatedText[i] == "Начиная с")
                        translatedText[i] = "Начнется через";
                    if (gameName == "Temtem" && translatedText[i] == "Высокий сезон")
                        translatedText[i] = "Наивысший";
                    if (gameName == "Temtem" && translatedText[i] == "НАЙТИ СОВПАДЕНИЕ")
                        translatedText[i] = "НАЙТИ МАТЧ";
                    if (gameName == "Temtem" && translatedText[i] == "Характер")
                        translatedText[i] = "Персонаж";
                    if (gameName == "Temtem" && translatedText[i] == "Выигрывает")
                        translatedText[i] = "Побед";
                    if (gameName == "Temtem" && translatedText[i] == "Покажите")
                        translatedText[i] = "Показать";
                    if (gameName == "Temtem" && translatedText[i] == "Сбросить символ")
                        translatedText[i] = "Сбросить персонажа";
                    if (gameName == "Temtem" && translatedText[i] == "Возобновиться")
                        translatedText[i] = "Возобновить";
                    if (gameName == "Temtem" && translatedText[i] == "Движение мыши")
                        translatedText[i] = "Движение с помощью мыши";
                    if (gameName == "Temtem" && translatedText[i] == "Показывать учебные пособия")
                        translatedText[i] = "Показывать обучающие подсказки";
                    if (gameName == "Temtem" && translatedText[i] == "Учебные пособия")
                        translatedText[i] = "Обучающие подсказки";
                    if (gameName == "Temtem" && translatedText[i] == "Показать опыт Укротителя")
                        translatedText[i] = "Показывать опыт TamerPasss";
                    if (gameName == "Temtem" && translatedText[i] == "Утроить")
                        translatedText[i] = "Высокочастотный";
                    if (gameName == "Temtem" && originalText[i] == "Head")
                        translatedText[i] = "Голова";
                    if (gameName == "Temtem" && originalText[i] == "Ultra")
                        translatedText[i] = "Ультра";
                    if (gameName == "Temtem" && originalText[i] == "Off")
                        translatedText[i] = "Off";
                    if (gameName == "Temtem" && originalText[i] == "Fps")
                        translatedText[i] = "Fps";
                    if (gameName == "Temtem" && originalText[i] == "Fps+")
                        translatedText[i] = "Fps+";
                    if (gameName == "Temtem")
                        translatedText[i] = translatedText[i].Replace("Днища", "Штаны");
                    if (gameName == "Temtem" && translatedText[i] == "Блокируйте неизвестный шепот")
                        translatedText[i] = "Заблокировать лс от неизвестных игроков";
                    if (gameName == "Temtem" && translatedText[i] == "Голоса персонажей диалога")
                        translatedText[i] = "Голоса персонажей в диалогах";
                    if (gameName == "Temtem" && translatedText[i] == "Автоматическое масштабиррование темы отключено")
                        translatedText[i] = "Авто-масштабиррование Тэмов отключено";
                    if (gameName == "Temtem" && translatedText[i] == "Автоматическое масштабиррование темы включено")
                        translatedText[i] = "Авто-масштабиррование Тэмов включено";
                    if (gameName == "Temtem" && translatedText[i] == "Автоматическое масштабиррование тэмы отключено")
                        translatedText[i] = "Авто-масштабиррование Тэмов отключено";
                    if (gameName == "Temtem" && translatedText[i] == "Автоматическое масштабиррование тэмы включено")
                        translatedText[i] = "Авто-масштабиррование Тэмов включено";
                    if (gameName == "Temtem" && originalText[i] == "Tamer Pass")
                        translatedText[i] = "Tamer Pass";
                    #endregion
                    if (originalText[i] == "V SYNC")
                        translatedText[i] = "V SYNC";
                    if (originalText[i] == "LOOK SENSITIVITY")
                        translatedText[i] = "LOOK SENSITIVITY";
                    if (originalText[i] == "Loading...")
                        translatedText[i] = "Загрузка...";
                    #region The Mortuary Assistant
                    if (gameName == "The Mortuary Assistant")
                        translatedText[i] = translatedText[i].Replace("\"", @"\""");
                    #endregion
                    if (gameName != "NewWorld")
                        translatedText[i] = translatedText[i].Replace("&amp;", "™");
                    #endregion

                    System.Threading.Thread.Sleep(500);
                    if (gameName == "Escape Simulator")
                    {
                        if (originalText[i].Contains("'") && translatedText[i].Contains("\""))
                            translatedText[i] = translatedText[i].Replace("\"", "'");

                        if (!originalText[i].Contains("\"") && translatedText[i].Contains("\""))
                            translatedText[i] = translatedText[i].Replace("\"", "");

                        translatedText[i] = "\"" + translatedText[i] + "\"";
                    }

                    //translatedText[i] = translatedText[i].Replace(@"\x20", " ");

                    for (int ii = 0; ii < lineNumbers.Count; ii++)
                    {
                        if (gameName == "Temtem" || gameName == "CoreKeeper")
                        {
                            if (arraysBackup[lineNumbers[ii]] == originalText3[i])
                            {
                                if (gameName != "A Space for the Unbound")
                                {
                                    arraysBackup[lineNumbers[ii]] = translatedText[i];
                                }
                                #region перевод с учетом пола
                                //else
                                //{
                                //    string[] splitTranslatedText = translatedText[i].Split("говорит - - ");
                                //
                                //    if (splitTranslatedText.Length > 1)
                                //    {
                                //        splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //        arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //    }
                                //    else
                                //    {
                                //        splitTranslatedText = translatedText[i].Split("говорит: - - ");
                                //
                                //        if (splitTranslatedText.Length > 1)
                                //        {
                                //            splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //            arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //        }
                                //        else
                                //        {
                                //            splitTranslatedText = translatedText[i].Split("говорит - ");
                                //
                                //            if (splitTranslatedText.Length > 1)
                                //            {
                                //                splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //                arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //            }
                                //            else
                                //            {
                                //                splitTranslatedText = translatedText[i].Split("говорит: - ");
                                //
                                //                if (splitTranslatedText.Length > 1)
                                //                {
                                //                    splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //                    arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //                }
                                //                else
                                //                {
                                //                    splitTranslatedText = translatedText[i].Split("says - - - ");
                                //
                                //                    if (splitTranslatedText.Length > 1)
                                //                    {
                                //                        splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //                        arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //                    }
                                //                    else
                                //                    {
                                //                        splitTranslatedText = translatedText[i].Split("says - - ");
                                //
                                //                        if (splitTranslatedText.Length > 1)
                                //                        {
                                //                            splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //                            arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //                        }
                                //                        else
                                //                        {
                                //                            splitTranslatedText = translatedText[i].Split("говорит... - ");
                                //
                                //                            if (splitTranslatedText.Length > 1)
                                //                            {
                                //                                splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //                                arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //                            }
                                //                            else
                                //                            {
                                //                                splitTranslatedText = translatedText[i].Split("говорит... ");
                                //
                                //                                if (splitTranslatedText.Length > 1)
                                //                                {
                                //                                    splitTranslatedText[1] = splitTranslatedText[1].TrimStart();
                                //                                    arraysBackup[lineNumbers[ii]] = splitTranslatedText[1];
                                //                                }
                                //                                else
                                //                                {
                                //                                    arraysBackup[lineNumbers[ii]] = translatedText[i];
                                //                                }
                                //                            }
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                                #endregion

                                break;
                            }
                        }
                        else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                        {
                            //if (arraysBackup[lineNumbers[ii]].Split(':')[0] == originalText[i])
                            if (arraysBackup[lineNumbers[ii]].Contains(originalText3[i]) || arraysBackup[lineNumbers[ii]].Contains(originalText3[i].Replace(": ", ":")))
                            {
                                arraysBackup[lineNumbers[ii]] = arraysBackup[lineNumbers[ii]].Split(':')[0] + ": " + translatedText[i];

                                break;
                            }
                        }
                        else if (gameName == "NewWorld" || gameName == "DungeonCrawler")
                        {
                            //if (arraysBackup[lineNumbers[ii]].Split(':')[0] == originalText[i])
                            if (arraysBackup[lineNumbers[ii]].Contains(originalText3[i]))
                            {
                                string[] t = arraysBackup[lineNumbers[ii]].Split(SeparatorDifferentGames());

                                if (t.Length == 2)
                                    arraysBackup[lineNumbers[ii]] = t[0] + SeparatorDifferentGames() + translatedText[i];
                                else if (t.Length == 3)
                                    arraysBackup[lineNumbers[ii]] = t[0] + SeparatorDifferentGames() + translatedText[i] + SeparatorDifferentGames() + t[2];

                                break;
                            }
                        }
                        else if (gameName == "nwr")
                        {

                            //if (arraysBackup[lineNumbers[ii]].Split(':')[0] == originalText[i])
                            if (arraysBackup[lineNumbers[ii]].Contains(originalText3[i]))
                            {
                                arraysBackup[lineNumbers[ii]] = "\"" + arraysBackup[lineNumbers[ii]].Split(SeparatorDifferentGames())[1] + "\"" + "    " + "\"" + translatedText[i] + "\"";

                                break;
                            }
                        }
                    }
                }

                if (gameName == "nwr")
                    File.WriteAllLines(path + @"\backup.txt", arraysBackup, encoding: Encoding.UTF8);
                else
                    File.WriteAllLines(localizationPath + @"backup.txt", arraysBackup, encoding: Encoding.UTF8);
            }
        }

        void Translator(string sourceText, List<int> lineNumbers)
        {
            //if (sourceText == "")
            //    return;

            if (!translatedFirstTime && sourceText != "")
            {
                if (File.Exists(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"C:\Program Files\Mozilla Firefox\firefox.exe") || File.Exists(@"X:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"X:\Program Files\Mozilla Firefox\firefox.exe") ||
                    File.Exists(@"D:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"D:\Program Files\Mozilla Firefox\firefox.exe") || File.Exists(@"Y:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"Y:\Program Files\Mozilla Firefox\firefox.exe") ||
                    File.Exists(@"Z:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"Z:\Program Files\Mozilla Firefox\firefox.exe") || File.Exists(@"E:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"E:\Program Files\Mozilla Firefox\firefox.exe") ||
                    File.Exists(@"F:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"F:\Program Files\Mozilla Firefox\firefox.exe") || File.Exists(@"S:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"S:\Program Files\Mozilla Firefox\firefox.exe") ||
                    File.Exists(@"G:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"G:\Program Files\Mozilla Firefox\firefox.exe") || File.Exists(@"H:\Program Files (x86)\Mozilla Firefox\firefox.exe") || File.Exists(@"H:\Program Files\Mozilla Firefox\firefox.exe"))
                {
                    cmdKill("geckodriver.exe");
                    cmdKill("firefox.exe");

                    if (!File.Exists(Application.StartupPath + @"geckodriver.exe"))
                        File.WriteAllBytesAsync(Application.StartupPath + @"geckodriver.exe", Resources.geckodriver_exe);

                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Предупреждение на случай, если Вы используете Firefox как основной браузер - возможно он выключится несколько раз во время русификации игры...", Color.Red)));
                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Перевод строк...", Color.Black)));

                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    FirefoxDriverService firefoxService = FirefoxDriverService.CreateDefaultService();

                    firefoxService.HideCommandPromptWindow = true;
                    firefoxService.FirefoxBinaryPath = @"H:\Program Files\Mozilla Firefox\firefox.exe"; //путь к firefox
                    //firefoxOptions.AddArguments("--headless");

                    string translatorUrl = "https://www.deepl.com/translator";
                    if (deeplButton.Checked)
                        translatorUrl = "https://www.deepl.com/translator";
                    /*else if (googleButton.Checked)
                        translatorUrl = "https://translate.google.com";*/
                    else if (yandexButton.Checked)
                        translatorUrl = "https://translate.yandex.ru";

                    driver = new FirefoxDriver(firefoxService, firefoxOptions) //*[@id="textbox2"]/div[3]/div[1]/button
                    {
                        Url = translatorUrl
                    };

                    System.Threading.Thread.Sleep(5000);

                    try
                    {
                        driver.FindElement(By.XPath("//*[@dl-test=\"cookie-banner-lax-close-button\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-target-lang-btn\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@dl-test=\"translator-lang-option-ru\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                    }
                    catch
                    {
                    }

                    try
                    {
                        driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[2]/button/div[3]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[1]/div/div[3]/div/div[3]/div[6]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[5]/button/div[3]")).Click();
                        System.Threading.Thread.Sleep(500);
                        var ruButton = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[2]/c-wiz/div[2]/div/div[3]/div/div[2]/div[89]"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(document.body.scrollHeight,0);");

                        new Actions(driver).MoveToElement(ruButton).Click().Perform();
                    }
                    catch
                    {
                    }

                    try
                    {
                        //yandex
                        driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div[3]/div/div")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@id=\"settingOptions\"]/div[1]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@id=\"srcLangButton\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[5]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@id=\"dstLangButton\"]")).Click();
                        System.Threading.Thread.Sleep(500);
                        driver.FindElement(By.XPath("//*[@id=\"langSelect\"]/div[2]/div/div[68]")).Click();
                    }
                    catch
                    {
                    }

                    translatedFirstTime = true;
                }
                else
                {
                    MessageBox.Show(
                       "На вашем компьютере должен быть установлен браузер Mozilla Firefox.\nПрогресс русификации сохранен, просто нажмите \"Обновить/Переустановить\" после установки браузера.",
                       "Отсутствует необходимый браузер",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error,
                       MessageBoxDefaultButton.Button1,
                       MessageBoxOptions.DefaultDesktopOnly);

                    return;
                }
            }

            TranslatedTextConstructor(sourceText, lineNumbers);
        }

        bool hasRuLetters(string text)
        {
            text = text.ToLower();

            if (text.Contains("а") || text.Contains("е") || text.Contains("ё") || text.Contains("и") || text.Contains("о") || text.Contains("у") || text.Contains("ы") || text.Contains("э") || text.Contains("ю") || text.Contains("я") ||
                 text.Contains("б") || text.Contains("в") || text.Contains("г") || text.Contains("д") || text.Contains("ж") || text.Contains("з") || text.Contains("й") || text.Contains("к") || text.Contains("л") || text.Contains("м") ||
                  text.Contains("н") || text.Contains("п") || text.Contains("р") || text.Contains("с") || text.Contains("т") || text.Contains("ф") || text.Contains("х") || text.Contains("ц") || text.Contains("ч") || text.Contains("ш") ||
                   text.Contains("щ") || text.Contains("ъ") || text.Contains("ь"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool hasEnLetters(string text)
        {
            text = text.ToLower();

            if (text.Contains("q") || text.Contains("w") || text.Contains("e") || text.Contains("r") || text.Contains("t") || text.Contains("y") || text.Contains("u") || text.Contains("i") || text.Contains("o") || text.Contains("p") ||
                 text.Contains("a") || text.Contains("s") || text.Contains("d") || text.Contains("f") || text.Contains("g") || text.Contains("h") || text.Contains("j") || text.Contains("k") || text.Contains("l") || text.Contains("z") ||
                  text.Contains("x") || text.Contains("c") || text.Contains("v") || text.Contains("b") || text.Contains("n") || text.Contains("m"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IdCheck(string id)
        {
            if (id == "" || id == null)
                return true;

            if (machineName == myMachineName && gameName == "Temtem")
                return true;

            if (gameName == "Temtem")
            {
                id = id.TrimStart('"');
                id = id.TrimEnd('"');
            }

            if (skillsNameCheckTranslate.Checked)
            {
                if (gameName == "Temtem")
                {
                    if (id.Contains("Techniques/Tech_"))
                        return false;
                }

                if (gameName == "CoreKeeper")
                {
                    if (id.Contains("Skills/") || id.Contains("SkillTalents/"))
                        return false;
                }
            }

            if (skillsDescriptionCheckTranslate.Checked)
            {
                if (gameName == "Temtem")
                {
                    if (id.Contains("TechniquesDesc/Tech_"))
                        return false;
                }

                if (gameName == "CoreKeeper")
                {
                    if (id.Contains("Conditions/"))
                        return false;
                }
            }

            if (itemsCheckTranslate.Checked)
            {
                if (gameName == "Temtem")
                {
                    if (id.Contains("Inventory"))
                        return false;
                }

                if (gameName == "CoreKeeper")
                {
                    if (id.Contains("Items/") || id.Contains("FooDungeonCrawlerjectives/") || id.Contains("FoodNouns/"))
                        return false;
                }
            }

            if (npcCheckTranslate.Checked)
            {
                if (gameName == "Temtem")
                {
                    if (id.Contains("DialoguesNames/"))
                        return false;
                }
            }

            if (temtemCheckTranslate.Checked)
            {
                if (gameName == "Temtem")
                {
                    if (id.Contains("Monsters/") || id.Contains("MonstersDesc/"))
                        return false;
                }
            }

            return true;
        }

        string ReadText(string id, string dr, int lineNumber)
        {
            if (dr == "" || dr == null)
                return "";

            string strRow = "";

            string dr2 = dr;

            if (dr2.Contains("{") && dr2.Contains("}"))
            {
                int t = 0;
                int count = dr2.Split("}").Length - 1;

                for (int j = 0; j < count; j++)
                {
                    if (j == count)
                        break;

                    if (j > 0)
                    {
                        t = dr2.IndexOf("}", t + 1);
                        //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                        if (dr2.IndexOf("{", t) == -1)
                            break;
                    }

                    int start = dr2.IndexOf("{", t);
                    int end = dr2.IndexOf("}", start);
                    string result = dr2.Substring(start + 1, end - start - 1);

                    if (result.Any(char.IsLetter))
                    {
                        dr2 = dr2.Remove(start + 1, end - start - 1);
                    }
                }
            }
            if (dr2.Contains("[") && dr2.Contains("]"))
            {
                int t = 0;
                int count = dr2.Split("]").Length - 1;

                for (int j = 0; j < count; j++)
                {
                    if (j == count)
                        break;

                    if (j > 0)
                    {
                        t = dr2.IndexOf("]", t + 1);
                        //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                        if (dr2.IndexOf("[", t) == -1)
                            break;
                    }

                    int start = dr2.IndexOf("[", t);
                    int end = dr2.IndexOf("]", start);
                    string result = dr2.Substring(start + 1, end - start - 1);

                    if (result.Any(char.IsLetter))
                    {
                        dr2 = dr2.Remove(start + 1, end - start - 1);
                    }
                }
            }
            if (dr2.Contains("<") && dr2.Contains(">"))
            {
                int t = 0;
                int count = dr2.Split(">").Length - 1;

                for (int j = 0; j < count; j++)
                {
                    if (j == count)
                        break;

                    if (j > 0)
                    {
                        t = dr2.IndexOf(">", t + 1);
                        //if (originalText[i][t] == '>' && originalText[i].IndexOf("<", t) == -1)
                        if (dr2.IndexOf("<", t) == -1)
                            break;
                    }

                    int start = dr2.IndexOf("<", t);
                    int end = dr2.IndexOf(">", start);
                    string result = dr2.Substring(start + 1, end - start - 1);

                    if (result.Any(char.IsLetter))
                    {
                        dr2 = dr2.Remove(start + 1, end - start - 1);
                    }
                }
            }

            if (gameName == "Temtem")
                dr2 = dr2.Replace("pansuns", "<213>_ ");
            dr2 = dr2.Replace(@"\n\n", "<236>_ ");
            dr2 = dr2.Replace(@"\n", "<216>_ ");

            if (gameName != "NewWorld")
            {
                if (IdCheck(id) && dr2.Any(char.IsLetter) && !hasRuLetters(dr2) && dr != "\"{0}K\"" && dr != "\"{0}M\"" && dr != "SPACE" && dr != "Off-hand use: {0}" && dr != "off" && dr != "on"
                    && dr != "RIGHT CTRL" && dr != "RIGHT SHIFT" && dr != "Tab" && dr != "SHIFT" && dr != "ENTER" && dr != "Quit" && dr != "Backspace" && !dr.Contains('★') && !dr.Contains("★")
                    && id != "Corporation1_\"Off\"" && id != "Corporation1_\"Closed\"" && id != "Corporation1_\"Welcome\"" && id != "c3hellowlrd" && id != "c3bye" && id != "back" && !dr.Contains("#don't translate")
                    && id != "Space1_TIME1247" && !id.Contains("Keyboard/") && !id.Contains("Bridge_Chapter6/SETTEXT") && id != "Classroom_Chapter3/SAY.Chapter3.2542." && id != "Classroom_Chapter3/SAY.Chapter3.2493.")
                {
                    bool contains = false;

                    if (alreadyTranslated.Length > 0)
                    {
                        if (!alreadyTranslated[lineNumber].Contains(dr))
                            contains = true;
                    }

                    if (!contains)
                        strRow = dr;
                }
                //else if (IdCheck(id) && dr2.Any(char.IsLetter) && hasRuLetters(dr2) && hasEnLetters(dr2) && dr != "\"{0}K\"" && dr != "\"{0}M\"" && dr != "SPACE" && dr != "Off-hand use: {0}" && dr != "off" && dr != "on"
                //    && dr != "RIGHT CTRL" && dr != "RIGHT SHIFT" && dr != "Tab" && dr != "SHIFT" && dr != "ENTER" && dr != "Quit" && dr != "Backspace" && !dr.Contains('★') && !dr.Contains("★")
                //    && id != "Corporation1_\"Off\"" && id != "Corporation1_\"Closed\"" && id != "Corporation1_\"Welcome\"" && id != "c3hellowlrd" && id != "c3bye" && id != "back" && !dr.Contains("#don't translate")
                //    && id != "Space1_TIME1247" && !id.Contains("Keyboard/") && !id.Contains("Bridge_Chapter6/SETTEXT") && id != "Classroom_Chapter3/SAY.Chapter3.2542." && id != "Classroom_Chapter3/SAY.Chapter3.2493.")
                //{
                //    bool contains = false;
                //
                //    if (alreadyTranslated.Length > 0)
                //    {
                //        if (!alreadyTranslated[lineNumber].Contains(dr))
                //            contains = true;
                //    }
                //
                //    if (!contains)
                //        strRow = dr;
                //}
            }
            else
            {
                if (dr.Any(char.IsLetter) && !hasRuLetters(dr))
                {
                    if (id.Contains("PerkID") && id.Contains("Name") || id.Contains("MasterName") || id.Contains("mastername") || !id.Contains("Name") && !id.Contains("name"))
                    {
                        if (id.Contains("title") && id.Contains("desc") || id.Contains("title") && id.Contains("description") || !id.Contains("Title"))
                        {
                            if (!id.Contains("Prefix") && !id.Contains("Suffix"))
                            {
                                if (id.Contains("ui_minor") && id.Contains("description") || id.Contains("ui_major") && id.Contains("description")
                                    || id.Contains("ui_leaving") && id.Contains("description") || id.Contains("ui_preddieu_farm_attack") && id.Contains("description")
                                    || id.Contains("ui_boatencounter_brysedd") && id.Contains("description") || id.Contains("ui_boatencounter_scallywag") && id.Contains("description")
                                    || id.Contains("ui_boatencounter_scallywag") && id.Contains("description") || !id.Contains("ui_") && !id.Contains("_attack")
                                    || id.Contains("objective_msq_beach_chaplain") && id.Contains("description") || id.Contains("objective_msq_heartforge_guardian") && id.Contains("description")
                                    || id.Contains("ui_turkeyterror_worldboss_Turkulon") && id.Contains("description") || id.Contains("ui_poi_outpost") && id.Contains("description")
                                    || id.Contains("ui_or_") && id.Contains("desc") || id.Contains("ui_poi_") && id.Contains("description") || id.Contains("GlobalDesc") && id.Contains("description")
                                    || id.Contains("ui_alchemy") && id.Contains("desc") || id.Contains("ui_blacksmith") && id.Contains("desc") || id.Contains("ui_carpentry") && id.Contains("desc")
                                    || id.Contains("ui_masonry") && id.Contains("desc") || id.Contains("ui_smelter") && id.Contains("desc") || id.Contains("inv_equipLoad") && id.Contains("Tooltip")
                                    || id.Contains("CategoryData") && id.Contains("Desc") || id.Contains("ui_emote") && id.Contains("description") || id.Contains("Status_Rune") && id.Contains("Desc")
                                    || id.Contains("crafting_azothbonus_") && id.Contains("tooltip"))
                                {
                                    if (!id.Contains("url") && !id.Contains("incursion") && !id.Contains("ui_crestforeground") && !id.Contains("reward_type")
                                        && !id.Contains("owg_rank_bounty") && !id.Contains("owg_target") && id != "owg_unlock_rank" && id != "owg_company" && !id.Contains("hotspot_")
                                        && !id.Contains("lifestyle_button") && !id.Contains("ui_queue") && id != "ui_provisioning" && id != "ui_repair_trinket" && id != "ui_logging"
                                        && id != "ui_azoth_staff_tradeskill" && id != "ui_skinning" && id != "ui_fishing" && id != "ui_crafting" && id != "ui_repairing"
                                        && id != "ui_gathering" && id != "ui_refining" && id != "ui_taxes" && id != "ui_weaponsmithing" && id != "ui_jewelcrafting" && id != "ui_kiln"
                                        && id != "ui_wildernesssurvival" && !id.Contains("arcana") && !id.Contains("armoring") && !id.Contains("blacksmithing")
                                        && !id.Contains("_camping") && !id.Contains("_cooking") && !id.Contains("engineering") && !id.Contains("furnishing")
                                        && !id.Contains("outfitting") && !id.Contains("woodworking") && !id.Contains("mining") && !id.Contains("leatherworking")
                                        && !id.Contains("harvesting") && !id.Contains("stonecutting") && !id.Contains("smelting") && !id.Contains("weaving")
                                        && id != "ftue_archetypes_attributes" && id != "ftue_archetypes_tradeskills" && id != "ftue_archetypes_equipment"
                                        && id != "ftue_archetypes_details" && !id.Contains("ftue_archetypes_attr") && !id.Contains("ftue_archetypes_skill")
                                        && id != "ui_outpost_rush_summoning_circle" && !id.Contains("cc_") && id != "inv_salvage" && id != "capslock" && id != "backspace"
                                        && id != "enter" && id != "home" && id != "end" && id != "delete" && id != "pgup" && id == "pgdn" && id != "photomode_keybind"
                                        && id != "insert" && id != "scrolllock" && id != "print" && id != "pause" && id != "ui_aimsensitivity" && id != "ui_cards" && id != "ui_lmb"
                                        && id != "ui_lmbrmb" && id != "ui_rmb" && id != "ui_numnlock" && id != "ui_rmbctrl" && id != "ui_tab" && !id.Contains("ui_currency")
                                        && !id.Contains("ui_coin") && !id.Contains("ui_wallet") && id != "ui_weapon_mastery" && id != "ui_weapon_mastered"
                                        && id != "ui_tradeskill_mastered" && id != "ui_weapon_mastery_level" && !id.Contains("ui_mace") && !id.Contains("ui_rapier")
                                        && !id.Contains("ui_throwingaxe") && !id.Contains("ui_spear") && !id.Contains("ui_straightsword") && !id.Contains("ui_greataxe")
                                        && !id.Contains("ui_greatsword") && !id.Contains("ui_juggernaut") && !id.Contains("ui_crowdcrusher") && !id.Contains("ui_warhammer")
                                        && !id.Contains("ui_bow") && !id.Contains("ui_musket") && !id.Contains("ui_blunderbuss") && !id.Contains("ui_firemagic")
                                        && !id.Contains("ui_lifemagic") && !id.Contains("ui_icemagic") && !id.Contains("ui_voidmagic") && !id.Contains("ui_hatchet")
                                        && !id.Contains("ui_blood") && !id.Contains("ui_evade") && !id.Contains("ui_sword") && !id.Contains("ui_shield") && !id.Contains("ui_turret")
                                        && !id.Contains("ui_cooldown") && !id.Contains("ui_gearscore") && !id.Contains("ui_bindOnPickup") && !id.Contains("ui_bindOnEquip")
                                        && !id.Contains("ui_axe") && !id.Contains("ui_pickaxe") && !id.Contains("ui_sickle") && !id.Contains("ui_skinningknife")
                                        && !id.Contains("hwm_loot") && id != "ui_time_display" && !id.Contains("ui_tooltip") && !id.Contains("ui_dialogueoption")
                                        && id != "ui_autopinObjectivesSkillProgression" && id != "ui_autopinobjectivesEvent" && id != "ui_autopinObjectivesComm"
                                        && id != "ui_autopinObjectivesCrafting" && id != "ui_auto_pin_toggle_on" && id != "ui_auto_pin_toggle_off" && id != "ui_number_range"
                                            && id != "ui_guides" && id != "ui_leftgroupmessage" && id != "ui_guildcresttab" && !id.Contains("ui_Chopping")
                                            && !id.Contains("ui_Cutting") && !id.Contains("ui_Dressing") && !id.Contains("ui_Mining") && !id.Contains("ui_Fishing")
                                            && id != "fc_equipbutton" && id != "fc_fieldcrafting" && !id.Contains("ui_physical_defense") && !id.Contains("ui_elemental_defense")
                                            && !id.Contains("ui_damage_resistance") && id != "ui_Strength_short" && id != "ui_Dexterity_short" && id != "ui_Intelligence_short"
                                            && id != "ui_Focus_short" && id != "ui_Constitution_short" && id != "tutorial_salvage_items" && !id.Contains("BackstoryLore")
                                            && !id.Contains("Lore_Chapter_") && !id.Contains("Topic_0") && id != "ui_onehanded_weapons" && id != "ui_twohanded_weapons"
                                            && id != "ui_magic_skills" && id != "ui_ranged_weapons" && !id.Contains("ui_farmplants") && !id.Contains("ui_farmplants")
                                            && !id.Contains("ui_aloe") && !id.Contains("ui_alchemy") && !id.Contains("iconTypeUnlock_Pigment") && !id.Contains("ui_smallprey")
                                            && !id.Contains("ui_mediumprey") && !id.Contains("ui_largeprey") && !id.Contains("ui_smallpredator") && !id.Contains("ui_largepredator")
                                            && !id.Contains("ui_iron") && !id.Contains("ui_oil") && !id.Contains("ui_silver") && !id.Contains("ui_lodestone")
                                            && !id.Contains("ui_gold") && !id.Contains("ui_starmetal") && !id.Contains("ui_platinum") && !id.Contains("ui_sandstone")
                                            && !id.Contains("ui_brimstone") && !id.Contains("ui_orichalcum") && !id.Contains("ui_wyrdwood") && !id.Contains("ui_ironwood")
                                            && !id.Contains("ui_alchemyStones") && !id.Contains("ui_toolrequired") && !id.Contains("ui_skinning") && !id.Contains("ui_tracking")
                                            && !id.Contains("iconTypeUnlock_youngtrees") && !id.Contains("iconTypeUnlock_maturetrees") && id != "ui_fishing_hotspot_tutorial_label"
                                            && !id.Contains("ui_milestone_") && !id.Contains("TerritoryRecommendation_") && id != "ui_new_recommended_territory"
                                            && !id.Contains("blacksmith") && !id.Contains("cooking") && !id.Contains("engineering") && !id.Contains("outfitting")
                                            && !id.Contains("alchemy") && !id.Contains("camp") && !id.Contains("masonry") && !id.Contains("weaving") && !id.Contains("milling")
                                            && !id.Contains("smelting") && !id.Contains("tanning") && !id.Contains("carpentry") && id != "cr_gearscore" && id != "cr_gearscore_alt"
                                            && id != "cr_requires" && !id.Contains("cr_tier") && !id.Contains("crafting_carried"))
                                    {
                                        bool contains = false;

                                        if (alreadyTranslated.Length > 0)
                                        {
                                            if (!alreadyTranslated[lineNumber].Contains(dr))
                                            {
                                                contains = true;
                                            }
                                        }

                                        if (!contains)
                                            strRow = dr;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return strRow;
        }

        async void WorkAsync()
        {
            await Task.Run(() => Work());
        }

        void Work()
        {
            deleteFile(dataPath + @"\UPacker.exe");
            if (File.Exists(localizationPath + @"Parser_ULS.exe"))
                deleteFile(localizationPath + @"Parser_ULS.exe");

            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));

            string fileToRead = "";
            string backupTranslatedLocalizationPath = "";
            string backupOldLocalizationPath = "";

            if (gameName == "Temtem")
            {
                fileToRead = localizationPath + @"I2Languages_4.txt";
                backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono\Assembly-CSharp";
                backupOldLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp";
            }
            else if (gameName == "DungeonCrawler")
            {
                fileToRead = localizationPath + @"Game.locres.txt";
                backupTranslatedLocalizationPath = backupPath + @"\" + gameName + @"_translated_backup_folder\" + gameName + @"\Content\Localization\Game\en";
                backupOldLocalizationPath = backupPath + @"\" + gameName + @"_backup_folder\" + gameName + @"\Content\Localization\Game\en";
            }
            else if (gameName == "CoreKeeper")
            {
                fileToRead = localizationPath + @"Localization_3.txt";
                backupTranslatedLocalizationPath = backupPath + @"Localization_3.txt"; // надо уточнить
            }
            else if (gameName == "Escape Simulator")
            {
                fileToRead = localizationPath + @"English.txt";
                backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup";
                backupOldLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";
            }
            else if (gameName == "The Mortuary Assistant")
            {
                backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup";
                backupOldLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";
            }
            else if (gameName == "NewWorld")
            {
                backupTranslatedLocalizationPath = backupPath + @"levels\newlevels\localization\en-us";
            }
            else if (gameName == "nwr")
            {
                //localizationPath = path;
                fileToRead = path + @"\addon_english.txt";
                backupTranslatedLocalizationPath = backupPath; //translated russian
                backupOldLocalizationPath = backupPath;
            }

            if (gameName == "Escape Simulator")
            {
                if (!File.Exists(localizationPath + @"English2.txt") && !translateUpdate)
                {
                    using (StreamReader reader = new StreamReader(localizationPath + @"English.txt", encoding: Encoding.UTF8))
                    {
                        using (StreamWriter sw = new StreamWriter(localizationPath + @"English2.txt", false, encoding: Encoding.UTF8))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();

                                if (line != "" && line != " ")
                                {
                                    if (!line.Contains("#") || line.Contains("#") && line.Contains(":"))
                                        sw.WriteLine(line);
                                }
                            }
                        }
                    }

                    deleteFile(localizationPath + @"English.txt");
                    File.Copy(localizationPath + @"English2.txt", localizationPath + @"English.txt");
                    deleteFile(localizationPath + @"English2.txt");
                }
            }
            else if (gameName == "The Mortuary Assistant")
            {
                if (translateUpdate)
                {
                    File.Copy(localizationPath + @"Global_English.txt", backupPath + @"\Global_English.txt");
                    deleteFile(localizationPath + @"Global_English.txt");
                    System.Threading.Thread.Sleep(2500);
                    filesToRead = Directory.GetFiles(localizationPath);
                    System.Threading.Thread.Sleep(2500);
                    File.Copy(backupPath + @"\Global_English.txt", localizationPath + @"Global_English.txt");
                    deleteFile(backupPath + @"\Global_English.txt");

                    List<string> myList = filesToRead.ToList();
                    myList.Remove(localizationPath + "OriginalLocalization.txt");

                    filesToRead = myList.ToArray();
                }

                if (!File.Exists(localizationPath + @"Global_English.txt"))
                {
                    if (!translateUpdate)
                        filesToRead = Directory.GetFiles(localizationPath);

                    for (int i = 0; i < filesToRead.Length; i++)
                    {
                        string[] globalText = new string[] { };
                        if (File.Exists(localizationPath + @"Global_English.txt"))
                            globalText = File.ReadAllLines(localizationPath + @"Global_English.txt");

                        using (StreamReader reader = new StreamReader(filesToRead[i], encoding: Encoding.UTF8))
                        {
                            using (StreamWriter sw = new StreamWriter(localizationPath + @"Global_English.txt", true, encoding: Encoding.UTF8))
                            {
                                while (!reader.EndOfStream)
                                {
                                    string line = reader.ReadLine();

                                    if (line != "" && line != " " && line != "  ")
                                    {
                                        string[] idValues = line.Split(SeparatorDifferentGames());
                                        string newValues = "";

                                        if (idValues.Length > 1)
                                        {
                                            idValues[1] = idValues[1].Trim();
                                            newValues = idValues[1];
                                        }
                                        else
                                            newValues = idValues[0];

                                        bool exist = false;

                                        for (int ii = 0; ii < globalText.Length; ii++)
                                        {
                                            if (globalText[ii] != "" && globalText[ii] != " " && globalText[ii] != "  ")
                                            {
                                                string[] idValuesGlobal = globalText[ii].Split(SeparatorDifferentGames());
                                                string oldValues = "";

                                                if (idValuesGlobal.Length > 1)
                                                {
                                                    idValuesGlobal[1] = idValuesGlobal[1].Trim();
                                                    oldValues = idValuesGlobal[1];
                                                }
                                                else
                                                    oldValues = idValuesGlobal[0];

                                                idValues[0] = idValues[0].Trim();
                                                idValuesGlobal[0] = idValuesGlobal[0].Trim();

                                                if (idValuesGlobal[0] == idValues[0] && oldValues == newValues)
                                                {
                                                    exist = true;

                                                    break;
                                                }
                                            }
                                        }

                                        if (!exist)
                                        {
                                            line = line.TrimStart();
                                            sw.WriteLine(line);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                fileToRead = localizationPath + @"Global_English.txt";
            }
            else if (gameName == "Temtem")
            {
                if (!File.Exists(fileToRead))
                {
                    File.WriteAllTextAsync(localizationPath + @"unpack_CX.bat", Resources.unpack_CX_bat);
                    File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(fileToRead);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    //File.Copy(fileToRead, localizationPath + @"OriginalLocalization.txt");

                    if (gameName == "A Space for the Unbound")
                    {
                        ReplaceStringInFile(localizationPath + @"unpack_CX.bat", 1, "for %%a in (*.csv) do unPacker_CSV.exe -uc 2 \"%%a\"");
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = localizationPath + @"unpack_CX.bat",
                            WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();
                    }
                }
            }
            else if (gameName == "CoreKeeper")
            {
                if (!File.Exists(fileToRead))
                {
                    File.WriteAllTextAsync(localizationPath + @"unpack_CX.bat", Resources.unpack_CX_bat);
                    File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                    System.Threading.Thread.Sleep(500);

                    ReplaceStringInFile(localizationPath + @"unpack_CX.bat", 1, "for %%a in (Localization.csv) do unPacker_CSV.exe -uc 3 \"%%a\"");
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(fileToRead);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                }
            }
            else if (gameName == "NewWorld")
            {
                if (!File.Exists(localizationPath + @"Global_English.txt"))
                {
                    filesToRead = Directory.GetFiles(localizationPath);

                    for (int i = 0; i < filesToRead.Length; i++)
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(filesToRead[i]);

                        XmlElement xRoot = xDoc.DocumentElement;

                        if (xRoot != null)
                        {
                            foreach (XmlElement xnode in xRoot)
                            {
                                string? id = xnode.Attributes.GetNamedItem("key").Value;
                                string? text = xnode.InnerText;
                                XmlNode commentNode = xnode.Attributes.GetNamedItem("comment");
                                string comment = "";
                                if (commentNode != null)
                                    comment = commentNode.Value;
                                XmlNode genderNode = xnode.Attributes.GetNamedItem("gender");
                                string gender = "";
                                if (genderNode != null)
                                    gender = genderNode.Value;

                                if (id != "")
                                {
                                    if (id.Contains("PerkID") && id.Contains("Name") || filesToRead[i] == "javelindata_itemdefinitions_master.loc.xml" && id.Contains("name")
                                        || filesToRead[i] == "javelindata_itemdefinitions_master.loc.xml" && id.Contains("Name") || !id.Contains("Name") && !id.Contains("name"))
                                    {
                                        if (id.Contains("title") && id.Contains("desc") || id.Contains("title") && id.Contains("description") || !id.Contains("Title"))
                                        {
                                            if (!id.Contains("Prefix") && !id.Contains("Suffix"))
                                            {
                                                if (id.Contains("ui_minor") && id.Contains("description") || id.Contains("ui_major") && id.Contains("description")
                                                    || id.Contains("ui_leaving") && id.Contains("description") || id.Contains("ui_preddieu_farm_attack") && id.Contains("description")
                                                    || id.Contains("ui_boatencounter_brysedd") && id.Contains("description") || id.Contains("ui_boatencounter_scallywag") && id.Contains("description")
                                                    || id.Contains("ui_boatencounter_scallywag") && id.Contains("description") || !id.Contains("ui_") && !id.Contains("_attack")
                                                    || id.Contains("objective_msq_beach_chaplain") && id.Contains("description") || id.Contains("objective_msq_heartforge_guardian") && id.Contains("description")
                                                    || id.Contains("ui_turkeyterror_worldboss_Turkulon") && id.Contains("description") || id.Contains("ui_poi_outpost") && id.Contains("description")
                                                    || id.Contains("ui_or_") && id.Contains("desc") || id.Contains("ui_poi_") && id.Contains("description") || id.Contains("GlobalDesc") && id.Contains("description")
                                                    || id.Contains("ui_alchemy") && id.Contains("desc") || id.Contains("ui_blacksmith") && id.Contains("desc") || id.Contains("ui_carpentry") && id.Contains("desc")
                                                    || id.Contains("ui_masonry") && id.Contains("desc") || id.Contains("ui_smelter") && id.Contains("desc") || id.Contains("inv_equipLoad") && id.Contains("Tooltip")
                                                    || id.Contains("CategoryData") && id.Contains("Desc") || id.Contains("ui_emote") && id.Contains("description") || id.Contains("Status_Rune") && id.Contains("Desc")
                                                    || id.Contains("crafting_azothbonus_") && id.Contains("tooltip"))
                                                {
                                                    if (!id.Contains("url") && !id.Contains("incursion") && !id.Contains("ui_crestforeground") && !id.Contains("reward_type")
                                                        && !id.Contains("owg_rank_bounty") && !id.Contains("owg_target") && id != "owg_unlock_rank" && id != "owg_company" && !id.Contains("hotspot_")
                                                        && !id.Contains("lifestyle_button") && !id.Contains("ui_queue") && id != "ui_provisioning" && id != "ui_repair_trinket" && id != "ui_logging"
                                                        && id != "ui_azoth_staff_tradeskill" && id != "ui_skinning" && id != "ui_fishing" && id != "ui_crafting" && id != "ui_repairing"
                                                        && id != "ui_gathering" && id != "ui_refining" && id != "ui_taxes" && id != "ui_weaponsmithing" && id != "ui_jewelcrafting" && id != "ui_kiln"
                                                        && id != "ui_wildernesssurvival" && !id.Contains("arcana") && !id.Contains("armoring") && !id.Contains("blacksmithing")
                                                        && !id.Contains("_camping") && !id.Contains("_cooking") && !id.Contains("engineering") && !id.Contains("furnishing")
                                                        && !id.Contains("outfitting") && !id.Contains("woodworking") && !id.Contains("mining") && !id.Contains("leatherworking")
                                                        && !id.Contains("harvesting") && !id.Contains("stonecutting") && !id.Contains("smelting") && !id.Contains("weaving")
                                                        && id != "ftue_archetypes_attributes" && id != "ftue_archetypes_tradeskills" && id != "ftue_archetypes_equipment"
                                                        && id != "ftue_archetypes_details" && !id.Contains("ftue_archetypes_attr") && !id.Contains("ftue_archetypes_skill")
                                                        && id != "ui_outpost_rush_summoning_circle" && !id.Contains("cc_") && id != "inv_salvage" && id != "capslock" && id != "backspace"
                                                        && id != "enter" && id != "home" && id != "end" && id != "delete" && id != "pgup" && id == "pgdn" && id != "photomode_keybind"
                                                        && id != "insert" && id != "scrolllock" && id != "print" && id != "pause" && id != "ui_aimsensitivity" && id != "ui_cards" && id != "ui_lmb"
                                                        && id != "ui_lmbrmb" && id != "ui_rmb" && id != "ui_numnlock" && id != "ui_rmbctrl" && id != "ui_tab" && !id.Contains("ui_currency")
                                                        && !id.Contains("ui_coin") && !id.Contains("ui_wallet") && id != "ui_weapon_mastery" && id != "ui_weapon_mastered"
                                                        && id != "ui_tradeskill_mastered" && id != "ui_weapon_mastery_level" && !id.Contains("ui_mace") && !id.Contains("ui_rapier")
                                                        && !id.Contains("ui_throwingaxe") && !id.Contains("ui_spear") && !id.Contains("ui_straightsword") && !id.Contains("ui_greataxe")
                                                        && !id.Contains("ui_greatsword") && !id.Contains("ui_juggernaut") && !id.Contains("ui_crowdcrusher") && !id.Contains("ui_warhammer")
                                                        && !id.Contains("ui_bow") && !id.Contains("ui_musket") && !id.Contains("ui_blunderbuss") && !id.Contains("ui_firemagic")
                                                        && !id.Contains("ui_lifemagic") && !id.Contains("ui_icemagic") && !id.Contains("ui_voidmagic") && !id.Contains("ui_hatchet")
                                                        && !id.Contains("ui_blood") && !id.Contains("ui_evade") && !id.Contains("ui_sword") && !id.Contains("ui_shield") && !id.Contains("ui_turret")
                                                        && !id.Contains("ui_cooldown") && !id.Contains("ui_gearscore") && !id.Contains("ui_bindOnPickup") && !id.Contains("ui_bindOnEquip")
                                                        && !id.Contains("ui_axe") && !id.Contains("ui_pickaxe") && !id.Contains("ui_sickle") && !id.Contains("ui_skinningknife")
                                                        && !id.Contains("hwm_loot") && id != "ui_time_display" && !id.Contains("ui_tooltip") && !id.Contains("ui_dialogueoption")
                                                        && id != "ui_autopinObjectivesSkillProgression" && id != "ui_autopinobjectivesEvent" && id != "ui_autopinObjectivesComm"
                                                        && id != "ui_autopinObjectivesCrafting" && id != "ui_auto_pin_toggle_on" && id != "ui_auto_pin_toggle_off" && id != "ui_number_range"
                                                            && id != "ui_guides" && id != "ui_leftgroupmessage" && id != "ui_guildcresttab" && !id.Contains("ui_Chopping")
                                                            && !id.Contains("ui_Cutting") && !id.Contains("ui_Dressing") && !id.Contains("ui_Mining") && !id.Contains("ui_Fishing")
                                                            && id != "fc_equipbutton" && id != "fc_fieldcrafting" && !id.Contains("ui_physical_defense") && !id.Contains("ui_elemental_defense")
                                                            && !id.Contains("ui_damage_resistance") && id != "ui_Strength_short" && id != "ui_Dexterity_short" && id != "ui_Intelligence_short"
                                                            && id != "ui_Focus_short" && id != "ui_Constitution_short" && id != "tutorial_salvage_items" && !id.Contains("BackstoryLore")
                                                            && !id.Contains("Lore_Chapter_") && !id.Contains("Topic_0") && id != "ui_onehanded_weapons" && id != "ui_twohanded_weapons"
                                                            && id != "ui_magic_skills" && id != "ui_ranged_weapons" && !id.Contains("ui_farmplants") && !id.Contains("ui_farmplants")
                                                            && !id.Contains("ui_aloe") && !id.Contains("ui_alchemy") && !id.Contains("iconTypeUnlock_Pigment") && !id.Contains("ui_smallprey")
                                                            && !id.Contains("ui_mediumprey") && !id.Contains("ui_largeprey") && !id.Contains("ui_smallpredator") && !id.Contains("ui_largepredator")
                                                            && !id.Contains("ui_iron") && !id.Contains("ui_oil") && !id.Contains("ui_silver") && !id.Contains("ui_lodestone")
                                                            && !id.Contains("ui_gold") && !id.Contains("ui_starmetal") && !id.Contains("ui_platinum") && !id.Contains("ui_sandstone")
                                                            && !id.Contains("ui_brimstone") && !id.Contains("ui_orichalcum") && !id.Contains("ui_wyrdwood") && !id.Contains("ui_ironwood")
                                                            && !id.Contains("ui_alchemyStones") && !id.Contains("ui_toolrequired") && !id.Contains("ui_skinning") && !id.Contains("ui_tracking")
                                                            && !id.Contains("iconTypeUnlock_youngtrees") && !id.Contains("iconTypeUnlock_maturetrees") && id != "ui_fishing_hotspot_tutorial_label"
                                                            && !id.Contains("ui_milestone_") && !id.Contains("TerritoryRecommendation_") && id != "ui_new_recommended_territory"
                                                            && !id.Contains("blacksmith") && !id.Contains("cooking") && !id.Contains("engineering") && !id.Contains("outfitting")
                                                            && !id.Contains("alchemy") && !id.Contains("camp") && !id.Contains("masonry") && !id.Contains("weaving") && !id.Contains("milling")
                                                            && !id.Contains("smelting") && !id.Contains("tanning") && !id.Contains("carpentry") && id != "cr_gearscore" && id != "cr_gearscore_alt"
                                                            && id != "cr_requires" && !id.Contains("cr_tier") && !id.Contains("crafting_carried"))
                                                    {

                                                        using (StreamWriter sw = new StreamWriter(localizationPath + @"Global_English.txt", true, encoding: Encoding.UTF8))
                                                        {
                                                            bool doNotTranslate = false;
                                                            if (comment != "")
                                                                if (comment == "DO NOT TRANSLATE")
                                                                    doNotTranslate = true;

                                                            if (!doNotTranslate && gender == "")
                                                                sw.WriteLine(id + "\t" + text);
                                                            else if (!doNotTranslate && gender != "")
                                                                sw.WriteLine(id + "\t" + text + "\t" + gender);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                fileToRead = localizationPath + @"Global_English.txt";
            }

            System.Threading.Thread.Sleep(500);
            if (gameName == "nwr")
            {
                if (!File.Exists(path + @"\backup.txt"))
                    File.Copy(fileToRead, path + @"\backup.txt");
            }
            else
            {
                if (!File.Exists(localizationPath + @"backup.txt"))
                    File.Copy(fileToRead, localizationPath + @"backup.txt");
            }

            if (File.Exists(localizationPath + @"unpack_CX.bat"))
                deleteFile(localizationPath + @"unpack_CX.bat");
            if (File.Exists(localizationPath + @"unPacker_CSV.exe"))
                deleteFile(localizationPath + @"unPacker_CSV.exe");

            if (gameName == "nwr")
                arraysBackup = File.ReadAllLines(path + @"\backup.txt");
            else
                arraysBackup = File.ReadAllLines(localizationPath + @"backup.txt");


            if (installBreak || updateBreak)
            {
                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Проверка переведенных строк...", Color.Black)));
                alreadyTranslated = arraysBackup;
            }
            int progressPercent = 0;
            int rowsInFile = File.ReadAllLines(fileToRead).Length;
            int midpoint = rowsInFile / 20 + 1;
            int doneLines = 0;
            List<string> sourceText = new List<string>();
            List<int> lineNumbers = new List<int>(20);

            if (gameName == "The Mortuary Assistant" || gameName == "NewWorld")
            {
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = 0));
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + rowsInFile + 27));
            }

            string idFileRead = "";
            if (gameName == "Escape Simulator")
                idFileRead = fileToRead;
            else if (gameName == "CoreKeeper")
                idFileRead = localizationPath + @"Localization.csv";
            else if (gameName == "nwr")
                idFileRead = fileToRead;
            else
                idFileRead = localizationPath + FilesNameDifferentGames();

            while (doneLines < midpoint)
            {
                sourceText.Clear();
                lineNumbers.Clear();
                string corpusText = "";

                #region lines
                lineNumbers.Add(doneLines);
                lineNumbers.Add(midpoint + doneLines);
                lineNumbers.Add(midpoint * 2 + doneLines);
                lineNumbers.Add(midpoint * 3 + doneLines);
                lineNumbers.Add(midpoint * 4 + doneLines);
                lineNumbers.Add(midpoint * 5 + doneLines);
                lineNumbers.Add(midpoint * 6 + doneLines);
                lineNumbers.Add(midpoint * 7 + doneLines);
                lineNumbers.Add(midpoint * 8 + doneLines);
                lineNumbers.Add(midpoint * 9 + doneLines);
                lineNumbers.Add(midpoint * 10 + doneLines);
                lineNumbers.Add(midpoint * 11 + doneLines);
                lineNumbers.Add(midpoint * 12 + doneLines);
                lineNumbers.Add(midpoint * 13 + doneLines);
                lineNumbers.Add(midpoint * 14 + doneLines);
                lineNumbers.Add(midpoint * 15 + doneLines);
                lineNumbers.Add(midpoint * 16 + doneLines);
                lineNumbers.Add(midpoint * 17 + doneLines);
                lineNumbers.Add(midpoint * 18 + doneLines);
                lineNumbers.Add(midpoint * 19 + doneLines);

                string idLine = "";
                if (lineNumbers[0] < rowsInFile)
                    idLine = File.ReadLines(idFileRead).Skip(lineNumbers[0]).FirstOrDefault();
                string idLine2 = "";
                if (lineNumbers[1] < rowsInFile)
                    idLine2 = File.ReadLines(idFileRead).Skip(lineNumbers[1]).FirstOrDefault();
                string idLine3 = "";
                if (lineNumbers[2] < rowsInFile)
                    idLine3 = File.ReadLines(idFileRead).Skip(lineNumbers[2]).FirstOrDefault();
                string idLine4 = "";
                if (lineNumbers[3] < rowsInFile)
                    idLine4 = File.ReadLines(idFileRead).Skip(lineNumbers[3]).FirstOrDefault();
                string idLine5 = "";
                if (lineNumbers[4] < rowsInFile)
                    idLine5 = File.ReadLines(idFileRead).Skip(lineNumbers[4]).FirstOrDefault();
                string idLine6 = "";
                if (lineNumbers[5] < rowsInFile)
                    idLine6 = File.ReadLines(idFileRead).Skip(lineNumbers[5]).FirstOrDefault();
                string idLine7 = "";
                if (lineNumbers[6] < rowsInFile)
                    idLine7 = File.ReadLines(idFileRead).Skip(lineNumbers[6]).FirstOrDefault();
                string idLine8 = "";
                if (lineNumbers[7] < rowsInFile)
                    idLine8 = File.ReadLines(idFileRead).Skip(lineNumbers[7]).FirstOrDefault();
                string idLine9 = "";
                if (lineNumbers[8] < rowsInFile)
                    idLine9 = File.ReadLines(idFileRead).Skip(lineNumbers[8]).FirstOrDefault();
                string idLine10 = "";
                if (lineNumbers[9] < rowsInFile)
                    idLine10 = File.ReadLines(idFileRead).Skip(lineNumbers[9]).FirstOrDefault();
                string idLine11 = "";
                if (lineNumbers[10] < rowsInFile)
                    idLine11 = File.ReadLines(idFileRead).Skip(lineNumbers[10]).FirstOrDefault();
                string idLine12 = "";
                if (lineNumbers[11] < rowsInFile)
                    idLine12 = File.ReadLines(idFileRead).Skip(lineNumbers[11]).FirstOrDefault();
                string idLine13 = "";
                if (lineNumbers[12] < rowsInFile)
                    idLine13 = File.ReadLines(idFileRead).Skip(lineNumbers[12]).FirstOrDefault();
                string idLine14 = "";
                if (lineNumbers[13] < rowsInFile)
                    idLine14 = File.ReadLines(idFileRead).Skip(lineNumbers[13]).FirstOrDefault();
                string idLine15 = "";
                if (lineNumbers[14] < rowsInFile)
                    idLine15 = File.ReadLines(idFileRead).Skip(lineNumbers[14]).FirstOrDefault();
                string idLine16 = "";
                if (lineNumbers[15] < rowsInFile)
                    idLine16 = File.ReadLines(idFileRead).Skip(lineNumbers[15]).FirstOrDefault();
                string idLine17 = "";
                if (lineNumbers[16] < rowsInFile)
                    idLine17 = File.ReadLines(idFileRead).Skip(lineNumbers[16]).FirstOrDefault();
                string idLine18 = "";
                if (lineNumbers[17] < rowsInFile)
                    idLine18 = File.ReadLines(idFileRead).Skip(lineNumbers[17]).FirstOrDefault();
                string idLine19 = "";
                if (lineNumbers[18] < rowsInFile)
                    idLine19 = File.ReadLines(idFileRead).Skip(lineNumbers[18]).FirstOrDefault();
                string idLine20 = "";
                if (lineNumbers[19] < rowsInFile)
                    idLine20 = File.ReadLines(idFileRead).Skip(lineNumbers[19]).FirstOrDefault();

                string[] idValues = new string[] { };
                if (idLine != "")
                    idValues = idLine.Split(SeparatorDifferentGames());
                string[] idValues2 = new string[] { };
                if (idLine2 != "")
                    idValues2 = idLine2.Split(SeparatorDifferentGames());
                string[] idValues3 = new string[] { };
                if (idLine3 != "")
                    idValues3 = idLine3.Split(SeparatorDifferentGames());
                string[] idValues4 = new string[] { };
                if (idLine4 != "")
                    idValues4 = idLine4.Split(SeparatorDifferentGames());
                string[] idValues5 = new string[] { };
                if (idLine5 != "")
                    idValues5 = idLine5.Split(SeparatorDifferentGames());
                string[] idValues6 = new string[] { };
                if (idLine6 != "")
                    idValues6 = idLine6.Split(SeparatorDifferentGames());
                string[] idValues7 = new string[] { };
                if (idLine7 != "")
                    idValues7 = idLine7.Split(SeparatorDifferentGames());
                string[] idValues8 = new string[] { };
                if (idLine8 != "")
                    idValues8 = idLine8.Split(SeparatorDifferentGames());
                string[] idValues9 = new string[] { };
                if (idLine9 != "")
                    idValues9 = idLine9.Split(SeparatorDifferentGames());
                string[] idValues10 = new string[] { };
                if (idLine10 != "")
                    idValues10 = idLine10.Split(SeparatorDifferentGames());
                string[] idValues11 = new string[] { };
                if (idLine11 != "")
                    idValues11 = idLine11.Split(SeparatorDifferentGames());
                string[] idValues12 = new string[] { };
                if (idLine12 != "")
                    idValues12 = idLine12.Split(SeparatorDifferentGames());
                string[] idValues13 = new string[] { };
                if (idLine13 != "")
                    idValues13 = idLine13.Split(SeparatorDifferentGames());
                string[] idValues14 = new string[] { };
                if (idLine14 != "")
                    idValues14 = idLine14.Split(SeparatorDifferentGames());
                string[] idValues15 = new string[] { };
                if (idLine15 != "")
                    idValues15 = idLine15.Split(SeparatorDifferentGames());
                string[] idValues16 = new string[] { };
                if (idLine16 != "")
                    idValues16 = idLine16.Split(SeparatorDifferentGames());
                string[] idValues17 = new string[] { };
                if (idLine17 != "")
                    idValues17 = idLine17.Split(SeparatorDifferentGames());
                string[] idValues18 = new string[] { };
                if (idLine18 != "")
                    idValues18 = idLine18.Split(SeparatorDifferentGames());
                string[] idValues19 = new string[] { };
                if (idLine19 != "")
                    idValues19 = idLine19.Split(SeparatorDifferentGames());
                string[] idValues20 = new string[] { };
                if (idLine20 != "")
                    idValues20 = idLine20.Split(SeparatorDifferentGames());

                string curLine = "";
                string curLine2 = "";
                string curLine3 = "";
                string curLine4 = "";
                string curLine5 = "";
                string curLine6 = "";
                string curLine7 = "";
                string curLine8 = "";
                string curLine9 = "";
                string curLine10 = "";
                string curLine11 = "";
                string curLine12 = "";
                string curLine13 = "";
                string curLine14 = "";
                string curLine15 = "";
                string curLine16 = "";
                string curLine17 = "";
                string curLine18 = "";
                string curLine19 = "";
                string curLine20 = "";
                if (gameName == "Temtem" || gameName == "CoreKeeper")
                {
                    if (lineNumbers[0] < rowsInFile)
                        curLine = File.ReadLines(fileToRead).Skip(lineNumbers[0]).FirstOrDefault();
                    if (lineNumbers[1] < rowsInFile)
                        curLine2 = File.ReadLines(fileToRead).Skip(lineNumbers[1]).FirstOrDefault();
                    if (lineNumbers[2] < rowsInFile)
                        curLine3 = File.ReadLines(fileToRead).Skip(lineNumbers[2]).FirstOrDefault();
                    if (lineNumbers[3] < rowsInFile)
                        curLine4 = File.ReadLines(fileToRead).Skip(lineNumbers[3]).FirstOrDefault();
                    if (lineNumbers[4] < rowsInFile)
                        curLine5 = File.ReadLines(fileToRead).Skip(lineNumbers[4]).FirstOrDefault();
                    if (lineNumbers[5] < rowsInFile)
                        curLine6 = File.ReadLines(fileToRead).Skip(lineNumbers[5]).FirstOrDefault();
                    if (lineNumbers[6] < rowsInFile)
                        curLine7 = File.ReadLines(fileToRead).Skip(lineNumbers[6]).FirstOrDefault();
                    if (lineNumbers[7] < rowsInFile)
                        curLine8 = File.ReadLines(fileToRead).Skip(lineNumbers[7]).FirstOrDefault();
                    if (lineNumbers[8] < rowsInFile)
                        curLine9 = File.ReadLines(fileToRead).Skip(lineNumbers[8]).FirstOrDefault();
                    if (lineNumbers[9] < rowsInFile)
                        curLine10 = File.ReadLines(fileToRead).Skip(lineNumbers[9]).FirstOrDefault();
                    if (lineNumbers[10] < rowsInFile)
                        curLine11 = File.ReadLines(fileToRead).Skip(lineNumbers[10]).FirstOrDefault();
                    if (lineNumbers[11] < rowsInFile)
                        curLine12 = File.ReadLines(fileToRead).Skip(lineNumbers[11]).FirstOrDefault();
                    if (lineNumbers[12] < rowsInFile)
                        curLine13 = File.ReadLines(fileToRead).Skip(lineNumbers[12]).FirstOrDefault();
                    if (lineNumbers[13] < rowsInFile)
                        curLine14 = File.ReadLines(fileToRead).Skip(lineNumbers[13]).FirstOrDefault();
                    if (lineNumbers[14] < rowsInFile)
                        curLine15 = File.ReadLines(fileToRead).Skip(lineNumbers[14]).FirstOrDefault();
                    if (lineNumbers[15] < rowsInFile)
                        curLine16 = File.ReadLines(fileToRead).Skip(lineNumbers[15]).FirstOrDefault();
                    if (lineNumbers[16] < rowsInFile)
                        curLine17 = File.ReadLines(fileToRead).Skip(lineNumbers[16]).FirstOrDefault();
                    if (lineNumbers[17] < rowsInFile)
                        curLine18 = File.ReadLines(fileToRead).Skip(lineNumbers[17]).FirstOrDefault();
                    if (lineNumbers[18] < rowsInFile)
                        curLine19 = File.ReadLines(fileToRead).Skip(lineNumbers[18]).FirstOrDefault();
                    if (lineNumbers[19] < rowsInFile) //<=
                        curLine20 = File.ReadLines(fileToRead).Skip(lineNumbers[19]).FirstOrDefault();
                }
                else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                {
                    if (idValues.Length > 0)
                        if (lineNumbers[0] < rowsInFile && idValues[0] != "" && idValues[0] != " ")
                            if (idValues.Length > 2)
                                curLine = idValues[1] + ": " + idValues[2];
                            else
                                curLine = idValues[1];
                    if (idValues2.Length > 0)
                        if (lineNumbers[1] < rowsInFile && idValues2[0] != "" && idValues2[0] != " ")
                            if (idValues2.Length > 2)
                                curLine2 = idValues2[1] + ": " + idValues2[2];
                            else
                                curLine2 = idValues2[1];
                    if (idValues3.Length > 0)
                        if (lineNumbers[2] < rowsInFile && idValues3[0] != "" && idValues3[0] != " ")
                            if (idValues3.Length > 2)
                                curLine3 = idValues3[1] + ": " + idValues3[2];
                            else
                                curLine3 = idValues3[1];
                    if (idValues4.Length > 0)
                        if (lineNumbers[3] < rowsInFile && idValues4[0] != "" && idValues4[0] != " ")
                            if (idValues4.Length > 2)
                                curLine4 = idValues4[1] + ": " + idValues4[2];
                            else
                                curLine4 = idValues4[1];
                    if (idValues5.Length > 0)
                        if (lineNumbers[4] < rowsInFile && idValues5[0] != "" && idValues5[0] != " ")
                            if (idValues5.Length > 2)
                                curLine5 = idValues5[1] + ": " + idValues5[2];
                            else
                                curLine5 = idValues5[1];
                    if (idValues6.Length > 0)
                        if (lineNumbers[5] < rowsInFile && idValues6[0] != "" && idValues6[0] != " ")
                            if (idValues6.Length > 2)
                                curLine6 = idValues6[1] + ": " + idValues6[2];
                            else
                                curLine6 = idValues6[1];
                    if (idValues7.Length > 0)
                        if (lineNumbers[6] < rowsInFile && idValues7[0] != "" && idValues7[0] != " ")
                            if (idValues7.Length > 2)
                                curLine7 = idValues7[1] + ": " + idValues7[2];
                            else
                                curLine7 = idValues7[1];
                    if (idValues8.Length > 0)
                        if (lineNumbers[7] < rowsInFile && idValues8[0] != "" && idValues8[0] != " ")
                            if (idValues8.Length > 2)
                                curLine8 = idValues8[1] + ": " + idValues8[2];
                            else
                                curLine8 = idValues8[1];
                    if (idValues9.Length > 0)
                        if (lineNumbers[8] < rowsInFile && idValues9[0] != "" && idValues9[0] != " ")
                            if (idValues9.Length > 2)
                                curLine9 = idValues9[1] + ": " + idValues9[2];
                            else
                                curLine9 = idValues9[1];
                    if (idValues10.Length > 0)
                        if (lineNumbers[9] < rowsInFile && idValues10[0] != "" && idValues10[0] != " ")
                            if (idValues10.Length > 2)
                                curLine10 = idValues10[1] + ": " + idValues10[2];
                            else
                                curLine10 = idValues10[1];
                    if (idValues11.Length > 0)
                        if (lineNumbers[10] < rowsInFile && idValues11[0] != "" && idValues11[0] != " ")
                            if (idValues11.Length > 2)
                                curLine11 = idValues11[1] + ": " + idValues11[2];
                            else
                                curLine11 = idValues11[1];
                    if (idValues12.Length > 0)
                        if (lineNumbers[11] < rowsInFile && idValues12[0] != "" && idValues12[0] != " ")
                            if (idValues12.Length > 2)
                                curLine12 = idValues12[1] + ": " + idValues12[2];
                            else
                                curLine12 = idValues12[1];
                    if (idValues13.Length > 0)
                        if (lineNumbers[12] < rowsInFile && idValues13[0] != "" && idValues13[0] != " ")
                            if (idValues13.Length > 2)
                                curLine13 = idValues13[1] + ": " + idValues13[2];
                            else
                                curLine13 = idValues13[1];
                    if (idValues14.Length > 0)
                        if (lineNumbers[13] < rowsInFile && idValues14[0] != "" && idValues14[0] != " ")
                            if (idValues14.Length > 2)
                                curLine14 = idValues14[1] + ": " + idValues14[2];
                            else
                                curLine14 = idValues14[1];
                    if (idValues15.Length > 0)
                        if (lineNumbers[14] < rowsInFile && idValues15[0] != "" && idValues15[0] != " ")
                            if (idValues15.Length > 2)
                                curLine15 = idValues15[1] + ": " + idValues15[2];
                            else
                                curLine15 = idValues15[1];
                    if (idValues16.Length > 0)
                        if (lineNumbers[15] < rowsInFile && idValues16[0] != "" && idValues16[0] != " ")
                            if (idValues16.Length > 2)
                                curLine16 = idValues16[1] + ": " + idValues16[2];
                            else
                                curLine16 = idValues16[1];
                    if (idValues17.Length > 0)
                        if (lineNumbers[16] < rowsInFile && idValues17[0] != "" && idValues17[0] != " ")
                            if (idValues17.Length > 2)
                                curLine17 = idValues17[1] + ": " + idValues17[2];
                            else
                                curLine17 = idValues17[1];
                    if (idValues18.Length > 0)
                        if (lineNumbers[17] < rowsInFile && idValues18[0] != "" && idValues18[0] != " ")
                            if (idValues18.Length > 2)
                                curLine18 = idValues18[1] + ": " + idValues18[2];
                            else
                                curLine18 = idValues18[1];
                    if (idValues19.Length > 0)
                        if (lineNumbers[18] < rowsInFile && idValues19[0] != "" && idValues19[0] != " ")
                            if (idValues19.Length > 2)
                                curLine19 = idValues19[1] + ": " + idValues19[2];
                            else
                                curLine19 = idValues19[1];
                    if (idValues20.Length > 0)
                        if (lineNumbers[19] < rowsInFile && idValues20[0] != "" && idValues20[0] != " ") //<=
                            if (idValues20.Length > 2)
                                curLine20 = idValues20[1] + ": " + idValues20[2];
                            else
                                curLine20 = idValues20[1];
                }
                else if (gameName == "NewWorld" || gameName == "DungeonCrawler")
                {
                    if (lineNumbers[0] < rowsInFile)
                        curLine = File.ReadLines(fileToRead).Skip(lineNumbers[0]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[1] < rowsInFile)
                        curLine2 = File.ReadLines(fileToRead).Skip(lineNumbers[1]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[2] < rowsInFile)
                        curLine3 = File.ReadLines(fileToRead).Skip(lineNumbers[2]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[3] < rowsInFile)
                        curLine4 = File.ReadLines(fileToRead).Skip(lineNumbers[3]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[4] < rowsInFile)
                        curLine5 = File.ReadLines(fileToRead).Skip(lineNumbers[4]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[5] < rowsInFile)
                        curLine6 = File.ReadLines(fileToRead).Skip(lineNumbers[5]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[6] < rowsInFile)
                        curLine7 = File.ReadLines(fileToRead).Skip(lineNumbers[6]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[7] < rowsInFile)
                        curLine8 = File.ReadLines(fileToRead).Skip(lineNumbers[7]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[8] < rowsInFile)
                        curLine9 = File.ReadLines(fileToRead).Skip(lineNumbers[8]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[9] < rowsInFile)
                        curLine10 = File.ReadLines(fileToRead).Skip(lineNumbers[9]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[10] < rowsInFile)
                        curLine11 = File.ReadLines(fileToRead).Skip(lineNumbers[10]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[11] < rowsInFile)
                        curLine12 = File.ReadLines(fileToRead).Skip(lineNumbers[11]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[12] < rowsInFile)
                        curLine13 = File.ReadLines(fileToRead).Skip(lineNumbers[12]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[13] < rowsInFile)
                        curLine14 = File.ReadLines(fileToRead).Skip(lineNumbers[13]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[14] < rowsInFile)
                        curLine15 = File.ReadLines(fileToRead).Skip(lineNumbers[14]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[15] < rowsInFile)
                        curLine16 = File.ReadLines(fileToRead).Skip(lineNumbers[15]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[16] < rowsInFile)
                        curLine17 = File.ReadLines(fileToRead).Skip(lineNumbers[16]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[17] < rowsInFile)
                        curLine18 = File.ReadLines(fileToRead).Skip(lineNumbers[17]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[18] < rowsInFile)
                        curLine19 = File.ReadLines(fileToRead).Skip(lineNumbers[18]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                    if (lineNumbers[19] < rowsInFile) //<=
                        curLine20 = File.ReadLines(fileToRead).Skip(lineNumbers[19]).FirstOrDefault().Split(SeparatorDifferentGames())[1];
                }
                else if (gameName == "nwr")
                {
                    if (idValues.Length > 0)
                        if (lineNumbers[0] < rowsInFile && idValues[0] != "" && idValues[0] != " " && idValues[0] != "{" && idValues[0] != "}")
                            if (idValues.Length > 3)
                                curLine = idValues[3];
                    if (idValues2.Length > 0)
                        if (lineNumbers[1] < rowsInFile && idValues2[0] != "" && idValues2[0] != " " && idValues2[0] != "{" && idValues2[0] != "}")
                            if (idValues2.Length > 3)
                                curLine2 = idValues2[3];
                    if (idValues3.Length > 0)
                        if (lineNumbers[2] < rowsInFile && idValues3[0] != "" && idValues3[0] != " " && idValues3[0] != "{" && idValues3[0] != "}")
                            if (idValues3.Length > 3)
                                curLine3 = idValues3[3];
                    if (idValues4.Length > 0)
                        if (lineNumbers[3] < rowsInFile && idValues4[0] != "" && idValues4[0] != " " && idValues4[0] != "{" && idValues4[0] != "}")
                            if (idValues4.Length > 3)
                                curLine4 = idValues4[3];
                    if (idValues5.Length > 0)
                        if (lineNumbers[4] < rowsInFile && idValues5[0] != "" && idValues5[0] != " " && idValues5[0] != "{" && idValues5[0] != "}")
                            if (idValues5.Length > 3)
                                curLine5 = idValues5[3];
                    if (idValues6.Length > 0)
                        if (lineNumbers[5] < rowsInFile && idValues6[0] != "" && idValues6[0] != " " && idValues6[0] != "{" && idValues6[0] != "}")
                            if (idValues6.Length > 3)
                                curLine6 = idValues6[3];
                    if (idValues7.Length > 0)
                        if (lineNumbers[6] < rowsInFile && idValues7[0] != "" && idValues7[0] != " " && idValues7[0] != "{" && idValues7[0] != "}")
                            if (idValues7.Length > 3)
                                curLine7 = idValues7[3];
                    if (idValues8.Length > 0)
                        if (lineNumbers[7] < rowsInFile && idValues8[0] != "" && idValues8[0] != " " && idValues8[0] != "{" && idValues8[0] != "}")
                            if (idValues8.Length > 3)
                                curLine8 = idValues8[3];
                    if (idValues9.Length > 0)
                        if (lineNumbers[8] < rowsInFile && idValues9[0] != "" && idValues9[0] != " " && idValues9[0] != "{" && idValues9[0] != "}")
                            if (idValues9.Length > 3)
                                curLine9 = idValues9[3];
                    if (idValues10.Length > 0)
                        if (lineNumbers[9] < rowsInFile && idValues10[0] != "" && idValues10[0] != " " && idValues10[0] != "{" && idValues10[0] != "}")
                            if (idValues10.Length > 3)
                                curLine10 = idValues10[3];
                    if (idValues11.Length > 0)
                        if (lineNumbers[10] < rowsInFile && idValues11[0] != "" && idValues11[0] != " " && idValues11[0] != "{" && idValues11[0] != "}")
                            if (idValues11.Length > 3)
                                curLine11 = idValues11[3];
                    if (idValues12.Length > 0)
                        if (lineNumbers[11] < rowsInFile && idValues12[0] != "" && idValues12[0] != " " && idValues12[0] != "{" && idValues12[0] != "}")
                            if (idValues12.Length > 3)
                                curLine12 = idValues12[3];
                    if (idValues13.Length > 0)
                        if (lineNumbers[12] < rowsInFile && idValues13[0] != "" && idValues13[0] != " " && idValues13[0] != "{" && idValues13[0] != "}")
                            if (idValues13.Length > 3)
                                curLine13 = idValues13[3];
                    if (idValues14.Length > 0)
                        if (lineNumbers[13] < rowsInFile && idValues14[0] != "" && idValues14[0] != " " && idValues14[0] != "{" && idValues14[0] != "}")
                            if (idValues14.Length > 3)
                                curLine14 = idValues14[3];
                    if (idValues15.Length > 0)
                        if (lineNumbers[14] < rowsInFile && idValues15[0] != "" && idValues15[0] != " " && idValues15[0] != "{" && idValues15[0] != "}")
                            if (idValues15.Length > 3)
                                curLine15 = idValues15[3];
                    if (idValues16.Length > 0)
                        if (lineNumbers[15] < rowsInFile && idValues16[0] != "" && idValues16[0] != " " && idValues16[0] != "{" && idValues16[0] != "}")
                            if (idValues16.Length > 3)
                                curLine16 = idValues16[3];
                    if (idValues17.Length > 0)
                        if (lineNumbers[16] < rowsInFile && idValues17[0] != "" && idValues17[0] != " " && idValues17[0] != "{" && idValues17[0] != "}")
                            if (idValues17.Length > 3)
                                curLine17 = idValues17[3];
                    if (idValues18.Length > 0)
                        if (lineNumbers[17] < rowsInFile && idValues18[0] != "" && idValues18[0] != " " && idValues18[0] != "{" && idValues18[0] != "}")
                            if (idValues18.Length > 3)
                                curLine18 = idValues18[3];
                    if (idValues19.Length > 0)
                        if (lineNumbers[18] < rowsInFile && idValues19[0] != "" && idValues19[0] != " " && idValues19[0] != "{" && idValues19[0] != "}")
                            if (idValues19.Length > 3)
                                curLine19 = idValues19[3];
                    if (idValues20.Length > 0)
                        if (lineNumbers[19] < rowsInFile && idValues20[0] != "" && idValues20[0] != " " && idValues20[0] != "{" && idValues20[0] != "}") //<=
                            if (idValues20.Length > 3)
                                curLine20 = idValues20[3];
                }
                #endregion

                #region add in lists
                if (curLine != "")
                    sourceText.Add(ReadText(idValues[0], curLine, lineNumbers[0]));
                if (curLine2 != "")
                    sourceText.Add(ReadText(idValues2[0], curLine2, lineNumbers[1]));
                if (curLine3 != "")
                    sourceText.Add(ReadText(idValues3[0], curLine3, lineNumbers[2]));
                if (curLine4 != "")
                    sourceText.Add(ReadText(idValues4[0], curLine4, lineNumbers[3]));
                if (curLine5 != "")
                    sourceText.Add(ReadText(idValues5[0], curLine5, lineNumbers[4]));
                if (curLine6 != "")
                    sourceText.Add(ReadText(idValues6[0], curLine6, lineNumbers[5]));
                if (curLine7 != "")
                    sourceText.Add(ReadText(idValues7[0], curLine7, lineNumbers[6]));
                if (curLine8 != "")
                    sourceText.Add(ReadText(idValues8[0], curLine8, lineNumbers[7]));
                if (curLine9 != "")
                    sourceText.Add(ReadText(idValues9[0], curLine9, lineNumbers[8]));
                if (curLine10 != "")
                    sourceText.Add(ReadText(idValues10[0], curLine10, lineNumbers[9]));
                if (curLine11 != "")
                    sourceText.Add(ReadText(idValues11[0], curLine11, lineNumbers[10]));
                if (curLine12 != "")
                    sourceText.Add(ReadText(idValues12[0], curLine12, lineNumbers[11]));
                if (curLine13 != "")
                    sourceText.Add(ReadText(idValues13[0], curLine13, lineNumbers[12]));
                if (curLine14 != "")
                    sourceText.Add(ReadText(idValues14[0], curLine14, lineNumbers[13]));
                if (curLine15 != "")
                    sourceText.Add(ReadText(idValues15[0], curLine15, lineNumbers[14]));
                if (curLine16 != "")
                    sourceText.Add(ReadText(idValues16[0], curLine16, lineNumbers[15]));
                if (curLine17 != "")
                    sourceText.Add(ReadText(idValues17[0], curLine17, lineNumbers[16]));
                if (curLine18 != "")
                    sourceText.Add(ReadText(idValues18[0], curLine18, lineNumbers[17]));
                if (curLine19 != "")
                    sourceText.Add(ReadText(idValues19[0], curLine19, lineNumbers[18]));
                if (curLine20 != "")
                    sourceText.Add(ReadText(idValues20[0], curLine20, lineNumbers[19]));
                #endregion

                doneLines = doneLines + 1;

                for (int i = 0; i < sourceText.Count; i++)
                {
                    sourceText[i] = sourceText[i].TrimStart();

                    if (gameName == "Escape Simulator")
                    {
                        sourceText[i] = sourceText[i].TrimEnd('"');
                        sourceText[i] = sourceText[i].TrimStart('"');
                    }

                    if (gameName == "The Mortuary Assistant")
                    {
                        sourceText[i] = sourceText[i].TrimEnd(',');
                        sourceText[i] = sourceText[i].TrimEnd('"');
                        sourceText[i] = sourceText[i].TrimStart('"');
                    }

                    if (corpusText.Length + sourceText[i].Length + 4 >= 4000)
                        break;

                    if (sourceText[i] != "" && corpusText == "")
                    {
                        corpusText = string.Join("\n____\n", sourceText[i]);
                    }
                    else if (sourceText[i] != "" && corpusText != "")
                    {
                        corpusText = string.Join("\n____\n", corpusText, sourceText[i]);
                    }
                }

                if (progressBar.Value + 20 >= progressBar.Maximum)
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value -= 20));

                //if (corpusText != "")
                //{
                Translator(corpusText, lineNumbers);
                //}

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 20));
                progressPercent = (progressBar.Value + 20) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
            }

            if (driver != null)
            {
                driver.Close();
                driver.Quit();
                driver = null;

                System.Threading.Thread.Sleep(500);

                //if (Process.GetProcessesByName("geckodriver").Length > 0)
                cmdKill("geckodriver.exe");

                //if (Process.GetProcessesByName("firefox").Length > 0)
                cmdKill("firefox.exe");
            }

            System.Threading.Thread.Sleep(500);

            if (File.Exists(Application.StartupPath + @"geckodriver.exe"))
                deleteFile(Application.StartupPath + @"geckodriver.exe");

            if (gameName == "The Mortuary Assistant")
            {
                string[] translatedText = File.ReadAllLines(localizationPath + @"backup.txt");
                deleteFile(localizationPath + @"backup.txt");

                for (int i = 0; i < translatedText.Length; i++)
                {
                    if (translatedText[i] != "" && translatedText[i] != " ")
                    {
                        if (gameName == "The Mortuary Assistant")
                            translatedText[i] = "\"<size=60%>" + translatedText[i] + "</size>\",";
                    }
                }

                File.WriteAllLines(localizationPath + @"backup.txt", translatedText);
            }

            if (gameName != "CoreKeeper" && !translateUpdate && gameName != "nwr")
            {
                deleteFile(fileToRead);
                System.Threading.Thread.Sleep(500);
                File.Copy(localizationPath + @"backup.txt", fileToRead);
                System.Threading.Thread.Sleep(500);
                deleteFile(localizationPath + @"backup.txt");
                deleteFile(localizationPath + @"OriginalLocalization.txt");
            }

            if (gameName == "nwr")
            {
                File.Copy(path + @"\backup.txt", path + @"\addon_russian.txt");
                System.Threading.Thread.Sleep(500);
                deleteFile(path + @"\backup.txt");
            }

            if (translateUpdate)
            {
                MessageBox.Show(
                         "Корректировка новых переведённых строк",
                         "Корректировка",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                if (gameName != "NewWorld" && gameName != "CoreKeeper")
                {
                    if (gameName != "The Mortuary Assistant")
                        deleteFile(fileToRead);

                    if (gameName != "DungeonCrawler") // временно, потом убрать, когда разберусь с экспортом через quickbms
                    {
                        unArchivBackup();
                        System.Threading.Thread.Sleep(500);

                        unArchivTranslatedBackup();
                    }
                    System.Threading.Thread.Sleep(500);

                    if (gameName == "The Mortuary Assistant")
                    {
                        string[] backupFilesToRead = Directory.GetFiles(backupTranslatedLocalizationPath);

                        for (int i = 0; i < backupFilesToRead.Length; i++)
                        {
                            string[] globalText = new string[] { };

                            if (File.Exists(backupTranslatedLocalizationPath + @"\Global_English.txt"))
                                globalText = File.ReadAllLines(backupTranslatedLocalizationPath + @"\Global_English.txt");

                            using (StreamReader reader = new StreamReader(backupFilesToRead[i], encoding: Encoding.UTF8))
                            {
                                using (StreamWriter sw = new StreamWriter(backupTranslatedLocalizationPath + @"\Global_English.txt", true, encoding: Encoding.UTF8))
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        string line = reader.ReadLine();

                                        if (line != "{" && line != "}" && line != "" && line != " " && line != "  ")
                                        {
                                            string[] idValues = line.Split(SeparatorDifferentGames());
                                            idValues[0] = idValues[0].TrimStart();
                                            bool exist = false;

                                            for (int ii = 0; ii < globalText.Length; ii++)
                                            {
                                                string[] idValuesGlobal = globalText[ii].Split(SeparatorDifferentGames());

                                                if (idValuesGlobal[0] == idValues[0])
                                                {
                                                    exist = true;

                                                    break;
                                                }
                                            }

                                            if (!exist)
                                            {
                                                line = line.TrimStart();
                                                sw.WriteLine(line);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(500);

                        backupFilesToRead = Directory.GetFiles(backupOldLocalizationPath);

                        for (int i = 0; i < backupFilesToRead.Length; i++)
                        {
                            string[] globalText = new string[] { };
                            if (File.Exists(backupOldLocalizationPath + @"\Global_English.txt"))
                                globalText = File.ReadAllLines(backupOldLocalizationPath + @"\Global_English.txt");

                            using (StreamReader reader = new StreamReader(backupFilesToRead[i], encoding: Encoding.UTF8))
                            {
                                using (StreamWriter sw = new StreamWriter(backupOldLocalizationPath + @"\Global_English.txt", true, encoding: Encoding.UTF8))
                                {
                                    while (!reader.EndOfStream)
                                    {
                                        string line = reader.ReadLine();

                                        if (line != "{" && line != "}" && line != "" && line != " " && line != "  ")
                                        {
                                            string[] idValues = line.Split(SeparatorDifferentGames());
                                            idValues[0] = idValues[0].TrimStart();
                                            bool exist = false;

                                            for (int ii = 0; ii < globalText.Length; ii++)
                                            {
                                                string[] idValuesGlobal = globalText[ii].Split(SeparatorDifferentGames());

                                                if (idValuesGlobal[0] == idValues[0])
                                                {
                                                    exist = true;

                                                    break;
                                                }
                                            }

                                            if (!exist)
                                            {
                                                line = line.TrimStart();
                                                sw.WriteLine(line);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (gameName == "Temtem")
                    {
                        File.WriteAllTextAsync(backupTranslatedLocalizationPath + @"\I2.Loc\unpack_CX.bat", Resources.unpack_CX_bat);
                        File.WriteAllBytes(backupTranslatedLocalizationPath + @"\I2.Loc\unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                        System.Threading.Thread.Sleep(500);
                        if (gameName == "A Space for the Unbound")
                            ReplaceStringInFile(backupTranslatedLocalizationPath + @"\I2.Loc\unpack_CX.bat", 1, "for %%a in (*.csv) do unPacker_CSV.exe -uc 3 \"%%a\"");
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = backupTranslatedLocalizationPath + @"\I2.Loc\unpack_CX.bat",
                            WorkingDirectory = Path.GetDirectoryName(backupTranslatedLocalizationPath + @"\I2.Loc\unpack_CX.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();

                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages_4.txt");
                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages_3.txt");
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = backupTranslatedLocalizationPath + @"\I2.Loc\unpack_CX.bat",
                            WorkingDirectory = Path.GetDirectoryName(backupTranslatedLocalizationPath + @"\I2.Loc\unpack_CX.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();

                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\unpack_CX.bat");
                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\unPacker_CSV.exe");

                        File.WriteAllTextAsync(backupOldLocalizationPath + @"\I2.Loc\unpack_CX.bat", Resources.unpack_CX_bat);
                        File.WriteAllBytes(backupOldLocalizationPath + @"\I2.Loc\unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                        System.Threading.Thread.Sleep(500);
                        if (gameName == "A Space for the Unbound")
                            ReplaceStringInFile(backupOldLocalizationPath + @"\I2.Loc\unpack_CX.bat", 1, "for %%a in (*.csv) do unPacker_CSV.exe -uc 3 \"%%a\"");
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = backupOldLocalizationPath + @"\I2.Loc\unpack_CX.bat",
                            WorkingDirectory = Path.GetDirectoryName(backupOldLocalizationPath + @"\I2.Loc\unpack_CX.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();

                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\I2Languages_4.txt");
                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\I2Languages_3.txt");
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = backupOldLocalizationPath + @"\I2.Loc\unpack_CX.bat",
                            WorkingDirectory = Path.GetDirectoryName(backupOldLocalizationPath + @"\I2.Loc\unpack_CX.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();

                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\unpack_CX.bat");
                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\unPacker_CSV.exe");
                    }

                    string EnglishOriginal = textFromOriginalNew;

                    string[] textFromTranslatedBackup = new string[] { };
                    if (gameName == "Temtem")
                        textFromTranslatedBackup = File.ReadAllLines(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages_4.txt");
                    else if (gameName == "Escape Simulator")
                        textFromTranslatedBackup = File.ReadAllLines(backupTranslatedLocalizationPath + @"\English.txt");
                    else if (gameName == "The Mortuary Assistant")
                        textFromTranslatedBackup = File.ReadAllLines(backupTranslatedLocalizationPath + @"\Global_English.txt");
                    else if (gameName == "DungeonCrawler")
                        textFromTranslatedBackup = File.ReadAllLines(backupTranslatedLocalizationPath + @"\Game.locres.txt");

                    string[] textFromOldBackup = new string[] { };
                    if (gameName == "Temtem")
                        textFromOldBackup = File.ReadAllLines(backupOldLocalizationPath + @"\I2.Loc\I2Languages_4.txt");
                    else if (gameName == "Escape Simulator")
                        textFromOldBackup = File.ReadAllLines(backupOldLocalizationPath + @"\English.txt");
                    else if (gameName == "The Mortuary Assistant")
                        textFromOldBackup = File.ReadAllLines(backupOldLocalizationPath + @"\Global_English.txt");
                    else if (gameName == "DungeonCrawler")
                        textFromOldBackup = File.ReadAllLines(backupOldLocalizationPath + @"\Game.locres.txt");

                    System.Threading.Thread.Sleep(500);

                    if (gameName == "The Mortuary Assistant")
                        deleteFile(fileToRead);
                    if (gameName != "DungeonCrawler")
                    {
                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\I2Languages_4.txt");
                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\I2Languages_3.txt");
                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages_4.txt");
                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages_3.txt");
                    }

                    int rowsInNewFile = newLinesNumber + textFromTranslatedBackup.Length;

                    if (gameName == "Escape Simulator")
                    {
                        using (StreamWriter sw = new StreamWriter(localizationPath + @"OriginalLocalization.txt", false, encoding: Encoding.UTF8))
                        {
                            for (int i = 0; i < textArrayFromOriginalNew.Length; i++)
                            {
                                if (textArrayFromOriginalNew[i] != "" && textArrayFromOriginalNew[i] != " ")
                                {
                                    if (!textArrayFromOriginalNew[i].Contains("#") || textArrayFromOriginalNew[i].Contains("#") && textArrayFromOriginalNew[i].Contains(":"))
                                    {
                                        sw.WriteLine(textArrayFromOriginalNew[i]);
                                    }
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(500);

                        textArrayFromOriginalNew = File.ReadAllLines(localizationPath + @"OriginalLocalization.txt");
                        System.Threading.Thread.Sleep(500);
                        deleteFile(localizationPath + @"OriginalLocalization.txt");

                        deleteFile(backupOldLocalizationPath + @"\English.txt");

                        using (StreamWriter sw = new StreamWriter(backupOldLocalizationPath + @"\English.txt", false, encoding: Encoding.UTF8))
                        {
                            for (int i = 0; i < textFromOldBackup.Length; i++)
                            {
                                if (textFromOldBackup[i] != "" && textFromOldBackup[i] != " ")
                                {
                                    if (!textFromOldBackup[i].Contains("#") || textFromOldBackup[i].Contains("#") && textFromOldBackup[i].Contains(":"))
                                    {
                                        sw.WriteLine(textFromOldBackup[i]);
                                    }
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(500);

                        textFromOldBackup = File.ReadAllLines(backupOldLocalizationPath + @"\English.txt");
                    }

                    string[] arrayS = textArrayFromOriginalNew;

                    string[] newTranslatedText = File.ReadAllLines(localizationPath + @"backup.txt");

                    for (int i = 0; i < newTranslatedText.Length; i++)
                    {
                        newTranslatedText[i] = newTranslatedText[i].TrimStart();

                        if (gameName == "DungeonCrawler")
                        {
                            string[] t = arrayS[newLinesNumbers[i]].Split(SeparatorDifferentGames());

                            arrayS[newLinesNumbers[i]] = t[0] + SeparatorDifferentGames() + newTranslatedText[i];
                        }
                        else
                            arrayS[newLinesNumbers[i]] = newTranslatedText[i];
                    }

                    for (int i = 0; i < arrayS.Length; i++)
                    {
                        if (!hasRuLetters(arrayS[i]))
                        {
                            for (int ii = 0; ii < textFromOldBackup.Length; ii++)
                            {
                                if (arrayS[i] == textFromOldBackup[ii])
                                {
                                    arrayS[i] = textFromTranslatedBackup[ii];

                                    break;
                                }
                            }
                        }
                    }

                    if (gameName == "Temtem")
                    {
                        deleteFile(localizationPath + @"I2Languages2.csv");
                        System.Threading.Thread.Sleep(500);
                        deleteFile(localizationPath + @"I2Languages.csv");
                        System.Threading.Thread.Sleep(500);

                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv");
                        deleteFile(backupPath + @"Export_l2.bat");
                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\Parser_ULS.exe");
                        deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\export_uls.bat");
                        delFolder(backupTranslatedLocalizationPath + @"\I2.Loc");
                        delFolder(backupTranslatedLocalizationPath);
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono");

                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\I2Languages.csv");
                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\Parser_ULS.exe");
                        deleteFile(backupOldLocalizationPath + @"\I2.Loc\export_uls.bat");
                        delFolder(backupOldLocalizationPath + @"\I2.Loc");
                        delFolder(backupOldLocalizationPath);
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono");
                    }
                    else if (gameName == "Escape Simulator")
                    {
                        deleteFile(backupTranslatedLocalizationPath + @"\English.txt");
                        deleteFile(localizationPath + @"English2.txt");
                        deleteFile(localizationPath + @"English.txt");
                        deleteFile(backupPath + @"Export_txt.bat");
                        delFolder(backupTranslatedLocalizationPath);

                        deleteFile(backupOldLocalizationPath + @"\English.txt");
                        delFolder(backupOldLocalizationPath);
                    }
                    else if (gameName == "The Mortuary Assistant")
                    {
                        deleteFile(backupTranslatedLocalizationPath + @"\Global_English.txt");
                        deleteFile(localizationPath + @"Global_English2.txt");
                        deleteFile(localizationPath + @"Global_English.txt");
                        deleteFile(backupPath + @"Export_txt.bat");
                        delFolder(backupTranslatedLocalizationPath);

                        deleteFile(backupOldLocalizationPath + @"\Global_English.txt");
                        delFolder(backupOldLocalizationPath);
                    }
                    else if (gameName == "DungeonCrawler")
                    {
                        deleteFile(fileToRead);
                    }

                    if (gameName != "DungeonCrawler")
                    {
                        if (File.Exists(backupPath + @"UPacker.exe"))
                            deleteFile(backupPath + @"UPacker.exe");
                        if (File.Exists(backupPath + @"UPacker_Advanced.exe"))
                            deleteFile(backupPath + @"UPacker_Advanced.exe");
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup");
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                        delFolder(backupPath + "Unity_Assets_Files");

                        if (File.Exists(backupPath + gameName + "_translated_backup.assets"))
                        {
                            File.Copy(backupPath + gameName + "_translated_backup.assets", backupPath + gameName + "_translated_backup");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(backupPath + gameName + "_translated_backup.assets");
                        }

                        if (File.Exists(backupPath + gameName + "_backup.assets"))
                        {
                            File.Copy(backupPath + gameName + "_backup.assets", backupPath + gameName + "_backup");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(backupPath + gameName + "_backup.assets");
                        }
                    }
                    else
                    {
                        if (File.Exists(backupPath + gameName + "_translated_backup.pak"))
                        {
                            File.Copy(backupPath + gameName + "_translated_backup.pak", backupPath + gameName + "_translated_backup");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(backupPath + gameName + "_translated_backup.pak");
                        }

                        if (File.Exists(backupPath + gameName + "_backup.pak"))
                        {
                            File.Copy(backupPath + gameName + "_backup.pak", backupPath + gameName + "_backup");
                            System.Threading.Thread.Sleep(500);
                            deleteFile(backupPath + gameName + "_backup.pak");
                        }
                    }

                    deleteFile(localizationPath + @"backup.txt");

                    if (gameName == "Temtem")
                        fileToRead = localizationPath + @"I2Languages_4.txt";

                    File.WriteAllLines(fileToRead, arrayS);
                } // DungeonCrawler доделать
                if (gameName == "CoreKeeper")
                {
                    string[] newOriginalText = File.ReadAllLines(fileToRead);
                    deleteFile(fileToRead);
                    string[] translatedText = File.ReadAllLines(localizationPath + @"backup.txt");

                    File.WriteAllTextAsync(localizationPath + @"unpack_CX.bat", Resources.unpack_CX_bat);
                    File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                    System.Threading.Thread.Sleep(500);

                    ReplaceStringInFile(localizationPath + @"unpack_CX.bat", 1, "for %%a in (Localization.csv) do unPacker_CSV.exe -uc 3 \"%%a\"");
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    deleteFile(fileToRead);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"unpack_CX.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"unpack_CX.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();

                    //File.Copy(localizationPath + @"Localization_3.txt", localizationPath + @"Localization_3_2.txt");

                    deleteFile(localizationPath + @"unpack_CX.bat");
                    deleteFile(localizationPath + @"unPacker_CSV.exe");

                    string[] arrayS = File.ReadAllLines(fileToRead);
                    System.Threading.Thread.Sleep(500);
                    deleteFile(fileToRead);

                    for (int i = 0; i < translatedText.Length; i++)
                    {
                        arrayS[newLinesNumbers[i]] = translatedText[i];
                    }

                    //int pos = 0;
                    //for (int i = 0; i < translatedText.Length; i++)
                    //{
                    //    using (StreamReader reader = new StreamReader(localizationPath + @"Localization_3.txt", encoding: Encoding.UTF8))
                    //    {
                    //        while (!reader.EndOfStream)
                    //        {
                    //            string line = reader.ReadLine();
                    //
                    //            if (line != "" && line != " ")
                    //            {
                    //                if (line == newOriginalText[i])
                    //                {
                    //                    ReplaceStringInFile(localizationPath + @"Localization_3_2.txt", pos, translatedText[i]);
                    //                }
                    //            }
                    //
                    //            pos = pos + 1;
                    //        }
                    //    }
                    //}

                    deleteFile(localizationPath + @"backup.txt");

                    deleteFile(backupPath + @"Localization_3.txt");
                    deleteFile(backupPath + @"unpack_CX.bat");
                    deleteFile(backupPath + @"unPacker_CSV.exe");

                    File.WriteAllLines(fileToRead, arrayS);
                }
            } // изменить для Escape Simulator

            if (gameName == "Temtem")
            {
                if (translateUpdate)
                {
                    deleteFile(localizationPath + FilesNameDifferentGames());
                    System.Threading.Thread.Sleep(500);
                    File.Copy(localizationPath + @"OriginalLocalization.txt", localizationPath + FilesNameDifferentGames());
                    System.Threading.Thread.Sleep(500);
                    deleteFile(localizationPath + @"OriginalLocalization.txt");
                }

                File.WriteAllTextAsync(localizationPath + @"pack_CX.bat", Resources.pack_CX_bat);
                File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                System.Threading.Thread.Sleep(500);
                if (gameName == "A Space for the Unbound")
                    ReplaceStringInFile(localizationPath + @"pack_CX.bat", 1, @"for %%a in (*.csv) do unPacker_CSV.exe -pc 3 ""%%a""");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = localizationPath + @"pack_CX.bat",
                    WorkingDirectory = Path.GetDirectoryName(localizationPath + @"pack_CX.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
                System.Threading.Thread.Sleep(500);

                deleteFile(localizationPath + @"pack_CX.bat");
                deleteFile(localizationPath + @"unPacker_CSV.exe");
                deleteFile(fileToRead); // ?
            }

            if (gameName == "The Mortuary Assistant")
            {
                for (int i = 0; i < filesToRead.Length; i++)
                {
                    string[] originalLines = File.ReadAllLines(filesToRead[i]);
                    string[] globalTranslatedLines = File.ReadAllLines(fileToRead);

                    for (int ii = 0; ii < originalLines.Length; ii++)
                    {
                        string[] idValues = originalLines[ii].Split(SeparatorDifferentGames());

                        bool witeSpaces4 = false;
                        if (idValues[0].Contains("    "))
                            witeSpaces4 = true;

                        idValues[0] = idValues[0].TrimStart();

                        for (int iii = 0; iii < globalTranslatedLines.Length; iii++)
                        {
                            string[] idValuesGlobal = globalTranslatedLines[iii].Split(SeparatorDifferentGames());
                            if (translateUpdate)
                                idValuesGlobal[0] = idValuesGlobal[0].TrimStart();

                            if (idValuesGlobal[0] == idValues[0])
                            {
                                if (!translateUpdate)
                                {
                                    if (witeSpaces4)
                                        ReplaceStringInFile(filesToRead[i], ii, "    " + globalTranslatedLines[iii]);
                                    else
                                        ReplaceStringInFile(filesToRead[i], ii, "  " + globalTranslatedLines[iii]);
                                }
                                else
                                {
                                    if (witeSpaces4)
                                        ReplaceStringInFile(filesToRead[i], ii, "  " + globalTranslatedLines[iii]);
                                    else
                                        ReplaceStringInFile(filesToRead[i], ii, globalTranslatedLines[iii]);
                                }

                                break;
                            }
                        }
                    }
                }

                for (int i = 0; i < filesToRead.Length; i++)
                {
                    string[] originalLines = File.ReadAllLines(filesToRead[i]);

                    for (int ii = 0; ii < originalLines.Length; ii++)
                    {
                        if (ii == originalLines.Length - 1)
                        {
                            originalLines[ii] = originalLines[ii].TrimEnd(',');
                            ReplaceStringInFile(filesToRead[i], ii, originalLines[ii]);

                            break;

                            //if (originalLines[ii] != "  ")
                            //{
                            //    originalLines[ii] = originalLines[ii].TrimEnd(',');
                            //    ReplaceStringInFile(filesToRead[i], ii, originalLines[ii]);
                            //
                            //    break;
                            //}
                            //else
                            //{
                            //    originalLines[ii - 1] = originalLines[ii - 1].TrimEnd(',');
                            //    ReplaceStringInFile(filesToRead[i], ii - 1, originalLines[ii - 1]);
                            //
                            //    break;
                            //}
                        }
                    }
                }

                deleteFile(fileToRead);
                deleteFile(localizationPath + @"backup.txt");

                //if (itemsCheckTranslate.Checked)
                //{
                //    deleteFile(localizationPath + @"itemInfo.txt");
                //}
            }

            if (gameName == "Temtem" || gameName == "A Space for the Unbound")
            {
                using (StreamReader reader = new StreamReader(localizationPath + @"I2Languages.csv", encoding: Encoding.UTF8))
                {
                    using (StreamWriter sw = new StreamWriter(localizationPath + @"I2Languages2.csv", false, encoding: Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string idLine = reader.ReadLine();
                            string[] idValues = idLine.Split(';');
                            if (gameName == "Temtem")
                                sw.WriteLine($"{idValues[0].Replace("\"", "")};{idValues[1].Replace("\"", "")};{idValues[2].Replace("\"", "")};{idValues[3].Replace("\"", "")};{idValues[4].Replace("\"", "")};");
                            else
                                sw.WriteLine($"{idValues[0].Replace("\"", "")};{idValues[1].Replace("\"", "")};{idValues[2].Replace("\"", "")};{idValues[3].Replace("\"", "")};");
                        }
                    }
                }

                deleteFile(localizationPath + @"I2Languages.csv");
                System.Threading.Thread.Sleep(500);
                File.Copy(localizationPath + @"I2Languages2.csv", localizationPath + @"I2Languages.csv");
                System.Threading.Thread.Sleep(500);
                deleteFile(localizationPath + @"I2Languages2.csv");
            }
            else if (gameName == "CoreKeeper")
            {
                File.WriteAllTextAsync(localizationPath + @"pack_CX.bat", Resources.pack_CX_bat);
                File.WriteAllBytes(localizationPath + @"unPacker_CSV.exe", Resources.unPacker_CSV_exe);
                System.Threading.Thread.Sleep(500);

                ReplaceStringInFile(localizationPath + @"pack_CX.bat", 1, "for %%a in (Localization.csv) do unPacker_CSV.exe -pc 3 \"%%a\"");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = localizationPath + @"pack_CX.bat",
                    WorkingDirectory = Path.GetDirectoryName(localizationPath + @"pack_CX.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
                System.Threading.Thread.Sleep(500);

                deleteFile(localizationPath + @"pack_CX.bat");
                deleteFile(localizationPath + @"unPacker_CSV.exe");
                deleteFile(fileToRead);

                string csvText = File.ReadAllText(localizationPath + @"Localization.csv");

                csvText = csvText.Replace(@"\r", "");
                csvText = csvText.Replace(" [new]", "");
                csvText = csvText.Replace(@"\n" + "\"", "\"");

                File.WriteAllText(localizationPath + @"Localization.csv", csvText, encoding: Encoding.UTF8);
            }

            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 1));

            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
            progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

            MessageBox.Show(
                         "Перевод готов к импорту",
                         "Подтверждение",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

            if (gameName != "CoreKeeper" && gameName != "nwr")
                Import();
            else
            {
                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                int progressPercent2 = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent2 + "%"));

                string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
                if (configLines[0].Contains("firstrun = 1"))
                {
                    ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 0, "firstrun = 0");
                }

                if (!boolDeleteTranslate)
                {
                    if (File.Exists(backupPath + gameName + "_translated_backup"))
                        deleteFile(backupPath + gameName + "_translated_backup");
                    System.Threading.Thread.Sleep(500);
                    if (gameName == "CoreKeeper")
                        File.Copy(localizationPath + @"Localization.csv", backupPath + gameName + "_translated_backup");
                    else if (gameName == "nwr")
                        File.Copy(path + @"\addon_russian.txt", backupPath + gameName + "_translated_backup");

                    MessageBox.Show(
                         "Игра русифицирована!",
                         "Русификация завершена",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                    updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                    deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));

                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Русификация " + gameName + " завершена.", Color.Black)));
                }
                else
                {
                    MessageBox.Show(
                             "Перевод выбранной игры полность удален.\nЕсли игра всё еще на русском - проверьте целостность игровых файлов через Steam.",
                             "Удаление перевода",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information,
                             MessageBoxDefaultButton.Button1,
                             MessageBoxOptions.DefaultDesktopOnly);

                    updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = false));

                    BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Удаление " + gameName + " завершено.", Color.Black)));
                    deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = false));
                }

                installEnd = true;

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum));
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

                //translatedEnd = false;
                updateOrReinstall = false;
                translateUpdate = false;
                translateReinstall = false;
                //reinstallWithoutBackup = false;
                installBreak = false;
                updateBreak = false;
                translatedFirstTime = false;
                boolDeleteTranslate = false;

                //recoverBD.BeginInvoke((MethodInvoker)(() => recoverBD.Enabled = true));
                btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
                UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
                choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
                txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true)); // почитить form1_Load и сохранение прогресса русификации пока должно быть только для темтем
                                                                                    //fontSetup.BeginInvoke((MethodInvoker)(() => fontSetup.Enabled = true));

                temtemCheckTranslate.BeginInvoke((MethodInvoker)(() => temtemCheckTranslate.Enabled = true));
                skillsNameCheckTranslate.BeginInvoke((MethodInvoker)(() => skillsNameCheckTranslate.Enabled = true));
                itemsCheckTranslate.BeginInvoke((MethodInvoker)(() => itemsCheckTranslate.Enabled = true));
                skillsDescriptionCheckTranslate.BeginInvoke((MethodInvoker)(() => skillsDescriptionCheckTranslate.Enabled = true));
                npcCheckTranslate.BeginInvoke((MethodInvoker)(() => npcCheckTranslate.Enabled = true));

                yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
                if (machineName == myMachineName)
                    deeplButton.BeginInvoke((MethodInvoker)(() => deeplButton.Enabled = true));
            }
        }

        async void ImportAsync()
        {
            await Task.Run(() => Import());
        }

        void Import()
        {
            if (!boolDeleteTranslate)
                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Завершение русификации...", Color.Black)));
            else
                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Завершение удаления...", Color.Black)));

            // DungeonCrawler доделать импорт

            if (gameName != "DungeonCrawler")
            {
                if (gameName == "Temtem")
                {
                    File.WriteAllTextAsync(localizationPath + @"import_uls.bat", Resources.import_uls_bat);
                    File.WriteAllTextAsync(dataPath + @"\Import_l2.bat", Resources.Import_l2_bat);
                    System.Threading.Thread.Sleep(500);

                    if (!File.Exists(localizationPath + @"Parser_ULS.exe"))
                        File.WriteAllBytes(localizationPath + @"Parser_ULS.exe", Resources.Parser_ULS_exe);

                    System.Threading.Thread.Sleep(500);

                    if (translateUpdate && machineName == myMachineName)
                        CreateBackupTest();

                    if (progressBar.Value == 0)
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 4));

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    if (File.Exists(localizationPath + @"backup.txt"))
                        deleteFile(localizationPath + @"backup.txt");

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = localizationPath + @"import_uls.bat",
                        WorkingDirectory = Path.GetDirectoryName(localizationPath + @"import_uls.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                    System.Threading.Thread.Sleep(500);

                    deleteFile(localizationPath + @"I2Languages.csv");
                    deleteFile(localizationPath + @"Parser_ULS.exe");
                    deleteFile(localizationPath + @"import_uls.bat");
                    if (File.Exists(localizationPath + @"export_uls.bat"))
                        deleteFile(localizationPath + @"export_uls.bat");

                    if (!File.Exists(dataPath + @"\UPacker.exe"))
                        File.WriteAllBytes(dataPath + @"\UPacker.exe", Resources.UPacker_exe);

                    System.Threading.Thread.Sleep(1000);

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = dataPath + @"\Import_l2.bat",
                        WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Import_l2.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                    System.Threading.Thread.Sleep(500);

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    deleteFile(dataPath + @"\Import_l2.bat");
                    deleteFile(localizationPath + @"I2Languages.LanguageSourceAsset");
                    if (File.Exists(dataPath + @"\UPacker.exe"))
                        deleteFile(dataPath + @"\UPacker.exe");
                    if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                        deleteFile(dataPath + @"\UPacker_Advanced.exe");
                    delFolder(localizationPath);
                    if (gameName == "Temtem")
                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp");
                    else if (gameName == "A Space for the Unbound")
                        delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono\Assembly-CSharp-firstpass");
                    delFolder(dataPath + @"\Unity_Assets_Files\resources\Mono");
                    delFolder(dataPath + @"\Unity_Assets_Files\resources");
                    delFolder(dataPath + @"\Unity_Assets_Files");
                    System.Threading.Thread.Sleep(500);
                    string backupLocalizationPath = "";
                    if (gameName == "Temtem")
                        backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp";
                    else if (gameName == "A Space for the Unbound")
                        backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono\Assembly-CSharp-firstpass";

                    string backupTranslatedLocalizationPath = "";
                    if (gameName == "Temtem")
                        backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono\Assembly-CSharp";
                    else if (gameName == "A Space for the Unbound")
                        backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono\Assembly-CSharp-firstpassp";

                    if (Directory.Exists(backupLocalizationPath))
                    {
                        if (File.Exists(backupLocalizationPath + @"\I2.Loc\I2Languages.csv"))
                            deleteFile(backupLocalizationPath + @"\I2.Loc\I2Languages.csv");
                        if (File.Exists(backupLocalizationPath + @"\I2.Loc\I2Languages.LanguageSourceAsset"))
                            deleteFile(backupLocalizationPath + @"\I2.Loc\I2Languages.LanguageSourceAsset");
                        delFolder(backupLocalizationPath + @"\I2.Loc");
                        delFolder(backupLocalizationPath);
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup\Mono");
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_backup");
                        delFolder(backupPath + @"\Unity_Assets_Files");
                    }

                    if (Directory.Exists(backupTranslatedLocalizationPath))
                    {
                        if (File.Exists(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv"))
                            deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.csv");
                        if (File.Exists(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.LanguageSourceAsset"))
                            deleteFile(backupTranslatedLocalizationPath + @"\I2.Loc\I2Languages.LanguageSourceAsset");
                        delFolder(backupTranslatedLocalizationPath + @"\I2.Loc");
                        delFolder(backupTranslatedLocalizationPath);
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup\Mono");
                        delFolder(backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup");
                        delFolder(backupPath + @"\Unity_Assets_Files");
                    }
                }
                else if (gameName == "Escape Simulator" || gameName == "The Mortuary Assistant")
                {
                    if (progressBar.Value == 0)
                        progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 1));

                    if (translateUpdate && machineName == myMachineName)
                        CreateBackupTest();

                    File.WriteAllText(dataPath + @"\Import_txt.bat", Resources.Import_txt_bat);
                    if (!File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                        File.WriteAllBytes(dataPath + @"\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                    System.Threading.Thread.Sleep(500);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = dataPath + @"\Import_txt.bat",
                        WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Import_txt.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                    System.Threading.Thread.Sleep(500);


                    if (File.Exists(dataPath + @"\Export_txt.bat"))
                        deleteFile(dataPath + @"\Export_txt.bat");
                    if (File.Exists(dataPath + @"\Import_txt.bat"))
                        deleteFile(dataPath + @"\Import_txt.bat");

                    if (File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                        deleteFile(dataPath + @"\UPacker_Advanced.exe");
                    delFolder(localizationPath);
                    delFolder(dataPath + @"\Unity_Assets_Files");

                    if (gameName != "The Mortuary Assistant")
                        deleteFile(localizationPath + @"English.txt");
                    else
                    {
                        for (int i = 0; i < filesToRead.Length; i++)
                        {
                            deleteFile(filesToRead[i]);
                        }
                    }

                    string backupLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_backup";
                    string backupTranslatedLocalizationPath = backupPath + @"Unity_Assets_Files\" + gameName + @"_translated_backup";

                    if (Directory.Exists(backupLocalizationPath))
                    {
                        if (File.Exists(backupLocalizationPath + @"\English.txt"))
                            deleteFile(backupLocalizationPath + @"\English.txt");
                        else
                        {
                            string[] backupFilesToRead = Directory.GetFiles(backupLocalizationPath);
                            for (int i = 0; i < backupFilesToRead.Length; i++)
                            {
                                deleteFile(backupFilesToRead[i]);
                            }
                        }

                        delFolder(backupLocalizationPath);
                        delFolder(backupPath + @"\Unity_Assets_Files");
                    }

                    if (Directory.Exists(backupTranslatedLocalizationPath))
                    {
                        if (File.Exists(backupTranslatedLocalizationPath + @"\English.txt"))
                            deleteFile(backupTranslatedLocalizationPath + @"\English.txt");
                        else
                        {
                            string[] backupTranslatedFilesToRead = Directory.GetFiles(backupTranslatedLocalizationPath);
                            for (int i = 0; i < backupTranslatedFilesToRead.Length; i++)
                            {
                                deleteFile(backupTranslatedFilesToRead[i]);
                            }
                        }

                        delFolder(backupTranslatedLocalizationPath);
                        delFolder(backupPath + @"\Unity_Assets_Files");
                    }

                    if (gameName == "Escape Simulator" && machineName == myMachineName)
                    {
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2", dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4", dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1", dataPath + @"\StreamingAssets\AssetBundles\Levels\space1.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2", dataPath + @"\StreamingAssets\AssetBundles\Levels\space2.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3", dataPath + @"\StreamingAssets\AssetBundles\Levels\space3.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4", dataPath + @"\StreamingAssets\AssetBundles\Levels\space4.assets");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5", dataPath + @"\StreamingAssets\AssetBundles\Levels\space5.assets");

                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5");
                        System.Threading.Thread.Sleep(500);

                        Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(@"C:\Users\GlebV\Desktop\ES_textures\StreamingAssets", dataPath + @"\StreamingAssets\AssetBundles\Levels");

                        if (!File.Exists(dataPath + @"\StreamingAssets\AssetBundles\Levels\Import_textures.bat"))
                            File.WriteAllTextAsync(dataPath + @"\StreamingAssets\AssetBundles\Levels\Import_textures.bat", Resources.Import_textures_bat);
                        if (!File.Exists(dataPath + @"\StreamingAssets\AssetBundles\Levels\UPacker_Advanced.exe"))
                            File.WriteAllBytesAsync(dataPath + @"\StreamingAssets\AssetBundles\Levels\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                        System.Threading.Thread.Sleep(500);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = dataPath + @"\StreamingAssets\AssetBundles\Levels\Import_textures.bat",
                            WorkingDirectory = Path.GetDirectoryName(dataPath + @"\StreamingAssets\AssetBundles\Levels\Import_textures.bat"),
                            CreateNoWindow = true
                        }).WaitForExit();

                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\Import_textures.bat");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\UPacker_Advanced.exe");

                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\space1");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\space2");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\space3");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\space4");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5.assets", dataPath + @"\StreamingAssets\AssetBundles\Levels\space5");

                        Directory.CreateDirectory(@"C:\Users\GlebV\Desktop\ES_textures\StreamingAssets\Unity_Assets_Files");
                        System.Threading.Thread.Sleep(500);

                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4.assets");
                        deleteFile(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5.assets");
                        System.Threading.Thread.Sleep(500);

                        Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(dataPath + @"\StreamingAssets\AssetBundles\Levels\Unity_Assets_Files", @"C:\Users\GlebV\Desktop\ES_textures\StreamingAssets\Unity_Assets_Files");

                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\adventure2");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\adventure4");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\corporation1");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\corporation2");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\corporation3");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\corporation4");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\corporation5");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\space1");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\space2");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\space3");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\space4");
                        deleteFile(@"C:\Users\GlebV\Desktop\es_tex\space5");
                        System.Threading.Thread.Sleep(500);

                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure2", @"C:\Users\GlebV\Desktop\es_tex\adventure2");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\adventure4", @"C:\Users\GlebV\Desktop\es_tex\adventure4");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation1", @"C:\Users\GlebV\Desktop\es_tex\corporation1");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation2", @"C:\Users\GlebV\Desktop\es_tex\corporation2");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation3", @"C:\Users\GlebV\Desktop\es_tex\corporation3");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation4", @"C:\Users\GlebV\Desktop\es_tex\corporation4");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\corporation5", @"C:\Users\GlebV\Desktop\es_tex\corporation5");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space1", @"C:\Users\GlebV\Desktop\es_tex\space1");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space2", @"C:\Users\GlebV\Desktop\es_tex\space2");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space3", @"C:\Users\GlebV\Desktop\es_tex\space3");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space4", @"C:\Users\GlebV\Desktop\es_tex\space4");
                        File.Copy(dataPath + @"\StreamingAssets\AssetBundles\Levels\space5", @"C:\Users\GlebV\Desktop\es_tex\space5");
                    }
                }

                if (gameName == "The Mortuary Assistant" && machineName == myMachineName)
                {
                    System.Threading.Thread.Sleep(1000);
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 3));

                    Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(@"C:\Users\GlebV\Desktop\MortuaryFonts", dataPath);

                    System.Threading.Thread.Sleep(1000);

                    if (!File.Exists(dataPath + @"\UPacker_Advanced.exe"))
                        File.WriteAllBytes(dataPath + @"\UPacker_Advanced.exe", Resources.UPacker_Advanced_exe);
                    File.WriteAllTextAsync(dataPath + @"\Import_ALL_MBnew.bat", Resources.Import_ALL_MBnew_bat);

                    System.Threading.Thread.Sleep(1000);

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = dataPath + @"\Import_ALL_MBnew.bat",
                        WorkingDirectory = Path.GetDirectoryName(dataPath + @"\Import_ALL_MBnew.bat"),
                        CreateNoWindow = true
                    }).WaitForExit();
                    System.Threading.Thread.Sleep(500);

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                    deleteFile(dataPath + @"\UPacker_Advanced.exe");
                    deleteFile(dataPath + @"\Import_ALL_MBnew.bat");

                    Directory.CreateDirectory(@"C:\Users\GlebV\Desktop\MortuaryFonts\Unity_Assets_Files");
                    System.Threading.Thread.Sleep(500);

                    Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(dataPath + @"\Unity_Assets_Files", @"C:\Users\GlebV\Desktop\MortuaryFonts\Unity_Assets_Files");

                    if (File.Exists(backupPath + gameName + "_0_translated_backup"))
                        deleteFile(backupPath + gameName + "_0_translated_backup");
                    System.Threading.Thread.Sleep(500);

                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                    progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                    lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));
                }

                if (File.Exists(backupPath + gameName + "_backup.assets"))
                {
                    File.Copy(backupPath + gameName + "_backup.assets", backupPath + gameName + "_backup");
                    deleteFile(backupPath + gameName + "_backup.assets");
                }

                if (File.Exists(backupPath + gameName + "_translated_backup.assets"))
                {
                    File.Copy(backupPath + gameName + "_translated_backup.assets", backupPath + gameName + "_translated_backup");
                    deleteFile(backupPath + gameName + "_translated_backup.assets");
                }

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                int progressPercent2 = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent2 + "%"));
            }
            else
            {
                File.WriteAllTextAsync(localizationPath + @"import_txt_into_locres.bat", Resources.import_txt_into_locres_bat);
                File.WriteAllTextAsync(dataPath + @"\import_u4pak.bat", Resources.import_u4pak_bat);
                File.WriteAllBytes(dataPath + @"\test.u4pak", Resources.test_u4pak);
                File.WriteAllBytes(dataPath + @"\u4pak.exe", Resources.u4pak_exe);

                if (!File.Exists(localizationPath + "UE4localizationsTool.exe"))
                    File.WriteAllBytes(localizationPath + @"UE4localizationsTool.exe", Resources.UE4localizationsTool_exe);
                System.Threading.Thread.Sleep(500);

                if (translateUpdate && machineName == myMachineName)
                    CreateBackupTest();

                if (progressBar.Value == 0)
                    progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + 4));

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                int progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                if (File.Exists(localizationPath + @"backup.txt"))
                    deleteFile(localizationPath + @"backup.txt");

                Process.Start(new ProcessStartInfo
                {
                    FileName = localizationPath + @"import_txt_into_locres.bat",
                    WorkingDirectory = Path.GetDirectoryName(localizationPath + @"import_txt_into_locres.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
                System.Threading.Thread.Sleep(500);

                progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
                progressPercent = (progressBar.Value + 1) * 100 / progressBar.Maximum;
                lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%"));

                deleteFile(localizationPath + @"import_txt_into_locres.bat");
                deleteFile(localizationPath + @"UE4localizationsTool.exe");
                deleteFile(localizationPath + @"Game.locres.txt");
                deleteFile(localizationPath + @"Game.locres");
                File.Copy(localizationPath + @"Game_NEW.locres", localizationPath + @"Game.locres");
                System.Threading.Thread.Sleep(500);
                deleteFile(localizationPath + @"Game_NEW.locres");
                if (File.Exists(dataPath + @"\pakchunk0-Windows_0_P.pak"))
                    deleteFile(dataPath + @"\pakchunk0-Windows_0_P.pak");
                System.Threading.Thread.Sleep(500);

                ReplaceStringInFile(dataPath + @"\test.u4pak", 4, @"""" + dataPath + @"\pakchunk0-Windows_0_P.pak""");
                System.Threading.Thread.Sleep(500);
                ReplaceStringInFile(dataPath + @"\test.u4pak", 6, @""":zlib,rename=/:" + dataPath + @"\pakchunk0-Windows_0_P""");
                System.Threading.Thread.Sleep(500);

                Process.Start(new ProcessStartInfo
                {
                    FileName = dataPath + @"\import_u4pak.bat",
                    WorkingDirectory = Path.GetDirectoryName(dataPath + @"\import_u4pak.bat"),
                    CreateNoWindow = true
                }).WaitForExit();
                System.Threading.Thread.Sleep(500);

                deleteFile(dataPath + @"\import_u4pak.bat");
                deleteFile(dataPath + @"\u4pak.exe");
                deleteFile(dataPath + @"\test.u4pak");
                delFolder(dataPath + @"\pakchunk0-Windows_0_P");
            }

            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
            if (configLines[0].Contains("firstrun = 1"))
            {
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 0, "firstrun = 0");
            }

            if (!boolDeleteTranslate)
            {
                if (File.Exists(backupPath + gameName + "_translated_backup"))
                    deleteFile(backupPath + gameName + "_translated_backup");
                System.Threading.Thread.Sleep(500);

                if (gameName != "DungeonCrawler")
                {
                    File.Copy(dataPath + @"\resources.assets", backupPath + gameName + "_translated_backup");
                }
                else
                {
                    if (!Directory.Exists(dataPath + @"\~mods"))
                        Directory.CreateDirectory(dataPath + @"\~mods");

                    File.Copy(dataPath + @"\pakchunk0-Windows_0_P.pak", backupPath + gameName + "_translated_backup");

                    File.Copy(dataPath + @"\pakchunk0-Windows_0_P.pak", dataPath + @"\~mods" + @"\pakchunk0-Windows_0_P.pak");
                    System.Threading.Thread.Sleep(500);
                    deleteFile(dataPath + @"\pakchunk0-Windows_0_P.pak");
                    System.Threading.Thread.Sleep(500);
                    File.Copy(backupPath + gameName + "_backup", dataPath + @"\pakchunk0-Windows_0_P.pak");
                }

                MessageBox.Show(
                     "Игра русифицирована!",
                     "Русификация завершена",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);

                updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = true));
                deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = true));

                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Русификация " + gameName + " завершена.", Color.Black)));
            }
            else
            {
                MessageBox.Show(
                         "Перевод выбранной игры полность удален.\nЕсли игра всё еще на русском - проверьте целостность игровых файлов через Steam.",
                         "Удаление перевода",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                updateReinstall.BeginInvoke((MethodInvoker)(() => updateReinstall.Enabled = false));

                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Удаление " + gameName + " завершено.", Color.Black)));
                deleteTranslate.BeginInvoke((MethodInvoker)(() => deleteTranslate.Enabled = false));
            }

            installEnd = true;

            progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value = progressBar.Maximum - 2));

            lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = "100%"));

            //translatedEnd = false;
            updateOrReinstall = false;
            translateUpdate = false;
            translateReinstall = false;
            //reinstallWithoutBackup = false;
            installBreak = false;
            updateBreak = false;
            translatedFirstTime = false;
            boolDeleteTranslate = false;

            //recoverBD.BeginInvoke((MethodInvoker)(() => recoverBD.Enabled = true));
            btnStart.BeginInvoke((MethodInvoker)(() => btnStart.Enabled = true));
            UpdateButton.BeginInvoke((MethodInvoker)(() => UpdateButton.Enabled = true));
            choosePathBtn.BeginInvoke((MethodInvoker)(() => choosePathBtn.Enabled = true));
            txtPath.BeginInvoke((MethodInvoker)(() => txtPath.Enabled = true)); // почитить form1_Load и сохранение прогресса русификации пока должно быть только для темтем
            //fontSetup.BeginInvoke((MethodInvoker)(() => fontSetup.Enabled = true));

            temtemCheckTranslate.BeginInvoke((MethodInvoker)(() => temtemCheckTranslate.Enabled = true));
            skillsNameCheckTranslate.BeginInvoke((MethodInvoker)(() => skillsNameCheckTranslate.Enabled = true));
            itemsCheckTranslate.BeginInvoke((MethodInvoker)(() => itemsCheckTranslate.Enabled = true));
            skillsDescriptionCheckTranslate.BeginInvoke((MethodInvoker)(() => skillsDescriptionCheckTranslate.Enabled = true));
            npcCheckTranslate.BeginInvoke((MethodInvoker)(() => npcCheckTranslate.Enabled = true));

            yandexButton.BeginInvoke((MethodInvoker)(() => yandexButton.Enabled = true));
            if (machineName == myMachineName)
                deeplButton.BeginInvoke((MethodInvoker)(() => deeplButton.Enabled = true));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (progressBar.Value != 0 && lblPercent.Text != "100%")
            {
                DialogResult result = MessageBox.Show(
                         "Вы точно хотите закрыть русификатор? Весь прогресс русификации сохранится.\nДля продолжения русификации с места сохранения нужно будет нажать \"Установить\", если Вы устанавливали перевод "
                         + @"или ""Обновить\Переустановить"", если Вы обновляли перевод уже русифицированной игры.",
                         "Подтверждение",
                         MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
                else if (result == DialogResult.OK)
                {
                    cmdKill("geckodriver.exe");

                    cmdKill("firefox.exe");

                    myMachineName = "GOODDARK-DESKTO";
                }
            }
            else if (progressBar.Value == 0 || lblPercent.Text == "100%")
            {
                cmdKill("geckodriver.exe");

                cmdKill("firefox.exe");

                myMachineName = "GOODDARK-DESKTO";
            }
        }

        void vkBtn_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://vk.com/utranslator",
                UseShellExecute = true
            });
        }

        void gamesListButton_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        void myVkLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://vk.com/gooddark",
                UseShellExecute = true
            });
        }

        void instructionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://vk.com/@utranslator-manual",
                UseShellExecute = true
            });

            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = "Инструкция.pdf",
            //    UseShellExecute = true
            //});
        }

        #region google api
        /*public async Task<string> TranslatedTextConstructor(string sourceText)
        {
            if (sourceText == "")
                return "";

            int beginningQuotations = 0;
            int endQuotations = 0;

            for (int i = 0; i < sourceText.Length; i++)
            {
                if (sourceText[i] == '"')
                {
                    beginningQuotations = beginningQuotations + 1;
                }
                else
                {
                    break;
                }
            }
            for (int i = sourceText.Length; i <= sourceText.Length; i--)
            {
                if (sourceText[i - 1] == '"')
                {
                    endQuotations = endQuotations + 1;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < beginningQuotations; i++)
            {
                sourceText = sourceText.TrimStart('"');
            }
            for (int i = 0; i < endQuotations; i++)
            {
                sourceText = sourceText.TrimEnd('"');
            }

            sourceText = sourceText.Replace("[P:", "[P:notranslate");

            sourceText = sourceText.Replace("[", "notranslatesl");
            sourceText = sourceText.Replace("]", "_donttranslatesl");
            sourceText = sourceText.Replace("\"", "<>");

            sourceText = sourceText.Replace("? ", "?");
            sourceText = sourceText.Replace(". ", ".");
            sourceText = sourceText.Replace(@"\n\n", @"\n\n ");

            string toLanguage = "ru";
            string fromLanguage = "en";

            string url = $"https://clients5.google.com/translate_a/single?client=dict-chrome-ex&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(sourceText)}";

            WebClient webClient = new WebClient
            {
                Encoding = Encoding.UTF8,
            };

            string result = await webClient.DownloadStringTaskAsync(url);

            result = result.Replace("[[[\"", "");
            result = result.Replace("\u005C\"", "\"");

            result = result.TrimStart('[');
            result = result.TrimStart('"');
            result = result.TrimEnd(']');
            result = result.TrimEnd('"');

            result = result.Replace("? ", "?");
            result = result.Replace(". ", ".");

            result = result.Replace(@"\u003c", "<");
            result = result.Replace(@"\u003e", ">");
            result = result.Replace(@"\u003d", "=");
            result = result.Replace(@"\u0026", "&");
            result = result.Replace(@"\\", @"\");

            string[] parse = Regex.Split(result, "\\[|\\]|[^а-яА-Я ],|\",\"|\"|\"");

            string ruText = "";
            string enText = "";
            foreach (var item in parse)
            {
                if (hasRuLetters(item))
                    ruText = ruText + item.Replace("nul", "");
                else
                {
                    enText = enText + item;
                }
            }

            sourceText = enText;
            result = ruText + enText;

            int startIndexTrash = result.LastIndexOf(sourceText);
            string translatedTextTrash = result.Substring(0, startIndexTrash);
            string trash = result.Substring(startIndexTrash, result.Length - translatedTextTrash.Length);
            result = result.Replace(trash, "");

            result = result.Replace("notranslatesl", "[");
            result = result.Replace("lateSl", "[");
            result = result.Replace("_donttranslatesl", "]");
            result = result.Replace("_donttranslateslates", "]");
            result = result.Replace("_donttranslates", "]");
            result = result.Replace("_donttranslateSl", "]");
            result = result.Replace("NotransLatesl", "[");
            result = result.Replace("NotranslateSl", "[");
            result = result.Replace("_DONTTRANSLATESL", "]");
            result = result.Replace("notranslate", "");
            result = result.Replace("nranslate", "");
            result = result.Replace("непереводимый", "[");
            result = result.Replace("<>", "\"");
            result = result.Replace(@"\ n", @"\n");
            result = result.Replace(@"\ N", @"\n");
            result = result.Replace("<color = # ", "<color=#");
            result = result.Replace("</ color>", "</color>");
            result = result.Replace("<size = ", "<size=");
            result = result.Replace("</ size>", "</size>");
            result = result.Replace("loc: ", "Loc:");
            result = result.Replace("«", "“");
            result = result.Replace("*QOW", "*QOWB");
            result = result.Replace("{1 ", "{1}");

            result = result.Replace("?", "? ");

            result = result.Replace(".", ". ");
            result = result.Replace(" .", ".");
            result = result.Replace(". . . ", "...");
            result = result.Replace(" ...", "... ");
            result = result.TrimEnd();

            for (int i = 0; i < beginningQuotations; i++)
            {
                result = result.Insert(i, "\"");
            }

            for (int i = 0; i < endQuotations; i++)
            {
                result += '"';
            }

            result = result.Replace("'", "''");
            result = result.Replace("&lt", "<");
            result = result.Replace("&gt", ">");

            return result;

            //SqlUpdateTranslatedText(originalText, command, result);
        }*/
        #endregion

        void Downloading_Files(string link, string filename)//Загрузка файлов из интернета
        {
            var webClient = new WebClient();
            //webClient.DownloadProgressChanged += WebClientDownloadProgressChanged;
            webClient.DownloadFile(new Uri(link), filename);
        }

        void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Maximum = progressBar.Maximum + e.));
            //
            ////progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += 1));
            ////int progressPercent = progressBar.Value + (int)e.BytesReceived / (int)e.TotalBytesToReceive * 100 / progressBar.Maximum;
            ////lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = progressPercent + "%%"));
            //
            //progressBar.BeginInvoke((MethodInvoker)(() => progressBar.Value += e.ProgressPercentage));
            //int progressPercent = progressBar.Value + e.ProgressPercentage * 100 / progressBar.Maximum;
            //lblPercent.BeginInvoke((MethodInvoker)(() => lblPercent.Text = e.ProgressPercentage + "%"));

            // доделать потом
        }

        void UpdateButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Поиск новых версий русификатора...", Color.Black)));
            isRun = false;

            temtemCheckTranslate.Enabled = false;
            skillsNameCheckTranslate.Enabled = false;
            itemsCheckTranslate.Enabled = false;
            skillsDescriptionCheckTranslate.Enabled = false;
            npcCheckTranslate.Enabled = false;

            deeplButton.Enabled = false;
            yandexButton.Enabled = false;

            choosePathBtn.Enabled = false;
            deleteTranslate.Enabled = false;
            btnStart.Enabled = false;
            donateButton.Enabled = false;
            UpdateButton.Enabled = false;
            updateReinstall.Enabled = false;

            if (File.Exists(Application.StartupPath + "updater.exe"))//Если остался старый патч, удаляем
                deleteFile(Application.StartupPath + "updater.exe");

            if (!Directory.Exists(Application.StartupPath + "tmp"))//Создаём временную директорию
                Directory.CreateDirectory(Application.StartupPath + "tmp");

            if (File.Exists(Application.StartupPath + "tmp\\info.txt"))//Удаляем старый файл с информацией о последнем патче
                deleteFile(Application.StartupPath + "tmp\\info.txt");

            System.Threading.Thread.Sleep(500);

            string[] configLines = File.ReadAllLines(Application.StartupPath + @"UTranslator.config");
            string cur_ver = configLines[1].Replace("version = ", "");

            Downloading_Files("https://drive.google.com/u/0/uc?id=133H0Fa8zwFqjVJViY20GeLpABFxpj-Dj&export=download&confirm=t&uuid=539fa2d8-5ac2-48dd-b8d3-164bff2905da&at=AB6BwCCFIP68el8g5-AV873P7cJy:1698171282437", "tmp\\info.txt");//Загружаем файл с информацией о версии приложения
            //Downloading_Files("https://cloclo51.cloud.mail.ru/weblink/view/PDGf/x9oxppGd5", "tmp\\info.txt");//Загружаем файл с информацией о версии приложения

            System.Threading.Thread.Sleep(5000);

            string[] tmpLines = File.ReadAllLines(Application.StartupPath + "tmp\\info.txt");//Считываем первую строку, в ней указана версия
            string version_new = tmpLines[0].TrimEnd();

            if (version_new != cur_ver)//обновление программы
            {
                DialogResult result = MessageBox.Show(
                         "Обнаружена новая версия русификатора - " + version_new + "\nРусификатор обновится автоматически. Во время обновления он перезапустится несколько раз.",
                         "Новая версия",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);

                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver = null;
                }

                System.Threading.Thread.Sleep(2500);
                cmdKill("geckodriver.exe");
                cmdKill("firefox.exe");

                deleteFile(Application.StartupPath + "tmp\\info.txt");
                System.Threading.Thread.Sleep(500);
                Directory.Delete(Application.StartupPath + "tmp");

                if (Process.GetProcessesByName("UTranslator_x86").Length > 0)
                {
                    //Downloading_Files("https://cloclo52.cloud.mail.ru/weblink/view/7dNb/2yJGNCRZ6", Application.StartupPath + "updater.exe");

                    Downloading_Files("https://drive.google.com/u/0/uc?id=1x1mxAII4_mH4L8ZRv7OBtayQwuj9Vxc8&export=download&confirm=t&uuid=539fa2d8-5ac2-48dd-b8d3-164bff2905da&at=AB6BwCCFIP68el8g5-AV873P7cJy:1698171282437", Application.StartupPath + "updater.exe");
                }
                else
                {
                    //Downloading_Files("https://cloclo53.cloud.mail.ru/weblink/view/joxB/Tu94spC9S", Application.StartupPath + "updater.exe");

                    Downloading_Files("https://drive.google.com/u/0/uc?id=1zr--hFtZkgIGY7rBBLJEwjAw-jiUrb-q&export=download&confirm=t&uuid=539fa2d8-5ac2-48dd-b8d3-164bff2905da&at=AB6BwCA_uJnCoVPTe6tFz23w6Llp:1698171222554", Application.StartupPath + "updater.exe");
                }

                System.Threading.Thread.Sleep(5000);

                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 1, "version = " + version_new);

                Process.Start(new ProcessStartInfo
                {
                    FileName = Application.StartupPath + "updater.exe",
                    WorkingDirectory = Path.GetDirectoryName(Application.StartupPath + "updater.exe"),
                    Verb = "runas"
                });

                Close();
            }
            else
            {
                deleteFile(Application.StartupPath + "tmp\\info.txt");
                Directory.Delete(Application.StartupPath + "tmp");

                temtemCheckTranslate.Enabled = true;
                skillsNameCheckTranslate.Enabled = true;
                itemsCheckTranslate.Enabled = true;
                skillsDescriptionCheckTranslate.Enabled = true;
                npcCheckTranslate.Enabled = true;

                deeplButton.Enabled = false;
                yandexButton.Enabled = true;

                choosePathBtn.Enabled = true;
                deleteTranslate.Enabled = true;
                btnStart.Enabled = true;
                donateButton.Enabled = true;
                UpdateButton.Enabled = true;
                updateReinstall.Enabled = true;

                BeginInvoke((MethodInvoker)(() => AppendLine(txtStatus, "Установлена последняя версия.", Color.Black)));
                isRun = false;

                //MessageBox.Show(
                //         "Новая версия не обнаружена.",
                //         "Новая версия",
                //         MessageBoxButtons.OK,
                //         MessageBoxIcon.Information,
                //         MessageBoxDefaultButton.Button1,
                //         MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void skillsNameCheckTranslate_CheckedChanged(object sender, EventArgs e)
        {
            if (skillsNameCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 2, "skill = 1");
            else if (!skillsNameCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 2, "skill = 0");
        }

        private void skillsDescriptionCheckTranslate_CheckedChanged(object sender, EventArgs e)
        {
            if (skillsDescriptionCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 13, "skill_description = 1");
            else if (!skillsDescriptionCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 13, "skill_description = 0");
        }

        private void itemsNameCheckTranslate_CheckedChanged(object sender, EventArgs e)
        {
            if (itemsCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 3, "item = 1");
            else if (!itemsCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 3, "item = 0");
        }

        private void npcCheckTranslate_CheckedChanged(object sender, EventArgs e)
        {
            if (npcCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 4, "npc = 1");
            else if (!npcCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 4, "npc = 0");
        }

        private void temtemCheckTranslate_CheckedChanged(object sender, EventArgs e)
        {
            if (temtemCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 14, "temtem = 1");
            else if (!temtemCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 14, "temtem = 0");
        }

        private void deeplButton_CheckedChanged(object sender, EventArgs e)
        {
            if (deeplButton.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 15, "deepl = 1");
            else if (!deeplButton.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 15, "deepl = 0");
        }

        private void googleButton_CheckedChanged(object sender, EventArgs e)
        {
            if (googleButton.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 5, "google = 1");
            else if (!googleButton.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 5, "google = 0");
        }

        private void yandexButton_CheckedChanged(object sender, EventArgs e)
        {
            if (yandexButton.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 16, "yandex = 1");
            else if (!yandexButton.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 16, "yandex = 0");
        }

        private void donateButton_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(donateButton, "Поддержать разработчика");
        }

        private void donateButton_Click(object sender, EventArgs e)
        {
            DonateForm donateForm = new DonateForm();
            donateForm.Show();
        }

        private void vkBtn_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(vkBtn, "Группа ВК");
        }

        private void UpdateButton_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(UpdateButton, "Проверить наличие обновлений русификатора");
        }

        private void btnStart_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(btnStart, "Начать или продолжить русификацию игры");
        }

        private void updateReinstall_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(updateReinstall, "Обновить или переустановить перевод игры");
        }

        private void deleteTranslate_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(updateReinstall, "Удалить перевод выбранной игры");
        }

        private void choosePathBtn_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(choosePathBtn, "Укажите путь к папке с игрой");
        }

        private void temtemCheckTranslate_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(temtemCheckTranslate, "Стандартные названия Темов лучше не переводить, чтобы было ориентироваться в них");
        }

        private void myVkLink_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(myVkLink, "Я в ВК");
        }

        private void gamesListButton_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(gamesListButton, "Игры, которые можно русифицировать с помощью русификатора");
        }

        private void machineNameText_TextChanged(object sender, EventArgs e)
        {
            myMachineName = machineNameText.Text;
        }

        private void border_Click(object sender, EventArgs e)
        {

        }

        private void itemsDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (itemsCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 21, "item_desc = 1");
            else if (!itemsCheckTranslate.Checked && isRun)
                ReplaceStringInFile(Application.StartupPath + @"UTranslator.config", 21, "item_desc = 0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Process process = Process.GetProcessesByName("notepad")[0];
            //IntPtr processHandle = OpenProcess(0x1F0FFF, false, process.Id);
            //process.Ids

            //byte[] buffer = new byte[24];
            //byte[] buffer = Encoding.UTF8.GetBytes("memory access test");
            //string text = "memory access test";
            //int bytesRead = 0;
            //
            //File.WriteAllText(Application.StartupPath + "test.txt", text);
            //System.Threading.Thread.Sleep(500);
            //FileInfo fileSize = new FileInfo(Application.StartupPath + "test.txt");
            //
            //write_memory(31624, 0x18EB05727C1, text, buffer.Length);

            //string testPath = @"H:\Dark and Darker\DungeonCrawler\Content\Paks\pakchunk0-Windows_0_P\DungeonCrawler\Content\Localization\Game\en";
            //
            //string[] text = File.ReadAllLines(testPath + @"\Game.locres.txt");
            //
            //for (int i = 0; i < text.Length; i++)
            //{
            //    string[] split = text[i].Split("=");
            //
            //    if (split[0] == split[1])
            //    {
            //        text[i] = text[i].Replace(split[0] + "=" + split[1], split[0]);
            //    }
            //}
            //
            //deleteFile(testPath + @"\Game.locres.txt");
            //System.Threading.Thread.Sleep(2500);
            //File.WriteAllLines(testPath + @"\Game.locres.txt", text);

            //string testPath_translated = @"H:\Visual Projects\UTranslator\bin\Debug\net6.0-windows10.0.17763.0\backup\Unity_Assets_Files\Temtem_translated_backup\Mono\Assembly-CSharp\I2.Loc";
            //string testPath_orig = @"H:\Visual Projects\UTranslator\bin\Debug\net6.0-windows10.0.17763.0\backup\Unity_Assets_Files\Temtem_backup\Mono\Assembly-CSharp\I2.Loc";
            //string newFile = @"H:\Visual Projects\UTranslator\bin\Debug\net6.0-windows10.0.17763.0\backup\Unity_Assets_Files\test.csv";
            //
            //File.Create(newFile);
            //
            //string translatedText = File.ReadAllText(testPath_translated + @"\I2Languages.csv");
            //string[] translatedText2 = File.ReadAllLines(testPath_translated + @"\I2Languages.csv");
            //string[] origText = File.ReadAllLines(testPath_orig + @"\I2Languages.csv");
            //
            //for (int i = 0; i < origText.Length; i++)
            //{
            //    using (StreamWriter sw = new StreamWriter(newFile, true, encoding: Encoding.UTF8))
            //    {
            //        sw.WriteLine(" ");
            //    }
            //}
            //
            //System.Threading.Thread.Sleep(500);
            //
            //for (int i = 0; i < origText.Length; i++)
            //{
            //    string[] splitOrig = origText[i].Split(';');
            //    
            //    if (!translatedText.Contains(splitOrig[0]))
            //    {
            //        ReplaceStringInFile(newFile, i, origText[i]);
            //    }
            //}

            //deleteFile(testPath + @"\backup.txt");
            //System.Threading.Thread.Sleep(2500);
            //File.WriteAllLines(testPath + @"\backup.txt", text);

            MessageBox.Show(
                         "1",
                         "Новая 1",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information,
                         MessageBoxDefaultButton.Button1,
                         MessageBoxOptions.DefaultDesktopOnly);
        }

        private void TranslateEditorBtn_Click(object sender, EventArgs e)
        {
            //this.Hide();
            EditorForm editorForm = new EditorForm();
            editorForm.Show();
        }
    }
}