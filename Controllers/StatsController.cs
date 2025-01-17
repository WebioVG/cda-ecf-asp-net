using cda_ecf_asp_net.Models;
using Microsoft.AspNetCore.Mvc;

namespace cda_ecf_asp_net.Controllers;

public class StatsController : Controller
{
    private readonly MongoDbService _mongoDbService;

    public StatsController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    // Display stats from MongoDB
    [HttpGet("/stats")]
    public async Task<IActionResult> Index()
    {
        var stats = await _mongoDbService.GetLatestStatsAsync();

        if (stats == null)
        {
            TempData["Error"] = "Aucune statistique.";
            return RedirectToAction("PopulateStats");
        }

        return View(stats);
    }

    // Populate MongoDB with stats
    [HttpGet("/stats/populate")]
    public async Task<IActionResult> PopulateStats()
    {
        try
        {
            await _mongoDbService.PopulateStatsAsync();
            TempData["Message"] = "Stats ajoutés avec succès dans MongoDB.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Une erreur est survenue : {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}