using System.Collections.Generic;
using System.Linq;

public class Stock
{
	public string Symbol;
	public List<StockValue> Values;

	public decimal CurrentValue => Values[^1].Close;
	public decimal HighestValue => GetHighestValue().Close;
	public decimal LowestValue => GetLowestValue().Close;


	public Stock()
	{
		Symbol = "";
		Values = new List<StockValue>();
	}
	public Stock(string symbol, List<StockValue> values)
	{
		Symbol = symbol;
		Values = values;
	}
	public Stock(AlphaVantageData alphaVantageData)
	{
		Symbol = alphaVantageData.Symbol;

		List<StockValue> stockValues = (alphaVantageData.TimeSeries.Select(alphaVantageTimeValue => new StockValue(this, alphaVantageTimeValue))).ToList();
		Values = stockValues;
	}


	public StockValue GetHighestValue()
	{
		if(Values.Count > 0)
		{
			StockValue highestValue = Values[0];

			foreach (StockValue value in Values)
			{
				if (value.High > highestValue.High)
				{
					highestValue = value;
				}
			}

			return highestValue;
		}

		return null;
	}

	public StockValue GetLowestValue()
	{
		if (Values.Count > 0)
		{
			StockValue lowestValue = Values[0];

			foreach (StockValue value in Values)
			{
				if (value.Low < lowestValue.Low)
				{
					lowestValue = value;
				}
			}

			return lowestValue;
		}

		return null;
	}
}