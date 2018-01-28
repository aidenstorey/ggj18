using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Twity.DataModels.Core;

public class tweet : MonoBehaviour
{
    public int line_length = 50;
    public float line_height = 1.5f;
    public Font the_font;

    public GameObject textObject;

    public float create_tweet(Tweet tweet)
    {
        foreach (var tm in this.GetComponentsInChildren<TextMesh>())
        {
            DestroyImmediate(tm.gameObject);
        }

        List<string> lines = new List<string>();

        var text = tweet.text;
        while(text.Length > this.line_length)
        {
            var a = text.Substring(0, this.line_length);
            var b = text.Substring(this.line_length);

            if (!a.EndsWith(" ") && !b.StartsWith(" "))
            {
                var a_split = a.Split(' ');
                b = a.Last() + b;
                a = string.Join(" ", a_split.ToList().GetRange(0, a_split.Length - 1).ToArray());
            }
            else if (a.EndsWith(" "))
            {
                a = a.TrimEnd(' ');
            }
            else if (b.StartsWith(" "))
            {
                b = b.TrimStart(' ');
            }

            lines.Add(a);
            text = b;
        }

        float y_position = 0.0f;
        for (int i = 0; i < lines.Count; i++)
        {
            TextMesh text_mesh;
            var go = Instantiate(this.textObject, Vector3.zero, Quaternion.identity);

            text_mesh = go.GetComponent<TextMesh>();
            // text_mesh.font = this.the_font;
            text_mesh.fontSize = 14;
            text_mesh.characterSize = 0.2f;

            var mr = go.GetComponent<MeshRenderer>();
            mr.sortingOrder = 1;

            go.transform.SetParent(this.transform);
            go.transform.localPosition = new Vector3(0.0f, y_position);

            text_mesh.text = lines[i];
            y_position -= this.line_height;
        }

        var stats = this.GetComponentInChildren<stats>();
        stats.transform.localPosition = new Vector3(0.0f, y_position - 0.4f, 0.0f);

        return lines.Count * this.line_height + 0.4f;
    }
}
