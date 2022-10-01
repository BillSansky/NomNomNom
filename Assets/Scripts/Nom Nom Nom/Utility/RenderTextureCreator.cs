#if UNITY_EDITOR
using UnityEditor;
#endif
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nom_Nom_Nom.Utility
{
    public class RenderTextureCreator : MonoBehaviour
    {
        [SerializeField] private Camera renderingCamera;

        [SerializeField] private RenderTexture renderTexture;

        public Transform photographyTransform;

#if UNITY_EDITOR

        [Button(ButtonSizes.Medium)]
        public void Photograph(GameObject go)
        {
            renderingCamera.targetTexture = renderTexture;
            renderingCamera.Render();

            Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = renderTexture;
            photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            photo.Apply();
            RenderTexture.active = null;
            string path = EditorUtility.SaveFilePanel("Save Texture", "Assets", go.name + "_photo", ".png");
            AssetDatabase.CreateAsset(photo, path);
        }
#endif
    }
}