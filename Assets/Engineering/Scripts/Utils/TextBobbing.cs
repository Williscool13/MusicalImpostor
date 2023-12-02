using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBobbing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool textBobbing = false;
    // Update is called once per frame
    void Update()
    {
        if (!textBobbing) { transform.localScale = new Vector3(0, 0, 0); return; }
        
        float scale = Mathf.Sin(Time.time * 4) * 0.05f + 0.95f;
        // movetowards scale
        float targetScale = Mathf.MoveTowards(transform.localScale.x, scale, Time.deltaTime * 2);
        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    public void StartBobbing() {
        textBobbing = true;
    }
}
