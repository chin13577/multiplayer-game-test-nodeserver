using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class RestfulUtility
{
    /// <summary>
    /// Function PUT in REST API.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data">raw data</param>
    /// <param name="callback">return callback from backend.</param>
    /// <returns></returns>
    public static IEnumerator Put(string url, byte[] data, Action<DownloadHandler> callback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(url, data))
        {
            yield return request.Send();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.responseCode);
                if (callback != null)
                    callback(request.downloadHandler);
            }
        }
    }
    /// <summary>
    /// Function "PUT" in REST API.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data">raw data</param>
    /// <param name="callback">return callback from backend.</param>
    /// <returns></returns>
    public static IEnumerator Put(string url, string data, Action<DownloadHandler> callback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(url, data))
        {
            yield return request.Send();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.responseCode);
                if (callback != null)
                    callback(request.downloadHandler);
            }
        }
    }
    /// <summary>
    /// Function GET in REST API.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback">return callback from backend.</param>
    /// <returns></returns>
    public static IEnumerator Get(string url, Action<DownloadHandler> callback = null)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.responseCode);
            if (callback != null)
                callback(request.downloadHandler);
        }
    }
    /// <summary>
    /// Function POST in REST API.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="form">Unity WWWForm</param>
    /// <param name="callback">return callback from backend.</param>
    /// <returns></returns>
    public static IEnumerator Post(string url, WWWForm form, Action<DownloadHandler> callback = null)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.Send();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.responseCode);
            if (callback != null)
                callback(request.downloadHandler);
        }
    }
    /// <summary>
    /// return Texture from url.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback">return Unity Texture.</param>
    /// <returns></returns>
    public static IEnumerator GetImage(string url, Action<Texture> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.Send();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.responseCode);
            if (callback != null)
                callback(((DownloadHandlerTexture)request.downloadHandler).texture);
        }
    }
}
