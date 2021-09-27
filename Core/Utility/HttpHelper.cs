using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace NonsensicalKit.Utility
{
    public static class HttpHelper
    {
        public static IEnumerator LoadAssetbundle(string uri, uint version, uint crc, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri, version, crc);
            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }
        public static IEnumerator Post(string url, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, new WWWForm());

            IncreaseHeader(unityWebRequest, header);
            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }
        public static IEnumerator Post(string url, List<IMultipartFormSection> formData, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError=null)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, formData);

            IncreaseHeader(unityWebRequest, header);
            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }

        public static IEnumerator UploadFiles(string url, List<string> names, List<string> urls, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            for (int i = 0; i < urls.Count; i++)
            {
                UnityWebRequest crtFileRequest = UnityWebRequest.Get(urls[i]);

                yield return crtFileRequest.SendWebRequest();

                formData.Add(new MultipartFormFileSection(Path.GetFileName(names[i]), crtFileRequest.downloadHandler.data));
            }

            yield return Post(url, formData, header, callback, iHandleWebError);
        }

        public static IEnumerator UploadFiles(string url, List<string> fileFullNames, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {

            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

            foreach (var item in fileFullNames)
            {
                using (FileStream fs = new FileStream(item, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        byte[] buffur = new byte[fs.Length];
                        fs.Read(buffur, 0, (int)fs.Length);
                        formData.Add(new MultipartFormFileSection(Path.GetFileName(item), buffur));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            yield return Post(url, formData, header, callback, iHandleWebError);
        }

        public static IEnumerator Post(string url, Dictionary<string, string> formData, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, CreateForm(formData));

            IncreaseHeader(unityWebRequest, header);

            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }
        public static IEnumerator Get(string url, Action<UnityWebRequest> callback)
        {
            UnityWebRequest unityWebRequest = new UnityWebRequest(url)
            {
                timeout = 300
            };
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

             yield return SendRequest(unityWebRequest, callback);
        }

        public static IEnumerator Get(string url, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            UnityWebRequest unityWebRequest = new UnityWebRequest(url)
            {
                timeout = 300
            };
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            IncreaseHeader(unityWebRequest, header);
            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }

        public static IEnumerator GetTexture(string url, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            UnityWebRequest unityWebRequest = new UnityWebRequest(url)
            {
                timeout = 300
            };
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            unityWebRequest.downloadHandler = downloadTexture;

            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }

        public static IEnumerator PostTexture(string url, Dictionary<string, string> formData, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, CreateForm(formData));
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            unityWebRequest.downloadHandler = downloadTexture;

            IncreaseHeader(unityWebRequest, header);

            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }

        public static IEnumerator UploadBinaryFile(string url, string fieldName, byte[] fileByte, string fileName, string contentType, Dictionary<string, string> header, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData(fieldName, fileByte, fileName, contentType);

            UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, form);
            unityWebRequest.useHttpContinue = false;
            IncreaseHeader(unityWebRequest, header);

            yield return SendRequest(unityWebRequest, callback, iHandleWebError);
        }

        public static IEnumerator SendRequest(UnityWebRequest unityWebRequest, Action<UnityWebRequest> callback, IHandleWebError iHandleWebError = null)
        {
            yield return unityWebRequest.SendWebRequest();

            if (iHandleWebError == null)
            {
                callback?.Invoke(unityWebRequest);
            }
            else
            {
                switch (unityWebRequest.result)
                {
                    case UnityWebRequest.Result.Success:
                    case UnityWebRequest.Result.ProtocolError:
                        callback?.Invoke(unityWebRequest);
                        break;
                    case UnityWebRequest.Result.ConnectionError:
                        iHandleWebError?.OnConnectionError(unityWebRequest);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        iHandleWebError?.OnDataProcessingError(unityWebRequest);
                        break;
                    default:
                        iHandleWebError?.OnUnknowError(unityWebRequest);
                        break;
                }
            }
            unityWebRequest.Dispose();
        }

        #region PrivateMethod
        private static WWWForm CreateForm(Dictionary<string, string> formData)
        {
            WWWForm form = new WWWForm();
            if (formData != null)
            {
                foreach (var item in formData)
                {
                    form.AddField(item.Key, item.Value);
                }
            }
            return form;
        }

        private static void IncreaseHeader(UnityWebRequest unityWebRequest, Dictionary<string, string> headerData)
        {
            if (headerData != null)
            {
                foreach (var tmp in headerData)
                {
                    unityWebRequest.SetRequestHeader(tmp.Key, tmp.Value);
                }
                DictionaryPool<string, string>.Set(headerData);
            }
        }

        #endregion
    }
}
