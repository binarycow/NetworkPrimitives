#nullable enable

using System;

namespace NetworkPrimitives
{
    /// <summary>
    /// Provides build information
    /// </summary>
    public class BuildInformationAttribute : Attribute
    {
        /// <summary>
        /// The git commit hash
        /// </summary>
        public string? CommitHash { get; }
        /// <summary>
        /// The timestamp when this build was built
        /// </summary>
        public string? BuildTimestamp { get; }
        /// <summary>
        /// The unique build number.
        /// </summary>
        public string? BuildNumber { get; }
        
        /// <summary>
        /// Branch name this build was created from
        /// </summary>
        public string? BranchName { get; }

        /// <summary>
        /// Create an instance of <see cref="BuildInformationAttribute"/>
        /// </summary>
        /// <param name="commitHash">The git commit hash</param>
        /// <param name="buildTimestamp">The timestamp when this build was built</param>
        /// <param name="buildNumber">The unique build number.</param>
        /// <param name="branchName">Branch name this build was created from</param>
        public BuildInformationAttribute(
            string? commitHash = null,
            string? buildTimestamp = null,
            string? buildNumber = null,
            string? branchName = null
        )
        {
            this.CommitHash = commitHash;
            this.BuildTimestamp = buildTimestamp;
            this.BuildNumber = buildNumber;
            this.BranchName = branchName;
        }
    }
}