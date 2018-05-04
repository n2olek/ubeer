using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace S9.Utility
{
    public static class XMLConvert
    {
        private static string UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static byte[] StringToUTF8ByteArray(String xmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        }

        /// <summary>
        /// Serial object to xml string
        /// </summary>
        /// <typeparam name="T">any object type</typeparam>
        /// <param name="anyObject">target object type</param>
        /// <returns>xml string</returns>
        public static string SerializeObject<T>(this T anyObject) where T : class
        {
            try
            {
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var serializer = new XmlSerializer(typeof(T));
                var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, OmitXmlDeclaration = true };

                using (var stream = new StringWriter())
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, anyObject, emptyNamepsaces);
                    return stream.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


        /// <summary>
        /// Serial object to xml string UTF-8 
        /// </summary>
        /// <typeparam name="T">any object type</typeparam>
        /// <param name="anyObject">target object type</param>
        /// <returns>xml string</returns>
        public static string SerializeObject<T>(this T anyObject, bool hasIndent) where T : class
        {
            try
            {
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var serializer = new XmlSerializer(typeof(T));
                StringBuilder sb = new StringBuilder();
                using (StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, Encoding.UTF8))
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = hasIndent };
                    settings.Indent = true;
                    //settings.CloseOutput = true;
                    XmlWriter writer = XmlWriter.Create(stringWriter, settings);
                    serializer.Serialize(writer, anyObject, emptyNamepsaces);
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


        /// <summary>
        /// Deserialize to target object 
        /// </summary>
        /// <typeparam name="T">target object type</typeparam>
        /// <param name="xmlString">any xml string</param>
        /// <returns>target object</returns>
        public static T DeserializeObject<T>(this string xmlString) where T : class
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            return (T)xs.Deserialize(memoryStream);
        }


        /// <summary>
        /// Convert xml string to XElement type
        /// </summary>
        /// <param name="xmlString">any xml string</param>
        /// <returns>XElement</returns>
        public static XElement ToXElement(this string xmlString)
        {
            XElement xml = XElement.Parse(xmlString);
            return xml;
        }
    }

    public class StringWriterWithEncoding : StringWriter
    {
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            this.m_Encoding = encoding;
        }
        private readonly Encoding m_Encoding;
        public override Encoding Encoding
        {
            get
            {
                return this.m_Encoding;
            }
        }
    }

}
