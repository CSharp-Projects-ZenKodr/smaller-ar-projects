using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Scratchable : MonoBehaviour {

    #region Variables
    /// <summary>
    /// Return true for player control.  Return false for no player control.
    /// </summary>
    [Tooltip("Return true for player control.  Return false for no player control.")]
    public bool control = true;

    /// <summary>
    /// The camera that is in the scene.
    /// </summary>
    [Tooltip("The camera that is in the scene.")]
    public Camera sceneCamera;

    /// <summary>
    /// The SpriteRenderer attached to the Winnings BG.
    /// </summary>
    [Tooltip("The SpriteRenderer attached to the Winnings BG.")]
    public SpriteRenderer winningsBG;

    /// <summary>
    /// The GameObject that holds the scratch mask.
    /// </summary>
    [Tooltip("The GameObject that holds the scratch mask.")]
    public GameObject scratchMask;

    /// <summary>
    /// The GameObject that holds the complete mask.
    /// </summary>
    [Tooltip("The GameObject that holds the complete mask.")]
    public GameObject completeMask;

    /// <summary>
    /// The GameObject that the maskObjects will parent to.
    /// </summary>
    [Tooltip("The GameObject that the maskObjects will parent to.")]
    public Transform parentObject;

    [Header("Fade Variables")]
    /// <summary>
    /// The image that will fade to white.
    /// </summary>
    [Tooltip("The image that will fade to white.")]
    public Image whiteFade;

    /// <summary>
    /// The value the whiteFade will fade by.
    /// </summary>
    [Tooltip("The value the whiteFade will fade by.")]
    public float fadeValue = 0.01f;

    /// <summary>
    /// Return true to fade whiteFade in.
    /// </summary>
    [HideInInspector]
    public bool fadeIn = false;
    #endregion

    /*void emp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
            }
        }
    }*/

    private void Start()
    {
        completeMask.SetActive(false);
    }

    private void Update()
    {

        if (control)
        {
            //ScratchWinnings();
            TestScratching();
        }

        Checking();
    }

    void Checking()
    {
        if (fadeIn)
        {
            FadeToWhite();
        } else {
            FadeToTransparency();
        }
        if (whiteFade.color.a > 1.0f)
        {
            completeMask.SetActive(true);
            fadeIn = false;
        }
        if (whiteFade.color.a < 0.0f)
        {
            whiteFade.color = Color.clear;
        }
    }

    private void TestScratching()
    {
        if (control) {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    SpawnScratchGraphic();
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    SpawnScratchGraphic();
                }
            }
        }
    }

    /*private void OnMouseDrag()
    {
        SpawnScratchGraphic();
    }*/

    void SpawnScratchGraphic()
    {
        Touch touch = Input.GetTouch(0);

        if (control) {

            Ray ray = sceneCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject a = Instantiate(scratchMask, parentObject);
                a.transform.position = hit.point;
                a.transform.rotation = hit.collider.gameObject.transform.rotation;
            }
        }
    }

    void ScratchWinnings()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = sceneCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Instantiate(scratchMask, touch.position, hit.collider.gameObject.transform.rotation);
                }
            }
        }
    }

    void FadeToWhite ()
    {
        if (whiteFade.color.a < 1.0f) {
            Color temp = whiteFade.color;

            temp.a += fadeValue * Time.deltaTime;

            whiteFade.color = temp;
        }
    }

    void FadeToTransparency ()
    {
        if (whiteFade.color.a > 0.0f) {
            Color temp = whiteFade.color;

            temp.a -= fadeValue * Time.deltaTime;

            whiteFade.color = temp;
        }
    }
}