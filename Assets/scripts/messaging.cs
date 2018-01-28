using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twity.DataModels.Core;

public class messaging : MonoBehaviour
{
    public static messaging instance = null;

    private bool caps_on = false;

    public bool will_send_tweet = true;
    public float max_character = 30;

    public string debug;

    private TextMesh text_mesh;
    public string extra;

    // private TwitterApi twitter_api;

    void Start()
    {
        Screen.SetResolution(1024, 768, false);
        if (messaging.instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        messaging.instance = this;

        this.text_mesh = this.GetComponent<TextMesh>();
        this.text_mesh.fontSize = 14;
    }

    public void send_tweet()
    {
        Twity.Oauth.consumerKey       = "RaizrypDxXRkW5tN9AbXVt8fM";
        Twity.Oauth.consumerSecret    = "eXIjyN8jkNryK48UYjhpvwSQ0kSEkBO3KOMDnHLguxUKbXhoPY";
        Twity.Oauth.accessToken       = "956804503180726277-bGmXyW0gDVCyK5Y7BRv72fHHoGFJtZK";
        Twity.Oauth.accessTokenSecret = "q2tQuMPVB5dorcHJebf4JbJ5Xep1kYcCmUpKrZh73qE5P";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["status"] = this.text_mesh.text + this.extra;

        if (this.will_send_tweet) {
            StartCoroutine (Twity.Client.Post ("statuses/update", parameters, Callback));
        }
        this.text_mesh.text= "";
    }

    void Callback(bool success, string response) {}

    public void handle_key_press(string key)
    {
        if (key.Length == 1 && this.text_mesh.text.Length < this.max_character)
        {
            this.text_mesh.text += this.caps_on ? key.ToUpper() : key;
        }

        if (key == "del")
        {
            this.text_mesh.text = this.text_mesh.text.Substring(0, this.text_mesh.text.Length - 1);
        }

        if (key == "" && this.text_mesh.text.Length < this.max_character)
        {
            this.text_mesh.text += " ";
        }

        if (key == "caps")
        {
            this.caps_on = !this.caps_on;
        }

        if (key == "enter")
        {
            this.send_tweet();
        }

        if (key == "power lol")
        {
            Debug.Log("Lol");
            Application.Quit();
        }

        this.text_mesh.text = this.text_mesh.text;
    }
}
