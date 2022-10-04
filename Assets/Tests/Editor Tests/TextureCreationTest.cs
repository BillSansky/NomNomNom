#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Nom_Nom_Nom.Utility;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TextureCreationTest
{
    private static string textureCreatingSceneName = "Texture Creator";

    // A Test behaves as an ordinary method
    [Test]
    public void TestTextureCreation()
    {
        var currentScenes = EditorSceneManager.GetSceneManagerSetup();

        foreach (var sceneSetup in currentScenes)
        {
            EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(sceneSetup.path), true);
        }

        var assets = AssetDatabase.FindAssets(textureCreatingSceneName);

        Debug.Assert(assets.Length != 0, "The texture creation scene wasnt found");


        var scene = EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(assets[0]) + ".unity");

        Debug.Assert(scene.IsValid(), "The texture creation scene couldnt be loaded");

        var gosGuis = AssetDatabase.FindAssets("t:GameObject");

        Debug.Assert(gosGuis.Length > 0, "No Prefab Found!");

        foreach (var o in scene.GetRootGameObjects())
        {
            var potentialTextureCreators = o.GetComponentsInChildren<RenderTextureCreator>();
            if (potentialTextureCreators.Length == 0)
            {
                continue;
            }

            foreach (var gui in gosGuis)
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(gui));
                var gorRep = potentialTextureCreators[0].Photograph(go, "_Test");
                var path = AssetDatabase.GetAssetPath(gorRep);
                AssetDatabase.SaveAssets();
                AssetDatabase.MoveAsset(path, "Assets/Tests/" + gorRep.name + ".png");

            
            }
            
            foreach (var sceneSetup in currentScenes)
            {
                EditorSceneManager.OpenScene(sceneSetup.path, OpenSceneMode.Additive);
            }

            EditorSceneManager.CloseScene(scene, true);
            return;
        }

        Debug.Assert(false, "No Render texture creator script found");
    }
}
#endif