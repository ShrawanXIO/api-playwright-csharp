param(
    [Parameter(Mandatory=$true)]
    [string]$ApiName
)

$root = "$ApiName"

New-Item -ItemType Directory -Force -Path "$root\Models" | Out-Null
New-Item -ItemType Directory -Force -Path "$root\Services" | Out-Null
New-Item -ItemType Directory -Force -Path "$root\Tests" | Out-Null

@"
namespace ApiTests.$ApiName.Models;

// TODO: add request/response models for $ApiName here
"@ | Set-Content "$root\Models\${ApiName}Models.cs"

@"
using ApiTests.Core;
using ApiTests.$ApiName.Models;

namespace ApiTests.$ApiName.Services;

public class ${ApiName}Service : BaseService
{
    public ${ApiName}Service(ApiClient apiClient) : base(apiClient)
    {
    }

    // TODO: add endpoint methods here
}
"@ | Set-Content "$root\Services\${ApiName}Service.cs"

@"
using ApiTests.Core;
using ApiTests.$ApiName.Models;
using ApiTests.$ApiName.Services;
using NUnit.Framework;

namespace ApiTests.$ApiName.Tests;

public class ${ApiName}Tests : BaseApiTest
{
    protected override string BaseUrl => Settings.${ApiName}BaseUrl;

    private ${ApiName}Service _service = null!;

    [SetUp]
    public void SetupService()
    {
        _service = new ${ApiName}Service(apiClient);
    }

    // TODO: add [Test] methods here
}
"@ | Set-Content "$root\Tests\${ApiName}Tests.cs"

Write-Host "Scaffolded $ApiName under $root" -ForegroundColor Green
Write-Host "Remember: add ${ApiName}BaseUrl to appsettings.json and ApiSettings.cs manually" -ForegroundColor Yellow
