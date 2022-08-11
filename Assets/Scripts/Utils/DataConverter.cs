using System.Collections.Generic;

public static class DataConverter
{
	//LIST AND ARRAYS
	public static T[] ListToArray<T>(List<T> list)
	{
		T[] array = new T[list.Count];

		for (int i = 0; i < list.Count; i++)
		{
			array[i] = list[i];
		}

		return array;
	}
	public static List<T> ArrayToList<T>(T[] array)
	{
		List<T> list = new List<T>();

		for (int i = 0; i < array.Length; i++)
		{
			list.Add(array[i]);
		}

		return list;
	}


	//DICTIONARIES
	public static Dictionary<T1, T2> ArraysToDictionary<T1, T2>(T1[] keys, T2[] values)
    {
		Dictionary<T1, T2> output = new Dictionary<T1, T2>();

		for(int i=0; i< keys.Length; i++)
        {
			output.Add(keys[i], values[i]);
		}

		return output;
	}


	//MATH
	public static double Map(double value, double oldMinValue, double oldMaxValue, double newMinValue, double newMaxValue)
	{
		return newMinValue + (value - oldMinValue) * (newMaxValue - newMinValue) / (oldMaxValue - oldMinValue); //Map value from old range onto a new range
	}
	public static int Map(int value, int oldMinValue, int oldMaxValue, int newMinValue, int newMaxValue)
	{
		return newMinValue + (value - oldMinValue) * (newMaxValue - newMinValue) / (oldMaxValue - oldMinValue); //Map value from old range onto a new range
	}

	public static double MapFrom01(double value, double newMinValue, double newMaxValue)
	{
		return Map(value, 0.0d, 1.0d, newMinValue, newMaxValue);
	}
	public static double MapTo01(double value, double oldMinValue, double oldMaxValue)
	{
		return Map(value, oldMinValue, oldMaxValue, 0.0d, 1.0d);
	}
}
