using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Estat.Sri.Ws.SubmitStructure
{
    public class SubmitStructureUtil
    {

        #region Public Methods

        /// <summary>
        /// Validate v21 Document
        /// </summary>
        /// <param name="xDomSource">Document to validate</param>
        /// <returns>Action value</returns>
        public static string ValidateDocument(XmlDocument xDomSource)
        {
            string actionValue = string.Empty;

            try
            {
                actionValue = GetAction21(xDomSource);

                // Error: Missing tag "SubmitStructureRequest" or Missing attribute "action"
                if (actionValue == string.Empty)
                    throw new Exception("Invalid message: missing element 'SubmitStructureRequest' or Missing attribute 'action'");

                // Error: Missing tag "Structures"
                if (xDomSource.SelectSingleNode("//*[local-name()='Structures']") == null)
                    throw new Exception("Invalid message: missing element 'Structures'");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return actionValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDomSource"></param>
        /// <returns></returns>
        public static string GetAction20(XmlDocument xDomSource)
        {
            XmlNodeList nodeList = xDomSource.SelectNodes("//*[local-name()='SubmittedStructure']");

            foreach (XmlNode node in nodeList)
            {
                if (node.NamespaceURI == "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")
                {
                    XmlAttribute xAtt = node.Attributes["action"];
                    if (xAtt != null)
                        return xAtt.Value;
                }
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDomSource"></param>
        /// <returns></returns>
        public static string GetAction21(XmlDocument xDomSource)
        {
            XmlNodeList nodeList = xDomSource.SelectNodes("//*[local-name()='SubmitStructureRequest']");

            foreach (XmlNode node in nodeList)
            {
                if (node.NamespaceURI == "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")
                {
                    XmlAttribute xAtt = node.Attributes["action"];
                    if (xAtt != null)
                        return xAtt.Value;
                }
            }
            return "";
        }

        /// <summary>
        /// Convert a Message to XMLDocument
        /// </summary>
        /// <param name="msg">Message to convert</param>
        /// <returns></returns>
        public static XmlDocument MessageToXDom(System.ServiceModel.Channels.Message msg)
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms);

            msg.WriteMessage(writer);

            writer.Flush();
            ms.Position = 0;

            doc.Load(ms);

            return doc;
        }

        /// <summary>
        /// Convert a XMLDocument to Bytes Array
        /// </summary>
        /// <param name="doc">XMLDocument to convert</param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(XmlDocument doc)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] docAsBytes = encoding.GetBytes(doc.OuterXml);
            return docAsBytes;
        }

        public static void SetHeader(XmlDocument xDomTemp, XmlDocument xDomSource)
        {
            XmlNode xNodeSourceHeader = null;
            XmlNode xNodeTempHeader = null;
            XmlNode xNodeTempStructure = null;

            // Trovo nel doc source l'elemento Header con namespace message
            XmlNodeList nodeList = xDomSource.SelectNodes("//*[local-name()='Header']");
            foreach (XmlNode node in nodeList)
            {
                if (node.NamespaceURI == "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")
                    xNodeSourceHeader = node;
            }

            // Se trovo l'elemento 
            if (xNodeSourceHeader != null)
            {
                // Mi posiziono sul tag Structure del template elimino il figlio e gli aggiungo l'Header del source
                xNodeTempStructure = xDomTemp.SelectSingleNode("//*[local-name()='Structure']");

                xNodeTempStructure.RemoveChild(xNodeTempStructure.FirstChild);
                xNodeTempHeader = xDomTemp.CreateElement("Header", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");
                xNodeTempHeader.InnerXml = xNodeSourceHeader.InnerXml;
                xNodeTempStructure.AppendChild(xNodeTempHeader);
            }
        }


        #endregion

    }
}
