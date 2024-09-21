namespace GeneralEntries.Helpers.Response
{
    public static class ApiMessages
    {
        public const string UnauthorizedUser = "Error : User Info not found. The User is no authorize!";
        public const string InvalidInput = "Error : Invalid input!";
        public const string NotFound = "Error : No record found!";
        public const string AccountIdNull = "Error : Account Id is null!";
        public const string RecordIsExist = "Error : The record already exist!";
        public const string SuccessUpdate = "The record was successfully updated!";
        public const string SuccessDelete = "The record was successfully deleted!";
        public const string SuccessCreate = "The record was successfully created!";
        public const string FailedUpdate = "Error: Unable to update!";
        public const string FailedDelete = "Error: Unable to delete, record in use!";



        public const string Error_InternalError = "The server encountered an internal error!";
        public const string Error_ValueGreaterThan = "Please enter a value bigger than {1} !";
        public const string Error_ValueBetween = "The value must be between {1} and {2} !";
        public const string Error_HubAccess = "You dont have access to this HUB!";
        public const string Error_HubEnd = "The Exam did not get started yet. you cannot end it!";
        public const string Error_HubExpire = "The exam is expired. The endtime is bigger than exam Endtime!";
        public const string Error_ReviewHidden = "Review is already hidden.";
        public const string Error_HideNotAllow = "Unauthorize. You are not the author for this review.";
    }
}
