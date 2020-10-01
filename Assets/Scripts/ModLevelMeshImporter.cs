using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using System.IO;
using Siccity.GLTFUtility;
using UnityEngine.Networking;
using Directory = System.IO.Directory;
using Random = UnityEngine.Random;

public class ModLevelMeshImporter : MonoBehaviour
{
    [SerializeField] private Transform levelModelRoot;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject staticObjectPrefab;
    private string _pathToMods;

    private void Start()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        Debug.Log("Streaming Assets Path: " + Application.streamingAssetsPath);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.gltf", SearchOption.AllDirectories);

        foreach (FileInfo file in allFiles)
        {
            if (file.Directory != null && file.FullName.Contains(".gltf"))
            {
                GameObject result = Importer.LoadFromFile(file.FullName, Format.GLTF);
                result.transform.parent = levelModelRoot;
                StaticBatchingUtility.Combine(levelModelRoot.gameObject);
                MeshRenderer mr = result.GetComponent<MeshRenderer>();
                mr.material = defaultMaterial;

                MeshCollider mc = result.AddComponent<MeshCollider>();
                mc.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
            }
            Debug.Log(file.FullName);
        }
    }

    private void DirectoryCreation()
    {
        _pathToMods = Application.dataPath + "/Resources";
        if (!Directory.Exists(_pathToMods))
            Directory.CreateDirectory(_pathToMods);
    }

    private void MeshModding()
    {
        string[] meshModPaths = Directory.GetFiles(_pathToMods + "/", "*.fbx");
        string modToUse = "mod";
        string extensionToRemove = ".fbx";
        if (meshModPaths.Length == 1)
        {
            modToUse = Path.GetFileName(meshModPaths[0]);
            modToUse = modToUse.Replace(extensionToRemove, "");
            GameObject spawnObject = SpawnObject("Imported Model");
            MeshRenderer mr = spawnObject.AddComponent<MeshRenderer>();
            MeshFilter mf = spawnObject.AddComponent<MeshFilter>();

            mf.mesh = Resources.Load<Mesh>(modToUse);
            mr.material = defaultMaterial;
        }
        else if (meshModPaths.Length > 1)
        {
            int file = Random.Range(0, meshModPaths.Length);
            modToUse = Path.GetFileName(meshModPaths[file]);
            modToUse = modToUse.Replace(extensionToRemove, "");
            GameObject spawnObject = SpawnObject("Imported Model");
            MeshRenderer mr = spawnObject.AddComponent<MeshRenderer>();
            MeshFilter mf = spawnObject.AddComponent<MeshFilter>();

            mf.mesh = Resources.Load<Mesh>(modToUse);
            mr.material = defaultMaterial;
        }
    }

    private GameObject SpawnObject(string name, Type[] types = null)
    {
        GameObject spawnObject = new GameObject(name);
        spawnObject.transform.parent = levelModelRoot;

        return spawnObject;
    }


    private IEnumerator LoadLevelMesh(FileInfo fileInfo)
    {
        string fileNoExtension = Path.GetFileNameWithoutExtension(fileInfo.ToString());
        
        string wwwPlayerFilePath = "file://" + fileInfo.FullName.ToString();
        UnityWebRequest www = new UnityWebRequest(wwwPlayerFilePath);
        yield return www;
        
        GameObject spawnObject = SpawnObject("Imported Model");
        MeshRenderer mr = spawnObject.AddComponent<MeshRenderer>();
        MeshFilter mf = spawnObject.AddComponent<MeshFilter>();
        
        Mesh mesh = new Mesh();

        
        mr.material = defaultMaterial;


    }
}
