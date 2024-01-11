using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    [RequireComponent(typeof(Image))]
    public class ImageWrapper : BaseResWrapper<Image>
    {
        [SerializeField] private string sourceImagePath;
        [SerializeField] private string materialPath;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        protected override void RenderRes()
        {
            "TODO => 渲染".Error();
        }

        protected override void ClearRenderRes()
        {
            originalComponent.sprite = null;
            originalComponent.material = null;
        }
    }
}

