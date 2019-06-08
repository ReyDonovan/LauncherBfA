using System.Collections.Generic;

namespace Ignite.Core.Components.Game
{
    public class GameCfg
    {
        public Dictionary<int, string> Folders { get; set; } = new Dictionary<int, string>();

        public GameCfg() { }

        public void AddFolder(int id, string path)
        {
            if (Folders == null) Folders = new Dictionary<int, string>();

            if (Folders.ContainsKey(id))
            {
                Folders.Remove(id);
            }

            Folders.Add(id, path);
        }

        public string GetFolder(int id)
        {
            if (Folders == null) Folders = new Dictionary<int, string>();

            if(Folders.ContainsKey(id))
            {
                return Folders[id];
            }

            return null;
        }
    }
}
