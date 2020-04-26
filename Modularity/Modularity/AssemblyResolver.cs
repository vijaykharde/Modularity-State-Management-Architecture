using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CsTech.Modularity.Constants;

namespace CsTech.Modularity
{
    /// <summary>
    /// Handles AppDomain's AssemblyResolve event to be able to load assemblies dynamically in 
    /// the LoadFrom context, but be able to reference the type from assemblies loaded in the Load context.
    /// </summary>
    internal class AssemblyResolver : IAssemblyResolver, IDisposable
    {
        private readonly List<AssemblyInfo> _registeredAssemblies = new List<AssemblyInfo>();
        private bool _handlesAssemblyResolve;

        /// <summary>
        /// Registers the specified assembly and resolves the types in it when the AppDomain requests for it.
        /// </summary>
        /// <param name="moduleInfo">The path to the assembly to load in the LoadFrom context.</param>
        /// <remarks>This method does not load the assembly immediately, but lazily until someone requests a <see cref="Type"/>
        /// declared in the assembly.</remarks>
        public void LoadAssembly(ModuleInfo moduleInfo)
        {
            if (!_handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                _handlesAssemblyResolve = true;
            }

            var assemblyUri = GetFileUri(moduleInfo.Ref);
            if (assemblyUri == null)
            {
                throw new ArgumentException(ExceptionMessages.InvalidArgumentAssemblyUri, nameof(moduleInfo));
            }
            if (!File.Exists(assemblyUri.LocalPath))
            {
                throw new FileNotFoundException();
            }

            var assemblyName = AssemblyName.GetAssemblyName(assemblyUri.LocalPath);
            var assemblyInfo = _registeredAssemblies.FirstOrDefault(a => assemblyName == a.AssemblyName);
            if (assemblyInfo != null)
            {
                return;
            }

            assemblyInfo = new AssemblyInfo() { AssemblyName = assemblyName, AssemblyUri = assemblyUri };
            _registeredAssemblies.Add(assemblyInfo);
        }

        private static Uri GetFileUri(string filePath)
        {
            Uri uri;
            if (string.IsNullOrEmpty(filePath) || !Uri.TryCreate(filePath, UriKind.Absolute, out uri) || !uri.IsFile)
            {
                return null;
            }
            return uri;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            var assemblyInfo = _registeredAssemblies.FirstOrDefault(a => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName));
            if (assemblyInfo != null)
            {
                return assemblyInfo.Assembly ?? (assemblyInfo.Assembly = Assembly.LoadFrom(assemblyInfo.AssemblyUri.LocalPath));
            }
            return null;
        }

        private class AssemblyInfo
        {
            public AssemblyName AssemblyName { get; set; }
            public Uri AssemblyUri { get; set; }
            public Assembly Assembly { get; set; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the associated <see cref="AssemblyResolver"/>.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, it is being called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
                _handlesAssemblyResolve = false;
            }
        }
    }
}
