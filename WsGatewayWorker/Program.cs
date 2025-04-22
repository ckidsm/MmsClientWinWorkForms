using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WsGatewayWorker;
using WsGatewayWorker.Controllers;
using WsGatewayWorker.Models;
using WsGatewayWorker.Services;
using WsGatewayWorker.State;
using WsGatewayWorker.Utils;

var builder = WebApplication.CreateBuilder(args);

// ✅ 콘솔 로그 설정
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ✅ 설정 로드
SettingsService.LoadFromJsonConfig();
var currentSettings = SettingsService.Current;

// ✅ DI 등록
builder.Services.AddSingleton(currentSettings);
builder.Services.AddSingleton<UnitController>(sp =>
{
   return new UnitController(
       unitId: "UNIT001",
       masterServerUrl: currentSettings.MasterServer ?? "http://localhost:5000",
       serialPort: "COM1" // COM 포트는 필요 시 추후 수정
   );
});

// 핵심 모듈 DI
builder.Services.AddSingleton<WebSocketRouter>();
builder.Services.AddSingleton<WsConnectionManager>();

// WebSocket 미들웨어 등록
builder.Services.AddTransient<WebSocketMiddleware>();

// HttpClient 기반 REST 연동 서비스
builder.Services.AddHttpClient<MasterApiService>(client =>
{
   client.BaseAddress = new Uri(currentSettings.MasterServer ?? "http://localhost:5000");
});

// REST API 컨트롤러 활성화
builder.Services.AddControllers();

var app = builder.Build();

// WebSocket 사용
app.UseWebSockets();

// WebSocket 경로 등록
app.Map("/ws", wsApp =>
{
   wsApp.UseMiddleware<WebSocketMiddleware>();
});

//REST API 경로 활성화
app.MapControllers();

// 상태 확인용 기본 경로
app.MapGet("/", () => "✅ WsGatewayWorker 서버 실행 중 (/ws + /api/*)");

app.Run();
