using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Launcher.Data
{
    public static class Store
    {
        public static short[] InitializePattern { get; set; }
        public static short[] CertificateBundle { get; set; }
        public static short[] SignatureBundle   { get; set; }
        public static byte[]  CertificatePatch  { get; set; }
        public static byte[]  SignaturePatch    { get; set; }
        
        public static void Boot()
        {
            InitializePattern = Patterns.Retail.Windows.Init;
            CertificateBundle = Patterns.Retail.Windows.CertBundle;
            SignatureBundle   = Patterns.Retail.Windows.Signature;
            CertificatePatch  = Patches.Retail.Windows.CertBundle;
            SignaturePatch    = Patches.Retail.Windows.Signature;
        }
    }
}
