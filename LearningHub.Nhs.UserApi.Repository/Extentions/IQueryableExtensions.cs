namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.EntityFrameworkCore.Query.Internal;

    /// <summary>
    /// IQueryableExtensions.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// CountWithNoLock.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="expression">Expression.</param>
        /// <returns>Count.</returns>
        public static int CountWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            var strategy = GetDbContext(query).Database.CreateExecutionStrategy();
            return strategy.Execute(
                () =>
                {
                    using var scope = CreateTransaction();
                    if (expression is object)
                    {
                        query = query.Where(expression);
                    }

                    int result = query.Count();
                    scope.Complete();
                    return result;
                });
        }

        /// <summary>
        /// CountWithNoLockAsync.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">cancellationToken.</param>
        /// <returns>Count.</returns>
        public static async Task<int> CountWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var strategy = GetDbContext(query).Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(
                async () =>
                {
                    using var scope = CreateTransactionAsync();
                    if (expression is object)
                    {
                        query = query.Where(expression);
                    }

                    int result = await query.CountAsync(cancellationToken);
                    scope.Complete();
                    return result;
                });
        }

        /// <summary>
        /// FirstOrDefaultWithNoLock.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="expression">Expression.</param>
        /// <returns>First or default object.</returns>
        public static T FirstOrDefaultWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            var strategy = GetDbContext(query).Database.CreateExecutionStrategy();
            return strategy.Execute(
                () =>
                {
                    using var scope = CreateTransaction();
                    if (expression is object)
                    {
                        query = query.Where(expression);
                    }

                    T result = query.FirstOrDefault();
                    scope.Complete();
                    return result;
                });
        }

        /// <summary>
        /// FirstOrDefaultWithNoLockAsync.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>First or default object.</returns>
        public static async Task<T> FirstOrDefaultWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var strategy = GetDbContext(query).Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(
                async () =>
                {
                    using var scope = CreateTransactionAsync();
                    if (expression is object)
                    {
                        query = query.Where(expression);
                    }

                    T result = await query.FirstOrDefaultAsync(cancellationToken);
                    scope.Complete();
                    return result;
                });
        }

        /// <summary>
        /// ToListWithNoLock..
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="expression">Expression.</param>
        /// <returns>List of object.</returns>
        public static List<T> ToListWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            var strategy = GetDbContext(query).Database.CreateExecutionStrategy();
            return strategy.Execute(
                () =>
                {
                    List<T> result = default;
                    using (var scope = CreateTransaction())
                    {
                        if (expression is object)
                        {
                            query = query.Where(expression);
                        }

                        result = query.ToList();
                        scope.Complete();
                    }

                    return result;
                });
        }

        /// <summary>
        /// ToListWithNoLockAsync.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>List of object.</returns>
        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var strategy = GetDbContext(query).Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(
                async () =>
                {
                    List<T> result = default;
                    using (var scope = CreateTransactionAsync())
                    {
                        if (expression is object)
                        {
                            query = query.Where(expression);
                        }

                        result = await query.ToListAsync(cancellationToken);
                        scope.Complete();
                    }

                    return result;
                });
        }

        private static TransactionScope CreateTransactionAsync()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled);
        }

        private static TransactionScope CreateTransaction()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted });
        }

        private static DbContext GetDbContext(IQueryable query)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var queryCompiler = typeof(EntityQueryProvider).GetField("_queryCompiler", bindingFlags).GetValue(query.Provider);
            var queryContextFactory = queryCompiler.GetType().GetField("_queryContextFactory", bindingFlags).GetValue(queryCompiler);
            var dependencies = typeof(RelationalQueryContextFactory).GetProperty("Dependencies", bindingFlags).GetValue(queryContextFactory);
            var queryContextDependencies = typeof(DbContext).Assembly.GetType(typeof(QueryContextDependencies).FullName);
            var stateManagerProperty = queryContextDependencies.GetProperty("StateManager", bindingFlags | BindingFlags.Public).GetValue(dependencies);
            var stateManager = (IStateManager)stateManagerProperty;

            return stateManager.Context;
        }
    }
}
