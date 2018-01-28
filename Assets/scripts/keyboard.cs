using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;

public class keyboard : MonoBehaviour
{
    public TextAsset keyboard_layout;

    public GameObject key_prefab;

    public float width;
    public float height;
    public float spacing;

    public float row_unit_count = 14.5f;

    public static keyboard instance = null;

    public Vector2 top_left;
    public Vector2 bottom_right;

    public Sprite[] sprites;

    void Start()
    {
        if (keyboard.instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        keyboard.instance = this;

        this.create_keyboard();
    }

    public void create_keyboard()
    {
        return; // Don't let the keyboard get fucked up.

        foreach (var k in this.GetComponentsInChildren<key>())
        {
            DestroyImmediate(k.gameObject);
        }

        float x_start = -(this.width / 2.0f);
        float y_start = -(this.height / 2.0f);

        var rows = Regex.Split(this.keyboard_layout.text, "\n|\r|\r\n");
        var row_y_size = (height - (spacing * (rows.Length - 1))) / rows.Length;

        float this_x = this.transform.position.x;
        float this_y = this.transform.position.y;

        float y_offset = y_start; float x_offset = x_start;
        for (int r = 0; r < rows.Length; r++)
        {
            var row = rows[r];
            var keys = row.Split('&');

            var row_x_size = (width - (spacing * (keys.Length - 1))) / this.row_unit_count;

            y_offset -= row_y_size / 2.0f;

            float[] order = {1.0f, 1.25f, 1.5f, 1.75f, 2.25f, 5.0f};

            x_offset = x_start;
            for (int k = 0; k < keys.Length; k++)
            {
                var key = keys[k];

                var split = key.Split('*');
                var name = split[0];
                var units = float.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);

                var key_x_size = row_x_size * units;

                x_offset += key_x_size / 2.0f;

                var go = Instantiate(this.key_prefab, Vector3.zero, Quaternion.identity);

                var sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = this.sprites[order.ToList().IndexOf(units)];

                go.transform.SetParent(this.transform);
                go.transform.localPosition = new Vector3(x_offset, y_offset, 0.0f);

                var text = go.GetComponentInChildren<TextMesh>();
                text.text = name;
                text.fontSize = 70;

                if (name.Length > 1)
                {
                    text.characterSize /= 2.0f;
                }

                var renderer = go.GetComponentInChildren<MeshRenderer>();
                renderer.sortingOrder = 1000;

                go.name = name;

                x_offset += key_x_size / 2.0f + this.spacing;
            }

            y_offset -= row_y_size / 2.0f + this.spacing;
        }

        this.top_left = new Vector3(x_start, y_start) + this.transform.position;
        this.bottom_right = new Vector3(x_offset, y_offset) + this.transform.position;
    }
}
