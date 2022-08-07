using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
    public static bool IsMobile()
	{
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return true;
		}

		return false;
	}
}
