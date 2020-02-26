using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoly : MonoBehaviour
{
    abstract class Weapon_Item 
    {
        public string weaponName;
    }
    class Gun_Item : Weapon_Item 
    {
        public int bulletCount;
    }
    class Pistol_Item : Gun_Item { }


    abstract class Data { }
    abstract class Weapon_Data<T> : Data 
        where T : Weapon_Item, new()
    {
        public T weaponItem = new T();
    }
    //class Gun_Data : Weapon_Data<Gun_Item> { }
    //class Pistol_Data : Gun_Data { }
    class Pistol_Data : Weapon_Data<Pistol_Item> { }


    abstract class State_Base
    {
        public abstract void InitData(Data data);
    }
    abstract class Weapon_State<T> : State_Base
        where T : Weapon_Item, new()
    {
        protected Weapon_Data<T> data;

        public override void InitData(Data data)
        {
            //Debug.Log($"Is Parameter Data Null? {data == null}");
            this.data = data as Weapon_Data<T>;
            //Debug.Log($"Is Data Null? {this.data == null}");
        }
    }
    class Gun_Idle_State : Weapon_State<Gun_Item> 
    {
        public void Test()
        {
            //data.weaponItem.bulletCount = 10;
            //Debug.Log($"BulletCount: {data.weaponItem.bulletCount}");
        }
    }


    abstract class Weapon_Controller<T> where T : Data, new()
    {
        protected T data = new T();
    }
    class Pistol_Controller : Weapon_Controller<Pistol_Data>
    {
        private Gun_Idle_State state_Idle = new Gun_Idle_State();

        public void Init()
        {
            state_Idle.InitData(data);
            state_Idle.Test();
        }
    }

    private void Awake()
    {
        Pistol_Controller pistol = new Pistol_Controller();
        pistol.Init();
    }
}
