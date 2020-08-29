using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsORM.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsORM.Controllers
{
    public class HomeController : Controller
    {

        private static Context _context;

        public HomeController(Context DBContext)
        {
            _context = DBContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.BaseballLeagues = _context.Leagues
                .Where(l => l.Sport.Contains("Baseball"))
                .ToList();
            return View();
        }

        [HttpGet("level_1")]
        public IActionResult Level1()
        {
            ViewBag.Womens = _context.Leagues.Where(w => w.Name.Contains("Women")).ToList();
            ViewBag.Hockey = _context.Leagues.Where(h => h.Name.Contains("Hockey")).ToList();
            ViewBag.Football = _context.Leagues.Where(f => !f.Name.Contains("Football")).ToList();
            ViewBag.Conference = _context.Leagues.Where(c => c.Name.Contains("Conference")).ToList();
            ViewBag.Atlantic = _context.Leagues.Where(a => a.Name.Contains("Atlantic")).ToList();
            ViewBag.Dallas = _context.Teams.Where(d => d.Location == "Dallas").ToList();
            ViewBag.Raptors = _context.Teams.Where(r => r.TeamName == "Raptors").ToList();
            ViewBag.HasACity = _context.Teams.Where(c => c.Location.Contains("City")).ToList();
            ViewBag.StartsWithT = _context.Teams.Where(t => t.TeamName.StartsWith("T")).ToList();
            ViewBag.OrderedByLocation = _context.Teams.OrderBy(l => l.Location).ToList();
            ViewBag.DecendingTeamName = _context.Teams.OrderByDescending(b => b.TeamName).ToList();
            ViewBag.CooperPlayers = _context.Players.Where(c => c.LastName == "Cooper").ToList();
            ViewBag.FirstNameJoshua = _context.Players.Where(j => j.FirstName == "Joshua").ToList();
            ViewBag.CooperNotJosh = _context.Players.Where(n => n.LastName == "Cooper" && n.FirstName != "Joshua").ToList();
            ViewBag.AlexanderOrWyatt = _context.Players.Where(n => n.FirstName == "Alexander" || n.FirstName == "Wyatt").ToList();
            return View();
        }

        [HttpGet("level_2")]
        public IActionResult Level2()
        {
            ViewBag.AtlanticSoccer = _context.Teams.Include(l => l.CurrLeague)
                .Where(l => l.CurrLeague.Name
                .Contains("Atlantic Soccer Conference"))
                .ToList();
            ViewBag.PenguinsPlayers = _context.Players.Include(t => t.CurrentTeam)
                .Where(t => t.CurrentTeam.TeamName == "Penguins" && t.CurrentTeam.Location == "Boston")
                .ToList();
            ViewBag.ICBCPlayers = _context.Players.Include(t => t.CurrentTeam)
                .Where(t => t.CurrentTeam.CurrLeague.Name == "International Collegiate Baseball Conference")
                .ToList();
            ViewBag.LopezPlayers = _context.Players.Include(t => t.CurrentTeam)
                .Where(t => t.CurrentTeam.CurrLeague.Name == "American Conference of Amateur Football")
                .ToList();
            ViewBag.Football = _context.Players.Include(t => t.CurrentTeam)
                .Where(t => t.CurrentTeam.CurrLeague.Sport == "Football")
                .ToList();
            ViewBag.SophiaTeams = _context.Teams.Include(t => t.CurrentPlayers)
                .Where(t => t.CurrentPlayers.Any(p => p.FirstName == "Sophia"))
                .ToList();
            ViewBag.SophiaLeagues = _context.Leagues.Include(l => l.Teams)
                .Where(l => l.Teams.Any(t => t.CurrentPlayers.Any(p => p.FirstName == "Sophia")))
                .ToList();
            ViewBag.NotWashingtonFlores = _context.Players.Include(p => p.CurrentTeam)
                .Where(p => p.CurrentTeam.TeamName + p.CurrentTeam.Location != "WashingtonRoughriders" && p.LastName == "Flores")
                .ToList();
            return View();
        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {
            ViewBag.SamEvans = _context.Players
                .Include(player => player.AllTeams)
                .ThenInclude(playerteams => playerteams.TeamOfPlayer)
                .FirstOrDefault(pt => pt.FirstName == "Samuel" && pt.LastName == "Evans");
            ViewBag.Manitoba = _context.Players
                .Include(p => p.AllTeams)
                .ThenInclude(t => t.TeamOfPlayer)
                .Where(p => p.AllTeams.Any(yolo => yolo.TeamOfPlayer.Location + yolo.TeamOfPlayer.TeamName == "ManitobaTiger-Cats"));
            ViewBag.NotWichita = _context.Players
                .Include(p => p.AllTeams)
                .ThenInclude(p => p.TeamOfPlayer)
                .Where(p => p.AllTeams.Any(what => what.TeamOfPlayer.Location + what.TeamOfPlayer.TeamName == "WichitaVikings" && p.CurrentTeam.Location + p.CurrentTeam.TeamName != "WichitaVikings"));
            ViewBag.GrayNotColts = _context.Teams
                .Include(team => team.AllPlayers)
                .ThenInclude(playerTeams => playerTeams.PlayerOnTeam)
                .Where(panda => panda.AllPlayers.Any(grey => grey.PlayerOnTeam.FirstName + grey.PlayerOnTeam.LastName == "JacobGray"))
                .Where(p => p.Location + p.TeamName != "OregonColts");
            ViewBag.JoshAFABP = _context.PlayerTeams
                .Include(player => player.TeamOfPlayer)
                .ThenInclude(team => team.CurrLeague)
                .Where(peon => peon.PlayerOnTeam.FirstName == "Joshua")
                .Where(abc => abc.TeamOfPlayer.CurrLeague.Name == "Atlantic Federation of Amateur Baseball Players");
            ViewBag.MorePlayers = _context.Teams
                .Where(team => team.AllPlayers.Count > 12);
            ViewBag.PlayerSort = _context.Players
                .OrderByDescending(num => num.AllTeams.Count)
                .ToList();


            return View();
        }

    }
}