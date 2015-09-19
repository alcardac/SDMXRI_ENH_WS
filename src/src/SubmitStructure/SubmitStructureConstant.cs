using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Estat.Sri.Ws.SubmitStructure
{
    public class SubmitStructureConstant
    {
        public enum ActionType
        {
            Append,
            Replace,
            Delete
        }

        public static string xmlTemplate = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<mes:Structure " +
            "xmlns:mes=\"http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message\">" +
            "  <mes:Header>" +
            "    <mes:ID>SUBMITSTRUCTURE</mes:ID> " +
            "    <mes:Test>false</mes:Test>" +
            "    <mes:Prepared>2014-05-06T21:53:11.874Z</mes:Prepared>" +
            "    <mes:Sender id=\"MG\"/>" +
            "    <mes:Receiver id=\"unknown\"/>" +
            "  </mes:Header> " +
            "</mes:Structure>";

    }
}
