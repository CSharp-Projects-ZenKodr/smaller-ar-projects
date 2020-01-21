using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Vuforia;

public class AudioARController : MonoBehaviour, ITrackableEventHandler {

    public Animator anim;
    public AudioSource audioS;
    public TrackableBehaviour mTrackableBehavior;

    private void Start() {
        if (mTrackableBehavior) {
            mTrackableBehavior.RegisterTrackableEventHandler(this);
        }
        anim.StopPlayback();
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
            anim.Play(0);
            audioS.Play();
        } else {
            anim.StopPlayback();
            audioS.Stop();
        }
    }
}
