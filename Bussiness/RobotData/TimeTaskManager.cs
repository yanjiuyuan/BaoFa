using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
 


public abstract class AbstractTimeTask
{
    #region 字段
    private object obj;//任务id
    private uint m_uiTimeId;//任务id
    private int m_iInterval;//任务重复时间间隔,为0不重复
    private ulong m_ulNextTick;//下一次触发的时间点
    #endregion
    #region 属性
    public uint TimeId
    {
        get
        {
            return m_uiTimeId;
        }
        set
        {
            m_uiTimeId = value;
        }
    }
    public int Interval
    {
        get { return m_iInterval; }
        set { m_iInterval = value; }
    }
    public ulong NextTick
    {
        get
        {
            return m_ulNextTick;
        }
        set
        {
            this.m_ulNextTick = value;
        }
    }
    /// <summary>
    /// 抽象属性，给子类自定义自己的action委托
    /// </summary>
    public abstract Action <object > Action
    {
        get;
        set;
    }

    public object Obj
    {
        get
        {
            return obj;
        }

        set
        {
            obj = value;
        }
    }
    #endregion
    #region 公有方法
    /// <summary>
    /// 抽象方法，给自己自己定义执行委托
    /// </summary>
    public abstract void DoAction();
    #endregion
}

public class TimeTask : AbstractTimeTask
{
    #region 字段
    private Action<object> m_action;//定义自己的委托
 
    #endregion
    #region 属性
    public override Action<object> Action 
    {
        get
        {
            return m_action;
        }
        set
        {
            m_action = value;
        }
    }
    #endregion
    #region 公有方法
    /// <summary>
    /// 重新父类的委托方法
    /// </summary>
    public override void DoAction()
    {
        m_action(Obj);
    }
    #endregion
}



public class TimeTaskManager
{
    #region 字段
    private static uint m_uiNextTimeId;//总的id，需要分配给task，也就是每加如一个task，就自增
    private static uint m_uiTick;//总的时间，用来和task里面的nexttick变量来进行比较，看是否要触发任务
    private static List<AbstractTimeTask> m_queue;
    private static Stopwatch m_stopWatch;//c#自带的计时器，不会的自行百度
    private static readonly object m_queueLock = new object();//队列锁
    #endregion
    #region 构造方法
    private TimeTaskManager()
    {

    }
    static TimeTaskManager()
    {
        m_queue = new List<AbstractTimeTask>();
        m_stopWatch = new Stopwatch();
    }
    #endregion
    #region 公有方法
    /// <summary>
    /// 吧Task加入到队列里面来管理，既然是个管理器肯定要有个添加task的操作
    /// </summary>
    /// <param name="start">多久之后开始执行ms</param>
    /// <param name="interval">重复时间间隔ms</param>
    /// <param name="action">任务委托</param>
    /// <returns>任务id</returns>
    public static uint AddTimer(uint start, int interval, Action<object> action,object obj)
    {
        AbstractTimeTask task = GetTimeTask(new TimeTask(), start, interval, action, obj);
        lock (m_queueLock)
        {
            m_queue.Add(task);
        }
        return task.TimeId;
    }
    /// <summary>
    /// 周期性执行
    /// </summary>
    public static void Tick(object value)
    {
        try { 
        TimeTaskManager.m_uiTick += (uint)(m_stopWatch.ElapsedMilliseconds);
 
        m_stopWatch.Reset();
        m_stopWatch.Start();

        if (m_queue.Count == 0)
            return;

        for(int i=0;i< m_queue.Count;i++)
        {
            AbstractTimeTask task;
            lock (m_queueLock)
            {
                task = m_queue[i];
             }
            //时间未到
            if (TimeTaskManager.m_uiTick < task.NextTick) 
            {
                continue;
            }
            //时间已到，
            if (task.Interval > 0)//如果需要重复的话
            {
                task.NextTick += (ulong)task.Interval;
 
                task.DoAction();
            }
            //无需重复，的话，需要删除这个task
            else
            {
                task.DoAction();//执行委托
                lock (m_queueLock)
                {
                     m_queue.Remove(task);
                }


            }

        }
        }
        catch(Exception EX)
        {
            
        }


    }
    #endregion
    #region 私有方法
    private static AbstractTimeTask GetTimeTask(AbstractTimeTask task, uint start, int interval, Action<object> action,object obj)
    {
        task.Interval = interval;
        task.TimeId = ++TimeTaskManager.m_uiNextTimeId;
        task.NextTick = TimeTaskManager.m_uiTick + start;
        task.Action = action;
        task.Obj = obj;
        return task;
    }
    #endregion
}
