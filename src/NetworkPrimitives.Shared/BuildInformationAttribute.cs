#nullable enable

using System;

namespace NetworkPrimitives
{
    public class BuildInformationAttribute : Attribute
    {
        public string? CommitHash { get; }
        public string? BuildTimestamp { get; }
        public string? BuildNumber { get; }
        public string? BranchName { get; }

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