using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Method to pixelate an image
/// <author>Stefaaan06</author>
/// <version>1.1.0</version>
/// </summary>
public class ImagePixelator : MonoBehaviour
{
    public int rows = 4; // Number of rows
    public int columns = 4; // Number of columns
    public float delayBetweenParts = 0.5f; // Delay between each part in seconds

    private Image _image;    //the attached image
    private Color[,] _originalColors;    //the original colors of the image
    private Coroutine _splitCoroutine;  //the coroutine that splits the image

    void Awake()
    {
        _image = GetComponent<Image>();

        Texture2D newTexture = new Texture2D(_image.sprite.texture.width, _image.sprite.texture.height);

        newTexture.SetPixels(_image.sprite.texture.GetPixels());
        newTexture.Apply();

        Rect rect = new Rect(0, 0, newTexture.width, newTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite newSprite = Sprite.Create(newTexture, rect, pivot);

        _image.sprite = newSprite;

        _originalColors = GetOriginalColors(_image.sprite.texture);

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
    public void PixelateImage()
    {
        StopAllCoroutines();
        RestoreOriginalImage();
        if (_splitCoroutine != null)
        {
            StopCoroutine(_splitCoroutine);
        }

        _splitCoroutine = StartCoroutine(PixelateImageCoroutine());
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
        int width = _image.sprite.texture.width;
        int height = _image.sprite.texture.height;

        int partWidth = width / columns;
        int partHeight = height / rows;

        Color[] transparentColors = new Color[partWidth * partHeight];
        for (int i = 0; i < transparentColors.Length; i++)
        {
            transparentColors[i] = Color.clear;
        }

        _image.sprite.texture.SetPixels(col * partWidth, row * partHeight, partWidth, partHeight, transparentColors);
        _image.sprite.texture.Apply();
    }

    /// <summary>
    ///  Restores the original image
    /// </summary>
   public void RestoreOriginalImage()
    {
        SetImageColors(_originalColors);
    }
    
    /// <summary>
    ///  Restores the original image
    /// </summary>
    public void RePixelateImage()
    {
        StopAllCoroutines();
        StartCoroutine(RePixelateCoroutine());
    }
    
    /// <summary>
    /// Coroutine that restores the original image
    /// </summary>
    /// <returns>Waits between each part for</returns>
    IEnumerator RePixelateCoroutine()
    {
        // Make all pixels transparent at the start
        if(_image == null)
            _image = GetComponent<Image>();
        SetImageColors(GetTransparentColors(_image.sprite.texture));

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
        int width = _image.sprite.texture.width;
        int height = _image.sprite.texture.height;

        int partWidth = width / columns;
        int partHeight = height / rows;

        // Copy the colors for the part from the originalColors array
        for (int i = 0; i < partHeight; i++)
        {
            for (int j = 0; j < partWidth; j++)
            {
                Color color = _originalColors[col * partWidth + j, row * partHeight + i];
                _image.sprite.texture.SetPixel(col * partWidth + j, row * partHeight + i, color);
            }
        }

        // Apply the changes to the texture
        _image.sprite.texture.Apply();
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
        int width = _image.sprite.texture.width;
        int height = _image.sprite.texture.height;

        Color[] flatColors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                flatColors[x + y * width] = colors[x, y];
            }
        }

        _image.sprite.texture.SetPixels(flatColors);
        _image.sprite.texture.Apply();
    }
}
