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

		//Get All Users
		[HttpGet]
		public ActionResult<List<User>> Get() =>
		_golfService.Get();

		[HttpGet("{userName}", Name = "FindUser")]
		public ActionResult<User> Get(string userName)
		{
			var user = _golfService.GetUser(userName);
			if (user == null)
				return NotFound();
			return user;
		}
		//Get user and one request game by gameID
		[HttpGet("{user}/{gameId}", Name = "GetGame")]
		public ActionResult<User> Get(string username, string gameId)
		{
			var user = _golfService.Get(username, gameId);
			if (user == null)
			{
				return NotFound();
			}
			return user;
		}
		// [ActionName("current")]
		// [HttpGet("{user}/{coursename}")]
		// public ActionResult<User> GetUserCurrentGame(string username, string coursename)
		// {
		// 	var user = _golfService.GetCurrentGame();

		// 	return user;

		// }

		//Create User
		[ActionName("AddUser")]
		[HttpPost("createuser")]
		public ActionResult<User> Create([FromBody]User user)
		{
			_golfService.Create(user);
			return CreatedAtRoute("FindUser", new { userName = user.Name.ToLower() }, user);
		}

		// [ActionName("AddHole")]
		// [HttpPost("{gameId}")]
		// public ActionResult<Hole> Create(string gameId)
		// {
		// 	var hole = null;

		// 	return hole;
		// }
		//This should really be by ID
		[HttpPost("{userName}")]
		public ActionResult<User> CreateGame(string userName, [FromBody]Game game)
		{
			_golfService.CreateGame(userName, game);
			return Ok();
			// return CreatedAtRoute("GetGame", new { username = userName, gameId = game.GameId }, null);
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