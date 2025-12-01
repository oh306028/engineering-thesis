using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Thesis.api;
using Thesis.app.Dtos.Account;
using Thesis.app.Events;
using Thesis.app.Exceptions;
using Thesis.app.Services;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;


namespace Thesis.app.Commands
{
    public class AccountCommand
    {
        public class Register : IRequest<AccountLoginModel> 
        {
            public AccountRegisterModel Model{ get; set; }

            public Register(AccountRegisterModel model)
            {
                Model = model;
            }
        }

        public class RegisterTeacher : IRequest<Unit>   
        {
            public TeacherAccountRegisterModel Model { get; set; }
            public IFormFile File { get; set; }     

            public RegisterTeacher(TeacherAccountRegisterModel model)
            {
                Model = model;
                File = model.File;        
            }   
        }

        public class Login : IRequest<AccountSuccesLoginModel>   
        {
            public AccountLoginModel Model { get; set; }

            public Login(AccountLoginModel model)    
            {
                Model = model;
            }
        }

    }

    public class RegisterHandler : IRequestHandler<AccountCommand.Register, AccountLoginModel>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public RegisterHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<AccountLoginModel> Handle(AccountCommand.Register request, CancellationToken cancellationToken)
        {
            var passwordHasher = new PasswordHasher<User>();
            
            var parentAccount = new Parent();
            var studentAccount = new Student();

            var passwordHash = passwordHasher.HashPassword(parentAccount, request.Model.Password);

            parentAccount.Email = request.Model.Email;

            parentAccount.Login = request.Model.Login;
            parentAccount.PasswordHash = passwordHash;
            parentAccount.FirstName = request.Model.ParentFirstName;
            parentAccount.LastName = request.Model.ParentLastName;

            studentAccount.PasswordHash = passwordHash;
            studentAccount.FirstName = request.Model.StudentFirstName;
            studentAccount.LastName = request.Model.StudentLastName;
            studentAccount.DateOfBirth = request.Model.StudentDateOfBirth;
            studentAccount.Login = request.Model.Login + "-u";


            studentAccount.AccountLevelId = DbContext.AccountLevels.FirstOrDefault(p => p.Level == 1).Id;


            await DbContext.Users.AddRangeAsync(parentAccount, studentAccount);
            studentAccount.Parent = parentAccount;

            await DbContext.SaveChangesAsync();

            return new AccountLoginModel()
            {
                Login = request.Model.Login,
                Password = request.Model.Password
            };


        }
    }

    public class TeacherRegisterHandler : IRequestHandler<AccountCommand.RegisterTeacher, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public IFileService FileService { get; }
        public AzureStorageOptions StorageOptions { get; }

        public TeacherRegisterHandler(AppDbContext dbContext, IFileService fileService, AzureStorageOptions storageOptions)  
        {
            DbContext = dbContext;
            FileService = fileService;
            StorageOptions = storageOptions;
        }

        public async Task<Unit> Handle(AccountCommand.RegisterTeacher request, CancellationToken cancellationToken)
        {
            var passwordHasher = new PasswordHasher<User>();    

            var account = new Teacher();

            var passwordHash = passwordHasher.HashPassword(account, request.Model.Password);

            account.Email = request.Model.Email;

            account.Login = request.Model.Login;
            account.PasswordHash = passwordHash;
            account.FirstName = request.Model.FirstName;
            account.LastName = request.Model.LastName;
            account.IsAccepted = false;


            var blobName = $"{StorageOptions.ContainerName}/{Guid.NewGuid()}_{request.File.FileName}";
            var url = await FileService.UploadAsync(request.File, blobName);

            account.CertificateUrl = url;

            await DbContext.Users.AddAsync(account, cancellationToken);
            await DbContext.SaveChangesAsync();

            return Unit.Value;  

        }
    }

    public class LoginHandler : IRequestHandler<AccountCommand.Login, AccountSuccesLoginModel>, IHandler    
    {
        public AppDbContext DbContext { get; set; }
        public JwtOptions AuthenticationOptions { get; set; }
        public IMediator MediatR { get; }

        public LoginHandler(AppDbContext dbContext, JwtOptions authenticationOptions, IMediator mediatR)
        {
            DbContext = dbContext;
            AuthenticationOptions = authenticationOptions;
            MediatR = mediatR;
        }

        public async Task<AccountSuccesLoginModel> Handle(AccountCommand.Login request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(p => p.Login == request.Model.Login);
            
            if (user == null)
                throw new NotFoundException("Błędny login lub hasło");

            if (user is Teacher teacher && teacher.IsAccepted.HasValue && !teacher.IsAccepted.Value)
                throw new NotFoundException("Konto oczekuje na akceptację przez administratora");


            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Model.Password);

            var loginAttempt = new LoginHistory();
            loginAttempt.LoginDate = DateTime.Now;
            loginAttempt.UserId = user.Id;

            if (result == PasswordVerificationResult.Failed)
            {               
                loginAttempt.IsSucceeded = false;
               
                DbContext.LoginHistories.Add(loginAttempt);
                await DbContext.SaveChangesAsync();

                throw new NotFoundException("Błędny login lub hasło");
            }

            loginAttempt.IsSucceeded = true;

            DbContext.LoginHistories.Add(loginAttempt);
            await DbContext.SaveChangesAsync();

            if(user is Student student)
                await MediatR.Publish(new LogInEvent(student.Id, DateTime.Now));

            var token = GenerateToken(user);
            return new AccountSuccesLoginModel() {Token = token};
        }

        private string GenerateToken(User user) 
        {
            var key = Encoding.ASCII.GetBytes(AuthenticationOptions.Key);
            var securityKey = new SymmetricSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),

            }),
                Expires = DateTime.UtcNow.AddDays(3),
                Issuer = AuthenticationOptions.Issuer,
                Audience = AuthenticationOptions.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
