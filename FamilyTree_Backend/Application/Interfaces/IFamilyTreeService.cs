using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IFamilyTreeService
    {

        public Task<FamilyTreeModel> FindFamilyTree(long treeId);

        public Task<FamilyTreeUpdateResponseModel> UpdateFamilyTree(long treeId, FamilyTreeInputModel model);

        public Task DeleteFamilyTree(long treeId);

        public Task<FamilyTreeModel> AddFamilyTree(FamilyTreeInputModel model, ClaimsPrincipal user);

        public Task<IEnumerable<FamilyTreeListItemModel>> FindAllTree();
        public Task<IEnumerable<FamilyTreeListItemModel>> FindAllTreeAccessibleToUser(ClaimsPrincipal user);
        public Task<IEnumerable<string>> AddUsersToEditor(long treeId, IList<string> userNames);
        public Task<IEnumerable<string>> RemoveUsersFromEditor(long treeId, IList<string> userNames);
        public FamilyTreeContributorsModel GetTreeEditors(long treeId);

    }
}
