using System;

public class AlphaVantageTimeValue
{
    public DateTime Timestamp { get; set; }
	public decimal Open { get; set; }
	public decimal High { get; set; }
	public decimal Low { get; set; }
	public decimal Close { get; set; }
	public decimal Volume { get; set; }

    public AlphaVantageTimeValue(DateTime timestamp, decimal open, decimal high, decimal low, decimal close, decimal volume)
    {
        Timestamp = timestamp;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }
    public AlphaVantageTimeValue(string[] values)
	{
        Timestamp = DateTime.Parse(values[0]);
        Open = decimal.Parse(values[1]);
        High = decimal.Parse(values[2]);
        Low = decimal.Parse(values[3]);
        Close = decimal.Parse(values[4]);
        Volume = decimal.Parse(values[5]);
    }

    public static AlphaVantageTimeValue FromCSV(string csvLine)
    {
        csvLine = csvLine.Trim();

        if (csvLine != "")
        {
            string[] values = csvLine.Split(',');

            //Replace all '.' to ',' in deciamal values
            for (int i = 1; i < values.Length; i++)
		    {
                values[i] = values[i].Replace('.', ',');
		    }
        
            AlphaVantageTimeValue alphaVantageTimeValue = new AlphaVantageTimeValue(values);

            return alphaVantageTimeValue;
        }

        return null;
    }
}



/* AlphaVantage CSV Structure:
timestamp,open,high,low,close,volume
2022-08-01 19:55:00,132.0400,132.0400,132.0400,132.0400,212
2022-08-01 18:10:00,132.0400,132.0400,132.0400,132.0400,477
2022-08-01 17:25:00,132.1500,132.1500,132.1500,132.1500,100
2022-08-01 17:15:00,132.0000,132.0000,132.0000,132.0000,401 
*/


/* AlphaVantage JSON Structure:
{
    "Meta Data": {
        "1. Information": "Intraday (5min) open, high, low, close prices and volume",
        "2. Symbol": "IBM",
        "3. Last Refreshed": "2022-08-01 19:55:00",
        "4. Interval": "5min",
        "5. Output Size": "Compact",
        "6. Time Zone": "US/Eastern"
    },
    "Time Series (5min)": {
        "2022-08-01 19:55:00": {
            "1. open": "132.0400",
            "2. high": "132.0400",
            "3. low": "132.0400",
            "4. close": "132.0400",
            "5. volume": "212"
        },
        "2022-08-01 18:10:00": {
            "1. open": "132.0400",
            "2. high": "132.0400",
            "3. low": "132.0400",
            "4. close": "132.0400",
            "5. volume": "477"
        },
        "2022-08-01 17:25:00": {
            "1. open": "132.1500",
            "2. high": "132.1500",
            "3. low": "132.1500",
            "4. close": "132.1500",
            "5. volume": "100"
        },
        "2022-08-01 17:15:00": {
            "1. open": "132.0000",
            "2. high": "132.0000",
            "3. low": "132.0000",
            "4. close": "132.0000",
            "5. volume": "401"
        }
    }
}
 */