using UnityEngine;
using UnityEditor;
using System.Collections;

public class MaskMaker : EditorWindow
{
	[MenuItem ("MaskMaker/OpenTheMaskMaker")]

	static void Init ()
	{
		((MaskMaker)EditorWindow.GetWindow (typeof(MaskMaker))).title = "MaskMaker";
	}

	static TextureFormat CompressionFormat {
		get {
			switch (EditorUserBuildSettings.activeBuildTarget) {
			case BuildTarget.Android:
				return TextureFormat.ETC_RGB4;
			case BuildTarget.iPhone:
				return TextureFormat.PVRTC_RGB4;
			default:
				return TextureFormat.DXT1;
			}
		}
	}

	static TextureImporterFormat ImporterFormat{
		get{
			switch (EditorUserBuildSettings.activeBuildTarget) {
			case BuildTarget.Android:
				return TextureImporterFormat.ETC_RGB4;
			case BuildTarget.iPhone:
				return TextureImporterFormat.PVRTC_RGB4;
			default:
				return TextureImporterFormat.DXT1;
			}
		}
	}

	private Object[] objects;
	private bool showTexture = true;

	void OnGUI ()
	{
		objects = Selection.objects;

		EditorGUILayout.LabelField ("if press create, generate mask texture");
		EditorGUILayout.Space ();

		showTexture = EditorGUILayout.Foldout( showTexture, "Textures" );

		if (showTexture) {
			int i = 1;
			foreach (Object obj in objects) {
				if (obj is Texture2D) {
					EditorGUILayout.LabelField (i++ + " : " + obj.name);
				}
			}
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Create")) {

			foreach(Object obj in objects){
				if (!(obj is Texture2D))
					continue;

				CreateMask (obj as Texture2D);

			}
		}
	}

	void CreateMask(Texture2D texture){
		if (texture == null) {
			Debug.Log ("texture = null , unselected Texture");
			return;
		}

		//Textureのパスを取得
		string path = AssetDatabase.GetAssetPath (texture);

		//パスからTextureImportへアクセス
		TextureImporter importer = AssetImporter.GetAtPath (path) as UnityEditor.TextureImporter; 
		//Textureを読み込み可へ
		if (importer.isReadable == false)
			importer.isReadable = true; 
		//マスク画像を作成するため、テクスチャのフォーマットをRGBA32へ変更
		importer.textureFormat = TextureImporterFormat.RGBA32;
		importer.mipmapEnabled = false;
		//TextureImporterの更新
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);


		Texture2D mask = new Texture2D (texture.width, texture.height, TextureFormat.RGB24, false);
		mask.wrapMode = TextureWrapMode.Clamp;

		var pixels = texture.GetPixels ();
		for (int i = 0; i < pixels.Length; i++) {
			var a = pixels [i].a;
			pixels [i] = new Color (a, a, a);
		}
		mask.SetPixels (pixels);

		//MaskTetureを圧縮
		EditorUtility.CompressTexture(mask,CompressionFormat,TextureCompressionQuality.Best);
		string maskPath = path.Replace (".png","_mask.asset");

		Object maskAsset = AssetDatabase.LoadAssetAtPath (maskPath, typeof(Texture2D)) as Texture2D;
		if (maskAsset == null) {
			AssetDatabase.CreateAsset (mask, maskPath);
		} else {
			EditorUtility.CopySerialized (mask, maskAsset);
		}

		//プラットフォームに対応したフォーマットに設定
		importer.textureFormat = ImporterFormat;
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
	}

}
