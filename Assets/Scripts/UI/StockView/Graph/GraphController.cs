using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class GraphController : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Vector2 dotOffset;
	[SerializeField] private Vector2 labelOffset;

	[Header("References")]
	[SerializeField] private GraphGenerator graphGenerator;
	[SerializeField] private UILineRenderer graphUILineRenderer;

	[Space(15)]

	[SerializeField] private GameObject indicatorGameObject;
	[SerializeField] private UILineRenderer indicatorUILineRenderer;
	[SerializeField] private RectTransform indicatorDotRect;
	[SerializeField] private RectTransform indicatorLabelRect;
	[SerializeField] private TextMeshProUGUI indicatorLabelText;

	private Camera cam;
	private RectTransform _graphRect;
	private Vector3[] _graphPanelCorners = new Vector3[4];


	private void Start()
	{
		cam = Camera.main;

		_graphRect = graphUILineRenderer.GetComponent<RectTransform>();

		graphGenerator.OnGeneratedGraph.AddListener(GetGraphCorners); //Update graphrect corners on generation
	}

	private void Update()
	{
		// Value indicator 
		if (StockViewer.CurrentStock != null)
		{
			//Get Player touch / cursor input if in uiLineRenderer rectPanel
			if (IsTouchInputOnGraph() && DeviceDetector.IsMobile())
			{
				ShowPointData(GetClosestPointIndex(cam.ScreenToWorldPoint(Input.GetTouch(0).position)));
			}
			else if (IsMouseInputOnGraph() && !DeviceDetector.IsMobile())
			{
				ShowPointData(GetClosestPointIndex(_graphRect.InverseTransformPoint(Input.mousePosition)));
			}
			else
			{
				//Hide point data
				ShowPointData(-1);
			}
		}
	}


	// Get closest point from graph to touch/mouse
	private int GetClosestPointIndex(Vector3 position)
	{
		int first = 0;
		int last = graphUILineRenderer.Points.Length - 1;
		int mid = 0;

		// Do 10 binary search splits to narrow down area to look for closest index
		for (int i=0; i < 10; i++)
		{
			mid = first + (last - first) / 2;

			if (graphUILineRenderer.Points[mid].x == position.x)
			{
				return mid;
			}
			else
			{
				if (position.x > graphUILineRenderer.Points[mid].x)
				{
					last = mid - 1;
				}
				else
				{
					first = mid + 1;
				}
			}
		}

		// Go through all points in area and find closest one
		// if getting away from closest break loop and return index
		int closestIndex = first;
		float prevDistance = float.MaxValue;
		
		for (int i = first; i <= last; i++)
		{
			float distance = Mathf.Abs(position.x - graphUILineRenderer.Points[i].x);

			if(distance < prevDistance)
			{
				closestIndex = i;
				prevDistance = distance;
			}
			else
			{
				break;
			}
		}

		return closestIndex;
	}

	private void ShowPointData(int pointIndex)
	{
		if (StockViewer.CurrentStock != null && indicatorGameObject != null)
		{
			//If on graph
			if (pointIndex >= 0 && pointIndex < StockViewer.CurrentStock.Values.Count)
			{
				//Show indicator
				indicatorGameObject.SetActive(true);

				//Set lines position
				if (indicatorUILineRenderer != null)
				{
					indicatorUILineRenderer.Points = new Vector2[3];

					indicatorUILineRenderer.Points[0] = new Vector2(_graphPanelCorners[0].x, graphUILineRenderer.Points[pointIndex].y);
					indicatorUILineRenderer.Points[1] = new Vector2(graphUILineRenderer.Points[pointIndex].x, graphUILineRenderer.Points[pointIndex].y);
					indicatorUILineRenderer.Points[2] = new Vector2(graphUILineRenderer.Points[pointIndex].x, _graphPanelCorners[0].y);
				}
				
				//Set dot position
				if(indicatorDotRect != null)
				{
					indicatorDotRect.localPosition = new Vector2(graphUILineRenderer.Points[pointIndex].x + dotOffset.x, graphUILineRenderer.Points[pointIndex].y + dotOffset.y);
				}
				
				//Set label position and text
				if(indicatorLabelRect != null && indicatorLabelText != null)
				{
					indicatorLabelRect.localPosition = new Vector2(graphUILineRenderer.Points[pointIndex].x + labelOffset.x, indicatorLabelRect.localPosition.y + labelOffset.y);

					indicatorLabelText.SetText($"{StockViewer.CurrentStock.Values[pointIndex].Close}\n{StockViewer.CurrentStock.Values[pointIndex].Timestamp}");
				}
			}
			else
			{
				//Hide indicator if not on mobile and out of graph boundries
				if(indicatorGameObject != null && !DeviceDetector.IsMobile())
				{
					indicatorGameObject.SetActive(false);
				}
			}
		}
		else
		{
			//Hide indicator if there's no graph
			if (indicatorGameObject != null)
			{
				indicatorGameObject.SetActive(false);
			}
		}
	}

	public bool IsTouchInputOnGraph()
	{
		if (Input.touchCount > 0)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(_graphRect, Input.GetTouch(0).position))
			{
				return true;
			}
		}

		return false;
	}
	public bool IsMouseInputOnGraph()
	{
		Vector2 localMousePosition = _graphRect.InverseTransformPoint(Input.mousePosition);

		if (_graphRect.rect.Contains(localMousePosition))
		{
			return true;
		}

		return false;
	}

	private void GetGraphCorners()
	{
		_graphRect.GetLocalCorners(_graphPanelCorners);
	}
}
