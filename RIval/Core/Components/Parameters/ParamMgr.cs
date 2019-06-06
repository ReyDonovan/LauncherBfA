using Ignite.Core.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string GetGameFolder(int serverId)
        {
            return GameSettings.GetFolder(serverId) ?? "NOT FOUND";
        }
    }
}
