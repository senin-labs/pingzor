using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Collections.Concurrent;
using WebPingzor.Core;

namespace WebPingzor.Web.Core;

internal sealed class ConnectionTracker : CircuitHandler, IWebSocketTracker
{
  private readonly ConcurrentDictionary<string, IWebSocketTracker.ConnectionState> _connections = new();

  public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
  {
    _connections.AddOrUpdate(circuit.Id, _ => IWebSocketTracker.ConnectionState.Connected, (_, _) => IWebSocketTracker.ConnectionState.Connected);

    OnConnectionUp?.Invoke(circuit.Id);
    OnChange?.Invoke(circuit.Id);

    return base.OnConnectionUpAsync(circuit, cancellationToken);
  }

  public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
  {
    _connections.AddOrUpdate(circuit.Id, _ => IWebSocketTracker.ConnectionState.Reconnecting, (_, _) => IWebSocketTracker.ConnectionState.Reconnecting);

    OnConnectionDown?.Invoke(circuit.Id);
    OnChange?.Invoke(circuit.Id);
    return base.OnConnectionDownAsync(circuit, cancellationToken);
  }

  public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
  {
    _connections.AddOrUpdate(circuit.Id, _ => IWebSocketTracker.ConnectionState.Reconnecting, (_, _) => IWebSocketTracker.ConnectionState.Reconnecting);

    OnConnectionOpened?.Invoke(circuit.Id);
    OnChange?.Invoke(circuit.Id);
    return base.OnCircuitOpenedAsync(circuit, cancellationToken);
  }

  public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
  {
    _connections.TryRemove(circuit.Id, out _);

    OnConnectionClosed?.Invoke(circuit.Id);
    OnChange?.Invoke(circuit.Id);
    return base.OnCircuitClosedAsync(circuit, cancellationToken);
  }

  public IReadOnlyDictionary<string, IWebSocketTracker.ConnectionState> Connections => _connections;

  public event Func<string, Task>? OnConnectionUp;
  public event Func<string, Task>? OnConnectionDown;
  public event Func<string, Task>? OnConnectionOpened;
  public event Func<string, Task>? OnConnectionClosed;
  public event Func<string, Task>? OnChange;
}
