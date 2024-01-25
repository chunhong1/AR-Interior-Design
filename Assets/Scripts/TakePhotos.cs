using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NativeGalleryNamespace;
public class TakePhotos : MonoBehaviour
{
    public void TakePhoto()
    {
        StartCoroutine(TakeAPhoto());
    }
    IEnumerator TakeAPhoto()
    {
        yield return new WaitForEndOfFrame();
        Camera camera = Camera.main;
        int width = Screen.width;
        int height = Screen.height;
        RenderTexture rt = new RenderTexture(width, height, 24);
        camera.targetTexture = rt;

        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(width, height);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        camera.targetTexture = null;

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(image, "AR Interior Design", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.Log("Permission result: " + permission);

        //prevent memory leaks
        Destroy(rt);
        Destroy(image);

    }
}
