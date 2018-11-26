using System;
using System.Linq;
using NoskheAPI_Beta.Models;

namespace NoskheAPI_Beta.Classes.UIGenerator
{
    public static class UOIG // unique order identifier generator
    {
        public static string Generate()
        {
            NoskheContext db  = new NoskheContext();
            if(db.Orders.Count() != 0)
            {
                string uoiString = db.Orders.Last().UOI;
                int uoiInt = int.Parse(uoiString.Substring(1,6));
                return "O" + uoiInt;
            }
            return "O100000";
        }
    }
}