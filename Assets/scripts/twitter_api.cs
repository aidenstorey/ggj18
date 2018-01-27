using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Code based off of https://blog.dantup.com/2016/07/simplest-csharp-code-to-post-a-tweet-using-oauth/
/// Changed to work with Unity
///
/// Simple class for sending tweets to Twitter using Single-user OAuth.
/// https://dev.twitter.com/oauth/overview/single-user
///
/// Get your access keys by creating an app at apps.twitter.com then visiting the
/// "Keys and Access Tokens" section for your app. They can be found under the
/// "Your Access Token" heading.
/// </summary>
class TwitterApi
{
    const string TwitterApiBaseUrl = "https://api.twitter.com/1.1/";
    readonly string consumerKey, consumerSecret, accessToken, accessSecret;
    readonly HMACSHA1 sigHasher;
    readonly DateTime epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Creates an object for sending tweets to Twitter using Single-user OAuth.
    ///
    /// Get your access keys by creating an app at apps.twitter.com then visiting the
    /// "Keys and Access Tokens" section for your app. They can be found under the
    /// "Your Access Token" heading.
    /// </summary>
    public TwitterApi(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
    {
        this.consumerKey = consumerKey;
        this.consumerSecret = consumerSecret;
        this.accessToken = accessToken;
        this.accessSecret = accessSecret;

        this.sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes(string.Format("{0}&{1}", this.consumerSecret, this.accessSecret)));
    }

    public IEnumerator Tweet(string text)
    {
        var data = new Dictionary<string, string> {
            { "status", text },
            { "trim_user", "1" }
        };

        return SendRequest("statuses/update.json", data);
    }

    IEnumerator SendRequest(string url, Dictionary<string, string> data)
    {
        var fullUrl = TwitterApiBaseUrl + url;

        // Timestamps are in seconds since 1/1/1970.
        var timestamp = (int)((DateTime.UtcNow - this.epochUtc).TotalSeconds);

        // Add all the OAuth headers we'll need to use when constructing the hash.
        data.Add("oauth_consumer_key", this.consumerKey);
        data.Add("oauth_signature_method", "HMAC-SHA1");
        data.Add("oauth_timestamp", timestamp.ToString());
        data.Add("oauth_nonce", "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg"); // Required, but Twitter doesn't appear to use it, so "a" will do.
        data.Add("oauth_token", this.accessToken);
        data.Add("oauth_version", "1.0");

        // Generate the OAuth signature and add it to our payload.
        data.Add("oauth_signature", GenerateSignature(fullUrl, data));

        // Build the OAuth HTTP Header from the data.
        string oAuthHeader = GenerateOAuthHeader(data);
        messaging.instance.debug = oAuthHeader.ToString();
        Debug.Log(oAuthHeader);

        // Build the form data (exclude OAuth stuff that's already in the header).
        // var formData = new FormUrlEncodedContent(data.Where(kvp => !kvp.Key.StartsWith("oauth_")));
        var formData = data.Where(kvp => !kvp.Key.StartsWith("oauth_")).ToDictionary(s => s.Key, s => s.Value);

        return SendRequest(fullUrl, oAuthHeader, formData);
    }

    string GenerateSignature(string url, Dictionary<string, string> data)
    {
        var sigString = string.Join(
            "&",
            data
                .Union(data)
                .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                .OrderBy(s => s)
                .ToArray()
        );

        var fullSigData = string.Format(
            "{0}&{1}&{2}",
            "POST",
            Uri.EscapeDataString(url),
            Uri.EscapeDataString(sigString.ToString())
        );

        return Convert.ToBase64String(this.sigHasher.ComputeHash(new ASCIIEncoding().GetBytes(fullSigData.ToString())));
    }

    string GenerateOAuthHeader(Dictionary<string, string> data)
    {
        return "OAuth " + string.Join(
            ", ",
            data
                .Where(kvp => kvp.Key.StartsWith("oauth_"))
                .Select(kvp => string.Format("{0}=\"{1}\"", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                // .OrderBy(s => s)
                .ToArray()
        );
    }

    IEnumerator SendRequest(string fullUrl, string oAuthHeader, Dictionary<string, string> formData)
    {
        var form = new WWWForm();
        foreach (var kvp in formData)
        {
            form.AddField(kvp.Key, kvp.Value);
        }

        var headers = form.headers;
        headers["Authorization"] = oAuthHeader;

        using (var www = new WWW(fullUrl, form.data, headers))
        {
            yield return www;

            Debug.Log(www.text);
        }
        // var form = new List<IMultipartFormSection>();
        // form.Add(
        //     new MultipartFormDataSection(
        //         string.Join(
        //             "&",
        //             formData.Select(
        //                 kvp => string.Format(
        //                     "{0}={1}",
        //                     Uri.EscapeDataString(kvp.Key),
        //                     Uri.EscapeDataString(kvp.Value)
        //                 )
        //             ).ToArray()
        //         )
        //     )
        // );

        // var www = UnityWebRequest.Post(fullUrl, formData);
        // www.SetRequestHeader("Authorization", oAuthHeader);

        // yield return www.SendWebRequest();

        // Debug.Log(www.downloadHandler.text);
    }
}