using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.Editor
{
    public abstract class BaseResWrapperInspector<T>: UnityEditor.Editor
        where T: Component
    {
        protected BaseResWrapper<T> resWrapper;

        protected T originalComponent
        {
            get => typeof(BaseResWrapper<T>).GetField("originalComponent", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(resWrapper) as T;
            set => typeof(BaseResWrapper<T>).GetField("originalComponent", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(resWrapper, value);
        }

        private void RenderRes() => typeof(BaseResWrapper<T>).GetMethod("RenderRes", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(resWrapper, new object[]{});
        private void ClearRenderRes() => typeof(BaseResWrapper<T>).GetMethod("ClearRenderRes", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(resWrapper, new object[]{});

        
        private void OnEnable()
        {
            resWrapper = target as BaseResWrapper<T>;
            if(originalComponent == null) originalComponent = resWrapper.GetComponent<T>();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using(new EditorGUILayout.VerticalScope())
            {
                // RenderOriginalComponent();
                RenderBtn();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void RenderOriginalComponent()
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(new SerializedObject(originalComponent).FindProperty("m_Script"));
                GUI.enabled = true;
            }
        }

        private void RenderBtn()
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                if(GUILayout.Button("渲染"))
                {
                    RenderRes();
                }
                if(GUILayout.Button("清空渲染"))
                {
                    ClearRenderRes();
                }
            }
        }
    }
}