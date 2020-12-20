using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatformController : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;

    private PlayerBehaviour player;
    private BoxCollider2D bc;
    private float speed;

    public AudioClip[] clips;
    private AudioSource aud;
    private bool isActive;


    void Start()
    {
        aud = GetComponent<AudioSource>();
        startPos = transform.position;
        endPos = new Vector2(transform.position.x, transform.position.y+5);
        bc = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= endPos.y)//check top bounds and move down
        {
            speed = -1;
        }
        else if(transform.position.y <= startPos.y)//check bottom bounds and move up
        {
            speed = 1;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y + speed*Time.deltaTime);

        if (Mathf.Abs(player.transform.position.x - transform.position.x) < bc.size.x*(transform.localScale.x+0.1f)&& player.transform.position.y > transform.position.y && player.transform.position.y - transform.position.y < 1.6f)
            //check if player is on the platform and overlaps on x axis
        {
            //Debug.Log("x delta=" + Mathf.Abs(player.transform.position.x - transform.position.x));
            //Debug.Log("y delta=" +(player.transform.position.y - transform.position.y));
            if (!isActive)
            {
                aud.clip = clips[0];
                aud.Play();
                StartCoroutine(SetXScale(0));//set x scale to 0
            }
        }
        else//if player is not on the platform, reset x scale to 1
        {
            //Debug.Log("fall delta=" + (player.transform.position.y - transform.position.y));
            //Debug.Log(bc.size);
            if (isActive)
            {
                aud.clip = clips[1];
                aud.Play();
                StopAllCoroutines();
                StartCoroutine(SetXScale(1));
            }
        }
    }

    IEnumerator SetXScale(int targerScaleX)//must be 1 or 0
    {
        isActive = (targerScaleX == 0)?true:false;//set active condition based on the target scale x
        float duration = 2f;//total time costs from 1 to 0, or 0 to 1
        float startScaleX = transform.localScale.x;
        float ratio = (targerScaleX == 0)? startScaleX / 1: 1-(startScaleX / 1);//the percentage of "current scale x/target scale x"
        duration *= ratio;// culculate the new duration

        float t = 0;



        while (t < 1)
        {
            t += Time.deltaTime / duration;
            transform.localScale = new Vector2(Mathf.Lerp(startScaleX, targerScaleX, t), 1);
            yield return null;
        }
        isActive = false;
    }
}
