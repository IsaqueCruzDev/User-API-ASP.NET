﻿namespace BukiApi.Dto
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
