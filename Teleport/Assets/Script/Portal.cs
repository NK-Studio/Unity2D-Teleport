using System;
using System.Collections.Generic;
using UnityEngine;

public enum PortalType
{
    NONE,
    RED,
    BLUE
}

enum MoveDir
{
    LEFT,
    RIGHT
}

public class Portal : MonoBehaviour
{
    //이동될 다른 포탈 트랜스폼 변수
    public GameObject MoveToPortal;

    //이동되는 느낌을 주는 클론 오브젝트
    public GameObject CloneImage;

    //이동할 다른 포탈의 이미지 사이즈
    public Vector2 PortalSize;

    //스프라이트 렌더 변수
    private SpriteRenderer _spr;

    //포탈의 타입을 정함
    public PortalType PT;

    //포탈을 지나가는 객체가 왼쪽에서 오른쪽, 오른쪽에서 왼쪽으로 이동하는지 체크하는 변수
    private Dictionary<string, MoveDir> MD = new Dictionary<string, MoveDir>();

    //오브젝트가 포탈을 모두 통과했는지에 대한 값
    private Dictionary<string, float> _portalxx = new Dictionary<string, float>();

    public Dictionary<string, Transform> Clones = new Dictionary<string, Transform>();

    private void Awake()
    {
        //데이터 할당
        _spr = GetComponent<SpriteRenderer>();

        //이미지의 사이즈를 가져옴
        PortalSize = MoveToPortal.GetComponent<SpriteRenderer>().size;

        //포탈의 레이어 마스크를 포탈로 설정해줌
        gameObject.layer = LayerMask.NameToLayer("Portal");

        //클론과 포탈이 충돌처리가 이루어지지 않도록 함
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Clone"), LayerMask.NameToLayer("Portal"), true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //객체의 클론을 만듬
        var Clone = Instantiate(CloneImage).GetComponent<PortalClone>();

        //Other의 스프라이트 렌더러를 수정하기 위함
        var OtherSpr = other.GetComponent<SpriteRenderer>();

        //Other가 어느 방향으로 접근하는지 알기 위함
        var OtherRig = other.GetComponent<Rigidbody2D>();

        //어떤 객체의 클론인지 이름을 정의함
        Clone.name = $"{other.name}_Clone";

        //클론S에 클론의 이름과 트랜스폼을 저장함
        Clones[Clone.name] = Clone.transform;

        //클론의 이미지를 플레이어의 이미지로 교체함
        Clone.setSprite(OtherSpr.sprite);

        //클론의 이미지의 이미지 그리기 레이어를 플레이어의 이미지 그리기 레이어와 동기화 함
        Clone.setSortingLayerName(OtherSpr.sortingLayerID);

        //클론의 이미지의 이미지 그리기 순서를 플레이어의 이미지 그리기 순서와 동기화 함
        Clone.setSortingOrder(OtherSpr.sortingOrder);

        //클론의 레이어를 Clone으로 변경함
        Clone.gameObject.layer = LayerMask.NameToLayer("Clone");

        //클론에게 충돌처리를 위한 콜라이더를 부여함
//        Clone.gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;

        //클론의 Position을 재 정의함
        var ClonePos = Clone.transform.position;

        //Other와 클론의 y를 동기화 함
        ClonePos.y = other.transform.position.y;
        Clone.transform.position = ClonePos;

        //클론의 회전을 Other의 회전으로 변경함
        Clone.transform.rotation = OtherSpr.transform.rotation;

        //클론의 크기를 Other의 크기로 변경함
        Clone.transform.localScale = OtherSpr.transform.localScale;

        //레드 포탈의 경우
        if (PT == PortalType.RED)
        {
            //오브젝트가 오른쪽 방향의 힘으로 접근했다면, A -> O 
            if (OtherRig.velocity.x > 0)
            {
                //객체가 오른쪽으로 이동하고 있음
                MD[$"{other.name}_Clone"] = MoveDir.RIGHT;

                //클론을 포탈의 안보이는 영역 왼쪽에 숨겨놓음
                ClonePos.x = MoveToPortal.transform.position.x - PortalSize.x;
                Clone.transform.position = ClonePos;

                //접근한 객체의 마스크 처리를 VisibleOutsideMask로 처리함
                OtherSpr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

                //클론의 이미지 마스크 처리를 VisibleOutsideMask로 처리함
                Clone.setMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
            }
            else //오브젝트가 왼쪽 방향의 힘으로 접근했다면, O <- A
            {
                //객체가 왼쪽으로 이동하고 있음
                MD[$"{other.name}_Clone"] = MoveDir.LEFT;

                //클론을 포탈의 안보이는 영역 오른쪽에 숨겨놓음
                ClonePos.x = MoveToPortal.transform.position.x + PortalSize.x;
                Clone.transform.position = ClonePos;

                //접근한 객체의 마스크 처리를 VisibleInsideMask로 처리함
                OtherSpr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                //클론의 이미지 마스크 처리를 VisibleInsideMask로 처리함
                Clone.setMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
            }
        }
        else if (PT == PortalType.BLUE)
        {
            //오브젝트가 오른쪽 방향의 힘으로 접근했다면, A -> O 
            if (OtherRig.velocity.x > 0)
            {
                //객체가 오른쪽으로 이동하고 있음
                MD[$"{other.name}_Clone"] = MoveDir.RIGHT;

                //클론을 포탈의 안보이는 영역 왼쪽에 숨겨놓음
                ClonePos.x = MoveToPortal.transform.position.x - PortalSize.x;
                Clone.transform.position = ClonePos;

                //접근한 객체의 마스크 처리를 VisibleInsideMask로 처리함
                OtherSpr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                //클론의 이미지 마스크 처리를 VisibleInsideMask로 처리함
                Clone.setMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
            }
            else //오브젝트가 왼쪽 방향의 힘으로 접근했다면, O <- A
            {
                //객체가 왼쪽으로 이동하고 있음
                MD[$"{other.name}_Clone"] = MoveDir.LEFT;

                //클론을 포탈의 안보이는 영역 오른쪽에 숨겨놓음
                ClonePos.x = MoveToPortal.transform.position.x + PortalSize.x;
                Clone.transform.position = ClonePos;

                //접근한 객체의 마스크 처리를 VisibleOutsideMask로 처리함
                OtherSpr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

                //클론의 이미지 마스크 처리를 VisibleOutsideMask로 처리함
                Clone.setMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //포탈에 들어가기 전 좌표
        var ImageStartX = transform.position.x - _spr.bounds.size.x;

        //포탈에 완전히 나갔을 때 좌표 
        var ImageEndX = transform.position.x + _spr.bounds.size.x;

        //오브젝트가 포탈의 앞 부분과 뒷 부분까지 이동함을 0~1로 표현 함
        _portalxx[$"{other.name}_Clone"] = (other.transform.position.x - ImageStartX) / (ImageEndX - ImageStartX);

        //클론의 x의 이동을 동기화 함
        var Clone = Clones[$"{other.name}_Clone"].position;
        Clone.x = Mathf.Lerp(MoveToPortal.transform.position.x - _spr.bounds.size.x,
            MoveToPortal.transform.position.x + _spr.bounds.size.x, _portalxx[$"{other.name}_Clone"]);

        //클론의 y의 이동을 동기화 함
        Clone.y = other.transform.position.y;

        //적용
        Clones[$"{other.name}_Clone"].position = Clone;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //객체가 오른쪽으로 이동하고 있고,
        if (MD[$"{other.name}_Clone"] == MoveDir.RIGHT)
        {
            //포탈 통과를 완료 했을 경우,
            if (_portalxx[$"{other.name}_Clone"] > 0.7f)
            {
                //클론과 객체의 위치를 변경함
                other.transform.position = Clones[$"{other.name}_Clone"].position;
                
                //클론 삭제
                Destroy(Clones[$"{other.name}_Clone"].gameObject);
            }
            else //포탈 통과를 하려다가 안했을 경우
                Destroy(Clones[$"{other.name}_Clone"].gameObject); //클론만 삭제
        }
        else if (MD[$"{other.name}_Clone"] == MoveDir.LEFT)
        {
            //포탈 통과를 완료 했을 경우,
            if (_portalxx[$"{other.name}_Clone"] < 0.3f)
            {
                //클론과 객체의 위치를 변경함
                other.transform.position = Clones[$"{other.name}_Clone"].position;

                //클론 삭제
                Destroy(Clones[$"{other.name}_Clone"].gameObject);
            }
            else //포탈 통과를 하려다가 안했을 경우
                Destroy(Clones[$"{other.name}_Clone"].gameObject); //클론만 삭제
        }

        //객체의 마스크를 초기화 함
        other.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
    }
}