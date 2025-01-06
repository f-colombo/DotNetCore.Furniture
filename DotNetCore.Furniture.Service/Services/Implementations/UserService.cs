using DotNetCore.Furniture.Data.Repositories.Interfaces;
using DotNetCore.Furniture.Domain.Common.Generics;
using DotNetCore.Furniture.Domain.DataTransferObjects;
using DotNetCore.Furniture.Domain.Entities;
using DotNetCore.Furniture.Service.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCore.Furniture.Service.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;
        private const string CacheKeyPrefix = "VerificationCode_";
        private readonly IMemoryCache cache;
        private readonly string jwtSecret = "hjejehukkehheukndhuywuiuwjbncduhbwiubdvuwyveyduwivuyegvryefrebuhjwbfjweuhbwllo"; // Change this to a secure secret key
        private readonly double jwtExpirationMinutes = 60; // Token expiration time in minutes
        private readonly ILogger logger;

        public UserService(IUserRepository userRepository, IEmailService emailService, IMemoryCache cache, ILogger logger)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.cache = cache;
            this.logger = logger;
        }

        // private const string SessionKeyPrefix = "VerificationCode_";
        public async Task<NewResult<User>> RegisterUser(User user)
        {
            // Hash the password 
            user.Password = HashPassword(user.Password);
            NewResult<User> result = new NewResult<User>();
            try
            {
                var userExists = await userRepository.GetUserByEmail(user.EmailAddress);
                if (userExists != null)
                {
                    return NewResult<User>.Duplicate(null, "Email address unavailable");
                }
                var response = await userRepository.RegisterUser(user);
                if (response == null)
                {
                    result = NewResult<User>.Failed(null, "Failed");
                }

                var newResponse = new User()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    isActivated = false,
                    Role = user.Role
                };
                // Generate activation code
                string verificationCode = GenerateVerificationCode();

                // Store the verification code temporarily (example: in session or cache)
                StoreVerificationCodeTemporarily(user.EmailAddress, verificationCode);

                //// Construct activation link
                //// string activationLink = $"https://yourdomain.com/activate?token={verificationCode}";

                // Send activation email with the activation link
                await emailService.SendActivationEmail(user.EmailAddress, verificationCode);
                result = NewResult<User>.Success(newResponse, "User registration successful");
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<NewResult<User>> ActivateAccount(string emailAddress, string activationCode)
        {
            NewResult<User> result = new NewResult<User>();
            try
            {
                // Retrieve the stored verification code for the given email address
                string storedVerificationCode = RetrieveVerificationCode(emailAddress);

                // Check if the verification code is found in the cache
                if (string.IsNullOrEmpty(storedVerificationCode))
                {
                    // If the verification code is not found, it means it has expired or does not exist
                    return NewResult<User>.Failed(null, "Verification code not found or expired.");
                }

                // Check if the verification code matches the one provided by the user
                if (storedVerificationCode != activationCode)
                {
                    // If the verification code does not match, return a failure result
                    return NewResult<User>.Failed(null, "Invalid verification code.");
                }

                // Proceed with account activation
                // Retrieve the user by email address
                var user = await userRepository.GetUserByEmail(emailAddress);
                if (user is not null)
                {
                    // Activate the user account (example: set IsActive flag to true)
                    user.isActivated = true;

                    // Update the user in the database
                    await userRepository.UpdateUserActivationStatus(emailAddress, user.isActivated);

                    // Optionally, you may want to remove the verification code from storage
                    RemoveVerificationCodeFromCache(emailAddress);

                    // Return a success result
                    return NewResult<User>.Success(null, "Account activated successfully.");
                }
                else
                {
                    // If the user is not found, return a failure result
                    return NewResult<User>.Failed(null, "User not found.");
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs during the activation process, return a failure result
                return NewResult<User>.Error(null, $"Error activating account: {ex.Message}");
            }
        }

        public async Task<NewLoginResult<User>> UserLogin(LoginRequest loginRequest)
        {
            NewLoginResult<User> result = new NewLoginResult<User>();
            try
            {
                var user = await userRepository.GetUserByEmail(loginRequest.EmailAddress);
                if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
                {
                    //throw new Exception("Invalid email or password.");
                    return NewLoginResult<User>.Failed(null, "Invalid email or password");
                }

                if (!user.isActivated)
                {
                    return NewLoginResult<User>.Failed(null, "Account is not activated");
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);
                result = NewLoginResult<User>.Success(user, token, "Login successful");
                return result;



            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return NewLoginResult<User>.Error(null, "An error occured while trying to login");
            }
        }

        private string GenerateVerificationCode()
        {
            // Generate a random 6-digit verification code
            Random random = new Random();
            int verificationCode = random.Next(100000, 999999);
            return verificationCode.ToString();
        }

        public void StoreVerificationCodeTemporarily(string emailAddress, string verificationCode)
        {
            // Generate a unique cache key for the verification code
            string cacheKey = $"{CacheKeyPrefix}{emailAddress}";

            // Store the verification code in the MemoryCache with a sliding expiration time
            cache.Set(cacheKey, verificationCode, TimeSpan.FromMinutes(10));
        }

        public string RetrieveVerificationCode(string emailAddress)
        {
            // Generate the cache key for the given email address
            string cacheKey = $"{CacheKeyPrefix}{emailAddress}";

            // Retrieve the verification code from the MemoryCache
            return cache.Get<string>(cacheKey);
        }

        private string HashPassword(string password)
        {
            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            // Verify the entered password against the hashed password using BCrypt
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, user.Id),
                // You can add more claims here as needed
            }),
                Expires = DateTime.UtcNow.AddMinutes(jwtExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateAdminJwtToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, admin.AdminId),
                // You can add more claims here as needed
            }),
                Expires = DateTime.UtcNow.AddMinutes(jwtExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<NewResult<string>> ResetPassword(string emailAddress, string verificationCode, string newPassword)
        {
            NewResult<string> result = new NewResult<string>();
            try
            {
                // Step 1: Check if the email exists
                if (await userRepository.EmailExists(emailAddress))
                {
                    // Step 2: Generate and send verification code to the email
                    string generatedVerificationCode = GenerateVerificationCode();
                    await emailService.SendActivationEmail(emailAddress, generatedVerificationCode);

                    // Step 3: Verify verification code
                    if (await VerifyVerificationCode(emailAddress, verificationCode))
                    {
                        // Step 4: Proceed and reset password
                        await userRepository.ResetPassword(emailAddress, newPassword);
                        return NewResult<string>.Success(null, "Password reset successfully");
                    }
                    else
                    {
                        return NewResult<string>.Failed(null, "Invalid verification code");
                    }
                }
                else
                {
                    return NewResult<string>.Failed(null, "Email address does not exist");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> VerifyVerificationCode(string emailAddress, string verificationCode)
        {
            // Retrieve the stored verification code from the cache
            string storedVerificationCode = RetrieveVerificationCode(emailAddress);

            // Check if the verification code matches
            return storedVerificationCode == verificationCode;
        }

        private void RemoveVerificationCodeFromCache(string emailAddress)
        {
            // Generate the cache key for the given email address
            string cacheKey = $"{CacheKeyPrefix}{emailAddress}";

            // Remove the verification code from the cache
            cache.Remove(cacheKey);
        }

        public async Task<NewResult<string>> InitiatePasswordReset(string emailAddress)
        {
            try
            {
                // Step 1: Check if the email exists
                if (await userRepository.EmailExists(emailAddress))
                {
                    // Step 2: Generate a verification code
                    string generatedVerificationCode = GenerateVerificationCode();

                    // Step 3: Send the verification code to the user's email
                    await emailService.SendActivationEmail(emailAddress, generatedVerificationCode);

                    // Step 4: Return success response with the generated verification code
                    return NewResult<string>.Success(generatedVerificationCode, "Verification code sent to your email.");
                }
                else
                {
                    // Email address does not exist, return failure response
                    return NewResult<string>.Failed(null, "Email address does not exist");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<NewResult<Admin>> RegisterAdmin(Admin admin)
        {
            try
            {
                // Hash the password 
                admin.Password = HashPassword(admin.Password);
                NewResult<Admin> result = new NewResult<Admin>();

                var AdminExists = await userRepository.GetAdminByEmail(admin.EmailAddress);
                if (AdminExists != null)
                {
                    return NewResult<Admin>.Duplicate(null, "Email address unavailable");
                }

                // Download the image data as byte array before creating the Admin object
                byte[] profilePictureData = await DownloadImageAsByteArray(admin.ProfilePictureUrl);

                // Instantiate the Admin object with the profile picture byte array
                Admin aadmin = new Admin
                {
                    AdminId = admin.AdminId,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    EmailAddress = admin.EmailAddress,
                    Password = admin.Password,
                    ProfilePictureUrl = admin.ProfilePictureUrl,
                    AdminLoginId = "A86478927",
                    isActivated = admin.isActivated,
                    Role = admin.Role
                };


                var response = await userRepository.RegisterAdmin(aadmin);
                if (response == null)
                {
                    result = NewResult<Admin>.Failed(null, "Failed");
                }

                var newResponse = new Admin()
                {
                    AdminId = admin.AdminId,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    EmailAddress = admin.EmailAddress,
                    Password = admin.Password,
                    ProfilePictureUrl = admin.ProfilePictureUrl,
                    AdminLoginId = "A86478927",
                    //PhoneNumber = admin.,
                    // Address = admin.,
                    isActivated = false,
                    Role = admin.Role
                };
                // Generate activation code
                string verificationCode = GenerateVerificationCode();

                // Store the verification code temporarily (example: in session or cache)
                StoreVerificationCodeTemporarily(admin.EmailAddress, verificationCode);

                //// Construct activation link
                //// string activationLink = $"https://yourdomain.com/activate?token={verificationCode}";

                // Send activation email with the activation link
                await emailService.SendActivationEmail(admin.EmailAddress, verificationCode);
                result = NewResult<Admin>.Success(newResponse, "Admin registration successful");
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<NewLoginResult<Admin>> AdminLogin(AdminLoginRequest loginRequest)
        {
            NewLoginResult<Admin> result = new NewLoginResult<Admin>();
            try
            {
                var admin = await userRepository.GetAdminByLoginId(loginRequest.AdminLoginId);
                if (admin == null || !VerifyPassword(loginRequest.Password, admin.Password))
                {
                    //throw new Exception("Invalid email or password.");
                    return NewLoginResult<Admin>.Failed(null, "Invalid login credientials");
                }

                if (!admin.isActivated)
                {
                    return NewLoginResult<Admin>.Failed(null, "Account is not activated");
                }

                // Generate JWT token
                var token = GenerateAdminJwtToken(admin);
                result = NewLoginResult<Admin>.Success(admin, token, "Login successful");
                return result;

            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return NewLoginResult<Admin>.Error(null, "An error occured while trying to login");
            }
        }

        public async Task<byte[]> DownloadImageAsByteArray(string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                }
                else
                {
                    // Handle error response
                    throw new Exception($"Failed to download image from URL: {imageUrl}.");
                }
            }
        }

        public async Task<NewResult<Admin>> ActivateAdminAccount(string emailAddress, string activationCode)
        {
            // NewResult<User> result = new NewResult<User>();
            try
            {
                // Retrieve the stored verification code for the given email address
                string storedVerificationCode = RetrieveVerificationCode(emailAddress);

                // Check if the verification code is found in the cache
                if (string.IsNullOrEmpty(storedVerificationCode))
                {
                    // If the verification code is not found, it means it has expired or does not exist
                    return NewResult<Admin>.Failed(null, "Verification code not found or expired.");
                }

                // Check if the verification code matches the one provided by the user
                if (storedVerificationCode != activationCode)
                {
                    // If the verification code does not match, return a failure result
                    return NewResult<Admin>.Failed(null, "Invalid verification code.");
                }

                // Proceed with account activation
                // Retrieve the admin by email address
                var admin = await userRepository.GetAdminByEmail(emailAddress);
                if (admin is not null)
                {
                    // Activate the user account (example: set IsActive flag to true)
                    admin.isActivated = true;

                    // Update the user in the database
                    await userRepository.UpdateAdminActivationStatus(emailAddress, admin.isActivated);

                    // Optionally, you may want to remove the verification code from storage
                    RemoveVerificationCodeFromCache(emailAddress);

                    // Return a success result
                    return NewResult<Admin>.Success(null, "Account activated successfully.");
                }
                else
                {
                    // If the user is not found, return a failure result
                    return NewResult<Admin>.Failed(null, "Admin not found.");
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs during the activation process, return a failure result
                return NewResult<Admin>.Failed(null, $"Error activating account: {ex.Message}");
            }
        }

        public async Task<NewResult<string>> ResendVerificationCode(string emailAddress)
        {
            try
            {
                // Generate a new verification code
                string newVerificationCode = GenerateVerificationCode();

                // Store the new verification code temporarily
                StoreVerificationCodeTemporarily(emailAddress, newVerificationCode);

                // Send the new verification code to the user's email address
                await emailService.SendActivationEmail(emailAddress, newVerificationCode);

                return NewResult<string>.Success(null, "Verification code resent successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the resend process
                return NewResult<string>.Failed(null, $"Failed to resend verification code: {ex.Message}");
            }
        }

        Task<NewResult<User>> IUserService.RegisterUser(User user)
        {
            throw new NotImplementedException();
        }

        Task<NewResult<User>> IUserService.ActivateAccount(string emailAddress, string activationCode)
        {
            throw new NotImplementedException();
        }

        Task<NewResult<string>> IUserService.ResetPassword(string emailAddress, string verificationCode, string newPassword)
        {
            throw new NotImplementedException();
        }

        Task<NewResult<string>> IUserService.InitiatePasswordReset(string emailAddress)
        {
            throw new NotImplementedException();
        }

        Task<NewResult<Admin>> IUserService.RegisterAdmin(Admin admin)
        {
            throw new NotImplementedException();
        }

        Task<NewResult<Admin>> IUserService.ActivateAdminAccount(string emailAddress, string activationCode)
        {
            throw new NotImplementedException();
        }

        Task<NewResult<string>> IUserService.ResendVerificationCode(string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
