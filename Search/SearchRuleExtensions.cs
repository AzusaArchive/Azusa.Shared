namespace Azusa.Shared.Search;

public static class SearchRuleExtensions
{
    public static IEnumerable<TEntity> SearchByRule<TEntity>(this IEnumerable<TEntity> entities,
        SearchRule? rule = null)
    {
        if (rule is null)
            return entities;

        if (!string.IsNullOrWhiteSpace(rule.Keyword))
        {
            var filter = SearchRuleHelper.BuildKeywordSearchExpression<TEntity>(rule.Keyword);
            entities = entities.Where(filter.Compile());
        }

        if (!string.IsNullOrWhiteSpace(rule.Sorting))
        {
            var filter = SearchRuleHelper.BuildSortingExpression<TEntity>(rule.Sorting);
            entities = rule.Descending ? entities.OrderByDescending(filter.Compile()) : entities.OrderBy(filter.Compile());
        }

        if (rule.Skip is not null)
        {
            entities = entities.Skip(rule.Skip.Value);
        }

        if (rule.Take is not null)
        {
            entities = entities.Take(rule.Take.Value);
        }

        return entities;
    }
}