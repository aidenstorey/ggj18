using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour
{
    private Camera camera_main;

    public bool is_taunting = false;

    public static fly instance;

    void Start()
    {
        if (fly.instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        fly.instance = this;

        this.camera_main = Camera.main;
    }

    void Update()
    {
        var mousePosition = this.camera_main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;

        this.is_taunting = this.transform.position == mousePosition;

        this.transform.position = mousePosition;
    }
}
