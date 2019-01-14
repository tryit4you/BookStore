using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories
{
    public class p_CommentsRepository : I_p_CommentsRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public p_CommentsRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<P_Comment> AllComments()=>_dbContext.P_Comments.ToList();

        public IEnumerable<P_Comment> AllCommentsByBookId(string id) => _dbContext.P_Comments.Where(x => x.eBookId == id).OrderBy(x=>x.CreatedDate).ToList();

        public bool CreatedComment(P_Comment p_Comments) {
            var result = _dbContext.P_Comments.Add(p_Comments).Entity;
            if (result != null)
            {

                return true;
                
            }
            else
                return false;
        }

        public P_Comment getComment(string id) => _dbContext.P_Comments.FirstOrDefault(x=>x.p_id==id);

        public void SaveChange() => _dbContext.SaveChanges();
    }
}
