namespace Template.Application.Errors
{
    public static class ValidationErrors
    {
        public static class Shared
        {
            /// <summary>
            /// The list of {0} is empty.
            /// </summary>
            public static readonly string EmptyList = "Validations_Shared_EmptyList";

            /// <summary>
            /// The entity {0} could not be found.
            /// </summary>
            public static readonly string EntityNotFound = "Validations_Shared_EntityNotFound";
        }

        public static class Sample
        {
            public static class GetSampleListById
            {
                /// <summary>
                /// The name of the list already exist.
                /// </summary>
                public static readonly string ListWithSameNameAlreadyExists = "Validations_Sample_GetSampleListById_ListWithSameNameAlreadyExists";
            }
        }
    }
}
