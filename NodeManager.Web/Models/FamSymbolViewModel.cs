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

        
        public static string ImageByteToBase64ImageTag(byte[] array)
        {
            string base64 = Convert.ToBase64String(array);
            return base64;
        }
    }
}