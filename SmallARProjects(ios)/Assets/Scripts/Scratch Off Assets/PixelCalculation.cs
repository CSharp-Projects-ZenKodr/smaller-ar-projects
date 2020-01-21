using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PixelCalculation : MonoBehaviour {

    #region
    public List<Color> colorsInRTexture = new List<Color>();

    /// <summary>
    /// The amount of Contained Pixels in the RTexture.
    /// </summary>
    [Tooltip("The amount of Contained Pixels in the RTexture.")]
    public int containedPixels;

    /// <summary>
    /// The total amount of pixels within the RTexture.
    /// </summary>
    [Tooltip("The total amount of pixels within the RTexture.")]
    public int totalPixelsInRTexture;

    /// <summary>
    /// Percentage of the Winnings revealed.
    /// </summary>
    [Tooltip("Percentage of the Winnings revealed.")]
    public float clearedPercentile = 0.0f;

    /// <summary>
    /// The Render Texture that the winnings render to.
    /// </summary>
    [Tooltip("The Render Texture that the winnings render to.")]
    public RenderTexture rTexture;

    /// <summary>
    /// The Scrathable Class needed for reference.
    /// </summary>
    [Tooltip("The Scrathable Class needed for reference.")]
    public Scratchable scratch;

    /// <summary>
    /// The initial blank color.
    /// </summary>
    [Tooltip("The initial blank color.")]
    public Color initialBlankColor = Color.white;

    /// <summary>
    /// The Text component that visualizes the percentile of the scratch off shown.
    /// </summary>
    [Tooltip("The Text component that visualizes the percentile of the scratch off shown.")]
    public Text percentileText;

    /// <summary>
    /// The Image that holds the You Win graphic.
    /// </summary>
    [Tooltip("The Image that holds the You Win graphic.")]
    public Image youWinScreen;

    /// <summary>
    /// Has the player cleared the winnings?
    /// </summary>
    private bool cleared = false;
    #endregion

    private void Start()
    {
        youWinScreen.enabled = false;
        StartCoroutine(SceneBuffer());
        percentileText.text = "0.0%";
    }

    private void Update()
    {
        TestMethod();

        if (!cleared) {
            if (clearedPercentile > 85.5f)
            {
                scratch.control = false;
                scratch.fadeIn = true;
                cleared = true;
                StartCoroutine(ShowYouWinScreen());
            }
        }
    }

    void TestMethod()
    {
        if (scratch.control) {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) {
                    colorsInRTexture.Clear();

                    GetColorsInTexture();
                }
            }
        }
    }

    void GetColorsInTexture ()
    {
        //Touch touch = Input.GetTouch(0);

        RenderTexture.active = rTexture;
        containedPixels = 0;

        var texture2d = new Texture2D(rTexture.width, rTexture.height);
        texture2d.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);
        texture2d.Apply();

        initialBlankColor = texture2d.GetPixel(0, 0);

        for (int row = 0; row < texture2d.width; row++)
        {
            for (int col = 0; col < texture2d.height; col++)
            {
                if (!colorsInRTexture.Contains(texture2d.GetPixel(row, col)))
                {
                    colorsInRTexture.Add(texture2d.GetPixel(row, col));

                    if (texture2d.GetPixel(row, col) != initialBlankColor)
                    {
                        containedPixels++;
                    }
                }
            }
        }

        /*for (int i = 0; i < colorsInRTexture.Count; i++)
        {
            if (colorsInRTexture[i] != initialBlankColor)
            {
                containedPixels++;
            }
        }*/

        clearedPercentile = (((float)containedPixels / (float)totalPixelsInRTexture) * 100);
        percentileText.text = clearedPercentile.ToString("##.#") + "%";
        if (clearedPercentile <= 0.0f)
        {
            percentileText.text = "0.0%";
        }
    }

    IEnumerator SceneBuffer ()
    {
        yield return new WaitForSeconds(1.0f);
        GetColorsInTexture();
        StopCoroutine(SceneBuffer());
    }

    IEnumerator ShowYouWinScreen()
    {
        yield return new WaitForSeconds(5.0f);
        youWinScreen.enabled = true;
        StopCoroutine(ShowYouWinScreen());
    }
}