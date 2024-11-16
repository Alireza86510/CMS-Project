using System.Globalization;

namespace CMS.Core.Convertors;

public static class DateConvertor
{
    public static DateTime ToShamsi(this DateTime dateTime)
    {
        PersianCalendar pc = new PersianCalendar();
        return DateTime.Parse(pc.GetYear(dateTime) + "/" + pc.GetMonth(dateTime) + "/" + pc.GetDayOfMonth(dateTime));
    }
}