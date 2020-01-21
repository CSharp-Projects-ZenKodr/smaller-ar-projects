using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PosterShooting : MonoBehaviour {

    #region Variables
    /// <summary>
    /// The Poster Hole prefab that will spawn on touch ended.
    /// </summary>
    [Tooltip("The Poster Hole prefab that will spawn on touch ended.")]
    public GameObject posterHoleObject;
    /// <summary>
    /// The AR Camera that is used in the scene.
    /// </summary>
    [Tooltip("The AR Camera that is used in the scene.")]
    public Camera ARCamera;
    /// <summary>
    /// The UI Reticle that will be used to aim.
    /// </summary>
    [Tooltip("The UI Reticle that will be used to aim.")]
    public Image reticle;
    /// <summary>
    /// The Text component used to debug things.
    /// </summary>
    [Tooltip("The Text component used to debug things.")]
    public Text debugText;

    /// <summary>
    /// The little reticle childed to the reticle.
    /// </summary>
    private Image littleReticle;
    /// <summary>
    /// Return true if you want the reticle to be green.
    /// </summary>
    private bool isGreen = false;
    #endregion

    private void Start() {
        littleReticle = reticle.transform.GetChild(0).GetComponent<Image>();
    }

    private void Update() {
        PlayerTouch();
        ReticleManagement();
    }

    void PlayerTouch() {
        if (!isGreen) {
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) {
                    GameObject spawnedHole = Instantiate(posterHoleObject, gameObject.transform);
                    spawnedHole.transform.parent = gameObject.transform;
                    spawnedHole.transform.position = touch.position;
                    DebugSnippet(spawnedHole.transform.position.ToString());

                    Ray ray = ARCamera.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit)) {
                        isGreen = true;
                        StartCoroutine(TurnReticleBackToRed());
                    }
                }
            }
        }
    }

    void DebugSnippet (string displayText) {
        debugText.text = displayText;
        StartCoroutine(MakeTextBlank());
    }

    void ReticleManagement () {
        if (isGreen) {
            reticle.color = Color.green;
            littleReticle.color = Color.green;
        } else {
            reticle.color = Color.red;
            littleReticle.color = Color.red;
        }
    }

    IEnumerator TurnReticleBackToRed () {
        yield return new WaitForSeconds(0.25f);
        isGreen = false;
        StopCoroutine(TurnReticleBackToRed());
    }

    IEnumerator MakeTextBlank() {
        yield return new WaitForSeconds(3.0f);
        debugText.text = string.Empty;
        StopCoroutine(MakeTextBlank());
    }
}