#region Copyright
//
// Copyright(C) Eric Singh., 2017.
//
#endregion

namespace Bio.Library
{
    /// <summary>
    /// Possible messages from the client. Can be expanded
    /// </summary>
    public static class ClientMessageConstants
    {
        /// <summary>
        /// HELO
        /// </summary>
        public const string Helo = "HELO";

        /// <summary>
        /// COUNT
        /// </summary>
        public const string Count = "COUNT";

        /// <summary>
        /// CONNECTIONS
        /// </summary>
        public const string Connections = "CONNECTIONS";

        /// <summary>
        /// PRIME
        /// </summary>
        public const string Prime = "PRIME";

        /// <summary>
        /// TERMINATE
        /// </summary>
        public const string Terminate = "TERMINATE";
    }
}
