using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{
    public void Move(Vector2 moveTowards)
    {
        if (moveTowards != Vector2.zero)
        {
            //float angle = Mathf.Atan2(moveTowards.y, moveTowards.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.up = moveTowards;
            transform.position += new Vector3(moveTowards.x, moveTowards.y, 0f);
        }
    }
}
