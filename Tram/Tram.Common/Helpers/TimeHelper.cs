using System;

namespace Tram.Common.Helpers
{
    public static class TimeHelper
    {
        public static DateTime GetTime(int hours, int minutes)
        {
            return (new DateTime()).AddHours(hours).AddMinutes(minutes);
        }

        // string format - 'HH:MM'
        public static DateTime GetTime(string timeStr)
        {
            string[] parts = timeStr.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            return (new DateTime()).AddHours(int.Parse(parts[0])).AddMinutes(int.Parse(parts[1]));
        }

        public static string GetTimeStr(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        public static string GetExtTimeStr(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        // -1 time1 is earlier than time2
        //  0 time1 is the same as time2
        //  1 time1 is later than time2
        public static int CompareTimes(DateTime time1, DateTime time2)
        {
            int time1Value = time1.Hour * 3600 + time1.Minute * 60 + time1.Second;
            int time2Value = time2.Hour * 3600 + time2.Minute * 60 + time2.Second;

            if (time1Value < time2Value)
            {
                return -1;
            }
            else if (time1Value > time2Value)
            {
                return 1;
            }

            return 0;
        }
    }
}
