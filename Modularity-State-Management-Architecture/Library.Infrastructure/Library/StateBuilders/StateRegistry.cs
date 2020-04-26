using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.StateBuilders
{
    public class StateRegistry : IStateRegistry
    {
        private readonly Dictionary<(Type, string), Type> _stateRegistry = new Dictionary<(Type, string), Type>();
        private readonly IStateRegistry _parent;

        public StateRegistry() : this(null)
        {
        }

        public StateRegistry(IStateRegistry parent)
        {
            _parent = parent;
        }

        public void RegisterStateType(Type workItemName, string stateName, Type stateType)
        {
            _stateRegistry[(workItemName, stateName)] = stateType;
        }

        public Type ResolveStateType(Type workItemName, string stateName)
        {
            if (_stateRegistry.TryGetValue((workItemName, stateName), out var stateType))
            {
                return stateType;
            }
            return _parent?.ResolveStateType(workItemName, stateName);
        }
    }
}