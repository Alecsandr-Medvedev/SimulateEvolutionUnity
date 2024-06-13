using System.Collections;
using System.Collections.Generic;

public class OrganismCard : UnityEngine.MonoBehaviour
{
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI _Name;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI _Gen1;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI _Gen2;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI _Gen3;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI _Gen4;
    [UnityEngine.SerializeField]
    private UnityEngine.UI.Image _Renderer;

    public void Init(string Name, string Gen1, string Gen2, string Gen3, string Gen4, UnityEngine.Color color)
    {
        _Name.text = Name;
        _Gen1.text = "�����������: " + Gen1;
        _Gen2.text = "���������: " + Gen2;
        _Gen3.text = "��������: " + Gen3;
        _Gen4.text = "����: " + Gen4;
        _Renderer.color = color;
    }
    public void Init(string Name, float[] Gens)
    {
        float[] c = Settings.getGensToColor(Gens);
        UnityEngine.Color color = new UnityEngine.Color(c[0], c[1], c[2], c[3]);
        _Name.text = Name;
        _Gen1.text = $"�����������: {Gens[0]}";
        _Gen2.text = $"���������: {Gens[1]}";
        _Gen3.text = $"��������: {Gens[2]}";
        _Gen4.text = $"����: {Gens[3]}";
        _Renderer.color = color;
    }

}
