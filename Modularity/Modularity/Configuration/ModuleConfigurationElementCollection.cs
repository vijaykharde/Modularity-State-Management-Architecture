using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace CsTech.Modularity.Configuration
{
    /// <summary>
    /// A collection of <see cref="ModuleConfigurationElement"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class ModuleConfigurationElementCollection : NameTypeConfigurationElementCollection<ModuleConfigurationElement, ModuleConfigurationElement>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ModuleConfigurationElementCollection"/>.
        /// </summary>
        public ModuleConfigurationElementCollection()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ModuleConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="modules">The initial set of <see cref="ModuleConfigurationElement"/>.</param>
        /// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="modules"/> is <see langword="null"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModuleConfigurationElementCollection(ModuleConfigurationElement[] modules)
        {
            if (modules == null)
            {
                throw new ArgumentNullException(nameof(modules));
            }
            foreach (var module in modules)
            {
                BaseAdd(module);
            }
        }

        /// <summary>
        /// Gets a value indicating whether an exception should be raised if a duplicate element is found.
        /// This property will always return true.
        /// </summary>
        /// <value>A <see cref="bool"/> value.</value>
        protected override bool ThrowOnDuplicate => true;

        ///<summary>
        ///Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection" />.
        ///</summary>
        ///<value>
        ///The <see cref="T:System.Configuration.ConfigurationElementCollectionType" /> of this collection.
        ///</value>
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        ///<summary>
        ///Gets the name used to identify this collection of elements in the configuration file when overridden in a derived class.
        ///</summary>
        ///<value>
        ///The name of the collection; otherwise, an empty string.
        ///</value>
        protected override string ElementName => "module";

        /// <summary>
        /// Gets the <see cref="ModuleConfigurationElement"/> located at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index of the element in the collection.</param>
        /// <returns>A <see cref="ModuleConfigurationElement"/>.</returns>
        public ModuleConfigurationElement this[int index] => (ModuleConfigurationElement)BaseGet(index);

        /// <summary>
        /// Searches the collection for all the <see cref="ModuleConfigurationElement"/> that match the specified predicate.
        /// </summary>
        /// <param name="match">A <see cref="Predicate{T}"/> that implements the match test.</param>
        /// <returns>A <see cref="List{T}"/> with the successful matches.</returns>
        /// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="match"/> is null.</exception>
        public IList<ModuleConfigurationElement> FindAll(Predicate<ModuleConfigurationElement> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            IList<ModuleConfigurationElement> found = new List<ModuleConfigurationElement>();
            foreach (var moduleElement in this)
            {
                if (match(moduleElement))
                {
                    found.Add(moduleElement);
                }
            }
            return found;
        }

        /// <summary>
        /// Get the configuration object for each <see cref="NameTypeConfigurationElement"/> object in the collection.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> that is deserializing the element.</param>
        protected override Type RetrieveConfigurationElementType(XmlReader reader)
        {
            var configurationElementType = base.RetrieveConfigurationElementType(reader);
            if (configurationElementType == typeof(ModuleConfigurationElement))
            {
                for (var go = reader.MoveToFirstAttribute(); go; go = reader.MoveToNextAttribute())
                {
                    if (reader.Name.Equals("type"))
                    {
                        var providerType = Type.GetType(reader.Value, false);
                        if (providerType == null)
                        {
                            throw new Exception();  // TODO: Add a better exception here
                        }
                    }
                }
            }
            return configurationElementType;
        }
    }
}
