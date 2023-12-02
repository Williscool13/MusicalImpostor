using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBouncer : MonoBehaviour
{
    [SerializeField] Vector2 delayRange = new Vector2(0.5f, 1f);
    [SerializeField] Vector2 jumpSpeedRange = new Vector2(3f, 6f);
    [SerializeField] Vector2 jumpHeightRange = new Vector2(-0.5f, 0.5f);


    float bounceTimer = 0;
    float currDelay = 0;

    Vector3 basePos;
    IEnumerator jumpCoroutine;
    WaitForFixedUpdate wait = new WaitForFixedUpdate();

    void Start()
    {
        basePos = transform.position;
        currDelay = Random.Range(delayRange.x, delayRange.y);
    }

    void Update()
    {
        if (jumpCoroutine != null) { 
            return; 
        }
        if (bounceTimer < currDelay) { 
            bounceTimer += Time.deltaTime;
            return; 
        }


        jumpCoroutine = Jump(Random.Range(jumpSpeedRange.x, jumpSpeedRange.y));
        StartCoroutine(jumpCoroutine);
    }

    IEnumerator Jump(float amp) {
        float baseTime = Time.time;
        float height = Random.Range(jumpHeightRange.x, jumpHeightRange.y);
        while (Time.time - baseTime < 1 / amp) {
            transform.position = basePos + Vector3.up * Mathf.Max(Mathf.Sin((Time.time - baseTime) * amp * Mathf.PI) + height, 0);
            yield return wait;
        }
        transform.position  = basePos;

        bounceTimer = 0;
        currDelay = Random.Range(delayRange.x, delayRange.y);
        jumpCoroutine = null;
    }
}
