using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using NodeManager.Domain;

namespace NodeManager.Web.Models
{
    public class FamSymbolViewModel
    {
        public FamilySymbol _familySymbol { get; set; }
        public IEnumerable<RevitParameter> _revitParameters { get; set; }

        public string _userName { get; set; }

        public static string ImageByteToBase64ImageTag(byte[] array)
        {
            //string base64 = Convert.ToBase64String(array);
            return Convert.ToBase64String(array);
        }

        public static string GetCutString(string str)
        {
            if (str.Length < 27) return str;
            else return str.Substring(0, 27) + "...";
        }

        public string userName = null;
    }
}