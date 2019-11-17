using System.Collections.Generic;
using GolfApi.Models;
using GolfApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GolfApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GolfController : ControllerBase
	{
		private readonly GolfService _golfService;
		public GolfController(GolfService golfService)
		{
			_golfService = golfService;
		}

		//Get All Documents
		[HttpGet]
		public ActionResult<List<Golf>> Get() =>
		_golfService.Get();

		[HttpGet("getUser/{userName}", Name = "FindUser")]
		public ActionResult<Golf> GetUser(string userName)
		{
			var golf = _golfService.GetUser(userName);
			if (golf == null)
				return NotFound();
			return golf;
		}

		[HttpGet("{user}/{gameId}", Name = "GetGame")]
		public ActionResult<Golf> Get(string user, string gameId)
		{
			var golf = _golfService.Get(user, gameId);
			if (golf == null)
			{
				return NotFound();
			}
			return golf;
		}

		[HttpPut("deletegame/{gameId}")]
		public IActionResult DeleteGame(string gameId)
		{
			var game = _golfService.GetGames(gameId);
			if (game == null)
			{
				return NotFound();
			}
			_golfService.DeleteGame(gameId);
			return Ok(new { message = $"Deleted {typeof(Game)} with game Id: {gameId}" });
		}
	}
}