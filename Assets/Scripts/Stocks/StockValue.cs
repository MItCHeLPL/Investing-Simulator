using System;
using UnityEngine;

public class StockValue
{
	public Stock Stock;

	public DateTime Timestamp;
	public decimal Open;
	public decimal High;
	public decimal Low;
	public decimal Close;
	public decimal Volume;

	public StockValue()
	{
		Stock = null;
		Timestamp = DateTime.Now;
		Open = 0;
		High = 0;
		Low = 0;
		Close = 0;
		Volume = 0;
	}
	public StockValue(Stock stock, DateTime timestamp, decimal open, decimal high, decimal low, decimal close, decimal volume)
	{
		Stock = stock;
		Timestamp = timestamp;
		Open = open;
		High = high;
		Low = low;
		Close = close;
		Volume = volume;
	}
	public StockValue(Stock stock, AlphaVantageTimeValue alphaVantageTimeValue)
	{
		Stock = stock;
		Timestamp = alphaVantageTimeValue.Timestamp;
		Open = alphaVantageTimeValue.Open;
		High = alphaVantageTimeValue.High;
		Low = alphaVantageTimeValue.Low;
		Close = alphaVantageTimeValue.Close;
		Volume = alphaVantageTimeValue.Volume;
	}
}
