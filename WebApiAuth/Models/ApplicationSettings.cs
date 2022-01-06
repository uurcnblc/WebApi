﻿namespace WebApiAuth.Models
{
    public class ApplicationSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
