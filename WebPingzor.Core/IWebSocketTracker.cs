using System.Collections.Generic;
namespace WebPingzor.Core;

public interface IWebSocketTracker
{
  public enum ConnectionState
  {
    Connected = 1,
    Reconnecting = 2
  }

  public IReadOnlyDictionary<string, ConnectionState> Connections { get; }

  public event Func<string, Task>? OnConnectionUp;
  public event Func<string, Task>? OnConnectionDown;
  public event Func<string, Task>? OnConnectionOpened;
  public event Func<string, Task>? OnConnectionClosed;
  public event Func<string, Task>? OnChange;
}