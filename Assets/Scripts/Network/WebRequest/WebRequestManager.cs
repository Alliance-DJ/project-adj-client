using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

using Header = System.Collections.Generic.Dictionary<string, string>;
using Data = System.Collections.Generic.Dictionary<string, string>;
using Query = System.Collections.Generic.Dictionary<string, string>;

public static class NetworkManager
{
    private const string kHttpVerbPatch = "PATCH";

    private static readonly string API_BASE_URL = "localhost:3000";  // host

    public static string AccessToken { get; private set; }

    public static void SetAccessToken(string token)
    {
        AccessToken = Rest.GetBearerOAuthToken(token);
    }

    private static T GetJson<T>(Response req, string group = null)
    {
        var res = req.ResponseBody;

        if (string.IsNullOrWhiteSpace(group) == false)
        {
            var builder = new StringBuilder();
            builder.Append("{\"");
            builder.Append(group);
            builder.Append("\":");
            builder.Append(res);
            builder.Append("}");
            res = builder.ToString();
        }

        var data = JsonConvert.DeserializeObject<T>(res);
        return data;
    }

    private static string MakeURL(string endpoint, Query query = null)
    {
        try
        {
            var url = new StringBuilder($"{API_BASE_URL}{endpoint}");
            Debug.Log($"ENDPOINT - {endpoint} QUERY - {query?.Count ?? -1}");
            if (query != null)
            {
                url.Append($"?{Encoding.UTF8.GetString(UnityWebRequest.SerializeSimpleForm(query))}");
            }

            var str = url.ToString();
            Debug.Log($"URL - {str}");
            return str;
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }

    public static async Task<T> GET<T>(string endpoint, string group = "", Query query = null, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint, query);
        Debug.Log($"GET - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header() 
            {
                {"Authorization", AccessToken }
            };
        }
        var response = await Rest.GetAsync(url, headers);

        Debug.Log("EXIT - GET");
        return GetJson<T>(response, group);
    }

    public static async Task<T> POST<T>(string endpoint, WWWForm form, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"POST - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken }
            };
        }
        var response = await Rest.PostAsync(url, form, headers);

        Debug.Log("EXIT - POST");
        return GetJson<T>(response);
    }

    public static async Task<T> PUT<T>(string endpoint, Data data, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"PUT - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken }
            };
        }
        var json = JsonConvert.SerializeObject(data);
        var response = await Rest.PutAsync(url, json, headers);

        Debug.Log("EXIT - PUT");
        return GetJson<T>(response);
    }

    public static async Task DELETE(string endpoint, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"DELETE - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken }
            };
        }
        await Rest.DeleteAsync(url, headers);

        Debug.Log("EXIT - DELETE");
    }

    public static async Task<T> PATCH<T>(string endpoint, Data data, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"PATCH - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken }
            };
        }
        var json = JsonConvert.SerializeObject(data);
        var response = await Rest.PatchAsync(url, json, headers);

        Debug.Log("EXIT - PATCH");
        return GetJson<T>(response);
    }
}
