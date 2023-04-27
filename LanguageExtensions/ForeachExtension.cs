using System;

namespace Azusa.Shared.LanguageExtensions;

/// <summary>
/// Range扩展类，手动地实现IEnumerator
/// </summary>
public static class ForeachExtension
{
    /// <summary>
    /// 拓展Range结构实现GetEnumerator方法供foreach读取，实现foreach(var i in x..y)
    /// </summary>
    /// <returns></returns>
    public static CustomIntEnumerator GetEnumerator(this Range range)
    {
        return new CustomIntEnumerator(range);
    }

    /// <summary>
    /// 拓展int类实现GetEnumerator方法供foreach读取，实现foreach(var i in x)
    /// </summary>
    /// <param name="end"></param>
    /// <returns></returns>
    public static CustomIntEnumerator GetEnumerator(this int end)
    {
        return new CustomIntEnumerator(end);
        return new CustomIntEnumerator(new Range(0, end));//在执行空函数时性能比上一句低10倍，Why
    }
}

//使用引用结构体增强性能
public ref struct CustomIntEnumerator
{
    private int _current;
    private readonly int _end;

    public CustomIntEnumerator(Range range)
    {
        //避免某些时候从结尾开始编制
        // x.. 时会产生Range(x,^0)
        if (range.End.IsFromEnd)
        {
            throw new NotSupportedException("不支持从结尾编制索引");
        }
        _current = range.Start.Value - 1;
        _end = range.End.Value - 1;//迭代器不包含范围的尾部
    }

    public CustomIntEnumerator(int end)
    {
        _current = -1;
        _end = end;
    }

    //错误示例：不要实现IEnumerator接口！接口定义Current返回object，会装箱！
#if false
    public object Current => _current;
#endif
    public int Current => _current;

    public bool MoveNext()
    {
        _current++;
        return _current <= _end;
    }
}