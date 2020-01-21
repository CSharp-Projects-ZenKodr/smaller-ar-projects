using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PosterShooting : MonoBehaviour {

    #region Variables
    [Header("Items for Spawning")]
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

    [Space(8)]

    [Header("Items for UI")]
    /// <summary>
    /// The GameObject that holds the instructions for the game.
    /// </summary>
    [Tooltip("The GameObject that holds the instructions for the game.")]
    public GameObject instructionsText;
    /// <summary>
    /// The GameObject that holds the reload arrows.
    /// </summary>
    [Tooltip("The GameObject that holds the reload arrows.")]
    public GameObject reloadArrows;
    /// <summary>
    /// The rate at which the reload arrows will spin.
    /// </summary>
    [Tooltip("The rate at which the reload arrows will spin.")]
    public float arrowSpinSpeed;
    /// <summary>
    /// The UI Reticle that will be used to aim.
    /// </summary>
    [Tooltip("The UI Reticle that will be used to aim.")]
    public Image reticle;
    /// <summary>
    /// The Text component that will display how many shots the player has left.
    /// </summary>
    [Tooltip("The Text component that will display how many shots the player has left.")]
    public Text shotsLeftText;
    /// <summary>
    /// The Gradient that will change the color of shotsLeft based off of the ammo count.
    /// </summary>
    [Tooltip("The Gradient that will change the color of shotsLeft based off of the ammo count.")]
    public Gradient ammoGradient;

    [Space(8)]

    [Header("Items for Audio")]
    /// <summary>
    /// The audio clip that holds the sound of a shell reloading.
    /// </summary>
    [Tooltip("The audio clip that holds the sound of a shell reloading.")]
    public AudioSource reloadShell;
    /// <summary>
    /// The audio clip that holds the sound of a reload finishing.
    /// </summary>
    [Tooltip("The audio clip that holds the sound of a reload finishing.")]
    public AudioSource reloadFinal;

    //Private Variables

    /// <summary>
    /// A list of all the holes spawned.
    /// </summary>
    private List<GameObject> holes = new List<GameObject>();
    /// <summary>
    /// The little reticle childed to the reticle.
    /// </summary>
    private Image littleReticle;
    /// <summary>
    /// The Maximum Ammunition the player can have.
    /// </summary>
    private int maxAmmmoCount = 10;
    /// <summary>
    /// The Current Ammunition count the player has in their clip.
    /// </summary>
    private int currAmmoCount = 10;
    /// <summary>
    /// Return true if the player is able to fire.
    /// </summary>
    private bool abletoFire = true;
    /// <summary>
    /// Return true if you want the reticle to be green.
    /// </summary>
    private bool isGreen = false;
    /// <summary>
    /// Return true if you want the instructions to toggle on and off;
    /// </summary>
    private bool toggleTheInstructions = true;
    /// <summary>
    /// Return true to have the instructions display.
    /// </summary>
    private bool isDisplaying = true;
    /// <summary>
    /// Returns true if the clip's ammunition count is 0.
    /// </summary>
    private bool clipEmpty = false;
    /// <summary>
    /// Return true if the player is reloading.
    /// </summary>
    private bool reloading = false;
    #endregion

    private void Start() {
        littleReticle = reticle.transform.GetChild(0).GetComponent<Image>();
        reloadArrows.SetActive(false);
        SetInstructionsToNormal();
    }

    private void Update() {
        if (abletoFire) {
            PlayerTouch();
        }
        ReticleManagement();
        if (toggleTheInstructions) {
            ToggleInstructions();
        }
        AmmunitionManagement();
        if (reloading) {
            ReloadManagement();
        }
    }

    private void ReloadManagement() {
        reloadArrows.transform.Rotate(Vector3.forward * arrowSpinSpeed * Time.deltaTime);

        Text insTxt = instructionsText.GetComponent<Text>();
        insTxt.text = "Wait!";
    }

    void SetInstructionsToNormal () {
        Text insTxt = instructionsText.GetComponent<Text>();
        insTxt.fontStyle = FontStyle.Italic;
        insTxt.text = "Touch the screen to Fire!";
    }

    private void AmmunitionManagement() {
        if (currAmmoCount <= 0) {
            currAmmoCount = 0;
            clipEmpty = true;

            toggleTheInstructions = false;
            instructionsText.SetActive(true);
            Text insTxt = instructionsText.GetComponent<Text>();
            insTxt.fontStyle = FontStyle.Bold;
            if (!reloading) {
                insTxt.text = "Touch the screen to Reload!";
            }
        }

        shotsLeftText.text = currAmmoCount.ToString("00") + "/" + maxAmmmoCount.ToString("00");
        shotsLeftText.color = ammoGradient.Evaluate((float)currAmmoCount/(float)maxAmmmoCount);
    }

    private void ToggleInstructions() {
        if (isDisplaying) {
            instructionsText.SetActive(true);
        } else {
            instructionsText.SetActive(false);
        }
        if (isDisplaying) {
            StartCoroutine(ToggleDisplay(false));
        } else {
            StartCoroutine(ToggleDisplay(true));
        }
    }

    void PlayerTouch() {
        if (!isGreen) {
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) {
                    if (!clipEmpty) {
                        //Ray ray = ARCamera.ScreenPointToRay(touch.position);
                        RaycastHit hit;
                        
                        if (Physics.Raycast(ARCamera.transform.position, ARCamera.transform.forward, out hit)) {
                            GameObject spawnedHole = Instantiate(posterHoleObject, gameObject.transform);
                            spawnedHole.transform.position = hit.point;
                            spawnedHole.transform.rotation = hit.collider.gameObject.transform.rotation;
                            holes.Add(spawnedHole);

                            currAmmoCount--;

                            isGreen = true;
                            StartCoroutine(TurnReticleBackToRed());
                        }
                    } else {
                        reticle.gameObject.SetActive(false);
                        littleReticle.gameObject.SetActive(false);
                        reloadArrows.SetActive(true);
                        abletoFire = false;
                        reloading = true;
                        StartCoroutine(Reload());
                    }
                }
            }
        }
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

    #region Coroutines
    IEnumerator TurnReticleBackToRed () {
        yield return new WaitForSeconds(0.25f);
        isGreen = false;
        StopCoroutine(TurnReticleBackToRed());
    }

    IEnumerator ToggleDisplay (bool onOrOff) {
        yield return new WaitForSeconds(1.0f);
        isDisplaying = onOrOff;
        StopCoroutine(ToggleDisplay(onOrOff));
    }

    IEnumerator Reload () {
        for (int i = 0; i < maxAmmmoCount; i++) {
            currAmmoCount++;
            Destroy(holes[0]);
            holes.Remove(holes[0]);
            if (i < (maxAmmmoCount - 1)) {
                reloadShell.Play();
            } else if (i >= (maxAmmmoCount - 1)) {
                reloadFinal.Play();
            }
            yield return new WaitForSeconds(0.5f);
            continue;
        }
        reticle.gameObject.SetActive(true);
        littleReticle.gameObject.SetActive(true);
        reloadArrows.SetActive(false);
        SetInstructionsToNormal();
        toggleTheInstructions = true;
        abletoFire = true;
        clipEmpty = false;
        reloading = false;
        StopCoroutine(Reload());
    }
    #endregion
}