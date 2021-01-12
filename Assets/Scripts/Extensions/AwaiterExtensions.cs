using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public static class AwaiterExtensions
{
    public static TaskAwaiter<object> GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return tcs.Task.GetAwaiter();
    }
}
