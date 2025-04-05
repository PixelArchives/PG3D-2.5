using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class Facebook : P31RestKit
{
	public string accessToken;

	public string appAccessToken;

	private static Facebook _instance;

	public static Facebook instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new Facebook();
			}
			return _instance;
		}
	}

	public Facebook()
	{
		_baseUrl = "https://graph.facebook.com/";
		forceJsonResponse = true;
	}

	protected override IEnumerator send(string path, HTTPVerb httpVerb, Dictionary<string, object> parameters, Action<string, object> onComplete)
	{
		if (parameters == null)
		{
			parameters = new Dictionary<string, object>();
		}
		if (!parameters.ContainsKey("access_token"))
		{
			parameters.Add("access_token", accessToken);
		}
		return base.send(path, httpVerb, parameters, onComplete);
	}

	public void prepareForMetroUse(GameObject go, MonoBehaviour mb)
	{
		UnityEngine.Object.DontDestroyOnLoad(go);
		surrogateGameObject = go;
		base.surrogateMonobehaviour = mb;
	}

	public void graphRequest(string path, Action<string, object> completionHandler)
	{
		get(path, null, completionHandler);
	}

	public void graphRequest(string path, HTTPVerb verb, Action<string, object> completionHandler)
	{
		graphRequest(path, verb, null, completionHandler);
	}

	public void graphRequest(string path, HTTPVerb verb, Dictionary<string, object> parameters, Action<string, object> completionHandler)
	{
		base.surrogateMonobehaviour.StartCoroutine(send(path, verb, parameters, completionHandler));
	}

	public void graphRequestBatch(IEnumerable<FacebookBatchRequest> requests, Action<string, object> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (FacebookBatchRequest request in requests)
		{
			list.Add(request.requestDictionary());
		}
		dictionary.Add("batch", Json.jsonEncode(list));
		base.surrogateMonobehaviour.StartCoroutine(send(string.Empty, HTTPVerb.POST, dictionary, completionHandler));
	}

	public void restRequest(string method, HTTPVerb verb, Dictionary<string, object> parameters, Action<string, object> completionHandler)
	{
		if (parameters == null)
		{
			parameters = new Dictionary<string, object>();
		}
		if (!parameters.ContainsKey("format"))
		{
			parameters.Add("format", "json");
		}
		string path = "method/" + method;
		string baseUrl = _baseUrl;
		_baseUrl = "https://api.facebook.com/";
		base.surrogateMonobehaviour.StartCoroutine(send(path, verb, parameters, completionHandler));
		_baseUrl = baseUrl;
	}

	public void postMessage(string message, Action<string, object> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("message", message);
		Dictionary<string, object> parameters = dictionary;
		post("me/feed", parameters, completionHandler);
	}

	public void postMessageWithLink(string message, string link, string linkName, Action<string, object> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("message", message);
		dictionary.Add("link", link);
		dictionary.Add("name", linkName);
		Dictionary<string, object> parameters = dictionary;
		post("me/feed", parameters, completionHandler);
	}

	public void postMessageWithLinkAndLinkToImage(string message, string link, string linkName, string linkToImage, string caption, Action<string, object> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("message", message);
		dictionary.Add("link", link);
		dictionary.Add("name", linkName);
		dictionary.Add("picture", linkToImage);
		dictionary.Add("caption", caption);
		Dictionary<string, object> parameters = dictionary;
		post("me/feed", parameters, completionHandler);
	}

	public void postImage(byte[] image, string message, Action<string, object> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("picture", image);
		dictionary.Add("message", message);
		Dictionary<string, object> parameters = dictionary;
		post("me/photos", parameters, completionHandler);
	}

	public void postImageToAlbum(byte[] image, string caption, string albumId, Action<string, object> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("picture", image);
		dictionary.Add("message", caption);
		Dictionary<string, object> parameters = dictionary;
		post(albumId, parameters, completionHandler);
	}

	public void getFriends(Action<string, object> completionHandler)
	{
		get("me/friends", completionHandler);
	}

	public void extendAccessToken(string appId, string appSecret, Action<DateTime?> completionHandler)
	{
		if (instance.accessToken == null)
		{
			Debug.LogError("There is no access token to extend. The user must be autenticated before attempting to extend their access token");
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("client_id", appId);
		dictionary.Add("client_secret", appSecret);
		dictionary.Add("grant_type", "fb_exchange_token");
		dictionary.Add("fb_exchange_token", instance.accessToken);
		Dictionary<string, object> parameters = dictionary;
		get("oauth/access_token", parameters, delegate(string error, object obj)
		{
			if (obj is string)
			{
				string text = obj as string;
				if (text.StartsWith("access_token="))
				{
					Dictionary<string, string> dictionary2 = text.parseQueryString();
					instance.accessToken = dictionary2["access_token"];
					double value = double.Parse(dictionary2["expires"]);
					completionHandler(DateTime.Now.AddSeconds(value));
				}
				else
				{
					Debug.LogError("error extending access token: " + text);
					completionHandler(null);
				}
			}
			else
			{
				Debug.LogError("error extending access token: " + error);
				completionHandler(null);
			}
		});
	}

	public void getAppAccessToken(string appId, string appSecret, Action<string> completionHandler)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("client_id", appId);
		dictionary.Add("client_secret", appSecret);
		dictionary.Add("grant_type", "client_credentials");
		Dictionary<string, object> parameters = dictionary;
		get("oauth/access_token", parameters, delegate(string error, object obj)
		{
			if (obj is string)
			{
				string text = obj as string;
				if (text.StartsWith("access_token="))
				{
					appAccessToken = text.Replace("access_token=", string.Empty);
					completionHandler(appAccessToken);
				}
				else
				{
					completionHandler(null);
				}
			}
			else
			{
				completionHandler(null);
			}
		});
	}

	public void postScore(string userId, int score, Action<bool> completionHandler)
	{
		if (appAccessToken == null)
		{
			Debug.Log("you must first retrieve the app access token before posting a score");
			completionHandler(false);
			return;
		}
		if (userId == null)
		{
			Debug.Log("a valid userId is required to post a score");
			completionHandler(false);
			return;
		}
		string path = userId + "/scores";
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("score", score.ToString());
		dictionary.Add("app_access_token", appAccessToken);
		dictionary.Add("access_token", appAccessToken);
		Dictionary<string, object> parameters = dictionary;
		post(path, parameters, delegate(string error, object obj)
		{
			if (error == null && obj is bool)
			{
				bool obj2 = (bool)obj;
				completionHandler(obj2);
			}
			else
			{
				completionHandler(false);
			}
		});
	}

	public void getScores(string userId, Action<string, object> onComplete)
	{
		string path = userId + "/scores";
		get(path, onComplete);
	}
}
