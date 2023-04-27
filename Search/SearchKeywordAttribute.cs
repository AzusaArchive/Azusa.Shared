using System;

namespace Azusa.Shared.Search;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class SearchKeywordAttribute : Attribute
{
    
}