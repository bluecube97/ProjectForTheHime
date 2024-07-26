using UnityEngine;

namespace Script.ApiLibrary
{
    public class WebRequestManager : MonoBehaviour
    {
        public string GetAbsoluteUrl(string endpoint)
        {
            string baseUrl = Application.absoluteURL;
            if (string.IsNullOrEmpty(baseUrl))
            {
                Debug.LogError("Application.absoluteURL is empty. Make sure you are running this in a WebGL build.");
                return null;
            }

            // Extract the domain from the base URL
            System.Uri uri = new(baseUrl);
            string domain = uri.GetLeftPart(System.UriPartial.Authority);

            // Combine the domain with the endpoint
            return $"{domain}/{endpoint}";
        }
    }
}