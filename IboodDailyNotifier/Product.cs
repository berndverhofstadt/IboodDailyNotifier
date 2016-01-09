﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IboodDailyNotifier
{
    class Product
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class rss
        {

            private rssChannel channelField;

            private decimal versionField;

            /// <remarks/>
            public rssChannel channel
            {
                get
                {
                    return this.channelField;
                }
                set
                {
                    this.channelField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class rssChannel
        {

            private link linkField;

            private string titleField;

            private string link1Field;

            private string descriptionField;

            private rssChannelImage imageField;

            private string lastBuildDateField;

            private byte ttlField;

            private string languageField;

            private string copyrightField;

            private rssChannelItem[] itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/Atom")]
            public link link
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
            [System.Xml.Serialization.XmlElementAttribute("link")]
            public string link1
            {
                get
                {
                    return this.link1Field;
                }
                set
                {
                    this.link1Field = value;
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

            /// <remarks/>
            public rssChannelImage image
            {
                get
                {
                    return this.imageField;
                }
                set
                {
                    this.imageField = value;
                }
            }

            /// <remarks/>
            public string lastBuildDate
            {
                get
                {
                    return this.lastBuildDateField;
                }
                set
                {
                    this.lastBuildDateField = value;
                }
            }

            /// <remarks/>
            public byte ttl
            {
                get
                {
                    return this.ttlField;
                }
                set
                {
                    this.ttlField = value;
                }
            }

            /// <remarks/>
            public string language
            {
                get
                {
                    return this.languageField;
                }
                set
                {
                    this.languageField = value;
                }
            }

            /// <remarks/>
            public string copyright
            {
                get
                {
                    return this.copyrightField;
                }
                set
                {
                    this.copyrightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("item")]
            public rssChannelItem[] item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
        public partial class link
        {

            private string hrefField;

            private string relField;

            private string typeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string href
            {
                get
                {
                    return this.hrefField;
                }
                set
                {
                    this.hrefField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string rel
            {
                get
                {
                    return this.relField;
                }
                set
                {
                    this.relField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class rssChannelImage
        {

            private string urlField;

            private string titleField;

            private string linkField;

            /// <remarks/>
            public string url
            {
                get
                {
                    return this.urlField;
                }
                set
                {
                    this.urlField = value;
                }
            }

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
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class rssChannelItem
        {

            private string titleField;

            private rssChannelItemGuid guidField;

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
            public rssChannelItemGuid guid
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
        public partial class rssChannelItemGuid
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
}
