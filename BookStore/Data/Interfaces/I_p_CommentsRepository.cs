using BookStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Interfaces
{
    public interface I_p_CommentsRepository
    {
        P_Comment getComment(string id);
        IEnumerable<P_Comment> AllComments();
        IEnumerable<P_Comment> AllCommentsByBookId(string id);
        bool CreatedComment(P_Comment p_Comments);
        void SaveChange();
    }
}
