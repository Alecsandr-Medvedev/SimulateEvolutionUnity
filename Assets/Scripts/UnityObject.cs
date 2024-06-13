using System.Collections;
using System.Collections.Generic;

public class UnityObject : UnityEngine.MonoBehaviour
{
    GameObjectM gameObject_;
    UnityEngine.SpriteRenderer render;
    private bool isDraw = true;
    [UnityEngine.SerializeField]
    private UnityEngine.GameObject select;

    public void Initialize(GameObjectM gameobject)
    {
        gameObject_ = gameobject;
        render = GetComponent<UnityEngine.SpriteRenderer>();
        render.color = new UnityEngine.Color(gameObject_.getColor()[0], gameObject_.getColor()[1], gameObject_.getColor()[2], gameObject_.getColor()[3]);
        transform.position = new UnityEngine.Vector3(gameObject_.getRect().X() / 100, gameObject_.getRect().Y() / 100, 0);
        transform.localScale = new UnityEngine.Vector3(gameObject_.getRect().Width() / 100, gameObject_.getRect().Height() / 100, 0);
        if (!gameObject_.isShow())
        {
            render.color = new UnityEngine.Color(gameObject_.getColor()[0], gameObject_.getColor()[1], gameObject_.getColor()[2], 0);
            isDraw = false;
        }
    }

    public void setSelect(bool flag)
    {
        select.SetActive(flag);
    }

    // Update is called once per frame
    void Update()
    {
        if (! isDraw && gameObject_.isShow())
        {
            render.color = new UnityEngine.Color(gameObject_.getColor()[0], gameObject_.getColor()[1], gameObject_.getColor()[2], gameObject_.getColor()[3]);
            isDraw = true;
        }
        else if (isDraw && !gameObject_.isShow())
        {
            render.color = new UnityEngine.Color(gameObject_.getColor()[0], gameObject_.getColor()[1], gameObject_.getColor()[2], 0);
            isDraw = false;
        }
        if (gameObject_.isShow())
        {
            transform.position = new UnityEngine.Vector3(gameObject_.getRect().X() / 100, gameObject_.getRect().Y() / 100, 0);
            transform.localScale = new UnityEngine.Vector3(gameObject_.getRect().Width() / 100, gameObject_.getRect().Height() / 100, 0);

        }
        
        if (!gameObject_.IsAlive())
        {
            Destroy(gameObject);
            gameObject_ = null;
        }

        /*setSelect(gameObject_.isSelect);*/
    }

    public ulong Collide()
    {
        return gameObject_.getId();
    }
}
