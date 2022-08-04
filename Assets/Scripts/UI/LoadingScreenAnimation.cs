using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreenAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private float _timeBetweenDots = 0.5f;

    [HideInInspector] public bool FinishedAnimationOnece = false;


    private void OnEnable()
	{
        FinishedAnimationOnece = false;

        StartCoroutine(LoadingCoroutine());
	}

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator LoadingCoroutine()
    {
        while(isActiveAndEnabled)
		{
            _text.SetText(".");

            yield return new WaitForSecondsRealtime(_timeBetweenDots);

            _text.SetText("..");

            yield return new WaitForSecondsRealtime(_timeBetweenDots);

            _text.SetText("...");

            yield return new WaitForSecondsRealtime(_timeBetweenDots);

            _text.SetText("");

            yield return new WaitForSecondsRealtime(_timeBetweenDots);

            FinishedAnimationOnece = true;
        }
    }
}
