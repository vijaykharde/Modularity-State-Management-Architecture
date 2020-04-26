using System;

namespace CsTech.Modularity.Constants
{
    public static class ExceptionMessages
    {
        public const string ConfigurationStoreCannotBeNull = "The ConfigurationStore cannot contain a null value.";
        public const string CyclicDependencyFound = "At least one cyclic dependency has been found in the module catalog.Cycles in the module dependencies must be avoided.";
        public const string DependencyForUnknownModule = "Cannot add dependency for unknown module {0}";
        public const string DependencyOnMissingModule = "A module declared a dependency on another module which is not declared to be loaded.Missing module(s): {0}";
        public const string DirectoryNotFound = "Directory {0} was not found.";
        public const string DuplicatedModule = "A duplicated module with name {0} has been found by the loader.";
        public const string FailedToGetType = "Unable to retrieve the module type {0} from the loaded assemblies.You may need to specify a more fully-qualified type name.";
        public const string FailedToLoadModule = @"An exception occurred while initializing module '{0}'. 
- The exception message was: {2}
- The Assembly that the module was trying to be loaded from was:{1}
Check the InnerException property of the exception for more information.If the exception occurred while creating an object in a DI container, you can exception.GetRootException() to help locate the root cause of the problem.";
        public const string FailedToLoadModuleNoAssemblyInfo = @"An exception occurred while initializing module '{0}'. 
    - The exception message was: {1}
Check the InnerException property of the exception for more information.If the exception occurred
    while creating an object in a DI container, you can exception.GetRootException() to help locate the
    root cause of the problem.";
        public const string FailedToRetrieveModule = @"Failed to load type for module {0}. 
If this error occurred when using MEF in a Silverlight application, please ensure that the CopyLocal property of the reference to the MefExtensions assembly is set to true in the main application/shell and false in all other assemblies.
Error was: {1}.";
        public const string InvalidArgumentAssemblyUri = "The argument must be a valid absolute Uri to an assembly file.";
        public const string ModuleDependenciesNotMetInGroup = "Module {0} depends on other modules that don't belong to the same group.";
        public const string ModuleNotFound = "Module {0} was not found in the catalog.";
        public const string ModulePathCannotBeNullOrEmpty = "The ModulePath cannot contain a null value or be empty.";
        public const string NoRetrieverCanRetrieveModule = "There is currently no moduleTypeLoader in the ModuleManager that can retrieve the specified module.";
        public const string NullModuleCatalogException = "The IModuleCatalog is required and cannot be null in order to initialize the modules.";
        public const string NullUnityContainerException = "The IUnityContainer is required and cannot be null.";
        public const string StartupModuleDependsOnAnOnDemandModule = "Module {0} is marked for automatic initialization when the application starts, but it depends on modules that are marked as OnDemand initialization.To fix this error, mark the dependency modules for InitializationMode= WhenAvailable, or remove this validation by extending the ModuleCatalog class.";
        public const string StringCannotBeNullOrEmpty = "The provided String argument {0} must not be null or empty.";
        public const string ValueMustBeOfTypeModuleInfo = "The value must be of type ModuleInfo.";
    }
}
