using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterFlickerLight : MonoBehaviour
{
    public LineRenderer thrusterLine;
    public Light thrusterLight;
    public float maxLength = 30;

    [Range(0, 0.1f)]
    public float flickerAmount = 0.1f;
    public float flickerSpeed = 60;
    public bool velocityBasedLength = false;
    public float velocityModifier = 10; //adjust velocity based length effect based on the ship speed

    float lightIntensity;
    float speed;
    float length;
    Color thrusterColor;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        thrusterLine.SetPosition(1, -Vector3.forward * length);
        thrusterColor = thrusterLine.material.GetColor("_TintColor");
        lightIntensity = thrusterLight.intensity;
        InvokeRepeating("Flicker", 0, 1 / flickerSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        length = maxLength; //update length when changing it dynamically based on velocity
        if (velocityBasedLength == true)
        {
            ComputeThrusterLength();
            length = Mathf.Clamp(speed, 0, maxLength);
        }
    }

    void Flicker()
    {
        //generate random value - randomise the color, length, light intensity of the thruster
        float noise = Random.Range(1 - flickerAmount, 1);
        thrusterLine.material.SetColor("_TintColor", thrusterColor * noise); //dims material slightly
        thrusterLine.SetPosition(1, -Vector3.forward * length * noise);
        thrusterLight.intensity = noise * (Mathf.Clamp(length, 0, 8));
    }

    void ComputeThrusterLength()
    {
        speed = velocityModifier * (transform.position - position).magnitude;
        position = transform.position; //update the previous position
    }
}
