using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Ad;
using System.Runtime.InteropServices;

public class ad : MonoBehaviour
{
    #if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _requestIDFA();
    #endif

    IronSourceController ironSourceController;
    void Start()
    {
        ironSourceController = GetComponent<IronSourceController>();
#if UNITY_IOS
        _requestIDFA();
#endif
    }

    public void PlayAd()
    {
        ironSourceController.PlayRewardedVideo();
    }
}
