using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class FloatingText : MonoBehaviour
{
    private TextMesh _textMesh;
    private string _text;
    private float _tll;
    private float _timeLived = 0f;

    public static GameObject Make(string content, Vector3 position, Transform parent = null, float tll = 3f)
    {
        var prefab = AssetLoader.LoadAsset<GameObject>("Floating Text.prefab", "Generic/Floating Text");
        var gameObject = Instantiate(prefab, position, Quaternion.identity, parent);

        gameObject.GetComponent<FloatingText>().SetText(content).SetTll(tll);

        return gameObject;
    }

    public void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
        transform.FaceCamera();
    }

    public void Update()
    {
        if (_timeLived > _tll)
        {
            Destroy(gameObject);
            return;
        }

        UpdateTextPosition();
        UpdateTextTransparency();
        _timeLived += Time.deltaTime;
    }

    private void UpdateTextPosition() => transform.position += new Vector3(0, 0.001f);

    private void UpdateTextTransparency()
    {
        Color color = _textMesh.color;
        color.a = 1 - _timeLived / _tll;
        _textMesh.color = color;
    }

    private FloatingText SetText(string text)
    {
        _textMesh.text = text;

        return this;
    }

    private FloatingText SetTll(float tll)
    {
        _tll = tll;

        return this;
    }
}
