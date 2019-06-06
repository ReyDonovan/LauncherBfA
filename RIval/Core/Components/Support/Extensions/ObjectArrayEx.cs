using Ignite.Core.Components.Message;
using System;

namespace Ignite.Core
{
    public static class ObjectArrayEx
    {
        public static T[] Reinterpret<T>(this object[] arr)
        {
            try
            {
                T[] data = new T[arr.Length];

                for (int i = 0; i < arr.Length; i++)
                {
                    data[i] = (T)arr[i];
                }

                return data;
            }
            catch(Exception ex)
            {
                MessageBoxMgr.Instance.ShowReportError(
                    "#06-41123",
                    "Error occured while casting objects. Please report this!",
                    Components.Api.ApiFacade.Instance.GetUri("api-errorreporter-cast"),
                    ex.ToString());

                return (T[])(object)null;
            }
        }
    }
}
