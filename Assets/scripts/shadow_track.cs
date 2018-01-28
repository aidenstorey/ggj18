using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadow_track : MonoBehaviour
{
    public float speed_track = 10.0f;
    public float speed_taunted = 20.0f;
    public float speed_return = 20.0f;

    public Vector3 position_initial;

    public string debug;

    public float attacking_distance = 0.2f;
    public float attacking_time = 0.8f;
    public float attacking_cooldown_time = 1.5f;
    public float escape_time = 0.2f;

    public float attacking_radius_min = 0.2f;
    public float attacking_radius_max = 1.5f;
    public float attacking_radius = 0.0f;

    float attacking = 0.0f;
    float attacking_cooldown = 0.0f;

    public float scale_speed = 50.0f;
    public Vector3 scale_normal = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 scale_down = new Vector3(0.8f, 0.8f, 1.0f);

    Vector3 scale_target;


    void Start()
    {
        this.position_initial = this.transform.position;
        this.scale_target = scale_normal;
    }

    void Update()
    {
        var fp = fly.instance.transform.position;
        var tl = keyboard.instance.top_left;
        var br = keyboard.instance.bottom_right;

        this.debug = this.attacking.ToString();

        if (this.attacking == 0.0f)
        {
            var mouseX = Input.mousePosition.x;
            var mouseY = Input.mousePosition.y;

            if (mouseX > 0 && mouseX < Screen.width && mouseY > 0 && mouseY < Screen.height && !fly.instance.spawning)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, fp, (fly.instance.is_taunting ? this.speed_taunted : this.speed_track) * Time.deltaTime);

                if  (mouseY < Screen.height / 2.8f)
                {
                    if (this.attacking_cooldown == 0.0f && (fp - this.transform.position).magnitude <= this.attacking_distance)
                    {
                        this.attacking = this.attacking_time;
                        this.attacking_cooldown = this.attacking_cooldown_time;

                        this.attacking_radius = Random.Range(this.attacking_radius_min, this.attacking_radius_max);
                    }
                }
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.position_initial, this.speed_return * Time.deltaTime * 1.5f);
            }
        }

        if (this.attacking != 0.0f)
        {
            var previous_time = this.attacking;
            this.attacking = Mathf.Max(0.0f, this.attacking - Time.deltaTime);

            if (previous_time != this.attacking && previous_time > this.escape_time && this.attacking < this.escape_time)
            {
                this.scale_target = this.scale_down;

                var overlapping = Physics.OverlapSphere(this.transform.position, this.attacking_radius);
                foreach (var key in overlapping)
                {
                    if (key.name == "fly")
                    {
                        fly.instance.kill();
                    }
                    else
                    {
                        key.GetComponent<key>().set_down(true);
                    }
                }
            }

            if (this.attacking == 0.0f)
            {
                this.scale_target = this.scale_normal;

                var overlapping = Physics.OverlapSphere(this.transform.position, this.attacking_radius);
                foreach (var key in overlapping)
                {
                    if (key.name != "fly")
                    {
                        key.GetComponent<key>().set_down(false);
                    }
                }
            }
        }

        var speed = this.scale_target == this.scale_normal ? this.scale_speed / 5.0f : this.scale_speed;

        this.transform.localScale = Vector3.MoveTowards(this.transform.localScale, this.scale_target, speed * Time.deltaTime);

        if (this.attacking_cooldown != 0.0f)
        {
            this.attacking_cooldown = Mathf.Max(0.0f, this.attacking_cooldown - Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
    }
}
