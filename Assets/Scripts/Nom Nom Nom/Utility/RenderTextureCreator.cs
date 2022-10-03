#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using MonKey.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Nom_Nom_Nom.Utility
{
    public class RenderTextureCreator : MonoBehaviour
    {
        [SerializeField] private Camera renderingCamera;

        [SerializeField] private RenderTexture renderTexture;

        public Transform photographyTransform;

        public TextureFormat formatToExportTo = TextureFormat.RGB24;

        [Button(ButtonSizes.Medium)]
        public Texture2D Photograph(GameObject go)
        {
            string name = go.name;

            go = Instantiate(go, photographyTransform, false);
            go.transform.Reset();
            renderingCamera.Render();
            var prevActive = RenderTexture.active;
            RenderTexture.active = renderTexture;

            Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, formatToExportTo, false);
            photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            photo.Apply();

            RenderTexture.active = prevActive;

            string path = AssetDatabase.GetAssetPath(renderTexture).Replace(".renderTexture", ".png")
                .Replace(renderTexture.name, name + "_photo");

            File.WriteAllBytes(path, photo.EncodeToPNG());
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            DestroyImmediate(go);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
    }
}

#endif