using System;

namespace ServerSideApp.Helpers
{
    public static class TimeHelper
    {
        public static string ToRelativeTimeString(this DateTime yourDate) {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.Now.Ticks - yourDate.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < MINUTE) {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * MINUTE) {
                return "a minute ago";
            }
            if (delta < HOUR) {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 2 * HOUR) {
                return "an hour ago";
            }
            if (delta < DAY) {
                return ts.Hours + " hours ago";
            }
            if (delta < 2 * DAY) {
                return "yesterday";
            }
            if (delta < MONTH) {
                return ts.Days + " days ago";
            }
            if (delta < 12 * MONTH) {
                int months = Convert.ToInt32(Math.Floor(ts.Days / 30.0));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            int years = Convert.ToInt32(Math.Floor(ts.Days / 365.0));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}