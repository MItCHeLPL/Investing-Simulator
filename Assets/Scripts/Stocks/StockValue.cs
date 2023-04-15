using System;

[Serializable]
public struct StockValue
{
	public DateTime Timestamp => DateTime.FromBinary(TimestampBinary);
    public long TimestampBinary;
    public double Open;
	public double High;
	public double Low;
	public double Close;
	public double Volume;

	public StockValue(DateTime timestamp, double open, double high, double low, double close, double volume)
	{
        TimestampBinary = timestamp.ToBinary();
		Open = open;
		High = high;
		Low = low;
		Close = close;
		Volume = volume;
	}

	public StockValue(AlphaVantageTimeValue alphaVantageTimeValue)
	{
        TimestampBinary = alphaVantageTimeValue.Timestamp.ToBinary();
		Open = alphaVantageTimeValue.Open;
		High = alphaVantageTimeValue.High;
		Low = alphaVantageTimeValue.Low;
		Close = alphaVantageTimeValue.Close;
		Volume = alphaVantageTimeValue.Volume;
	}

    public StockValue(double zero)
    {
        TimestampBinary = DateTime.MinValue.ToBinary();
        Open = zero;
        High = zero;
        Low = zero;
        Close = zero;
        Volume = zero;
    }
}
