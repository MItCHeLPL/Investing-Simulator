using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

public class GraphGenerator : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private byte _xLabelsAmount = 5;
	[SerializeField] private byte _yLabelsAmount = 5;

	[Header("References")]
	[SerializeField] private GameObject labelPrefab;

	[Space(15)]

	[SerializeField] private Transform labelsContainer;
	[SerializeField] private UILineRenderer uiLineRenderer;

	private RectTransform graphRect;
	private Stock currentStock;


	private void Start()
	{
		graphRect = uiLineRenderer.GetComponent<RectTransform>();
	}

	private void Update()
	{
		// Value indicator 
		if(currentStock != null)
		{
			//Get Player touch / cursor input if in uiLineRenderer rectPanel
			if (IsTouchInputOnGraph())
			{
				ShowPointData(GetClosestPointIndex(Input.GetTouch(0).position)); // TODO: Check, touch position might need conversion to be realtive to local rect transform position
			}
			else if (IsMouseInputOnGraph())
			{
				ShowPointData(GetClosestPointIndex(graphRect.InverseTransformPoint(Input.mousePosition)));
			}
			else
			{
				//Hide point data
				ShowPointData(-1);
			}
		}
	}


	public void GeneratePoints(Stock stock)
	{
		currentStock = stock;

		uiLineRenderer.Points = new Vector2[stock.Values.Count];

		//Graph
		// 0 - bottom left
		// 2 - top right
		Vector3[] panelCorners = new Vector3[4];
		graphRect.GetLocalCorners(panelCorners);

		//Labels
		// X
		int xLabelCount = 0;
		int xLabelEvery = Mathf.FloorToInt((stock.Values.Count - 1) / (_xLabelsAmount - 1));

		// Y
		int yLabelCount = 0;
		int yLabelEvery = Mathf.FloorToInt((stock.Values.Count - 1) / (_yLabelsAmount - 1));

		//Generate
		for (int i = stock.Values.Count - 1; i >= 0; i--)
		{
			StockValue stockValue = stock.Values[i];

			// Set point position
			double time = DataConverter.MapTo01(stockValue.Timestamp.Ticks, stock.Values[^1].Timestamp.Ticks, stock.Values[0].Timestamp.Ticks);
			double value = DataConverter.MapTo01((double)stockValue.Close, (double)stock.GetLowestValue().Close, (double)stock.GetHighestValue().Close);

			double x = DataConverter.MapFrom01(time, panelCorners[0].x, panelCorners[2].x);
			double y = DataConverter.MapFrom01(value, panelCorners[0].y, panelCorners[2].y);

			Vector2 pointPositon = new Vector2((float)x, (float)y);

			uiLineRenderer.Points[i] = pointPositon;

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

				double labelValue = DataConverter.Map(i, 0, (stock.Values.Count - 1), (double)stock.GetLowestValue().Close, (double)stock.GetHighestValue().Close);

				labelText.SetText(labelValue.ToString("F2"));

				//Counter
				yLabelCount++;
			}
		}
	}

	private int GetClosestPointIndex(Vector3 position)
	{
		int closestIndex = -1;

		float closestDistance = float.MaxValue;

		for(int i=0; i<uiLineRenderer.Points.Length; i++)
		{
			if(Mathf.Abs(position.x - uiLineRenderer.Points[i].x) < closestDistance)
			{
				closestIndex = i;
				closestDistance = Mathf.Abs(position.x - uiLineRenderer.Points[i].x);
			}
		}

		//Debug.Log(closestIndex); // TEMP, TODO: remove after tested touch position

		return closestIndex;
	}

	private void ShowPointData(int pointIndex)
	{
		// Concept: Get point's index and position -> Show dot and draw lines from point positon -> Show data of stock.Values[point's index].

		if(currentStock != null)
		{
			if (pointIndex >= 0 && pointIndex < currentStock.Values.Count)
			{
				Debug.Log($"{currentStock.Values[pointIndex].Close} - {currentStock.Values[pointIndex].Timestamp}"); // TEMP

				// TODO
			}
		}
	}

	public bool IsTouchInputOnGraph()
	{
		if (Input.touchCount > 0)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(graphRect, Input.GetTouch(0).position))
			{
				return true;
			}
		}

		return false;
	}
	public bool IsMouseInputOnGraph()
	{
		Vector2 localMousePosition = graphRect.InverseTransformPoint(Input.mousePosition);

		if (graphRect.rect.Contains(localMousePosition))
		{
			return true;
		}

		return false;
	}
}
