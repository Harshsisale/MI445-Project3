using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20;
    public float offsetX = 100;
    public float offsetY = 100;
    //public Transform player;
    private Texture2D texture;

    void Start()
    {
        offsetX = Random.Range(0f, 9999);
        offsetY = Random.Range(0f, 9999);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    void OnDestroy()
    {
        if (texture != null)
            Destroy(texture);
    }

    void Update()
    {
        /*
        if (player != null)
        {
            offsetX = player.position.x;
            offsetY = player.position.z;
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = GenerateTexture();
        }
        */
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = calculateColor(x, y);
                texture.SetPixel(x,y,color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color calculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        if (sample < 0.2)
        {
            return Color.black;
        }
        else if (sample < 0.4)
        {
            return Color.purple;
        }
        else if (sample < 0.6)
        {
            return Color.grey;
        }
        else if (sample < 0.8)
        {
            return Color.blue;
        }
        else
        {
            return Color.white;
        }
    }
}
