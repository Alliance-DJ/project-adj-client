using UnityEditor;

using Object = UnityEngine.Object;

namespace Editor
{
    public static class UndoUtil
    {
        public static void RecordObject(Object objectToUndo, string name)
        {
            if (objectToUndo == null) return;

            Undo.RegisterCompleteObjectUndo(objectToUndo, name);

            if (!objectToUndo.IsSceneBound())
            {
                EditorUtility.SetDirty(objectToUndo);
            }

            if (objectToUndo.IsPrefabInstance())
            {
                EditorApplication.delayCall += () => { PrefabUtility.RecordPrefabInstancePropertyModifications(objectToUndo); };
            }
        }
    }
}