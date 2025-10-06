using System;
using System.Collections.Generic;

namespace CardValidation.Core.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();

        // ✅ Parameterless constructor
        public ValidationResult() { }

        // ✅ Optional constructor for convenience
        public ValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors ?? new List<string>();
        }
    }
}