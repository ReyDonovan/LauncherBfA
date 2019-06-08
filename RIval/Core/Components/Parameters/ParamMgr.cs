using Ignite.Core.Components.Game;
using System.Collections.Generic;

namespace Ignite.Core.Components.Parameters
{
    public class ParamMgr : Singleton<ParamMgr>
    {
        private GameCfg GameSettings { get; set; }

        public ParamMgr()
        {
            BootSettings();
        }

        private void BootSettings()
        {
            GameSettings = CfgMgr.Instance.GetProvider().Read<GameCfg>(false);
            if (GameSettings == null)
            {
                GameSettings = CfgMgr.Instance.GetProvider().Read<GameCfg>(true);
                if (GameSettings == null)
                {
                    CfgMgr.Instance.GetProvider().Add(new GameCfg(), new GameCfg()).Build();

                    BootSettings();
                }
            }
        }

        public void Append(Dictionary<int, string> dict)
        {
            if(GameSettings != null)
            {
                foreach(var elem in dict)
                {
                    if(GameSettings.GetFolder(elem.Key) != elem.Value)
                    {
                        GameSettings.AddFolder(elem.Key, elem.Value);
                    }
                }

                CfgMgr.Instance.GetProvider().Write(GameSettings, false);
            }
        }

        public void ResetAllSettings()
        {
            CfgMgr.Instance.GetProvider().Restore<GameCfg>();
        }

        public string GetGameFolder(int serverId)
        {
            return GameSettings.GetFolder(serverId) ?? "NOT FOUND";
        }

        public void SetFolder(int serverId, string path)
        {
            GameSettings.AddFolder(serverId, path);
        }
    }
}
