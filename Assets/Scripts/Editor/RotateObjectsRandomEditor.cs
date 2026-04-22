using UnityEngine;
using UnityEditor;

namespace Group1
{
    public class RotateObjectsRandomEditor
    {
        [MenuItem("Tools/Rotate Selected/Random Y Noise (90° Steps)")]
        private static void RotateSelectedRandomX()
        {
            foreach (var obj in Selection.transforms)
            {
                Undo.RecordObject(obj, "Random X Noise");
                Vector3 e = obj.localEulerAngles;
                e.y = 90f * Random.Range(0, 4); // 0, 90, 180, 270
                obj.localEulerAngles = e;
            }
        }
    }
}