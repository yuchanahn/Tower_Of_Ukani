using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

//루트(Root), 컴포짓(Composite), 데코레이터(Decorator), 서비스(Service), 태스크(Task)

namespace BT
{
    using Condition = Func<bool>;
    using BTAction = Func<bool>;
    using BTService = Action;

    public class  BT_Base : MonoBehaviour
    {
        public Dictionary<int, float> SvTime = new Dictionary<int, float>();
        protected Root root;
        bool Stop = true;
        public void InitBT(BlackBoard_Base BlackBoard)
        {
            root = new Root(BlackBoard, this);
            root.node.root = root;
            BT_Set();
        }

        virtual protected void Start() {
            
        }
        virtual protected void BT_Set()
        {
            Stop = false;
        }

        private void Update() {
            if(Stop) return;
            for (int i = 0; i < root.id; i++)
            {
                SvTime[i] += Time.deltaTime;
            }

            root.node.Process();
        }
    }


    public abstract class BT_Component
    {
        public Root root;
        BT_Component Parent;
        protected List<BT_Component> childrens = new List<BT_Component>();
        public BT_Component AddNode(BT_Component children)
        {
            children.Parent = this;

            if(root == null) Debug.Log("root null");

            children.root = root;
            childrens.Add(children);
            return children;
        }
        public BT_Component End()
        {
            return Parent;
        }

        abstract public bool Process();
    }
    public class Root
    {
        public Selector node;
        public int id = 0;
        public Dictionary<int, float> SvTime { get=>base_.SvTime; }
        BT_Base base_;
        public Root(BlackBoard_Base BlackBoard, BT_Base Base)
        {
            base_ = Base;
            node = new Selector();
        }

        public int GetSvId(){
            return id++;
        }
        
    }

    // public class Composite : BT_Component
    // {
    // }



    public class Sequence : BT_Component
    {
        override public bool Process()
        {
            foreach (var item in childrens)
            {
                if( !item.Process() ) return false;
            }
            return true;
        }
    }
    public class Selector : BT_Component
    {
        override public bool Process()
        {
            foreach (var item in childrens)
            {
                if( item.Process() ) return true;
            }
            return false;
        }
    }
    public class BTSNone : BT_Component
    {   
        override public bool Process()
        {
            return false;
        }
    }
    public class Decorator : BT_Component
    {
        Condition condi_;
        public Decorator(Condition condition){
            condi_ = condition;
        }
        override public bool Process()
        {
            bool result = condi_();

            if(!result) return false;

            foreach (var item in childrens)
            {
                if( item.Process() ) return true;
            }
            return false;
        }
    }

    public class SimpleParallel : BT_Component
    {
        public SimpleParallel()
        {

        }
        
        public override bool Process()
        {
            foreach (var item in childrens)
            {
                if(!item.Process()) break;
            }
            return true;
        }
    }
    public class Service : BT_Component
    {
        int mSvId;
        float time_;
        BTService act_;

        bool OnService = false;
        public Service(BTService act, float time = 0)
        {
            time_ = time;
            act_ = act;
        }
        public override bool Process()
        {
            if(!OnService){
                OnService = true;
                mSvId = root.GetSvId();
                root.SvTime[mSvId] = time_;
            }
            if (root.SvTime[mSvId] >= time_)
            {
                root.SvTime[mSvId] = 0;
                act_();
            }
            foreach (var item in childrens)
            {
                if(item.Process()) break;
            }
            return true;
        }
    }

    public class Task : BT_Component
    {
        BTAction act_;
        public Task(BTAction act){
            act_ = act;
        }
        public override bool Process()
        {
            return act_();
        }
    }
}

