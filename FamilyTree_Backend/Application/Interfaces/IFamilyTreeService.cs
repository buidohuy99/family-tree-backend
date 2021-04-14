using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IFamilyTreeService
    {

        public Task<FamilyTreeModel> FindFamilyTree(long treeId);

    }
}
