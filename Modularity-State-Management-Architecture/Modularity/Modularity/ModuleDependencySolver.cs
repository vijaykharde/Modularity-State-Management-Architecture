using System;
using System.Collections.Generic;
using System.Globalization;
using CsTech.Modularity.Constants;
using CsTech.Modularity.Exceptions;

namespace CsTech.Modularity
{
    /// <summary>
    /// Used by <see cref="ModuleInitializer"/> to get the load sequence for the modules to load according to their dependencies.
    /// </summary>
    internal class ModuleDependencySolver
    {
        private readonly ListDictionary<string, string> _dependencyMatrix = new ListDictionary<string, string>();
        private readonly List<string> _knownModules = new List<string>();

        /// <summary>
        /// Adds a module to the solver.
        /// </summary>
        /// <param name="name">The name that uniquely identifies the module.</param>
        public void AddModule(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.StringCannotBeNullOrEmpty, "name"));
            }

            AddToDependencyMatrix(name);
            AddToKnownModules(name);
        }

        /// <summary>
        /// Adds a module dependency between the modules specified by dependingModule and dependentModule.
        /// </summary>
        /// <param name="dependingModule">The name of the module with the dependency.</param>
        /// <param name="dependentModule">The name of the module dependingModule
        /// depends on.</param>
        public void AddDependency(string dependingModule, string dependentModule)
        {
            if (string.IsNullOrEmpty(dependingModule))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.StringCannotBeNullOrEmpty, "dependingModule"));
            }

            if (string.IsNullOrEmpty(dependentModule))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.StringCannotBeNullOrEmpty, "dependentModule"));
            }

            if (!_knownModules.Contains(dependingModule))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.DependencyForUnknownModule, dependingModule));
            }

            AddToDependencyMatrix(dependentModule);
            _dependencyMatrix.Add(dependentModule, dependingModule);
        }

        private void AddToDependencyMatrix(string module)
        {
            if (!_dependencyMatrix.ContainsKey(module))
            {
                _dependencyMatrix.Add(module);
            }
        }

        private void AddToKnownModules(string module)
        {
            if (!_knownModules.Contains(module))
            {
                _knownModules.Add(module);
            }
        }

        /// <summary>
        /// Calculates an ordered vector according to the defined dependencies. Non-dependent modules appears at the beginning of the resulting array.
        /// </summary>
        /// <returns>The resulting ordered list of modules.</returns>
        /// <exception cref="CyclicDependencyFoundException">This exception is thrown when a cycle is found in the defined dependency graph.</exception>
        public string[] Solve()
        {
            var skip = new List<string>();
            while (skip.Count < _dependencyMatrix.Count)
            {
                var leaves = FindLeaves(skip);
                if (leaves.Count == 0 && skip.Count < _dependencyMatrix.Count)
                {
                    throw new CyclicDependencyFoundException(ExceptionMessages.CyclicDependencyFound);
                }
                skip.AddRange(leaves);
            }
            skip.Reverse();

            if (skip.Count > _knownModules.Count)
            {
                var moduleNames = FindMissingModules(skip);
                throw new ModularityException(moduleNames, string.Format(CultureInfo.CurrentCulture, ExceptionMessages.DependencyOnMissingModule, moduleNames));
            }

            return skip.ToArray();
        }

        private string FindMissingModules(List<string> skip)
        {
            var missingModules = "";

            foreach (var module in skip)
            {
                if (!_knownModules.Contains(module))
                {
                    missingModules += ", ";
                    missingModules += module;
                }
            }

            return missingModules.Substring(2);
        }

        /// <summary>
        /// Gets the number of modules added to the solver.
        /// </summary>
        /// <value>The number of modules.</value>
        public int ModuleCount => _dependencyMatrix.Count;

        private List<string> FindLeaves(List<string> skip)
        {
            var result = new List<string>();

            foreach (var precedent in _dependencyMatrix.Keys)
            {
                if (skip.Contains(precedent))
                {
                    continue;
                }

                var count = 0;
                foreach (var dependent in _dependencyMatrix[precedent])
                {
                    if (skip.Contains(dependent))
                    {
                        continue;
                    }
                    count++;
                }
                if (count == 0)
                {
                    result.Add(precedent);
                }
            }
            return result;
        }
    }
}