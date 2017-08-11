namespace Collectively.Services.Mailing.Domain
{
    public static class EmailTemplates
    {
        public static string ResetPassword => "reset_password";
        public static string ActivateAccount => "activate_account";
        public static string RemarkCreated => "remark_created";
        public static string RemarkStateChanged => "remark_state_changed";
        public static string PhotosAddedToRemark => "photos_added_to_remark";
        public static string CommentAddedToRemark => "comment_added_to_remark";
    }
}