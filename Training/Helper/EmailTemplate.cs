namespace Training.Helper
{
    public class EmailTemplate
    {
        public static string GetConfirmationEmailSubject()
        {
            return "Confirm your email";
        }

        public static string GetConfirmationEmailBody(string confirmationLink)
        {
            return $@"
                <h2>Welcome!</h2>
                <p>Please confirm your email by clicking the link below:</p>
                <a href=""{confirmationLink}"">Confirm Email</a>
            ";
        }
    }
}
