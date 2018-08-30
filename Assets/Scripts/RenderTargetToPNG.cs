using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RenderTargetToPNG : MonoBehaviour {

    public RenderTexture renderTexture;
    public string pngOutPath;


    [ContextMenu("RenderIt!")]
    public void RenderIT()
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }
}
