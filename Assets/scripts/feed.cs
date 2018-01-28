using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twity.DataModels.Responses;

public class feed : MonoBehaviour
{
    public static feed instance = null;
    public float tweet_spacing = 0.5f;

    private tweet[] tweets;

    void Start()
    {
        if (feed.instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        feed.instance = this;

        this.refresh_feed();
    }

    public void refresh_feed()
    {
        Twity.Oauth.consumerKey = "RaizrypDxXRkW5tN9AbXVt8fM";
        Twity.Oauth.consumerSecret = "eXIjyN8jkNryK48UYjhpvwSQ0kSEkBO3KOMDnHLguxUKbXhoPY";
        Twity.Oauth.accessToken = "956804503180726277-bGmXyW0gDVCyK5Y7BRv72fHHoGFJtZK";
        Twity.Oauth.accessTokenSecret = "q2tQuMPVB5dorcHJebf4JbJ5Xep1kYcCmUpKrZh73qE5P";

        this.tweets = this.GetComponentsInChildren<tweet>();

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters ["count"] = this.tweets.Length.ToString();
        StartCoroutine (Twity.Client.Get ("statuses/home_timeline", parameters, callback));
    }

    void callback(bool success, string response) {
        if (success) {
            StatusesHomeTimelineResponse Response = JsonUtility.FromJson<StatusesHomeTimelineResponse> (response);
            float y_offset = 0.0f;
            for (int i = 0; i < Response.items.Length; i++)
            {
                var lp = this.tweets[i].transform.localPosition;
                lp.y = y_offset;

                this.tweets[i].transform.localPosition = lp;

                y_offset -= this.tweets[i].create_tweet(Response.items[i]) + this.tweet_spacing;
            }
        }
    }
}