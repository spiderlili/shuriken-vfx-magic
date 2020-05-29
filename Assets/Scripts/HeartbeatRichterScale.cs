using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatRichterScale : MonoBehaviour
{
    private float yPos;
    [SerializeField] private int interval = 3;
    [SerializeField] private float yPosMax = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % interval == 0)
        {
            yPos = Random.Range(1, yPosMax);
            this.gameObject.transform.position = new Vector3(1, yPos, 1);
        }
    }
}
