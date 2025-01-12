﻿namespace WebPingzor.Core;

public interface IAuthService
{
  Task Login(string username, string password);
  Task Logout();
  Task<bool> IsAuthenticated();
  Task<int?> GetUserId();
  Task<string> GetUsername();
}
