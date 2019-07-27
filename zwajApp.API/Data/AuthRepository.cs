using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using zwajApp.API.Models;
using ZwajApp.API.Data;

namespace zwajApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<Users> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserName==username);
            if (user==null)return null;
            if(!VerifyPasswordHash(password,user.PaswordSalt,user.PasswordHash))
            return null;
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] paswordSalt, byte[] passwordHash)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(paswordSalt)){
                
               var ComputedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for (int i = 0; i < ComputedHash.Length;  i++)
               {
                   if (ComputedHash[i]!=passwordHash[i]){
                       return false;
                   }
                      
               }
                return true; 
            }
        }

        public async Task<Users> Register(Users users, string password)
        {
            byte[] passwordHash , passwordSalt;
            CreatPasswordHash(password,out passwordHash,out passwordSalt);
            users.PaswordSalt=passwordSalt;
            users.PasswordHash=passwordHash;
            await _context.Users.AddAsync(users);
            await _context.SaveChangesAsync();
            return users;
        }

        private void CreatPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=>x.UserName==username))return true;
            return false;
        }
    }
}