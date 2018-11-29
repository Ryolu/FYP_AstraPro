﻿using UnityEngine;
using UnityEngine.UI;

public class BodyMaskRenderer : MonoBehaviour
{
    private long _lastFrameIndex = -1;
    private Texture2D _texture;
    private const TextureFormat Format = TextureFormat.RGBA32;

    private void Start()
    {
        _texture = new Texture2D(320, 240, Format, false);
        if (GetComponent<Image>())
            GetComponent<Image>().sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
        else
            GetComponent<Renderer>().material.mainTexture = _texture;
    }

    public void OnNewFrame(Astra.ColorizedBodyFrame frame)
    {
        if (frame.Width == 0 ||
            frame.Height == 0)
        {
            return;
        }

        if (_lastFrameIndex == frame.FrameIndex)
        {
            return;
        }

        _lastFrameIndex = frame.FrameIndex;
        EnsureTexture(frame.Width, frame.Height);
        _texture.LoadRawTextureData(frame.DataPtr, (int)frame.ByteLength);
        _texture.Apply();
    }

    private void EnsureTexture(int width, int height)
    {
        if (_texture == null)
        {
            _texture = new Texture2D(width, height, Format, false);
            if (GetComponent<Image>())
                GetComponent<Image>().sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
            else
                GetComponent<Renderer>().material.mainTexture = _texture;
            return;
        }

        if (_texture.width != width ||
            _texture.height != height)
        {
            _texture.Resize(width, height);
        }
    }
}