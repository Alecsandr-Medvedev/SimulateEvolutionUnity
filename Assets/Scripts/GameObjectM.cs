using System.Collections;
using System.Collections.Generic;

public class GameObjectM
{
    protected Rect rect_;
    protected byte shape_;
    protected float[] color_;
    protected ulong id_;
    protected bool is_alive_;
    protected bool is_show;
    public bool isSelect;

    public GameObjectM(Rect rect, float[] color, ulong id, byte shape=0)
    {
        rect_ = rect;
        color_ = color;
        id_ = id;
        shape_ = shape;
        is_alive_ = true;
        is_show = true;
        isSelect = false;
    }

    public Rect getRect()
    {
        return rect_;
    }

    public byte getShape()
    {
        return shape_;
    }

    public float[] getColor()
    {
        return color_;
    }

    public ulong getId()
    {
        return id_;
    }

    public void Move(float x, float y)
    {
        rect_.Move(x, y);
    }

    public void MoveTo(float x, float y)
    {
        rect_.MoveTo(x, y);
    }

    public void setSize(float width, float height)
    {
        rect_.setWidht(width);
        rect_.setHeight(height);
    }

    public bool isIntersets(GameObjectM obj)
    {
        return rect_.Intersects(obj.getRect());
    }

    public bool IsAlive()
    {
        return is_alive_;
    }

    public void setAlive(bool al)
    {
        is_alive_ = al;
    }

    public bool isShow()
    {
        return is_show;
    }
    public void setShow(bool var)
    {
        is_show = var;
    }

}
