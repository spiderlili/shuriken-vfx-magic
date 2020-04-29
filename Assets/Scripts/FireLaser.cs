using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLaser : MonoBehaviour
{
    public LineRenderer laserLine;
    public Transform laserPoint;
    public Light laserLight;
    public float fadeSpeed = 5;
    public float laserLength = 1000;

    Color laserColor;
    float lightIntensity;

    void Start()
    {
        laserColor = laserLine.material.GetColor("_TintColor");
        lightIntensity = laserLight.intensity;
        laserLine.SetPosition(1, -Vector3.forward * laserLength); //set end position to match defined laserLength
        laserLine.useWorldSpace = true; //when the ship is moving around and firing the laser, the laser effect should exist on its own in space rather than sticking to ship
        laserLight.intensity = 0;
        laserLine.material.SetColor("_TintColor", Color.black);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        //interpolate tint color towards black to fade it out
        laserLine.material.SetColor("_TintColor", Color.Lerp(laserLine.material.GetColor("_TintColor"), Color.black,Time.deltaTime * fadeSpeed));
        laserLight.intensity = Mathf.Lerp(laserLight.intensity, 0, Time.deltaTime * fadeSpeed);
    }

    void Fire()
    {
        laserLine.material.SetColor("_TintColor", laserColor);
        laserLight.intensity = lightIntensity;

        //update start and end positions for line renderer
        laserLine.SetPosition(0, laserPoint.position);
        laserLine.SetPosition(1, laserPoint.position + laserPoint.TransformDirection(-Vector3.forward) * laserLength);
    }
}
