using UnityEngine;

public class RotateEndlessly : MonoBehaviour
{
    [SerializeField] private float rotateYAmount = 2;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, rotateYAmount, 0);
    }
}
