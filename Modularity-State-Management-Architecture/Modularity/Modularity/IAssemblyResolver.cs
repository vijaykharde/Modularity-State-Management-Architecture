using System;

namespace CsTech.Modularity
{
    /// <summary>
    /// Interface for classes that are responsible for resolving and loading assembly files. 
    /// </summary>
    internal interface IAssemblyResolver
    {
        /// <summary>
        /// Load an assembly when it's required by the application. 
        /// </summary>
        /// <param name="moduleInfo"></param>
        void LoadAssembly(ModuleInfo moduleInfo);
    }
}
