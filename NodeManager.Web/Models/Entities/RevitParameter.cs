using System.Xml.Serialization;

namespace NodeManager.Web.Models.Entities
{
    public class RevitParameter
    {
        [XmlIgnoreAttribute]
        public RevitView Parent { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public StorageType StorageType { get; set; }
    }

    public enum StorageType
    {
        //
        // Summary:
        //     None represents an invalid storage type. This value should not be used.
        None = 0,
        //
        // Summary:
        //     The internal data is stored in the form of a signed 32 bit integer.
        Integer = 1,
        //
        // Summary:
        //     The data will be stored internally in the form of an 8 byte floating point number.
        Double = 2,
        //
        // Summary:
        //     The internal data will be stored in the form of a string of characters.
        String = 3,
        //
        // Summary:
        //     The data type represents an element and is stored as the id of the element.
        ElementId = 4
    }
}