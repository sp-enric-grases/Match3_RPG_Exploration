using UnityEngine;
using UnityEditor;

namespace SocialPoint.Tools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FlyThroughPath))]
    public class FlyThroughPathInspector : BoxLayoutInspector
    {
        private FlyThroughPath ft;
        private SerializedProperty canvas;
        private SerializedProperty nextPath;
        private SerializedProperty cam;
        private SerializedProperty trigger;
        private SerializedProperty enemies;
        private SerializedProperty door;
        private SerializedProperty influencers;

        void OnEnable()
        {
            ft = (FlyThroughPath)target;
            canvas = serializedObject.FindProperty("canvas");
            nextPath = serializedObject.FindProperty("nextPath");
            cam = serializedObject.FindProperty("cam");
            trigger = serializedObject.FindProperty("trigger");
            door = serializedObject.FindProperty("door");
            influencers = serializedObject.FindProperty("inf");
            enemies = serializedObject.FindProperty("enemies");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            GUILayout.Space(5);

            SectionCanvas();
            SectionPathProperties();
            SectionRelocate();
            SectionEnemies();
            SectionOthers();
            SectionInfluencers();

            if (EditorGUI.EndChangeCheck())
                Undo.RegisterCompleteObjectUndo(ft, "Fly Through Path");

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed) EditorUtility.SetDirty(ft);
        }

        private void SectionCanvas()
        {
            Header("Select Main Canvas");
            EditorGUILayout.PropertyField(canvas, new GUIContent("Canvas"));
            EditorGUILayout.PropertyField(nextPath, new GUIContent("Next Path"), true);
            Footer();
        }

        private void SectionPathProperties()
        {
            Header("Path Properties");
            
            EditorGUILayout.PropertyField(cam, new GUIContent("Camera"));
            ft.cameraStartsFlying = EditorGUILayout.Toggle("Camera Starts Flying", ft.cameraStartsFlying);
            if (!ft.cameraStartsFlying) EditorGUILayout.PropertyField(trigger, new GUIContent("Trigger"));
            ft.pathDuration = Mathf.Clamp(EditorGUILayout.FloatField("Path Duration", ft.pathDuration), 2.0f, Mathf.Infinity);
            ft.curvePath = EditorGUILayout.CurveField("Curve", ft.curvePath, Color.green, new Rect(0, 0, 1, 1));
            
            CheckPathTime();
            Footer();
        }

        private void SectionRelocate()
        {
            Header("Camera Behaviour");

            EditorGUILayout.LabelField("Initial Relocation", EditorStyles.boldLabel);
            ft.timeToInitRelocatation = Mathf.Clamp(EditorGUILayout.FloatField("Time to Initial Relocation", ft.timeToInitRelocatation), 0.1f, Mathf.Infinity);
            ft.curveInitRelocation = EditorGUILayout.CurveField("Curve", ft.curveInitRelocation, Color.cyan, new Rect(0, 0, 1, 1));
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Final Relocation", EditorStyles.boldLabel);
            ft.finalRotation = EditorGUILayout.Vector3Field("Final Camera Rotation", ft.finalRotation);
            ft.timeToFinalRelocation = Mathf.Clamp(EditorGUILayout.FloatField("Time to Final Relocation", ft.timeToFinalRelocation), 0.1f,  Mathf.Infinity);
            ft.curveFinalRelocation = EditorGUILayout.CurveField("Curve", ft.curveFinalRelocation, Color.cyan, new Rect(0, 0, 1, 1));
            ft.offsetCompensation = EditorGUILayout.FloatField("Final compensation", ft.offsetCompensation);
            CheckPathTime();
            Footer();
        }

        private void SectionEnemies()
        {
            Header("Enemies");
            EditorGUILayout.PropertyField(enemies, new GUIContent("List of Enemies"), true);
            Footer();
        }

        private void SectionOthers()
        {
            Header("Others");
            EditorGUILayout.PropertyField(door, new GUIContent("Dungeon Door Container"), true);
            Footer();
        }

        private void SectionInfluencers()
        {
            Header("Influencers");
            EditorGUILayout.PropertyField(influencers, new GUIContent("List of Influencers"), true);
            Footer();
        }

        private void CheckPathTime()
        {
            float totalTime = ft.pathDuration - (ft.timeToInitRelocatation + ft.timeToFinalRelocation);

            if (totalTime <= 0)
                EditorGUILayout.HelpBox("The addition of the camera's init and final time relocation, should not be more than the path's duration time.", MessageType.Warning);
        }
    }
}