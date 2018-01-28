using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour
{
    private Camera camera_main;

    public bool is_taunting = false;

    public static fly instance;
    public Sprite[] ded_fly_sprites;

    public float spawn_speed = 50.0f;
    public bool spawning = true;

    void Start()
    {
        if (fly.instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        fly.instance = this;

        this.camera_main = Camera.main;
        this.transform.position = Random.insideUnitCircle.normalized * 30.0f;
    }

    void Update()
    {
        var mousePosition = this.camera_main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;

        if (spawning)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, mousePosition, this.spawn_speed * Time.deltaTime);
            if ((this.transform.position - mousePosition).magnitude < 0.01f)
            {
                this.spawning = false;
            }
        }
        else
        {
            this.is_taunting = this.transform.position == mousePosition;

            this.transform.position = mousePosition;
        }
    }

    public void kill()
    {
        var go = new GameObject("ded_fly");
        go.transform.position = this.transform.position;
        go.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = this.ded_fly_sprites[Random.Range(0, ded_fly_sprites.Length)];
        sr.sortingOrder = 50;

        this.transform.position = Random.insideUnitCircle.normalized * 30.0f;
        this.spawning = true;
    }
}
