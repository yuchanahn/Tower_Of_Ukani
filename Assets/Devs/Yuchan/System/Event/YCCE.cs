using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class YCCE<T>
{
    private Action pr_ev = () => { };
    private Action aft_ev = () => { };

    T value = default(T);
    public T Value
    {
        get => value; 
        set
        {
            pr_ev(); 
            this.value = value; 
            aft_ev();
        }
    }
    public void add_prev_event(Action act) => pr_ev += act;
    public void add_event(Action act) => aft_ev += act;
}