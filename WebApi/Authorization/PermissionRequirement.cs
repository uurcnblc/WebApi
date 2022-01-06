﻿using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization
{
    internal class PermissionRequirement:IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
