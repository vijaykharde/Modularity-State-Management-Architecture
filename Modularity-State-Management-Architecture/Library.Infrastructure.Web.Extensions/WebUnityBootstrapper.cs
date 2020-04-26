using CsTech.Modularity;
using CsTech.Modularity.Configuration;
using CsTech.Modularity.Unity;
using System.Data;
using System.IO;
using Unity;
using System.Linq;
namespace Library.Infrastructure.Web.Extensions
{
    public class WebUnityBootstrapper : UnityBootstrapper
    {
        private readonly IUnityContainer _publicContainer;

        public WebUnityBootstrapper(IUnityContainer publicContainer)
        {
            _publicContainer = publicContainer;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModulesConfigurationSection configurationSection = new ModulesConfigurationSection();
            ModuleConfigurationElements ds = new ModuleConfigurationElements();
            if (File.Exists("ModuleConfigurationElementCollection.xml") && File.Exists("ModuleConfigurationElementCollection.xsd"))
            {
                ds.ReadXml("ModuleConfigurationElementCollection.xml");
                ds.ReadXmlSchema("ModuleConfigurationElementCollection.xsd");
                var elements = ds.Tables[0].Rows.Cast<DataRow>().Select(e => new ModuleConfigurationElement() { Name = e["Name"].ToString(), AssemblyFile = e["AssemblyFile"].ToString(), TypeName = e["TypeName"].ToString() }).ToArray();
                var modules = new ModuleConfigurationElementCollection(elements);
                configurationSection.Modules = modules;
            }

            return new ConfigurationModuleCatalog(configurationSection);
        }


        protected override IUnityContainer CreateContainer()
        {
            return _publicContainer;
        }
    }

    public class ModuleConfigurationElementBase
    {
        public string Name { get; set; }
        public string AssemblyFile { get; set; }
        public string TypeName { get; set; }
    }
}
