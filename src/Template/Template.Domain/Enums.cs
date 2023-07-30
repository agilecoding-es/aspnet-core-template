using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain
{
    /// <summary>
    /// Acceptable validation summary rendering modes.
    /// </summary>
    public enum ValidationMessage
    {
        /// <summary>
        /// No validation summary.
        /// </summary>
        None,

        /// <summary>
        /// Validation summary with model-level errors only (excludes all property errors).
        /// </summary>
        Errors,

        /// <summary>
        /// Validation summary with model-level validations only (excludes all property errors).
        /// </summary>
        Validations,

        /// <summary>
        /// Validation summary with all errors.
        /// </summary>
        All
    }
}
