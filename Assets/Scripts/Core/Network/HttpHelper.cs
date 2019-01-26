using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    public static class HttpHelper
    {
#region GET
        public static IEnumerator GetText(string url, Action<string> success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke(www.downloadHandler.text);
            }
        }

        public static IEnumerator GetBytes(string url, Action<byte[]> success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke(www.downloadHandler.data);
            }
        }

        public static IEnumerator GetAssetBundle(string url, Action<AssetBundle> success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke(((DownloadHandlerAssetBundle)www.downloadHandler).assetBundle);
            }
        }

        public static IEnumerator GetTexture(string url, Action<Texture> success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke(((DownloadHandlerTexture)www.downloadHandler).texture);
            }
        }

        public static IEnumerator GetAudioClip(string url, Action<AudioClip> success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke(((DownloadHandlerAudioClip)www.downloadHandler).audioClip);
            }
        }
#endregion

#region POST
        public static IEnumerator Post(string url, List<IMultipartFormSection> formData, Action success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, formData);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke();
            }
        }
#endregion

#region PUT
        public static IEnumerator Put(string url, byte[] data, Action success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Put(url, data);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke();
            }
        }
#endregion

#region DELETE
        public static IEnumerator Delete(string url, Action success, Action<string> failure)
        {
            UnityWebRequest www = UnityWebRequest.Delete(url);
            yield return www.SendWebRequest();
     
            if(www.isNetworkError || www.isHttpError) {
                failure?.Invoke(www.error);
            } else {
                success?.Invoke();
            }
        }
#endregion
    }
}
