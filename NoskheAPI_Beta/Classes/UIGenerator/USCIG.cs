using System;
using System.Linq;
using NoskheAPI_Beta.Models;

namespace NoskheAPI_Beta.Classes.UIGenerator
{
    public static class USCIG // unique shopping cart identifier generator
    {
        /*
         *    N-{0}{1}{2}{3}{4}
         *    {0} : version
         *    {1} : Year : { EX: 97 }
         *    {2} : Month
         *    {3} : Day
         *    {4} : 0001 : 10^4 state
         */
        private static int Version = 1;
        public static string Generate()
        {
            try
            {
                NoskheContext db = new NoskheContext();
                if (db.ShoppingCarts.Count() != 0)
                {
                    long uoiString = db.ShoppingCarts.Last().ShoppingCartId;
                    return string.Format("N-{0}{1}{2}{3}{4}", Version, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), uoiString.ToString() + 1000);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}