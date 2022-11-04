using MovieCatalogBackend.Data.Tokens;

namespace MovieCatalogBackend.Services;

public class TokenListCleanerDemon : IDisposable
{
    private ILogger _logger;
    private Thread _demon;
    public bool _interruptionFlag = false; 
    private Mutex _cleanupRateMutex = new Mutex(true);

    public TimeSpan CleanRate { get; set; } = TimeSpan.FromMinutes(1);
    public bool Disposed { get; private set; } = false;

    public TokenListCleanerDemon(ILogger<TokenListCleanerDemon> logger)
    {
        _logger = logger;
        _demon = new Thread(_demonRuntime);
        _demon.Start();
    }
    
    ~TokenListCleanerDemon() => Dispose();

    private void _demonRuntime()
    {
        while (!_interruptionFlag)
        {
            while (!_interruptionFlag)
            {
                try
                {
                    using var tokenListContext = new TokenListContext();
                    var tokens = tokenListContext.Tokens.ToArray();
                    tokenListContext.Tokens.RemoveRange(
                        tokenListContext.Tokens.Where(t => t.Expiretion < DateTime.UtcNow));
                    tokenListContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Occured while cleaning TokenList database from expired tokens");
                }

                _cleanupRateMutex.WaitOne(CleanRate);
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }

    public void Dispose()
    {
        if (Disposed) return;
        _interruptionFlag = true;
        _cleanupRateMutex.ReleaseMutex();
        _demon.Join();  
        _cleanupRateMutex.Dispose();
        GC.SuppressFinalize(this);
        Disposed = true;
    }
}
