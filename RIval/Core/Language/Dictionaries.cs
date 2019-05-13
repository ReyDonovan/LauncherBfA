using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Language
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
                new Phrase("menu-products", "PRODUCTS"),
                new Phrase("menu-news", "NEWS"),
                new Phrase("menu-forum", "FORUM"),
                new Phrase("menu-vk-tooltip", "Redirect to official VK"),
                new Phrase("menu-discord-tooltip", "Redirect to official Discord"),
                new Phrase("general-close-tooltip", "Close this app"),
                new Phrase("general-minimise-tooltip", "Minimise this app"),
                new Phrase("menu-item-accmanage", "Manage this account"),
                new Phrase("menu-item-supportservice", "Support service"),
                new Phrase("menu-item-settings", "App settings"),
                new Phrase("menu-item-aboutupdate", "See about update"),
                new Phrase("menu-item-feedback", "Send the feedback"),
                new Phrase("menu-item-exit", "Exit from this account"),
                new Phrase("login-window-title", "AUHTORIZE"),
                new Phrase("login-label-login", "Login:"),
                new Phrase("login-label-password", "Password:"),
                new Phrase("login-link-reset", "Reset password"),
                new Phrase("login-link-register", "Create accout"),
                new Phrase("login-button-enter", "LOGIN"),
            });

            lng.SetLanguage(Languages.English);

            return lng;
        }
        public static LanguagePhrases CreateRussian()
        {
            var lng = new LanguagePhrases();
            lng.AddDictionary(new List<Phrase>()
            {
                new Phrase("menu-products", "ПРОДУКТЫ"),
                new Phrase("menu-news", "НОВОСТИ"),
                new Phrase("menu-forum", "ФОРУМ"),
                new Phrase("menu-vk-tooltip", "Перейти на официальную страницу в VK"),
                new Phrase("menu-discord-tooltip", "Перейти в официальную группу в Discord"),
                new Phrase("general-close-tooltip", "Выйти из приложения"),
                new Phrase("general-minimise-tooltip", "Свернуть окно"),
                new Phrase("menu-item-accmanage", "Управление учетной записью"),
                new Phrase("menu-item-supportservice", "Служба поддержки"),
                new Phrase("menu-item-settings", "Настройки приложения"),
                new Phrase("menu-item-aboutupdate", "Узнать о последнем обновлении"),
                new Phrase("menu-item-feedback", "Оставить отзыв"),
                new Phrase("menu-item-exit", "Выйти из учетной записи"),
                new Phrase("login-window-title", "АВТОРИЗАЦИЯ"),
                new Phrase("login-label-login", "Логин:"),
                new Phrase("login-label-password", "Пароль:"),
                new Phrase("login-link-reset", "Восстановить пароль"),
                new Phrase("login-link-register", "Создать аккаунт"),
                new Phrase("login-button-enter", "ВОЙТИ"),
            });

            lng.SetLanguage(Languages.Russian);

            return lng;
        }
    }
}
