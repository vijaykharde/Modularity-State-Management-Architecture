using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.StateBuilders
{
    public interface IStateRegistry
    {
        void RegisterStateType(Type workItemName, string stateName, Type stateType);
        Type ResolveStateType(Type workItemName, string stateName);
    }
}
