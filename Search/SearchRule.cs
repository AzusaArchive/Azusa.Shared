using System;
using System.Collections.Generic;

namespace Azusa.Shared.Search;

/// <summary>
/// 通用的API分页/排序查询条件
/// </summary>
[Serializable]
public class SearchRule
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Keyword { get; set; }
    public string? Sorting { get; set; }
    public bool Descending { get; set; }

    public SearchRule(int? skip = 0, int? take = 10, string? keyword = null, string? sorting = null, bool descending = false)
    {
        Skip = skip is -1 ? null : skip;
        Take = take is -1 ? null : take;
        Keyword = keyword;
        Sorting = sorting;
        Descending = descending;
    }
}
/// <summary>
/// 通用的API分页/排序查询条件，带有多字段排序属性
/// </summary>
[Serializable]
public class SearchRuleMultiSorting
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Keyword { get; set; }
    public IEnumerable<string>? SortingProperties { get; set; }
    public bool Descending { get; set; }

    public SearchRuleMultiSorting(int? skip = 0, int? take = 10, string? keyword = null, IEnumerable<string>? sortingProperties = null, bool descending = false)
    {
        Skip = skip is -1 ? null : skip;
        Take = take is -1 ? null : take;
        Keyword = keyword;
        SortingProperties = sortingProperties;
        Descending = descending;
    }
}