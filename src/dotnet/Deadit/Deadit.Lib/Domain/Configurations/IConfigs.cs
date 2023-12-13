﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Domain.Configurations;

public interface IConfigs
{
    public bool IsProduction { get; }

    public string DbServer { get; }
    public string DbDataBase { get; }
    public string DbUser { get; }
    public string DbPassword { get; }
}
