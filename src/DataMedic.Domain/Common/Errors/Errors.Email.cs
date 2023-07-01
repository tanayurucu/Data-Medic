using ErrorOr;

using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class Email
    {
        public static Error IdRequired =>
            Error.Validation(code: "Email.IdRequired", description: "Email ID is required.");
        public static Error NotFound =>
            Error.NotFound(code: "Email.NotFound", description: "Email not found.");

        public static Error AlreadyExists =>
            Error.Conflict(
                code: "Email.AlreadyExists",
                description: "Email already exists." 
            );

        public static class Address
        {
            public static Error Empty =>
                Error.Validation(
                    code: "Email.Address.Empty",
                    description: "Email address empty" 
                );

            public static Error TooLong =>
                Error.Validation(
                    code: "Email.Address.TooLong",
                    description: "Email address too long."
                );

            public static Error Invalid =>
                Error.Validation(
                    code: "Email.Address.Invalid",
                    description: "Email address invalid" 
                );
        }
    }
}