using pdxpartyparrot.Core.Splines;

using UnityEditor;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/curves-and-splines/

// TODO: move to Core.Editor.Math
namespace pdxpartyparrot.Core.Editor
{
    [CustomEditor(typeof(Line))]
    public class LineInspector : UnityEditor.Editor
    {
#region Unity Lifecycle
        private void OnSceneGUI()
        {
		    Line line = (Line)target;

		    Transform handle = line.transform;
		    Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handle.rotation : Quaternion.identity;

		    Vector3 p0 = handle.TransformPoint(line.P0);
		    Vector3 p1 = handle.TransformPoint(line.P1);

		    Handles.color = Color.white;
		    Handles.DrawLine(p0, p1);

		    EditorGUI.BeginChangeCheck();
		    p0 = Handles.DoPositionHandle(p0, handleRotation);
		    if(EditorGUI.EndChangeCheck()) {
			    Undo.RecordObject(line, "Move Point");
			    EditorUtility.SetDirty(line);
			    line.P0 = handle.InverseTransformPoint(p0);
		    }

		    EditorGUI.BeginChangeCheck();
		    p1 = Handles.DoPositionHandle(p1, handleRotation);
		    if(EditorGUI.EndChangeCheck()) {
			    Undo.RecordObject(line, "Move Point");
			    EditorUtility.SetDirty(line);
			    line.P1 = handle.InverseTransformPoint(p1);
		    }
	    }
#endregion
    }
}
