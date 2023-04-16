using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Stock
{
	public string Symbol;
	public List<StockValue> Values;

	public long GenerateTime;

	public double CurrentValue => Values.Count > 0 ? Values[^1].Close : -1;
	public double HighestCloseValue => GetHighestCloseValue().Close;
	public double LowestCloseValue => GetLowestCloseValue().Close;


	public Stock()
	{
		Symbol = "";
		Values = new List<StockValue>();
        GenerateTime = DateTime.Now.ToBinary(); //Serialize current date
    }
	public Stock(string symbol, List<StockValue> values)
	{
		Symbol = symbol;
		Values = values;
        GenerateTime = DateTime.Now.ToBinary(); //Serialize current date
    }
	public Stock(AlphaVantageData alphaVantageData)
	{
		Symbol = alphaVantageData.Symbol;

        Values = (alphaVantageData.TimeSeries.Select(alphaVantageTimeValue => new StockValue(alphaVantageTimeValue))).ToList();

        GenerateTime = DateTime.Now.ToBinary(); //Serialize current date
    }

    public Stock(Stock stock)
    {
        Symbol = stock.Symbol;

        Values = new(stock.Values);

        GenerateTime = DateTime.Now.ToBinary();
    }


    public StockValue GetHighestCloseValue()
	{
		if(Values.Count > 0)
		{
			StockValue highestValue = Values[0];

			foreach (StockValue value in Values)
			{
				if (value.Close > highestValue.Close)
				{
					highestValue = value;
				}
			}

			return highestValue;
		}

		return new(0);
	}

	public StockValue GetLowestCloseValue()
	{
		if (Values.Count > 0)
		{
			StockValue lowestValue = Values[0];

			foreach (StockValue value in Values)
			{
				if (value.Close < lowestValue.Close)
				{
					lowestValue = value;
				}
			}

			return lowestValue;
		}

		return new(0);
	}
}