// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/// <summary>
/// Response to a REST Call.
/// </summary>
public struct Response
{
    /// <summary>
    /// Was the REST call successful?
    /// </summary>
    public readonly bool Successful;

    /// <summary>
    /// Response body from the resource.
    /// </summary>
    public string ResponseBody => responseBody ??= responseBodyAction?.Invoke();

    private string responseBody;
    private readonly System.Func<string> responseBodyAction;

    /// <summary>
    /// Response data from the resource.
    /// </summary>
    public byte[] ResponseData => responseData ??= responseDataAction?.Invoke();

    private byte[] responseData;
    private readonly System.Func<byte[]> responseDataAction;

    /// <summary>
    /// Response code from the resource.
    /// </summary>
    public readonly long ResponseCode;

    /// <summary>
    /// Constructor.
    /// </summary>
    public Response(bool successful, string responseBody, byte[] responseData, long responseCode)
    {
        Successful = successful;
        responseBodyAction = null;
        this.responseBody = responseBody;
        responseDataAction = null;
        this.responseData = responseData;
        ResponseCode = responseCode;
    }

    public Response(bool successful, System.Func<string> responseBodyAction, System.Func<byte[]> responseDataAction,
        long responseCode)
    {
        Successful = successful;
        this.responseBodyAction = responseBodyAction;
        responseBody = null;
        this.responseDataAction = responseDataAction;
        responseData = null;
        ResponseCode = responseCode;
    }
}