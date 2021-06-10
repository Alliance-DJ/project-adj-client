using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Editor
{
    public static class UnityObjectUtility
    {
        public static Object GetPrefabDefinition(this Object obj)
        {
            if (!obj.IsValid()) return null;

            return PrefabUtility.GetCorrespondingObjectFromSource(obj);
        }

        public static bool IsPrefabInstance(this Object obj)
        {
            if (!obj.IsValid()) return false;

            return GetPrefabDefinition(obj).IsValid();
        }

        public static bool IsPrefabDefinition(this Object obj)
        {
            if (!obj.IsValid()) return false;

            return GetPrefabDefinition(obj) == null && PrefabUtility.GetPrefabInstanceHandle(obj).IsValid();
        }

        public static bool IsConnectedPrefabInstance(this Object obj)
        {
            if (!obj.IsValid()) return false;

            return IsPrefabInstance(obj) && PrefabUtility.GetPrefabInstanceHandle(obj).IsValid();
        }

        public static bool IsDisconnectedPrefabInstance(this Object obj)
        {
            if (!obj.IsValid()) return false;

            return IsPrefabInstance(obj) && !PrefabUtility.GetPrefabInstanceHandle(obj).IsValid();
        }

        public static bool IsSceneBound(this Object obj)
        {
            if (!obj.IsValid()) return false;

            return (obj is GameObject go && !IsPrefabDefinition(go)) ||
                   (obj is Component com && !IsPrefabDefinition(com.gameObject));
        }
    }
}
