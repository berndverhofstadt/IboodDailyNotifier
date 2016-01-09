using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IboodDailyNotifier
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class item
    {

        private string titleField;

        private itemGuid guidField;

        private string linkField;

        private string pubDateField;

        private string descriptionField;

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public itemGuid guid
        {
            get
            {
                return this.guidField;
            }
            set
            {
                this.guidField = value;
            }
        }

        /// <remarks/>
        public string link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        public string pubDate
        {
            get
            {
                return this.pubDateField;
            }
            set
            {
                this.pubDateField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class itemGuid
    {

        private bool isPermaLinkField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isPermaLink
        {
            get
            {
                return this.isPermaLinkField;
            }
            set
            {
                this.isPermaLinkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

}
