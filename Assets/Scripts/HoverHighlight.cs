using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HoverHighlight : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    private Material _material;
    private Color _originalEmissionColor;
   
    void Start()
    {
        _material = GetComponent<Renderer>().material;

        if (_material.HasProperty("_EmissionColor"))
        {
            _originalEmissionColor = _material.GetColor("_EmissionColor");
        }
    }

    void OnMouseEnter()
    {
        if (_material.HasProperty("_EmissionColor"))
        {
            _material.EnableKeyword("_EMISSION");
            _material.SetColor("_EmissionColor", highlightColor);

        }
    }

    void OnMouseExit()
    {
        if (_material.HasProperty("_EmissionColor"))
        {
            _material.SetColor("_EmissionColor", _originalEmissionColor);

        }
    }
}
