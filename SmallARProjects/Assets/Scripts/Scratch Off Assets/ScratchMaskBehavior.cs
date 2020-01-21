using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchMaskBehavior : MonoBehaviour {

    PixelCalculation pc;

	void Start () {
        pc = FindObjectOfType<PixelCalculation>();
	}
	
	void Update () {
		if (pc.clearedPercentile > 85.5f)
        {
            Destroy(gameObject, 2.0f);
        }
	}
}
