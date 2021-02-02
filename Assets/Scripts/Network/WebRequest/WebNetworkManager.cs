using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Data = System.Collections.Generic.Dictionary<string, string>;
using Header = System.Collections.Generic.Dictionary<string, string>;
using Query = System.Collections.Generic.Dictionary<string, string>;

public static class WebNetworkManager
{
    private const string APIBaseURL = "localhost:3000"; // host

    public static string AccessToken { get; private set; }

    public static void SetAccessToken(string token)
    {
        AccessToken = Rest.GetBearerOAuthToken(token);
    }

    private static T GetJson<T>(Response response, string group = null)
    {
        var body = response.ResponseBody;

        if (string.IsNullOrWhiteSpace(group) == false)
        {
            var builder = new StringBuilder();
            builder.Append("{\"");
            builder.Append(group);
            builder.Append("\":");
            builder.Append(body);
            builder.Append("}");
            body = builder.ToString();
        }

        var data = JsonConvert.DeserializeObject<T>(body);
        return data;
    }

    private static string MakeURL(string endpoint, Query query = null)
    {
        try
        {
            var url = new StringBuilder($"{APIBaseURL}{endpoint}");
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

    public static async Task<T> Get<T>(string endpoint, string group = "", Query query = null,
        bool useAccessToken = true)
    {
        var url = MakeURL(endpoint, query);
        Debug.Log($"GET - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken}
            };
        }

        var response = await Rest.GetAsync(url, headers);

        Debug.Log("EXIT - GET");
        return GetJson<T>(response, group);
    }

    public static async Task<T> Post<T>(string endpoint, WWWForm form, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"POST - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken}
            };
        }

        var response = await Rest.PostAsync(url, form, headers);

        Debug.Log("EXIT - POST");
        return GetJson<T>(response);
    }

    public static async Task<T> Put<T>(string endpoint, Data data, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"PUT - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken}
            };
        }

        var json = JsonConvert.SerializeObject(data);
        var response = await Rest.PutAsync(url, json, headers);

        Debug.Log("EXIT - PUT");
        return GetJson<T>(response);
    }

    public static async Task Delete(string endpoint, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"DELETE - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken}
            };
        }

        await Rest.DeleteAsync(url, headers);

        Debug.Log("EXIT - DELETE");
    }

    public static async Task<T> Patch<T>(string endpoint, Data data, bool useAccessToken = true)
    {
        var url = MakeURL(endpoint);
        Debug.Log($"PATCH - {url}");

        Header headers = null;
        if (useAccessToken)
        {
            headers = new Header()
            {
                {"Authorization", AccessToken}
            };
        }

        var json = JsonConvert.SerializeObject(data);
        var response = await Rest.PatchAsync(url, json, headers);

        Debug.Log("EXIT - PATCH");
        return GetJson<T>(response);
    }
}