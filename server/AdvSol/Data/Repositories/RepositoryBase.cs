using AdvSol.Services.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdvSol.Data.Repositories
{
    public interface IRepositoryBase<TEntity> 
        where TEntity : class
    {
        Task<PagedDto<TOutput>> Page<TInput, TOutput>(IQueryable<TInput> list, int pageSize, int pageNumber, string orderBy, string orderDir);
    }
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {
        protected DbSet<TEntity> DbSet { get; private set; }

        protected ICurrentUser _currentUser;

        protected AppDbContext DbContext { get; private set; }

        protected IMapper Mapper { get; private set; }

        public RepositoryBase(AppDbContext dbContext, IMapper mapper, ICurrentUser currentUser)
        {
            Mapper = mapper;
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
            _currentUser = currentUser;
        }

        public async Task<PagedDto<TOutput>> Page<TInput, TOutput>(IQueryable<TInput> list, int pageSize, int pageNumber, string orderBy, string direction = "")
        {
            var totalRecords = list.Count();

            if (pageNumber <= 0) pageNumber = 1;

            var pagedList = list.DynamicOrderBy($"{orderBy} {direction}") as IQueryable<TInput>;

            if (pageSize > 0)
            {
                var skipRecordCount = (pageNumber - 1) * pageSize;
                pagedList = pagedList.Skip(skipRecordCount)
                    .Take(pageSize);
            }

            var result = await pagedList.ToListAsync();

            IEnumerable<TOutput> outputList;

            if (typeof(TOutput) != typeof(TInput))
                outputList = Mapper.Map<IEnumerable<TInput>, IEnumerable<TOutput>>(result);
            else
                outputList = (IEnumerable<TOutput>)result;

            var pagedDTO = new PagedDto<TOutput>
            {
                SourceList = outputList,
                PageInfo = new PageInfo {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalRecords,
                    OrderBy = orderBy,
                    Direction = direction,
                    ItemCount = outputList.Count()
                }
            };

            return pagedDTO;
        }
    }
}
