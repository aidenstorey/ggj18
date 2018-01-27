using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public SpriteRenderer sprite_renderer;

    void Start()
    {
        this.sprite_renderer = this.GetComponent<SpriteRenderer>();
    }

    public void set_down(bool down)
    {
        var value = down ? 0.5f : 1.0f;
        this.sprite_renderer.color = new Color(value, value, value);

        if (down)
        {
            messaging.instance.handle_key_press(this.gameObject.name);
        }
    }
}
