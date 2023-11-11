namespace Azusa.Shared.Search;

public static class SearchRuleExtensions
{
    public static IQueryable<TEntity> SearchByRule<TEntity>(this IQueryable<TEntity> entities,
        SearchRule? rule = null)
    {
        if (rule is null)
            return entities;

        if (!string.IsNullOrWhiteSpace(rule.Keyword))
        {
            var filter = SearchRuleHelper.BuildKeywordSearchExpression<TEntity>(rule.Keyword, true);
            entities = entities.Where(filter);
        }

        if (!string.IsNullOrWhiteSpace(rule.Sorting))
        {
            var filter = SearchRuleHelper.BuildSortingExpression<TEntity>(rule.Sorting);
            entities = rule.Descending ? entities.OrderByDescending(filter) : entities.OrderBy(filter);
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

    public static IEnumerable<TEntity> SearchByRule<TEntity>(this IEnumerable<TEntity> entities,
        SearchRule? rule = null)
    {
        if (rule is null)
            return entities;

        if (!string.IsNullOrWhiteSpace(rule.Keyword))
        {
            var filter = SearchRuleHelper.BuildKeywordSearchExpression<TEntity>(rule.Keyword, true);
            entities = entities.Where(filter.Compile());
        }

        if (!string.IsNullOrWhiteSpace(rule.Sorting))
        {
            var filter = SearchRuleHelper.BuildSortingExpression<TEntity>(rule.Sorting);
            entities = rule.Descending ? entities.OrderByDescending(filter.Compile()) : entities.OrderBy(filter.Compile());
        }

        if (rule.Skip is not null and not -1)
        {
            entities = entities.Skip(rule.Skip.Value);
        }

        if (rule.Take is not null and not -1)
        {
            entities = entities.Take(rule.Take.Value);
        }

        return entities;
    }
}