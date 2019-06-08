using System;
using System.Collections.Generic;
using System.IO;
using IX.Composer.Architecture;
using Newtonsoft.Json;

namespace Ignite.Core.Components.Configuration.Providers
{
    public class JsonConfiguration : IConfiguration
    {
        private const string DEFAULTS_PATH = "defaults";
        private const string CONFIG_PATH = "config";

        private delegate void JsonConfigurationChanged(object type);
        private event JsonConfigurationChanged OnChanged;

        private Dictionary<object, object> m_ObjectsForSave = new Dictionary<object, object>();
        private Hardware m_CurrentHardware { get; set; }

        public JsonConfiguration()
        {
            m_CurrentHardware = ApplicationEnv.Instance.CurrentHardware;
        }

        public void Initialize() { }

        public void Add<T>(T data, T @default)
        {
            if(!m_ObjectsForSave.ContainsKey(data))
            {
                m_ObjectsForSave.Add(data, @default);
            }
        }

        public void Build()
        {
            MakeDirectories();

            foreach(var element in m_ObjectsForSave)
            {
                Write(element.Key, false);
                Write(element.Value, true);
            }

            //Clear for dont marking old sttings files
            m_ObjectsForSave.Clear();
        }

        public void Append<T>(T data, bool @default)
        {
            Write(data, @default);

            OnChanged?.Invoke(data);
        }

        public T Read<T>(bool @default)
        {
            MakeDirectories();

            string strObj = "";

            if(!@default)
            {
                if(File.Exists(CONFIG_PATH + "\\" + $"{typeof(T).Name}.json"))
                {
                    strObj = File.ReadAllText(CONFIG_PATH + "\\" + $"{typeof(T).Name}.json");
                }
                else
                {
                    return (T)(object)null;
                }
            }
            else
            {
                if (File.Exists(CONFIG_PATH + "\\" + $"{typeof(T).Name}.json"))
                {
                    strObj = File.ReadAllText(CONFIG_PATH + "\\" + DEFAULTS_PATH + "\\" + $"{typeof(T).Name}.json");
                }
                else
                {
                    return (T)(object)null;
                }
            }

            try
            {
                if (strObj != "")
                {
                    return JsonConvert.DeserializeObject<T>(strObj);
                }
                else
                {
                    return (T)(object)null;
                }
            }
            catch(Exception ex)
            {
                ex.ToLog(LogLevel.Error);

                return (T)(object)null;
            }
        }

        private void Write<T>(T data, bool isDef)
        {
            MakeDirectories();

            string path = "";

            if(isDef)
            {
                path = Path.Combine(CONFIG_PATH, DEFAULTS_PATH, $"{data.GetType().Name}.default.json");
            }
            else
            {
                path = Path.Combine(CONFIG_PATH, $"{data.GetType().Name}.json");
            }


            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }

        private void MakeDirectories()
        {
            if (!Directory.Exists(CONFIG_PATH))
            {
                Directory.CreateDirectory(CONFIG_PATH);
            }

            if (!Directory.Exists(CONFIG_PATH + "\\" + DEFAULTS_PATH))
            {
                Directory.CreateDirectory(CONFIG_PATH + "\\" + DEFAULTS_PATH);
            }
        }

        public static JsonConfiguration Prototype() => new JsonConfiguration();

        public void MakeDefault<T>()
        {
            string path = Path.Combine(CONFIG_PATH, DEFAULTS_PATH, $"{typeof(T).Name}.default.json");
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);

                T deserialized = JsonConvert.DeserializeObject<T>(data);

                Write(deserialized, false);
            }
        }
    }
}
