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
                new Phrase("StatusText_FilesDamaged", "Files damaged:"),
                new Phrase("StatusText_FilesDamaged_One", "File damaged:"),
                new Phrase("StatusText_GameStarted", "Game already runned"),
                new Phrase("StatusText_CheckFileBuilds", "Check file build:"),
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
                new Phrase("api_auth_incorrect_password", "Incorrect password for account: '{0}'"),
                new Phrase("api_register_email_already_exists", "Current email: '{0}' already exists!"),

                //Auth --mesagebox phrases
                new Phrase("Auth_MessageBox_Title_Error", "Authorize Error"),

                //Auth --windows phrases
                new Phrase("Auth_LoginComponent_Title", "AUTHORIZE"),
                new Phrase("Auth_LoginComponent_RememberMeCheckBox", "Remember Me"),
                new Phrase("Auth_LoginComponent_LoginButton", "Login"),
                new Phrase("Auth_LoginComponent_RegisterButton", "Create new account"),
                new Phrase("Auth_LoginComponent_RecoveryPasswordLink", "Reset password"),
                new Phrase("Auth_PrivacyLink", "Privacy Policy"),
                new Phrase("Auth_RegisterComponent_EmailBoxHelpText", "Enter EMail"),
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
                new Phrase("StatusText_FilesDamaged", "Файлы повреждены:"),
                new Phrase("StatusText_FilesDamaged_One", "Файл поврежден:"),
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
                new Phrase("api_auth_incorrect_password", "Неверный пароль для аккаунта: '{0}'"),
                new Phrase("api_register_email_already_exists", "Указанный email: '{0}' уже существует!"),

                //Auth --mesagebox phrases
                new Phrase("Auth_MessageBox_Title_Error", "Ошибка Авторизации"),

                //Auth --windows phrases
                new Phrase("Auth_LoginComponent_Title", "АВТОРИЗАЦИЯ"),
                new Phrase("Auth_LoginComponent_RememberMeCheckBox", "Запомнить меня"),
                new Phrase("Auth_LoginComponent_LoginButton", "Войти"),
                new Phrase("Auth_LoginComponent_RegisterButton", "Создать учетную запись"),
                new Phrase("Auth_LoginComponent_RecoveryPasswordLink", "Восстановление доступа"),
                new Phrase("Auth_PrivacyLink", "Политика конфиденциальности"),
                new Phrase("Auth_RegisterComponent_EmailBoxHelpText", "Введите адрес эл. почты"),
                new Phrase("Auth_RegisterComponent_QuestionsAnswerHelpText", "Введи ответ"),
                new Phrase("Auth_RegisterComponent_CreateAccount", "Создать учетную запись"),
                new Phrase("Auth_RegisterComponent_AlreadyExistsAccountButton", "У меня уже есть аккаунт"),

                //Auth --register phrases
                new Phrase("Register_questions_1", "Марка вашей первой машины"),
                new Phrase("Register_questions_2", "Название улицы, где вы жили, когда оканчивали школу"),
                new Phrase("Register_questions_3", "Место, куда вы в первый раз летали самолетом"),
                new Phrase("Register_questions_4", "Первая компьютерная игра, которую вы успешно прошли"),
                new Phrase("Register_questions_5", "Кличка вашего второго домашнего животного"),
                new Phrase("Register_questions_6", "Название любимой спортивной команды или имя любимого игрока"),
            });

            lng.SetLanguage(Languages.Russian);

            return lng;
        }
    }
}
