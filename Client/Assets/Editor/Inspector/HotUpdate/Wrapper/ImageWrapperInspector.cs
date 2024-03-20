using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ImageWrapper = Ninth.HotUpdate.ImageWrapper;

namespace Ninth.Editor
{
    [CustomEditor(typeof(ImageWrapper), true)]
    public class ImageWrapperInspector : BaseResWrapperInspector<Image>
    {
        private Sprite sourceImageWrapper;
        private string sourceImagePath
        {
            get => typeof(ImageWrapper).GetField("sourceImagePath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(resWrapper) as string;
            set => typeof(ImageWrapper).GetField("sourceImagePath", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(resWrapper, value);
        }
        
        private Material materialWrapper;
        private string materialPath
        {
            get => typeof(ImageWrapper).GetField("materialPath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(resWrapper) as string;
            set => typeof(ImageWrapper).GetField("materialPath", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(resWrapper, value);
        }

        public override void OnInspectorGUI()
        {
            using(new EditorGUILayout.VerticalScope())
            {
                GUI.enabled = false;
                StoreSourceImage();
                StoreMaterial();
                GUI.enabled = true;
            }
            base.OnInspectorGUI();
        }

        private void StoreSourceImage()
        {
            EditorGUILayout.ObjectField("Source Image", sourceImageWrapper, typeof(Sprite), true);
            Sprite sprite = originalComponent.sprite;
            if(sprite == null || sprite == sourceImageWrapper)
            {
                return;
            }
            sourceImageWrapper = sprite;
            sourceImagePath = sprite.name;
        }

        private void StoreMaterial()
        {
            EditorGUILayout.ObjectField("Material", materialWrapper, typeof(Material), true);
            Material material = originalComponent.material;
            if(material == null || material == materialWrapper)
            {
                return;
            }
            materialWrapper = material;
            materialPath = material.name;
        }
    }
}
