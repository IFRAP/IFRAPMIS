using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IFRAPMIS.Models;

namespace IFRAPMIS.Controllers.ThemeControllers;

public class FormsController : Controller
{
    public IActionResult BasicInputs() => View();
    public IActionResult CustomOptions() => View();
    public IActionResult Editors() => View();
    public IActionResult Extras() => View();
    public IActionResult FileUpload() => View();
    public IActionResult InputGroups() => View();
    public IActionResult Pickers() => View();
    public IActionResult Selects() => View();
    public IActionResult Sliders() => View();
    public IActionResult Switches() => View();
}
