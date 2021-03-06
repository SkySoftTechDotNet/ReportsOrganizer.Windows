﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading;
using System.Threading.Tasks;

namespace ReportsOrganizer.DAL.Extensions
{
    internal static class EntityExtensions
    {
        public static async Task AddAsync<TEntity>(this DbSet<TEntity> entities, TEntity entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            await Task.Run(() =>
            {
                entities.Add(entity);
            }, cancellationToken);
        }

        public static Task AddAsync<TEntity>(this ApplicationDbContext dbContext, TEntity entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            return AddAsync(dbContext.Set<TEntity>(), entity, cancellationToken);
        }

        public static async Task AddOrUpdateAsync<TEntity>(this DbSet<TEntity> entities, TEntity entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            await Task.Run(() =>
            {
                entities.AddOrUpdate(entity);
            }, cancellationToken);
        }

        public static Task AddOrUpdateAsync<TEntity>(this ApplicationDbContext dbContext, TEntity entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            return AddOrUpdateAsync(dbContext.Set<TEntity>(), entity, cancellationToken);
        }

        public static async Task DeleteAsync<TEntity>(this DbSet<TEntity> entities, TEntity entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            await Task.Run(() =>
            {
                entities.Remove(entity);
            }, cancellationToken);
        }

        public static Task DeleteAsync<TEntity>(this ApplicationDbContext dbContext, TEntity entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            return DeleteAsync(dbContext.Set<TEntity>(), entity, cancellationToken);
        }

        public static async Task DeleteAsync<TEntity>(this DbSet<TEntity> entities, IEnumerable<TEntity> entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            await Task.Run(() =>
            {
                entities.RemoveRange(entity);
            }, cancellationToken);
        }

        public static Task DeleteAsync<TEntity>(this ApplicationDbContext dbContext, IEnumerable<TEntity> entity,
            CancellationToken cancellationToken) where TEntity : class
        {
            return DeleteAsync(dbContext.Set<TEntity>(), entity, cancellationToken);
        }
    }
}
