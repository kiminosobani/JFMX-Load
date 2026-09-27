using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadStation.Services
{
    internal class PeerTcpCommunicator : IDisposable
    {
        private readonly TcpListener _listener;   // 只创建一次
        private CancellationTokenSource _cts;
        private bool _isListening = false;

        public bool IsListening => _isListening;
        public event Action<string, string> OnMessageReceived;

        public PeerTcpCommunicator(int localPort)
        {
            _listener = new TcpListener(IPAddress.Any, localPort);
            _listener.Server.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReuseAddress, true);
        }

        public async Task StartListeningAsync()
        {
            if (_isListening) return;
            _cts = new CancellationTokenSource();
            _listener.Start();
            _isListening = true;
            _ = Task.Run(() => AcceptClientsLoop(_cts.Token));
        }

        public void StopListening()
        {
            if (!_isListening) return;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            try { _listener.Stop(); } catch { }
            _isListening = false;
        }

        public void Dispose()
        {
            StopListening();
        }

        private async Task AcceptClientsLoop(CancellationToken token)
        {
            while(!token.IsCancellationRequested)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client, token));
                } 
                catch(Exception) when (!token.IsCancellationRequested)
                {

                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            string remoteEP = client.Client.RemoteEndPoint?.ToString() ?? "未知";
            try
            {
                using(NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[8192];
                    while(!token.IsCancellationRequested && client.Connected)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        if (bytesRead == 0) break;
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                        OnMessageReceived?.Invoke(message, remoteEP);
                    }
                }
            }
            catch (OperationCanceledException) { }   // 正常取消，静默退出
            catch (Exception) { }
            finally
            {
                client.Close();
            }
        }

        public async Task<bool> SendAsync(string targetIp, int targetPort, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;
            using(TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(targetIp, targetPort);
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        await stream.WriteAsync(data, 0, data.Length);
                        await stream.FlushAsync();
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


    }
}
