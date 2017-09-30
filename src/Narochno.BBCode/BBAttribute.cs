using System;

namespace Narochno.BBCode
{
    public class BBAttribute
    {
        public BBAttribute(string id, string name)
            : this(id, name, null, HtmlEncodingMode.HtmlEncode)
        {
        }
        public BBAttribute(string id, string name, Func<IAttributeRenderingContext, string> contentTransformer)
            : this(id, name, contentTransformer, HtmlEncodingMode.HtmlEncode)
        {
        }
        public BBAttribute(string id, string name, Func<IAttributeRenderingContext, string> contentTransformer, HtmlEncodingMode htmlEncodingMode)
        {
            if (!Enum.IsDefined(typeof(HtmlEncodingMode), htmlEncodingMode))
            {
                throw new ArgumentException("htmlEncodingMode");
            }

            ID = id ?? throw new ArgumentNullException("id");
            Name = name ?? throw new ArgumentNullException("name");
            ContentTransformer = contentTransformer;
            HtmlEncodingMode = htmlEncodingMode;
        }

        public string ID { get; private set; } //ID is used to reference the attribute value
        public string Name { get; private set; } //Name is used during parsing
        public Func<IAttributeRenderingContext, string> ContentTransformer { get; private set; } //allows for custom modification of the attribute value before rendering takes place
        public HtmlEncodingMode HtmlEncodingMode { get; set; }

        public static Func<IAttributeRenderingContext, string> AdaptLegacyContentTransformer(Func<string, string> contentTransformer)
        {
            return contentTransformer == null ? (Func<IAttributeRenderingContext, string>)null : ctx => contentTransformer(ctx.AttributeValue);
        }
    }
    public interface IAttributeRenderingContext
    {
        BBAttribute Attribute { get; }
        string AttributeValue { get; }
        string GetAttributeValueByID(string id);
    }
}
