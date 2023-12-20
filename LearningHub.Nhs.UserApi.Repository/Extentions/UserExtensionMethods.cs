// <copyright file="UserExtensionMethods.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Extentions
{
    using System;
    using System.Linq;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Shared;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The user extension methods.
    /// </summary>
    public static class UserExtensionMethods
    {
        /// <summary>
        /// The include collections.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The included column is not in the internal list.
        /// </exception>
        public static IQueryable<User> IncludeCollections(this IQueryable<User> query, UserIncludeCollectionsEnum[] includes)
        {
            if (includes == null)
            {
                return query;
            }

            for (var i = 0; i < includes.Length; i++)
            {
                query = includes[i] switch
                {
                    UserIncludeCollectionsEnum.UserUserGroup => query.Include(x => x.UserUserGroup),
                    UserIncludeCollectionsEnum.UserPasswordValidationToken => query.Include(x => x.UserPasswordValidationToken),
                    UserIncludeCollectionsEnum.UserEmployment => query.Include(x => x.UserEmployment),
                    UserIncludeCollectionsEnum.EmailLog => query.Include(x => x.EmailLog),
                    UserIncludeCollectionsEnum.LoginWizardStageActivity => query.Include(x => x.LoginWizardStageActivity),
                    UserIncludeCollectionsEnum.UserTermsAndConditions => query.Include(x => x.UserTermsAndConditions),
                    UserIncludeCollectionsEnum.UserSecurityQuestion => query.Include(x => x.UserSecurityQuestion),
                    UserIncludeCollectionsEnum.UserAttributes => query.Include(x => x.UserAttributes).ThenInclude(x => x.Attribute),
                    UserIncludeCollectionsEnum.UserRoleUpgrade => query.Include(x => x.UserRoleUpgrade),
                    _ => throw new ArgumentOutOfRangeException(includes[i].ToString()),
                };
            }

            return query;
        }
    }
}