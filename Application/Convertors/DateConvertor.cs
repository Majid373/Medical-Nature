using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            string mounth = "";
            var getMounth = pc.GetMonth(value);

            switch (getMounth)
            {
                case 1:
                    mounth = "فروردین";
                    break;

                case 2:
                    mounth = "اردیبهشت";
                    break;
                case 3:
                    mounth = "خرداد";
                    break;
                case 4:
                    mounth = "تیر";
                    break;
                case 5:
                    mounth = "مرداد";
                    break;
                case 6:
                    mounth = "شهریور";
                    break;
                case 7:
                    mounth = "مهر";
                    break;
                case 8:
                    mounth = "آبان";
                    break;
                case 9:
                    mounth = "آذر";
                    break;
                case 10:
                    mounth = "دی";
                    break;
                case 11:
                    mounth = "بهمن";
                    break;
                case 12:
                    mounth = "اسفند";
                    break;

            }

            return pc.GetDayOfMonth(value).ToString("00") + "  " + mounth + "  " +
                   pc.GetYear(value);
        }

        public static string FormattedDate(this DateOnly value)
        {
            DateOnly formattedDate = new DateOnly(value.Year, value.Month, value.Day);

            string month = "";
            var getMounth = formattedDate.Month;

            switch (getMounth)
            {
                case 1:
                    month = "فروردین";
                    break;

                case 2:
                    month = "اردیبهشت";
                    break;
                case 3:
                    month = "خرداد";
                    break;
                case 4:
                    month = "تیر";
                    break;
                case 5:
                    month = "مرداد";
                    break;
                case 6:
                    month = "شهریور";
                    break;
                case 7:
                    month = "مهر";
                    break;
                case 8:
                    month = "آبان";
                    break;
                case 9:
                    month = "آذر";
                    break;
                case 10:
                    month = "دی";
                    break;
                case 11:
                    month = "بهمن";
                    break;
                case 12:
                    month = "اسفند";
                    break;

            }

            return formattedDate.Day.ToString("00") + "  " + month + "  " +
                   formattedDate.Year;
        }


        public static DateOnly ConvertToShamsi(DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(value);
            int month= pc.GetMonth(value);
            int day = pc.GetDayOfMonth(value);

            return new DateOnly(year, month, day);  
        }
    }
}
