﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Components.Auth.Types
{
    public class AuthResult
    {
        public AuthResultEnum Code    { get; set; }
        public string         Message { get; set; }
    }

    public enum AuthResultEnum : int
    {
        Invalid = 0,
        Ok = 1,
        Error = 2
    }
}
