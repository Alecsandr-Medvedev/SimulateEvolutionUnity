using System.Collections;
using System.Collections.Generic;

public class Zone : GameObjectM
{
    private float temperature_, viscosity_, illumination_;
    private float[] allSettings_;

    public Zone(Rect rect, float t, float v, float i, ulong id, bool isshow) : base(rect, new float[4] { t, v, i, 0.5f }, id, 2)
    {
        temperature_ = t;
        viscosity_ = v;
        illumination_ = i;
        allSettings_ = new float[3] { t, v, i };
        is_show = isshow;
    }

    public float[] getSettings()
    {
        return allSettings_;
    }
}
