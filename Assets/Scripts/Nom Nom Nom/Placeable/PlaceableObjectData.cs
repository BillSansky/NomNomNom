using System;
using Nom_Nom_Nom.Utility;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif
using UnityEngine;

namespace Nom_Nom_Nom.Placeable
{
    [CreateAssetMenu(fileName = "Placeable Object Data", menuName = "Nom Nom Nom/Placeable Object Data")]
    public class PlaceableObjectData : ScriptableObject
    {
        [SerializeField] private string objectName;
        
        public string ObjectName => objectName;

        [SerializeField] [AssetsOnly] [AssetList]
        private PlaceableObject placeableObject;

        public PlaceableObject PlaceableObject => placeableObject;


        [SerializeField] private Texture2D uiRepresentation;

        public Texture2D UIRepresentation => uiRepresentation;

#if UNITY_EDITOR

        private static string textureCreatingSceneName = "Texture Creator";

        [Button(ButtonSizes.Medium),ShowIf("@uiRepresentation == false")]
        private void GenerateTextureForAsset()
        {
            var currentScenes = EditorSceneManager.GetSceneManagerSetup();

            foreach (var sceneSetup in currentScenes)
            {
                EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(sceneSetup.path), true);
            }

            var assets = AssetDatabase.FindAssets(textureCreatingSceneName);

            if (assets.Length == 0)
            {
                Debug.LogError("The texture creation scene couldnt be found", this);
                return;
            }

            var scene = EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(assets[0])+".unity");
            foreach (var o in scene.GetRootGameObjects())
            {
                var potentialTextureCreators = o.GetComponentsInChildren<RenderTextureCreator>();
                if (potentialTextureCreators.Length == 0)
                {
                    continue;
                }

                uiRepresentation = potentialTextureCreators[0].Photograph(placeableObject.gameObject);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();

                foreach (var sceneSetup in currentScenes)
                {
                    EditorSceneManager.OpenScene(sceneSetup.path, OpenSceneMode.Additive);
                }

                EditorSceneManager.CloseScene(scene, true);

                return;
            }
        }

#endif
    }
}