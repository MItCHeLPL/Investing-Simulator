using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;
using UnityEngine.Events;

public class GraphGenerator : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private byte _xLabelsAmount = 6;
	[SerializeField] private byte _yLabelsAmount = 5;

	[Header("References")]
	[SerializeField] private GameObject labelPrefab;

	[Space(15)]

	[SerializeField] private Transform labelsContainer;
	[SerializeField] private UILineRenderer graphUILineRenderer;

	private RectTransform _graphRect;

	[Header("Events")]
	public UnityEvent OnGeneratedGraph;


	private void Start()
	{
		_graphRect = graphUILineRenderer.GetComponent<RectTransform>();
	}


	public void GeneratePoints(Stock stock)
	{
		graphUILineRenderer.Points = new Vector2[stock.Values.Count];

		//Graph
		// 0 - bottom left
		// 2 - top right
		Vector3[] panelCorners = new Vector3[4];
		_graphRect.GetLocalCorners(panelCorners);

		double lowestValue = (double)stock.LowestCloseValue;
		double highestValue = (double)stock.HighestCloseValue;

		//Labels
		// X
		int xLabelCount = 0;
		int xLabelEvery = Mathf.FloorToInt((stock.Values.Count - 1) / (_xLabelsAmount - 1));

		// Y
		int yLabelCount = 0;
		int yLabelEvery = Mathf.FloorToInt((stock.Values.Count - 1) / (_yLabelsAmount - 1));

		//Destroy all old labels 
		DestroyChilren(labelsContainer.gameObject);


		//Generate
		for (int i = stock.Values.Count - 1; i >= 0; i--)
		{
			StockValue stockValue = stock.Values[i];

			// Set point position
			double time = DataConverter.MapTo01(stockValue.Timestamp.Ticks, stock.Values[^1].Timestamp.Ticks, stock.Values[0].Timestamp.Ticks);
			double value = DataConverter.MapTo01((double)stockValue.Close, lowestValue, highestValue);

			double x = DataConverter.MapFrom01(time, panelCorners[0].x, panelCorners[2].x);
			double y = DataConverter.MapFrom01(value, panelCorners[0].y, panelCorners[2].y);

			Vector2 pointPositon = new Vector2((float)x, (float)y);

			graphUILineRenderer.Points[i] = pointPositon;

			// Add labels
			// X
			if (((stock.Values.Count - 1) - i) == xLabelEvery * xLabelCount)
			{
				//Instantiation
				GameObject instantiation = Instantiate(labelPrefab, labelsContainer);

				//Position
				RectTransform labelRect = instantiation.GetComponent<RectTransform>();

				float labelXPosition = (float)DataConverter.Map(i, 0, (stock.Values.Count - 1), panelCorners[0].x, panelCorners[2].x);

				float xOffset = labelRect.sizeDelta.x / 4;
				float yOffset = -(labelRect.sizeDelta.y / 4);

				Vector2 labelPosition = new Vector2(labelXPosition + xOffset, panelCorners[0].y + yOffset);

				labelRect.anchoredPosition = labelPosition;

				//Text
				TextMeshProUGUI labelText = instantiation.GetComponent<TextMeshProUGUI>();

				long labelValue = (long)DataConverter.Map(i, 0, (stock.Values.Count - 1), stock.Values[^1].Timestamp.Ticks, stock.Values[0].Timestamp.Ticks);

				labelText.SetText(TimeConverter.ConvertTimeStrippedToHoursMinutes(new TimeSpan(labelValue)));

				//Counter
				xLabelCount++;
			}

			// Y
			if (((stock.Values.Count - 1) - i) == yLabelEvery * yLabelCount)
			{
				//Instantiation
				GameObject instantiation = Instantiate(labelPrefab, labelsContainer);

				//Position
				RectTransform labelRect = instantiation.GetComponent<RectTransform>();

				float labelYPosition = (float)DataConverter.Map(i, 0, (stock.Values.Count - 1), panelCorners[0].y, panelCorners[2].y);

				float xOffset = -(labelRect.sizeDelta.x / 4);
				float yOffset = labelRect.sizeDelta.y / 2;

				Vector2 labelPosition = new Vector2(panelCorners[0].x + xOffset, labelYPosition + yOffset);

				labelRect.anchoredPosition = labelPosition;

				//Text
				TextMeshProUGUI labelText = instantiation.GetComponent<TextMeshProUGUI>();

				double labelValue = DataConverter.Map(i, 0, (stock.Values.Count - 1), (double)stock.GetLowestCloseValue().Close, (double)stock.GetHighestCloseValue().Close);

				labelText.SetText(labelValue.ToString("F2"));

				//Counter
				yLabelCount++;
			}
		}

		OnGeneratedGraph.Invoke();
	}

	private void DestroyChilren(GameObject panel)
	{
		foreach (Transform child in panel.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
