using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpriteAnimate : MonoBehaviour
{
    public static TargetSpriteAnimate Instance;
    public float minScale = 0.1f;
    public float maxScale = 0.15f;
    public float duration = 1.0f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void FixedUpdate()
    {
        float scale = Mathf.PingPong(Time.time / duration, maxScale - minScale) + minScale;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
