using UnityEngine.Networking;

namespace NonsensicalKit
{
    public interface IHandleWebError
    {
        public void OnProtocolError(UnityWebRequest unityWebRequest);
        public void OnConnectionError(UnityWebRequest unityWebRequest);
        public void OnUnknowError(UnityWebRequest unityWebRequest);
        public void OnDataProcessingError(UnityWebRequest unityWebRequest);
    }
}

