using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{

    public float moveSpeed = 20.0f;
    public float rotateTime = 2.0f;

    private float timeSpan;
    private bool shouldPlay = false;

    void Update()
    {


        if (shouldPlay)
        {
            if (timeSpan < rotateTime)
            {
                timeSpan += Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
            }
            else
            {
                transform.Rotate(new Vector3(0, 90.0f, 0), Space.Self);
                timeSpan = 0.0f;
            }
        }

    }

    public void StartMove()
    {
        shouldPlay = true;
    }
}
