using FirebaseAdmin.Auth;

namespace AltermedManager.Helpers
    {
    public class FirebaseTokenValidator
        {
        public static async Task<string?> VerifyTokenAsync(string idToken)
            {
            try
                {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken.Uid;
                }
            catch (Exception ex)
                {
                Console.WriteLine("Firebase token verification failed: " + ex.Message);
                return null;
                }
            }
        }
    }
