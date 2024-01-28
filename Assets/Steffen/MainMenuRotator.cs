using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotator : MonoBehaviour
{
    public float radius = 30.0f;

    public float height = 25.0f;

    public float speed = 1.0f;

    public Vector3 lookAt = Vector3.zero;

    private float progress = 0.0f;

    void Update()
    {
        float x = Mathf.Sin(progress) * radius;
        float z = Mathf.Cos(progress) * radius;

        progress += Time.deltaTime * speed;

        Camera.main.transform.position = new Vector3(x, height, z);

        Camera.main.transform.LookAt(lookAt);
    }
}
