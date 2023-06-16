using UnityEditor;
using UnityEngine;

namespace EditorCustomisation.Editors
{
    [CustomEditor(typeof(Object), true), CanEditMultipleObjects]
    public class UnityObjectEditor : Editor
    {
    }
}