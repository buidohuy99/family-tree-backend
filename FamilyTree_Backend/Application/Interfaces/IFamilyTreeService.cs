using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Application.Models.FileIO;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IFamilyTreeService
    {
        //CRUD
        public Task<FamilyTreeModel> FindFamilyTree(long treeId);

        public Task<FamilyTreeUpdateResponseModel> UpdateFamilyTree(long treeId, FamilyTreeInputModel model);

        public Task DeleteFamilyTree(long treeId);

        public Task<FamilyTreeModel> AddFamilyTree(FamilyTreeInputModel model, ClaimsPrincipal user);

        public Task<IEnumerable<FamilyTreeListItemModel>> FindAllTree();
        public Task<FindTreesPaginationResponseModel> FindAllTree(PaginationModel model);
        public Task<IEnumerable<FamilyTreeListItemModel>> FindAllTreeAccessibleToUser(ClaimsPrincipal user);
        public Task<FindTreesPaginationResponseModel> FindAllTreeAccessibleToUser(ClaimsPrincipal user, PaginationModel model);

        public Task<IEnumerable<FamilyTreeListItemModel>> FindTreesFromKeyword(string keyword);
        public Task<FindTreesPaginationResponseModel> FindTreesFromKeyword(string keyword, PaginationModel model);
        public Task<IEnumerable<FamilyTreeListItemModel>> FindTreesFromKeywordAccessibleToUser(ClaimsPrincipal user, string keyword);
        public Task<FindTreesPaginationResponseModel> FindTreesFromKeywordAccessibleToUser(ClaimsPrincipal user, string keyword, PaginationModel model);

        //Editors
        public Task<IEnumerable<string>> AddUsersToEditor(long treeId, IList<string> userNames);
        public Task<IEnumerable<string>> RemoveUsersFromEditor(long treeId, IList<string> userNames);
        public FamilyTreeContributorsModel GetTreeEditors(long treeId);

        //Import-export
        public Task<FamilyTreeModel> ImportFamilyTree(FamilyTreeImportModel model, ClaimsPrincipal user);
        public Task<(string treeName, string payload)> ExportFamilyTreeJson(long treeId, bool isForBackup);
    }
}
