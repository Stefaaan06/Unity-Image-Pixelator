using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Method to pixelate an image
/// <author>Stefaaan06</author>
/// <version>1.0.0</version>
/// </summary>
public class ImagePixelator : MonoBehaviour
{
    public int rows = 4; // Number of rows
    public int columns = 4; // Number of columns
    public float delayBetweenParts = 0.5f; // Delay between each part in seconds

    private Image image;    //the attached image
    private Color[,] originalColors;    //the original colors of the image
    private Coroutine splitCoroutine;  //the coroutine that splits the image

    
    void Start()
    {
        image = GetComponent<Image>();
        originalColors = GetOriginalColors(image.sprite.texture);
    }
    
    /// <summary>
    /// Restore the original image when the application quits / a new scene is loaded to prevent the loss of the original image
    /// </summary>
    private void OnApplicationQuit()
    {
        RestoreOriginalImage();
    }
    
    
    private void OnDisable()
    {
        RestoreOriginalImage();
    }
    
    /// <summary>
    /// Call this to start the pixelation process
    /// </summary>
    void PixelateImage()
    {
        RestoreOriginalImage();
        if (splitCoroutine != null)
        {
            StopCoroutine(splitCoroutine);
        }

        splitCoroutine = StartCoroutine(PixelateImageCoroutine());
    }

    /// <summary>
    /// Coroutine that pixelates the image
    /// </summary>
    /// <returns>Waits between each pixelated area for "delayBetweenParts" amount of seconds</returns>
    IEnumerator PixelateImageCoroutine()
    {
        int totalParts = rows * columns;
        int[] indices = new int[totalParts];

        for (int i = 0; i < totalParts; i++)
        {
            indices[i] = i;
        }

        // Shuffle the array of indices
        System.Random rand = new System.Random();
        indices = indices.OrderBy(x => rand.Next()).ToArray();

        for (int i = 0; i < totalParts; i++)
        {
            yield return new WaitForSeconds(delayBetweenParts);

            int index = indices[i];
            int row = index / columns;
            int col = index % columns;

            SetPartTransparent(row, col);
        }
    }
    
    /// <summary>
    /// Sets a part of the image to transparent
    /// </summary>
    /// <param name="row">row index</param>
    /// <param name="col">col index</param>
    void SetPartTransparent(int row, int col)
    {
        int width = image.sprite.texture.width;
        int height = image.sprite.texture.height;

        int partWidth = width / columns;
        int partHeight = height / rows;

        Color[] transparentColors = new Color[partWidth * partHeight];
        for (int i = 0; i < transparentColors.Length; i++)
        {
            transparentColors[i] = Color.clear;
        }

        image.sprite.texture.SetPixels(col * partWidth, row * partHeight, partWidth, partHeight, transparentColors);
        image.sprite.texture.Apply();
    }

    /// <summary>
    ///  Restores the original image
    /// </summary>
   public void RestoreOriginalImage()
    {
        SetImageColors(originalColors);
    }
    
    /// <summary>
    ///  Restores the original image
    /// </summary>
    void RePixelateImage()
    {
        StartCoroutine(RePixelateCoroutine());
    }
    
    /// <summary>
    /// Coroutine that restores the original image
    /// </summary>
    /// <returns>Waits between each part for</returns>
    IEnumerator RePixelateCoroutine()
    {
        // Make all pixels transparent at the start
        SetImageColors(GetTransparentColors(image.sprite.texture));

        int totalParts = rows * columns;
        int[] indices = new int[totalParts];

        for (int i = 0; i < totalParts; i++)
        {
            indices[i] = i;
        }

        // Shuffle the array of indices
        System.Random rand = new System.Random();
        indices = indices.OrderBy(x => rand.Next()).ToArray();

        for (int i = 0; i < totalParts; i++)
        {
            yield return new WaitForSeconds(delayBetweenParts);

            int index = indices[i];
            int row = index / columns;
            int col = index % columns;

            SetPartOriginal(row, col);
        }
        RestoreOriginalImage();
    }

    /// <summary>
    /// Resets a part of the image to the original colors
    /// </summary>
    /// <param name="row">Row index of the Part</param>
    /// <param name="col">Column index of the Part</param>
    void SetPartOriginal(int row, int col)
    {
        int width = image.sprite.texture.width;
        int height = image.sprite.texture.height;

        int partWidth = width / columns;
        int partHeight = height / rows;

        // Copy the colors for the part from the originalColors array
        for (int i = 0; i < partHeight; i++)
        {
            for (int j = 0; j < partWidth; j++)
            {
                Color color = originalColors[col * partWidth + j, row * partHeight + i];
                image.sprite.texture.SetPixel(col * partWidth + j, row * partHeight + i, color);
            }
        }

        // Apply the changes to the texture
        image.sprite.texture.Apply();
    }

    /// <summary>
    /// Calculates the original colors for the image
    /// </summary>
    /// <param name="texture">The image texture</param>
    /// <returns>The original colors of the pixels in a 2D array</returns>
    Color[,] GetOriginalColors(Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;
        Color[,] colors = new Color[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[x, y] = texture.GetPixel(x, y);
            }
        }

        return colors;
    }
    
    /// <summary>
    /// Calculates the transparent colors for the image
    /// </summary>
    /// <param name="texture">The image texture</param>
    /// <returns>The transparent colors matching the image</returns>
    Color[,] GetTransparentColors(Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;
        Color[,] transparentColors = new Color[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                transparentColors[x, y] = Color.clear;
            }
        }

        return transparentColors;
    }

    /// <summary>
    /// Sets all pixels of the image to the given colors
    /// </summary>
    /// <param name="colors">2D array of colors</param>
    void SetImageColors(Color[,] colors)
    {
        int width = image.sprite.texture.width;
        int height = image.sprite.texture.height;

        Color[] flatColors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                flatColors[x + y * width] = colors[x, y];
            }
        }

        image.sprite.texture.SetPixels(flatColors);
        image.sprite.texture.Apply();
    }
}
