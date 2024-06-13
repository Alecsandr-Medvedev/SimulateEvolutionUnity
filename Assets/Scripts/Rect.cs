using System.Collections;
using System.Collections.Generic;

public class Rect
{
    private float x_, y_, width_, height_, centerX, centerY;
    public Rect(float x, float y, float w, float h)
    {
        x_ = x;
        centerX = x + (w / 2);
        y_ = y;
        centerY = y + (h / 2);
        width_ = w;
        height_ = h;
    }

    public float X()
    {
        return x_;
    }

    public float XC()
    {
        return centerX;
    }

    public float Y()
    {
        return y_;
    }

    public float YC()
    {
        return centerY;
    }

    public float Width()
    {
        return width_;
    }
    public float Height()
    {
        return height_;
    }

    public void MoveTo(float x, float y)
    {
        x_ = x;
        centerX = x + (width_ / 2);
        y_ = y;
        centerY = y + (height_ / 2);
    }

    public void Move(float x, float y)
    {
        x_ += x;
        y_ += y;
        centerX += x;
        centerY += y;
    }

    public bool Intersects(Rect rect)
    {
        return ! (rect.X() > x_ + width_ ||
                    rect.X() + rect.Width() < x_ ||
                    rect.Y() > height_ + y_ ||
                    rect.Y() + rect.Height() < y_);
    }

    public void setHeight(float h)
    {
        height_ = h;
    }

    public void setWidht(float w)
    {
        width_ = w;
    }
}
