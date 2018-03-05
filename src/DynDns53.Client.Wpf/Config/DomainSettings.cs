using System.Configuration;

namespace DynDns53.Core
{
    public class DomainSettings : ConfigurationSection
    {
        private static DomainSettings settings = ConfigurationManager.GetSection("domainSettings") as DomainSettings;
        public static DomainSettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public DomainCollection DomainList
        {
            get { return (DomainCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class DomainCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DomainElement)element).SubDomain;
        }

        public void Add(DomainElement element)
        {
            base.BaseAdd(element);
        }

        public void Clear()
        {
            base.BaseClear();
        }

        public void Remove(string subdomain)
        {
            base.BaseRemove(subdomain);
        }
    }

    public class DomainElement : ConfigurationElement
    {
        [ConfigurationProperty("subDomain", IsKey = true, IsRequired = true)]
        public string SubDomain
        {
            get { return (string)base["subDomain"]; }
            set { base["subDomain"] = value; }
        }

        [ConfigurationProperty("zoneId", IsRequired = true)]
        public string ZoneId
        {
            get { return (string)base["zoneId"]; }
            set { base["zoneId"] = value; }
        }
    }
}