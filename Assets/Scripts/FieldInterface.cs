using System.Collections;
using System.Collections.Generic;

public class FieldInterface : UnityEngine.MonoBehaviour
{
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI nameField;
    [UnityEngine.SerializeField]
    private TMPro.TMP_InputField variable;

    public void Initialized(string name, string var)
    {
        setName(name);
        setVariable(var);
    }

    public void setVariable(string var)
    {
        variable.text = var;
    }

    public void setName(string name)
    {
        nameField.text = name;
    }

    public void changeVariable()
    {
        Settings.setElement(nameField.text, variable.text);
    }
}
