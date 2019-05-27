using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ignite.Core
{
    public interface IWindowDispatcher
    {
        /// <summary>
        /// Реалзиация состояния загрузки для текущего компонента
        /// <see cref="IWindowDispatcher"/>
        /// </summary>
        void Loading();

        /// <summary>
        /// Реалзиация состояния отключения от сервера для текущего компонента
        /// <see cref="IWindowDispatcher"/>
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Реализация состояния подключения к серверу для текущего компонента
        /// <see cref="IWindowDispatcher"/>
        /// </summary>
        void Connected();

        /// <summary>
        /// Реализация состояния отказа в доступе для текущего компонента
        /// <see cref="IWindowDispatcher"/>
        /// </summary>
        void Banned();

        /// <summary>
        /// Реализация состояния блокировки для текущего компонента
        /// <see cref="IWindowDispatcher"/>
        /// </summary>
        void Blocked();

        /// <summary>
        /// Реализация нормального состояния для текущего компонента
        /// <see cref="IWindowDispatcher"/>
        /// </summary>
        void Normal();

        void AppendLocale(LanguageMgr langMgr);

        /// <summary>
        /// Реализация интерпритации к объекту текущего компонента
        /// <see cref="IWindowDispatcher"/>
        /// </summary>
        /// <returns></returns>
        Window ToWindow();
    }
}
