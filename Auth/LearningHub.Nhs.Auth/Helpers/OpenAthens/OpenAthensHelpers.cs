// <copyright file="OpenAthensHelpers.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Helpers.OpenAthens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LearningHub.Nhs.Models.OpenAthens;

    /// <summary>
    /// The open athens helpers.
    /// </summary>
    internal static class OpenAthensHelpers
    {
        /// <summary>
        /// The extract open athens props.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="payload">
        /// The payload.
        /// </param>
        internal static void ExtractOpenAthensProps(this BeginOpenAthensLinkToLearningHubUser model, OpenAthensAuthServerPayload payload)
        {
            if (!payload.Claims.Any())
            {
                return;
            }

            model.OaUserId = payload.Claims["eduPersonTargetedID"];
            model.OaOrganisationId = payload.Claims["eduPersonScopedAffiliation"];
        }
    }
}
