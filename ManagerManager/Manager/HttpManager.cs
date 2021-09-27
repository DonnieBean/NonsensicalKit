using NonsensicalKit.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NonsensicalKit.Manager
{
    public class HttpManager : NonsensicalManagerBase<HttpManager>
    {
        public IHandleWebError HttpErrorProcess { get; set; }

        public Dictionary<string, string> Header { get; set; }

        public void Get(string url, Action<UnityWebRequest> callback)
        {
            StartCoroutine(HttpHelper.Get(url, Header, callback, HttpErrorProcess));
        }

        public void Post(string url, Dictionary<string, string> formData, Action<UnityWebRequest> callback)
        {
            StartCoroutine(HttpHelper.Post(url, formData, Header, callback, HttpErrorProcess));
        }

        public void Post(string url, List<IMultipartFormSection> formData, Action<UnityWebRequest> callback)
        {
            StartCoroutine(HttpHelper.Post(url, formData, Header, callback, HttpErrorProcess));
        }

        public void LoadAssetbundle(string url, uint version, uint crc, Action<AssetBundle> callback)
        {
            StartCoroutine(HttpHelper.LoadAssetbundle(url, version, crc,
                (unityWebRequest) =>
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);

                    callback(bundle);

                }, HttpErrorProcess));
        }

        public void LoadAssetbundle(string url, uint version, Action<AssetBundle> callback)
        {
            StartCoroutine(HttpHelper.LoadAssetbundle(url, version, 0,
                (unityWebRequest) =>
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);

                    callback(bundle);

                }, HttpErrorProcess));
        }

        public void UploadPng(string url, byte[] imageByte, Action<UnityWebRequest> callback)
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

            formData.Add(new MultipartFormFileSection("preview", imageByte, "preview", "image/png"));

            StartCoroutine(HttpHelper.Post(url, formData, Header, callback, HttpErrorProcess));
        }

        public void UploadPngWithDatas(string url, byte[] imageByte, Dictionary<string, string> datas, Action<UnityWebRequest> callback)
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

            formData.Add(new MultipartFormFileSection("preview", imageByte, "preview", "image/png"));

            foreach (var item in datas)
            {
                formData.Add(new MultipartFormDataSection(item.Key, item.Value));
            }

            StartCoroutine(HttpHelper.Post(url, formData, Header, callback, HttpErrorProcess));
        }

        protected override void InitStart()
        {
            InitComplete();
        }

        protected override void LateInitStart()
        {
            LateInitComplete();
        }
    }

}

