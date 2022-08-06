using System;

public static class TimeConverter
{
	//Set output
	public static string ConvertTime(TimeSpan timeSpan, string format)
	{
		string output = timeSpan.ToString(format);
		return output;
	}
	public static string ConvertTime(double time, string format)
	{
		return ConvertTime(TimeSpan.FromSeconds(time), format);
	}
	public static string ConvertTime(float time, string format)
	{
		return ConvertTime((double)time, format);
	}

	//output: xx:xx:xxxxx (mm:ss:ms)
	public static string ConvertTime(TimeSpan timeSpan)
	{
		string output = timeSpan.ToString(@"mm\:ss\:fffff");
		return output;
	}
	public static string ConvertTime(double time)
	{
		return ConvertTime(TimeSpan.FromSeconds(time));
	}
	public static string ConvertTime(float time)
	{
		return ConvertTime((double)time);
	}

	//output: xx:xx:xxx (mm:ss:ms)
	public static string ConvertTimeStripped(TimeSpan timeSpan)
	{
		string output = timeSpan.ToString(@"mm\:ss\:fff");
		return output;
	}
	public static string ConvertTimeStripped(double time)
	{
		return ConvertTimeStripped(TimeSpan.FromSeconds(time));
	}
	public static string ConvertTimeStripped(float time)
	{
		return ConvertTimeStripped((double)time);
	}

	//output: xx:xx (mm:ss)
	public static string ConvertTimeStrippedToSeconds(TimeSpan timeSpan)
	{
		string output = timeSpan.ToString(@"mm\:ss");
		return output;
	}
	public static string ConvertTimeStrippedToSeconds(double time)
	{
		return ConvertTimeStrippedToSeconds(TimeSpan.FromSeconds(time));
	}
	public static string ConvertTimeStrippedToSeconds(float time)
	{
		return ConvertTimeStrippedToSeconds((double)time);
	}

	//output: xx:xx (hh:mm:ss)
	public static string ConvertTimeStrippedToHoursMinutesSeconds(TimeSpan timeSpan)
	{
		string output = timeSpan.ToString(@"hh\:mm\:ss");
		return output;
	}
	public static string ConvertTimeStrippedToHoursMinutesSeconds(double time)
	{
		return ConvertTimeStrippedToHoursMinutesSeconds(TimeSpan.FromSeconds(time));
	}
	public static string ConvertTimeStrippedToHoursMinutesSeconds(float time)
	{
		return ConvertTimeStrippedToHoursMinutesSeconds((double)time);
	}

	//output: xx:xx (hh:mm)
	public static string ConvertTimeStrippedToHoursMinutes(TimeSpan timeSpan)
	{
		string output = timeSpan.ToString(@"hh\:mm");
		return output;
	}
	public static string ConvertTimeStrippedToHoursMinutes(double time)
	{
		return ConvertTimeStrippedToHoursMinutes(TimeSpan.FromSeconds(time));
	}
	public static string ConvertTimeStrippedToHoursMinutes(float time)
	{
		return ConvertTimeStrippedToHoursMinutes((double)time);
	}
}
