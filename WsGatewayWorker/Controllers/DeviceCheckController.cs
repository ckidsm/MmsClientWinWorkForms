
using Microsoft.AspNetCore.Mvc;
using WsGatewayWorker.Models;
using WsGatewayWorker.Services;
using WsGatewayWorker.State;
using WsGatewayWorker.Utils;  
using WsGatewayWorker.Enums;

namespace WsGatewayWorker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceCheckController : ControllerBase
    {
        private readonly UnitController _unitController;
        private readonly MasterApiService _masterApi;

        public DeviceCheckController(UnitController unitController, MasterApiService masterApi)
        {
            _unitController = unitController;
            _masterApi = masterApi;
        }

        [HttpPost("feed-ready")]
        public async Task<IActionResult> FeedReady([FromBody] DeviceRequestDto req)
        {
            var device = _unitController.GetDevice(req.DeviceId);
            if (device == null) return NotFound("unknown device");

            device.State = MmsClientWinForms.Enums.DeviceState.DS_FEEDING;

            var data = device.ToDictionary();
            data["monitor_id"] = req.MonitorId;
            data["location"] = req.Location;

            await _masterApi.PostDeviceResultAsync(_unitController.UnitId, data);
            return Ok(new { message = "OK" });
        }

        [HttpPost("step")]
        public async Task<IActionResult> Step([FromBody] DeviceRequestDto req)
        {
            var device = _unitController.GetDevice(req.DeviceId);
            if (device == null) return NotFound("unknown device");

            device.Steps[req.Name!] = req.Passed == "true";
            var data = device.ToDictionary();
            data["monitor_id"] = req.MonitorId;
            data["location"] = req.Location;

            await _masterApi.PostDeviceResultAsync(_unitController.UnitId, data);
            return Ok(new { message = "OK" });
        }

        [HttpPost("result")]
        public async Task<IActionResult> Result([FromBody] DeviceRequestDto req)
        {
            var device = _unitController.GetDevice(req.DeviceId);
            if (device == null) return NotFound("unknown device");

            device.Passed = req.Passed == "true";
            var data = device.ToDictionary();
            data["monitor_id"] = req.MonitorId;
            data["location"] = req.Location;

            await _masterApi.PostDeviceResultAsync(_unitController.UnitId, data);
            return Ok(new { message = "OK" });
        }
    }
}
