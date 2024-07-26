using UnityEngine;

namespace Script.ApiLibrary
{
    public class WebRequestManager : MonoBehaviour
    {
        public static string GetAbsoluteUrl(string endpoint)
        {
            string baseUrl = Application.absoluteURL;
            Debug.Log("baseUrl: " + baseUrl);
            if (string.IsNullOrEmpty(baseUrl))
            {
                Debug.LogError("Application.absoluteURL is empty. Make sure you are running this in a WebGL build.");
                return null;
            }

            // Extract the domain from the base URL
            System.Uri uri = new(baseUrl);
            string domain = uri.GetLeftPart(System.UriPartial.Authority);

            Debug.Log("domain: " + domain);

            // Combine the domain with the endpoint
            return $"{domain}/{endpoint}";
        }
    }
}