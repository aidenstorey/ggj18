using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene_track : MonoBehaviour
{
    public float margin_top = 0.2f;
    public float margin_bottom = 0.3f;

    public float position_top = -6.0f;
    public float position_bottom = +6.0f;

    public float camera_speed = 5.0f;

    private fly fly_instance;

    private Camera camera_main;
    private float camera_height;
    private float camera_margin_top;
    private float camera_margin_bottom;

    public string debug;

    void Start()
    {
        this.fly_instance = GameObject.FindObjectOfType<fly>();

        this.camera_main = Camera.main;
        this.camera_height = this.camera_main.pixelHeight;

        this.camera_margin_bottom = this.camera_height * this.margin_bottom;
        this.camera_margin_top = this.camera_height - (this.camera_height * this.margin_top);
    }

    void Update()
    {
        this.debug = this.camera_margin_top.ToString();
        var fly_position_screen = Input.mousePosition;

        // Move screen down
        if (fly_position_screen.y <= this.camera_margin_bottom)
        {
            if ( this.transform.position.y != this.position_bottom)
            {
                var position = this.transform.position;
                position.y = Mathf.Min(this.position_bottom, position.y + this.camera_speed * Time.deltaTime);

                this.transform.position = position;
            }
        }

        // Move screen up
        if (fly_position_screen.y >= this.camera_margin_top)
        {
            if ( this.transform.position.y != this.position_top)
            {
                var position = this.transform.position;
                position.y = Mathf.Max(this.position_top, position.y - this.camera_speed * Time.deltaTime);

                this.transform.position = position;
            }
        }
    }
}
