using NonsensicalKit;
using UnityEngine;
using UnityEngine.Networking;

namespace NonsensicalKit
{
    public class TestWebErrorHandle : IHandleWebError
    {
        public void OnProtocolError(UnityWebRequest unityWebRequest)
        {
            Debug.Log(unityWebRequest.downloadHandler.error);
        }

        public void OnConnectionError(UnityWebRequest unityWebRequest)
        {
            Debug.Log(unityWebRequest.downloadHandler.error);
        }

        public void OnDataProcessingError(UnityWebRequest unityWebRequest)
        {
            Debug.Log(unityWebRequest.downloadHandler.error);
        }

        public void OnUnknowError(UnityWebRequest unityWebRequest)
        {
            Debug.Log("连接出现未知错误");
        }
    }

}