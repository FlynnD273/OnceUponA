// From https://discussions.unity.com/t/change-default-script-folder/92375/4

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

//purpose of this postprocessor is to ensure that listed file extensions
// are not in certain filepaths, when they are they are moved to a 
//specified default path
public class FileImportHandler : AssetPostprocessor
{
    //only evaluate files imported into these paths
    private static readonly List<string> pathsToMoveFrom = new() { "Assets" };

    private static readonly Dictionary<string, string> defaultFileLocationByExtension = new()
    {
		/* {".mp4",   "Assets/StreamingAssets/"},//for IOS, movies need to be in StreamingAssets */

		/* {".anim",   "Assets/Art/Animations/"}, */
		/* {".mat",    "Assets/Art/Materials/"}, */
		/* {".fbx",    "Assets/Art/Meshes/"}, */

		//Images has subfolders for Textures, Maps, Sprites, etc.
		// up to the user to properly sort the images folder
		/* {".bmp",    "Assets/Art/Images/"}, */
		/* {".png",    "Assets/Art/Images/"}, */
		/* {".jpg",    "Assets/Art/Images/"}, */
		/* {".jpeg",   "Assets/Art/Images/"}, */
		/* {".psd",    "Assets/Art/Images/"}, */
		
		/* {".mixer",    "Assets/Audio/Mixers"}, */
        //like images, there are sub folders that the user must manage
		/* {".wav",    "Assets/Audio/Sources"}, */ 

        //like images, there are sub folders that the user must manage
		{".cs",     "Assets/Scripts/"}, 
		/* {".shader", "Assets/Dev/Shaders/"}, */
		/* {".cginc",  "Assets/Dev/Shaders/"} */
	};

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] _1, string[] _2, string[] _3)
    {
        foreach (string oldFilePath in importedAssets)
        {
            string directory = Path.GetDirectoryName(oldFilePath);
            if (!pathsToMoveFrom.Contains(directory)) { continue; }

            string extension = Path.GetExtension(oldFilePath).ToLower();
            if (!defaultFileLocationByExtension.ContainsKey(extension)) { continue; }

            string filename = Path.GetFileName(oldFilePath);
            string newPath = defaultFileLocationByExtension[extension];

            _ = AssetDatabase.MoveAsset(oldFilePath, newPath + filename);


            Debug.Log(string.Format("Moving asset ({0}) to path: {1}", filename, newPath));
        }
    }
}

