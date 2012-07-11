using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Helpers;

namespace Backend.Facade.Interfaces
{
    public interface IRouletteFacade
    {
        bool CreateStake(StakeDTO[] stakes);
    }
}
