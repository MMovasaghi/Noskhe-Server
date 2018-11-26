using System;
using System.Linq;
using NoskheAPI_Beta.Models;

namespace NoskheAPI_Beta.Classes.UIGenerator
{
    public static class UPIG // unique pharmacy identifier generator
    {
        public static string Generate()
        {
            NoskheContext db  = new NoskheContext();
            if(db.Pharmacies.Count() != 0)
            {
                string upiString = db.Pharmacies.Last().UPI;
                int usciInt = int.Parse(upiString.Substring(2,6));
                return "PH" + usciInt;
            }
            return "PH100000";
        }
    }
}