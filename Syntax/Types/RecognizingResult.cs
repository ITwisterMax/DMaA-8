using System;

namespace Syntax.Types
{
    /// <summary>
    ///     Recognazing result class
    /// </summary>
    public class RecognizingResult
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="errorElementName">Error element name</param>
        /// <param name="isImage">Is man</param>
        public RecognizingResult(string errorElementName, bool isImage)
        {
            ErrorElementName = errorElementName;
            IsImage = isImage;
        }

        /// <summary>
        ///     Error element name
        /// </summary>
        public String ErrorElementName { get; set; }

        /// <summary>
        ///     Is man
        /// </summary>
        public bool IsImage { get; set; }
    }
}