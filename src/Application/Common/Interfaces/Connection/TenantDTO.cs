using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces.Connection;

public class TenantDTO
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}