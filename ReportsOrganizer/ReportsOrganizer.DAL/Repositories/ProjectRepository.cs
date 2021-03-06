﻿using System;
using ReportsOrganizer.DAL.Abstractions;
using ReportsOrganizer.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportsOrganizer.DAL.Repositories
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<Project> FindById(int id, CancellationToken cancellationToken);
        IQueryable<Project> FindByShortName(string shortName);
        Task<IEnumerable<Project>> ToListAsync(CancellationToken cancellationToken);
    }

    internal class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepository(ApplicationDbContext dbContext) : base(dbContext)
            => _dbContext = dbContext;

        public override Task AddAsync(Project entity, CancellationToken cancellationToken)
        {
            entity.ShortName = entity.ShortName.ToUpper();
            return base.AddAsync(entity, cancellationToken);
        }

        public override Task AddOrUpdateAsync(Project entity, CancellationToken cancellationToken)
        {
            entity.ShortName = entity.ShortName.ToUpper();
            return base.AddOrUpdateAsync(entity, cancellationToken);
        }

        public async Task<Project> FindById(int id, CancellationToken cancellationToken)
            => await _dbContext.Projects.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public IQueryable<Project> FindByShortName(string normalizedShortName)
            => _dbContext.Projects.Where(property => property.ShortName == normalizedShortName);

        public async Task<IEnumerable<Project>> ToListAsync(CancellationToken cancellationToken)
            => await _dbContext.Projects.ToListAsync(cancellationToken);
    }
}
