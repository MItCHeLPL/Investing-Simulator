using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
	private Scrollbar scrollbar;
	[SerializeField] private float _amount = 0.2f;

    void Start()
    {
		scrollbar = GetComponent<Scrollbar>();
	}

    void Update()
    {
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			scrollbar.value += _amount;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			scrollbar.value -= _amount;
		}
	}
}
