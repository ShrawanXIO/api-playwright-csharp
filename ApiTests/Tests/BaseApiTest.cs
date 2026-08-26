using ApiTests.Core;
using NUnit.Framework;

namespace ApiTests.Tests;

public abstract class BaseApiTest
{
    protected static readonly ApiSettings Settings = ConfigLoader.Load();
   protected ApiClient apiClient {get; private set;} = null!;

   protected abstract string BaseUrl { get; }

   [SetUp]
   public async Task Setup()
   {
      apiClient = new ApiClient();
      await apiClient.InitializeAsync(BaseUrl);
   }

    [TearDown]
    public async Task TearDown()
    {
        await apiClient.DisposeAsync();
    }
    
}