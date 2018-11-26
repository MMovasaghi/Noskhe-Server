using System;
using System.Collections.Generic;

namespace NoskheBackend_Beta.General
{
    public static class FirstAndLastWeekDayOfTheDay
    {
        public static List<DateTime> ReturnFirstAndLastDate()
        {
            DateTime today = DateTime.Today;
            List<DateTime> firstAndLast = new List<DateTime>();
            switch (today.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    firstAndLast.Add(DateTime.Now.AddDays(0).Date);
                    firstAndLast.Add(DateTime.Now.AddDays(7).Date);
                    break;
                case DayOfWeek.Sunday:
                    firstAndLast.Add(DateTime.Now.AddDays(-1).Date);
                    firstAndLast.Add(DateTime.Now.AddDays(6).Date);
                    break;
                case DayOfWeek.Monday:
                    firstAndLast.Add(DateTime.Now.AddDays(-2).Date);
                    firstAndLast.Add(DateTime.Now.AddDays(5).Date);
                    break;
                case DayOfWeek.Tuesday:
                    firstAndLast.Add(DateTime.Now.AddDays(-3).Date);
                    firstAndLast.Add(DateTime.Now.AddDays(4).Date);
                    break;
                case DayOfWeek.Wednesday:
                    firstAndLast.Add(DateTime.Now.AddDays(-4).Date);
                    firstAndLast.Add(DateTime.Now.AddDays(3).Date);
                    break;
                case DayOfWeek.Thursday:
                    firstAndLast.Add(DateTime.Now.AddDays(-5).Date);
                    firstAndLast.Add(DateTime.Now.AddDays(2));
                    break;
                case DayOfWeek.Friday:
                    firstAndLast.Add(DateTime.Now.AddDays(-6).Date);
                    firstAndLast.Add(DateTime.Now.AddDays(1).Date);
                    break;
                default:
                    break;
            }
            return firstAndLast;
        }
    }
}