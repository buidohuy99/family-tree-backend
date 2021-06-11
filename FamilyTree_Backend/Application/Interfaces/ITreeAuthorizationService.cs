using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface ITreeAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, long treeId, IAuthorizationRequirement requirements);
        public Task<AuthorizationResult> AuthorizeWithPersonAsync(ClaimsPrincipal user, long personId, IAuthorizationRequirement requirements);
        public Task<AuthorizationResult> AuthorizeWithEventAsync(ClaimsPrincipal user, long eventId, IAuthorizationRequirement requirements);
        public Task<AuthorizationResult> AuthorizeWithMemoryAsync(ClaimsPrincipal user, long memoryId, IAuthorizationRequirement requirements);
    }
}
