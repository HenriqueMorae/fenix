using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0.75f, 5f).setLoopPingPong();
    }
}
