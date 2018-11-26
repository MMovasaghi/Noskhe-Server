using System;
using System.Linq;
using NoskheAPI_Beta.Models;

namespace NoskheAPI_Beta.Classes.UIGenerator
{
    public static class USCIG // unique shopping cart identifier generator
    {
        public static string Generate()
        {
            NoskheContext db  = new NoskheContext();
            if(db.ShoppingCarts.Count() != 0)
            {
                string usciString = db.ShoppingCarts.Last().USCI;
                int usciInt = int.Parse(usciString.Substring(2,6));
                if(db.ShoppingCarts.Where(s => s.USCI == "").FirstOrDefault() == null)
                    return "SH" + usciInt;
                // else?!
            }
            return "SH100000";
        }
    }
}