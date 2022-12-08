using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using NodeManager.Web.Abstract;

namespace NodeManager.Web.Models
{
    public class FamSymbolViewModel : IUser
    {
        public FamilySymbol _familySymbol { get; set; }
        public IEnumerable<RevitParameter> _revitParameters { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }
        public bool IsLogin { get; set; }

        public static string ImageByteToBase64ImageTag(byte[] array)
        {
            if(array == null) return null;
            //string base64 = Convert.ToBase64String(array);
            return Convert.ToBase64String(array);
        }

        public static string GetCutString(string str)
        {
            if (str.Length < 27) return str;
            else return str.Substring(0, 27) + "...";
        }
    }
}