using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Language
{
    public class Dictionaries
    {
        public static LanguagePhrases CreateByLanguage(Languages lang)
        {
            if      (lang == Languages.English) return CreateEnglish();
            else if (lang == Languages.Russian) return CreateRussian();
            else return null;
        }
        public static LanguagePhrases CreateEnglish()
        {
            var lng = new LanguagePhrases();
            lng.AddDictionary(new List<Phrase>()
            {
                new Phrase("MagazineButtonHeader", "STORE"),
                new Phrase("ACPButtonHeader", "PRIVATE CABINET"),
                new Phrase("ForumButtonHeader", "FORUM"),
                new Phrase("ServersLabel", "SERVERS"),
                new Phrase("MiscLabel", "MISC"),
                new Phrase("LinksLabel", "LINKS"),
                new Phrase("SettingsButton", "SETTINGS"),
                new Phrase("Bugreport_Button", "BUGTRACKER"),
                new Phrase("StatusText_Ready", "Ready to play"),
                new Phrase("StatusText_Init", "Initializing ..."),
                new Phrase("DescText_Init", "Initializing file list"),
                new Phrase("StatusText_ChooseGame", "Need choose the game folder"),
                new Phrase("StatusText_CheckFiles", "Check the files:"),
                new Phrase("StatusText_Download", "Downloading:"),
                new Phrase("GameComponentTitle", "{0} actual news:"),
                new Phrase("PlayButton", "PLAY"),
                new Phrase("CheckButton", "CHECK"),
                new Phrase("ButtonChooseGame", "CHOOSE"),
                new Phrase("EmptyNews_Text", "PLEASE WAIT THE ACTUAL NEWS"),
                new Phrase("StatusText_UpdateError", "Updating error"),
                new Phrase("StatusText_FilesDamaged", "Files corrupted"),
                new Phrase("StatusText_FilesDamaged_One", "File corrupted"),
                new Phrase("StatusText_GameStarted", "Game already runned"),
                new Phrase("StatusText_CheckFileBuilds", "Check files build:"),
                new Phrase("Tb", "TB"),
                new Phrase("Gb", "GB"),
                new Phrase("Mb", "MB"),
                new Phrase("Kb", "KB"),
                new Phrase("Bytes", "Bytes"),
                new Phrase("Seconds_Short", "s"),
                new Phrase("Error_FreeSpaceUnavailable_Title", "Installation error"),
                new Phrase("LangButton", "LANGUAGE"),

                //Api phrases
                new Phrase("api_server_error", "Server not responding. Please try again later"),
                new Phrase("api_auth_password_too_short", "Password must be at least 6 characters long"),
                new Phrase("api_auth_incorrect_password", "Incorrect login or password. Please try again"),
                new Phrase("api_register_email_already_exists", "Email already exists!"),

                //Auth --mesagebox phrases
                new Phrase("Auth_MessageBox_Title_Error", "Authorize Error"),
                new Phrase("Auth_LoginComponent_EmailHelpText", "E-Mail: "),
                new Phrase("Auth_LoginComponent_PasswordHelpText", "Password: "),
                new Phrase("Auth_RegisterComponent_AccountNameHelpText", "Account name:"),
                new Phrase("Auth_RegisterComponent_PasswordNameHelpText", "Password:"),
                new Phrase("Auth_RegisterComponent_QuestionNameHelpText", "Choose question:"),
                new Phrase("Auth_RegisterComponent_AnswerNameHelpText", "Answer:"),
                new Phrase("Auth_RegisterComponent_RegisterWindowTitle", "REGISTRATION"),

                //Auth --windows phrases
                new Phrase("Auth_LoginComponent_Title", "AUTHORIZE"),
                new Phrase("Auth_LoginComponent_RememberMeCheckBox", "Remember Me"),
                new Phrase("Auth_LoginComponent_LoginButton", "Login"),
                new Phrase("Auth_LoginComponent_RegisterButton", "Create new account"),
                new Phrase("Auth_LoginComponent_RecoveryPasswordLink", "Reset password"),
                new Phrase("Auth_PrivacyLink", "Privacy Policy"),
                new Phrase("Auth_RegisterComponent_EmailBoxHelpText", "Enter EMail"),
                new Phrase("Auth_RegisterComponent_LoginBoxHelpText", "Enter username"),
                new Phrase("Auth_RegisterComponent_QuestionsAnswerHelpText", "Enter answer"),
                new Phrase("Auth_RegisterComponent_CreateAccount", "Create new account"),
                new Phrase("Auth_RegisterComponent_AlreadyExistsAccountButton", "I already have account"),

                //Auth --register phrases
                new Phrase("Register_questions_1", "Brand of your first car"),
                new Phrase("Register_questions_2", "The name of the street where you lived when you graduated from high school"),
                new Phrase("Register_questions_3", "The place where you first flew the plane"),
                new Phrase("Register_questions_4", "The first computer game that you have successfully passed"),
                new Phrase("Register_questions_5", "The name of your second pet"),
                new Phrase("Register_questions_6", "Name of your favorite sports team or name of your favorite player"),

                //MessageBox phrases
                new Phrase("MessageBox_Title_Error", "Critical error"),
                new Phrase("MessageBox_Title_Success", "Success"),
                new Phrase("MessageBox_Title_Warning", "Warning"),

                //MessageBox --buttons phrases
                new Phrase("MessageBox_OkButton", "Continue"),
                new Phrase("MessageBox_ReportButton", "Report"),

                //Settings Window --general
                new Phrase("SettingsWindow_General_Title", "App settings"),
                new Phrase("SettingsWindow_General_AccountSettingsTitle", "Account settings"),
                new Phrase("SettingsWindow_General_GameSettingsTabTitle", "Games settings"),
                new Phrase("SettingsWindow_General_ResetSettingsButton", "Reset settings"),
                new Phrase("SettingsWindow_General_CancelButton", "Cancel"),
                new Phrase("SettingsWindow_General_AppendSettingsButton", "Append"),

                //Settings Window --realm_settings
                new Phrase("RealmSettings_Title", "Game client settings"),
                new Phrase("AtalDazarPath_Title", "Path: "),
                new Phrase("ChangeAtaldazar_Button", "Change"),
                new Phrase("MotherlodePath_Title", "Path: "),
                new Phrase("ChangeMotherLode_Button", "Change"),

                //Settings Window --reset alert
                new Phrase("SettingsWindow_General_ResetMBHead", "Reset all settings"),
                new Phrase("SettingsWindow_General_ResetMBDesc", "All application settings will be reset. To continue, right-click on the 'Reset' button"),
                new Phrase("SettingsWindow_General_ResetMBActionButton", "Reset"),
                new Phrase("SettingsWindow_General_ResetMBPrimaryButton", "Cancel"),
                new Phrase("SettingsWindow_General_ResetMBDescSuccess", "Settings successfully reset. The application will be reloaded"),

                //MainWindow new filemgr system, --mb-box after checking
                new Phrase("MainWindow_Downloading_MBStartButton", "Download"),
                new Phrase("MainWindow_Downloading_MBStartHeader", "Needed update"),
                new Phrase("MainWindow_Downloading_MBStartDesc", "The client files are corrupted. To continue, the application will download the necessary data to the client"),

                //MainWindow download stopped success
                new Phrase("MainWindow_DownloadStop_Success_Title", "Updating stopped"),
                new Phrase("MainWindow_DownloadStop_Success_Desc", "Game client has been succefully updated. For game press the 'Play' button"),
                new Phrase("MainWindow_DownloadStop_Error_Title", "Updating error"),
                new Phrase("MainWindow_DownloadStop_Error_Desc", "An error occurred during the upgrade. Try running the application as an Administrator and try again. If the problem persists, contact Administrator with this error."),
                new Phrase("MainWindow_EnoughSpace_Title", "#02-6632"),
                new Phrase("MainWindow_EnoughSpace_Desc", "There is not enough space on the selected drive. Requires ~50GB to install"),

                //Updater --window
                new Phrase("Updater_UpdateErrorMB_Title", "#01-378"),
                new Phrase("Updater_UpdateErrorMB_Desc", "An internal error occurred during the upgrade. Try again later, if the problem persists - contact your Administrator"),

                //Tooltips --phrases
                new Phrase("Tooltip_CloseApp", "Close the app"),
                new Phrase("Tooltip_Minimise", "Minimize window"),
                new Phrase("Tooltip_Settigs", "Settings"),
                new Phrase("Tooltip_Logout", "Logout"),
                new Phrase("Tooltip_NewsLinkHelp", "Right-click to view the full content"),

                //Launch --phrases
                new Phrase("StatusText_GamePrepare", "Restoring data"),
                new Phrase("StatusText_GameStarted_Error", "Initialization error"),

                //MainWindow --runas
                new Phrase("MainWindow_Init_RunAs_Header", "#01-0011"),
                new Phrase("MainWindow_Init_RunAs_Desc", "To run the application correctly, you must run it as an Administrator"),

                //New download system
                new Phrase("Download_Speed_Title", "Download speed: {0}/s"),
                new Phrase("Download_Speed_Downloaded", "Downloaded: {0}"),
            });

            lng.SetLanguage(Languages.English);

            return lng;
        }

        public static LanguagePhrases CreateRussian()
        {
            var lng = new LanguagePhrases();
            lng.AddDictionary(new List<Phrase>()
            {
                new Phrase("MagazineButtonHeader", "МАГАЗИН"),
                new Phrase("ACPButtonHeader", "ЛИЧНЫЙ КАБИНЕТ"),
                new Phrase("ForumButtonHeader", "ФОРУМ"),
                new Phrase("ServersLabel", "СЕРВЕРА"),
                new Phrase("MiscLabel", "ПРОЧЕЕ"),
                new Phrase("LinksLabel", "ССЫЛКИ"),
                new Phrase("SettingsButton", "НАСТРОЙКИ"),
                new Phrase("Bugreport_Button", "БАГТРЕКЕР"),
                new Phrase("StatusText_Ready", "Готов к запуску"),
                new Phrase("StatusText_Init", "Иницализация ..."),
                new Phrase("DescText_Init", "Получение списка файлов"),
                new Phrase("StatusText_ChooseGame", "Необходимо указать папку с игрой"),
                new Phrase("StatusText_CheckFiles", "Проверка файлов:"),
                new Phrase("StatusText_Download", "Загрузка:"),
                new Phrase("GameComponentTitle", "{0} свежие новости:"),
                new Phrase("PlayButton", "ИГРАТЬ"),
                new Phrase("CheckButton", "ПРОВЕРИТЬ"),
                new Phrase("ButtonChooseGame", "УКАЗАТЬ"),
                new Phrase("EmptyNews_Text", "ОЖИДАЙТЕ СВЕЖИХ НОВОСТЕЙ"),
                new Phrase("StatusText_UpdateError", "Ошибка обновления"),
                new Phrase("StatusText_FilesDamaged", "Файлы повреждены"),
                new Phrase("StatusText_FilesDamaged_One", "Файл поврежден"),
                new Phrase("StatusText_GameStarted", "Игра запущена"),
                new Phrase("StatusText_CheckFileBuilds", "Проверка целостности:"),
                new Phrase("Tb", "ТБ"),
                new Phrase("Gb", "ГБ"),
                new Phrase("Mb", "МБ"),
                new Phrase("Kb", "КБ"),
                new Phrase("Bytes", "Байт"),
                new Phrase("Seconds_Short", "с"),
                new Phrase("Error_FreeSpaceUnavailable_Title", "Ошибка инсталляции"),
                new Phrase("LangButton", "ЯЗЫК"),

                 //Api phrases
                new Phrase("api_server_error", "Сервер не отвечает. Повторите свой запрос позже."),
                new Phrase("api_auth_password_too_short", "Пароль должен состоять из не менее 6 символов."),
                new Phrase("api_auth_incorrect_password", "Неверный логин или пароль. Повторите попытку"),
                new Phrase("api_register_email_already_exists", "Указанный email уже существует!"),

                //Auth --mesagebox phrases
                new Phrase("Auth_MessageBox_Title_Error", "Ошибка Авторизации"),
                new Phrase("Auth_LoginComponent_EmailHelpText", "E-Mail: "),
                new Phrase("Auth_LoginComponent_PasswordHelpText", "Пароль: "),
                new Phrase("Auth_RegisterComponent_AccountNameHelpText", "Имя аккаунта:"),
                new Phrase("Auth_RegisterComponent_PasswordNameHelpText", "Пароль:"),
                new Phrase("Auth_RegisterComponent_QuestionNameHelpText", "Выберите вопрос:"),
                new Phrase("Auth_RegisterComponent_AnswerNameHelpText", "Ваш ответ:"),
                new Phrase("Auth_RegisterComponent_RegisterWindowTitle", "РЕГИСТРАЦИЯ"),

                //Auth --windows phrases
                new Phrase("Auth_LoginComponent_Title", "АВТОРИЗАЦИЯ"),
                new Phrase("Auth_LoginComponent_RememberMeCheckBox", "Запомнить меня"),
                new Phrase("Auth_LoginComponent_LoginButton", "Войти"),
                new Phrase("Auth_LoginComponent_RegisterButton", "Создать учетную запись"),
                new Phrase("Auth_LoginComponent_RecoveryPasswordLink", "Восстановление доступа"),
                new Phrase("Auth_PrivacyLink", "Политика конфиденциальности"),
                new Phrase("Auth_RegisterComponent_EmailBoxHelpText", "Введите адрес эл. почты"),
                new Phrase("Auth_RegisterComponent_LoginBoxHelpText", "Введите логин"),
                new Phrase("Auth_RegisterComponent_QuestionsAnswerHelpText", "Введите ответ"),
                new Phrase("Auth_RegisterComponent_CreateAccount", "Создать учетную запись"),
                new Phrase("Auth_RegisterComponent_AlreadyExistsAccountButton", "У меня уже есть аккаунт"),

                //Auth --register phrases
                new Phrase("Register_questions_1", "Марка вашей первой машины"),
                new Phrase("Register_questions_2", "Название улицы, где вы жили, когда оканчивали школу"),
                new Phrase("Register_questions_3", "Место, куда вы в первый раз летали самолетом"),
                new Phrase("Register_questions_4", "Первая компьютерная игра, которую вы успешно прошли"),
                new Phrase("Register_questions_5", "Кличка вашего второго домашнего животного"),
                new Phrase("Register_questions_6", "Название любимой спортивной команды или имя любимого игрока"),

                //MessageBox phrases
                new Phrase("MessageBox_Title_Error", "Критическая ошибка"),
                new Phrase("MessageBox_Title_Success", "Успешное действие"),
                new Phrase("MessageBox_Title_Warning", "Предупреждение"),

                //MessageBox --buttons phrases
                new Phrase("MessageBox_OkButton", "Продолжить"),
                new Phrase("MessageBox_ReportButton", "Сообщить"),

                //Settings Window --general
                new Phrase("SettingsWindow_General_Title", "Настройки приложения"),
                new Phrase("SettingsWindow_General_AccountSettingsTitle", "Учетные записи"),
                new Phrase("SettingsWindow_General_GameSettingsTabTitle", "Настройки игры"),
                new Phrase("SettingsWindow_General_ResetSettingsButton", "Сбросить все настройки"),
                new Phrase("SettingsWindow_General_CancelButton", "Отменить"),
                new Phrase("SettingsWindow_General_AppendSettingsButton", "Применить"),

                //Settings Window --realm_settings
                new Phrase("RealmSettings_Title", "Настройки игрового клиента"),
                new Phrase("AtalDazarPath_Title", "Путь: "),
                new Phrase("ChangeAtaldazar_Button", "Изменить"),
                new Phrase("MotherlodePath_Title", "Путь: "),
                new Phrase("ChangeMotherLode_Button", "Изменить"),

                //Settings Window --reset alert
                new Phrase("SettingsWindow_General_ResetMBHead", "Сброс всех настроек"),
                new Phrase("SettingsWindow_General_ResetMBDesc", "Все настройки приложения будут сброшены. Для продолжения нажмите правой кнопкой мыши на кнопку 'Сбросить'"),
                new Phrase("SettingsWindow_General_ResetMBActionButton", "Сбросить"),
                new Phrase("SettingsWindow_General_ResetMBPrimaryButton", "Отмена"),
                new Phrase("SettingsWindow_General_ResetMBDescSuccess", "Настройки успешно сброшены. Приложение будет перезагружено"),

                //MainWindow new filemgr system, --mb-box after checking
                new Phrase("MainWindow_Downloading_MBStartButton", "Загрузить"),
                new Phrase("MainWindow_Downloading_MBStartHeader", "Требуется обновление"),
                new Phrase("MainWindow_Downloading_MBStartDesc", "Файлы клиента повреждены. Для дальнейшего продолжения, приложение загрузить необходимые клиенту данные"),

                //MainWindow download stopped success/errors
                new Phrase("MainWindow_DownloadStop_Success_Title", "Обновление окончено"),
                new Phrase("MainWindow_DownloadStop_Success_Desc", "Игровой клиент обновлен успешно. Чтобы войти в игру нажмите кнопку 'Играть'"),
                new Phrase("MainWindow_DownloadStop_Error_Title", "Ошибка обновления"),
                new Phrase("MainWindow_DownloadStop_Error_Desc", "Во время обновления произошла ошибка. Попробуйте запустить приложение от имени Администратора и повторить попытку. Если проблема не ушла, то обратитесь с этой ошибкой к Администратору."),
                new Phrase("MainWindow_EnoughSpace_Title", "#02-6632"),
                new Phrase("MainWindow_EnoughSpace_Desc", "Не достаточно места на выбранном Вами диске. Для установки требуется ~50GB"),

                //Updater --window
                new Phrase("Updater_UpdateErrorMB_Title", "#01-378"),
                new Phrase("Updater_UpdateErrorMB_Desc", "Во время обновления произошла внутренняя ошибка. Попробуйте позже, если проблема не решилась - обратитесь к Администратору"),

                //Tooltips --phrases
                new Phrase("Tooltip_CloseApp", "Выйти из приложения"),
                new Phrase("Tooltip_Minimise", "Свернуть окно"),
                new Phrase("Tooltip_Settigs", "Параметры"),
                new Phrase("Tooltip_Logout", "Сменить пользователя"),
                new Phrase("Tooltip_NewsLinkHelp", "Для просмотра полного содержимого нажмите правой кнопкой мыши"),

                //Launch --phrases
                new Phrase("StatusText_GamePrepare", "Восстановление данных"),
                new Phrase("StatusText_GameStarted_Error", "Ошибка запуска"),

                //MainWindow --runas
                new Phrase("MainWindow_Init_RunAs_Header", "#01-0011"),
                new Phrase("MainWindow_Init_RunAs_Desc", "Для корректной работы приложения требуется запустить его от имени Администратора"),

                //New download system
                new Phrase("Download_Speed_Title", "Скорость: {0}/с"),
                new Phrase("Download_Speed_Downloaded", "Загружено: {0}"),
            });

            lng.SetLanguage(Languages.Russian);

            return lng;
        }
    }
}
