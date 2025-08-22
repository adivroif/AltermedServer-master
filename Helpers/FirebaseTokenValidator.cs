using FirebaseAdmin.Auth;

namespace AltermedManager.Helpers
    {
    public class FirebaseTokenValidator
        {
        private readonly ILogger<FirebaseTokenValidator> _logger;

        public FirebaseTokenValidator(ILogger<FirebaseTokenValidator> logger)
            {
            _logger = logger;
            }
        public static async Task<string?> VerifyTokenAsync(string idToken)
            {
            try
                {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken.Uid;
                }
            catch (Exception ex)
                {                
                return null;
                }
            }
        }
    }
