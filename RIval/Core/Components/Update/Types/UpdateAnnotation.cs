using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Update.Types
{
    public class UpdateAnnotation
    {
        public string Version { get; set; }
        public string Description { get; set; }

        public string Link { get; set; }

        public UpdateAnnotation() { }
    }
}
