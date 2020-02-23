﻿namespace Grpc.Dotnet.Shared.Helpers.Authorization
{
    using System;
    using Grpc.Dotnet.Permissions.V1;
    using Grpc.Dotnet.Shared.Helpers.Rpc.Client;
    using Microsoft.Extensions.Logging;

    public class AuthorizeService : IAuthorizeService
    {
        private readonly IServiceClient<PermissionsService.PermissionsServiceClient> permissionsClient;
        private readonly ILogger<AuthorizeService> logger;

        public AuthorizeService(IServiceClient<PermissionsService.PermissionsServiceClient> permissionsClient, ILogger<AuthorizeService> logger)
        {
            this.permissionsClient = permissionsClient;
            this.logger = logger;
        }

        public bool IsCurrentUserAuthorized(string permission)
        {
            try
            {
                var request = new IsUserAllowedRequest
                {
                    UserId = "1db87eca-faab-402d-ac00-3e9f82a8c838", //get user id from UserContext,
                    Permission = permission
                };

                var isUserAllowed = this.permissionsClient.Execute<IsUserAllowedRequest, IsUserAllowedResponse>(c => c.IsUserAllowed, request);

                return isUserAllowed.IsAllowed;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error on fetching user permissions.");
                return false;
            }
        }
    }
}