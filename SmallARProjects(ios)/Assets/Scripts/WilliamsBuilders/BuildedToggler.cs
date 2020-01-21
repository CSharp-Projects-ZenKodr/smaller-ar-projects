using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Vuforia;

using NelsonTools;

public class BuildedToggler : MonoBehaviour {
    public GameObject imageTarget;
    public BuildModifier buildMod;

    private bool isTrackingMarker()
    {
        var trackable = imageTarget.GetComponent<TrackableBehaviour>();
        var status = trackable.CurrentStatus;
        return status == TrackableBehaviour.Status.TRACKED;
    }

    private void Start()
    {
        buildMod.DisabledAll();
    }

    private void Update()
    {
        if (!isTrackingMarker())
        {
            buildMod.enabled = false;
        } else
        {
            buildMod.enabled = true;
        }
    }
}