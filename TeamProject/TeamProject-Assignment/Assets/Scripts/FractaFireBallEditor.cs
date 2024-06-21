using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FireBall
{
    [CustomEditor(typeof(FractalFireBall))]
    [CanEditMultipleObjects]
    public class FractaFireBallEditor : Editor
    {
        bool materialAdvence = false;

        SerializedProperty mainTex;
        SerializedProperty noiseTex;
        SerializedProperty material;

        SerializedProperty heat;
        SerializedProperty alpha;

        SerializedProperty speed;
        SerializedProperty frequency;
        SerializedProperty scattering;

        SerializedProperty octaves;
        int[] octaveNums = { 1, 2, 3, 4, 5 };
        GUIContent[] octaveStrings = { new GUIContent("옥타브_1"), new GUIContent("옥타브_2"), new GUIContent("옥타브_3"), new GUIContent("옥타브_4"), new GUIContent("옥타브_5") };

        SerializedProperty quality;
        int[] qualityNums = { 0, 1, 2 };
        GUIContent[] qualityStrings = { new GUIContent("낮음"), new GUIContent("중간"), new GUIContent("높음") };

        public void OnEnable()
       {
            mainTex = serializedObject.FindProperty("TextureColor");
            noiseTex = serializedObject.FindProperty("Noise");
            material = serializedObject.FindProperty("Material");

            heat = serializedObject.FindProperty("Heat");
            alpha = serializedObject.FindProperty("Alpha");

            speed = serializedObject.FindProperty("Speed");
            frequency = serializedObject.FindProperty("Frequency");
            scattering = serializedObject.FindProperty("Scattering");

            octaves = serializedObject.FindProperty("Octaves");
            quality = serializedObject.FindProperty("Octaves");
        }

        public override void OnInspectorGUI()
        {
            FractalFireBall Mat = (FractalFireBall)target;
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();
            if(materialAdvence = EditorGUILayout.Foldout(materialAdvence, "머테리얼"))
            {
                EditorGUILayout.PropertyField(mainTex, new GUIContent("그라데이션"));
                EditorGUILayout.PropertyField(noiseTex, new GUIContent("노멀맵(노이즈)"));
                EditorGUILayout.PropertyField(material, new GUIContent("머테리얼"));
            }


            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tip) 검정 <-- 1=빨강 --> 흰색 ");
            EditorGUILayout.PropertyField(heat, new GUIContent("온도"));
            EditorGUILayout.Slider(alpha, 0, 1, new GUIContent("알파 값"));

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(speed, new GUIContent("노이즈 스피드"));
            EditorGUILayout.PropertyField(frequency, new GUIContent("노이즈 빈도"));
            EditorGUILayout.PropertyField(scattering, new GUIContent("노이즈 산란"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tip) 옥타브 = 화면의 프리퀀시(진동수)나 구체의 정도 조절");
            EditorGUILayout.IntPopup(octaves, octaveStrings, octaveNums, new GUIContent("옥타브"));
            EditorGUILayout.IntPopup(quality, qualityStrings, qualityNums, new GUIContent("품질"));
            serializedObject.ApplyModifiedProperties();

            //셰이더 업데이트를 위해서
            if (EditorGUI.EndChangeCheck() || Event.current.commandName == "UndoRedoPerformed") //undo(취소) 이벤트 확인
            {
                Mat.ShaderProperties();
            }

        }
    }
}
