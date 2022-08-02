using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;

public static class AlphaVantageHandler
{
	public static Stock GetStock(string function, string symbol, string interval, string apiKey)
	{
        //Get CSV string
        string csv = GetStockCSV(function, symbol, interval, apiKey);

        //Get AlphaVantageData object from CSV
        AlphaVantageData alphaVantageData = GetAlphaVantageDataFromCSV(symbol, interval, csv);

        //Create stock object from AlphaVantageData object
        Stock stock = new Stock(alphaVantageData);

        return stock;
	}

    private static AlphaVantageData GetAlphaVantageDataFromCSV(string symbol, string interval, string csv)
	{
        //Split CSV string to array of strings that contains each line from csv string
        string[] csvLines = csv.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        //Convert CSV to Objects
        //Without first (title) line and last (empty) line
        List<AlphaVantageTimeValue> alphaVantageValues = csvLines
                                                        .Skip(1)
                                                        .SkipLast(1)
                                                        .Select(AlphaVantageTimeValue.FromCSV)
                                                        .ToList();

        //Create AlphaVantageData object
        AlphaVantageData alphaVantageData = new AlphaVantageData(symbol, interval, alphaVantageValues);

        return alphaVantageData;
    }

    private static string GetStockJson(string function, string symbol, string interval, string apiKey)
	{
		//https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=IBM&interval=5min&apikey=demo
		string url = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&interval={interval}&apikey={apiKey}";

		Uri queryUri = new Uri(url);

		using (WebClient client = new WebClient())
		{
			return client.DownloadString(queryUri);
		}
    }

    private static string GetStockCSV(string function, string symbol, string interval, string apiKey)
    {
        //https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=IBM&interval=5min&apikey=demo
        string url = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&interval={interval}&apikey={apiKey}&datatype=csv";

        Uri queryUri = new Uri(url);

        using (WebClient client = new WebClient())
        {
            return client.DownloadString(queryUri);
        }
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