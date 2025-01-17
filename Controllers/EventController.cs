using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using cda_ecf_asp_net.Models;
using cda_ecf_asp_net.Repositories;

namespace cda_ecf_asp_net.Controllers;

public class EventController : Controller
{
    private readonly ILogger<EventController> _logger;
    private readonly IEventRepository _eventRepository;

    public EventController(ILogger<EventController> logger, IEventRepository eventRepository)
    {
        _logger = logger;
        _eventRepository = eventRepository;
    }

    public async Task<IActionResult> Index()
    {
        var events = await _eventRepository.GetAll();
        
        return View(events);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Event @event)
    {
        if (ModelState.IsValid)
        {
            await _eventRepository.Create(@event);
            return RedirectToAction(nameof(Index));
        }
        return View(@event);
    }
    
    [HttpGet]
    public IActionResult Register(int id)
    {
        var @event = _eventRepository.GetById(id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var @event = await _eventRepository.GetById(id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Event @event)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _eventRepository.Update(@event);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        return View(@event);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var @event = await _eventRepository.GetById(id);
            if (@event == null)
            {
                return NotFound($"L'événement n'a pas été trouvé.");
            }

            await _eventRepository.Delete(@event);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return BadRequest($"Une erreur est survenur lors de la suppression : {ex.Message}");
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
