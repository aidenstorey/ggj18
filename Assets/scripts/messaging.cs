using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twity.DataModels.Core;

public class messaging : MonoBehaviour
{
    public static messaging instance = null;

    public string message = "Hello world. If this works we just tweeted from our #GGJ18 game! #GameDev #IndieDev #WhatHaveWeDone #OhGodWhy";

    private bool caps_on = false;

    public string debug;

    // private TwitterApi twitter_api;

    void Start()
    {
        if (messaging.instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        messaging.instance = this;
    }

    public void send_tweet()
    {
        Twity.Oauth.consumerKey       = "RaizrypDxXRkW5tN9AbXVt8fM";
        Twity.Oauth.consumerSecret    = "eXIjyN8jkNryK48UYjhpvwSQ0kSEkBO3KOMDnHLguxUKbXhoPY";
        Twity.Oauth.accessToken       = "956804503180726277-bGmXyW0gDVCyK5Y7BRv72fHHoGFJtZK";
        Twity.Oauth.accessTokenSecret = "q2tQuMPVB5dorcHJebf4JbJ5Xep1kYcCmUpKrZh73qE5P";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["status"] = this.message;

        StartCoroutine (Twity.Client.Post ("statuses/update", parameters, Callback));
    }

    void Callback(bool success, string response) {
        if (success) {
            Tweet tweet = JsonUtility.FromJson<Tweet> (response);
        } else {
            Debug.Log (response);
        }
    }

    public void handle_key_press(string key)
    {
        if (key.Length == 1)
        {
            this.message += this.caps_on ? key.ToUpper() : key;
        }

        if (key == "delete")
        {
            this.message = this.message.Substring(0, this.message.Length - 1);
        }

        if (key == "space")
        {
            this.message += " ";
        }

        if (key == "caps lock")
        {
            this.caps_on = !this.caps_on;
        }
    }
}
