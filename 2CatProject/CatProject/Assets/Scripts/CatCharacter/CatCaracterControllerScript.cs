using UnityEngine;
using System.Collections;

public class CatCaracterControllerScript : MonoBehaviour
{
    //переменные для определения наличия движения
    public float moveLeftRight = 0;
    public float moveBottomTop = 0;
   
    //переменные для установки макс. скорости персонажа
    public float maxSpeedLeftRight = 10f;
    public float maxSpeedBottomTop = 5f;

    //переменная для определения направления персонажа вправо/влево
    private bool isFacingRight = false;

    //ссылка на компонент анимаций
    private Animator anim;

    //переменные для просмотра всех анимаций
    private bool keyZ = false;
    private bool keyX = false;
    private bool keyC = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //для просмотра всех анимаций
        if (Input.GetKeyDown(KeyCode.Z))
            {
            if (keyZ == false)
            {
                anim.SetBool("IsDoor", true);
                keyZ = true;
            }
            else
            {
                anim.SetBool("IsDoor", false);
                keyZ = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (keyX == false)
            {
                anim.SetBool("IsHeal", true);
                keyX = true;
            }
            else
            {
                anim.SetBool("IsHeal", false);
                keyX = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (keyC == false)
            {
                anim.SetBool("IsSleep", true);
                keyC = true;
            }
            else
            {
                anim.SetBool("IsSleep", false);
                keyC = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //присваиваем  move значения от -1 до 1 в зависимости от длительности нажания на кнопок WSAD
        moveLeftRight = Input.GetAxis("Horizontal");
        moveBottomTop = Input.GetAxis("Vertical");

        //делим moveBT на moveB и moveT
        float moveBottom = 0;
        float moveTop = 0;

        if (moveBottomTop > 0)
            moveTop = moveBottomTop;
        else
            moveBottom = moveBottomTop;

        //в компоненте анимаций изменяем значение параметра Speed на значение move
         anim.SetFloat("SpeedLeftRight", Mathf.Abs(moveLeftRight));
         anim.SetFloat("SpeedBottom", moveBottom);
         anim.SetFloat("SpeedTop", moveTop);
        
        //переходы между анимациями ходьбы
        if (Mathf.Abs(moveLeftRight) > Mathf.Abs(moveBottomTop) || Mathf.Abs(moveLeftRight) == Mathf.Abs(moveBottomTop))
        {
            anim.SetBool("IsRunLeftRight", true);
            anim.SetBool("IsRunBottom", false);
            anim.SetBool("IsRunTop", false);
        }
        else
        {
            if (-moveBottom > moveTop || -moveBottom == moveTop)
            {
                anim.SetBool("IsRunBottom", true);
                anim.SetBool("IsRunLeftRight", false);
                anim.SetBool("IsRunTop", false);
            }
            else
            {
                anim.SetBool("IsRunTop", true);
                anim.SetBool("IsRunLeftRight", false);
                anim.SetBool("IsRunBottom", false);
            }
        }
        
        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х,Y 
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveLeftRight * maxSpeedLeftRight, moveBottomTop* maxSpeedBottomTop);
                
        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if (moveLeftRight > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (moveLeftRight < 0 && isFacingRight)
            Flip();
    }

    
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
    }
}
