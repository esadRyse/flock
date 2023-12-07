using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTargetAnimate : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public GameObject ScanShip;
    public GameObject ScanShipSleepTarget;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    void Update()
    {
        if (ScanShip == null) return;
        if (!ScanShip.activeInHierarchy) ScanShip.transform.position = ScanShipSleepTarget.transform.position;
        if (Input.GetMouseButtonDown(1))
        {
            if (!spriteRenderer.enabled) spriteRenderer.enabled = true;
            if (!ScanShip.activeInHierarchy) ScanShip.SetActive(true);
            Vector3 scanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(scanPos.x, scanPos.y, 1);
        }
        transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
